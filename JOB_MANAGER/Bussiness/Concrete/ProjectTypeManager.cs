using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class ProjectTypeManager : IService<PROJECT_TYPES, ProjectTypeExtented>
    {
        private ProjectTypeDal _dal;

        public ProjectTypeManager(ProjectTypeDal projectTypeDal)
        {
            _dal = projectTypeDal;
        }

        public ShowState AddorUpdate(PROJECT_TYPES param)
        {
            return _dal.AddorUpdate(param, f => f.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID);
        }

        public ShowState Delete(PROJECT_TYPES param)
        {
            return _dal.Delete(param);
        }

        public PROJECT_TYPES Get(PROJECT_TYPES param)
        {
            return _dal.GetAll(f => f.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID).FirstOrDefault();
        }

        public List<ProjectTypeExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
