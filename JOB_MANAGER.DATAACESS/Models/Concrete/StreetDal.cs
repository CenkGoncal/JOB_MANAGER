using JOB_MANAGER.DATAACESS.Helper;
using JOB_MANAGER.DATAACESS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class StreetDal : EntityRepositoryBase<STREET, JOB_MANAGER_DBEntities, StreetExtented>
    {
        
     
        public override List<StreetExtented> GetAll2(Expression<Func<STREET, bool>> filter = null)
        {
            var data = (from ct in context.COUNTRIES

                        join e in context.EMPLOYEES
                        on ct.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()
                        select new StreetExtented
                        {
                            COUNTRY_NAME = ct.COUNTRY_NAME,
                            COUNTRY_CODE = ct.COUNTRY_CODE,
                            FLAG_URL = ct.FLAG_URL,
                            COUNTRY_ID = ct.COUNTRY_ID,
                            IS_DEFAULT = ct.IS_DEFAULT,
                            IS_CANCELED = ct.IS_CANCELED,
                            CREATION_DATE = ct.CREATION_DATE != null ?
                                                    ct.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (ct.CREATION_DATE.Month > 9 ? ct.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + ct.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    ct.CREATION_DATE.Day : null,
                            MODIFIED_DATE = ct.MODIFIED_DATE != null ?
                                                    ct.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (ct.MODIFIED_DATE.Month > 9 ? ct.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + ct.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    ct.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                    ).OrderBy(o => o.COUNTRY_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(STREET param, STREET dbitem)
        {
            if (dbitem == null)
            {                
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.STREET_ID = dbitem.STREET_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(STREET entity, ShowState showState)
        {
            if(!context.STREET.Any(w => w.STREET_ID == entity.STREET_ID))
            {
                showState.isError = true;
                showState.ErrorMessage = "Street not found";
            }
        }
    }
}
