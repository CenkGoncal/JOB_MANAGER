using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class StateDal : EntityRepositoryBase<STATES, JOB_MANAGER_DBEntities, StateExtented>
    {        
        public UserInfo UserInfo;

        public StateDal(UserInfo _userInfo)
        {            
            UserInfo = _userInfo;
        }

        public override List<StateExtented> GetAll2(Expression<Func<STATES, bool>> filter = null)
        {
            var query = (from st in context.STATES
                         join ct in context.COUNTRIES on st.COUNTRY_ID equals ct.COUNTRY_ID

                         join e in context.EMPLOYEES
                         on st.CREATED_BY equals e.EMP_ID into e_join
                         from e_left in e_join.DefaultIfEmpty()
                         select new StateExtented
                         {
                             COUNTRY_NAME = ct.COUNTRY_NAME,
                             COUNTRY_ID = ct.COUNTRY_ID,
                             STATE_ID = st.STATE_ID,
                             STATE_NAME = st.STATE_NAME,
                             STATE_ABBREVIATION = st.STATE_ABBREVIATION,
                             IS_DEFAULT = st.IS_DEFAULT,
                             IS_CANCELED = st.IS_CANCELED,
                             CREATION_DATE = st.CREATION_DATE != null ?
                                                    st.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (st.CREATION_DATE.Month > 9 ? st.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + st.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    st.CREATION_DATE.Day : null,
                             MODIFIED_DATE = st.MODIFIED_DATE != null ?
                                                    st.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (st.MODIFIED_DATE.Month > 9 ? st.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + st.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    st.MODIFIED_DATE.Day : null,
                             CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                         }
                         ).OrderBy(o => o.STATE_NAME).ToList();

            return query;
        }

        public override void SaveHelper_ModifyBeforeSave(STATES param, STATES dbitem)
        {
            if (dbitem == null)
            {                
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.STATE_ID = dbitem.STATE_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                
                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(STATES entity, ShowState showState)
        {
            var checCity = context.CITIES.Where(w => w.STATE_ID == entity.STATE_ID).FirstOrDefault();
            if (checCity != null)
            {
                showState.ErrorMessage = "State are available to use this country!! You can change cancelled type.";
                showState.isError = true;
            }
            else
            if(context.STATES.Any(w => w.STATE_ID == entity.STATE_ID))
            {
                showState.ErrorMessage = "State not found";
                showState.isError = true;
            }
        }

    }
}