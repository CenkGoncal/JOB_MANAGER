using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;
using Newtonsoft.Json;

namespace JOB_MANAGER.Business.Concrete
{
    public class RoleMenuManager : IRoleMenuService
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

        public List<MenuRolesDto> GetMenuByRole(int roleId)
        {
            var roleMenu = _dal.GetAll2(f => f.ROLE_ID == roleId).FirstOrDefault();
            if (string.IsNullOrEmpty(roleMenu.MENUS_STR))
                return null;

            return JsonConvert.DeserializeObject<List<MenuRolesDto>>(roleMenu.MENUS_STR);
        }

        public List<RoleMenuExtented> GetAllByEmployee()
        {
            return _dal.GetAllByLoginEmployee();
        }
    }
}
