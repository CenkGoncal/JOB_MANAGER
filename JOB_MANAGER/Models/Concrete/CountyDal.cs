using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class CountyDal : EntityRepositoryBase<COUNTRIES, JOB_MANAGER_DBEntities, CountryExtented>
    {        
        public override List<CountryExtented> GetAll2(Expression<Func<COUNTRIES, bool>> filter = null)
        {
            var data = (from ct in context.COUNTRIES

                        join e in context.EMPLOYEES
                        on ct.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()
                        select new CountryExtented
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

        public override void SaveHelper_ModifyBeforeSave(COUNTRIES param, COUNTRIES dbitem)
        {
            if (dbitem == null)
            {
                param.FLAG_URL = param.FLAG_URL == "" ? "http://placehold.it/50x30" : param.FLAG_URL;
                param.CREATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = DateTime.Now;
            }
            else
            {
                param.FLAG_URL = dbitem.FLAG_URL;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.COUNTRY_ID = dbitem.COUNTRY_ID;
                
                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void SaveControls(COUNTRIES entity, ShowState showState)
        {
            COUNTRIES Countryies = context.COUNTRIES.Where(w => w.COUNTRY_ID == entity.COUNTRY_ID).FirstOrDefault();
            
            if (Countryies == null)
            {
                if (context.COUNTRIES.Any(x => x.COUNTRY_NAME.Equals(entity.COUNTRY_NAME) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Country already exists.";
                    showState.isError = true;
                }
                if (entity.IS_DEFAULT)
                {
                    if (context.COUNTRIES.Any(x => x.IS_DEFAULT == true))
                    {
                        showState.ErrorMessage = "Only one country can be set as default";
                        showState.isError = true;
                    }
                }
            }
            else
            {
                if (context.COUNTRIES.Any(x => x.COUNTRY_NAME.Equals(entity.COUNTRY_NAME) && x.COUNTRY_ID != entity.COUNTRY_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Country already exists.";
                    showState.isError = true;
                }
                if (entity.IS_DEFAULT)
                {
                    if (context.COUNTRIES.Any(x => x.IS_DEFAULT == true && x.COUNTRY_ID != entity.COUNTRY_ID))
                    {
                        showState.ErrorMessage = "Only one country can be set as default";
                        showState.isError = true;
                    }
                }
            }
        }

        public override void DeleteControls(COUNTRIES entity, ShowState showState)
        {
            var checkState = context.STATES.Where(w => w.COUNTRY_ID == entity.COUNTRY_ID).FirstOrDefault();
            var checCity = context.CITIES.Where(w => w.COUNTRY_ID == entity.COUNTRY_ID).FirstOrDefault();

            if (checkState != null)
            {
                showState.isError = true;
                showState.ErrorMessage = "State are available to use this country!! You can change cancelled type.";
            }
            else
            if (checkState != null)
            {
                showState.isError = true;
                showState.ErrorMessage = "City are available to use this country!!";
            }
        }
    }
}
