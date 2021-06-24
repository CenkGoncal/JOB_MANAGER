using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class ContractTypeDal:EntityRepositoryBase<CONTRACT_TYPES, JOB_MANAGER_DBEntities, ContractTypesExtented>
    {        
        public GlobalTools.UserInfo UserInfo;
        public ContractTypeDal(GlobalTools.UserInfo _userInfo)
        {
            UserInfo = _userInfo;
        }
    
        public override void SaveControls(CONTRACT_TYPES entity,ShowState showState)
        {            
            if (!context.CONTRACT_TYPES.Any(w => w.CONTRACT_TYPE_ID == entity.CONTRACT_TYPE_ID))
            {
                if (context.CONTRACT_TYPES.Any(x => x.CONTRACT_TYPE_NAME.Equals(entity.CONTRACT_TYPE_NAME) && x.IS_CANCELED == false && (x.COMPANY_ID == -1 || x.COMPANY_ID == UserInfo.CompanyId)))
                {
                    showState.ErrorMessage = "Type already exists";
                    showState.isError = true;
                }
            }
            else
            {
                if (context.CONTRACT_TYPES.Any(x => x.CONTRACT_TYPE_NAME.Equals(entity.CONTRACT_TYPE_NAME) && x.CONTRACT_TYPE_ID != entity.CONTRACT_TYPE_ID && x.IS_CANCELED == false && (x.COMPANY_ID == -1 || x.COMPANY_ID == UserInfo.CompanyId)))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = false;
                }
            }

            //base.SaveControls(entity, showState);
        }

        public override void SaveHelper_ModifyBeforeSave(CONTRACT_TYPES param, CONTRACT_TYPES dbitem)
        {            
            if (dbitem == null)
            {
                param.COMPANY_ID = UserInfo.CompanyId;
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE =  DateTime.Now;
            }
            else
            {
                param.COMPANY_ID = UserInfo.CompanyId;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.CONTRACT_TYPE_ID = dbitem.CONTRACT_TYPE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(CONTRACT_TYPES entity, ShowState showState)
        {
            var checkEmployee = context.EMPLOYEES.Where(w => w.CONTRACT_TYPE_ID == entity.CONTRACT_TYPE_ID).FirstOrDefault();
            if (checkEmployee != null)
            {
                showState.ErrorMessage = "Employees are available to use this contact types!! Please try to check cancelled.";
                showState.isError = true;
            }
        }

        public override List<ContractTypesExtented> GetAll2(Expression<Func<CONTRACT_TYPES, bool>> filter = null)
        {
            var _contactTypes = filter != null ? context.Set<CONTRACT_TYPES>().Where(filter).ToList()
                            : context.Set<CONTRACT_TYPES>().ToList();            

            var data = (from ct in _contactTypes

                        from e_left in context.EMPLOYEES.Where(w=>w.EMP_ID == ct.CREATED_BY).DefaultIfEmpty()

                        select new ContractTypesExtented
                        {
                            CONTRACT_TYPE_ID = ct.CONTRACT_TYPE_ID,
                            CONTRACT_TYPE_NAME = ct.CONTRACT_TYPE_NAME,
                            CONTRACT_TYPE_CODE = ct.CONTRACT_TYPE_CODE,
                            CONTRACT_TYPE_DESC = ct.CONTRACT_TYPE_DESC,
                            IS_CANCELED = ct.IS_CANCELED,
                            CREATION_DATE = ct.CREATION_DATE != null ?
                                                    ct.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (ct.CREATION_DATE.Month > 9 ? ct.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + ct.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    ct.CREATION_DATE.Day : null,
                            MODIFIED_DATE = ct.MODIFIED_DATE != null ?
                                                    ct.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (ct.MODIFIED_DATE.Month > 9 ? ct.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + ct.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    ct.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left != null ? e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME : SqlConstants.stringEmpty
                        }
                ).OrderBy(o => o.CONTRACT_TYPE_NAME).ToList();

            return data;
        }


    }
}