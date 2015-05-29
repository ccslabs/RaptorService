using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FacesLib
{
    [Serializable]
    [DataContract]
    public partial class FacesObject
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string COFace { get; set; }
        [DataMember]
        public long COId { get; set; }
    }
}

