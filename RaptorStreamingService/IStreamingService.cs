using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RaptorStreamingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IStreamingService
    {

        [OperationContract(IsTerminating = false, IsOneWay = true)]
        void SendContentObject(Stream co);


        // Use a data contract as illustrated in the sample below to add composite types to service operations.
    }
    internal class Hashing
    {
        internal static string HashString(string password)
        {
            byte[] hash = null;
            var data = Encoding.UTF8.GetBytes(password);
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(data);
            }

            return BytesToString(hash);
        }

        internal static string BytesToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "").ToUpper();
        }

        internal static byte[] StringToBytes(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        //public static string HashString(string text)
        //{
        //    text = text.ToLowerInvariant().Trim().ToString(); // Ensure that hashing a particular text is always from the same base

        //    string hash = "";
        //    SHA512 alg = SHA512.Create();
        //    byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(text));
        //    hash = Encoding.UTF8.GetString(result);
        //    return hash;
        //}
    }

    [Serializable]
    [DataContract]
    public class ContentObject
    {
        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string DisplayImage { get; set; }

        public static explicit operator ContentObject(RaptorDb.ContentObjectData v)
        {
            return new ContentObject() { Hash = v.Hash, Data = v.Data, FileName = v.FileName, DisplayImage = v.DisplayImage };
        }
    }
}
