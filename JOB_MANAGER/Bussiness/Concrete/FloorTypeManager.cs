using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class FloorTypeManager : IService<FLOOR_TYPES, FloorTypeExtented>
    {
        private FloorTypeDal _dal;

        public FloorTypeManager(FloorTypeDal floorTypeDal)
        {
            _dal = floorTypeDal;
        }

        public ShowState AddorUpdate(FLOOR_TYPES param)
        {
            return _dal.AddorUpdate(param, f => f.FLOOR_TYPE_ID == param.FLOOR_TYPE_ID);
        }

        public ShowState Delete(FLOOR_TYPES param)
        {
            return _dal.Delete(param);
        }

        public FLOOR_TYPES Get(FLOOR_TYPES param)
        {
            return _dal.GetAll(f => f.FLOOR_TYPE_ID == param.FLOOR_TYPE_ID).FirstOrDefault();
        }

        public List<FloorTypeExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
