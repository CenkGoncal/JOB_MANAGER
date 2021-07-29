using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class DocumentTypeDal : EntityRepositoryBase<DOCUMENT_TYPES, JOB_MANAGER_DBEntities, DocumentTypeExtented>
    {    

        public override List<DocumentTypeExtented> GetAll2(Expression<Func<DOCUMENT_TYPES, bool>> filter = null)
        {
            var _documentTypeList = filter != null ? context.Set<DOCUMENT_TYPES>().Where(filter).ToList()
                    : context.Set<DOCUMENT_TYPES>().ToList();

            var data = (from D in _documentTypeList

                        join e in context.EMPLOYEES
                        on D.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()

                        where D.IS_CANCELED == false
                        select new DocumentTypeExtented
                        {
                            DOCUMENT_TYPE_ID = D.DOCUMENT_TYPE_ID,
                            DOCUMENT_TYPE_NAME = D.DOCUMENT_TYPE_NAME,
                            DOCUMENT_EXTENSION = D.DOCUMENT_EXTENSION,
                            FONT_AWESOME_ICON = D.FONT_AWESOME_ICON,
                            IS_CANCELED = D.IS_CANCELED,
                            CREATION_DATE = D.CREATION_DATE != null ?
                                                        D.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                        (D.CREATION_DATE.Month > 9 ? D.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + D.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                        D.CREATION_DATE.Day : null,
                            MODIFIED_DATE = D.MODIFIED_DATE != null ?
                                                        D.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                        (D.MODIFIED_DATE.Month > 9 ? D.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + D.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                        D.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME

                        }
                    ).OrderBy(o => o.DOCUMENT_TYPE_NAME).ToList();

            return data;
        }

        public override void SaveControls(DOCUMENT_TYPES entity, ShowState showState)
        {
            DOCUMENT_TYPES control = context.DOCUMENT_TYPES.Where(w => w.DOCUMENT_TYPE_ID == entity.DOCUMENT_TYPE_ID).FirstOrDefault();
            
            if (control == null)
            {                
                if (context.DOCUMENT_TYPES.Any(x => x.DOCUMENT_TYPE_NAME.Equals(entity.DOCUMENT_TYPE_NAME) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = true;
                }

                if (context.DOCUMENT_TYPES.Any(x => x.DOCUMENT_EXTENSION.Equals(entity.DOCUMENT_EXTENSION) && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Doc ext. already exists.";
                    showState.isError = true;
                }
            }
            else
            {
                if (context.DOCUMENT_TYPES.Any(x => x.DOCUMENT_TYPE_NAME.Equals(entity.DOCUMENT_TYPE_NAME) && x.DOCUMENT_TYPE_ID != entity.DOCUMENT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Type already exists.";
                    showState.isError = true;
                }

                if (context.DOCUMENT_TYPES.Any(x => x.DOCUMENT_EXTENSION.Equals(entity.DOCUMENT_EXTENSION) && x.DOCUMENT_TYPE_ID != entity.DOCUMENT_TYPE_ID && x.IS_CANCELED == false))
                {
                    showState.ErrorMessage = "Doc ext. already exists.";
                    showState.isError = true;
                }
            }
        }

        public override void SaveHelper_ModifyBeforeSave(DOCUMENT_TYPES param, DOCUMENT_TYPES dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.DOCUMENT_TYPE_ID = dbitem.DOCUMENT_TYPE_ID;
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(DOCUMENT_TYPES entity, ShowState showState)
        {
            if (context.DOCUMENTS.Any(w => w.DOCUMENT_TYPE_ID == entity.DOCUMENT_TYPE_ID))
            {
                showState.ErrorMessage = "Quonte or Project are available to use Document Type!! Please try to check cancelled";
                showState.isError = true;
            }
            else
            if (!context.DOCUMENT_TYPES.Any(w => w.DOCUMENT_TYPE_ID == entity.DOCUMENT_TYPE_ID))
            {
                showState.ErrorMessage = "Document Type not found!!";
                showState.isError = true;
            }
        }
    }
}
