﻿using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class VehicleDal : EntityRepositoryBase<VEHICLES, JOB_MANAGER_DBEntities, VehiclesExtented>
    {
        public UserInfo UserInfo;
        public VehicleDal(UserInfo _userInfo)
        {
            UserInfo = _userInfo;
        }

        public override List<VehiclesExtented> GetAll2(Expression<Func<VEHICLES, bool>> filter = null)
        {
            var _vehiclelist = filter != null ? context.Set<VEHICLES>().Where(filter).ToList()
                            : context.Set<VEHICLES>().ToList();
            var data = (from vb in _vehiclelist

                        join e in context.EMPLOYEES
                        on vb.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        join bt in context.VEHICLE_BODY_TYPES
                        on vb.BODY_TYPE_ID equals bt.BODY_TYPE_ID into bt_join
                        from bt_left in bt_join.DefaultIfEmpty()

                        select new VehiclesExtented
                        {
                            BODY_TYPE_ID = vb.BODY_TYPE_ID,
                            BODY_TYPE_NAME = bt_left.BODY_TYPE_NAME,
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

        public override void SaveHelper_ModifyBeforeSave(VEHICLES param, VEHICLES dbitem)
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

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }
    }
}