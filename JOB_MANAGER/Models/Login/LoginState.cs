using JOB_MANAGER.Controllers;
using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JOB_MANAGER.Models.Login
{
    public class LoginState
    {
        public JOB_MANAGER_DBEntities db;
        public LoginState()
        {
            db = new JOB_MANAGER_DBEntities();
        }

        public LoginEmpoloyeeDto chekLoginState(string username,string password,string remember,bool DoAuth)
        {                        
            string encrptPassword = GlobalTools.EncryptSystemString(password);
            //string aaa = GlobalTools.DecryptSystemString("jtd3z0SNMdLLiCzO/EMuIA==");
            var emp = db.EMPLOYEES.Where(w => w.SYSTEM_USERNAME == username && w.SYSTEM_PASSWORD == encrptPassword).FirstOrDefault();
            
            if (emp != null)//username == "Admin" && password == "Omtomdoy123!!!")
            {
                LoginEmpoloyeeDto empoloyeeDto = new LoginEmpoloyeeDto();
                empoloyeeDto.Employee = emp;
                empoloyeeDto.ContractRole = emp.ROLES;
                empoloyeeDto.ContractType = emp.CONTRACT_TYPES;
                empoloyeeDto.EmployeeStatus = emp.STATUS;
                empoloyeeDto.Title = emp.TITLES;
                //ThreadGlobals.EmpAuthHeader.Value = empoloyeeDto;                

                bool REMEMBER_ME = remember == "true" ? true : false;

                if (DoAuth)
                {
                    FormsAuthentication.SetAuthCookie(emp.SYSTEM_USERNAME, REMEMBER_ME);                    
                }



                return empoloyeeDto;
            }

            return null;
        }
    }
}