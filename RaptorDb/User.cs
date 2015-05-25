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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.ARecords = new HashSet<ARecord>();
            this.CRecords = new HashSet<CRecord>();
            this.OnlineHistories = new HashSet<OnlineHistory>();
            this.ProcessedBies = new HashSet<ProcessedBy>();
            this.RankHistories = new HashSet<RankHistory>();
            this.RRecords = new HashSet<RRecord>();
            this.Urls = new HashSet<Url>();
            this.WorkRecords = new HashSet<WorkRecord>();
        }
    
        public long Id { get; set; }
        public string UserFullName { get; set; }
        public string UserCountry { get; set; }
        public string UserLanguage { get; set; }
        public bool UserIsAdult { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserLicenseNumber { get; set; }
        public int UserStatusId { get; set; }
        public int UserRankId { get; set; }
        public long UserRankHistoryId { get; set; }
        public System.DateTime UserJoinedDate { get; set; }
        public Nullable<long> UserWorkRecordId { get; set; }
        public bool IsOnline { get; set; }
        public Nullable<double> TotalRunTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARecord> ARecords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRecord> CRecords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnlineHistory> OnlineHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProcessedBy> ProcessedBies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RankHistory> RankHistories { get; set; }
        public virtual Rank Rank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RRecord> RRecords { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Url> Urls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkRecord> WorkRecords { get; set; }
    }
}
