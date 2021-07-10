using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class CitiesDal : EntityRepositoryBase<CITIES, JOB_MANAGER_DBEntities, CitiesExtented>
    {        
        public UserInfo UserInfo;

        public CitiesDal(UserInfo _userInfo)
        {            
            UserInfo = _userInfo;
        }

        public override List<CitiesExtented> GetAll2(Expression<Func<CITIES, bool>> filter = null)
        {
            var query = (from c in context.CITIES
                         join st in context.STATES on c.STATE_ID equals st.STATE_ID
                         join ct in context.COUNTRIES on c.COUNTRY_ID equals ct.COUNTRY_ID

                         join e in context.EMPLOYEES
                         on c.CREATED_BY equals e.EMP_ID into e_join
                         from e_left in e_join.DefaultIfEmpty()
                         select new CitiesExtented
                         {
                             COUNTRY_NAME = ct.COUNTRY_NAME,
                             COUNTRY_ID = ct.COUNTRY_ID,
                             STATE_ID = st.STATE_ID,
                             STATE_NAME = st.STATE_NAME,
                             CITY_ID = c.CITY_ID,
                             CITY_NAME = c.CITY_NAME,
                             POSTAL_CODE = c.POSTAL_CODE,
                             IS_DEFAULT = c.IS_DEFAULT,
                             IS_CANCELED = c.IS_CANCELED,
                             CREATION_DATE = c.CREATION_DATE != null ?
                                                    c.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (c.CREATION_DATE.Month > 9 ? c.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + c.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    c.CREATION_DATE.Day : null,
                             MODIFIED_DATE = c.MODIFIED_DATE != null ?
                                                    c.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (c.MODIFIED_DATE.Month > 9 ? c.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + c.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    c.MODIFIED_DATE.Day : null,
                             CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                         }
                               ).OrderBy(o => o.CITY_NAME).ToList();

            return query;
        }

        public override void SaveControls(CITIES entity, ShowState showState)
        {
            if (context.CITIES.Any(w => w.CITY_ID == entity.CITY_ID))
            {
                if (context.CITIES.Any(x => x.CITY_NAME.Equals(entity.CITY_NAME) && x.POSTAL_CODE.Equals(entity.POSTAL_CODE) && 
                                       x.STATE_ID == entity.STATE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "City already exists.";
                    showState.isError = true;
                }
                if (entity.IS_DEFAULT)
                {
                    if (context.CITIES.Any(x => x.IS_DEFAULT == true && x.STATE_ID == entity.STATE_ID))
                    {
                        showState.ErrorMessage = "Only one City can be set as default";
                        showState.isError = true;
                    }
                }
            }
            else
            {
                if (context.CITIES.Any(x => x.CITY_NAME.Equals(entity.CITY_NAME) && x.CITY_ID != entity.CITY_ID && x.POSTAL_CODE.Equals(entity.POSTAL_CODE) && x.STATE_ID == entity.STATE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "City already exists.";
                    showState.isError = true;
                }
                if (entity.IS_DEFAULT)
                {
                    if (context.CITIES.Any(x => x.IS_DEFAULT == true && x.CITY_ID != entity.CITY_ID && x.STATE_ID == entity.STATE_ID))
                    {
                        showState.ErrorMessage =  "Only one City can be set as default";
                        showState.isError = true;
                    }
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(CITIES param, CITIES dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
            }
            else
            {
                param.MODIFIED_DATE = dbitem.MODIFIED_DATE;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.CITY_ID = dbitem.CITY_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(CITIES entity, ShowState showState)
        {
            if(!context.CITIES.Any(w => w.CITY_ID == entity.CITY_ID))
            {
                showState.isError = true;
                showState.ErrorMessage = "City not found";
            }
        }
    }
}
