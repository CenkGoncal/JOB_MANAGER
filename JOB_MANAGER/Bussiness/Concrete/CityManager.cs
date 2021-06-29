using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER_BUSSINESS.Concrete
{
    public class CityManager : IService<CITIES, CitiesExtented>
    {
        private CitiesDal _citiesDal;
        public CityManager(CitiesDal citiesDal)
        {
            _citiesDal = citiesDal;
        }

        public ShowState AddorUpdate(CITIES param)
        {            
            return _citiesDal.AddorUpdate(param, s => s.CITY_ID == param.CITY_ID);
        }

        public ShowState Delete(CITIES param)
        {
            return _citiesDal.Delete(param);
        }

        public CITIES Get(CITIES param)
        {
            return _citiesDal.Get(s=>s.CITY_ID == param.CITY_ID);
        }

        public List<CitiesExtented> GetAll()
        {
            return _citiesDal.GetAll2();
        }
    }
}
