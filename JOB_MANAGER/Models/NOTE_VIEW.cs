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

    public partial class NOTE_VIEW: IEntity
    {
        public int NOTE_ID { get; set; }
        public int UNIQ_ID { get; set; }
        public short NOTE_KIND_ID { get; set; }
        public string NOTE_DESC { get; set; }
        public string NOTE_SUBJECT { get; set; }
        public short NOTE_TYPE_ID { get; set; }
        public string NOTE_TYPE_NAME { get; set; }
        public Nullable<int> PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public Nullable<int> TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public Nullable<bool> SET_REMINDER { get; set; }
        public string ASSINGED_MEMBERS { get; set; }
        public string TO_REMINDER { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public Nullable<System.DateTime> REMINDER_DATE { get; set; }
    }
}
