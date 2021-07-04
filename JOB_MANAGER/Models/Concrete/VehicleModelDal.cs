using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class VehicleModelDal : EntityRepositoryBase<VEHICLE_MODELS, JOB_MANAGER_DBEntities, VehicleModelExtented>
    {
        public UserInfo UserInfo;
        public VehicleModelDal(UserInfo _userInfo)
        {
            UserInfo = _userInfo;
        }

        public override List<VehicleModelExtented> GetAll2(Expression<Func<VEHICLE_MODELS, bool>> filter = null)
        {
            var _vehiclelist = filter != null ? context.Set<VEHICLE_MODELS>().Where(filter).ToList()
                          : context.Set<VEHICLE_MODELS>().ToList();

            var data = (from vm in context.VEHICLE_MODELS

                        join e in context.EMPLOYEES
                        on vm.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new VehicleModelExtented
                        {
                            VEHICLE_MAKE_ID = vm.VEHICLE_MAKE_ID,
                            BODY_TYPE = vm.BODY_TYPE,
                            VEHICLE_MODEL_ID = vm.VEHICLE_MODEL_ID,
                            VEHICLE_MODEL_NAME = vm.VEHICLE_MODEL_NAME,
                            IS_CANCELED = vm.IS_CANCELED,
                            CREATION_DATE = vm.CREATION_DATE != null ?
                                                    vm.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (vm.CREATION_DATE.Month > 9 ? vm.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + vm.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    vm.CREATION_DATE.Day : null,
                            MODIFIED_DATE = vm.MODIFIED_DATE != null ?
                                                    vm.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (vm.MODIFIED_DATE.Month > 9 ? vm.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + vm.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    vm.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                    ).OrderBy(o => o.VEHICLE_MODEL_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(VEHICLE_MODELS param, VEHICLE_MODELS dbitem)
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
                param.VEHICLE_MODEL_ID = dbitem.VEHICLE_MODEL_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

    }
}