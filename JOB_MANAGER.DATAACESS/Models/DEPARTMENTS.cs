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
    
    public partial class DEPARTMENTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEPARTMENTS()
        {
            this.EMPLOYEES = new HashSet<EMPLOYEES>();
        }
    
        public short DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
        public int COMPANY_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEES> EMPLOYEES { get; set; }
    }
}
