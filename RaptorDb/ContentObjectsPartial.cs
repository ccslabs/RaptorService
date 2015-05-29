using System;
using System.Collections.Generic;
using ContentObjects;

namespace RaptorDb
{
   
    public partial class ContentObject
    {
        public static explicit operator ContentObject(ContentObjectData v)
        {
            return new ContentObject() { COData = v.Data, CODisplayImage = v.DisplayImage, COHash = v.Hash, COName = v.FileName };
        }
    }
}

