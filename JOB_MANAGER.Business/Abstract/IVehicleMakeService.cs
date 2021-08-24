using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IVehicleMakeService : IService<VEHICLE_MAKES, VehicleMakeExtented>
    {
    }
}
