using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER.DATAACESS.Models.ComplexType
{
    public class MembersDto
    {
        public int PROJECT_ID { get; set; }
        public int PROJECT_MEMBER_ID { get; set; }
        public int EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public byte[] EMP_IMG { get; set; }
        public int SUPERVISOR_ID { get; set; }
        public string SUP_NAME { get; set; }
        public bool IS_SUPERVISOR { get; set; }
        public int TITLE_ID { get; set; }
        public string TITLE_NAME { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
    }
}
