using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers;
using JOB_MANAGER.DATAACESS.Helper;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.Controllers
{
    public class EmployeeController : BaseController
    {

        [AuthorityControl("Employees")]
        public ActionResult Index()
        {
            DepartmentManager department = new DepartmentManager(new DepartmentDal());
            ViewBag.DEPARTMENT_LIST = new SelectList(department.GetDepartmentCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "DEPARTMENT_ID", "DEPARTMENT_NAME");

            StatusManager status = new StatusManager(new StatusDal());
            ViewBag.EMPLOYEE_STATUS_LIST = new SelectList(status.GetAllByType((int)StatusType.Employee), "STATUS_ID", "STATUS_NAME");

            RoleManager role = new RoleManager(new RoleDal());
            ViewBag.ROLE_LIST = new SelectList(role.GetRoleCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "ROLE_ID", "ROLE_NAME");

            TitleManager title = new TitleManager(new TitleDal());
            ViewBag.TITLE_LIST = new SelectList(title.GetTitleCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "TITLE_ID", "TITLE_NAME");

            ContractTypeManager contractType = new ContractTypeManager(new ContractTypeDal());
            ViewBag.CONTRACT_TYPE_LIST = new SelectList(contractType.GetContractTypesCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "CONTRACT_TYPE_ID", "CONTRACT_TYPE_NAME");

            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            ViewBag.SUPERVISOR_LIST = new SelectList(employee.GetEmployeesByTypes(true,false,false, ThreadGlobals.UserAuthInfo.Value.CompanyId), "EMP_ID", "EMP_NAME");

            ViewBag.DefaultProfileImg = Convert.ToBase64String(GlobalTools.GetParamByteVal("NO_PICTURE_IMG", ThreadGlobals.UserAuthInfo.Value.CompanyId));

            string DefaultPhoneMask = GlobalTools.GetParamStrVal("PHONE_MASK", ThreadGlobals.UserAuthInfo.Value.CompanyId);
            ViewBag.DefaultPhoneMask = string.IsNullOrEmpty(DefaultPhoneMask) ? "(999) 999-9999" : DefaultPhoneMask;

            string DefaultMobileMask = GlobalTools.GetParamStrVal("MOBILE_MASK", ThreadGlobals.UserAuthInfo.Value.CompanyId);
            ViewBag.DefaultMobileMask = string.IsNullOrEmpty(DefaultMobileMask) ? "(999) 999-9999" : DefaultMobileMask;

            return View();
        }

        [HttpGet]
        public ActionResult GetEmployeeList()
        {            
            if (ThreadGlobals.UserAuthInfo.Value.UserId < 0)
            {
                return RedirectToAction("LockedUser", "Login");
            }

            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            return Json(new { Getlist = employee.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());

            if (ThreadGlobals.UserAuthInfo.Value.UserId < 0)
            {
                return RedirectToAction("LockedUser", "Login");
            }

            EMPLOYEES emp = new EMPLOYEES();
            emp.EMP_ID = ThreadGlobals.UserAuthInfo.Value.UserId;

            ViewBag.Employee = employee.Get(emp);
            ViewBag.DefaultProfileImg = GlobalTools.GetParamByteVal("NO_PICTURE_IMG", ThreadGlobals.UserAuthInfo.Value.CompanyId);

            string DefaultPhoneMask = GlobalTools.GetParamStrVal("PHONE_MASK", ThreadGlobals.UserAuthInfo.Value.CompanyId);
            ViewBag.DefaultPhoneMask = string.IsNullOrEmpty(DefaultPhoneMask) ? "(999) 999-9999" : DefaultPhoneMask;

            string DefaultMobileMask = GlobalTools.GetParamStrVal("MOBILE_MASK", ThreadGlobals.UserAuthInfo.Value.CompanyId);
            ViewBag.DefaultMobileMask = string.IsNullOrEmpty(DefaultMobileMask) ? "(999) 999-9999" : DefaultMobileMask;

            return View();
        }

        [HttpPost]
        public ActionResult UpdateEmployeeImg(int EmpID, HttpPostedFileBase Image, int Remove)
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());

            EMPLOYEES emp = new EMPLOYEES();
            emp.EMP_ID = EmpID;
            emp.EMP_IMG = Image != null ? new byte[Image.ContentLength] : null;

            var control = employee.UpdateEmployeeImage(emp);
            string message = control.isError ? control.ErrorMessage : (Image != null ? "Image Changed" : (Remove == 1 ? "Image Changed" : "No Image found"));

            return Json(new { success = !control.isError, message = message });
        }

        [HttpPost]
        public JsonResult UpdateProfileDetails(int id, string phone, string extension, string mobile)
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());

            EMPLOYEES emp = new EMPLOYEES();
            emp.EMP_ID = id;
            emp.EMP_PHONE = phone;
            emp.MOBILE_PHONE = mobile;

            var control = employee.UpdateProfileDetails(emp);            
            if(!control.isError)
                Session["UserInfo"] = employee;

            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "Information Info Changed" });
        }
    

        [HttpPost]
        public JsonResult ChangePassword(int id, string oldPwd, string newPwd, string confirmPwd)
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            var control = employee.ChangePassword(id, oldPwd, newPwd, confirmPwd);

            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateEmailSettings(int id, string email, string pwd, string signature, bool showSignature)
        {
            EMPLOYEES employees = new EMPLOYEES();
            employees.EMP_ID = id;
            employees.EMAIL_USERNAME = email;
            employees.EMAIL_PASSWORD = pwd;
            employees.SIGNATURE = signature;
            employees.SHOW_SIGNATURE = showSignature;


            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            var control = employee.UpdateEmailSettings(employees);

            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }

        [HttpPost]
        public JsonResult AddOrUpdateEmployee(EMPLOYEES param)
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            var control = employee.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int EmpID)
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());

            EMPLOYEES emp = new EMPLOYEES();
            emp.EMP_ID = EmpID;

            var control = employee.Delete(emp);
            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }


        [HttpPost]
        public JsonResult UpdateNavbarClass(int Type, string navbarClass)
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            var control = employee.UpdateNavbarClass(ThreadGlobals.UserAuthInfo.Value.UserId,Type,navbarClass);
            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }

        [HttpGet]        
        public bool CopyNavbarAll()
        {
            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            var control = employee.CopyNavbarAll(ThreadGlobals.UserAuthInfo.Value.UserId);
            return control.isError;
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
                EmployeeManager employee = new EmployeeManager(new EmployeeDal());
                var emplist = employee.GetAllActive().Select(s => new { EMAIL = s.EMAIL_USERNAME, NAME = s.EMP_NAME });

                return Json(new { Getlist = emplist, success = true }, JsonRequestBehavior.AllowGet);
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
