using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using JOB_MANAGER.CrossCuttingConsers.Mapping;
using JOB_MANAGER.CrossCuttingConsers.Security.Web;
using JOB_MANAGER.CrossCuttingConsers.Transaction;
using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.ComplexType;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using Newtonsoft.Json;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class EmployeeManager : IService<EMPLOYEES, EmployeeExtended>
    {
        private EmployeeDal _dal;
        public EmployeeManager(EmployeeDal employeeDal)
        {
            _dal = employeeDal;
        }

        public ShowState AddorUpdate(EMPLOYEES param)
        {
            return _dal.AddorUpdate(param, f => f.EMP_ID == param.EMP_ID);
        }

        public ShowState Delete(EMPLOYEES param)
        {
            return _dal.Delete(param);
        }

        public EMPLOYEES Get(EMPLOYEES param)
        {
            return _dal.GetAll(f => f.EMP_ID == param.EMP_ID).FirstOrDefault();
        }


        public EmployeeExtended Get(string Username)
        {
            return _dal.GetAll2(f => f.SYSTEM_USERNAME == Username).FirstOrDefault();
        }


        public bool CheckLogin(string userName, string password,bool remember, bool doAuth)
        {
            string encrptPassword = GlobalTools.EncryptSystemString(password);
            var emp = _dal.GetAll(f => f.SYSTEM_USERNAME == userName && f.SYSTEM_PASSWORD == encrptPassword).FirstOrDefault();
            if (emp == null)
                return false;
            
            if(doAuth)
            {
                RoleMenuManager menuManager = new RoleMenuManager(new RoleMenuDal());

                RoleManager roleManager = new RoleManager(new RoleDal());

                var role = roleManager.GetAll().FirstOrDefault(w => w.ROLE_ID == emp.ROLE_ID);

                UserInfoDto userInfo = new UserInfoDto();
                userInfo.UserId = emp.EMP_ID;
                userInfo.UserName = emp.SYSTEM_USERNAME;
                userInfo.EmpName = emp.FIRST_NAME + SqlConstants.stringEmpty + emp.LAST_NAME;
                userInfo.CompanyId = emp.COMPANY_ID;
                userInfo.Email = emp.EMAIL_USERNAME;
                userInfo.EmailPassword = emp.EMAIL_PASSWORD;
                userInfo.RoleName = role.ROLE_NAME;
                userInfo.MenuRoles = menuManager.GetMenuByRole(emp.ROLE_ID).Where(w=>w.state == true).Select(s=>s.name).ToList();

                AuthenticationHelper.CreateAuthCookie(emp.EMP_ID, emp.SYSTEM_USERNAME, userInfo, DateTime.Now.AddDays(15), remember);

            }

            return true;
        }

        public List<EmployeeExtended> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<EmployeeExtended> GetAllByCompany(int CompanyID)
        {
            var emplist = _dal.GetAll2(f => f.IS_CANCELED == false && f.COMPANY_ID == CompanyID);          
            return emplist;
        }

        public List<EmployeeExtended> GetAllActive()
        {
            var emplist = _dal.GetAll2(f => f.IS_CANCELED == false);
            return emplist;
        }
        public List<EmployeeExtended> GetEmployeesByTypes(bool IsSupervisor,bool IsInstaler,bool IsDriver,int CompanyID)
        {
            var emplist = _dal.GetAll2(f => f.IS_CANCELED == false && f.COMPANY_ID == CompanyID);

            if (IsSupervisor)
                emplist = emplist.Where(w => w.IS_SUPERVISOR == true).OrderBy(o=>o.EMP_ID).ToList();
            else
            if (IsInstaler)
                emplist = emplist.Where(w => w.IS_INSTALLER == true).OrderBy(o => o.EMP_ID).ToList();
            else
            if (IsDriver)
                emplist = emplist.Where(w => w.IS_DRIVER == true).OrderBy(o => o.EMP_ID).ToList();

            return emplist;
        }

        [TransactionControl(TransactionType.Write)]
        public ShowState UpdateEmployeeImage(EMPLOYEES param)
        {
            var emp = _dal.GetAll(f => f.EMP_ID == param.EMP_ID).FirstOrDefault();
            if (emp == null)
            {
                return new ShowState() { isError = true, ErrorMessage = "Employee not found" };
            }

            emp.EMP_IMG = param.EMP_IMG;
            return AddorUpdate(emp);
        }

        [TransactionControl(TransactionType.Write)]
        public ShowState UpdateProfileDetails(EMPLOYEES param)
        {
            ShowState showState = new ShowState();
            
            if (string.IsNullOrEmpty(param.EMP_PHONE) && string.IsNullOrEmpty(param.MOBILE_PHONE))
            {
                showState.isError = true;
                showState.ErrorMessage = "Phone/Mobile cannot be empty\r\n";
                return showState;
            }
            else
            {
                var emp = _dal.GetAll(f => f.EMP_ID == param.EMP_ID).FirstOrDefault();
                if (emp == null)
                {
                    return new ShowState() { isError = true, ErrorMessage = "Employee not found" };
                }

                emp.EMP_PHONE = param.EMP_PHONE;
                emp.MOBILE_PHONE = param.MOBILE_PHONE;
                return AddorUpdate(emp);
            }
        }

        public ShowState ChangePassword(int empId, string oldPwd, string newPwd, string confirmPwd)
        {
            ShowState showState = new ShowState();            

            if (string.IsNullOrEmpty(oldPwd as string))
            {
                showState.ErrorMessage += "Old Password cannot be empty\r\n";
                showState.isError = true;
            }
            
            if (string.IsNullOrEmpty(newPwd as string))
            {
                showState.ErrorMessage += "New Password cannot be empty\r\n";
                showState.isError = true;
            }
            
            if (string.IsNullOrEmpty(confirmPwd as string))
            {
                showState.ErrorMessage += "Password Confirmation cannot be empty\r\n";
                showState.isError = true;
            }                            
            
            if (!newPwd.Equals(confirmPwd))
            {
                showState.ErrorMessage += "Password and Confirmation don't match";
                showState.isError = true;
            }

            if(showState.isError == false)
            {
                var emp = _dal.GetAll(f => f.EMP_ID == empId).FirstOrDefault();
                if (emp == null)
                {
                    return new ShowState() { isError = true, ErrorMessage = "Employee not found" };
                }

                if (!emp.SYSTEM_PASSWORD.Equals(GlobalTools.EncryptSystemString(oldPwd)))
                {
                    showState.ErrorMessage += "Old password doesn't not match";
                    showState.isError = true;
                }
                else
                {
                    emp.SYSTEM_PASSWORD = GlobalTools.EncryptSystemString(newPwd);
                    showState = AddorUpdate(emp);
                }
            }

            return showState;
        }

        [TransactionControl(TransactionType.Write)]
        public ShowState UpdateEmailSettings(EMPLOYEES param)
        {
            ShowState showState = new ShowState();
            if (string.IsNullOrEmpty(param.EMAIL_USERNAME))
            {
                showState.isError = true;
                showState.ErrorMessage += "Email field is required\r\n";
            }
            else
            {
                if(param.EMAIL_USERNAME.IsEmail() == false)
                {
                    return new ShowState() { isError = true, ErrorMessage = "Email format not valid" };
                }
                
                var emp = _dal.GetAll(f => f.EMP_ID == param.EMP_ID).FirstOrDefault();
                if (emp == null)
                {
                    return new ShowState() { isError = true, ErrorMessage = "Employee not found" };
                }

                emp.EMAIL_USERNAME = param.EMAIL_USERNAME;
                if ((emp.EMAIL_PASSWORD != null && !emp.EMAIL_PASSWORD.Equals(param.EMAIL_PASSWORD)) ||
                    (emp.EMAIL_PASSWORD == null && !string.IsNullOrEmpty(param.EMAIL_PASSWORD))
                   )
                    emp.EMAIL_PASSWORD = GlobalTools.EncryptSystemString(param.EMAIL_PASSWORD, true);

                emp.SIGNATURE = param.SIGNATURE;
                emp.SHOW_SIGNATURE = param.SHOW_SIGNATURE;
                showState = AddorUpdate(emp);
            }

            return showState;
        }
        
        public ShowState UpdateNavbarClass(int EmpID,int Type, string navbarClass)
        {
            var emp = _dal.GetAll(f => f.EMP_ID == EmpID).FirstOrDefault();
            if(emp == null)
            {
                return new ShowState(){ isError = true, ErrorMessage = "Employee not found" };
            }

            List<CustimezeThemeDto> objNavbarClass = new List<CustimezeThemeDto>();

            if (string.IsNullOrEmpty(emp.NAVBAR_CLASS))
            {
                CustimezeThemeDto objNavbar = new CustimezeThemeDto();
                objNavbar.Type = 1; objNavbar.Class = "navbar-white";
                objNavbarClass.Add(objNavbar);
                CustimezeThemeDto objNavbar2 = new CustimezeThemeDto();
                objNavbar2.Type = 2; objNavbar2.Class = "sidebar-dark-primary";
                objNavbarClass.Add(objNavbar2);
                CustimezeThemeDto objNavbar3 = new CustimezeThemeDto();
                objNavbar3.Type = 3; objNavbar3.Class = "navbar-gray-dark";
                objNavbarClass.Add(objNavbar3);
            }
            else
            {
                objNavbarClass = JsonConvert.DeserializeObject<List<CustimezeThemeDto>>(emp.NAVBAR_CLASS);
                foreach (var item in objNavbarClass)
                {
                    if (item.Type == Type)
                    {
                        item.Class = navbarClass;
                        break;
                    }
                }
            }

            emp.NAVBAR_CLASS = JsonConvert.SerializeObject(objNavbarClass);
            return AddorUpdate(emp);
        }
        
        [TransactionControl(TransactionType.Write)]
        public ShowState CopyNavbarAll(int EmpID)        {

            var emp = _dal.GetAll(f => f.EMP_ID == EmpID).FirstOrDefault();
            if (emp == null)
            {
                return new ShowState() { isError = true, ErrorMessage = "Employee not found" };
            }

            ShowState showState = new ShowState();
            foreach (var employee in _dal.GetAll(w => w.IS_CANCELED == false))
            {
                employee.NAVBAR_CLASS = emp.NAVBAR_CLASS;

                showState = AddorUpdate(employee);
                if (showState.isError)
                    break;
            }

            return showState;            
        }
    }
}
