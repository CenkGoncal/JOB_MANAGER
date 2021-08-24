using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class StatusManager: IStatusService
    {
        private StatusDal _dal;
        public StatusManager(StatusDal statusDal)
        {
            _dal = statusDal;
        }
        public ShowState AddorUpdate(STATUS param)
        {
            return _dal.AddorUpdate(param, f => f.STATUS_ID == param.STATUS_ID);
        }

        public ShowState Delete(STATUS param)
        {
            return _dal.Delete(param);
        }

        public STATUS Get(STATUS param)
        {
            return _dal.GetAll(f => f.STATUS_ID == param.STATUS_ID).FirstOrDefault();
        }

        public List<StatusExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<StatusExtented> GetAllByType(int StatusType)
        {
            return _dal.GetAll2(f=>f.STATUS_TYPE == StatusType);
        }
    }
}
