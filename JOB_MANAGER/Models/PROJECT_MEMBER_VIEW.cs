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

    public partial class PROJECT_MEMBER_VIEW: IEntity
    {
        public int PROJECT_MEMBER_ID { get; set; }
        public bool IS_SUPERVISOR { get; set; }
        public string EMP_NAME { get; set; }
        public short TITLE_ID { get; set; }
        public string TITLE_NAME { get; set; }
        public Nullable<int> PROJECT_PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public Nullable<int> TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public int PROJECT_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public Nullable<int> SUPERVISOR_ID { get; set; }
        public string SUP_NAME { get; set; }
        public byte[] EMP_IMG { get; set; }
        public byte[] SUP_IMG { get; set; }
    }
}
