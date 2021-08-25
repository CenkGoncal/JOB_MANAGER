using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class MateryalManager : IMateryalService
    {
        private MateryalDal _dal;
        public MateryalManager(MateryalDal materyalDal)
        {
            _dal = materyalDal;
        }

        public ShowState AddorUpdate(MATERIALS param)
        {
            return _dal.AddorUpdate(param,f=>f.MATERIAL_ID == param.MATERIAL_ID);
        }

        public ShowState Delete(MATERIALS param)
        {
            return _dal.Delete(param);
        }

        public MATERIALS Get(MATERIALS param)
        {
            return _dal.GetAll(f => f.MATERIAL_ID == param.MATERIAL_ID).FirstOrDefault();
        }

        public List<MaterialExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
