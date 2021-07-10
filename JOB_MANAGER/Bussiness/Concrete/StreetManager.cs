using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class StreetManager : IService<STREET, StreetExtented>
    {
        private StreetDal _dal;
        public StreetManager(StreetDal streetDal)
        {
            _dal = streetDal;
        }

        public ShowState AddorUpdate(STREET param)
        {
            return _dal.AddorUpdate(param, f => f.STREET_ID == param.STREET_ID);
        }

        public ShowState Delete(STREET param)
        {
            return _dal.Delete(param);
        }

        public STREET Get(STREET param)
        {
            return _dal.GetAll(f=>f.STREET_ID == param.STREET_ID).FirstOrDefault();
        }

        public List<StreetExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
