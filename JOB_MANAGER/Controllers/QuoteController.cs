using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Login;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.Controllers
{
    public class QuoteController : BaseController
    {
        public ActionResult Quotes()
        {
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

            ViewBag.STREET_LIST = new SelectList(db.STREET.Where(w => w.IS_CANCELED == false), "STREET_ID", "STREET_NAME");
            ViewBag.PROJECT_TYPE_LIST = new SelectList(db.PROJECT_TYPES.Where(w => w.IS_CANCELED == false), "PROJECT_TYPE_ID", "PROJECT_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(w => w.IS_CANCELED == false), "CLIENT_ID", "CLIENT_NAME");
            ViewBag.MATERIAL_LIST = new SelectList(db.MATERIALS.Where(w => w.IS_CANCELED == false), "MATERIAL_ID", "MATERIAL_NAME");            
            ViewBag.FLOOR_LIST = new SelectList(db.FLOOR_TYPES.Where(w => w.IS_CANCELED == false ), "FLOOR_TYPE_ID", "FLOOR_TYPE_NAME");

            var data = (from s in db.STATUS

                        join e in db.EMPLOYEES
                        on s.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()
                        where s.STATUS_TYPE == (int)StatusType.Project && s.IS_CANCELED == false
                        select new StatusDto
                        {
                            STATUS_ID = s.STATUS_ID,
                            STATUS_NAME = s.STATUS_NAME,
                            DISPLAY_CLASS = s.DISPLAY_CLASS,
                        }
                 ).OrderBy(o => o.STATUS_NAME).ToList();
            ViewBag.STATUS_LIST = data;

            return View();
        }

        [HttpGet]
        public ActionResult QuoteView(int id, int Layout)
        {
            var CompanyID = GetCompanyID();
            var data = GlobalTools.GetQuontesLookUp(id, CompanyID);

            ViewBag.Layout = Layout;
            return PartialView("QuoteView", data[0]);
        }


        [HttpGet]
        public JsonResult GetQuoteList(int QuanteID)
        {
            var CompanyID = GetCompanyID();

            var data = GlobalTools.GetQuontesLookUp(QuanteID, CompanyID);

            if (QuanteID > 0)
            {              
                return Json(new { Template = data }, JsonRequestBehavior.AllowGet);
            }
            
            data = data.Where(w => w.USE_AS_TEMPLATE == false).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQuonteAdvacedList(QuoteAdvancedSearch param)
        {
            var CompanyID = GetCompanyID();
            var data = (from Q in db.QUOTES
                        join C in db.CLIENTS on Q.CLIENT_ID equals C.CLIENT_ID
                        join PT in db.PROJECT_TYPES on Q.PROJECT_TYPE_ID equals PT.PROJECT_TYPE_ID
                        join S in db.STATUS on Q.QUOTE_STATUS equals S.STATUS_ID
                        join CT in db.CITIES on Q.QUOTE_CITY equals CT.CITY_ID
                        join ST in db.STATES on Q.QUOTE_STATE equals ST.STATE_ID
                        join CN in db.COUNTRIES on Q.QUOTE_COUNTRY equals CN.COUNTRY_ID
                        join F in db.FLOOR_TYPES on Q.FLOOR_TYPE equals F.FLOOR_TYPE_ID
                        join M in db.MATERIALS on Q.MATERIAL_ID equals M.MATERIAL_ID
                        join STT in db.STREET on Q.STREET_SUFFIX_ID equals STT.STREET_ID
                        where Q.IS_CANCELED == false && Q.COMPANY_ID == CompanyID && Q.USE_AS_TEMPLATE == false
                        select new
                        {
                            QUOTE_ID = Q.QUOTE_ID,
                            QUOTE_NUMBER = Q.QUOTE_NUMBER,
                            CLIENT_ID = Q.CLIENT_ID,
                            CLIENT_NAME = C.CLIENT_NAME,
                            PROJECT_TYPE_ID = Q.PROJECT_TYPE_ID,
                            PROJECT_TYPE_NAME = PT.PROJECT_TYPE_NAME,
                            QUOTE_STATUS = Q.QUOTE_STATUS,
                            QUOTE_NAME = Q.QUOTE_NAME,
                            DISPLAY_CLASS = S.DISPLAY_CLASS,
                            STATUS_NAME = S.STATUS_NAME,
                            ADR_LOT_NBR = Q.ADR_LOT_NBR,
                            ADR_UNIT_NBR = Q.ADR_UNIT_NBR,
                            ADR_STREET_NBR = Q.ADR_STREET_NBR,
                            ADR_STREET_NAME = Q.ADR_STREET_NAME,
                            QUOTE_ADDRESS = Q.QUOTE_ADDRESS,
                            QUOTE_COUNTRY = Q.QUOTE_COUNTRY,
                            COUNTRY_NAME = CN.COUNTRY_NAME,
                            QUOTE_STATE = Q.QUOTE_STATE,
                            STATE_NAME = ST.STATE_NAME,
                            QUOTE_CITY = Q.QUOTE_CITY,
                            CITY_NAME = CT.CITY_NAME,
                            QUOTE_POSTAL_CODE = Q.QUOTE_POSTAL_CODE,
                            QUOTE_AMOUNT = Q.QUOTE_AMOUNT,
                            CABINETS_NBR = Q.CABINETS_NBR,
                            QUOTE_DESC = Q.QUOTE_DESC,
                            QUOTE_START_DATE_DT = Q.QUOTE_START_DATE,
                            QUOTE_START_DATE = Q.QUOTE_START_DATE != null ?
                                                    Q.QUOTE_START_DATE.Year + SqlConstants.stringMinus +
                                                    (Q.QUOTE_START_DATE.Month > 9 ? Q.QUOTE_START_DATE.Month + SqlConstants.stringMinus : "0" + Q.QUOTE_START_DATE.Month + SqlConstants.stringMinus) +
                                                    Q.QUOTE_START_DATE.Day : null,
                            QUOTE_EXPIRY_DATE_DT = Q.QUOTE_EXPIRY_DATE,
                            QUOTE_EXPIRY_DATE = Q.QUOTE_EXPIRY_DATE != null ?
                                                    Q.QUOTE_EXPIRY_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (Q.QUOTE_EXPIRY_DATE.Value.Month > 9 ? Q.QUOTE_EXPIRY_DATE.Value.Month + SqlConstants.stringMinus : "0" + Q.QUOTE_EXPIRY_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    Q.QUOTE_EXPIRY_DATE.Value.Day : null,
                            DISPLAY = Q.DISPLAY,
                            FLOOR_TYPE = Q.FLOOR_TYPE,
                            FLOOR_TYPE_NAME = F.FLOOR_TYPE_NAME,
                            MATERIAL_ID = Q.MATERIAL_ID,
                            MATERIAL_NAME = M.MATERIAL_NAME,
                            STREET_SUFFIX_ID = Q.STREET_SUFFIX_ID,
                            STREET_NAME = STT.STREET_NAME,
                            CONTACT_DESC = Q.CONTACT_DESC,
                            USE_AS_TEMPLATE = Q.USE_AS_TEMPLATE,
                            CREATION_DATE = Q.CREATION_DATE != null ?
                                                    Q.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (Q.CREATION_DATE.Month > 9 ? Q.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + Q.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    Q.CREATION_DATE.Day : null,
                            MODIFIED_DATE = Q.MODIFIED_DATE != null ?
                                                    Q.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (Q.MODIFIED_DATE.Month > 9 ? Q.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + Q.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    Q.MODIFIED_DATE.Day : null,

                        }
                       ).OrderBy(o => o.CLIENT_NAME).ToList();

            if(string.IsNullOrEmpty(param.LotNumber) == false)
            {
                data = data.Where(w => w.ADR_LOT_NBR == param.LotNumber).ToList();
            }

            if(param.Material > 0)
            {
                data = data.Where(w => w.MATERIAL_ID == param.Material).ToList();
            }

            if (param.Floor > 0)
            {
                data = data.Where(w => w.FLOOR_TYPE == param.Floor).ToList();
            }

            if (param.FromInit > DateTime.MinValue)
            {
                data = data.Where(w => w.QUOTE_START_DATE_DT >= param.FromInit).ToList();
            }

            if (param.EndInit > DateTime.MinValue)
            {
                data = data.Where(w => w.QUOTE_START_DATE_DT <= param.FromInit).ToList();
            }
            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        #region Note
        [HttpGet]
        public JsonResult GetQuoteNoteList(int QuanteID)
        {

            var data = (from N in db.NOTES
                        join e in db.EMPLOYEES
                        on N.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        where N.UNIQ_ID == QuanteID && N.IS_CANCELED == false && N.NOTE_KIND_ID == (int)NoteType.Quote
                        select new
                        {
                            UNIQ_ID = N.UNIQ_ID,
                            NOTE_DESC = N.NOTE_DESC,
                            NOTE_SUBJECT = N.NOTE_SUBJECT,
                            NOTE_KIND_ID = N.NOTE_KIND_ID,
                            NOTE_TYPE_ID = N.NOTE_TYPE_ID,
                            CREATION_DATE = N.CREATION_DATE != null ?
                                                    N.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (N.CREATION_DATE.Month > 9 ? N.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + N.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    N.CREATION_DATE.Day : null,
                            MODIFIED_DATE = N.MODIFIED_DATE != null ?
                                                    N.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (N.MODIFIED_DATE.Month > 9 ? N.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + N.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    N.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                        ).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateNote(NotesExtented param)
        {
            bool _success = false;
            string _message = string.Empty;            
            try
            {
                NOTES control = db.NOTES.Where(w => w.NOTE_ID == param.NOTE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    NOTES insert = new NOTES();
                    insert.UNIQ_ID = param.UNIQ_ID;
                    insert.NOTE_TYPE_ID = param.NOTE_TYPE_ID;
                    insert.NOTE_SUBJECT = param.NOTE_SUBJECT;
                    insert.NOTE_DESC = param.NOTE_DESC;
                    insert.NOTE_KIND_ID = param.NOTE_KIND_ID;
                    insert.PHASE_ID = param.PHASE_ID;
                    insert.TASK_ID = param.TASK_ID;
                    insert.SET_REMINDER = param.SET_REMINDER;
                    //if(param.ASSINGED_MEMBERS_ARR.Count > 0) 
                    insert.ASSINGED_MEMBERS = param.ASSINGED_MEMBERS_ARR == null ? string.Empty : JsonConvert.SerializeObject(param.ASSINGED_MEMBERS_ARR);                    
                    insert.TO_REMINDER = param.TO_REMIND_ARR == null ? string.Empty :  JsonConvert.SerializeObject(param.TO_REMIND_ARR);
                    insert.REMINDER_DATE = param.REMINDER_DATE;
                    insert.NOTE_TYPE_ID = param.NOTE_TYPE_ID;
                    insert.IS_CANCELED = param.IS_CANCELED;
                    insert.CREATED_BY = param.UPDATED_BY = GetUserID();
                    insert.MODIFIED_DATE = insert.CREATION_DATE = DateTime.Now;

                    db.NOTES.Add(insert);
                    db.SaveChanges();
                }
                else
                {
                    control.NOTE_TYPE_ID = param.NOTE_TYPE_ID;
                    control.NOTE_SUBJECT = param.NOTE_SUBJECT;
                    control.NOTE_DESC = param.NOTE_DESC;
                    control.NOTE_KIND_ID = param.NOTE_KIND_ID;
                    control.PHASE_ID = param.PHASE_ID;
                    control.TASK_ID = param.TASK_ID;
                    control.SET_REMINDER = param.SET_REMINDER;
                    control.ASSINGED_MEMBERS = JsonConvert.SerializeObject(param.ASSINGED_MEMBERS_ARR);
                    control.TO_REMINDER = JsonConvert.SerializeObject(param.TO_REMIND_ARR);
                    control.REMINDER_DATE = param.REMINDER_DATE;
                    control.NOTE_TYPE_ID = param.NOTE_TYPE_ID;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();                    

                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.NOTES.Attach(control);
                    db.Entry(control).Property(x => x.NOTE_TYPE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.NOTE_SUBJECT).IsModified = true;
                    db.Entry(control).Property(x => x.NOTE_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.NOTE_KIND_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PHASE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.TASK_ID).IsModified = true;
                    db.Entry(control).Property(x => x.SET_REMINDER).IsModified = true;
                    db.Entry(control).Property(x => x.REMINDER_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.ASSINGED_MEMBERS).IsModified = true;
                    db.Entry(control).Property(x => x.TO_REMINDER).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.NOTE_TYPE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                    
                }

                _success = true;
                _message = "Operation Successful";

                return Json(new { success = _success });
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult RemoveNote(NOTES param)
        {
            NOTES anNote = db.NOTES.Find(param.NOTE_ID);
            if (anNote != null)
            {
                try
                {
                    anNote.IS_CANCELED = true;
                    anNote.UPDATED_BY = GetUserID();
                    db.NOTES.Attach(anNote);

                    db.Entry(anNote).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anNote).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Note not found" });
        }
        #endregion
        [HttpGet]
        public JsonResult GetListFilters()
        {
            int CompanyID = GetCompanyID();
            var dataCity = (from C in db.CITIES
                            join Q in db.QUOTES on C.CITY_ID equals Q.QUOTE_CITY
                            where Q.IS_CANCELED == false && Q.COMPANY_ID == CompanyID
                            select new
                            {
                                CITY_ID = Q.QUOTE_CITY,
                                CITY_NAME = C.CITY_NAME
                            }
                    ).OrderBy(o => o.CITY_NAME).Distinct().ToList();

            var dataClient = (from C in db.CLIENTS
                              join Q in db.QUOTES on C.CLIENT_ID equals Q.CLIENT_ID
                             where Q.IS_CANCELED == false && Q.COMPANY_ID == CompanyID
                             select new
                             {
                                 CLIENT_ID = C.CLIENT_ID,
                                 CLIENT_NAME = C.CLIENT_NAME
                             }
                    ).OrderBy(o => o.CLIENT_NAME).Distinct().ToList();

            var dataPType = (from Q in db.QUOTES
                             join P in db.PROJECT_TYPES on Q.PROJECT_TYPE_ID equals P.PROJECT_TYPE_ID
                             where Q.IS_CANCELED == false && Q.COMPANY_ID == CompanyID
                             select new
                             {
                                 PROJECT_TYPE_ID = P.PROJECT_TYPE_ID,
                                 PROJECT_TYPE_NAME = P.PROJECT_TYPE_NAME
                             }
                        ).OrderBy(o => o.PROJECT_TYPE_NAME).Distinct().ToList();


            var dataStatus = (from Q in db.QUOTES
                             join S in db.STATUS on Q.QUOTE_STATUS equals S.STATUS_ID
                             where Q.IS_CANCELED == false && Q.COMPANY_ID == CompanyID
                             select new
                             {
                                 STATUS_ID = S.STATUS_ID,
                                 STATUS_NAME = S.STATUS_NAME
                             }
            ).OrderBy(o => o.STATUS_NAME).Distinct().ToList();


            return Json(new { City = dataCity, Client = dataClient, Project = dataPType, Status = dataStatus }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllTemplate()
        {
            var CompanyID = GetCompanyID();

            var data = (from Q in db.QUOTES
                        where Q.USE_AS_TEMPLATE == true
                        select new
                        {
                            TEMPLATE_ID = Q.QUOTE_ID,
                            TEMPLATE_NAME = Q.TEMPLATE_NAME
                        }
                        ).ToList();

            return Json(new { Template = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateQuonte(QUOTES param)
        {
            bool _success = false;
            string _message = string.Empty;
            int _ID = param.QUOTE_ID;
            try
            {
                QUOTES control = new QUOTES();
                if(param.USE_AS_TEMPLATE)
                    control = db.QUOTES.Where(w => w.TEMPLATE_NAME == param.TEMPLATE_NAME).FirstOrDefault();
                else
                    control = db.QUOTES.Where(w => w.QUOTE_ID == param.QUOTE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                }

                if (isNew)
                {

                    int lastQuanteID = db.QUOTES.Where(w => w.IS_CANCELED == false).OrderByDescending(o => o.QUOTE_ID).Select(s => s.QUOTE_ID).FirstOrDefault();

                    param.QUOTE_NUMBER = string.Format("QUO-{0}", lastQuanteID + 1);
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();

                    db.QUOTES.Add(param);
                    db.SaveChanges();

                    _ID = param.QUOTE_ID;
                }
                else
                {
                    control.MODIFIED_DATE = DateTime.Now;
                    control.QUOTE_NUMBER = param.QUOTE_NUMBER;
                    control.CLIENT_ID = param.CLIENT_ID;
                    control.PROJECT_TYPE_ID = param.PROJECT_TYPE_ID;
                    control.QUOTE_NAME = param.QUOTE_NAME;
                    control.QUOTE_STATUS = param.QUOTE_STATUS;
                    control.ADR_LOT_NBR = param.ADR_LOT_NBR;
                    control.ADR_STREET_NBR = param.ADR_STREET_NBR;
                    control.ADR_STREET_NAME = param.ADR_STREET_NAME;
                    control.ADR_UNIT_NBR = param.ADR_UNIT_NBR;
                    control.QUOTE_ADDRESS = param.QUOTE_ADDRESS;
                    control.QUOTE_CITY = param.QUOTE_CITY;
                    control.QUOTE_STATE = param.QUOTE_STATE;
                    control.QUOTE_COUNTRY = param.QUOTE_COUNTRY;
                    control.QUOTE_POSTAL_CODE = param.QUOTE_POSTAL_CODE;
                    control.QUOTE_AMOUNT = param.QUOTE_AMOUNT;
                    control.CABINETS_NBR = param.CABINETS_NBR;
                    control.QUOTE_DESC = param.QUOTE_DESC;
                    control.QUOTE_START_DATE = param.QUOTE_START_DATE;
                    control.QUOTE_EXPIRY_DATE = param.QUOTE_EXPIRY_DATE;
                    control.FLOOR_TYPE = param.FLOOR_TYPE;
                    control.MATERIAL_ID = param.MATERIAL_ID;
                    control.STREET_SUFFIX_ID = param.STREET_SUFFIX_ID;
                    control.CONTACT_DESC = param.CONTACT_DESC;
                    control.USE_AS_TEMPLATE = param.USE_AS_TEMPLATE;
                    control.TEMPLATE_NAME = param.TEMPLATE_NAME;
                    control.DISPLAY = param.DISPLAY;
                    control.UPDATED_BY = GetUserID();

                    db.QUOTES.Attach(control);
                    db.Entry(control).Property(x => x.QUOTE_NUMBER).IsModified = true;
                    db.Entry(control).Property(x => x.CLIENT_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TYPE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_STATUS).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_LOT_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_UNIT_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_STREET_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_STREET_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_ADDRESS).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_CITY).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_STATE).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_COUNTRY).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_POSTAL_CODE).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_AMOUNT).IsModified = true;
                    db.Entry(control).Property(x => x.CABINETS_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_START_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_EXPIRY_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.FLOOR_TYPE).IsModified = true;
                    db.Entry(control).Property(x => x.MATERIAL_ID).IsModified = true;
                    db.Entry(control).Property(x => x.STREET_SUFFIX_ID).IsModified = true;
                    db.Entry(control).Property(x => x.CONTACT_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.USE_AS_TEMPLATE).IsModified = true;
                    db.Entry(control).Property(x => x.TEMPLATE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.DISPLAY).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();

                    _ID = control.QUOTE_ID;
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }


            return Json(new { success = _success, Message = _message, ID= _ID });
        }

        [HttpPost]
        public JsonResult DeleteQuote(int QuanteID)
        {
            QUOTES anQuante = db.QUOTES.Find(QuanteID);
            if (anQuante != null)
            {
                try
                {
                    anQuante.IS_CANCELED = true;
                    anQuante.UPDATED_BY = GetUserID();
                    db.QUOTES.Attach(anQuante);

                    db.Entry(anQuante).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anQuante).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Quante not found" });
        }

        #region file
        [HttpGet]
        public JsonResult GetQuoteFileList(int QuanteID)
        {

            var data = (from D in db.DOCUMENTS
                        join DT in db.DOCUMENT_TYPES on D.DOCUMENT_TYPE_ID equals DT.DOCUMENT_TYPE_ID 
                            
                        join E in db.EMPLOYEES
                        on D.CREATED_BY equals E.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        where D.UNIQ_ID == QuanteID && D.DOC_KIND == (int)DocumentType.Quote && D.IS_CANCELED == false
                        select new
                        {
                            DOC_ID = D.DOC_ID,
                            UNIQ_ID = D.UNIQ_ID,
                            DOC_NAME = D.DOC_NAME,
                            DOC_DESC = D.DOC_DESC,
                            DOC_PATH = D.DOC_PATH,
                            FONT_AWESOME_ICON = DT.FONT_AWESOME_ICON,
                            CREATION_DATE = D.CREATION_DATE != null ?
                                                    D.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (D.CREATION_DATE.Month > 9 ? D.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + D.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    D.CREATION_DATE.Day : null,
                            MODIFIED_DATE = D.MODIFIED_DATE != null ?
                                                    D.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (D.MODIFIED_DATE.Month > 9 ? D.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + D.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    D.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                        ).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }
        
        
        [HttpPost]
        public JsonResult UpdateFile(int QuoteID, HttpPostedFileBase File,string FileDesc,int FileType)
        {
            QUOTES control = db.QUOTES.Find(QuoteID);
            if (control != null)
            {
                int fileSize = File.ContentLength;
                var fileName = Path.GetFileName(File.FileName);

                byte[] fileByteArray = new byte[fileSize];
                File.InputStream.Read(fileByteArray, 0, fileSize);

                //Uploading properly formatted file to server.
                string fileLocation = Path.Combine(Server.MapPath("~/Uploads/Quote/"+control.QUOTE_ID+""), fileName);
                if (!Directory.Exists(Server.MapPath("~/Uploads/Quote/" + control.QUOTE_ID + "")))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/Quote/" + control.QUOTE_ID + ""));
                File.SaveAs(fileLocation);                
                
                DOCUMENTS insert = new DOCUMENTS();
                insert.DOC_NAME = fileName;
                insert.DOC_DESC = FileDesc;
                insert.DOC_PATH = fileLocation;
                insert.UNIQ_ID = QuoteID;
                insert.DOCUMENT_TYPE_ID = (short)FileType;
                insert.DOC_KIND = (int)DocumentType.Quote;
                insert.MODIFIED_DATE = insert.CREATION_DATE = DateTime.Now;
                insert.CREATED_BY = insert.UPDATED_BY = GetUserID();

                db.DOCUMENTS.Add(insert);
                db.SaveChanges();

                return Json(new { success = true, message = "Files Added" });
            }
            else
                return Json(new { success = false, message = "Quante not found" });
        }

        [HttpPost]
        public JsonResult UpdateDocInf(DOCUMENTS param)
        {
            DOCUMENTS anDoc = db.DOCUMENTS.Find(param.DOC_ID);
            if (anDoc != null)
            {
                try
                {
                    anDoc.DOC_NAME = param.DOC_NAME;
                    anDoc.DOC_DESC = param.DOC_DESC;
                    anDoc.UPDATED_BY = GetUserID();
                    db.DOCUMENTS.Attach(anDoc);

                    db.Entry(anDoc).Property(x => x.DOC_NAME).IsModified = true;
                    db.Entry(anDoc).Property(x => x.DOC_DESC).IsModified = true;
                    db.Entry(anDoc).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Quante not found" });
        }

        [HttpPost]
        public JsonResult RemoveDocument(DOCUMENTS param)
        {
            var control = db.DOCUMENTS.Find(param.DOC_ID);
            if (control != null)
            {
                try
                {
                    if (System.IO.File.Exists(control.DOC_PATH))
                    {
                        System.IO.File.Delete(control.DOC_PATH);
                    }

                    control.IS_CANCELED = true;
                    control.UPDATED_BY = GetUserID();
                    db.DOCUMENTS.Attach(control);

                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Document not found" });
        }

        public void OpenFolder(string folderPath)
        {
            if (System.IO.File.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }
        #endregion
    }
}