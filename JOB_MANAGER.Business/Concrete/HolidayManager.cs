using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class HolidayManager : IService<HOLIDAYS, HolidayExtented>
    {
        private HolidayDal _holidayDal;
        public HolidayManager(HolidayDal holidayDal)
        {
            _holidayDal = holidayDal;
        }

        public ShowState AddorUpdate(HOLIDAYS param)
        {
            return _holidayDal.AddorUpdate(param, f => f.HOLIDAY_ID == param.HOLIDAY_ID);
        }

        public ShowState Delete(HOLIDAYS param)
        {
            return _holidayDal.Delete(param);
        }

        public HOLIDAYS Get(HOLIDAYS param)
        {
            return _holidayDal.GetAll(f => f.HOLIDAY_ID == param.HOLIDAY_ID).FirstOrDefault();
        }

        public List<HolidayExtented> GetAll()
        {
            return _holidayDal.GetAll2();
        }
    }
}
