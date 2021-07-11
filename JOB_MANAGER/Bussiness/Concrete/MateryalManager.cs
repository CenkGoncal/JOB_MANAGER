using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class MateryalManager : IService<MATERIALS, MaterialExtented>
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
