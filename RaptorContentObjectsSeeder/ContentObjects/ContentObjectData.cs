using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace ContentObjects
{
    [Serializable]
    [DataContract]
    public class ContentObjectData
    {
        [DataMember]
        public string Hash { get; set; }
        [DataMember]
        public string Data { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string DisplayImage { get; set; }
    }
}

namespace Face
{ 
    [Serializable]
    [DataContract]
    public partial class Face
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string COFace { get; set; }
        [DataMember]
        public long COId { get; set; }        
    }
}
