//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RaptorDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class COSource
    {
        public long Id { get; set; }
        public long COId { get; set; }
        public Nullable<long> UrlId { get; set; }
    
        public virtual ContentObject ContentObject { get; set; }
        public virtual Url Url { get; set; }
    }
}
