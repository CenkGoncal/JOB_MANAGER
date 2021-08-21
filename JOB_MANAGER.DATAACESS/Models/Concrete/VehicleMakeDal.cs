using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class VehicleMakeDal : EntityRepositoryBase<VEHICLE_MAKES, JOB_MANAGER_DBEntities, VehicleMakeExtented>
    {
     
        public override List<VehicleMakeExtented> GetAll2(Expression<Func<VEHICLE_MAKES, bool>> filter = null)
        {
            var _vehiclelist = filter != null ? context.Set<VEHICLE_MAKES>().Where(filter).ToList()
                          : context.Set<VEHICLE_MAKES>().ToList();

            var data = (from vm in context.VEHICLE_MAKES

                        join e in context.EMPLOYEES
                        on vm.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new VehicleMakeExtented
                        {
                            VEHICLE_MAKE_ID = vm.VEHICLE_MAKE_ID,
                            VEHICLE_MAKE_NAME = vm.VEHICLE_MAKE_NAME,
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
          ).OrderBy(o => o.VEHICLE_MAKE_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(VEHICLE_MAKES param, VEHICLE_MAKES dbitem)
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
                param.VEHICLE_MAKE_ID = dbitem.VEHICLE_MAKE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(VEHICLE_MAKES entity, ShowState showState)
        {
            if (context.VEHICLE_MODELS.Any(w => w.VEHICLE_MAKE_ID == entity.VEHICLE_MAKE_ID))
            {
                showState.ErrorMessage = "Vehicle Body Type is available to use vehicle model!! Please try to check cancelled";
                showState.isError = true;
            }

            if (!context.VEHICLE_MAKES.Any(w => w.VEHICLE_MAKE_ID == entity.VEHICLE_MAKE_ID))
            {
                showState.ErrorMessage = "Vehicle Make Type not found!!!";
                showState.isError = true;
            }
        }
    }
}
