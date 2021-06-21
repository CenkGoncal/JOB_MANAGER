using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JOB_MANAGER.Controllers
{
    public class EmployeeController : BaseController
    {

        [AuthorityControl("Employees")]
        public ActionResult Index()
        {
            var CompanyID = GetCompanyID();
            ViewBag.DEPARTMENT_LIST = new SelectList(db.DEPARTMENTS.Where(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyID)), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.EMPLOYEE_STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Employee), "STATUS_ID", "STATUS_NAME");
            ViewBag.ROLE_LIST = new SelectList(db.ROLES.Where(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyID)), "ROLE_ID", "ROLE_NAME");
            ViewBag.TITLE_LIST = new SelectList(db.TITLES.Where(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyID)), "TITLE_ID", "TITLE_NAME");
            ViewBag.CONTRACT_TYPE_LIST = new SelectList(db.CONTRACT_TYPES.Where(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyID)), "CONTRACT_TYPE_ID", "CONTRACT_TYPE_NAME");
            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            ViewBag.DefaultProfileImg = Convert.ToBase64String(GlobalTools.GetParamByteVal("NO_PICTURE_IMG", GetCompanyID()));

            string DefaultPhoneMask = GlobalTools.GetParamStrVal("PHONE_MASK", GetCompanyID());
            ViewBag.DefaultPhoneMask = string.IsNullOrEmpty(DefaultPhoneMask) ? "(999) 999-9999" : DefaultPhoneMask;

            string DefaultMobileMask = GlobalTools.GetParamStrVal("MOBILE_MASK", GetCompanyID());
            ViewBag.DefaultMobileMask = string.IsNullOrEmpty(DefaultMobileMask) ? "(999) 999-9999" : DefaultMobileMask;

            return View();
        }

        [HttpGet]
        public ActionResult GetEmployeeList()
        {
            var empID = GetUserID();

            if (empID < 0)
            {
                return RedirectToAction("LockedUser", "Login");
            }

            var CompanyID = GetCompanyID();

            var data = (from e in db.EMPLOYEES
                        join c in db.CONTRACT_TYPES on e.CONTRACT_TYPE_ID equals c.CONTRACT_TYPE_ID
                        join d in db.DEPARTMENTS on e.DEPARTMENT_ID equals d.DEPARTMENT_ID
                        join s in db.STATUS on e.EMP_STATUS_ID equals s.STATUS_ID
                        where e.IS_CANCELED == false && e.COMPANY_ID == CompanyID
                        select new EmployeeDto
                        {
                            EMP_ID = e.EMP_ID,
                            EMP_IMG = e.EMP_IMG,
                            EMP_INITIALS = e.EMP_INITIALS,
                            EMP_NUMBER = e.EMP_NUMBER,
                            FIRST_NAME = e.FIRST_NAME,
                            LAST_NAME = e.LAST_NAME,
                            EMP_NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME,
                            TITLE_ID = e.TITLE_ID,
                            ROLE_ID = e.ROLE_ID,
                            EMP_TITLE = e.TITLES.TITLE_NAME,
                            CONTRACT_TYPE_ID = e.CONTRACT_TYPE_ID,
                            EMP_CONTRACT = c.CONTRACT_TYPE_CODE,
                            DEPARTMENT_ID = e.DEPARTMENT_ID,
                            EMP_DEPARTMENT = d.DEPARTMENT_NAME,
                            EMP_EMAIL = e.EMAIL_USERNAME,
                            EMP_STATUS_ID = e.EMP_STATUS_ID,
                            SYSTEM_USERNAME = e.SYSTEM_USERNAME,
                            SYSTEM_PASSWORD = e.SYSTEM_PASSWORD,
                            HIRED_DATE_STR = e.HIRED_DATE != null ?
                                                    e.HIRED_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (e.HIRED_DATE.Value.Month > 9 ? e.HIRED_DATE.Value.Month + SqlConstants.stringMinus : "0" + e.HIRED_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    e.HIRED_DATE.Value.Day : null,
                            EMP_STATUS = s.STATUS_NAME,
                            DISPLAY_CLASS = s.DISPLAY_CLASS,
                            EMP_PHONE = e.EMP_PHONE,
                            PHONE_EXTENSION = e.PHONE_EXTENSION,
                            MOBILE_PHONE = e.MOBILE_PHONE,
                            IS_SUPERVISOR = e.IS_SUPERVISOR,
                            IS_DRIVER = e.IS_DRIVER,
                            EMP_COMPANY_ID = e.COMPANY_ID,

                        }
                       ).OrderBy(o => o.EMP_NAME).ToList();


            foreach (var item in data)
            {
                item.SYSTEM_PASSWORD = GlobalTools.DecryptSystemString(item.SYSTEM_PASSWORD);
            }

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var empID = GetUserID();

            if (empID < 0)
            {
                return RedirectToAction("LockedUser", "Login");
            }

            EMPLOYEES employee = db.EMPLOYEES.Find(empID);

            if (employee == null)
            {
                RedirectToAction("Index", "Home");
            }

            ViewBag.Employee = employee;
            ViewBag.DefaultProfileImg = GlobalTools.GetParamByteVal("NO_PICTURE_IMG", GetCompanyID());

            string DefaultPhoneMask = GlobalTools.GetParamStrVal("PHONE_MASK", GetCompanyID());
            ViewBag.DefaultPhoneMask = string.IsNullOrEmpty(DefaultPhoneMask) ? "(999) 999-9999" : DefaultPhoneMask;

            string DefaultMobileMask = GlobalTools.GetParamStrVal("MOBILE_MASK", GetCompanyID());
            ViewBag.DefaultMobileMask = string.IsNullOrEmpty(DefaultMobileMask) ? "(999) 999-9999" : DefaultMobileMask;

            return View();
        }

        [HttpPost]
        public ActionResult UpdateEmployeeImg(int EmpID, HttpPostedFileBase Image, int Remove)
        {
            EMPLOYEES employee = db.EMPLOYEES.Find(EmpID);
            if (employee != null)
            {
                try
                {
                    string Message = string.Empty;
                    bool State = false;

                    if (Image != null)
                    {
                        employee.EMP_IMG = new byte[Image.ContentLength];
                        Image.InputStream.Read(employee.EMP_IMG, 0, Image.ContentLength);
                        //employee.EMPLOYEE_IMG = GlobalTools.ResizeEmployeeImage(employee.EMPLOYEE_IMG);

                        State = true;
                        Message = "Image Chamged";
                    }
                    else
                    {
                        employee.EMP_IMG = null;

                        State = Remove == 1;
                        Message = State ? "Image Chamged" : "No Image found";
                    }

                    db.EMPLOYEES.Attach(employee);
                    db.Entry(employee).Property(x => x.EMP_IMG).IsModified = true;


                    db.SaveChanges();

                    return Json(new { success = State, message = Message });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                //}
            }

            return Json(new { success = false, message = "No Employee found" });
        }

        [HttpPost]
        public JsonResult UpdateProfileDetails(int id, string phone, string extension, string mobile)
        {
            bool checkData = true;
            string errorMessage = string.Empty;
            EMPLOYEES employee = db.EMPLOYEES.Find(id);
            if (employee != null)
            {
                if (string.IsNullOrEmpty(phone as string) && string.IsNullOrEmpty(mobile as string))
                {
                    checkData = false;
                    errorMessage += "Phone/Mobile cannot be empty\r\n";
                }
                if (checkData)
                {
                    try
                    {
                        employee.EMP_PHONE = phone;
                        employee.PHONE_EXTENSION = extension;
                        employee.MOBILE_PHONE = mobile;

                        db.EMPLOYEES.Attach(employee);
                        db.Entry(employee).Property(x => x.EMP_PHONE).IsModified = true;
                        db.Entry(employee).Property(x => x.PHONE_EXTENSION).IsModified = true;
                        db.Entry(employee).Property(x => x.MOBILE_PHONE).IsModified = true;

                        db.SaveChanges();

                        Session["UserInfo"] = employee;

                        return Json(new { success = true, message = "Information Info Changed" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                }
            }
            return Json(new { success = false, message = errorMessage });
        }

        [HttpPost]
        public JsonResult ChangePassword(int id, string oldPwd, string newPwd, string confirmPwd)
        {
            bool checkData = true;
            string errorMessage = string.Empty;
            EMPLOYEES employee = db.EMPLOYEES.Find(id);
            if (employee != null)
            {
                if (string.IsNullOrEmpty(oldPwd as string))
                {
                    errorMessage += "Old Password cannot be empty\r\n";
                    checkData = false;
                }
                if (string.IsNullOrEmpty(newPwd as string))
                {
                    errorMessage += "New Password cannot be empty\r\n";
                    checkData = false;
                }
                if (string.IsNullOrEmpty(confirmPwd as string))
                {
                    errorMessage += "Password Confirmation cannot be empty\r\n";
                    checkData = false;
                }
                if (!checkData)
                    goto ErrorWasFound;
                if (!employee.SYSTEM_PASSWORD.Equals(GlobalTools.EncryptSystemString(oldPwd)))
                {
                    errorMessage += "Old password doesn't not match";
                    goto ErrorWasFound;
                }
                if (!newPwd.Equals(confirmPwd))
                {
                    errorMessage += "Password and Confirmation don't match";
                    goto ErrorWasFound;
                }
                if (checkData)
                {
                    try
                    {
                        employee.SYSTEM_PASSWORD = GlobalTools.EncryptSystemString(newPwd);
                        db.EMPLOYEES.Attach(employee);
                        db.Entry(employee).Property(x => x.SYSTEM_PASSWORD).IsModified = true;

                        db.SaveChanges();

                        return Json(new { success = true, message = "Success" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                }
            }
        ErrorWasFound:
            return Json(new { success = false, message = errorMessage });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateEmailSettings(int id, string email, string pwd, string signature, bool showSignature)
        {
            bool checkData = true;
            string errorMessage = string.Empty;
            EMPLOYEES employee = db.EMPLOYEES.Find(id);
            if (employee != null)
            {
                if (string.IsNullOrEmpty(email as string))
                {
                    checkData = false;
                    errorMessage += "Email field is required\r\n";
                }
                if (!checkData)
                    goto ErrorWasFound;
                if (checkData)
                {
                    try
                    {
                        employee.EMAIL_USERNAME = email;
                        if ((employee.EMAIL_PASSWORD != null && !employee.EMAIL_PASSWORD.Equals(pwd)) ||
                            (employee.EMAIL_PASSWORD == null && !string.IsNullOrEmpty(pwd as string))
                           )
                            employee.EMAIL_PASSWORD = GlobalTools.EncryptSystemString(pwd, true);

                        employee.SIGNATURE = signature;
                        employee.SHOW_SIGNATURE = showSignature;
                        db.EMPLOYEES.Attach(employee);
                        db.Entry(employee).Property(x => x.EMAIL_USERNAME).IsModified = true;
                        if ((employee.EMAIL_PASSWORD != null && !employee.EMAIL_PASSWORD.Equals(pwd)) ||
                            (employee.EMAIL_PASSWORD == null && !string.IsNullOrEmpty(pwd as string))
                           )
                            db.Entry(employee).Property(x => x.EMAIL_PASSWORD).IsModified = true;

                        db.Entry(employee).Property(x => x.SIGNATURE).IsModified = true;
                        db.Entry(employee).Property(x => x.SHOW_SIGNATURE).IsModified = true;

                        db.SaveChanges();
                        return Json(new { success = true, message = "Success" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                }
            }
        ErrorWasFound:
            return Json(new { success = false, message = errorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateEmployee(EMPLOYEES param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                EMPLOYEES emp = db.EMPLOYEES.Where(w => w.EMP_ID == param.EMP_ID).FirstOrDefault();

                bool isNew = false;
                if (emp == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();
                    param.SYSTEM_PASSWORD = GlobalTools.EncryptSystemString(param.SYSTEM_PASSWORD);

                    db.EMPLOYEES.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    emp.MODIFIED_DATE = DateTime.Now;
                    emp.EMP_INITIALS = param.EMP_INITIALS;
                    emp.EMP_NUMBER = param.EMP_NUMBER;
                    emp.FIRST_NAME = param.FIRST_NAME;
                    emp.LAST_NAME = param.LAST_NAME;
                    emp.TITLE_ID = param.TITLE_ID;
                    emp.ROLE_ID = param.ROLE_ID;
                    emp.CONTRACT_TYPE_ID = param.CONTRACT_TYPE_ID;
                    emp.DEPARTMENT_ID = param.DEPARTMENT_ID;
                    emp.EMP_STATUS_ID = param.EMP_STATUS_ID;
                    emp.HIRED_DATE = param.HIRED_DATE;
                    emp.EMP_PHONE = param.EMP_PHONE;
                    emp.PHONE_EXTENSION = param.PHONE_EXTENSION;
                    emp.MOBILE_PHONE = param.MOBILE_PHONE;
                    emp.IS_SUPERVISOR = param.IS_SUPERVISOR;
                    emp.IS_DRIVER = param.IS_DRIVER;
                    emp.IS_INSTALLER = param.IS_INSTALLER;
                    emp.EMAIL_USERNAME = param.EMAIL_USERNAME;
                    emp.TEAM_LEADER_ID = param.TEAM_LEADER_ID == 0 ? null : param.TEAM_LEADER_ID;
                    emp.SYSTEM_USERNAME = param.SYSTEM_USERNAME;

                    if (!string.IsNullOrEmpty(param.SYSTEM_PASSWORD))
                        emp.SYSTEM_PASSWORD = GlobalTools.EncryptSystemString(param.SYSTEM_PASSWORD);
                    //emp.IS_CANCELED = param.IS_CANCELED;
                    emp.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;

                    db.EMPLOYEES.Attach(emp);
                    db.Entry(emp).Property(x => x.EMP_INITIALS).IsModified = true;
                    db.Entry(emp).Property(x => x.EMP_NUMBER).IsModified = true;
                    db.Entry(emp).Property(x => x.FIRST_NAME).IsModified = true;
                    db.Entry(emp).Property(x => x.LAST_NAME).IsModified = true;
                    db.Entry(emp).Property(x => x.TITLE_ID).IsModified = true;
                    db.Entry(emp).Property(x => x.ROLE_ID).IsModified = true;
                    db.Entry(emp).Property(x => x.CONTRACT_TYPE_ID).IsModified = true;
                    db.Entry(emp).Property(x => x.DEPARTMENT_ID).IsModified = true;
                    db.Entry(emp).Property(x => x.EMP_STATUS_ID).IsModified = true;
                    db.Entry(emp).Property(x => x.HIRED_DATE).IsModified = true;
                    db.Entry(emp).Property(x => x.EMP_PHONE).IsModified = true;
                    db.Entry(emp).Property(x => x.PHONE_EXTENSION).IsModified = true;
                    db.Entry(emp).Property(x => x.MOBILE_PHONE).IsModified = true;
                    db.Entry(emp).Property(x => x.IS_SUPERVISOR).IsModified = true;
                    db.Entry(emp).Property(x => x.IS_DRIVER).IsModified = true;
                    db.Entry(emp).Property(x => x.IS_INSTALLER).IsModified = true;
                    db.Entry(emp).Property(x => x.UPDATED_BY).IsModified = true;
                    db.Entry(emp).Property(x => x.EMAIL_USERNAME).IsModified = true;
                    db.Entry(emp).Property(x => x.TEAM_LEADER_ID).IsModified = true;
                    db.Entry(emp).Property(x => x.SYSTEM_USERNAME).IsModified = true;
                    db.Entry(emp).Property(x => x.SYSTEM_PASSWORD).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }


            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int EmpID)
        {
            EMPLOYEES anEmployee = db.EMPLOYEES.Find(EmpID);
            if (anEmployee != null)
            {
                try
                {
                    anEmployee.IS_CANCELED = true;
                    anEmployee.UPDATED_BY = GetUserID();
                    db.EMPLOYEES.Attach(anEmployee);

                    db.Entry(anEmployee).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anEmployee).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Employee not found" });
        }


        [HttpPost]
        public JsonResult UpdateNavbarClass(int Type, string navbarClass)
        {
            int EmpID = GetUserID();
            EMPLOYEES anEmployee = db.EMPLOYEES.Find(EmpID);
            if (anEmployee != null)
            {
                try
                {
                    List<CustimezeThemeDto> objNavbarClass = new List<CustimezeThemeDto>();

                    if (string.IsNullOrEmpty(anEmployee.NAVBAR_CLASS) )
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
                        objNavbarClass = JsonConvert.DeserializeObject<List<CustimezeThemeDto>>(anEmployee.NAVBAR_CLASS);
                        foreach (var item in objNavbarClass)
                        {
                            if (item.Type == Type)
                            {
                                item.Class = navbarClass;
                                break;
                            }
                        }
                    }

                    anEmployee.NAVBAR_CLASS = JsonConvert.SerializeObject(objNavbarClass);                   
                    anEmployee.UPDATED_BY = GetUserID();
                    db.EMPLOYEES.Attach(anEmployee);

                    db.Entry(anEmployee).Property(x => x.NAVBAR_CLASS).IsModified = true;
                    db.Entry(anEmployee).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Employee not found" });
        }

        [HttpGet]
        public bool CopyNavbarAll()
        {
            int EmpID = GetUserID();
            EMPLOYEES anEmployee = db.EMPLOYEES.Find(EmpID);
            if (anEmployee != null)
            {

                try
                {
                    var objNavbarClass = JsonConvert.DeserializeObject<List<CustimezeThemeDto>>(anEmployee.NAVBAR_CLASS);


                    var OtherEmp = db.EMPLOYEES.Where(w => w.EMP_ID != EmpID && w.IS_CANCELED == false).ToList();
                    foreach (var emp in OtherEmp)
                    {
                        emp.NAVBAR_CLASS = anEmployee.NAVBAR_CLASS;
                        emp.UPDATED_BY = GetUserID();
                        db.EMPLOYEES.Attach(emp);

                        db.Entry(emp).Property(x => x.NAVBAR_CLASS).IsModified = true;
                        db.Entry(emp).Property(x => x.UPDATED_BY).IsModified = true;

                        db.SaveChanges();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        [HttpPost]
        public JsonResult SendEmail(string SenderMail, string CCMail, string Subject, string Body)
        {
            string _message = string.Empty;

            Body = Body.Replace("&lt;", "<").Replace("&gt;", ">");

            List<string> DosyaEkli = new List<string>();
            _message = GlobalTools.SendEmail(SenderMail, CCMail, Subject, Body, DosyaEkli, GetUserID());


            return Json(new { success = _message.IndexOf("Sended Email") > 0, Message = _message });
        }

        [HttpGet]
        public JsonResult GetAllEmailAdress(int TypeID)
        {
            if(TypeID == 1)
            {
                var query = (from e in db.EMPLOYEES
                             where e.IS_CANCELED == false && e.EMAIL_USERNAME != SqlConstants.stringEmpty
                             select new
                             {
                                 EMAIL = e.EMAIL_USERNAME,
                                 NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
                             });
                return Json(new { Getlist = query.ToList(), success = true }, JsonRequestBehavior.AllowGet);
            }
            else if(TypeID == 2)
            {
                var query2 = (from c in db.CONTACTS
                              where c.IS_CANCELED == false && c.CONTACT_EMAIL != SqlConstants.stringEmpty
                              select new
                              {
                                  EMAIL = c.CONTACT_EMAIL,
                                  NAME = c.CONTACT_FIRST_NAME + SqlConstants.stringWhiteSpace + c.CONTACT_LAST_NAME
                              });

                return Json(new { Getlist = query2.ToList(), success = true }, JsonRequestBehavior.AllowGet);
            }
            else if (TypeID == 3)
            {
                var query3 = (from s in db.SUPPLIERS
                              where s.IS_CANCELED == false && s.SUPPLIER_EMAIL != SqlConstants.stringEmpty
                              select new
                              {
                                  EMAIL = s.SUPPLIER_EMAIL,
                                  NAME = s.SUPPLIER_NAME
                              });

                return Json(new { Getlist = query3.ToList(), success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}