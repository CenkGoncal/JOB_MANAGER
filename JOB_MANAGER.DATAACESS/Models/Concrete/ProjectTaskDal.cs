using System;
using System.Linq;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class ProjectTaskDal : EntityRepositoryBase<PROJECT_TASK, JOB_MANAGER_DBEntities, HolidayExtented>
    {
        public override void DeleteControls(PROJECT_TASK entity, ShowState showState)
        {
            if (!context.PROJECT_TASK.Any(x => x.PROJECT_TASK_ID == entity.PROJECT_TASK_ID))
            {
                showState.ErrorMessage = "Project Task not exists.";
                showState.isError = true;
            }
        }


        public override void SaveControls(PROJECT_TASK entity, ShowState showState)
        {
            PROJECT_TASK control = context.PROJECT_TASK.Where(w => w.PROJECT_TASK_ID == entity.PROJECT_TASK_ID).FirstOrDefault();

            if (control == null)
            {
                if (context.PROJECT_TASK.Any(x => x.PROJECT_TASK_NAME.Equals(entity.PROJECT_TASK_NAME) && x.PROJECT_ID == entity.PROJECT_ID && x.PROJECT_PHASE_ID == entity.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Task Name already exists.";
                    showState.isError = true;
                }
                if (context.PROJECT_TASK.Any(x => x.TASK_ORDER == entity.TASK_ORDER && x.PROJECT_ID == entity.PROJECT_ID && x.PROJECT_PHASE_ID == entity.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Order already exists in the same phase.";
                    showState.isError = true;
                }
            }
            else
            {
                if (context.PROJECT_TASK.Any(x => x.PROJECT_TASK_ID != entity.PROJECT_TASK_ID && x.PROJECT_TASK_NAME.Equals(entity.PROJECT_TASK_NAME) && x.PROJECT_ID == entity.PROJECT_ID && x.PROJECT_PHASE_ID == entity.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Task Name already exists.";
                    showState.isError = true;
                }
                if (context.PROJECT_TASK.Any(x => x.PROJECT_TASK_ID != entity.PROJECT_TASK_ID && x.TASK_ORDER == entity.TASK_ORDER && x.PROJECT_ID == entity.PROJECT_ID && x.PROJECT_PHASE_ID == entity.PROJECT_PHASE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Order already exists in the same phase.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(PROJECT_TASK param, PROJECT_TASK dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.MODIFIED_DATE = dbitem.MODIFIED_DATE;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.PROJECT_TASK_ID = dbitem.PROJECT_TASK_ID;
                param.CREATED_BY = dbitem.CREATED_BY;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

    }
}
