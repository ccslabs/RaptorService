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
    
    public partial class OnlineHistory
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public System.DateTime OnlineAt { get; set; }
        public Nullable<System.DateTime> OfflineAt { get; set; }
        public Nullable<int> SessionTime { get; set; }
    
        public virtual User User { get; set; }
    }
}
