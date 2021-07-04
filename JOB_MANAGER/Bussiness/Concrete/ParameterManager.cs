using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class ParameterManager : IService<PARAMETERS, ParameterExtented>
    {
        private ParameterDal _dal;
        public ParameterManager(ParameterDal countyDal)
        {
            _dal = countyDal;
        }

        public ShowState AddorUpdate(PARAMETERS param)
        {
            var showstate = _dal.AddorUpdate(param, f => f.PARAM_NAME == param.PARAM_NAME);
            GlobalCache global = new GlobalCache();
            global.CreateCacheParameter(true);

            return showstate;
        }

        public ShowState Delete(PARAMETERS param)
        {
            return _dal.Delete(param);
        }

        public PARAMETERS Get(PARAMETERS param)
        {
            throw new NotImplementedException();
        }

        public List<ParameterExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}