using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class StateManager : IStateService
    {
        private StateDal _dal;
        public StateManager(StateDal stateDal)
        {
            _dal = stateDal;
        }

        public ShowState AddorUpdate(STATES param)
        {
            return _dal.AddorUpdate(param, f => f.STATE_ID == param.STATE_ID);
        }

        public ShowState Delete(STATES param)
        {
            return _dal.Delete(param);
        }

        public STATES Get(STATES param)
        {
            return _dal.GetAll(f => f.STATE_ID == param.STATE_ID).FirstOrDefault();
        }

        public List<StateExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
