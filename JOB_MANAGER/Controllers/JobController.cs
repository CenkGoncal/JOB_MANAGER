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
    public class JobController : BaseController
    {
        #region Job
        [AuthorityControl("Jobs")]
        public ActionResult Jobs(int? phaseID)
        {
            int CompanyID = GetCompanyID();
            int defaultCountry = db.COUNTRIES.Where(w => w.IS_DEFAULT == true).Select(s => s.COUNTRY_ID).FirstOrDefault();
            if (defaultCountry > 0)
            {
                ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "COUNTRY_ID", "COUNTRY_NAME");
            }
            else
            {
                ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
            }
            
            ViewBag.CONTACT_LIST = new SelectList(Enumerable.Empty<SelectListItem>(), "CONTACT_ID", "CONTACT_NAME");
            ViewBag.STATES_LIST = new SelectList(Enumerable.Empty<SelectListItem>(), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(Enumerable.Empty<SelectListItem>(), "CITY_ID", "CITY_NAME");
            ViewBag.STREET_LIST = new SelectList(db.STREET.Where(w => w.IS_CANCELED == false), "STREET_ID", "STREET_NAME");
            ViewBag.PROJECT_TYPE_LIST = new SelectList(db.PROJECT_TYPES.Where(w => w.IS_CANCELED == false), "PROJECT_TYPE_ID", "PROJECT_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(w => w.IS_CANCELED == false), "CLIENT_ID", "CLIENT_NAME");
            ViewBag.MATERIAL_LIST = new SelectList(db.MATERIALS.Where(w => w.IS_CANCELED == false), "MATERIAL_ID", "MATERIAL_NAME");            
            ViewBag.FLOOR_LIST = new SelectList(db.FLOOR_TYPES.Where(w => w.IS_CANCELED == false), "FLOOR_TYPE_ID", "FLOOR_TYPE_NAME");
            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");
            ViewBag.INSTALER_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_INSTALLER == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");


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

            ViewBag.PROJECT_PHASE = phaseID ?? 0;

            return View();
        }

        [HttpGet]
        public JsonResult GetListFilters()
        {
            int CompanyID = GetCompanyID();
            var dataCity = (from C in db.CITIES
                            join P in db.PROJECTS on C.CITY_ID equals P.PROJECT_CITY
                            where P.IS_CANCELED == false && P.COMPANY_ID == CompanyID
                            select new
                            {
                                CITY_ID = P.PROJECT_CITY,
                                CITY_NAME = C.CITY_NAME
                            }
                    ).OrderBy(o => o.CITY_NAME).Distinct().ToList();

            var dataClient = (from C in db.CLIENTS
                              join P in db.PROJECTS on C.CLIENT_ID equals P.CLIENT_ID
                              where P.IS_CANCELED == false && P.COMPANY_ID == CompanyID
                              select new
                              {
                                  CLIENT_ID = P.CLIENT_ID,
                                  CLIENT_NAME = C.CLIENT_NAME
                              }
                    ).OrderBy(o => o.CLIENT_NAME).Distinct().ToList();

            var dataPType = (from P in db.PROJECTS
                             join PT in db.PROJECT_TYPES on P.PROJECT_TYPE_ID equals PT.PROJECT_TYPE_ID
                             where P.IS_CANCELED == false && P.COMPANY_ID == CompanyID
                             select new
                             {
                                 PROJECT_TYPE_ID = P.PROJECT_TYPE_ID,
                                 PROJECT_TYPE_NAME = PT.PROJECT_TYPE_NAME
                             }
                        ).OrderBy(o => o.PROJECT_TYPE_NAME).Distinct().ToList();


            var dataStatus = (from P in db.PROJECTS
                              join S in db.STATUS on P.PROJECT_STATUS equals S.STATUS_ID
                              where P.IS_CANCELED == false && P.COMPANY_ID == CompanyID
                              select new
                              {
                                  STATUS_ID = S.STATUS_ID,
                                  STATUS_NAME = S.STATUS_NAME
                              }
            ).OrderBy(o => o.STATUS_NAME).Distinct().ToList();


            return Json(new { City = dataCity, Client = dataClient, Project = dataPType, Status = dataStatus }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]        
        public JsonResult GetJobList(int ProjectID,bool IsTemplate)
        {
            var data = ProjectList(ProjectID, IsTemplate);

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobAdvacedList(JobAdvancedSearch param)
        {
            var data = ProjectList(-1, false);

            if (string.IsNullOrEmpty(param.LotNumber) == false)
            {
                data = data.Where(w => w.ADR_LOT_NBR == param.LotNumber).ToList();
            }

            if (param.Material > 0)
            {
                data = data.Where(w => w.MATERIAL_ID == param.Material).ToList();
            }

            if (param.Floor > 0)
            {
                data = data.Where(w => w.FLOOR_TYPE == param.Floor).ToList();
            }

            if (param.Supervisor > 0)
            {
                data = data.Where(w => w.CLIENT_SUPRVISOR == param.Supervisor).ToList();
            }

            if (param.Client > 0)
            {
                data = data.Where(w => w.CLIENT_ID == param.Client).ToList();
            }

            if (param.FromStart > DateTime.MinValue)
            {
                data = data.Where(w => w.PROJECT_START_DATE_DT >= param.FromStart).ToList();
            }

            if (param.EndStart > DateTime.MinValue)
            {
                data = data.Where(w => w.PROJECT_START_DATE_DT <= param.EndStart).ToList();
            }

            if(param.Phase > 0)
            {
                data = data.Where(w => db.PROJECT_PHASES.Any(a => a.PROJECT_ID == w.PROJECT_ID && a.PHASE_ID == param.Phase)).ToList();
            }

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateJob(PROJECTS param)
        {
            bool _success = true;
            string _message = string.Empty;
            int CompanyID = GetCompanyID();
            int DefEventSize = GlobalTools.GetParamIntVal("START_YEAR_MODEL", CompanyID);
            if (DefEventSize < 0)
                DefEventSize = 30;

            try
            {
                PROJECTS control = db.PROJECTS.Where(w => w.PROJECT_ID == param.PROJECT_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                    
                    if (db.PROJECTS.Any(x => x.PROJECT_NAME.Equals(param.PROJECT_NAME) && x.IS_CANCELED == false && x.USE_AS_TEMPLATE == false && x.COMPANY_ID == CompanyID))
                    {
                        _success = false;
                        _message = "Project Name already exists.";
                    }
                    if (db.PROJECTS.Any(x => x.CLIENT_PO_NBR.Equals(param.CLIENT_PO_NBR) && x.IS_CANCELED == false && x.USE_AS_TEMPLATE == false && x.COMPANY_ID == CompanyID))
                    {
                        _success = false;
                        _message = "PO # already exists.";
                    }

                }
                else
                {
                    
                    if (db.PROJECTS.Any(x => x.PROJECT_NAME.Equals(param.PROJECT_NAME) && x.PROJECT_ID != param.PROJECT_ID && x.USE_AS_TEMPLATE == false && x.IS_CANCELED == false && x.COMPANY_ID == CompanyID))
                    {
                        _success = false;
                        _message = "Project Name already exists.";
                    }
                    if (db.PROJECTS.Any(x => x.CLIENT_PO_NBR.Equals(param.CLIENT_PO_NBR) && x.PROJECT_ID != param.PROJECT_ID && x.USE_AS_TEMPLATE == false && x.IS_CANCELED == false && x.COMPANY_ID == CompanyID))
                    {
                        _success = false;
                        _message = "PO # already exists.";
                    }

                }

                
                DateTime EndTime = param.INSTALLATION_DATE.Value.AddMinutes(DefEventSize);
                var Isholiday = (from H in db.HOLIDAYS
                                 where (H.START_DATE >= param.INSTALLATION_DATE && H.END_DATE <= param.INSTALLATION_DATE) ||
                                       (H.START_DATE >= EndTime && H.END_DATE <= EndTime)
                                 select H).ToList();
                if (Isholiday != null)
                {
                    _success = false;
                    _message = "The Start or End day coincides with the holiday.";                    
                }

                if (_success == false)
                    return Json(new { success = _success, Message = _message });


                if (isNew)
                {
                    int lastjOBID = db.PROJECTS.Where(w => w.IS_CANCELED == false).OrderByDescending(o => o.PROJECT_ID).Select(s => s.PROJECT_ID).FirstOrDefault();

                    param.PROJECT_ACTUAL_VAL = 0;
                    param.PROJECT_ESTIMATED_VAL = 0;                    
                    param.EVENT_SIZE = 30;

                    param.PROJECT_NUMBER = string.Format("J0B-{0}", lastjOBID + 1);
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();

                    db.PROJECTS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.MODIFIED_DATE = DateTime.Now;
                    control.PROJECT_NAME = param.PROJECT_NAME;
                    control.ADR_LOT_NBR = param.ADR_LOT_NBR;
                    control.ADR_STREET_NAME = param.ADR_STREET_NAME;
                    control.ADR_UNIT_NBR = param.ADR_UNIT_NBR;
                    control.CABINETS_NBR = param.CABINETS_NBR;
                    control.CLIENT_ID = param.CLIENT_ID;
                    control.CLIENT_PO_NBR = param.CLIENT_PO_NBR;
                    control.CLIENT_SUPRVISOR = param.CLIENT_SUPRVISOR;
                    control.DISPLAY = param.DISPLAY;
                    //control.EVENT_SIZE = param.EVENT_SIZE;
                    control.INSTALLATION_DATE = param.INSTALLATION_DATE;
                    control.INSTALLER_ID = param.INSTALLER_ID;
                    control.MATERIAL_ID = param.MATERIAL_ID;                    
                    control.PROJECT_ADDRESS = param.PROJECT_ADDRESS;
                    control.PROJECT_AMOUNT = param.PROJECT_AMOUNT;
                    control.PROJECT_CITY = param.PROJECT_CITY;
                    control.PROJECT_COUNTRY = param.PROJECT_COUNTRY;
                    control.PROJECT_DESC = param.PROJECT_DESC;
                    control.PROJECT_END_DATE = param.PROJECT_END_DATE;
                    //control.PROJECT_ESTIMATED_VAL = param.PROJECT_ESTIMATED_VAL;
                    control.PROJECT_POSTAL_CODE = param.PROJECT_POSTAL_CODE;
                    control.PROJECT_PROGRESS = param.PROJECT_PROGRESS;
                    control.PROJECT_START_DATE = param.PROJECT_START_DATE;
                    control.PROJECT_STATE = param.PROJECT_STATE;
                    control.PROJECT_STATUS = param.PROJECT_STATUS;
                    control.PROJECT_SUPERVISOR = param.PROJECT_SUPERVISOR;
                    control.PROJECT_TYPE_ID = param.PROJECT_TYPE_ID;
                    control.QUOTE_ID = param.QUOTE_ID;
                    control.STREET_SUFFIX_ID = param.STREET_SUFFIX_ID;
                    control.TEMPLATE_NAME = param.TEMPLATE_NAME;
                    control.USE_AS_TEMPLATE = param.USE_AS_TEMPLATE;                    
                    control.UPDATED_BY = GetUserID();

                    db.PROJECTS.Attach(control);
                    db.Entry(control).Property(x => x.PROJECT_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_LOT_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_STREET_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.ADR_UNIT_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.CABINETS_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.CLIENT_ID).IsModified = true;
                    db.Entry(control).Property(x => x.CLIENT_PO_NBR).IsModified = true;
                    db.Entry(control).Property(x => x.CLIENT_SUPRVISOR).IsModified = true;
                    db.Entry(control).Property(x => x.DISPLAY).IsModified = true;
                    db.Entry(control).Property(x => x.EVENT_SIZE).IsModified = true;
                    db.Entry(control).Property(x => x.INSTALLATION_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.INSTALLER_ID).IsModified = true;
                    db.Entry(control).Property(x => x.MATERIAL_ID).IsModified = true;
                    //db.Entry(control).Property(x => x.PROJECT_ACTUAL_VAL).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_ADDRESS).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_AMOUNT).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_CITY).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_COUNTRY).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_ESTIMATED_VAL).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_POSTAL_CODE).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_PROGRESS).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_START_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_STATE).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_STATUS).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_SUPERVISOR).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TYPE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.QUOTE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.STREET_SUFFIX_ID).IsModified = true;
                    db.Entry(control).Property(x => x.TEMPLATE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.USE_AS_TEMPLATE).IsModified = true;
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
        public JsonResult DeleteJob(int ProjectID)
        {
            PROJECTS anProject = db.PROJECTS.Find(ProjectID);
            if (anProject != null)
            {
                try
                {
                    anProject.IS_CANCELED = true;
                    anProject.UPDATED_BY = GetUserID();
                    db.PROJECTS.Attach(anProject);

                    db.Entry(anProject).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anProject).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Project not found" });
        }

        [HttpGet]
        public JsonResult GetAllTemplate()
        {
            var CompanyID = GetCompanyID();

            var data = (from P in db.PROJECTS
                        where P.USE_AS_TEMPLATE == true
                        select new
                        {
                            TEMPLATE_ID = P.PROJECT_ID,
                            TEMPLATE_NAME = P.TEMPLATE_NAME
                        }
                        ).ToList();

            return Json(new { Template = data }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        private List<ProjectViewDto> ProjectList(int  ProjectID,bool IsTemplate)
        {
            List<ProjectViewDto> result = new List<ProjectViewDto>();

            var CompanyID = GetCompanyID();

            var data = (from P in db.PROJECTS                        
                        join C in db.CLIENTS on P.CLIENT_ID equals C.CLIENT_ID
                        join PT in db.PROJECT_TYPES on P.PROJECT_TYPE_ID equals PT.PROJECT_TYPE_ID
                        join S in db.STATUS on P.PROJECT_STATUS equals S.STATUS_ID
                        join CT in db.CITIES on P.PROJECT_CITY equals CT.CITY_ID
                        join ST in db.STATES on P.PROJECT_STATE equals ST.STATE_ID
                        join CN in db.COUNTRIES on P.PROJECT_COUNTRY equals CN.COUNTRY_ID
                        join F in db.FLOOR_TYPES on P.FLOOR_TYPE equals F.FLOOR_TYPE_ID
                        join M in db.MATERIALS on P.MATERIAL_ID equals M.MATERIAL_ID
                        join STT in db.STREET on P.STREET_SUFFIX_ID equals STT.STREET_ID
                        from CS_LEFT in db.CONTACTS.Where(w => w.CONTACT_ID == P.CLIENT_SUPRVISOR).DefaultIfEmpty()
                        from PS_LEFT in db.EMPLOYEES.Where(w => w.EMP_ID == P.PROJECT_SUPERVISOR).DefaultIfEmpty()     
                        from Q_LEFT in db.QUOTES.Where(w=>w.QUOTE_ID == P.QUOTE_ID).DefaultIfEmpty()
                        where P.IS_CANCELED == false && P.COMPANY_ID == CompanyID
                        select new ProjectViewDto
                        {
                            PROJECT_ID = P.PROJECT_ID,
                            QUOTE_ID = Q_LEFT.QUOTE_ID,
                            QUOTE_NUMBER = Q_LEFT.QUOTE_NUMBER,
                            PROJECT_NUMBER = P.PROJECT_NUMBER,
                            CLIENT_ID = P.CLIENT_ID,
                            CLIENT_NAME = C.CLIENT_NAME,
                            CLIENT_PO_NBR = P.CLIENT_PO_NBR,
                            PROJECT_TYPE_ID = P.PROJECT_TYPE_ID,
                            PROJECT_TYPE_NAME = PT.PROJECT_TYPE_NAME,
                            PROJECT_STATUS = P.PROJECT_STATUS,
                            PROJECT_NAME = P.PROJECT_NAME,
                            DISPLAY_CLASS = S.DISPLAY_CLASS,
                            STATUS_NAME = S.STATUS_NAME,
                            ADR_LOT_NBR = P.ADR_LOT_NBR,
                            ADR_UNIT_NBR = P.ADR_UNIT_NBR,
                            ADR_STREET_NBR = P.ADR_STREET_NBR,
                            ADR_STREET_NAME = P.ADR_STREET_NAME,
                            PROJECT_ADDRESS = P.PROJECT_ADDRESS,
                            PROJECT_COUNTRY = P.PROJECT_COUNTRY,
                            COUNTRY_NAME = CN.COUNTRY_NAME,
                            PROJECT_STATE = P.PROJECT_STATE,
                            STATE_NAME = ST.STATE_NAME,
                            PROJECT_CITY = P.PROJECT_CITY,
                            CITY_NAME = CT.CITY_NAME,
                            PROJECT_POSTAL_CODE = P.PROJECT_POSTAL_CODE,
                            PROJECT_AMOUNT = P.PROJECT_AMOUNT,
                            PROJECT_ESTIMATED_VAL = P.PROJECT_ESTIMATED_VAL,
                            PROJECT_ACTUAL_VAL = P.PROJECT_ACTUAL_VAL,
                            CLIENT_SUPRVISOR = P.CLIENT_SUPRVISOR,
                            CLI_SUPERVISOR_NAME = PS_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + PS_LEFT.LAST_NAME,
                            PROJECT_SUPERVISOR = P.PROJECT_SUPERVISOR,
                            PRO_SUPERVISOR_NAME = CS_LEFT.CONTACT_FIRST_NAME +SqlConstants.stringWhiteSpace + CS_LEFT.CONTACT_LAST_NAME,
                            PROJECT_PROGRESS = P.PROJECT_PROGRESS,
                            CABINETS_NBR = P.CABINETS_NBR,
                            PROJECT_DESC = P.PROJECT_DESC,
                            INSTALLATION_DATE = P.INSTALLATION_DATE != null ?
                                                    P.INSTALLATION_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (P.INSTALLATION_DATE.Value.Month > 9 ? P.INSTALLATION_DATE.Value.Month + SqlConstants.stringMinus : "0" + P.INSTALLATION_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    P.INSTALLATION_DATE.Value.Day : null,
                            TIME = P.INSTALLATION_DATE != null ? P.INSTALLATION_DATE.Value.Hour + SqlConstants.stringIkiNokta + P.INSTALLATION_DATE.Value.Minute : null,
                            PROJECT_START_DATE = P.PROJECT_START_DATE != null ?
                                                    P.PROJECT_START_DATE.Year + SqlConstants.stringMinus +
                                                    (P.PROJECT_START_DATE.Month > 9 ? P.PROJECT_START_DATE.Month + SqlConstants.stringMinus : "0" + P.PROJECT_START_DATE.Month + SqlConstants.stringMinus) +
                                                    P.PROJECT_START_DATE.Day : null,
                            PROJECT_START_DATE_DT = P.PROJECT_START_DATE,
                            PROJECT_END_DATE = P.PROJECT_END_DATE != null ?
                                                    P.PROJECT_END_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (P.PROJECT_END_DATE.Value.Month > 9 ? P.PROJECT_END_DATE.Value.Month + SqlConstants.stringMinus : "0" + P.PROJECT_END_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    P.PROJECT_END_DATE.Value.Day : null,
                            PROJECT_END_DATE_DT = P.PROJECT_END_DATE,
                            DISPLAY = P.DISPLAY,
                            FLOOR_TYPE = P.FLOOR_TYPE,
                            FLOOR_TYPE_NAME = F.FLOOR_TYPE_NAME,
                            MATERIAL_ID = P.MATERIAL_ID,
                            MATERIAL_NAME = M.MATERIAL_NAME,
                            STREET_SUFFIX_ID = P.STREET_SUFFIX_ID,
                            STREET_NAME = STT.STREET_NAME,
                            USE_AS_TEMPLATE = P.USE_AS_TEMPLATE,
                            CREATION_DATE = P.CREATION_DATE != null ?
                                                    P.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (P.CREATION_DATE.Month > 9 ? P.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + P.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    P.CREATION_DATE.Day : null,
                            MODIFIED_DATE = P.MODIFIED_DATE != null ?
                                                    P.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (P.MODIFIED_DATE.Month > 9 ? P.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + P.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    P.MODIFIED_DATE.Day : null,

                        }
                       ).OrderBy(o => o.CLIENT_NAME).ToList();

            if (ProjectID > 0)
                data = data.Where(w => w.PROJECT_ID == ProjectID).ToList();                
            
            if(IsTemplate)
                data = data.Where(w => w.USE_AS_TEMPLATE == true).ToList();
            else
                data = data.Where(w => w.USE_AS_TEMPLATE == false).ToList();

            return data;
        }

        [HttpGet]
        [AuthorityControl("Jobs")]
        public ActionResult JobView(int id,int Layout)
        {
            ProjectViewDto newdata;
            var data = ProjectList(id,false);            
            if(data == null)
            {
                newdata = new ProjectViewDto();                                
            }
            else
            {
                newdata = data.FirstOrDefault();

                newdata.ProjectDto = new ProjectDto();
                newdata.ProjectDto.Document = GlobalTools.GetDocumentLookUp(2, id, 0, 0, 0);
                newdata.ProjectDto.Note = GlobalTools.GetNoteLookUp(2, id, 0, 0, 0);
                newdata.ProjectDto.Phase = GlobalTools.GetPhasesLookUp(id, 0);
                newdata.ProjectDto.Task = GlobalTools.GetTasksLookUp(id, 0,0);

                newdata.ProjectDto.Document.OrderBy(o => o.DOC_FOLDER_NAME);
            }

            ViewBag.Layout = Layout;
            ViewBag.DefaultProfileImg = Convert.ToBase64String(GlobalTools.GetParamByteVal("NO_PICTURE_IMG", GetCompanyID()));

            return PartialView("JobView", newdata);
        }
        

        [HttpGet]
        public JsonResult ProjectOpDetails(int ProjectID)
        {

            var Phases = GlobalTools.GetPhasesLookUp(ProjectID, 0);

            foreach (var phase in Phases)
            {
                phase.TASKS = GlobalTools.GetTasksLookUp(ProjectID, phase.PROJECT_PHASE_ID, 0);
            }

            var Members = GlobalTools.GetMembersLookUp(ProjectID, 0, 0);
            var Notes = GlobalTools.GetNoteLookUp(2,ProjectID, 0, 0, 0);
            int ProjectProg = db.PROJECTS.Where(w => w.PROJECT_ID == ProjectID).Select(s => s.PROJECT_PROGRESS).FirstOrDefault();

           return Json(new { Phases = Phases, Members = Members, Notes= Notes, ProjectProg= ProjectProg }, JsonRequestBehavior.AllowGet);
        }

        #region Phase
        [HttpGet]
        public  ActionResult EditPhase(int? id)
        {
            int CompanyID = GetCompanyID();

            ViewBag.EMPLOYEE_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == CompanyID)
            .Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");
            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.PhaseTask), "STATUS_ID", "STATUS_NAME");



            PhasesDto Phase = new PhasesDto();

            int project = db.PROJECT_PHASES.Where(w => w.PROJECT_PHASE_ID == id).Select(s => s.PROJECT_ID).FirstOrDefault();
            Phase = GlobalTools.GetPhasesLookUp(project, id).FirstOrDefault();
            Phase.TASKS = GlobalTools.GetTasksLookUp(project, id,0);
            Phase.MEMBERS = GlobalTools.GetMembersLookUp(project, id, 0);

        
            return PartialView("EditPhase", Phase);
        }

        [HttpGet]
        public ActionResult ViewPhase(int id)
        {
            int project = db.PROJECT_PHASES.Where(w => w.PROJECT_PHASE_ID == id).Select(s=>s.PROJECT_ID).FirstOrDefault();
            var phase = GlobalTools.GetPhasesLookUp(project, id).FirstOrDefault();
            return PartialView("ViewPhase", phase);
        }        
        
        [HttpPost]
        public JsonResult AddOrUpdatePhase(PhasesExtented param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                PROJECT_PHASES control = db.PROJECT_PHASES.Where(w => w.PROJECT_PHASE_ID == param.PROJECT_PHASE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                }
               
                if (param.PHASE_START_DATE != null && param.PHASE_END_DATE != null &&
                        ((DateTime)param.PHASE_START_DATE).CompareTo((DateTime)param.PHASE_END_DATE) > 0)
                {
                    _message = "End date must be greater or equal to start date!";
                    _success = false;
                    return Json(new { success = _success, Message = _message });
                }


                if (isNew)
                {
                    //int lastOrder = db.PROJECT_PHASES.Where(w => w.IS_CANCELED == false).OrderByDescending(o => o.PHASE_ORDER).Select(s => s.PHASE_ORDER).FirstOrDefault();       
                    db.PROJECT_PHASES.Add(param);
                    db.SaveChanges();
                }
                else
                {             
                    control.MODIFIED_DATE = DateTime.Now;
                    control.PHASE_ORDER = param.PHASE_ORDER;
                    control.PROJECT_PHASE_STATUS = param.PROJECT_PHASE_STATUS;
                    control.PROJECT_PHASE_PROGRESS = param.PROJECT_PHASE_PROGRESS;
                    control.SUPERVISOR_ID = param.SUPERVISOR_ID;
                    control.PHASE_START_DATE = param.PHASE_START_DATE;
                    control.PHASE_END_DATE = param.PHASE_END_DATE;
                    control.PROJECT_PHASE_DESC = param.PROJECT_PHASE_DESC;
                    control.UPDATED_BY = GetUserID();

                    db.PROJECT_PHASES.Attach(control);
                    db.Entry(control).Property(x => x.PHASE_ORDER).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_PHASE_STATUS).IsModified = true;
                    db.Entry(control).Property(x => x.SUPERVISOR_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_PHASE_PROGRESS).IsModified = true;
                    db.Entry(control).Property(x => x.PHASE_START_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.PHASE_END_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_PHASE_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    db.SaveChanges();
                }
                

                _success = true;
                _message = "Saved Phase";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }


            return Json(new { success = _success, Message = _message});
        }
        #endregion

        #region Task

        [HttpGet]
        [AuthorityControl("Jobs")]
        public ActionResult EditTask(int projectID, int? TaskID)
        {
            int CompanyID = GetCompanyID();

            ViewBag.PROJECT_PHASE_ID = new SelectList(GlobalTools.GetPhasesLookUp(projectID, 0), "PROJECT_PHASE_ID", "PHASE_NAME");
            ViewBag.EMPLOYEE_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == CompanyID)
            .Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");
            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.PhaseTask), "STATUS_ID", "STATUS_NAME");

            TasksDto task = new TasksDto();

            var TASKS = GlobalTools.GetTasksLookUp(projectID, 0, 0);
            if (TaskID.HasValue && TaskID.Value > 0)
                task = TASKS.Where(w => w.PROJECT_TASK_ID == TaskID.Value).FirstOrDefault();
            else
                task.PROJECT_ID = projectID;

            return PartialView("EditTask", task);
        }

        [HttpGet]
        [AuthorityControl("Jobs")]
        public ActionResult ViewTask(int TaskID)
        {
            int projectID = db.PROJECT_TASK.Where(w => w.PROJECT_TASK_ID== TaskID).Select(s => s.PROJECT_ID).FirstOrDefault();
            var TASKS = GlobalTools.GetTasksLookUp(projectID, 0, TaskID).FirstOrDefault();
            ViewBag.DefaultProfileImg = GlobalTools.GetParamByteVal("NO_PICTURE_IMG", GetCompanyID());

            return PartialView("ViewTask", TASKS);
        }

        [HttpPost]
        public JsonResult AddOrUpdateTask(TaskExtented param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                PROJECT_TASK control = db.PROJECT_TASK.Where(w => w.PROJECT_TASK_ID == param.PROJECT_TASK_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                    if (db.PROJECT_TASK.Any(x => x.PROJECT_TASK_NAME.Equals(param.PROJECT_TASK_NAME) && x.PROJECT_ID == param.PROJECT_ID && x.PROJECT_PHASE_ID == param.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Task Name already exists.";
                        _success = false;
                    }
                    if (db.PROJECT_TASK.Any(x => x.TASK_ORDER == param.TASK_ORDER && x.PROJECT_ID == param.PROJECT_ID && x.PROJECT_PHASE_ID == param.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Order already exists in the same phase.";
                        _success = false;
                    }
                }
                else
                {
                    if (db.PROJECT_TASK.Any(x => x.PROJECT_TASK_ID != param.PROJECT_TASK_ID && x.PROJECT_TASK_NAME.Equals(param.PROJECT_TASK_NAME) && x.PROJECT_ID == param.PROJECT_ID && x.PROJECT_PHASE_ID == param.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Task Name already exists.";
                        _success = false;
                    }
                    if (db.PROJECT_TASK.Any(x => x.PROJECT_TASK_ID != param.PROJECT_TASK_ID && x.TASK_ORDER == param.TASK_ORDER && x.PROJECT_ID == param.PROJECT_ID && x.PROJECT_PHASE_ID == param.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Order already exists in the same phase.";
                        _success = false;
                    }
                }

                if(_success == false)
                    return Json(new { success = _success, Message = _message });

                if (isNew)
                {
                    PROJECT_TASK insert = new PROJECT_TASK();
                    insert.PROJECT_ID = param.PROJECT_ID;
                    insert.PROJECT_PHASE_ID = param.PROJECT_PHASE_ID;
                    insert.TASK_ORDER = param.TASK_ORDER;
                    insert.PROJECT_TASK_NAME = param.PROJECT_TASK_NAME;
                    insert.PROJECT_TASK_STATUS = param.PROJECT_TASK_STATUS;
                    insert.PROJECT_TASK_PROGRESS = param.PROJECT_TASK_PROGRESS;
                    insert.TASK_START_DATE = param.TASK_START_DATE;
                    insert.TASK_END_DATE = param.TASK_END_DATE;
                    insert.PROJECT_TASK_DESC = param.PROJECT_TASK_DESC;
                    insert.IS_CANCELED = param.IS_CANCELED;
                    insert.MODIFIED_DATE = insert.CREATION_DATE = DateTime.Now;
                    insert.CREATED_BY = insert.UPDATED_BY = GetUserID();

                    db.PROJECT_TASK.Add(insert);
                    db.SaveChanges();
                }
                else
                {
                    control.MODIFIED_DATE = DateTime.Now;
                    control.PROJECT_ID = param.PROJECT_ID;
                    control.PROJECT_PHASE_ID = param.PROJECT_PHASE_ID;
                    control.TASK_ORDER = param.TASK_ORDER;
                    control.PROJECT_TASK_NAME = param.PROJECT_TASK_NAME;
                    control.PROJECT_TASK_STATUS = param.PROJECT_TASK_STATUS;
                    control.PROJECT_TASK_PROGRESS = param.PROJECT_TASK_PROGRESS;
                    control.TASK_START_DATE = param.TASK_START_DATE;
                    control.TASK_END_DATE = param.TASK_END_DATE;
                    control.PROJECT_TASK_DESC = param.PROJECT_TASK_DESC;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();

                    db.PROJECT_TASK.Attach(control);
                    db.Entry(control).Property(x => x.PROJECT_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_PHASE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.TASK_ORDER).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TASK_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TASK_STATUS).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TASK_PROGRESS).IsModified = true;
                    db.Entry(control).Property(x => x.TASK_START_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.TASK_END_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TASK_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    db.SaveChanges();
                }

                if (param.ASSINGED_MEMBERS_ARR != null)
                {
                    foreach (int EmpID in param.ASSINGED_MEMBERS_ARR)
                    {
                        if (EmpID == 0)
                            continue;

                        PROJECT_MEMBERS member = new PROJECT_MEMBERS();
                        member.PROJECT_ID = param.PROJECT_ID;
                        member.PROJECT_PHASE_ID = param.PROJECT_PHASE_ID;
                        member.PROJECT_TASK_ID = param.PROJECT_TASK_ID;
                        member.PROJECT_MEMBER_ID = 0;
                        member.EMPLOYEE_ID = EmpID;

                        AddOrUpdateProjectMember(member);
                    }
                }
                _success = true;
                _message = "Saved Task";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }


            return Json(new { success = _success, Message = _message });
        }
        
        [HttpPost]
        public JsonResult RemoveTask(PROJECT_TASK param)
        {
            PROJECT_TASK anProject = db.PROJECT_TASK.Find(param.PROJECT_TASK_ID);
            if (anProject != null)
            {
                try
                {
                    anProject.IS_CANCELED = true;
                    anProject.UPDATED_BY = GetUserID();
                    db.PROJECT_TASK.Attach(anProject);

                    db.Entry(anProject).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anProject).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Project Task not found" });
        }
        #endregion

        #region Member
        [HttpGet]
        [AuthorityControl("Jobs")]
        public ActionResult EditMember(int projectID, int? MemberID)
        {
            int CompanyID = GetCompanyID();

            ViewBag.EMPLOYEE_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_CANCELED == false && c.COMPANY_ID == CompanyID)
            .Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");
            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.PhaseTask), "STATUS_ID", "STATUS_NAME");


            ViewBag.PROJECT_PHASE_ID = new SelectList(GlobalTools.GetPhasesLookUp(projectID, 0), "PROJECT_PHASE_ID", "PHASE_NAME");
            ViewBag.PROJECT_TASK_ID = new SelectList(GlobalTools.GetTasksLookUp(projectID, 0, 0), "PROJECT_TASK_ID", "PROJECT_TASK_NAME");

            MembersDto member = new MembersDto();

            var MEMBERS = GlobalTools.GetMembersLookUp(projectID, 0, 0);
            if (MemberID.HasValue && MemberID.Value > 0)
                member = MEMBERS.Where(w => w.PROJECT_MEMBER_ID == MemberID.Value).FirstOrDefault();
            else
                member.PROJECT_ID = projectID;

            return PartialView("EditMember", member);
        }

        [HttpPost]
        public JsonResult AddOrUpdateProjectMember(PROJECT_MEMBERS param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {                                
                PROJECT_MEMBERS control = db.PROJECT_MEMBERS.Where(w => w.PROJECT_MEMBER_ID == param.PROJECT_MEMBER_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                    PROJECT_MEMBERS control2 = db.PROJECT_MEMBERS.Where(w => w.EMPLOYEE_ID == param.EMPLOYEE_ID && w.PROJECT_ID == param.PROJECT_ID  ).FirstOrDefault();
                    if(control2 != null)
                    {
                        if (control2.IS_CANCELED == true)
                        {
                            isNew = false;
                            control = control2;
                            control.IS_CANCELED = false;
                        }
                        else
                        if (control2.IS_CANCELED == false && control2.PROJECT_MEMBER_ID != param.PROJECT_MEMBER_ID)
                        {
                            return Json(new { success = false, Message = "This Member has already been added" });
                        }
                    }                        

                }
                else
                {
                    var control2 = db.PROJECT_MEMBERS.Where(w => w.EMPLOYEE_ID == param.EMPLOYEE_ID && w.PROJECT_ID == param.PROJECT_ID && w.PROJECT_MEMBER_ID != param.PROJECT_MEMBER_ID
                                                       &&  w.IS_CANCELED == false
                                  ).FirstOrDefault();

                    if(control2 != null)
                    {
                        return Json(new { success = false, Message = "This Member has already been added" });
                    }
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();                    

                    db.PROJECT_MEMBERS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.MODIFIED_DATE = DateTime.Now;
                    control.PROJECT_ID = param.PROJECT_ID;
                    control.PROJECT_PHASE_ID = param.PROJECT_PHASE_ID;
                    control.PROJECT_TASK_ID = param.PROJECT_TASK_ID;
                    control.EMPLOYEE_ID = param.EMPLOYEE_ID;
                    control.SUPERVISOR_ID = param.SUPERVISOR_ID;
                    control.IS_SUPERVISOR = param.IS_SUPERVISOR;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();

                    db.PROJECT_MEMBERS.Attach(control);
                    db.Entry(control).Property(x => x.PROJECT_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_PHASE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TASK_ID).IsModified = true;
                    db.Entry(control).Property(x => x.SUPERVISOR_ID).IsModified = true;
                    db.Entry(control).Property(x => x.SUPERVISOR_ID).IsModified = true;
                    db.Entry(control).Property(x => x.IS_SUPERVISOR).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Saved Member";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }


            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult RemoveMembers(PROJECT_MEMBERS param)
        {
            PROJECT_MEMBERS anProject = db.PROJECT_MEMBERS.Find(param.PROJECT_MEMBER_ID);
            if (anProject != null)
            {
                try
                {
                    anProject.IS_CANCELED = true;
                    anProject.UPDATED_BY = GetUserID();
                    db.PROJECT_MEMBERS.Attach(anProject);

                    db.Entry(anProject).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anProject).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Project Task not found" });
        }
        #endregion

        [HttpGet]
        [AuthorityControl("Jobs")]
        public ActionResult EditNotes(int projectID, int? NoteID)
        {
            int CompanyID = GetCompanyID();

            ViewBag.MEMBER_LIST = new SelectList(GlobalTools.GetMembersLookUp(projectID, 0, 0), "EMP_ID", "EMP_NAME");
            ViewBag.PROJECT_PHASE_ID = new SelectList(GlobalTools.GetPhasesLookUp(projectID, 0), "PROJECT_PHASE_ID", "PHASE_NAME");
            ViewBag.PROJECT_TASK_ID = new SelectList(GlobalTools.GetTasksLookUp(projectID, 0, 0), "PROJECT_TASK_ID", "PROJECT_TASK_NAME");
            ViewBag.NOTE_TYPE_ID = new SelectList(db.NOTE_TYPES.Where(w=>w.IS_CANCELED == false), "NOTE_TYPE_ID", "NOTE_TYPE_NAME");

            NoteDto note = new NoteDto();
            var Notes = GlobalTools.GetNoteLookUp(2, projectID, 0, 0, 0);
            if (NoteID.HasValue && NoteID > 0)
                note = Notes.Where(w => NoteID == NoteID.Value).FirstOrDefault();
            else
                note.UNIQ_ID = projectID;

            return PartialView("EditNotes", note);
        }

        [HttpGet]
        public JsonResult GetDocumentList(int ProjectID, int? PhaseID, int? TaskID)
        {

            var Members = GlobalTools.GetMembersLookUp(ProjectID, PhaseID, TaskID);
            var NoteType = (from NT in db.NOTE_TYPES
                           where NT.IS_CANCELED == false
                           select new 
                           {
                               NOTE_TYPE_ID = NT.NOTE_TYPE_ID,
                               NOTE_TYPE_NAME = NT.NOTE_TYPE_NAME
                           }).ToList();

            return Json(new { Members = Members, NoteType= NoteType }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPhaseList(int ProjectID,int? PhaseID)
        {
            return Json(new { Getlist = GlobalTools.GetPhasesLookUp(ProjectID, PhaseID) }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetTaskList(int ProjectID, int? PhaseID, int? TaskID)
        {
            return Json(new { Getlist = GlobalTools.GetTasksLookUp(ProjectID, PhaseID, TaskID) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetNoteList(int TypeID,int UniqID,int? PhaseID, int? TaskID, int? NoteID)
        {        
            return Json(new { Getlist = GlobalTools.GetNoteLookUp(TypeID,UniqID,PhaseID,TaskID,NoteID) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDocumentFileList(int TypeID, int UniqID, int? PhaseID, int? TaskID, int? NoteID)
        {            
            return Json(new { Getlist = GlobalTools.GetDocumentLookUp(TypeID, UniqID, PhaseID, TaskID, NoteID) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMembersList(int ProjectID, int? PhaseID, int? TaskID)
        {
            return Json(new { Getlist = GlobalTools.GetMembersLookUp(ProjectID, PhaseID, TaskID) }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCalenderJobs(int? ProjectID)
        {
            var list = (from P in db.JOB_CALENDAR_VIEW
                        select new
                        {
                            PROJECT_ID = P.PROJECT_ID,
                            TITLE = P.TITLE,
                            START_DATE = P.START_DATE,
                            STATUS_ID = P.STATUS_ID,
                            END_DATE = P.END_DATE,
                            COLOR = P.COLOR,
                            CONTENT = P.CONTENT
                        }).ToList();

            if(ProjectID.HasValue && ProjectID.Value > 0)
            {
                list = list.Where(w => w.PROJECT_ID == ProjectID.Value).ToList();
            }

            return Json(new { Getlist = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateInstalationDate(DateTime start, DateTime end, int status, int projectId)
        {
            if (start > end)
                return Json(new { success = false, message = "Start date  must be less then End date" });

            var Isholiday = (from H in db.HOLIDAYS
                             where (H.START_DATE >= start && H.END_DATE <= start) ||
                                   (H.START_DATE >= end && H.END_DATE <= end)
                             select H).ToList();
            if(Isholiday != null)
                return Json(new { success = false, message = "The Start or End day coincides with the holiday." });

            PROJECTS anProject = db.PROJECTS.Find(projectId);
            if (anProject != null)
            {
                try
                {
                    var minute = end - start;

                    if (status > 0)
                        anProject.PROJECT_STATUS = (short)status;

                    anProject.INSTALLATION_DATE = start;
                    anProject.EVENT_SIZE = minute.Minutes;
                    anProject.UPDATED_BY = GetUserID();
                    db.PROJECTS.Attach(anProject);

                    if (status > 0)
                        db.Entry(anProject).Property(x => x.PROJECT_STATUS).IsModified = true;

                    db.Entry(anProject).Property(x => x.INSTALLATION_DATE).IsModified = true;
                    db.Entry(anProject).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();

                    string statusColor = db.STATUS.Where(w => w.STATUS_ID == anProject.PROJECT_STATUS).Select(s => s.DISPLAY_CLASS).FirstOrDefault();

                    return Json(new { success = true, color = statusColor });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Project not found" });
        }

        [HttpPost]
        public JsonResult UpdateFile(int UniqID,int PhaseID,int NoteID,int TaskID, HttpPostedFileBase File, string FileDesc, int FileType)
        {
            PROJECTS control = db.PROJECTS.Find(UniqID);
            if (control != null)
            {
                int fileSize = File.ContentLength;
                var fileName = Path.GetFileName(File.FileName);

                byte[] fileByteArray = new byte[fileSize];
                File.InputStream.Read(fileByteArray, 0, fileSize);

                var Folder = NoteID > 0 ? "Note" : (TaskID > 0 ? "Task" : (PhaseID > 0 ? "Phase" : ""));
                var ID = NoteID > 0 ? NoteID : (TaskID > 0 ? TaskID : (PhaseID > 0 ? PhaseID : 0));

                if (ID <= 0)
                    return Json(new { success = false, message = "Can not be Added Folder" });

                //Uploading properly formatted file to server.
                string fileLocation = Path.Combine(Server.MapPath("~/Uploads/Project/" + control.PROJECT_ID+ "/"+ Folder + "/"+ ID), fileName);
                if (!Directory.Exists(Server.MapPath("~/Uploads/Project/" + control.PROJECT_ID + "/" + Folder + "/" + ID)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/Project/" + control.PROJECT_ID + "/" + Folder + "/" + ID));
                File.SaveAs(fileLocation);

                DOCUMENTS insert = new DOCUMENTS();
                insert.DOC_NAME = fileName;
                insert.DOC_DESC = FileDesc;
                insert.DOC_PATH = fileLocation;
                insert.UNIQ_ID = UniqID;
                insert.DOCUMENT_TYPE_ID = (short)FileType;
                insert.DOC_KIND = (int)DocumentType.Project;
                insert.MODIFIED_DATE = insert.CREATION_DATE = DateTime.Now;
                insert.CREATED_BY = insert.UPDATED_BY = GetUserID();

                if (TaskID > 0)
                    insert.TASK_ID = TaskID;

                if (PhaseID > 0)
                    insert.PHASE_ID = PhaseID;

                if (NoteID > 0)
                    insert.NOTE_ID = NoteID;
                db.DOCUMENTS.Add(insert);
                db.SaveChanges();

                return Json(new { success = true, message = "Files Added" });
            }
            else
                return Json(new { success = false, message = "Quante not found" });
        }
        

    }
}