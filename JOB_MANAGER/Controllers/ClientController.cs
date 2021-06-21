using JOB_MANAGER.Helper;
//using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JOB_MANAGER.Models;

namespace JOB_MANAGER.Controllers
{
    public class ClientController : BaseController
    {
        #region Client
        public ActionResult Clients()
        {

            int CompanyID = GetCompanyID();
            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            int defaultCountry = db.COUNTRIES.Where(w => w.IS_DEFAULT == true).Select(s => s.COUNTRY_ID).FirstOrDefault();
            if (defaultCountry > 0)
            {
                ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(db.STATES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(db.CITIES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "CITY_ID", "CITY_NAME");
            }
            else
            {
                ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(db.STATES.Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(db.CITIES.Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            }

            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Client), "STATUS_ID", "STATUS_NAME");
            ViewBag.COMPANY_TYPE_LIST = new SelectList(db.COMPANY_TYPES.Where(w => w.IS_CANCELED == false), "COMPANY_TYPE_ID", "COMPANY_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == CompanyID), "CLIENT_ID", "CLIENT_NAME");            
            ViewBag.ROLE_LIST = new SelectList(db.ROLES.Where(w => w.IS_CANCELED == false), "ROLE_ID", "ROLE_NAME");

            return View();
        }
        [HttpGet]
        public JsonResult GetClientList()
        {
            var CompanyID = GetCompanyID();

            var data = (from v in db.CLIENTS

                        where v.IS_CANCELED == false && v.COMPANY_ID == CompanyID
                        select new
                        {
                            CLIENT_ID = v.CLIENT_ID,
                            CLIENT_NAME = v.CLIENT_NAME,
                            CLIENT_SHORT_NAME = v.CLIENT_SHORT_NAME,
                            CLIENT_TYPE_ID = v.CLIENT_TYPE_ID,
                            COMPANY_TYPE_NAME = v.COMPANY_TYPES.COMPANY_TYPE_NAME,
                            CLIENT_STATUS_ID = v.CLIENT_STATUS_ID,
                            STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            CLIENT_PHONE = v.CLIENT_PHONE,
                            CLIENT_URL = v.CLIENT_URL,
                            CLIENT_LINKEDIN = v.CLIENT_LINKEDIN,
                            CLIENT_CODE = v.CLIENT_CODE,
                            CLIENT_COUNTRY = v.CLIENT_COUNTRY,
                            COUNTRY_NAME = v.COUNTRIES.COUNTRY_NAME,
                            CLIENT_STATE = v.CLIENT_STATE,
                            STATE_NAME = v.STATES.STATE_NAME,
                            CLIENT_CITY = v.CLIENT_CITY,
                            CITY_NAME = v.CITIES.CITY_NAME,
                            CLIENT_ADDRESS = v.CLIENT_ADDRESS,
                            POSTAL_CODE = v.POSTAL_CODE,
                            CLIENT_SINCE = v.CREATION_DATE != null ?
                                                    v.CLIENT_SINCE.Value.Year + SqlConstants.stringMinus +
                                                    (v.CLIENT_SINCE.Value.Month > 9 ? v.CLIENT_SINCE.Value.Month + SqlConstants.stringMinus : "0" + v.CLIENT_SINCE.Value.Month + SqlConstants.stringMinus) +
                                                    v.CLIENT_SINCE.Value.Day : null,
                            CREATION_DATE = v.CREATION_DATE != null ?
                                                    v.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (v.CREATION_DATE.Month > 9 ? v.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + v.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    v.CREATION_DATE.Day : null,
                            MODIFIED_DATE = v.MODIFIED_DATE != null ?
                                                    v.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (v.MODIFIED_DATE.Month > 9 ? v.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + v.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    v.MODIFIED_DATE.Day : null,

                        }
                       ).OrderBy(o => o.CLIENT_NAME).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetListFilters()
        {
            int CompanyID = GetCompanyID();
            var dataCity = (from C in db.CLIENTS

                            where C.IS_CANCELED == false && C.COMPANY_ID == CompanyID
                            select new
                            {
                                CITY_ID = C.CLIENT_CITY,
                                CITY_NAME = C.CITIES.CITY_NAME
                            }
                    ).OrderBy(o => o.CITY_NAME).Distinct().ToList();

            var dataState = (from C in db.CLIENTS

                           where C.IS_CANCELED == false && C.COMPANY_ID == CompanyID
                           select new
                           {
                               STATE_ID = C.CLIENT_STATE,
                               STATE_NAME = C.STATES.STATE_NAME
                           }
                    ).OrderBy(o => o.STATE_NAME).Distinct().ToList();

            var dataCType = (from C in db.CLIENTS

                             where C.IS_CANCELED == false && C.COMPANY_ID == CompanyID
                             select new
                             {
                                 COMPANY_TYPE_ID = C.CLIENT_TYPE_ID,
                                 COMPANY_TYPE_NAME = C.COMPANY_TYPES.COMPANY_TYPE_NAME
                             }
                        ).OrderBy(o => o.COMPANY_TYPE_NAME).Distinct().ToList();


            return Json(new { City = dataCity, State = dataState, CompanyType = dataCType }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateClient(CLIENTS param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                CLIENTS client = db.CLIENTS.Where(w => w.CLIENT_ID == param.CLIENT_ID).FirstOrDefault();

                bool isNew = false;
                if (client == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();

                    db.CLIENTS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    client.MODIFIED_DATE = DateTime.Now;
                    client.CLIENT_NAME = param.CLIENT_NAME;
                    client.CLIENT_SHORT_NAME = param.CLIENT_SHORT_NAME;
                    client.CLIENT_COUNTRY = param.CLIENT_COUNTRY;
                    client.CLIENT_STATE = param.CLIENT_STATE;
                    client.CLIENT_CITY = param.CLIENT_CITY;
                    client.CLIENT_PHONE = param.CLIENT_PHONE;
                    client.CLIENT_URL = param.CLIENT_URL;
                    client.POSTAL_CODE = param.POSTAL_CODE;
                    client.CLIENT_LINKEDIN = param.CLIENT_LINKEDIN;
                    client.CLIENT_CODE = param.CLIENT_CODE;
                    client.CLIENT_ADDRESS = param.CLIENT_ADDRESS;                    
                    client.CLIENT_SINCE = param.CLIENT_SINCE;
                    client.CLIENT_TYPE_ID = param.CLIENT_TYPE_ID;
                    client.CLIENT_STATUS_ID = param.CLIENT_STATUS_ID;
                    client.UPDATED_BY = GetUserID();

                    db.CLIENTS.Attach(client);
                    db.Entry(client).Property(x => x.CLIENT_NAME).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_SHORT_NAME).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_COUNTRY).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_STATE).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_CITY).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_PHONE).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_URL).IsModified = true;
                    db.Entry(client).Property(x => x.POSTAL_CODE).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_LINKEDIN).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_CODE).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_ADDRESS).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_SINCE).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_TYPE_ID).IsModified = true;
                    db.Entry(client).Property(x => x.CLIENT_STATUS_ID).IsModified = true;
                    db.Entry(client).Property(x => x.UPDATED_BY).IsModified = true;
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
        public JsonResult DeleteClient(int ClientID)
        {
            CLIENTS anClient = db.CLIENTS.Find(ClientID);
            if (anClient != null)
            {
                try
                {
                    anClient.IS_CANCELED = true;
                    anClient.UPDATED_BY = GetUserID();
                    db.CLIENTS.Attach(anClient);

                    db.Entry(anClient).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anClient).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Client not found" });
        }

        #endregion

        #region Contact
        public ActionResult Contacts()
        {
            int CompanyID = GetCompanyID();
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == CompanyID), "CLIENT_ID", "CLIENT_NAME");
            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Client), "STATUS_ID", "STATUS_NAME");
            ViewBag.ROLE_LIST = new SelectList(db.ROLES.Where(w => w.IS_CANCELED == false ), "ROLE_ID", "ROLE_NAME");
            return View();
        }

        [HttpGet]
        public ActionResult ContactView(int id, int Layout)
        {
            var data = (from C in db.CONTACTS
                        join CL in db.CLIENTS on C.CLIENT_ID equals CL.CLIENT_ID
                        join S in db.STATUS on C.CONTACT_STATUS equals S.STATUS_ID
                        where C.IS_CANCELED == false && C.CONTACT_ID == id
                        select new ContactViewDto
                        {
                            CONTACT_ID = C.CONTACT_ID,
                            CONTACT_FIRST_NAME = C.CONTACT_FIRST_NAME,
                            CONTACT_LAST_NAME = C.CONTACT_LAST_NAME,
                            CONTACT_NAME = C.CONTACT_FIRST_NAME + SqlConstants.stringWhiteSpace + C.CONTACT_LAST_NAME,
                            CLIENT_ID = C.CLIENT_ID,
                            CLIENT_NAME = CL.CLIENT_NAME,
                            CONTACT_TITLE = C.CONTACT_TITLE,
                            CONTACT_STATUS = C.CONTACT_STATUS,
                            STATUS_NAME = S.STATUS_NAME,
                            DISPLAY_CLASS = S.DISPLAY_CLASS,
                            CONTACT_ROLE = C.CONTACT_ROLE,
                            ROLE_NAME = C.ROLES.ROLE_NAME,
                            CONTACT_PHONE = C.CONTACT_PHONE,
                            CONTACT_PHONE_EX = C.CONTACT_PHONE_EX,
                            CONTACT_PHONE_ALL = C.CONTACT_PHONE + "/" + C.CONTACT_PHONE_EX,
                            CONTACT_MOBILE = C.CONTACT_MOBILE,
                            IS_PRIMARY = C.IS_PRIMARY,
                            CONTACT_EMAIL = C.CONTACT_EMAIL,
                            CREATION_DATE = C.CREATION_DATE != null ?
                                                    C.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (C.CREATION_DATE.Month > 9 ? C.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + C.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    C.CREATION_DATE.Day : null,
                            MODIFIED_DATE = C.MODIFIED_DATE != null ?
                                                    C.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (C.MODIFIED_DATE.Month > 9 ? C.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + C.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    C.MODIFIED_DATE.Day : null,

                        }
                     ).FirstOrDefault();

            ViewBag.Layout = Layout;

            return PartialView("ContactView", data);
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
            CONTACTS anContact = db.CONTACTS.Find(ContactID);
            if (anContact != null)
            {
                try
                {
                    anContact.IS_CANCELED = true;
                    anContact.UPDATED_BY = GetUserID();
                    db.CONTACTS.Attach(anContact);

                    db.Entry(anContact).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anContact).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Contact not found" });
        }

        [HttpGet]
        public JsonResult GetContactList(int ClientID)
        {
            var CompanyID = GetCompanyID();

            var data = (from v in db.CONTACTS
                        join CL in db.CLIENTS on v.CLIENT_ID equals CL.CLIENT_ID
                        where v.IS_CANCELED == false && v.COMPANY_ID == CompanyID
                        select new
                        {
                            CONTACT_ID = v.CONTACT_ID,
                            CONTACT_FIRST_NAME =v.CONTACT_FIRST_NAME,
                            CONTACT_LAST_NAME = v.CONTACT_LAST_NAME,
                            CONTACT_NAME = v.CONTACT_FIRST_NAME+SqlConstants.stringWhiteSpace+v.CONTACT_LAST_NAME,
                            CLIENT_ID = v.CLIENT_ID,
                            CLIENT_NAME = CL.CLIENT_NAME,
                            CONTACT_TITLE = v.CONTACT_TITLE,
                            CONTACT_STATUS = v.CONTACT_STATUS,
                            STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            CONTACT_ROLE  = v.CONTACT_ROLE,
                            ROLE_NAME = v.ROLES.ROLE_NAME,
                            CONTACT_PHONE = v.CONTACT_PHONE,
                            CONTACT_PHONE_EX = v.CONTACT_PHONE_EX,
                            CONTACT_PHONE_ALL = v.CONTACT_PHONE+"/"+ v.CONTACT_PHONE_EX,
                            CONTACT_MOBILE = v.CONTACT_MOBILE,
                            IS_PRIMARY = v.IS_PRIMARY,
                            CONTACT_EMAIL = v.CONTACT_EMAIL,
                            CREATION_DATE = v.CREATION_DATE != null ?
                                                    v.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (v.CREATION_DATE.Month > 9 ? v.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + v.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    v.CREATION_DATE.Day : null,
                            MODIFIED_DATE = v.MODIFIED_DATE != null ?
                                                    v.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (v.MODIFIED_DATE.Month > 9 ? v.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + v.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    v.MODIFIED_DATE.Day : null,

                        }
                       ).OrderBy(o => o.CONTACT_NAME).ToList();

            if (ClientID > 0)
                data = (from D in data where D.CLIENT_ID == ClientID select D).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateContact(CONTACTS param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                CONTACTS control = db.CONTACTS.Where(w => w.CONTACT_ID == param.CONTACT_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();

                    db.CONTACTS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.MODIFIED_DATE = DateTime.Now;
                    control.CONTACT_FIRST_NAME = param.CONTACT_FIRST_NAME;
                    control.CONTACT_LAST_NAME = param.CONTACT_LAST_NAME;
                    control.CLIENT_ID = param.CLIENT_ID;
                    control.CONTACT_TITLE = param.CONTACT_TITLE;
                    control.CONTACT_STATUS = param.CONTACT_STATUS;
                    control.CONTACT_ROLE = param.CONTACT_ROLE;
                    control.CONTACT_PHONE_EX = param.CONTACT_PHONE_EX;
                    control.CONTACT_PHONE = param.CONTACT_PHONE;
                    control.CONTACT_MOBILE = param.CONTACT_MOBILE;
                    control.IS_PRIMARY = param.IS_PRIMARY;
                    control.CONTACT_EMAIL = param.CONTACT_EMAIL;
                    control.UPDATED_BY = GetUserID();

                    db.CONTACTS.Attach(control);
                    db.Entry(control).Property(x => x.CONTACT_FIRST_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_LAST_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.CLIENT_ID).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_TITLE).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_STATUS).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_ROLE).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_PHONE_EX).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_PHONE).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_MOBILE).IsModified = true;
                    db.Entry(control).Property(x => x.IS_PRIMARY).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_EMAIL).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
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
        public JsonResult CopyContactData(int SourceClient)
        {            
               var ContactClients = (from C in db.CONTACTS
                                      join CL in db.CLIENTS on C.CLIENT_ID equals CL.CLIENT_ID
                                      where C.CLIENT_ID != SourceClient && C.IS_CANCELED == false
                                      select new
                                      {
                                          CLIENT_ID = C.CLIENT_ID,
                                          CLIENT_NAME = CL.CLIENT_NAME,
                                          CONTACT_ID = C.CONTACT_ID,
                                          CONTACT_NAME = C.CONTACT_FIRST_NAME + SqlConstants.stringWhiteSpace + C.CONTACT_LAST_NAME,
                                          CONTACT_EMAIL = C.CONTACT_EMAIL,
                                          CONTACT_MOBILE = C.CONTACT_MOBILE,
                                          STATUS_NAME = C.STATUS.STATUS_NAME,
                                          DISPLAY_CLASS =C.STATUS.DISPLAY_CLASS
                                      }).Distinct().ToList();

            return Json(new { Getlist = ContactClients }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool CopyContact(int ClientID,List<int> SourceContact)
        {
            try
            {
                foreach (int ContactID in SourceContact)
                {
                    var contact = db.CONTACTS.Where(w => w.CONTACT_ID == ContactID && w.CLIENT_ID == ClientID).FirstOrDefault();
                    if (contact != null )
                    {                        
                        CONTACTS insert = new CONTACTS();

                        insert.MODIFIED_DATE = insert.CREATION_DATE = DateTime.Now;
                        insert.CREATED_BY = insert.UPDATED_BY = GetUserID();
                        insert.COMPANY_ID = GetCompanyID();
                        insert.CONTACT_FIRST_NAME = contact.CONTACT_FIRST_NAME;
                        insert.CONTACT_LAST_NAME = contact.CONTACT_LAST_NAME;
                        insert.CLIENT_ID = ClientID;
                        insert.CONTACT_TITLE = contact.CONTACT_TITLE;
                        insert.CONTACT_STATUS = contact.CONTACT_STATUS;
                        insert.CONTACT_ROLE = contact.CONTACT_ROLE;
                        insert.CONTACT_PHONE_EX = contact.CONTACT_PHONE_EX;
                        insert.CONTACT_PHONE = contact.CONTACT_PHONE;
                        insert.CONTACT_MOBILE = contact.CONTACT_MOBILE;
                        insert.IS_PRIMARY = contact.IS_PRIMARY;
                        insert.CONTACT_EMAIL = contact.CONTACT_EMAIL;
                        insert.UPDATED_BY = GetUserID();

                        db.CONTACTS.Add(insert);
                        db.SaveChanges();

                    }
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
        #endregion
    }
}