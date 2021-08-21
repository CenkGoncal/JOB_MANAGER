using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class VehicleModelManager : IService<VEHICLE_MODELS, VehicleModelExtented>
    {
        private VehicleModelDal _dal;

        public VehicleModelManager(VehicleModelDal vehicleModelDal)
        {
            _dal = vehicleModelDal;
        }

        public ShowState AddorUpdate(VEHICLE_MODELS param)
        {
            return _dal.AddorUpdate(param, f => f.VEHICLE_MODEL_ID == param.VEHICLE_MODEL_ID);
        }

        public ShowState Delete(VEHICLE_MODELS param)
        {
            return _dal.Delete(param);
        }

        public VEHICLE_MODELS Get(VEHICLE_MODELS param)
        {
            return _dal.GetAll(f => f.VEHICLE_MODEL_ID == param.VEHICLE_MODEL_ID).FirstOrDefault();
        }

        public List<VehicleModelExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
