using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class RoleMenuManager : IService<ROLE_MENU, RoleMenuExtented>
    {
        private RoleMenuDal _dal;

        public RoleMenuManager(RoleMenuDal roleMenuDal)
        {
            _dal = roleMenuDal;
        }

        public ShowState AddorUpdate(ROLE_MENU param)
        {
            return _dal.AddorUpdate(param, f => f.ROLE_ID == param.ROLE_ID);
        }

        public ShowState Delete(ROLE_MENU param)
        {
            return _dal.Delete(param);
        }

        public ROLE_MENU Get(ROLE_MENU param)
        {
            return _dal.GetAll(f => f.ROLE_ID == param.ROLE_ID).FirstOrDefault();
        }

        public List<RoleMenuExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<RoleMenuExtented> GetAllByEmployee()
        {
            return _dal.GetAllByLoginEmployee();
        }
    }
}
