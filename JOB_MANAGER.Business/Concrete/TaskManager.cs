using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class TaskManager : IService<DEF_TASKS, DefTaskExtented>
    {
        private TaskDal _dal;
        public TaskManager(TaskDal taskDal)
        {
            _dal = taskDal;
        }
        public ShowState AddorUpdate(DEF_TASKS param)
        {
            return _dal.AddorUpdate(param, f => f.TASK_ID == param.TASK_ID);
        }

        public ShowState Delete(DEF_TASKS param)
        {
            return _dal.Delete(param);
        }

        public DEF_TASKS Get(DEF_TASKS param)
        {
            return _dal.GetAll(f=>f.TASK_ID == param.TASK_ID).FirstOrDefault();
        }

        public List<DefTaskExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
