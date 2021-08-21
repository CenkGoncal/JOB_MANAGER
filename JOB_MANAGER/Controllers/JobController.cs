using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.Bussiness.Concrete;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers;
using JOB_MANAGER.DATAACESS.Helper;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.DATAACESS.Models.ComplexType;
using JOB_MANAGER.Models;
using System;
using System.Collections.Generic;
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
            CountyManager countyManager = new CountyManager(new CountyDal());
            StreetManager streetManager = new StreetManager(new StreetDal());
            ProjectTypeManager projectTypeManager  = new ProjectTypeManager(new ProjectTypeDal());
            MateryalManager materyalManager   = new MateryalManager(new MateryalDal());
            FloorTypeManager  floorTypeManager   = new FloorTypeManager(new FloorTypeDal());
            EmployeeManager employeeManager = new EmployeeManager(new EmployeeDal());
            StatusManager statusManager = new StatusManager(new StatusDal());

            int defaultCountry = countyManager.GetAll().Where(w => w.IS_DEFAULT == true).Select(s => s.COUNTRY_ID).FirstOrDefault();
            if (defaultCountry > 0)
            {
                ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "COUNTRY_ID", "COUNTRY_NAME");
            }
            else
            {
                ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll().Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
            }

            

            ViewBag.CONTACT_LIST = new SelectList(Enumerable.Empty<SelectListItem>(), "CONTACT_ID", "CONTACT_NAME");
            ViewBag.STATES_LIST = new SelectList(Enumerable.Empty<SelectListItem>(), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(Enumerable.Empty<SelectListItem>(), "CITY_ID", "CITY_NAME");
            ViewBag.STREET_LIST = new SelectList(streetManager.GetAll().Where(w => w.IS_CANCELED == false), "STREET_ID", "STREET_NAME");
            ViewBag.PROJECT_TYPE_LIST = new SelectList(projectTypeManager.GetAll().Where(w => w.IS_CANCELED == false), "PROJECT_TYPE_ID", "PROJECT_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(w => w.IS_CANCELED == false), "CLIENT_ID", "CLIENT_NAME");
            ViewBag.MATERIAL_LIST = new SelectList(materyalManager.GetAll().Where(w => w.IS_CANCELED == false), "MATERIAL_ID", "MATERIAL_NAME");            
            ViewBag.FLOOR_LIST = new SelectList(floorTypeManager.GetAll().Where(w => w.IS_CANCELED == false), "FLOOR_TYPE_ID", "FLOOR_TYPE_NAME");
            ViewBag.SUPERVISOR_LIST = new SelectList(employeeManager.GetEmployeesByTypes(true,false,false,ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(e => new
            {
                e.EMP_ID,
                NAME = e.EMP_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");
            ViewBag.INSTALER_LIST = new SelectList(employeeManager.GetEmployeesByTypes(false, true, false, ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");


            var data = statusManager.GetAllByType((int)StatusType.Project);
             
            ViewBag.STATUS_LIST = data;

            ViewBag.PROJECT_PHASE = phaseID ?? 0;

            return View();
        }

        [HttpGet]
        public JsonResult GetListFilters()
        {
            CityManager cityManager = new CityManager(new CitiesDal());

            var dataCity = cityManager.GetAll().Where(w=>w.COUNTRY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).OrderBy(o => o.CITY_NAME).Distinct().ToList();

            ClientManager clientManager = new ClientManager(new ClientDal());

            var dataClient = clientManager.GetAll().Where(w=>w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).OrderBy(o => o.CLIENT_NAME).Distinct().ToList();

            ProjectManager projectManager = new ProjectManager(new ProjectDal());

            //var dataPType = (from P in db.PROJECTS
            //                 join PT in db.PROJECT_TYPES on P.PROJECT_TYPE_ID equals PT.PROJECT_TYPE_ID
            //                 where P.IS_CANCELED == false && P.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId
            //                 select new
            //                 {
            //                     PROJECT_TYPE_ID = P.PROJECT_TYPE_ID,
            //                     PROJECT_TYPE_NAME = PT.PROJECT_TYPE_NAME
            //                 }
            //            ).OrderBy(o => o.PROJECT_TYPE_NAME).Distinct().ToList();


            //var dataStatus = (from P in db.PROJECTS
            //                  join S in db.STATUS on P.PROJECT_STATUS equals S.STATUS_ID
            //                  where P.IS_CANCELED == false && P.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId
            //                  select new
            //                  {
            //                      STATUS_ID = S.STATUS_ID,
            //                      STATUS_NAME = S.STATUS_NAME
            //                  }
            //).OrderBy(o => o.STATUS_NAME).Distinct().ToList();


            return Json(new { City = dataCity, Client = dataClient, Project = projectManager.GetAll(), Status = projectManager.GetAll() }, JsonRequestBehavior.AllowGet);
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

            ProjectManager projectManager = new ProjectManager(new ProjectDal());
            var control = projectManager.AddorUpdate(param);
          

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult DeleteJob(int ProjectID)
        {
            ProjectManager projectManager = new ProjectManager(new ProjectDal());
            var control = projectManager.Delete(new PROJECTS() {PROJECT_ID = ProjectID});

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
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
        private List<ProjectExtented> ProjectList(int  ProjectID,bool IsTemplate)
        {
            ProjectManager projectManager = new ProjectManager(new ProjectDal());
            var data = projectManager.GetAll();

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
            ProjectExtented newdata;
            var data = ProjectList(id,false);            
            if(data == null)
            {
                newdata = new ProjectExtented();                                
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

            ProjectManager projectManager = new ProjectManager(new ProjectDal());

            var ProjectProg = projectManager.Get(new PROJECTS() { PROJECT_ID = ProjectID });

           return Json(new { Phases = Phases, Members = Members, Notes= Notes, ProjectProg= ProjectProg.PROJECT_PROGRESS }, JsonRequestBehavior.AllowGet);
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

            ProjectTaskManager projectManager = new ProjectTaskManager(new ProjectTaskDal());
            var control = projectManager.AddorUpdate(param);            
            
            foreach (int EmpID in param.ASSINGED_MEMBERS_ARR)
            {
                PROJECT_MEMBERS member = new PROJECT_MEMBERS();
                member.PROJECT_ID = param.PROJECT_ID;
                member.PROJECT_PHASE_ID = param.PROJECT_PHASE_ID;
                member.PROJECT_TASK_ID = param.PROJECT_TASK_ID;
                member.PROJECT_MEMBER_ID = 0;
                member.EMPLOYEE_ID = EmpID;

                AddOrUpdateProjectMember(member);
            }

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        
        [HttpPost]
        public JsonResult RemoveTask(PROJECT_TASK param)
        {
            ProjectTaskManager projectManager = new ProjectTaskManager(new ProjectTaskDal());
            var control = projectManager.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
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
            ProjectMemberManager projectMemberManager = new ProjectMemberManager(new ProjectMemberDal());
            var control = projectMemberManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveMembers(PROJECT_MEMBERS param)
        {
            ProjectMemberManager projectMemberManager = new ProjectMemberManager(new ProjectMemberDal());
            var control = projectMemberManager.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
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
