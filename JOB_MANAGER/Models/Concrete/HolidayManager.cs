using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Models.Concrete
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
