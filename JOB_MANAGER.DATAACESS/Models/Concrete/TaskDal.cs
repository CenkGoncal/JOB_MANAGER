using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class TaskDal : EntityRepositoryBase<DEF_TASKS, JOB_MANAGER_DBEntities, DefTaskExtented>
    {
        public override void SaveControls(DEF_TASKS entity, ShowState showState)
        {
            DEF_TASKS control = context.DEF_TASKS.Where(w => w.TASK_ID == entity.TASK_ID).FirstOrDefault();
            
            if (control == null)
            {
                if (context.DEF_TASKS.Any(x => x.TASK_NAME.Equals(entity.TASK_NAME) && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Task already exists.";
                    showState.isError = true;
                }
                if (context.DEF_TASKS.Any(x => x.TASK_ORDER == entity.TASK_ORDER && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Order already exists.";
                    showState.isError = true;
                }
            }
            else
            {
                if (context.DEF_TASKS.Any(x => x.TASK_NAME.Equals(entity.TASK_NAME) && x.TASK_ID != entity.TASK_ID && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Task already exists.";
                    showState.isError = true;
                }
                if (context.DEF_TASKS.Any(x => x.TASK_ORDER == entity.TASK_ORDER && x.TASK_ID != entity.TASK_ID && x.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Order already exists.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(DEF_TASKS param, DEF_TASKS dbitem)
        {            

            if (dbitem == null)
            {
                int ORDER = context.DEF_TASKS.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && w.PHASE_ID == param.PHASE_ID).Select(s => s.PHASE_ID).ToList().Count() + 1;
                param.TASK_ORDER = (short)ORDER;

                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.PHASE_ID = dbitem.PHASE_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(DEF_TASKS entity, ShowState showState)
        {
            if( !context.DEF_TASKS.Any(w => w.TASK_ID == entity.TASK_ID))
            {
                showState.ErrorMessage = "Task not found!!";
                showState.isError = true;
            }
        }

        public override List<DefTaskExtented> GetAll2(Expression<Func<DEF_TASKS, bool>> filter = null)
        {
            var _tasklist = filter != null ? context.Set<DEF_TASKS>().Where(filter).ToList()
                    : context.Set<DEF_TASKS>().ToList();

            var data = (from T in _tasklist
                        from E in context.EMPLOYEES.Where(w => w.EMP_ID == T.CREATED_BY).DefaultIfEmpty()
                          //where T.PROJECT_TYPE_ID == item.PROJECT_TYPE_ID && T.PHASE_ID == phase.PHASE_ID
                          select new DefTaskExtented
                          {
                              TASK_ID = T.TASK_ID,
                              TASK_NAME = T.TASK_NAME,
                              TASK_DESC = T.TASK_DESC,
                              PROJECT_TYPE_ID = T.PROJECT_TYPE_ID,
                              PHASE_ID = T.PHASE_ID,
                              TASK_ORDER = T.TASK_ORDER,
                              ALLOCATE_SUPERVISOR = T.ALLOCATE_SUPERVISOR,
                              COMPLETE_ON_UPLOAD = T.COMPLETE_ON_UPLOAD,
                              IS_CANCELED = T.IS_CANCELED,
                              CREATION_DATE = T.CREATION_DATE != null ?
                                                          T.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                          (T.CREATION_DATE.Month > SqlConstants.int9 ? T.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + T.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                          T.CREATION_DATE.Day : null,
                              MODIFIED_DATE = T.MODIFIED_DATE != null ?
                                                          T.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                          (T.MODIFIED_DATE.Month > SqlConstants.int9 ? T.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + T.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                          T.MODIFIED_DATE.Day : null,
                              CREATE_BY = E.FIRST_NAME + SqlConstants.stringWhiteSpace + E.LAST_NAME
                          }
                      ).OrderBy(o => o.TASK_NAME).ToList();

            return data;
        }
    }
}
