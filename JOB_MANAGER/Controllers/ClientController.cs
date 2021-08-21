//using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JOB_MANAGER.Business.Concrete;
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

            EmployeeManager employee = new EmployeeManager(new EmployeeDal());
            ViewBag.SUPERVISOR_LIST = new SelectList(employee.GetAll().Where(w => w.IS_SUPERVISOR == true && w.IS_CANCELED == false && w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(e => new
            {
                e.EMP_ID,
                NAME = e.EMP_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            CountyManager county = new CountyManager(new CountyDal());
            StateManager state = new StateManager(new StateDal());
            CityManager city = new CityManager(new CitiesDal());
            
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

            StatusManager status = new StatusManager(new StatusDal());
            CompanyTypeManager companyType = new CompanyTypeManager(new CompanyTypeDal());
            RoleManager role = new RoleManager(new RoleDal());
            
            ViewBag.STATUS_LIST = new SelectList(status.GetAll().Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Client), "STATUS_ID", "STATUS_NAME");
            ViewBag.COMPANY_TYPE_LIST = new SelectList(companyType.GetAll().Where(w => w.IS_CANCELED == false), "COMPANY_TYPE_ID", "COMPANY_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId), "CLIENT_ID", "CLIENT_NAME");            
            ViewBag.ROLE_LIST = new SelectList(role.GetAll().Where(w => w.IS_CANCELED == false), "ROLE_ID", "ROLE_NAME");

            return View();
        }
        [HttpGet]
        public JsonResult GetClientList()
        {
            ClientManager client = new ClientManager(new ClientDal());         
            return Json(new { Getlist = client.GetAll() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetListFilters()
        {
            //int CompanyID = GetCompanyID();
            ClientManager client = new ClientManager(new ClientDal());

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
            ClientManager client = new ClientManager(new ClientDal());
            var control = client.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult DeleteClient(int ClientID)
        {
            ClientManager client = new ClientManager(new ClientDal());
            var control = client.Delete(new CLIENTS() { CLIENT_ID = ClientID });

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        #endregion

        #region Contact
        public ActionResult Contacts()
        {
            ClientManager client = new ClientManager(new ClientDal());
            StatusManager status = new StatusManager(new StatusDal());
            RoleManager role = new RoleManager(new RoleDal());

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
            var dataClient = (from C in db.CLIENTS

                            where C.IS_CANCELED == false && C.COMPANY_ID == CompanyID
                            select new
                            {
                                CLIENT_ID = C.CLIENT_ID,
                                CLIENT_NAME = C.CLIENT_NAME
                            }
                    ).OrderBy(o => o.CLIENT_NAME).Distinct().ToList();

            var dataRole = (from C in db.CONTACTS

                             where C.IS_CANCELED == false && C.COMPANY_ID == CompanyID
                             select new
                             {
                                 ROLE_ID = C.CONTACT_ROLE,
                                 ROLE_NAME = C.ROLES.ROLE_NAME
                             }
                    ).OrderBy(o => o.ROLE_NAME).Distinct().ToList();
       
            return Json(new { Client = dataClient, Role = dataRole}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteContact(int ContactID)
        {
            ContactManager contactManager = new ContactManager(new ContactDal());
            var control = contactManager.Delete(new CONTACTS() { CONTACT_ID = ContactID });
            return Json(new { success = !control.isError, message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetContactList(int ClientID)
        {
            ContactManager contactManager = new ContactManager(new ContactDal());

            var data = contactManager.GetAll().Where(w => w.CONTACT_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).ToList();

            if (ClientID > 0)
                data = (from D in data where D.CLIENT_ID == ClientID select D).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateContact(CONTACTS param)
        {
            ContactManager contactManager = new ContactManager(new ContactDal());
            var control = contactManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult CopyContactData(int SourceClient)
        {            
            ContactManager contactManager = new ContactManager(new ContactDal());
            var ContactClients = contactManager.GetAll().Where(w=>w.CLIENT_ID != SourceClient).Distinct().ToList();

            return Json(new { Getlist = ContactClients }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool CopyContact(int ClientID,List<int> SourceContact)
        {
            try
            {
                ContactManager contactManager = new ContactManager(new ContactDal());

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
