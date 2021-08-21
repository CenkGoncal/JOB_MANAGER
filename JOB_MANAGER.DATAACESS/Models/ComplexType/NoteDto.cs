using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER.DATAACESS.Models.ComplexType
{
    public class NoteDto
    {
        public int NOTE_ID { get; set; }
        public int UNIQ_ID { get; set; }
        public int NOTE_KIND_ID { get; set; }
        public int NOTE_TYPE_ID { get; set; }
        public string NOTE_TYPE_NAME { get; set; }
        public string NOTE_DESC { get; set; }
        public string NOTE_SUBJECT { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public bool SET_REMINDER { get; set; }
        public string REMINDER_DATE { get; set; }
        public string REMINDER_TIME { get; set; }
        public List<int> ASSINGED_MEMBERS_ARR { get; set; }
        public List<int> TO_REMIND_ARR { get; set; }
    }
}
