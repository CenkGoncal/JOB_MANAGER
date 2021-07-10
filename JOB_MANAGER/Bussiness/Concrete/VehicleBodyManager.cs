using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class VehicleBodyManager : IService<VEHICLE_BODY_TYPES, VehicleBodysExtented>
    {
        private VehicleBodyDal _dal;

        public VehicleBodyManager(VehicleBodyDal vehicleBodyDal)
        {
            _dal = vehicleBodyDal;
        }

        public ShowState AddorUpdate(VEHICLE_BODY_TYPES param)
        {
            return _dal.AddorUpdate(param, f => f.BODY_TYPE_ID == param.BODY_TYPE_ID);
        }

        public ShowState Delete(VEHICLE_BODY_TYPES param)
        {
            return _dal.Delete(param);
        }

        public VEHICLE_BODY_TYPES Get(VEHICLE_BODY_TYPES param)
        {
            return _dal.GetAll(f => f.BODY_TYPE_ID == param.BODY_TYPE_ID).FirstOrDefault();
        }

        public List<VehicleBodysExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
