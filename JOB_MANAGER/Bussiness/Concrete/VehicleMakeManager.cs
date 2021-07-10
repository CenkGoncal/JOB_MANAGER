﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class VehicleMakeManager : IService<VEHICLE_MAKES, VehicleMakeExtented>
    {
        private VehicleMakeDal _dal;
        public VehicleMakeManager(VehicleMakeDal vehicleMakeDal)
        {
            _dal = vehicleMakeDal;
        }

        public ShowState AddorUpdate(VEHICLE_MAKES param)
        {
            return _dal.AddorUpdate(param, f => f.VEHICLE_MAKE_ID == param.VEHICLE_MAKE_ID);
        }

        public ShowState Delete(VEHICLE_MAKES param)
        {
            return _dal.Delete(param);
        }

        public VEHICLE_MAKES Get(VEHICLE_MAKES param)
        {
            return _dal.GetAll(f => f.VEHICLE_MAKE_ID == param.VEHICLE_MAKE_ID).FirstOrDefault();
        }

        public List<VehicleMakeExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}