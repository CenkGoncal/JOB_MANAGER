using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class NoteTypeDal : EntityRepositoryBase<NOTE_TYPES, JOB_MANAGER_DBEntities, NoteTypeExtented>
    {
  
        public override List<NoteTypeExtented> GetAll2(Expression<Func<NOTE_TYPES, bool>> filter = null)
        {

            var _noteTypeList = filter != null ? context.Set<NOTE_TYPES>().Where(filter).ToList()
               : context.Set<NOTE_TYPES>().ToList();

            var data = (from N in context.NOTE_TYPES

                        join e in context.EMPLOYEES
                        on N.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        select new NoteTypeExtented
                        {
                            NOTE_TYPE_ID = N.NOTE_TYPE_ID,
                            NOTE_TYPE_NAME = N.NOTE_TYPE_NAME,
                            IS_CANCELED = N.IS_CANCELED,
                            CREATION_DATE = N.CREATION_DATE != null ?
                                                        N.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                        (N.CREATION_DATE.Month > 9 ? N.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + N.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                        N.CREATION_DATE.Day : null,
                            MODIFIED_DATE = N.MODIFIED_DATE != null ?
                                                        N.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                        (N.MODIFIED_DATE.Month > 9 ? N.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + N.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                        N.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME

                        }
                    ).OrderBy(o => o.NOTE_TYPE_NAME).ToList();

            return data;
        }

        public override void SaveControls(NOTE_TYPES entity, ShowState showState)
        {
            NOTE_TYPES control = context.NOTE_TYPES.Where(w => w.NOTE_TYPE_ID == entity.NOTE_TYPE_ID).FirstOrDefault();
            
            if (control == null)
            {
                if (context.NOTE_TYPES.Any(x => x.NOTE_TYPE_NAME.Equals(entity.NOTE_TYPE_NAME) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = true;
                }

            }
            else
            {
                if (context.NOTE_TYPES.Any(x => x.NOTE_TYPE_NAME.Equals(entity.NOTE_TYPE_NAME) && x.NOTE_TYPE_ID != entity.NOTE_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(NOTE_TYPES param, NOTE_TYPES dbitem)
        {
            if(dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.NOTE_TYPE_ID = dbitem.NOTE_TYPE_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(NOTE_TYPES entity, ShowState showState)
        {            
            if (context.NOTES.Any(w => w.NOTE_TYPE_ID == entity.NOTE_TYPE_ID))
            {
                showState.ErrorMessage = "Quonte is available to use Note Type!! Please try to check cancelled";
                showState.isError = true;
            }
            else
            if(!context.NOTE_TYPES.Any(w => w.NOTE_TYPE_ID == entity.NOTE_TYPE_ID))
            {
                showState.ErrorMessage = "Note Type not found!!";
                showState.isError = true;
            }
        }
    }
}
