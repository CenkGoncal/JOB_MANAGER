using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            int CompanyID = ThreadGlobals.UserAuthInfo.Value.CompanyId;

            DasboradDto dasborad = new DasboradDto();
            dasborad.JobSummary = db.JOBSSUMMARY_VIEW.Where(w => w.COMPANY_ID == CompanyID || w.COUNT == SqlConstants.int0).ToList();
            dasborad.PhaseSummary = db.JOBSPHASESSUMMARY_VIEW.Where(w => w.COMPANY_ID == CompanyID || w.COUNT == SqlConstants.int0).ToList();

            ViewBag.JobTypesCount = db.PROJECT_TYPES.Where(w => w.IS_CANCELED == false).Count();

            return View(dasborad);
        }

        [HttpGet]
        public JsonResult MyTask(bool IsCompleted)
        {
            int EmpID = ThreadGlobals.UserAuthInfo.Value.UserId;
            var ProjectTasks = (from pm in db.PROJECT_MEMBER_VIEW
                                join pt in db.PROJECT_TASK on pm.TASK_ID equals pt.PROJECT_TASK_ID
                                join ps in db.PROJECT_PHASES on pt.PROJECT_PHASE_ID equals ps.PROJECT_PHASE_ID
                                join ds in db.DEF_PROJECT_PHASES on ps.PHASE_ID equals ds.PHASE_ID
                                join s in db.STATUS on pt.PROJECT_TASK_STATUS equals s.STATUS_ID
                                join p in db.PROJECTS on pt.PROJECT_ID equals p.PROJECT_ID
                                where pm.EMPLOYEE_ID == EmpID && pm.TASK_ID > SqlConstants.int0
                                select new
                                {
                                    PROJECT_ID = pm.PROJECT_ID,
                                    PROJECT_NAME = p.PROJECT_NAME,
                                    TASK_ID = pm.TASK_ID,
                                    PHASE_NAME = ds.PHASE_NAME,
                                    PROJECT_TASK_NAME = pm.TASK_NAME,
                                    TASK_START_DATE = pt.TASK_START_DATE != null ?
                                                     pt.TASK_START_DATE.Value.Year + SqlConstants.stringMinus +
                                                     (pt.TASK_START_DATE.Value.Month > 9 ? pt.TASK_START_DATE.Value.Month + SqlConstants.stringMinus : "0" + pt.TASK_START_DATE.Value.Month + SqlConstants.stringMinus) +
                                                     pt.TASK_START_DATE.Value.Day : null,
                                    TASK_ORDER = pt.TASK_ORDER,
                                    PROJECT_TASK_PROGRESS = pt.PROJECT_TASK_PROGRESS,
                                    STATUS_ID = s.STATUS_ID,
                                    STATUS_NAME = s.STATUS_NAME,
                                    DISPLAY_CLASS = s.DISPLAY_CLASS
                                }
                               ).ToList();

            int ComplatedStatusID = GlobalTools.GetParamIntVal("PHASE_OR_TASK_COMPLATED_STATUS_ID", GetCompanyID());
            if(ComplatedStatusID > 0)
            {
                if (IsCompleted)
                {
                    ProjectTasks = ProjectTasks.Where(w => w.STATUS_ID == ComplatedStatusID).ToList();
                }
                else
                {
                    ProjectTasks = ProjectTasks.Where(w => w.STATUS_ID != ComplatedStatusID).ToList();
                }
            }

            return Json(new { Getlist = ProjectTasks }, JsonRequestBehavior.AllowGet);
        }

        [AuthorityControl("Calendar")]
        [HttpGet]
        public ActionResult Calendar()
        {
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

            ViewBag.StatusList = data;

            int CompanyID = GetCompanyID();
            ViewBag.PROJECT_TYPE_LIST = new SelectList(db.PROJECT_TYPES.Where(w => w.IS_CANCELED == false), "PROJECT_TYPE_ID", "PROJECT_TYPE_NAME");
            ViewBag.CLIENT_LIST = new SelectList(db.CLIENTS.Where(w => w.IS_CANCELED == false), "CLIENT_ID", "CLIENT_NAME");
            ViewBag.MATERIAL_LIST = new SelectList(db.MATERIALS.Where(w => w.IS_CANCELED == false), "MATERIAL_ID", "MATERIAL_NAME");            
            ViewBag.FLOOR_LIST = new SelectList(db.FLOOR_TYPES.Where(w => w.IS_CANCELED == false), "FLOOR_TYPE_ID", "FLOOR_TYPE_NAME");
            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
