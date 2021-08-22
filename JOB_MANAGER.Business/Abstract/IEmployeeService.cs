using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IEmployeeService
    {
        public bool CheckLogin(string userName, string password, bool remember, bool doAuth);
        public List<EmployeeExtended> GetAllByCompany(int CompanyID);
        public ShowState UpdateEmployeeImage(EMPLOYEES param);
        public List<EmployeeExtended> GetEmployeesByTypes(bool IsSupervisor, bool IsInstaler, bool IsDriver, int CompanyID);
        public List<EmployeeExtended> GetAllActive();
        public ShowState UpdateEmailSettings(EMPLOYEES param);
        public ShowState ChangePassword(int empId, string oldPwd, string newPwd, string confirmPwd);
        public ShowState UpdateProfileDetails(EMPLOYEES param);
        public ShowState UpdateNavbarClass(int EmpID, int Type, string navbarClass);
        public ShowState CopyNavbarAll(int EmpID);
    }
}
