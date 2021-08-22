using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

using System;
using System.Collections.Generic;

namespace JOB_MANAGER.Business.Concrete
{
    public class ContractTypeManager : IService<CONTRACT_TYPES, ContractTypesExtented>, IContractTypeService
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
