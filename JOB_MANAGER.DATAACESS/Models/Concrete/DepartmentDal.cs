using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class DepartmentDal : EntityRepositoryBase<DEPARTMENTS, JOB_MANAGER_DBEntities, DepartmentExtented>
    {        
      
        public override List<DepartmentExtented> GetAll2(Expression<Func<DEPARTMENTS, bool>> filter = null)
        {
            List<DepartmentExtented> data = new List<DepartmentExtented>();

            ShowState showState = new ShowState();


            showState = base.HandleExepciton(() => { 
                var _department = filter != null ? context.Set<DEPARTMENTS>().Where(filter).ToList()
                         : context.Set<DEPARTMENTS>().ToList();

                 data = (from dt in _department

                            from e_left in context.EMPLOYEES.Where(w => w.EMP_ID == dt.CREATED_BY).DefaultIfEmpty()

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
                                CREATE_BY = e_left == null ? e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME : SqlConstants.stringEmpty
                            }
                     ).OrderBy(o => o.DEPARTMENT_NAME).ToList();
                
            }, showState);

            return data;
        }

        
        public override void SaveHelper_ModifyBeforeSave(DEPARTMENTS param, DEPARTMENTS dbitem)
        {
            if (dbitem == null)
            {
                param.COMPANY_ID = ThreadGlobals.UserAuthInfo.Value.CompanyId;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.COMPANY_ID = dbitem.COMPANY_ID;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.DEPARTMENT_ID = dbitem.DEPARTMENT_ID;
                
                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(DEPARTMENTS entity, ShowState showState)
        {
            var checkEmployee = context.EMPLOYEES.Where(w => w.DEPARTMENT_ID == entity.DEPARTMENT_ID).FirstOrDefault();
            if (checkEmployee != null)
            {
                showState.ErrorMessage = "Departmant is available to use this department!! Please try to check cancelled.";
                showState.isError = true;
            }

            if (!context.DEPARTMENTS.Any(w => w.DEPARTMENT_ID == entity.DEPARTMENT_ID))
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
