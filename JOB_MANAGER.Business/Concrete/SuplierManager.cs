using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class SuplierManager : ISuplierService
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
