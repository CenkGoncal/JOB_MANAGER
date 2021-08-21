using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class EmployeeDal : EntityRepositoryBase<EMPLOYEES, JOB_MANAGER_DBEntities, EmployeeExtended>
    {
        //public UserInfo UserInfo;

        //public EmployeeDal(UserInfo _userInfo)
        //{
        //    UserInfo = _userInfo;
        //}

        public override List<EmployeeExtended> GetAll2(Expression<Func<EMPLOYEES, bool>> filter = null)
        {
            var _emplist = filter != null ? context.Set<EMPLOYEES>().Where(filter).ToList()
                    : context.Set<EMPLOYEES>().ToList();

            var data = (from e in _emplist
                        join c in context.CONTRACT_TYPES on e.CONTRACT_TYPE_ID equals c.CONTRACT_TYPE_ID
                        join d in context.DEPARTMENTS on e.DEPARTMENT_ID equals d.DEPARTMENT_ID
                        join s in context.STATUS on e.EMP_STATUS_ID equals s.STATUS_ID
                        where e.IS_CANCELED == false && e.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId
                        select new EmployeeExtended
                        {
                            EMP_ID = e.EMP_ID,
                            EMP_IMG = e.EMP_IMG,
                            EMP_INITIALS = e.EMP_INITIALS,
                            EMP_NUMBER = e.EMP_NUMBER,
                            FIRST_NAME = e.FIRST_NAME,
                            LAST_NAME = e.LAST_NAME,
                            EMP_NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME,
                            TITLE_ID = e.TITLE_ID,
                            ROLE_ID = e.ROLE_ID,
                            EMP_TITLE = e.TITLES.TITLE_NAME,
                            CONTRACT_TYPE_ID = e.CONTRACT_TYPE_ID,
                            EMP_CONTRACT = c.CONTRACT_TYPE_CODE,
                            DEPARTMENT_ID = e.DEPARTMENT_ID,
                            EMP_DEPARTMENT = d.DEPARTMENT_NAME,
                            EMAIL_USERNAME = e.EMAIL_USERNAME,
                            EMP_STATUS_ID = e.EMP_STATUS_ID,
                            SYSTEM_USERNAME = e.SYSTEM_USERNAME,
                            SYSTEM_PASSWORD = e.SYSTEM_PASSWORD,
                            HIRED_DATE_STR = e.HIRED_DATE != null ?
                                                    e.HIRED_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (e.HIRED_DATE.Value.Month > 9 ? e.HIRED_DATE.Value.Month + SqlConstants.stringMinus : "0" + e.HIRED_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    e.HIRED_DATE.Value.Day : null,
                            EMP_STATUS = s.STATUS_NAME,
                            DISPLAY_CLASS = s.DISPLAY_CLASS,
                            EMP_PHONE = e.EMP_PHONE,
                            PHONE_EXTENSION = e.PHONE_EXTENSION,
                            MOBILE_PHONE = e.MOBILE_PHONE,
                            IS_SUPERVISOR = e.IS_SUPERVISOR,
                            IS_DRIVER = e.IS_DRIVER,
                            COMPANY_ID = e.COMPANY_ID,

                        }
                   ).OrderBy(o => o.EMP_NAME).ToList();

            foreach (var item in data)
            {
                item.SYSTEM_PASSWORD = GlobalTools.DecryptSystemString(item.SYSTEM_PASSWORD);
            }

            return data;
        }

        public override void DeleteControls(EMPLOYEES entity, ShowState showState)
        {
            if(!context.EMPLOYEES.Any(w=>w.EMP_ID == entity.EMP_ID))
            {
                showState.ErrorMessage = "Employee not found";
                showState.isError = true;
            }
        }

        public override void SaveHelper_ModifyBeforeSave(EMPLOYEES param, EMPLOYEES dbitem)
        {
            if(dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.COMPANY_ID = ThreadGlobals.UserAuthInfo.Value.CompanyId;
                param.SYSTEM_PASSWORD = GlobalTools.EncryptSystemString(param.SYSTEM_PASSWORD);
            }
            else
            {
                param.EMP_ID = dbitem.EMP_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.COMPANY_ID = dbitem.COMPANY_ID;

                param.TEAM_LEADER_ID = param.TEAM_LEADER_ID == 0 ? null : param.TEAM_LEADER_ID;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.MODIFIED_DATE = DateTime.Now;

                if (!string.IsNullOrEmpty(param.SYSTEM_PASSWORD))
                    param.SYSTEM_PASSWORD = GlobalTools.EncryptSystemString(param.SYSTEM_PASSWORD);
            }
        }
    }
}
