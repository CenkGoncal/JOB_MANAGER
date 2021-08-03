using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class SuplierManager : IService<SUPPLIERS, SuplierExtended>
    {
        private SuplierDal _suplierDal;
        public SuplierManager(SuplierDal suplierDal)
        {
            _suplierDal = suplierDal;
        }

        public ShowState AddorUpdate(SUPPLIERS param)
        {
            return _suplierDal.AddorUpdate(param, f => f.SUPPLIER_ID == param.SUPPLIER_ID);
        }

        public ShowState Delete(SUPPLIERS param)
        {
            return _suplierDal.Delete(param);
        }

        public SUPPLIERS Get(SUPPLIERS param)
        {
            return _suplierDal.GetAll(f=>f.SUPPLIER_ID == param.SUPPLIER_ID).FirstOrDefault();
        }

        public List<SuplierExtended> GetAll()
        {
            return _suplierDal.GetAll2();
        }
    }
}
