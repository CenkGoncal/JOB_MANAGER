using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class VehicleBodyDal : EntityRepositoryBase<VEHICLE_BODY_TYPES, JOB_MANAGER_DBEntities, VehicleBodysExtented>
    {
        public UserInfo UserInfo;
        public VehicleBodyDal(UserInfo _userInfo)
        {
            UserInfo = _userInfo;
        }

        public override List<VehicleBodysExtented> GetAll2(Expression<Func<VEHICLE_BODY_TYPES, bool>> filter = null)
        {
            var _vehiclelist = filter != null ? context.Set<VEHICLE_BODY_TYPES>().Where(filter).ToList()
                            : context.Set<VEHICLE_BODY_TYPES>().ToList();
            var data = (from vb in _vehiclelist

                        join e in context.EMPLOYEES
                        on vb.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()                  

                        select new VehicleBodysExtented
                        {
                            BODY_TYPE_ID = vb.BODY_TYPE_ID,
                            BODY_TYPE_NAME = vb.BODY_TYPE_NAME,
                            IS_CANCELED = vb.IS_CANCELED,
                            CREATION_DATE = vb.CREATION_DATE != null ?
                                                    vb.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (vb.CREATION_DATE.Month > 9 ? vb.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + vb.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    vb.CREATION_DATE.Day : null,
                            MODIFIED_DATE = vb.MODIFIED_DATE != null ?
                                                    vb.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (vb.MODIFIED_DATE.Month > 9 ? vb.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + vb.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    vb.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
           ).OrderBy(o => o.BODY_TYPE_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(VEHICLE_BODY_TYPES param, VEHICLE_BODY_TYPES dbitem)
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
                param.BODY_TYPE_ID = dbitem.BODY_TYPE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(VEHICLE_BODY_TYPES entity, ShowState showState)
        {            
            if (context.VEHICLE_BODY_TYPES.Any(w => w.BODY_TYPE_ID == entity.BODY_TYPE_ID))
            {
                showState.ErrorMessage = "Vehicle Body Type is available to use vehicle model!! Please try to check cancelled";
                showState.isError = true;
            }

            if (!context.VEHICLE_MODELS.Any(w => w.BODY_TYPE == entity.BODY_TYPE_ID))
            {
                showState.ErrorMessage = "Vehicle Body Type not found!!!";
                showState.isError = true;
            }
        }

    }
}