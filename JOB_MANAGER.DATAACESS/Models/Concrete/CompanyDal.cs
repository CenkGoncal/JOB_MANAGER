using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class CompanyDal : EntityRepositoryBase<COMPANY, JOB_MANAGER_DBEntities, CompanyExtented>
    {
        public override List<CompanyExtented> GetAll2(Expression<Func<COMPANY, bool>> filter = null)
        {
            var _companylist = filter != null ? context.Set<COMPANY>().Where(filter).ToList()
                : context.Set<COMPANY>().ToList();

            return (from c in _companylist
                    where c.IS_CANCELED == false
                    select new CompanyExtented
                    {
                        COMPANY_ID = c.COMPANY_ID,
                        COMPANY_CODE = c.COMPANY_CODE,
                        COMPANY_NAME = c.COMPANY_NAME,
                        COMPANY_TYPE_ID = c.COMPANY_TYPE_ID,
                        COMPANY_COUNTRY = c.COMPANY_COUNTRY,
                        COMPANY_STATE = c.COMPANY_STATE,
                        COMPANY_CITY = c.COMPANY_CITY,
                        COMPANY_ADDRESS = c.COMPANY_ADDRESS,
                        POSTAL_CODE = c.POSTAL_CODE,
                        WEB_URL = c.WEB_URL,
                        COMPANY_ABN = c.COMPANY_ABN,
                        COMPANY_PHONE = c.COMPANY_PHONE,
                        COMPANY_OWNER = c.COMPANY_OWNER,
                        COMPANY_LINKED_IN = c.COMPANY_LINKED_IN,
                        COMPANY_LOGO = c.COMPANY_LOGO,
                        COMPANY_DESC = c.COMPANY_DESC
                    }
                       ).OrderBy(o => o.COMPANY_NAME).ToList();
        }

        public override void SaveHelper_ModifyBeforeSave(COMPANY param, COMPANY dbitem)
        {
            if (dbitem == null)
            {
                param.COMPANY_ID = ThreadGlobals.UserAuthInfo.Value.CompanyId;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.COMPANY_ID = dbitem.COMPANY_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }
    }
}
