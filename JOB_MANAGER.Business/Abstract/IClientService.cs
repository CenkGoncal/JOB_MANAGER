using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IClientService : IService<CLIENTS, ClientExtented>
    {
        public List<ClientExtented> GetAllByCompany(int CompanyId);
    }
}
