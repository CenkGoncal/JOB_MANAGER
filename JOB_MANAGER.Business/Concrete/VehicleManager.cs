using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class VehicleManager : IVehicleService
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
