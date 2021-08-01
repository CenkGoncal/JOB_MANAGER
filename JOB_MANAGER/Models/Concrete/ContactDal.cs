using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using JOB_MANAGER.Helper;

namespace JOB_MANAGER.Models.Concrete
{
    public class ContactDal : EntityRepositoryBase<CONTACTS, JOB_MANAGER_DBEntities, ContactsExtented>
    {
        public override List<ContactsExtented> GetAll2(Expression<Func<CONTACTS, bool>> filter = null)
        {
            var data = (from C in context.CONTACTS
                        join CL in context.CLIENTS on C.CLIENT_ID equals CL.CLIENT_ID
                        join S in context.STATUS on C.CONTACT_STATUS equals S.STATUS_ID
                        where C.IS_CANCELED == false //&& C.CONTACT_ID == id
                        select new ContactsExtented
                        {
                            CONTACT_ID = C.CONTACT_ID,
                            CONTACT_FIRST_NAME = C.CONTACT_FIRST_NAME,
                            CONTACT_LAST_NAME = C.CONTACT_LAST_NAME,
                            CONTACT_NAME = C.CONTACT_FIRST_NAME + SqlConstants.stringWhiteSpace + C.CONTACT_LAST_NAME,
                            CLIENT_ID = C.CLIENT_ID,
                            CLIENT_NAME = CL.CLIENT_NAME,
                            CONTACT_TITLE = C.CONTACT_TITLE,
                            CONTACT_STATUS = C.CONTACT_STATUS,
                            STATUS_NAME = S.STATUS_NAME,
                            DISPLAY_CLASS = S.DISPLAY_CLASS,
                            CONTACT_ROLE = C.CONTACT_ROLE,
                            ROLE_NAME = C.ROLES.ROLE_NAME,
                            CONTACT_PHONE = C.CONTACT_PHONE,
                            CONTACT_PHONE_EX = C.CONTACT_PHONE_EX,
                            CONTACT_PHONE_ALL = C.CONTACT_PHONE + "/" + C.CONTACT_PHONE_EX,
                            CONTACT_MOBILE = C.CONTACT_MOBILE,
                            IS_PRIMARY = C.IS_PRIMARY,
                            CONTACT_EMAIL = C.CONTACT_EMAIL,
                            CREATION_DATE = C.CREATION_DATE != null ?
                                                    C.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (C.CREATION_DATE.Month > 9 ? C.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + C.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    C.CREATION_DATE.Day : null,
                            MODIFIED_DATE = C.MODIFIED_DATE != null ?
                                                    C.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (C.MODIFIED_DATE.Month > 9 ? C.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + C.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    C.MODIFIED_DATE.Day : null,
                        }
               ).ToList();

            return data;
        }

        public override void DeleteControls(CONTACTS entity, ShowState showState)
        {         
            if (context.COMPANY_TYPES.Find(entity.CONTACT_ID) == null)
            {
                showState.ErrorMessage = "Contact not found!!";
                showState.isError = true;
            }
        }

        public override void SaveHelper_ModifyBeforeSave(CONTACTS param, CONTACTS dbitem)
        {
            if (dbitem == null)
            {                
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
                param.COMPANY_ID = ThreadGlobals.UserAuthInfo.Value.CompanyId;
            }
            else
            {                
                param.CREATED_BY = dbitem.CREATED_BY;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.CONTACT_ID = dbitem.CONTACT_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }
    }
}
