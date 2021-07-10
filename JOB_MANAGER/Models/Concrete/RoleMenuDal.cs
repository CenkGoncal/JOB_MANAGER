using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class RoleMenuDal: EntityRepositoryBase<ROLE_MENU, JOB_MANAGER_DBEntities, RoleMenuExtented>
    {
        public UserInfo UserInfo;

        public RoleMenuDal(UserInfo _userInfo)
        {            
            UserInfo = _userInfo;
        }

        public override List<RoleMenuExtented> GetAll2(Expression<Func<ROLE_MENU, bool>> filter = null)
        {
            var _rolesmenu = filter != null ? context.Set<ROLE_MENU>().Where(filter).ToList()
             : context.Set<ROLE_MENU>().ToList();

            var data = (from r in context.ROLES

                        join rm in _rolesmenu
                        on r.ROLE_ID equals rm.ROLE_ID into rm_join
                        from rm_left in rm_join.DefaultIfEmpty()

                        join e in context.EMPLOYEES
                        on r.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new RoleMenuExtented
                        {
                            ROLE_ID = r.ROLE_ID,
                            ROLE_NAME = r.ROLE_NAME,
                            MENU_ID = rm_left != null ? rm_left.MENU_ID : 0,
                            MENUS_STR = rm_left != null ? rm_left.MENUS_STR : null
                        }
                ).OrderBy(o => o.ROLE_NAME).Distinct().ToList();//otdo distinct

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(ROLE_MENU param, ROLE_MENU dbitem)
        {
            if(dbitem == null)
            {                
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;                
                param.ROLE_ID = dbitem.ROLE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
        }

        public List<RoleMenuExtented> GetAllByLoginEmployee()
        {            
              var data = (from r in context.ROLES

                          join rm in context.ROLE_MENU
                          on r.ROLE_ID equals rm.ROLE_ID

                          join e in context.EMPLOYEES
                          on r.ROLE_ID equals e.ROLE_ID

                          where e.EMP_ID == UserInfo.UserId
                          select new RoleMenuExtented
                          {
                              ROLE_ID = r.ROLE_ID,
                              ROLE_NAME = r.ROLE_NAME,
                              MENU_ID = rm != null ? rm.MENU_ID : 0,
                              MENUS_STR = rm != null ? rm.MENUS_STR : null
                          }
            ).OrderBy(o => o.ROLE_NAME).ToList();

            return data;
        }
    }
}
