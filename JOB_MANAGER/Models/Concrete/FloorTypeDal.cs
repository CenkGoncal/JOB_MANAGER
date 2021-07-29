using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class FloorTypeDal : EntityRepositoryBase<FLOOR_TYPES, JOB_MANAGER_DBEntities, FloorTypeExtented>
    {
    
        public override List<FloorTypeExtented> GetAll2(Expression<Func<FLOOR_TYPES, bool>> filter = null)
        {
            var _floortypelist = filter != null ? context.Set<FLOOR_TYPES>().Where(filter).ToList()
                                     : context.Set<FLOOR_TYPES>().ToList();

            var data = (from vb in _floortypelist

                        join e in context.EMPLOYEES
                        on vb.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new FloorTypeExtented
                        {
                            FLOOR_TYPE_ID = vb.FLOOR_TYPE_ID,
                            FLOOR_TYPE_NAME = vb.FLOOR_TYPE_NAME,
                            IS_CANCELED = vb.IS_CANCELED,
                            CREATION_DATE = vb.CREATION_DATE != null ?
                                                    vb.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (vb.CREATION_DATE.Month > 9 ? vb.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + vb.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    vb.CREATION_DATE.Day : null,
                            MODIFIED_DATE = vb.MODIFIED_DATE != null ?
                                                    vb.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (vb.MODIFIED_DATE.Month > 9 ? vb.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + vb.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    vb.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                         ).OrderBy(o => o.FLOOR_TYPE_NAME).ToList();

            return data;
        }

        public override void SaveControls(FLOOR_TYPES entity, ShowState showState)
        {
            FLOOR_TYPES control = context.FLOOR_TYPES.Where(w => w.FLOOR_TYPE_ID == entity.FLOOR_TYPE_ID).FirstOrDefault();
            
            if (control == null)
            {                
                if (context.FLOOR_TYPES.Any(x => x.FLOOR_TYPE_NAME.Equals(entity.FLOOR_TYPE_NAME) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Floor Type already exists.";
                    showState.isError = true;
                }
            }
            else
            {
                if (context.FLOOR_TYPES.Any(x => x.FLOOR_TYPE_NAME.Equals(entity.FLOOR_TYPE_NAME) && x.FLOOR_TYPE_ID != entity.FLOOR_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Floor Type already exists.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(FLOOR_TYPES param, FLOOR_TYPES dbitem)
        {
            if(dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.FLOOR_TYPE_ID = dbitem.FLOOR_TYPE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(FLOOR_TYPES entity, ShowState showState)
        {
            if(!context.FLOOR_TYPES.Any(w=>w.FLOOR_TYPE_ID == entity.FLOOR_TYPE_ID))
            {                
                showState.ErrorMessage = "Floor Type not found!!";
                showState.isError = true;
            }
            else
            if (context.QUOTES.Any(w => w.FLOOR_TYPE == entity.FLOOR_TYPE_ID))
            {
                showState.ErrorMessage = "Quante is available to use floor Type!! Please try to check cancelled";
                showState.isError = true;
            }
        }
    }
}
