using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class CompanyTypeDal : EntityRepositoryBase<COMPANY_TYPES, JOB_MANAGER_DBEntities, CompanyTypesExtented>
    {        
        public UserInfo UserInfo;
        public CompanyTypeDal(UserInfo _userInfo)
        {          
            UserInfo = _userInfo;
        }

        public override List<CompanyTypesExtented> GetAll2(Expression<Func<COMPANY_TYPES, bool>> filter = null)
        {
            var _contactTypes = filter != null ? context.Set<COMPANY_TYPES>().Where(filter).ToList()
                            : context.Set<COMPANY_TYPES>().ToList();

            var data = (from ct in context.COMPANY_TYPES

                        join e in context.EMPLOYEES
                        on ct.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new CompanyTypesExtented
                        {
                            COMPANY_TYPE_ID = ct.COMPANY_TYPE_ID,
                            COMPANY_TYPE_NAME = ct.COMPANY_TYPE_NAME,
                            COMPANY_TYPE_DESC = ct.COMPANY_TYPE_DESC,
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
                   ).OrderBy(o => o.COMPANY_TYPE_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(COMPANY_TYPES param, COMPANY_TYPES dbitem)
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
                param.COMPANY_TYPE_ID = dbitem.COMPANY_TYPE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(COMPANY_TYPES entity, ShowState showState)
        {
            var companyType = context.COMPANY.Where(w => w.COMPANY_TYPE_ID == entity.COMPANY_TYPE_ID).FirstOrDefault();
            if (companyType != null)
            {
                showState.ErrorMessage = "Company Type is available to use company!! Please try to check cancelled";
                showState.isError = true;
            }

            if(!context.COMPANY_TYPES.Any(w => w.COMPANY_TYPE_ID == entity.COMPANY_TYPE_ID))
            {
                showState.ErrorMessage = "Company Type not found!!";
                showState.isError = true;
            }
        }

    }
}