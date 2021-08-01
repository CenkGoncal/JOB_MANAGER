using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class VehicleDal : EntityRepositoryBase<VEHICLES, JOB_MANAGER_DBEntities, VehiclesExtented>
    {        
        public override List<VehiclesExtented> GetAll2(Expression<Func<VEHICLES, bool>> filter = null)
        {
            var _vehiclelist = filter != null ? context.Set<VEHICLES>().Where(filter).ToList()
                            : context.Set<VEHICLES>().ToList();
            var data = (from v in _vehiclelist

                        where v.IS_CANCELED == false// && v.COMPANY_ID == CompanyID
                        select new VehiclesExtented
                        {
                            VEHICLE_ID = v.VEHICLE_ID,
                            VEHICLE_NBR = v.VEHICLE_NBR,
                            VEHICLE_STATUS = v.VEHICLE_STATUS,
                            VEHICLE_STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            CURRENT_DRIVER = v.EMPLOYEES.EMP_ID,
                            DRIVER_NAME = v.EMPLOYEES.FIRST_NAME + SqlConstants.stringWhiteSpace + v.EMPLOYEES.LAST_NAME,
                            REGISTRATION_NUMBER = v.REGISTRATION_NUMBER,
                            REGISTRATION_EXPIRY = v.REGISTRATION_EXPIRY != null ?
                                                    v.REGISTRATION_EXPIRY.Value.Year + SqlConstants.stringMinus +
                                                    (v.REGISTRATION_EXPIRY.Value.Month > 9 ? v.REGISTRATION_EXPIRY.Value.Month + SqlConstants.stringMinus : "0" + v.REGISTRATION_EXPIRY.Value.Month + SqlConstants.stringMinus) +
                                                    v.REGISTRATION_EXPIRY.Value.Day : null,
                            ASSIGNED_TAG = v.ASSIGNED_TAG,
                            VEHICLE_MAKE_ID = v.VEHICLE_MAKE_ID,
                            VEHICLE_MAKE_NAME = v.VEHICLE_MAKES.VEHICLE_MAKE_NAME,
                            BODY_TYPE_ID = v.BODY_TYPE_ID,
                            BODY_TYPE_NAME = v.VEHICLE_BODY_TYPES.BODY_TYPE_NAME,
                            VEHICLE_MODEL_ID = v.VEHICLE_MODEL_ID,
                            VEHICLE_MODEL_NAME = v.VEHICLE_MODELS.VEHICLE_MODEL_NAME,
                            VEHICLE_YEAR = v.VEHICLE_YEAR,
                            VEHICLE_COLOR = v.VEHICLE_COLOR,
                            VEHICLE_DESC = v.VEHICLE_DESC
                        }
                       ).OrderBy(o => o.VEHICLE_NBR).ToList(); 

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(VEHICLES param, VEHICLES dbitem)
        {
            if (dbitem == null)
            {
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.VEHICLE_ID = dbitem.VEHICLE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }
    }
}
