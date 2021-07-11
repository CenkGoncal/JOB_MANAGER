using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class ContractTypeManager : IService<CONTRACT_TYPES, ContractTypesExtented>
    {
        private ContractTypeDal _dal;
        public ContractTypeManager(ContractTypeDal companyTypeDal)
        {
            _dal = companyTypeDal;
        }
          
        public ShowState AddorUpdate(CONTRACT_TYPES param)
        {
            return _dal.AddorUpdate(param, s => s.CONTRACT_TYPE_ID == param.CONTRACT_TYPE_ID);
        }

        public ShowState Delete(CONTRACT_TYPES param)
        {
            return _dal.Delete(param);
        }

        public CONTRACT_TYPES Get(CONTRACT_TYPES param)
        {
            throw new NotImplementedException();
        }

        public List<ContractTypesExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<ContractTypesExtented> GetContractTypesCompany(int CompanyId)
        {
            return _dal.GetAll2(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyId));
        }
    
    }
}
