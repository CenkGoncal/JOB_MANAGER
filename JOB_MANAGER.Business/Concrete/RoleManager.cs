using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;


namespace JOB_MANAGER.Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private RoleDal _dal;

        public RoleManager(RoleDal roleDal)
        {
            _dal = roleDal;
        }

        public ShowState AddorUpdate(ROLES param)
        {
            return _dal.AddorUpdate(param, f => f.ROLE_ID == param.ROLE_ID);
        }

        public ShowState Delete(ROLES param)
        {
            return _dal.Delete(param);
        }

        public ROLES Get(ROLES param)
        {
            return _dal.GetAll(f => f.ROLE_ID == param.ROLE_ID).FirstOrDefault();
        }

        public List<RolesExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<RolesExtented> GetRoleCompany(int CompanyId)
        {
            return _dal.GetAll2(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyId)).ToList();
        }
    }
}
