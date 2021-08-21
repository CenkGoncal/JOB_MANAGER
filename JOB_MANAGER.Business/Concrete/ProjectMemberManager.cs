using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class ProjectMemberManager : IService<PROJECT_MEMBERS, ProjectMemberExtented>
    {
        private ProjectMemberDal _projectMemberDal;
        public ProjectMemberManager(ProjectMemberDal projectMemberDal)
        {
            _projectMemberDal = projectMemberDal;
        }
        public ShowState AddorUpdate(PROJECT_MEMBERS param)
        {
            return _projectMemberDal.AddorUpdate(param, f => f.PROJECT_MEMBER_ID == param.PROJECT_MEMBER_ID);
        }

        public ShowState Delete(PROJECT_MEMBERS param)
        {
            return _projectMemberDal.Delete(param);
        }

        public PROJECT_MEMBERS Get(PROJECT_MEMBERS param)
        {
            return _projectMemberDal.GetAll(f => f.PROJECT_MEMBER_ID == param.PROJECT_MEMBER_ID).FirstOrDefault();
        }

        public List<ProjectMemberExtented> GetAll()
        {
            return _projectMemberDal.GetAll2();
        }
    }
}
