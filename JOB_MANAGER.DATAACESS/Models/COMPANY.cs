//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JOB_MANAGER.DATAACESS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class COMPANY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMPANY()
        {
            this.CLIENTS = new HashSet<CLIENTS>();
            this.CONTACTS = new HashSet<CONTACTS>();
            this.QUOTES = new HashSet<QUOTES>();
            this.PROJECTS = new HashSet<PROJECTS>();
        }
    
        public int COMPANY_ID { get; set; }
        public string COMPANY_CODE { get; set; }
        public string COMPANY_NAME { get; set; }
        public short COMPANY_TYPE_ID { get; set; }
        public int COMPANY_COUNTRY { get; set; }
        public int COMPANY_STATE { get; set; }
        public int COMPANY_CITY { get; set; }
        public string COMPANY_ADDRESS { get; set; }
        public string POSTAL_CODE { get; set; }
        public string WEB_URL { get; set; }
        public string COMPANY_ABN { get; set; }
        public string COMPANY_PHONE { get; set; }
        public string COMPANY_OWNER { get; set; }
        public string COMPANY_LINKED_IN { get; set; }
        public byte[] COMPANY_LOGO { get; set; }
        public string COMPANY_DESC { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
    
        public virtual CITIES CITIES { get; set; }
        public virtual COMPANY_TYPES COMPANY_TYPES { get; set; }
        public virtual COUNTRIES COUNTRIES { get; set; }
        public virtual STATES STATES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTS> CLIENTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTACTS> CONTACTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUOTES> QUOTES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROJECTS> PROJECTS { get; set; }
    }
}