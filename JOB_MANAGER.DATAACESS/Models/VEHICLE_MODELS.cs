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
    
    public partial class VEHICLE_MODELS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VEHICLE_MODELS()
        {
            this.VEHICLES = new HashSet<VEHICLES>();
            this.VEHICLES1 = new HashSet<VEHICLES>();
        }
    
        public int VEHICLE_MODEL_ID { get; set; }
        public string VEHICLE_MODEL_NAME { get; set; }
        public int VEHICLE_MAKE_ID { get; set; }
        public int BODY_TYPE { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
    
        public virtual VEHICLE_BODY_TYPES VEHICLE_BODY_TYPES { get; set; }
        public virtual VEHICLE_MAKES VEHICLE_MAKES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEHICLES> VEHICLES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEHICLES> VEHICLES1 { get; set; }
    }
}
