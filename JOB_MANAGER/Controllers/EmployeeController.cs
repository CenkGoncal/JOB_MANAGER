using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.Business.Ninject;
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
            IDepartmentService department = InstanceFactory.GetInstance<IDepartmentService>();
            ViewBag.DEPARTMENT_LIST = new SelectList(department.GetDepartmentCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "DEPARTMENT_ID", "DEPARTMENT_NAME");

            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            ViewBag.EMPLOYEE_STATUS_LIST = new SelectList(status.GetAllByType((int)StatusType.Employee), "STATUS_ID", "STATUS_NAME");

            IRoleService role = InstanceFactory.GetInstance<IRoleService>();
            ViewBag.ROLE_LIST = new SelectList(role.GetRoleCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "ROLE_ID", "ROLE_NAME");

            ITitleService title = InstanceFactory.GetInstance<ITitleService>();
            ViewBag.TITLE_LIST = new SelectList(title.GetTitleCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "TITLE_ID", "TITLE_NAME");

            IContractTypeService contractType = InstanceFactory.GetInstance<IContractTypeService>();
            ViewBag.CONTRACT_TYPE_LIST = new SelectList(contractType.GetContractTypesCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId), "CONTRACT_TYPE_ID", "CONTRACT_TYPE_NAME");

            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
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
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();

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
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();

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
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();

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
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
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


            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
            var control = employee.UpdateEmailSettings(employees);

            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }

        [HttpPost]
        public JsonResult AddOrUpdateEmployee(EMPLOYEES param)
        {
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
            var control = employee.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int EmpID)
        {
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();

            EMPLOYEES emp = new EMPLOYEES();
            emp.EMP_ID = EmpID;

            var control = employee.Delete(emp);
            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }


        [HttpPost]
        public JsonResult UpdateNavbarClass(int Type, string navbarClass)
        {
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
            var control = employee.UpdateNavbarClass(ThreadGlobals.UserAuthInfo.Value.UserId,Type,navbarClass);
            return Json(new { success = !control.isError, message = control.isError ? control.ErrorMessage : "success" });
        }

        [HttpGet]        
        public bool CopyNavbarAll()
        {
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
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
                IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
                var emplist = employee.GetAllActive().Select(s => new { EMAIL = s.EMAIL_USERNAME, NAME = s.EMP_NAME });

                return Json(new { Getlist = emplist, success = true }, JsonRequestBehavior.AllowGet);
            }
            else if(TypeID == 2)
            {
                IContactService  contact = InstanceFactory.GetInstance<IContactService>();
                var query2 = contact.GetAll().Where(w => w.CONTACT_EMAIL != SqlConstants.stringEmpty).Select(s => new
                {
                    EMAIL = s.CONTACT_EMAIL,
                    NAME = s.CONTACT_FIRST_NAME + SqlConstants.stringWhiteSpace + s.CONTACT_LAST_NAME
                }).ToList();

                return Json(new { Getlist = query2.ToList(), success = true }, JsonRequestBehavior.AllowGet);
            }
            else if (TypeID == 3)
            {
                ISuplierService suplier = InstanceFactory.GetInstance<ISuplierService>();
                var query3 = suplier.GetAll().Where(w => w.SUPPLIER_EMAIL != SqlConstants.stringEmpty).
                             Select(s => new
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
