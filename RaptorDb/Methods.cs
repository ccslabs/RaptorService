using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaptorDb
{
    public class Methods
    {
        private static RaptorEntities1 raptorEntities;
        public static User user; // Populated in ContinueLogin

        public Methods()
        {
            try
            {
                raptorEntities = new RaptorEntities1();
                raptorEntities.Database.Connection.Open();
            }
            catch (Exception)
            {
                // This is not good! But what to do about it?
                // Usually a TimeOut
                raptorEntities = new RaptorEntities1();
                raptorEntities.Database.Connection.Open();
            }

        }

        public bool CheckEmailAddressIsFree(string emailAddress)
        {
            var cu = raptorEntities.Users.Where(r => r.UserEmail == emailAddress);
            if (cu.Count() < 1)
                return false;
            else
            {
                return true;
            }
        }

        public User GetUserByName(string userName, string password)
        {
            return raptorEntities.Users.SingleOrDefault(r => r.UserFullName == userName && r.UserPassword == password); //returns a single item.
        }

        public int GetUserAccountStatus(string name, string passwordHash)
        {
            try
            {
                var UserAccountStatus = raptorEntities.Users.SingleOrDefault(r => r.UserFullName == name && r.UserPassword == passwordHash); //returns a single item.

                if (UserAccountStatus != null)
                {
                    return UserAccountStatus.UserStatusId;
                }
                else
                    return -10;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool RegisterUser(string name, string country, string emailAddress, string passwordHash)
        {
            using (var rapEn = new RaptorEntities1())
            {
                rapEn.Database.Connection.Open();
                User user = new User();
                user.UserFullName = name;
                user.UserCountry = GetUserCountry(country);
                user.UserEmail = emailAddress;
                user.IsOnline = false;      // Technically not online yet
                user.UserRankId = 25;       // Non-Investigating Agent - Alterable when completing Profile
                user.UserStatusId = 1;      // Non-Operational - Alterable when completing Profile
                user.UserIsAdult = true;    // Take user's word for it just now. Later we can check against our database for Age of Majority for the Jurisdiction the User is in!
                user.UserJoinedDate = DateTime.UtcNow;
                user.UserLanguage = GetUserLanguage(country);
                user.UserLicenseNumber = GenerateUserLicense();
                user.UserPassword = passwordHash;
                rapEn.Users.Add(user);

                long id = user.Id; // Yes it's here

                // Generate the User's Rank History
                RankHistory rankHistory = new RankHistory();
                rankHistory.UserId = id;
                rankHistory.RankId = user.UserRankId;
                rankHistory.ObtainedOn = user.UserJoinedDate;
                rapEn.RankHistories.Add(rankHistory);

                int res = rapEn.SaveChanges();

                rapEn.Database.Connection.Close();
                if (res == 0)
                    return false;
                return true;
            }
        }

        public bool DeleteUser()
        {
            var UserToDelete = user;

            if (UserToDelete != null)
            {
                UserToDelete.UserStatusId = 10;     // Released (We do not actually DELETE records because of later possible investigations)
                LogUserOff();

                int res = raptorEntities.SaveChanges();      // {"The DELETE statement conflicted with the REFERENCE constraint \"FK_RankHistories_Users\". The conflict occurred in database \"Raptor\", table \"dbo.RankHistories\", column 'UserId'.\r\nThe statement has been terminated."}
                raptorEntities.Database.Connection.Close();
                return true;
            }

            return false;
        }

        public IQueryable<Url> GetUnprocessedUrls(int numberOfUrlsToIncreaseQueueBy)
        {
            // Get a Url which has not been processed
            // which is not a Content Object (They are processed in the Raptor Manager)
            // Which has not a faulted Error Code - may need to change this later! redirection responses may be ok.
            var res = raptorEntities.Urls.OrderBy(u => u.Id).Where(w => w.Processed == false && w.IsContentObject == false && w.ResponseCode == 0).Take(numberOfUrlsToIncreaseQueueBy);
            return res;
        }

        public void LogUserOff()
        {
            try
            {
                var u = raptorEntities.Users.First(us => us.Id == user.Id);
                u.IsOnline = false;

                try
                {
                    // double oh2 = raptorEntities.OnlineHistories.Where(us => us.Id == user.Id).Sum(r => (double?) r.SessionTime) ?? 0; // Need the stupid cast as we may get a null !
                    // var oh2 = raptorEntities.OnlineHistories.Where(us => us.Id == user.Id && us.SessionTime != null).Sum(r => r.SessionTime);
                    var oh2 = raptorEntities.OnlineHistories.Where(us => us.UserId == user.Id).Sum(r => r.SessionTime);
                    u.TotalRunTime = oh2;
                }
                catch (Exception ex)
                {
                    // Shouldn't really get here!
                    Console.WriteLine("LogUserOff() " + ex.Message);
                }

                try
                {
                    var oh = raptorEntities.OnlineHistories.First(r => r.UserId == user.Id && r.OfflineAt == null);
                    oh.OfflineAt = DateTime.UtcNow;     // Log the User off on the OnlineHistories Table
                                                        //    raptorEntities.OnlineHistories.Add(oh);  // Don't add as it creates a new entry - I think?
                }
                catch (Exception ex2)
                {
                    // If the user has already crashed out they will have lost their connection and logged out so this will be null - just ignore.
                }

                raptorEntities.SaveChanges();
                raptorEntities.Database.Connection.Close();
            }
            catch (Exception ex3)
            {
                // Sometimes get's here - not sure why!
            }

        }

        RaptorEntities1 threadedRERaptorEntities = new RaptorEntities1();
        public void ReciveErrorUrl(string currentUrl, int ErrorCode)
        {
            threadedRERaptorEntities = new RaptorEntities1();
            threadedRERaptorEntities.Database.Connection.Open();
            UrlProcessedBy(currentUrl);

            var getCurrentUrl = threadedRERaptorEntities.Urls.First(u => u.URLPath == currentUrl);
            getCurrentUrl.ResponseCode = ErrorCode;
            getCurrentUrl.IsContentObject = false;
            getCurrentUrl.Processed = true;
            getCurrentUrl.ProcessedById = user.Id;
            getCurrentUrl.ProcessedOn = DateTime.Now;
            threadedRERaptorEntities.SaveChanges();
            threadedRERaptorEntities.Database.Connection.Close();

        }
        RaptorEntities1 threadedUPBEntities = new RaptorEntities1();

        private void UrlProcessedBy(string currentUrl)
        {
            threadedUPBEntities = new RaptorEntities1();
            Url OriginalUrlId = null;
            ProcessedBy DuplicateEntry = null;
            threadedUPBEntities.Database.Connection.Open();
            try
            {
                OriginalUrlId = threadedUPBEntities.Urls.First(u => u.URLPath == currentUrl);                   // Get the Id of the Current URL
                // If this has already been added then we will get a result which we can ignore - or it will throw an exception which we catch and process as a new record.
                DuplicateEntry = threadedUPBEntities.ProcessedBies.First(p => p.UrlId == OriginalUrlId.Id && p.UserId == user.Id);
            }
            catch (Exception)
            {
                if (DuplicateEntry == null)
                {
                    // So this is a new record then :P
                    DuplicateEntry = new ProcessedBy();
                    DuplicateEntry.UrlId = OriginalUrlId.Id;
                    DuplicateEntry.UserId = user.Id;
                    threadedUPBEntities.ProcessedBies.Add(DuplicateEntry);
                    threadedUPBEntities.SaveChanges();
                }
            }
            finally
            {
                threadedUPBEntities.Database.Connection.Close();
            }
        }


        private int taskCounter = 0;
        public void ReceiveUrls(string url, string urlHash, string sourceUrl, bool isContentObject, float ErrorCode)
        {
            while (taskCounter >= 4)
            {
                System.Threading.Thread.Sleep(250);
            }

            taskCounter++;
            Task tProcessUrls = new Task(() => ProcessReceivedUrlQueue(url, urlHash, sourceUrl, isContentObject, ErrorCode));
            tProcessUrls.Start();

            //UrlProcessedBy(sourceUrl);
            //ProcessReceivedUrlQueue(url, urlHash, sourceUrl, isContentObject, ErrorCode);
        }

        RaptorEntities1 threadedRaptorEntities = new RaptorEntities1();

        private void ProcessReceivedUrlQueue(string url, string urlHash, string sourceUrl, bool isContentObject, float ErrorCode)
        {
            threadedRaptorEntities.Database.Connection.Open();

            while (threadedRaptorEntities.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                System.Threading.Thread.Sleep(500);
                threadedRaptorEntities.Database.Connection.Open();
            }

            try
            {
                // Needs to be processed first so that the SourceUrl is marked as processed.
                var sourceURL = threadedRaptorEntities.Urls.First(u => u.URLPath == sourceUrl); // Get the Source URL
                sourceURL.Processed = true;
                sourceURL.ResponseCode = ErrorCode;
                sourceURL.ProcessedById = user.Id;
                sourceURL.ProcessedOn = DateTime.Now;
                threadedRaptorEntities.SaveChanges(); // Update the Source URL to show that it has been processed


                Url newURL = new Url();
                newURL.URLHash = urlHash;  // RaptorBackground does the hashing not us.
                newURL.URLPath = WebUtility.UrlDecode(ReturnRedirectUrl(url, sourceUrl)); // Also converts Relative to Absolute Paths, finally decodes Url
                newURL.FoundById = user.Id;
                newURL.FoundOn = DateTime.UtcNow;
                newURL.ResponseCode = 0;
                threadedRaptorEntities.Urls.Add(newURL);
                threadedRaptorEntities.SaveChanges(); // Add the new URL to the Database.

                taskCounter--;
                threadedRaptorEntities.Database.Connection.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Store update, insert, or delete statement affected an unexpected number of rows (0)."))
                { // Just ignore this it is a duplicate record.
                }
                else
                    throw ex;
            }
            finally
            {
                threadedRaptorEntities.Database.Connection.Close();
            }

        }// Method



        private string ReturnRedirectUrl(string Url, string urlSource)
        {
            if (!Url.ToLowerInvariant().StartsWith("http"))

                if (Url.ToLowerInvariant().Contains("=http"))
                {
                    string[] parts = Url.Split('=');
                    for (int idx = 0; idx < parts.Count(); idx++)
                    {
                        if (parts[idx].ToLowerInvariant().StartsWith("http") && idx > 0) // ignore first http: if it exists
                            return parts[idx];
                    }
                    return Url;
                }
                else if (Url.ToLowerInvariant().Contains("?http"))
                {
                    string[] parts = Url.Split('?');
                    for (int idx = 0; idx < parts.Count(); idx++)
                    {
                        if (parts[idx].ToLowerInvariant().StartsWith("http") && idx > 0) // ignore first http: if it exists
                            return parts[idx];
                    }
                    return Url;
                }
            return Url;
        }

        public AgentDetails GetDetails()
        {
            AgentDetails ag = new AgentDetails();

            //TODO: Technically we should be getting these each time incase there has been a change in circumstance!
            //Or at least the User's Status !!
            ag.AgentRank = GetAgentRank();
            ag.AgentLicenseNumber = user.UserLicenseNumber;
            ag.AgentJurisdiction = user.UserCountry;


            // The number of URLs found and processed
            ag.AgentsTotalURIsFound = threadedRaptorEntities.Urls.LongCount(u => u.FoundById == user.Id);
            ag.AgentsTotalURIsProcessed = threadedRaptorEntities.ProcessedBies.LongCount(u => u.Id == user.Id);
            var use = threadedRaptorEntities.Users.First(uu => uu.Id == user.Id);

            ag.AgentsTotalRunTime = use.TotalRunTime;
            // The total Runtime Prior to this Session Starting (updated when each client logs off)
            ag.WorldRunTime = threadedRaptorEntities.Users.Sum(us => us.TotalRunTime);
            if (ag.WorldRunTime == null) ag.WorldRunTime = 0;

            use = null;
            return ag;
        }


        public string GetAgentRank()
        {
            var rank = raptorEntities.Ranks.Single(r => r.Id == user.UserRankId);
            return rank.Rank1;
        }

        private string GetUserCountry(string userCountry)
        {
            // userCountry looks like this English (United Kingdom)
            // so get the contents of the braces
            string[] parts = userCountry.Split('(');
            return parts[1].Replace(")", "").Trim();
        }

        private string GetUserLanguage(string userCountry)
        {
            string[] parts = userCountry.Split('(');
            return parts[0].Trim();
        }

        private string GenerateUserLicense()
        {
            string Lic = GetFirstThreeOfUserName().ToUpperInvariant();
            Lic += GetLicenseDatePortion();
            Lic += SetCounter();
            Lic += GetCountryISOCode();

            // Does License exist already? If so - increment Counter and Retry.
            while (LicenseNotValid(Lic))
                IncrementLicense(Lic);
            return Lic;
        }

        private string GetFirstThreeOfUserName()
        {
            string firstThree = "";
            if (user.UserFullName.Length < 3)
            {
                int len = user.UserFullName.Length;
                int dif = 3 - len;
                for (int idx = 0; idx < len; idx++)
                {
                    firstThree += user.UserFullName[idx];
                }

                for (int idx2 = 0; idx2 < dif; idx2++)
                {
                    firstThree += "Z";
                }

                return firstThree;
            }
            else
            {
                return user.UserFullName.Substring(0, 3);
            }
        }

        private string GetLicenseDatePortion()
        {
            return DateTime.UtcNow.Year.ToString().Replace("20", "") + DateTime.UtcNow.DayOfYear.ToString("X4");
        }

        private string SetCounter()
        {
            return 0.ToString("X5").ToUpperInvariant().ToString();
        }

        private string GetCountryISOCode()
        {
            string cult = Thread.CurrentThread.CurrentCulture.Name.ToString();
            var c = new CultureInfo(cult);
            var r = new RegionInfo(c.LCID);
            return r.TwoLetterISORegionName;
        }

        private bool LicenseNotValid(string Lic)
        {
            var LicensePresent = raptorEntities.Users.SingleOrDefault(r => r.UserLicenseNumber == Lic); //returns a single item.

            if (LicensePresent != null)
                return true;
            return false;
        }

        private void IncrementLicense(string Lic)
        {
            int myHex = Int32.Parse(Lic, System.Globalization.NumberStyles.HexNumber);
            myHex++;
            string newValue = myHex.ToString("X5");
        }

        public bool ContinueLogin(string name, string passwordHash, int statusValue)
        {
            var UserToLogin = GetUserByName(name, passwordHash);

            if (UserToLogin != null)
            {
                UserToLogin.UserStatusId = statusValue;
                user = UserToLogin;
                LogUserOn();

                int res = raptorEntities.SaveChanges();      // 

                return true;
            }
            raptorEntities.Database.Connection.Close();
            return false;
        }

        private void LogUserOn()
        {
            OnlineHistory oh = new OnlineHistory();

            user.IsOnline = true;      // Log User On 
            oh.OnlineAt = DateTime.UtcNow;      // Create their Online History
            oh.UserId = user.Id;
            raptorEntities.OnlineHistories.Add(oh);
        }


    }

    public class AgentDetails
    {

        public string AgentRank { get; set; }

        public string AgentJurisdiction { get; set; }

        public string AgentLicenseNumber { get; set; }

        public double? WorldRunTime { get; set; }
        public double? AgentsTotalRunTime { get; set; }
        public double AgentsTotalURIsFound { get; set; }  // Unique unlike the client's calculated value which includes duplicates
        public double AgentsTotalContentObjectsFound { get; set; } // See Above

        public double AgentsTotalURIsProcessed { get; set; }
    }



    public class ReceivedURLCollection
    {
        public string url { get; set; }
        public string urlHash { get; set; }
        public string sourceUrl { get; set; }
        public bool isContentObject { get; set; }
        public float errorCode { get; set; }
    }

    public class UrlError
    {
        public string CurrentUrl { get; set; }
        public int ErrorCode { get; set; }
    }


}
