using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class ClientManager : IClientService
    {
        private ClientDal _clientdal;
        public ClientManager(ClientDal clientDal)
        {
            _clientdal = clientDal;
        }

        public ShowState AddorUpdate(CLIENTS param)
        {
            return _clientdal.AddorUpdate(param,f=>f.CLIENT_ID == param.CLIENT_ID);
        }

        public ShowState Delete(CLIENTS param)
        {
            return _clientdal.Delete(param);
        }

        public CLIENTS Get(CLIENTS param)
        {
            return _clientdal.GetAll(f => f.CLIENT_ID == param.CLIENT_ID).FirstOrDefault();
        }

        public List<ClientExtented> GetAll()
        {
            return _clientdal.GetAll2();
        }

        public List<ClientExtented> GetAllByCompany(int CompanyId)
        {
            return _clientdal.GetAll2().Where(w => w.COMPANY_ID == CompanyId).ToList();
        }
    }
}
