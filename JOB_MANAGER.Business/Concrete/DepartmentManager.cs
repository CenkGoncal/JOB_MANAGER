using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
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

        public List<DepartmentExtented> GetDepartmentCompany(int CompanyId)
        {
            return _dal.GetAll2(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyId)).ToList();
        }
    }
}
