using RaptorDb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaptorService
{
    public class CustomEventArgs : EventArgs
    {
        public string Message;
        public string CallingMethod;
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RaptorService : IRaptorService
    {
        public static event EventHandler<CustomEventArgs> CustomEvent;
        private string LastMessage = "";

        private void SendEvent(string Message)
        {
            CustomEventArgs cev = new CustomEventArgs();
            cev.Message = Message;

            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;

            cev.CallingMethod = name;
            CustomEvent(this, cev);
        }

        public RaptorService()
        {
            SendEvent("RaptorService New Connection");
            OperationContext.Current.InstanceContext.Closing += InstanceContext_Closing; // Required to log the Agent off.          
        }

        private void InstanceContext_Closing(object sender, EventArgs e)
        {
            SendEvent("RaptorService Client Connection Closing");
            RaptorDb.Methods iccrdb = new RaptorDb.Methods();
            iccrdb.LogUserOff();
        }

        public string LastError { get; set; }

        private static Queue<string> UrlQueue = new Queue<string>();
        private RaptorDb.Methods rdb = new RaptorDb.Methods();

        public string GetLastError()
        {
            if (LastMessage != "Client GetLastError()")
                Console.WriteLine("Client GetLastError()");
            LastMessage = "Client GetLastError()";
            return LastError;
        }

        #region Check Email Address Is Free

        public bool CheckEmailAddressIsFree(string emailAddress)
        {
            SendEvent("RaptorService Checking Registration Email Is Available");
            RaptorDb.Methods CErdb = new RaptorDb.Methods();
            return CErdb.CheckEmailAddressIsFree(emailAddress);
        }
        #endregion

        #region RegisterAgent
        public bool RegisterUser(string name, string country, string emailAddress, string passwordHash)
        {
            SendEvent("RaptorService Registring User");
            RaptorDb.Methods rurdb = new RaptorDb.Methods();
            return rurdb.RegisterUser(name, country, emailAddress, passwordHash);
        }
        #endregion

        #region Logon

        public bool Login(string Name, string passwordHash)
        {
            SendEvent("User Is Loging in");
            RaptorDb.Methods Lrdb = new RaptorDb.Methods();
            // Check the Status of the User before logging them in
            // If the User is !Deceased, !ROGUE, !Under Investigation, !Suspended, !Dismissed, They can Login
            // so if StatusValue > 3 then all is cool
            // If StatusValue > 0 && < 4  (Retired -> 1  On Medical -> 2 On Leave -> 3) // Refer to their Supervisor for Re-instatement
            int StatusValue = Lrdb.GetUserAccountStatus(Name, passwordHash); // This is only relevant for those using the Raptor CO-ARC Manager
            if (Lrdb.ContinueLogin(Name, passwordHash, StatusValue))
            {
                SendEvent("\tSucess");
                return true;
            }
            SendEvent("\tFailed");
            return false;
        }
        #endregion

        #region Get Agent's Details
        public AgentDetails GetDetails()
        {
            SendEvent("User Is Getting Details");
            RaptorDb.Methods dgrdb = new RaptorDb.Methods();
            return (AgentDetails)dgrdb.GetDetails();
        }


        #endregion

        #region Log The Agent Off
        public void LogOff()
        {
            SendEvent("User Is Logging Off");
            RaptorDb.Methods lordb = new RaptorDb.Methods();
            lordb.LogUserOff();
        }


        #endregion

        #region Remove User
        public bool DeleteUser()
        {
            SendEvent("RaptorService Deleting User");
            RaptorDb.Methods durdb = new RaptorDb.Methods();
            return durdb.DeleteUser();
        }

        #endregion

        #region Send Urls To Client
        public ArrayList SendUrls()
        {
            SendEvent("Sending Urls to Client");
            PopulateQueue(1);
            LastError = string.Empty;
            ArrayList alUrls = new ArrayList();

            while (UrlQueue.Count > 0)
            {
                alUrls.Add(UrlQueue.Dequeue());
            }
            return alUrls;
        }
        #endregion

        #region Utilities

        private User GetUserByUsernameAndPassword(string userName, string Password)
        {
            SendEvent("RaptorService is Getting User By Username and Password");
            RaptorDb.Methods guburdb = new RaptorDb.Methods();
            return guburdb.GetUserByName(userName, Password);
        }

        private void PopulateQueue(int NumberOfUrlsToIncreaseQueueBy)
        {
            try
            {
                SendEvent("RaptorService Is Populating Queue");
                RaptorDb.Methods pqrdb = new RaptorDb.Methods();
                if (NumberOfUrlsToIncreaseQueueBy > 0)
                {
                    // context.PersonSet.OrderByDescending(u => u.OnlineAccounts.Count).Take(5);

                    var res = pqrdb.GetUnprocessedUrls(NumberOfUrlsToIncreaseQueueBy);
                    foreach (Url url in res)
                    {
                        UrlQueue.Enqueue(url.URLPath);
                    }
                }
                pqrdb = null; ;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region Receive Urls from Client
        private static double UrlCount = 0;
        private Queue<ReceivedURLCollection> rucQueue = new Queue<ReceivedURLCollection>();


        public void ReceiveUrls(string url, string urlHash, string sourceUrl, bool IsContentObject, float ErrorCode)
        {
            ReceivedURLCollection ruc = new ReceivedURLCollection();
            ruc.url = url;
            ruc.urlHash = urlHash;
            ruc.sourceUrl = sourceUrl;
            ruc.isContentObject = IsContentObject;
            ruc.errorCode = ErrorCode;
            rucQueue.Enqueue(ruc);
            UrlCount++;
            SendEvent("RaptorService is Receiving Urls from the Client " + UrlCount.ToString("N0"));

            ProcessRucQueue();

        }

        private bool QueueRunning = false;
        private ReceivedURLCollection druc;
        private void ProcessRucQueue()
        {
            druc = new ReceivedURLCollection();
            if (!QueueRunning)
            {
                QueueRunning = true;
                while (rucQueue.Count > 0)
                {
                    druc = rucQueue.Dequeue();
                    RaptorDb.Methods rrdb = new RaptorDb.Methods();
                    rrdb.ReceiveUrls(druc.url, druc.urlHash, druc.sourceUrl, druc.isContentObject, druc.errorCode);
                    rrdb = null;
                }
                QueueRunning = false;
            }
        }

        public void SendError(UrlError uE)
        {
            UrlCount++;
            SendEvent("RaptorService is Receiving Errored Url from Client" + UrlCount.ToString("N0"));
            RaptorDb.Methods serdb = new RaptorDb.Methods();
            serdb.ReciveErrorUrl(uE.CurrentUrl, uE.ErrorCode);
            serdb = null;
        }

        #endregion
    }





}
