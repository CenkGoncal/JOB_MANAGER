using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IPhaseService : IService<DEF_PROJECT_PHASES, DefProjectPhaseExtented>
    {
        public List<ProjectTypeWithPhaseTask> GetProjectTypeWithPhaseTasks();
    }
}
