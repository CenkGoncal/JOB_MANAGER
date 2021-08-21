using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class ProjectManager : IService<PROJECTS, ProjectExtented>
    {
        private ProjectDal _projectDal;
        public ProjectManager(ProjectDal projectDal)
        {
            _projectDal = projectDal;
        }

        public ShowState AddorUpdate(PROJECTS param)
        {
            return _projectDal.AddorUpdate(param, f => f.PROJECT_ID == f.PROJECT_ID);
        }

        public ShowState Delete(PROJECTS param)
        {
            return _projectDal.Delete(param);
        }

        public PROJECTS Get(PROJECTS param)
        {
            return _projectDal.GetAll(f => f.PROJECT_ID == f.PROJECT_ID).FirstOrDefault();
        }

        public List<ProjectExtented> GetAll()
        {
            return _projectDal.GetAll2();
        }
    }
}
