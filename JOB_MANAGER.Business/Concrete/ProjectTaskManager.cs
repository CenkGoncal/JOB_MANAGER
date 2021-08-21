using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;


namespace JOB_MANAGER.Bussiness.Concrete
{
    public class ProjectTaskManager : IService<PROJECT_TASK, HolidayExtented>
    {
        private ProjectTaskDal _projectTaskDal;
        public ProjectTaskManager(ProjectTaskDal projectTaskDal)
        {
            _projectTaskDal = projectTaskDal;
        }
        public ShowState AddorUpdate(PROJECT_TASK param)
        {
            return _projectTaskDal.AddorUpdate(param, f => f.PROJECT_TASK_ID == param.PROJECT_TASK_ID);
        }

        public ShowState Delete(PROJECT_TASK param)
        {
            return _projectTaskDal.Delete(param);
        }

        public PROJECT_TASK Get(PROJECT_TASK param)
        {
            return _projectTaskDal.GetAll(f => f.PROJECT_TASK_ID == param.PROJECT_TASK_ID).FirstOrDefault();
        }

        public List<HolidayExtented> GetAll()
        {
            return _projectTaskDal.GetAll2();
        }
    }
}
