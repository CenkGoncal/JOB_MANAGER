using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class ContactManager : IService<CONTACTS, ContactsExtented>
    {
        private ContactDal _contactDal;
        public ContactManager(ContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        public ShowState AddorUpdate(CONTACTS param)
        {
            return _contactDal.AddorUpdate(param, f => f.CONTACT_ID == param.CONTACT_ID);
        }

        public ShowState Delete(CONTACTS param)
        {
            return _contactDal.Delete(param);
        }

        public CONTACTS Get(CONTACTS param)
        {
            return _contactDal.GetAll(f=>f.CONTACT_ID == param.CONTACT_ID).FirstOrDefault();
        }

        public List<ContactsExtented> GetAll()
        {
            return _contactDal.GetAll2();
        }

        public bool CopyContact(int ClientID, List<int> SourceContact)
        {
            foreach (int ContactID in SourceContact)
            {
                var contact = _contactDal.GetAll(f => f.CONTACT_ID == ContactID && f.CLIENT_ID == ClientID).FirstOrDefault();                
                if (contact != null)
                {
                    CONTACTS insert = new CONTACTS();

                    insert.CONTACT_FIRST_NAME = contact.CONTACT_FIRST_NAME;
                    insert.CONTACT_LAST_NAME = contact.CONTACT_LAST_NAME;
                    insert.CLIENT_ID = ClientID;
                    insert.CONTACT_TITLE = contact.CONTACT_TITLE;
                    insert.CONTACT_STATUS = contact.CONTACT_STATUS;
                    insert.CONTACT_ROLE = contact.CONTACT_ROLE;
                    insert.CONTACT_PHONE_EX = contact.CONTACT_PHONE_EX;
                    insert.CONTACT_PHONE = contact.CONTACT_PHONE;
                    insert.CONTACT_MOBILE = contact.CONTACT_MOBILE;
                    insert.IS_PRIMARY = contact.IS_PRIMARY;
                    insert.CONTACT_EMAIL = contact.CONTACT_EMAIL;

                    var control = _contactDal.AddorUpdate(insert, f => f.COMPANY_ID == -1);                    
                }
            }

            return true;
        }
    }
}
