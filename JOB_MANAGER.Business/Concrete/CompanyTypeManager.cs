using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;
using System;
using System.Collections.Generic;

namespace JOB_MANAGER.Business.Concrete
{
    public class CompanyTypeManager : ICompanyTypeService
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
