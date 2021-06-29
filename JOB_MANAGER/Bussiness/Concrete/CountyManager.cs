using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class CountyManager : IService<COUNTRIES, CountryExtented>
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