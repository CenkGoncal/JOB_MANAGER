//using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.Business.Ninject;
using JOB_MANAGER.DATAACESS.Helper;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.Models;

namespace JOB_MANAGER.Controllers
{
    public class ClientController : BaseController
    {
        #region Client
        public ActionResult Clients()
        {

            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();
            ViewBag.SUPERVISOR_LIST = new SelectList(employee.GetEmployeesByTypes(true, false, false, ThreadGlobals.UserAuthInfo.Value.CompanyId), "EMP_ID", "NAME");


            ICountyService county = InstanceFactory.GetInstance<ICountyService>();
            IStateService state = InstanceFactory.GetInstance<IStateService>();
            ICityService city = InstanceFactory.GetInstance<ICityService>();
            
            int defaultCountry = county.GetAll().Where(w => w.IS_DEFAULT == true).Select(s => s.COUNTRY_ID).FirstOrDefault();                
            if (defaultCountry > 0)
            {
                ViewBag.COUNTRIES_LIST = new SelectList(county.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(state.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(city.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "CITY_ID", "CITY_NAME");
            }
            else
            {
                ViewBag.COUNTRIES_LIST = new SelectList(county.GetAll().Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(state.GetAll().Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(city.GetAll().Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            }
            
            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            IContractTypeService companyType = InstanceFactory.GetInstance<IContractTypeService>();
            IRoleService role = InstanceFactory.GetInstance<IRoleService>();

            ViewBag.STATUS_LIST = new SelectList(status.GetAll().Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Client), "STATUS_ID", "STATUS_NAME");
            ViewBag.COMPANY_TYPE_LIST = new SelectList(companyType.GetAll().Where(w => w.IS_CANCELED == false), "COMPANY_TYPE_ID", "COMPANY_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId), "CLIENT_ID", "CLIENT_NAME");            
            ViewBag.ROLE_LIST = new SelectList(role.GetAll().Where(w => w.IS_CANCELED == false), "ROLE_ID", "ROLE_NAME");

            return View();
        }
        [HttpGet]
        public JsonResult GetClientList()
        {            
            IClientService client = InstanceFactory.GetInstance<IClientService>();
            return Json(new { Getlist = client.GetAll() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetListFilters()
        {
            //int CompanyID = GetCompanyID();
            IClientService client = InstanceFactory.GetInstance<IClientService>();

            var dataCity = client.GetAll().Where(w => w.IS_CANCELED == false && w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(s => new
            {
                CITY_ID = s.CLIENT_CITY,
                CITY_NAME = s.CITY_NAME
            });

            var dataState = client.GetAll().Where(w => w.IS_CANCELED == false && w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(s => new
            {
                STATE_ID = s.CLIENT_STATE,
                STATE_NAME = s.STATE_NAME
            });


            var dataCType = client.GetAll().Where(w => w.IS_CANCELED == false && w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(s => new
            {
                COMPANY_TYPE_ID = s.CLIENT_TYPE_ID,
                COMPANY_TYPE_NAME = s.COMPANY_TYPE_NAME
            });


            return Json(new { City = dataCity, State = dataState, CompanyType = dataCType }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateClient(CLIENTS param)
        {
            IClientService client = InstanceFactory.GetInstance<IClientService>();
            var control = client.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult DeleteClient(int ClientID)
        {
            IClientService client = InstanceFactory.GetInstance<IClientService>();
            var control = client.Delete(new CLIENTS() { CLIENT_ID = ClientID });

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        #endregion

        #region Contact
        public ActionResult Contacts()
        {
            IClientService client = InstanceFactory.GetInstance<IClientService>();
            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            IRoleService role = InstanceFactory.GetInstance<IRoleService>();

            int CompanyID = GetCompanyID();
            ViewBag.CLIENT_LIST = new SelectList(client.GetAll().Where(w=>w.IS_CANCELED == false && w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).ToList(), "CLIENT_ID", "CLIENT_NAME");
            ViewBag.STATUS_LIST = new SelectList(status.GetAllByType((int)StatusType.Client), "STATUS_ID", "STATUS_NAME");
            ViewBag.ROLE_LIST = new SelectList(role.GetAll().Where(w => w.IS_CANCELED == false ), "ROLE_ID", "ROLE_NAME");
            return View();
        }

        [HttpGet]
        public ActionResult ContactView(int id, int Layout)
        {

            ContactManager contactManager = new ContactManager(new ContactDal());

            ViewBag.Layout = Layout;

            return PartialView("ContactView", contactManager.GetAll().Where(w=>w.CONTACT_ID == id).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult GetListContactFilters()
        {
            int CompanyID = GetCompanyID();

            IClientService client = InstanceFactory.GetInstance<IClientService>();
            IContactService contact = InstanceFactory.GetInstance<IContactService>();

            var dataClient = client.GetAllByCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(s => new { s.CLIENT_ID, s.CLIENT_NAME }).OrderBy(o => o.CLIENT_NAME).Distinct().ToList(); ;

            var dataRole = contact.GetAllByCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(s => new { ROLE_ID = s.CONTACT_ROLE, ROLE_NAME = s.ROLE_NAME }).OrderBy(o => o.ROLE_NAME).Distinct().ToList(); ;

       
            return Json(new { Client = dataClient, Role = dataRole}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteContact(int ContactID)
        {            
            IContactService contactManager = InstanceFactory.GetInstance<IContactService>();
            var control = contactManager.Delete(new CONTACTS() { CONTACT_ID = ContactID });
            return Json(new { success = !control.isError, message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetContactList(int ClientID)
        {
            IContactService contactManager = InstanceFactory.GetInstance<IContactService>();

            var data = contactManager.GetAll().Where(w => w.CONTACT_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).ToList();

            if (ClientID > 0)
                data = (from D in data where D.CLIENT_ID == ClientID select D).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateContact(CONTACTS param)
        {
            IContactService contactManager = InstanceFactory.GetInstance<IContactService>();
            var control = contactManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult CopyContactData(int SourceClient)
        {
            IContactService contactManager = InstanceFactory.GetInstance<IContactService>();
            var ContactClients = contactManager.GetAll().Where(w=>w.CLIENT_ID != SourceClient).Distinct().ToList();

            return Json(new { Getlist = ContactClients }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool CopyContact(int ClientID,List<int> SourceContact)
        {
            try
            {
                IContactService contactManager = InstanceFactory.GetInstance<IContactService>();

                return contactManager.CopyContact(ClientID,SourceContact);
            }
            catch (Exception e)
            {

                return false;
            }
        }
        #endregion
    }
}
