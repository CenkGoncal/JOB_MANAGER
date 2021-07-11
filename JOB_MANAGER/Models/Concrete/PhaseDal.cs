using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class PhaseDal : EntityRepositoryBase<DEF_PROJECT_PHASES, JOB_MANAGER_DBEntities, DefProjectPhaseExtented>
    {
        public UserInfo UserInfo;

        public PhaseDal(UserInfo _userInfo)
        {
            UserInfo = _userInfo;
        }

        public override void SaveControls(DEF_PROJECT_PHASES entity, ShowState showState)
        {
            DEF_PROJECT_PHASES control = context.DEF_PROJECT_PHASES.Where(w => w.PHASE_ID == entity.PHASE_ID).FirstOrDefault();
            
            if (control == null)
            {
                if (context.DEF_PROJECT_PHASES.Any(x => x.PHASE_NAME.Equals(entity.PHASE_NAME) && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Phase already exists.";
                    showState.isError = true;
                }
                if (context.DEF_PROJECT_PHASES.Any(x => x.PHASE_ORDER == entity.PHASE_ORDER && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Order already exists.";
                    showState.isError = true;
                }
            }
            else
            {
                if (context.DEF_PROJECT_PHASES.Any(x => x.PHASE_NAME.Equals(entity.PHASE_NAME) && x.PHASE_ID != entity.PHASE_ID && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Phase already exists.";
                    showState.isError = true;
                }
                if (context.DEF_PROJECT_PHASES.Any(x => x.PHASE_ORDER == entity.PHASE_ORDER && x.PHASE_ID != entity.PHASE_ID && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Order already exists.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(DEF_PROJECT_PHASES param, DEF_PROJECT_PHASES dbitem)
        {
            param.CORRESPONDANT_STATUS = param.CORRESPONDANT_STATUS == 0 ? null : param.CORRESPONDANT_STATUS;

            if (dbitem == null)
            {
                int ORDER = context.DEF_PROJECT_PHASES.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID).Select(s => s.PHASE_ID).ToList().Count() + 1;
                param.PHASE_ORDER = (short)ORDER;

                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
            }
            else
            {
                param.PHASE_ID = dbitem.PHASE_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
        }

        public override void DeleteControls(DEF_PROJECT_PHASES entity, ShowState showState)
        {
            if( !context.DEF_PROJECT_PHASES.Any(w => w.PHASE_ID == entity.PHASE_ID))
            {
                showState.ErrorMessage = "Phase not found!!";
                showState.isError = true;
            }
        }

        public override List<DefProjectPhaseExtented> GetAll2(Expression<Func<DEF_PROJECT_PHASES, bool>> filter = null)
        {
            var data = (from PH in context.DEF_PROJECT_PHASES
                           from E in context.EMPLOYEES.Where(w => w.EMP_ID == PH.CREATED_BY).DefaultIfEmpty()
                           //where PH.PROJECT_TYPE_ID == item.PROJECT_TYPE_ID
                           select new DefProjectPhaseExtented
                           {
                               PHASE_ID = PH.PHASE_ID,
                               PHASE_NAME = PH.PHASE_NAME,
                               PROJECT_TYPE_ID = PH.PROJECT_TYPE_ID,
                               PHASE_ORDER = PH.PHASE_ORDER,
                               PHASE_DESC = PH.PHASE_DESC,
                               PHASE_COLOR = PH.PHASE_COLOR,
                               WIDGET_COLOR = PH.WIDGET_COLOR,
                               CORRESPONDANT_STATUS = PH.CORRESPONDANT_STATUS,
                               IS_CANCELED = PH.IS_CANCELED,
                               CREATION_DATE = PH.CREATION_DATE != null ?
                                                           PH.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                           (PH.CREATION_DATE.Month > SqlConstants.int9 ? PH.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + PH.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                           PH.CREATION_DATE.Day : null,
                               MODIFIED_DATE = PH.MODIFIED_DATE != null ?
                                                           PH.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                           (PH.MODIFIED_DATE.Month > SqlConstants.int9 ? PH.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + PH.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                           PH.MODIFIED_DATE.Day : null,
                               CREATE_BY = E.FIRST_NAME + SqlConstants.stringWhiteSpace + E.LAST_NAME
                           }
                           ).OrderBy(o => o.PHASE_ORDER).ToList();

            return data;
        }

        public List<ProjectTypeWithPhaseTask> GetProjectTypeWithPhaseTasks()
        {
            ProjectTypeDal projectType = new ProjectTypeDal(UserInfo);
            TaskDal taskDal = new TaskDal(UserInfo);
         
            var ProjectTypes = projectType.GetAll();

            List<ProjectTypeWithPhaseTask> PhaseList = new List<ProjectTypeWithPhaseTask>();

            foreach (var item in ProjectTypes)
            {
                ProjectTypeWithPhaseTask Phase = new ProjectTypeWithPhaseTask();

                Phase.PROJECT_TYPE_ID = item.PROJECT_TYPE_ID;
                Phase.PROJECT_TYPE_NAME = item.PROJECT_TYPE_NAME;
                Phase.PHASE = GetAll2(f => f.PROJECT_TYPE_ID == item.PROJECT_TYPE_ID);

                foreach (var phase in Phase.PHASE)
                {
                    phase.TASK = taskDal.GetAll2(f => f.PROJECT_TYPE_ID == item.PROJECT_TYPE_ID && f.PHASE_ID == phase.PHASE_ID);
                    phase.TASK = phase.TASK.OrderBy(o => o.TASK_NAME).ToList();               
                }

                PhaseList.Add(Phase);
            }

            return PhaseList;
        }

    }
}
