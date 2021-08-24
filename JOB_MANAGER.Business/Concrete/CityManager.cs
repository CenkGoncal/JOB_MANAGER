using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;
using System.Collections.Generic;

namespace JOB_MANAGER.Business.Concrete
{
    public class CityManager : ICityService
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
