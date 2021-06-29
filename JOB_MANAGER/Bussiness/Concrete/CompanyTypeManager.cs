using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class CompanyTypeManager : IService<COMPANY_TYPES, CompanyTypesExtented>
    {
        private CompanyTypeDal _companyTypeDal;
        public CompanyTypeManager(CompanyTypeDal companyTypeDal)
        {
            _companyTypeDal = companyTypeDal;
        }
          
        public ShowState AddorUpdate(COMPANY_TYPES param)
        {
            return _companyTypeDal.AddorUpdate(param, s => s.COMPANY_TYPE_ID == param.COMPANY_TYPE_ID);
        }

        public ShowState Delete(COMPANY_TYPES param)
        {
            return _companyTypeDal.Delete(param);
        }

        public COMPANY_TYPES Get(COMPANY_TYPES param)
        {
            throw new NotImplementedException();
        }

        public List<CompanyTypesExtented> GetAll()
        {
            return _companyTypeDal.GetAll2();
        }
    }
}