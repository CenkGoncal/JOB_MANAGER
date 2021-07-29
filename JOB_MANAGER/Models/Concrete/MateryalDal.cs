using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class MateryalDal : EntityRepositoryBase<MATERIALS, JOB_MANAGER_DBEntities, MaterialExtented>
    {     

        public override List<MaterialExtented> GetAll2(Expression<Func<MATERIALS, bool>> filter = null)
        {
            var _materialist = filter != null ? context.Set<MATERIALS>().Where(filter).ToList()
               : context.Set<MATERIALS>().ToList();

            var data = (from vb in _materialist

                        join e in context.EMPLOYEES
                        on vb.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new MaterialExtented
                        {
                            MATERIAL_ID = vb.MATERIAL_ID,
                            MATERIAL_NAME = vb.MATERIAL_NAME,
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
                    ).OrderBy(o => o.MATERIAL_NAME).ToList();

            return data;
        }

        public override void SaveControls(MATERIALS entity, ShowState showState)
        {
            MATERIALS control = context.MATERIALS.Where(w => w.MATERIAL_ID == entity.MATERIAL_ID).FirstOrDefault();
            
            if (control == null)
            {                
                if (context.MATERIALS.Any(x => x.MATERIAL_NAME.Equals(entity.MATERIAL_NAME) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Material already exists.";
                    showState.isError= true;
                }

            }
            else
            {
                if (context.MATERIALS.Any(x => x.MATERIAL_NAME.Equals(entity.MATERIAL_NAME) && x.MATERIAL_ID != entity.MATERIAL_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Material already exists.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(MATERIALS param, MATERIALS dbitem)
        {
            if(dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.MATERIAL_ID = dbitem.MATERIAL_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(MATERIALS entity, ShowState showState)
        {
            if(!context.MATERIALS.Any(w => w.MATERIAL_ID == entity.MATERIAL_ID))
            {
                showState.ErrorMessage = "Material not found!!";
                showState.isError = true;
            }
            else
            if (context.QUOTES.Any(w => w.MATERIAL_ID == entity.MATERIAL_ID))
            {
                showState.ErrorMessage = "Quante is available to use material!! Please try to check cancelled";
                showState.isError = true;
            }
        }
    }
}
