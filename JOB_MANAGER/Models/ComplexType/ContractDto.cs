
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace JOB_MANAGER.Models
{
    public class LoginEmpoloyeeDto
    {
        public EMPLOYEES Employee { get; set; }
        public STATUS EmployeeStatus { get; set; }
        public TITLES Title { get; set; }
        public CONTRACT_TYPES ContractType { get; set; }
        public ROLES ContractRole { get; set; }
    }

    public class WebLoadCountries
    {
        public string COUNTRY_NAME { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string FLAG { get; set; }
    }

    public class AssingEmpDto
    {
        public int EmpID { get; set; }        
    }

    public class LockedEmpInfo
    {
        public int EMP_ID { get; set; }

        public string EMP_NAME { get; set; }

        public byte[] EMP_IMG { get; set; }
    }

    public class QuoteAdvancedSearch
    {
        public DateTime FromInit { get; set; }
        public DateTime EndInit { get; set; }
        public string LotNumber { get; set; }
        public int Material { get; set; }
        public int Floor { get; set; }
        public int ProjectType { get; set; }
    }

    public class JobAdvancedSearch 
    {
        public DateTime FromStart { get; set; }
        public DateTime EndStart { get; set; }
        public string LotNumber { get; set; }
        public int Material { get; set; }
        public int Floor { get; set; }
        public int ProjectType { get; set; }
        public int Client { get; set; }
        public int Supervisor { get; set; }
        public int Phase { get; set; }
    }

    public class CustimezeThemeDto
    {
        public int Type { get; set; }

        public string Class { get; set; }

    }

    //public class MenuRolesDto
    //{
    //    public string name { get; set; }

    //    public bool state { get; set; }

    //}
    enum StatusType { Employee = 1, Vehicle = 2, Supplier = 3, Client = 4, Quonte = 5, Project = 6, PhaseTask = 7 };
    enum NoteType { Quote = 1, Project = 2};
    enum DocumentType { Quote = 1, Project = 2 };
}
