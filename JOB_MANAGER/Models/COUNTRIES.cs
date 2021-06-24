//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JOB_MANAGER.Models
{
    using System;
    using System.Collections.Generic;
    using static JOB_MANAGER.Models.Abstact.Entity;

    public partial class COUNTRIES: IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COUNTRIES()
        {
            this.STATES = new HashSet<STATES>();
            this.CITIES = new HashSet<CITIES>();
            this.COMPANY = new HashSet<COMPANY>();
            this.SUPPLIERS = new HashSet<SUPPLIERS>();
            this.CLIENTS = new HashSet<CLIENTS>();
            this.QUOTES = new HashSet<QUOTES>();
            this.PROJECTS = new HashSet<PROJECTS>();
        }
    
        public int COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string FLAG_URL { get; set; }
        public bool IS_DEFAULT { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATES> STATES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CITIES> CITIES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPANY> COMPANY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUPPLIERS> SUPPLIERS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTS> CLIENTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUOTES> QUOTES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROJECTS> PROJECTS { get; set; }
    }
}