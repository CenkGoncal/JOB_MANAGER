using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class DepartmentDal : EntityRepositoryBase<DEPARTMENTS, JOB_MANAGER_DBEntities, DepartmentExtented>
    {
        public JOB_MANAGER_DBEntities db;
        public GlobalTools.UserInfo UserInfo;

        public DepartmentDal(GlobalTools.UserInfo _userInfo)
        {
            db = new JOB_MANAGER_DBEntities();
            UserInfo = _userInfo;
        }


        public override List<DepartmentExtented> GetAll2(Expression<Func<DEPARTMENTS, bool>> filter = null)
        {
            var _department = filter != null ? db.Set<DEPARTMENTS>().Where(filter).ToList()
                     : db.Set<DEPARTMENTS>().ToList();

            var data = (from dt in _department

                        join e in db.EMPLOYEES
                        on dt.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()
                        select new DepartmentExtented
                        {
                            DEPARTMENT_ID = dt.DEPARTMENT_ID,
                            DEPARTMENT_NAME = dt.DEPARTMENT_NAME,
                            IS_CANCELED = dt.IS_CANCELED,
                            CREATION_DATE = dt.CREATION_DATE != null ?
                                                    dt.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (dt.CREATION_DATE.Month > 9 ? dt.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + dt.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    dt.CREATION_DATE.Day : null,
                            MODIFIED_DATE = dt.MODIFIED_DATE != null ?
                                                    dt.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (dt.MODIFIED_DATE.Month > 9 ? dt.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + dt.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    dt.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                 ).OrderBy(o => o.DEPARTMENT_NAME).ToList();

            return data;
        }

        
        public override void SaveHelper_ModifyBeforeSave(DEPARTMENTS param, DEPARTMENTS dbitem)
        {
            if (dbitem == null)
            {
                param.COMPANY_ID = UserInfo.CompanyId;
                param.CREATED_BY = param.UPDATED_BY =  UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.COMPANY_ID = dbitem.COMPANY_ID;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.CREATED_BY = dbitem.CREATED_BY;
                
                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(DEPARTMENTS entity, ShowState showState)
        {
            var checkEmployee = db.EMPLOYEES.Where(w => w.DEPARTMENT_ID == entity.DEPARTMENT_ID).FirstOrDefault();
            if (checkEmployee != null)
            {
                showState.ErrorMessage = "Departmant is available to use this department!! Please try to check cancelled.";
                showState.isError = true;
            }

            if (!db.DEPARTMENTS.Any(w => w.DEPARTMENT_ID == entity.DEPARTMENT_ID))
            {
                showState.ErrorMessage = "Departmant not found!!";
                showState.isError = true;
            }
        }

        public override void SaveControls(DEPARTMENTS entity, ShowState showState)
        {
            base.SaveControls(entity, showState);
        }
    }
}