using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class PhaseManager : IService<DEF_PROJECT_PHASES, DefProjectPhaseExtented>
    {
        private PhaseDal _dal;
        public PhaseManager(PhaseDal phaseDal)
        {
            _dal = phaseDal;
        }

        public ShowState AddorUpdate(DEF_PROJECT_PHASES param)
        {
            return _dal.AddorUpdate(param, f => f.PHASE_ID == param.PHASE_ID);
        }

        public ShowState Delete(DEF_PROJECT_PHASES param)
        {
            return _dal.Delete(param);
        }

        public DEF_PROJECT_PHASES Get(DEF_PROJECT_PHASES param)
        {
            return _dal.GetAll(f => f.PHASE_ID == param.PHASE_ID).FirstOrDefault();
        }

        public List<DefProjectPhaseExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<ProjectTypeWithPhaseTask> GetProjectTypeWithPhaseTasks()
        {
            return _dal.GetProjectTypeWithPhaseTasks();
        }

    }
}
