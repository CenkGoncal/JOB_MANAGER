using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class VehicleManager : IService<VEHICLES, VehiclesExtented>
    {  
        private VehicleDal _dal;
        public VehicleManager(VehicleDal vehicleDal)
        {
            _dal = vehicleDal;
        }
        public ShowState AddorUpdate(VEHICLES param)
        {
            return _dal.AddorUpdate(param, f => f.VEHICLE_ID == param.VEHICLE_ID);
        }

        public ShowState Delete(VEHICLES param)
        {
            return _dal.Delete(param);
        }

        public VEHICLES Get(VEHICLES param)
        {
            return _dal.GetAll(f => f.VEHICLE_ID == param.VEHICLE_ID).FirstOrDefault();
        }

        public List<VehiclesExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
