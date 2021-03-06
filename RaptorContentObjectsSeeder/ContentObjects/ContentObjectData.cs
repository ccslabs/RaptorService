﻿using System;
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


