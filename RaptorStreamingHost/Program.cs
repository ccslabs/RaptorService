using System;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ContentObjects;

namespace RaptorStreamingHost
{
    class Program
    {
        static void Main()
        {
            ServiceHost host = new ServiceHost(typeof(RaptorStreamingHost), new Uri("net.tcp://ccs-labs.com:804/"));
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            host.AddServiceEndpoint(typeof(RaptorStreamingHost), binding, string.Empty);
            host.Open();
            Console.WriteLine("Streaming Service started, press any key to exit");
            Console.ReadKey();
            host.Close();
        }
    }

    [ServiceContract]
    public class RaptorStreamingHost
    {
        RaptorDb.Methods RAPI = new RaptorDb.Methods();

        #region Operation Contracts

        [OperationContract]
        public void SendFaces(Stream data)
        {
            var memoryStream = new MemoryStream();
            using (data)
                CopyStream(data, memoryStream);
            byte[] alBytes = memoryStream.ToArray();
            data.Close();
            FacesLib.FacesObject fac = DeserializeFaceFromStream(alBytes);
            if (RAPI.SaveFaces((RaptorDb.Face)fac))
                Console.WriteLine("Saved Face");
            else
                Console.WriteLine("Failed to Save Face");

        }

        [OperationContract]
        public bool SendContentObject(Stream data)
        {
            var memoryStream = new MemoryStream();
            using (data)
                CopyStream(data, memoryStream);
            byte[] alBytes = memoryStream.ToArray();
            data.Close();
            ContentObjectData cod = DeserializeContentObjectFromStream(alBytes);

            bool res = RAPI.SaveContentObject((RaptorDb.ContentObject)cod);
            if (res)
            {
                Console.WriteLine("Saved Content Object");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to Save Content Object");
                return false;
            }
        }

        [OperationContract]
        public Stream GetContentObject()
        {
            Stream stream = null;
            stream = SerializeObjectToStream(RAPI.GetContentObject());
            stream.Position = 0L;
            return stream;
        }

        [OperationContract]
        public long GetContentObjectId(string coHash)
        {
            return RAPI.GetContentObjectId(coHash);
        }

        #endregion

        #region Deserialization

        private static FacesLib.FacesObject DeserializeFaceFromStream(byte[] allBytes)
        {
            using (var stream = new MemoryStream(allBytes))
                return (FacesLib.FacesObject)DeserializeFaceFromStream(stream);
        }

        private static FacesLib.FacesObject DeserializeFaceFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            FacesLib.FacesObject objectType = (FacesLib.FacesObject)formatter.Deserialize(stream);
            return objectType;
        }

        private static ContentObjectData DeserializeContentObjectFromStream(byte[] allBytes)
        {
            using (var stream = new MemoryStream(allBytes))
                return (ContentObjectData)DeserializeContentObjectFromStream(stream);
        }

        private static ContentObjectData DeserializeContentObjectFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            ContentObjectData objectType = (ContentObjectData)formatter.Deserialize(stream);
            return objectType;
        }

        #endregion

        #region Serialization

        private static Stream SerializeObjectToStream(object objectType)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectType);
            stream.Position = 0L; // REMEMBER to reset stream or WCF will just send the stream from the end resulting in an empty stream!
            return (Stream)stream;
        }
        #endregion

        #region Stream Management

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        #endregion


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


}
