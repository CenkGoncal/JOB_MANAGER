using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class StateManager : IService<STATES, StateExtented>
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
