using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class RoleMenuDal: EntityRepositoryBase<ROLE_MENU, JOB_MANAGER_DBEntities, RoleMenuExtented>
    {

        public override List<RoleMenuExtented> GetAll2(Expression<Func<ROLE_MENU, bool>> filter = null)
        {
            var _rolesmenu = filter != null ? context.Set<ROLE_MENU>().Where(filter).ToList()
             : context.Set<ROLE_MENU>().ToList();

            var data = (from rm in _rolesmenu                        
                        join r in context.ROLES on rm.ROLE_ID equals r.ROLE_ID

                        select new RoleMenuExtented
                        {
                            ROLE_ID = r.ROLE_ID,
                            ROLE_NAME = r.ROLE_NAME,
                            MENU_ID = rm.MENU_ID,
                            MENUS_STR = rm.MENUS_STR
                        }
                ).OrderBy(o => o.ROLE_NAME).ToList();//otdo distinct

            return data.Distinct().ToList();
        }

        public override void SaveHelper_ModifyBeforeSave(ROLE_MENU param, ROLE_MENU dbitem)
        {
            if(dbitem == null)
            {                
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;                
                param.ROLE_ID = dbitem.ROLE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public List<RoleMenuExtented> GetAllByLoginEmployee()
        {            
              var data = (from r in context.ROLES

                          join rm in context.ROLE_MENU
                          on r.ROLE_ID equals rm.ROLE_ID

                          join e in context.EMPLOYEES
                          on r.ROLE_ID equals e.ROLE_ID

                          where e.EMP_ID == ThreadGlobals.UserAuthInfo.Value.UserId
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
