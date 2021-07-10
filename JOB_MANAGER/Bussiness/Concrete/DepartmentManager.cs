using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class DepartmentManager : IService<DEPARTMENTS, DepartmentExtented>
    {
        private DepartmentDal _dal;
        public DepartmentManager(DepartmentDal departmentDal)
        {
            _dal = departmentDal;
        }
        public ShowState AddorUpdate(DEPARTMENTS param)
        {
            return _dal.AddorUpdate(param, f => f.DEPARTMENT_ID == param.DEPARTMENT_ID);
        }

        public ShowState Delete(DEPARTMENTS param)
        {
            return _dal.Delete(param);
        }

        public DEPARTMENTS Get(DEPARTMENTS param)
        {
            return _dal.GetAll(f=>f.DEPARTMENT_ID == param.DEPARTMENT_ID).FirstOrDefault();
        }

        public List<DepartmentExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
