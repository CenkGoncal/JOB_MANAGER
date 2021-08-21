using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class ParameterDal : EntityRepositoryBase<PARAMETERS, JOB_MANAGER_DBEntities, ParameterExtented>
    { 
        public override List<ParameterExtented> GetAll2(Expression<Func<PARAMETERS, bool>> filter = null)
        {
            var _parametersTypes = filter != null ? context.Set<PARAMETERS>().Where(filter).ToList()
                            : context.Set<PARAMETERS>().ToList();

            var data = (from p in _parametersTypes
                        join e in context.EMPLOYEES
                        on p.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new ParameterExtented
                        {
                            PARAM_NAME = p.PARAM_NAME,
                            PARAM_STR = p.PARAM_STR,
                            PARAM_IMG = p.PARAM_IMG,
                            PARAM_INT = p.PARAM_INT,
                            COMPANY_ID = p.COMPANY_ID,
                            CREATION_DATE = p.CREATION_DATE != null ?
                                                    p.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (p.CREATION_DATE.Month > 9 ? p.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + p.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    p.CREATION_DATE.Day : null,
                            MODIFIED_DATE = p.MODIFIED_DATE != null ?
                                                    p.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (p.MODIFIED_DATE.Month > 9 ? p.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + p.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    p.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME

                        }
                       ).OrderBy(o => o.PARAM_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(PARAMETERS param, PARAMETERS dbitem)
        {
            if (dbitem == null)
            {
                param.COMPANY_ID = -1;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.COMPANY_ID = -1;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.IDENTID = dbitem.IDENTID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }
    }
}
