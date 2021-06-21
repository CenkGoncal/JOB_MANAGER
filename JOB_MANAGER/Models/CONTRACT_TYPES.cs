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

    public partial class CONTRACT_TYPES: IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONTRACT_TYPES()
        {
            this.EMPLOYEES = new HashSet<EMPLOYEES>();
        }
    
        public short CONTRACT_TYPE_ID { get; set; }
        public string CONTRACT_TYPE_NAME { get; set; }
        public string CONTRACT_TYPE_CODE { get; set; }
        public string CONTRACT_TYPE_DESC { get; set; }
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
