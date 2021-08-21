using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class ClientDal : EntityRepositoryBase<CLIENTS, JOB_MANAGER_DBEntities, ClientExtented>
    {
        public override List<ClientExtented> GetAll2(Expression<Func<CLIENTS, bool>> filter = null)
        {
            var _clientList = filter != null ? context.Set<CLIENTS>().Where(filter).ToList()
                : context.Set<CLIENTS>().ToList();

            var data = (from v in _clientList

                        where v.IS_CANCELED == false && v.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId
                        select new ClientExtented
                        {
                            CLIENT_ID = v.CLIENT_ID,
                            CLIENT_NAME = v.CLIENT_NAME,
                            CLIENT_SHORT_NAME = v.CLIENT_SHORT_NAME,
                            CLIENT_TYPE_ID = v.CLIENT_TYPE_ID,
                            COMPANY_TYPE_NAME = v.COMPANY_TYPES.COMPANY_TYPE_NAME,
                            CLIENT_STATUS_ID = v.CLIENT_STATUS_ID,
                            STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            CLIENT_PHONE = v.CLIENT_PHONE,
                            CLIENT_URL = v.CLIENT_URL,
                            CLIENT_LINKEDIN = v.CLIENT_LINKEDIN,
                            CLIENT_CODE = v.CLIENT_CODE,
                            CLIENT_COUNTRY = v.CLIENT_COUNTRY,
                            COUNTRY_NAME = v.COUNTRIES.COUNTRY_NAME,
                            CLIENT_STATE = v.CLIENT_STATE,
                            STATE_NAME = v.STATES.STATE_NAME,
                            CLIENT_CITY = v.CLIENT_CITY,
                            CITY_NAME = v.CITIES.CITY_NAME,
                            CLIENT_ADDRESS = v.CLIENT_ADDRESS,
                            POSTAL_CODE = v.POSTAL_CODE,
                            CLIENT_SINCE = v.CREATION_DATE != null ?
                                                    v.CLIENT_SINCE.Value.Year + SqlConstants.stringMinus +
                                                    (v.CLIENT_SINCE.Value.Month > 9 ? v.CLIENT_SINCE.Value.Month + SqlConstants.stringMinus : "0" + v.CLIENT_SINCE.Value.Month + SqlConstants.stringMinus) +
                                                    v.CLIENT_SINCE.Value.Day : null,
                            CREATION_DATE = v.CREATION_DATE != null ?
                                                    v.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (v.CREATION_DATE.Month > 9 ? v.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + v.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    v.CREATION_DATE.Day : null,
                            MODIFIED_DATE = v.MODIFIED_DATE != null ?
                                                    v.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (v.MODIFIED_DATE.Month > 9 ? v.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + v.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    v.MODIFIED_DATE.Day : null,

                        }
                   ).OrderBy(o => o.CLIENT_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(CLIENTS param, CLIENTS dbitem)
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
                param.CLIENT_ID = dbitem.CLIENT_ID;

                param.MODIFIED_DATE = DateTime.Now;               
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(CLIENTS entity, ShowState showState)
        {
            if (!context.CLIENTS.Any(w => w.CLIENT_ID == entity.CLIENT_ID))
            {
                showState.ErrorMessage = "Client Type not found!!";
                showState.isError = true;
            }
        }
    }
}
