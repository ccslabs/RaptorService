using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RaptorDb;

namespace RaptorService
{

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IRaptorService
    {
        [OperationContract]
        string GetLastError();

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        bool Login(string name, string passwordHash);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void LogOff();

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        bool DeleteUser();

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        AgentDetails GetDetails();

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        ArrayList SendUrls();

        [OperationContract(IsOneWay = true, IsInitiating = true, IsTerminating = false)]
        void SendError(UrlError uE);

        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void ReceiveUrls(string url, string urlHash, string sourceUrl, bool IsContentObject, float ErrorCode);

        [OperationContract(IsInitiating = true)]
        bool RegisterUser(string name, string country, string emailAddress, string passwordHash);

        [OperationContract(IsInitiating = true, IsTerminating = true)]
        bool CheckEmailAddressIsFree(string emailAddress);

        
    }

    [DataContract]
    public class AgentDetails
    {
        [DataMember]
        public string AgentRank { get; set; }
        [DataMember]
        public string AgentJurisdiction { get; set; }
        [DataMember]
        public string AgentLicenseNumber { get; set; }
        [DataMember]
        public double? WorldRunTime { get; set; }
        [DataMember]
        public double? AgentsTotalRunTime { get; set; }
        [DataMember]
        public double AgentsTotalURIsFound { get; set; }  // Unique to the client's calculated value which includes duplicates
        [DataMember]
        public double AgentsTotalContentObjectsFound { get; set; } // See Above
        [DataMember]
        public double AgentsTotalURIsProcessed { get; set; }

        public static explicit operator AgentDetails(RaptorDb.AgentDetails v)
        {
            return new AgentDetails() { AgentJurisdiction = v.AgentJurisdiction, AgentLicenseNumber = v.AgentLicenseNumber, AgentRank = v.AgentRank, WorldRunTime = v.WorldRunTime,
            AgentsTotalRunTime = v.AgentsTotalRunTime, AgentsTotalURIsFound = v.AgentsTotalURIsFound, AgentsTotalContentObjectsFound = v.AgentsTotalContentObjectsFound, AgentsTotalURIsProcessed = v.AgentsTotalURIsProcessed};
        }
    }

    [DataContract]
    public class UrlError
    {
        [DataMember]
        public string CurrentUrl { get; set; }
        [DataMember]
        public int ErrorCode { get; set; }

        public static explicit operator UrlError(RaptorDb.UrlError v)
        {
            return new UrlError() { CurrentUrl = v.CurrentUrl, ErrorCode = v.ErrorCode  };
        }
    }



}
