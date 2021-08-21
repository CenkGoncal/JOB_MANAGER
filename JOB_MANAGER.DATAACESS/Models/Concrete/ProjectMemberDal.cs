using System;
using System.Linq;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class ProjectMemberDal : EntityRepositoryBase<PROJECT_MEMBERS, JOB_MANAGER_DBEntities, ProjectMemberExtented>
    {
        public override void SaveControls(PROJECT_MEMBERS entity, ShowState showState)
        {
            PROJECT_MEMBERS control = context.PROJECT_MEMBERS.Where(w => w.PROJECT_MEMBER_ID == entity.PROJECT_MEMBER_ID).FirstOrDefault();

            if (control == null)
            {
                PROJECT_MEMBERS control2 = context.PROJECT_MEMBERS.Where(w => w.EMPLOYEE_ID == entity.EMPLOYEE_ID && w.PROJECT_ID == entity.PROJECT_ID).FirstOrDefault();
                if (control2 != null)
                {

                    //else
                    if (control2.IS_CANCELED == false && control2.PROJECT_MEMBER_ID != entity.PROJECT_MEMBER_ID)
                    {
                        showState.isError = false;
                        showState.ErrorMessage = "This Member has already been added";
                    }
                }

            }
            else
            {
                var control2 = context.PROJECT_MEMBERS.Where(w => w.EMPLOYEE_ID == entity.EMPLOYEE_ID && w.PROJECT_ID == entity.PROJECT_ID && w.PROJECT_MEMBER_ID != entity.PROJECT_MEMBER_ID
                                                   && w.IS_CANCELED == false
                              ).FirstOrDefault();

                if (control2 != null)
                {
                    showState.isError = false;
                    showState.ErrorMessage = "This Member has already been added";
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(PROJECT_MEMBERS param, PROJECT_MEMBERS dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;

                if (param.IS_CANCELED == true)
                {
                    param.IS_CANCELED = false;
                }
            }
            else
            {
                param.MODIFIED_DATE = dbitem.MODIFIED_DATE;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.PROJECT_MEMBER_ID = dbitem.PROJECT_MEMBER_ID;
                param.CREATED_BY = dbitem.CREATED_BY;

                if (param.IS_CANCELED == true)
                {
                    param.IS_CANCELED = false;
                }

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(PROJECT_MEMBERS entity, ShowState showState)
        {
            if (!context.PROJECT_MEMBERS.Any(x => x.PROJECT_MEMBER_ID == entity.PROJECT_MEMBER_ID))
            {
                showState.ErrorMessage = "Project Member not exists.";
                showState.isError = true;
            }
        }
    }
}
