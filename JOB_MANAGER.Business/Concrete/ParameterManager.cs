using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DataAcess.Helper;
using JOB_MANAGER.DATAACESS.Models;
using System.Collections.Generic;
using System.Linq;

namespace JOB_MANAGER.Business.Concrete
{
    public class ParameterManager : IParametrerService
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
            return _dal.GetAll(f => f.PARAM_NAME == param.PARAM_NAME).FirstOrDefault();
        }

        public List<ParameterExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
