using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IRoleMenuService : IService<ROLE_MENU, RoleMenuExtented>
    {
        public List<MenuRolesDto> GetMenuByRole(int roleId);

        public List<RoleMenuExtented> GetAllByEmployee()İ
    }
}
