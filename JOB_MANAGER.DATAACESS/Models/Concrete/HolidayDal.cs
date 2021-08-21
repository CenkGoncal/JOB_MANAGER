using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class HolidayDal : EntityRepositoryBase<HOLIDAYS, JOB_MANAGER_DBEntities, HolidayExtented>
    {
        public override List<HolidayExtented> GetAll2(Expression<Func<HOLIDAYS, bool>> filter = null)
        {
            var _holidaylist = filter != null ? context.Set<HOLIDAYS>().Where(filter).ToList()
                : context.Set<HOLIDAYS>().ToList();

            var list = (from H in _holidaylist
                        from E_LEFT in context.EMPLOYEES.Where(w => w.EMP_ID == H.CREATED_BY).DefaultIfEmpty()

                        select new HolidayExtented
                        {
                            HOLIDAY_ID = H.HOLIDAY_ID,
                            HOLIDAY_NAME = H.HOLIDAY_NAME,
                            HOLIDAY_DESC = H.HOLIDAY_DESC,
                            START_DATE = H.START_DATE != null ?
                                        H.START_DATE.Year + SqlConstants.stringMinus +
                                        (H.START_DATE.Month > 9 ? H.START_DATE.Month + SqlConstants.stringMinus : "0" + H.START_DATE.Month + SqlConstants.stringMinus) +
                                        H.START_DATE.Day : null,
                            END_DATE = H.END_DATE != null ?
                                        H.END_DATE.Year + SqlConstants.stringMinus +
                                        (H.END_DATE.Month > 9 ? H.END_DATE.Month + SqlConstants.stringMinus : "0" + H.END_DATE.Month + SqlConstants.stringMinus) +
                                        H.END_DATE.Day : null,
                            IS_CANCELED = H.IS_CANCELED,
                            CREATION_DATE = H.CREATION_DATE != null ?
                                        H.CREATION_DATE.Year + SqlConstants.stringMinus +
                                        (H.CREATION_DATE.Month > 9 ? H.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + H.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                        H.CREATION_DATE.Day : null,
                            MODIFIED_DATE = H.MODIFIED_DATE != null ?
                                        H.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                        (H.MODIFIED_DATE.Month > 9 ? H.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + H.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                        H.MODIFIED_DATE.Day : null,
                            CREATE_BY = E_LEFT != null ? E_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + E_LEFT.LAST_NAME : SqlConstants.stringEmpty
                        }).ToList();

            return list;
        }

        public override void DeleteControls(HOLIDAYS entity, ShowState showState)
        {
            if (!context.HOLIDAYS.Any(x => x.HOLIDAY_ID == entity.HOLIDAY_ID))
            {
                showState.ErrorMessage = "Holiday not exists.";
                showState.isError = true;
            }
        }

        public override void SaveControls(HOLIDAYS entity, ShowState showState)
        {
            
            var control = context.HOLIDAYS.Where(w => w.HOLIDAY_ID == entity.HOLIDAY_ID).FirstOrDefault();
            if (control == null)
            {                

                if (context.HOLIDAYS.Any(x => x.HOLIDAY_NAME.Equals(entity.HOLIDAY_NAME) && x.IS_CANCELED == false && (x.COMPANY_ID == -1 || x.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId)))
                {
                    showState.ErrorMessage = "Holiday already exists.";
                    showState.isError = true;
                }

            }
            else
            {
                if (context.HOLIDAYS.Any(x => x.HOLIDAY_NAME.Equals(entity.HOLIDAY_NAME) && x.HOLIDAY_ID != entity.HOLIDAY_ID && x.IS_CANCELED == false && (x.COMPANY_ID == -1 || x.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId)))
                {
                    showState.ErrorMessage = "Holiday already exists.";
                    showState.isError = false;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(HOLIDAYS param, HOLIDAYS dbitem)
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
                param.HOLIDAY_ID = dbitem.HOLIDAY_ID;
                param.CREATED_BY = dbitem.CREATED_BY;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }
    }
}
