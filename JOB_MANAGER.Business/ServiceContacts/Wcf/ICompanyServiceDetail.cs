using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.ServiceContacts.Wcf
{
    [ServiceContract]
    public interface ICompanyServiceDetail
    {
        [OperationContract]
        public List<CompanyExtented> GetAll();
    }
}
