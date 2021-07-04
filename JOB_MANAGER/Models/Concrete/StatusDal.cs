using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class StatusDal : EntityRepositoryBase<STATUS, JOB_MANAGER_DBEntities, StatusExtented>
    {
        public UserInfo UserInfo;
        public StatusDal(UserInfo _userInfo)
        {
            UserInfo = _userInfo;
        }

        public override List<StatusExtented> GetAll2(Expression<Func<STATUS, bool>> filter = null)
        {
            var _statuslist = filter != null ? context.Set<STATUS>().Where(filter).ToList()
                          : context.Set<STATUS>().ToList();

            var data = (from s in context.STATUS

                        join e in context.EMPLOYEES
                        on s.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()
                        //where s.STATUS_TYPE == StatusType
                        select new StatusExtented
                        {
                            STATUS_ID = s.STATUS_ID,
                            STATUS_NAME = s.STATUS_NAME,
                            DISPLAY_CLASS = s.DISPLAY_CLASS,
                            IS_CANCELED = s.IS_CANCELED,
                            CREATION_DATE = s.CREATION_DATE != null ?
                                                    s.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (s.CREATION_DATE.Month > 9 ? s.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + s.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    s.CREATION_DATE.Day : null,
                            MODIFIED_DATE = s.MODIFIED_DATE != null ?
                                                    s.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (s.MODIFIED_DATE.Month > 9 ? s.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + s.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    s.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
              ).OrderBy(o => o.STATUS_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(STATUS param, STATUS dbitem)
        {
            if (dbitem == null)
            {
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.STATUS_ID = dbitem.STATUS_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(STATUS entity, ShowState showState)
        {
            if (context.EMPLOYEES.Any(w => w.EMP_STATUS_ID == entity.STATUS_ID))
            {
                showState.ErrorMessage = "Employees are available to use this employee status!! Please try to check cancelled.";
                showState.isError = true;
            }

            if (!context.STATUS.Any(w => w.STATUS_ID == entity.STATUS_ID))
            {
                showState.ErrorMessage = "Status not found!!";
                showState.isError = true;
            }
        }
     
    }
}