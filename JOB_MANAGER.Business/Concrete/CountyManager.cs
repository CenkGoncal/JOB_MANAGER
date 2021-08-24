using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;
using System;
using System.Collections.Generic;

namespace JOB_MANAGER.Business.Concrete
{
    public class CountyManager : ICountyService
    {
        private CountyDal _dal;
        public CountyManager(CountyDal countyDal)
        {
            _dal = countyDal;
        }
        public ShowState AddorUpdate(COUNTRIES param)
        {
            return _dal.AddorUpdate(param, f => f.COUNTRY_ID == param.COUNTRY_ID);
        }

        public ShowState Delete(COUNTRIES param)
        {
            return _dal.Delete(param);
        }

        public COUNTRIES Get(COUNTRIES param)
        {
            throw new NotImplementedException();
        }

        public List<CountryExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
