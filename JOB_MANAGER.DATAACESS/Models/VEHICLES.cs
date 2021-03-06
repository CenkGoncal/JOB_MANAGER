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
    
    public partial class VEHICLES
    {
        public int VEHICLE_ID { get; set; }
        public string VEHICLE_NBR { get; set; }
        public short VEHICLE_STATUS { get; set; }
        public Nullable<int> VEHICLE_MODEL_ID { get; set; }
        public Nullable<int> VEHICLE_MAKE_ID { get; set; }
        public Nullable<int> BODY_TYPE_ID { get; set; }
        public Nullable<int> CURRENT_DRIVER { get; set; }
        public string REGISTRATION_NUMBER { get; set; }
        public Nullable<System.DateTime> REGISTRATION_EXPIRY { get; set; }
        public string ASSIGNED_TAG { get; set; }
        public string VEHICLE_DESC { get; set; }
        public Nullable<int> VEHICLE_YEAR { get; set; }
        public string VEHICLE_COLOR { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
        public int COMPANY_ID { get; set; }
    
        public virtual EMPLOYEES EMPLOYEES { get; set; }
        public virtual STATUS STATUS { get; set; }
        public virtual VEHICLE_BODY_TYPES VEHICLE_BODY_TYPES { get; set; }
        public virtual VEHICLE_MAKES VEHICLE_MAKES { get; set; }
        public virtual VEHICLE_MODELS VEHICLE_MODELS { get; set; }
        public virtual VEHICLE_MODELS VEHICLE_MODELS1 { get; set; }
    }
}
