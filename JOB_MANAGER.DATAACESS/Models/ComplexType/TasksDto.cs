using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER.DATAACESS.Models.ComplexType
{
    public class TasksDto
    {
        public int PROJECT_ID { get; set; }
        public int PROJECT_TASK_ID { get; set; }
        public int TASK_ORDER { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public string PROJECT_TASK_NAME { get; set; }
        public int CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public int STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public int PROJECT_TASK_PROGRESS { get; set; }
        public string TASK_START_DATE { get; set; }
        public string TASK_END_DATE { get; set; }
        public string PROJECT_TASK_DESC { get; set; }
        public string MODIFIED_DATE { get; set; }
        public List<MembersDto> MEMBERS { get; set; }
    }
}
