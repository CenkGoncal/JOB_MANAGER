using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class SuplierDal : EntityRepositoryBase<SUPPLIERS, JOB_MANAGER_DBEntities, SuplierExtended>
    {
        public override List<SuplierExtended> GetAll2(Expression<Func<SUPPLIERS, bool>> filter = null)
        {
            var _suplierlist = filter != null ? context.Set<SUPPLIERS>().Where(filter).ToList()
                            : context.Set<SUPPLIERS>().ToList();  

            var data = (from v in _suplierlist

                        where v.IS_CANCELED == false// && v.COMPANY_ID == CompanyID
                        select new SuplierExtended
                        {
                            SUPPLIER_ID = v.SUPPLIER_ID,
                            SUPPLIER_NAME = v.SUPPLIER_NAME,
                            SUPPLIER_NUMBER = v.SUPPLIER_NUMBER,
                            SUPPLIER_STATUS_ID = v.SUPPLIER_STATUS_ID,
                            SUPPLIER_STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            SUPPLIER_URL = v.SUPPLIER_URL,
                            SUPPLIER_PHONE = v.SUPPLIER_PHONE,
                            SUPPLIER_EMAIL = v.SUPPLIER_EMAIL,
                            SUPPLIER_ADDRESS = v.SUPPLIER_ADDRESS,
                            SUPPLIER_COUNTRY = v.SUPPLIER_COUNTRY,
                            SUPPLIER_STATE = v.SUPPLIER_STATE,
                            SUPPLIER_CITY = v.SUPPLIER_CITY,
                            POSTAL_CODE = v.POSTAL_CODE,
                        }
           ).OrderBy(o => o.SUPPLIER_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(SUPPLIERS param, SUPPLIERS dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.MODIFIED_DATE = dbitem.MODIFIED_DATE;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.SUPPLIER_ID = dbitem.SUPPLIER_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(SUPPLIERS entity, ShowState showState)
        {
            if (!context.SUPPLIERS.Any(w => w.SUPPLIER_ID == entity.SUPPLIER_ID))
            {
                showState.isError = true;
                showState.ErrorMessage = "Supplier not found";
            }
        }
    }
}
