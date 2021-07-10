using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class RoleDal: EntityRepositoryBase<ROLES, JOB_MANAGER_DBEntities, RolesExtented>
    {        
        public UserInfo UserInfo;

        public RoleDal(UserInfo _userInfo)
        {            
            UserInfo = _userInfo;
        }

        public override void SaveHelper_ModifyBeforeSave(ROLES param, ROLES dbitem)
        {
            if (dbitem == null)
            {
                param.COMPANY_ID = UserInfo.CompanyId;
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.COMPANY_ID = dbitem.COMPANY_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.ROLE_ID = dbitem.ROLE_ID;
                
                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(ROLES entity, ShowState showState)
        {
            var checkEmployee = context.EMPLOYEES.Where(w => w.ROLE_ID == entity.ROLE_ID).FirstOrDefault();
            if (checkEmployee != null)
            {
                showState.ErrorMessage = "Employees are available to use this contact roles!! Please try to check cancelled.";
                showState.isError = true;
            }

            if(!context.ROLES.Any(w => w.ROLE_ID == entity.ROLE_ID))
            {
                showState.ErrorMessage = "Role not found!!";
                showState.isError = true;
            }
        }

        public override List<RolesExtented> GetAll2(Expression<Func<ROLES, bool>> filter = null)
        {
            var _roles = filter != null ? context.Set<ROLES>().Where(filter).ToList()
                         : context.Set<ROLES>().ToList();

            var data = (from r in _roles

                        from e_left in context.EMPLOYEES.Where(w=>w.EMP_ID == r.CREATED_BY).DefaultIfEmpty()

                        select new RolesExtented
                        {
                            ROLE_ID = r.ROLE_ID,
                            ROLE_NAME = r.ROLE_NAME,
                            IS_CANCELED = r.IS_CANCELED,
                            CREATION_DATE = r.CREATION_DATE != null ?
                                                    r.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (r.CREATION_DATE.Month > 9 ? r.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + r.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    r.CREATION_DATE.Day : null,
                            MODIFIED_DATE = r.MODIFIED_DATE != null ?
                                                    r.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (r.MODIFIED_DATE.Month > 9 ? r.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + r.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    r.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left != null ? e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME : SqlConstants.stringEmpty
                        }
                        ).OrderBy(o => o.ROLE_NAME).ToList();

            return data;
        }
    }
}
