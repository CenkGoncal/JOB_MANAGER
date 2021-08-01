using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class ClientManager : IService<CLIENTS, ClientExtented>
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
    }
}
