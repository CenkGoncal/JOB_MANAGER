using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER.DATAACESS.Models.ComplexType
{
    public class ProjectViewDto
    {
        public int PROJECT_ID { get; set; }
        public int? QUOTE_ID { get; set; }
        public string QUOTE_NUMBER { get; set; }
        public string PROJECT_NUMBER { get; set; }
        public int CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string CLIENT_PO_NBR { get; set; }
        public int PROJECT_TYPE_ID { get; set; }
        public string PROJECT_TYPE_NAME { get; set; }
        public int PROJECT_STATUS { get; set; }
        public string PROJECT_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string STATUS_NAME { get; set; }
        public string ADR_LOT_NBR { get; set; }
        public string ADR_UNIT_NBR { get; set; }
        public string ADR_STREET_NBR { get; set; }
        public string ADR_STREET_NAME { get; set; }
        public string PROJECT_ADDRESS { get; set; }
        public int PROJECT_COUNTRY { get; set; }
        public string COUNTRY_NAME { get; set; }
        public int PROJECT_STATE { get; set; }
        public string STATE_NAME { get; set; }
        public int PROJECT_CITY { get; set; }
        public string CITY_NAME { get; set; }
        public string PROJECT_POSTAL_CODE { get; set; }
        public decimal PROJECT_AMOUNT { get; set; }
        public decimal PROJECT_ESTIMATED_VAL { get; set; }
        public decimal PROJECT_ACTUAL_VAL { get; set; }
        public int? CLIENT_SUPRVISOR { get; set; }
        public string CLI_SUPERVISOR_NAME { get; set; }
        public int? PROJECT_SUPERVISOR { get; set; }
        public string PRO_SUPERVISOR_NAME { get; set; }
        public int PROJECT_PROGRESS { get; set; }
        public int CABINETS_NBR { get; set; }
        public string PROJECT_DESC { get; set; }
        public string INSTALLATION_DATE { get; set; }
        public string TIME { get; set; }
        public string PROJECT_START_DATE { get; set; }
        public DateTime PROJECT_START_DATE_DT { get; set; }
        public string PROJECT_END_DATE { get; set; }
        public DateTime? PROJECT_END_DATE_DT { get; set; }
        public bool DISPLAY { get; set; }
        public short FLOOR_TYPE { get; set; }
        public string FLOOR_TYPE_NAME { get; set; }
        public int MATERIAL_ID { get; set; }
        public string MATERIAL_NAME { get; set; }
        public short? STREET_SUFFIX_ID { get; set; }
        public string STREET_NAME { get; set; }
        public bool USE_AS_TEMPLATE { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }

        public ProjectDto ProjectDto { get; set; }
    }
}
