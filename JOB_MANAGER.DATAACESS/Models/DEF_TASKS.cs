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
    
    public partial class DEF_TASKS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEF_TASKS()
        {
            this.PROJECT_TASK = new HashSet<PROJECT_TASK>();
        }
    
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public int PROJECT_TYPE_ID { get; set; }
        public int PHASE_ID { get; set; }
        public int TASK_ORDER { get; set; }
        public string TASK_DESC { get; set; }
        public bool COMPLETE_ON_UPLOAD { get; set; }
        public int ALLOCATE_SUPERVISOR { get; set; }
        public string MEMBERS { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
    
        public virtual DEF_PROJECT_PHASES DEF_PROJECT_PHASES { get; set; }
        public virtual PROJECT_TYPES PROJECT_TYPES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROJECT_TASK> PROJECT_TASK { get; set; }
    }
}