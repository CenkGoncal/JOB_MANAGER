using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class ProjectTypeDal : EntityRepositoryBase<PROJECT_TYPES, JOB_MANAGER_DBEntities, ProjectTypeExtented>
    {
    

        public override List<ProjectTypeExtented> GetAll2(Expression<Func<PROJECT_TYPES, bool>> filter = null)
        {

            var _projectTypeList = filter != null ? context.Set<PROJECT_TYPES>().Where(filter).ToList()
               : context.Set<PROJECT_TYPES>().ToList();

            var data = (from vb in _projectTypeList

                        join e in context.EMPLOYEES
                        on vb.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new ProjectTypeExtented
                        {
                            PROJECT_TYPE_ID = vb.PROJECT_TYPE_ID,
                            PROJECT_TYPE_NAME = vb.PROJECT_TYPE_NAME,
                            PROJECT_TYPE_DESC = vb.PROJECT_TYPE_DESC,
                            DEFAULT_SUPERVISOR = vb.DEFAULT_SUPERVISOR,
                            IS_CANCELED = vb.IS_CANCELED,
                            CREATION_DATE = vb.CREATION_DATE != null ?
                                                    vb.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (vb.CREATION_DATE.Month > 9 ? vb.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + vb.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    vb.CREATION_DATE.Day : null,
                            MODIFIED_DATE = vb.MODIFIED_DATE != null ?
                                                    vb.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (vb.MODIFIED_DATE.Month > 9 ? vb.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + vb.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    vb.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                       ).OrderBy(o => o.PROJECT_TYPE_NAME).ToList();

            return data;
        }

        public override void SaveControls(PROJECT_TYPES entity, ShowState showState)
        {
            PROJECT_TYPES control = context.PROJECT_TYPES.Where(w => w.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID).FirstOrDefault();            
            if (control == null)
            {
                if (context.PROJECT_TYPES.Any(x => x.PROJECT_TYPE_NAME.Equals(entity.PROJECT_TYPE_NAME) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = true;
                }

                if (entity.IS_DEFAULT)
                {
                    if (context.PROJECT_TYPES.Any(x => x.IS_DEFAULT == true))
                    {
                        showState.ErrorMessage = "Only one type can be set as default";
                        showState.isError = true;
                    }
                }
            }
            else
            {
                if (context.PROJECT_TYPES.Any(x => x.PROJECT_TYPE_NAME.Equals(entity.PROJECT_TYPE_NAME) && x.PROJECT_TYPE_ID != entity.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = true;
                }

                if (entity.IS_DEFAULT)
                {
                    if (context.PROJECT_TYPES.Any(x => x.IS_DEFAULT == true && x.PROJECT_TYPE_ID != entity.PROJECT_TYPE_ID))
                    {
                        showState.ErrorMessage = "Only one type can be set as default";
                        showState.isError = true;
                    }
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(PROJECT_TYPES param, PROJECT_TYPES dbitem)
        {
            if(dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.PROJECT_TYPE_ID = dbitem.PROJECT_TYPE_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(PROJECT_TYPES entity, ShowState showState)
        {            
            if (context.QUOTES.Any(w => w.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID))
            {
                showState.ErrorMessage = "Quonte is available to use Project Type!! Please try to check cancelled";
                showState.isError = true;
            }
            else 
            if(!context.PROJECT_TYPES.Any(w=>w.PROJECT_TYPE_ID == entity.PROJECT_TYPE_ID))
            {
                showState.ErrorMessage = "Vehicle Body Type not found!!";
                showState.isError = true;
            }
        }
    }
}
