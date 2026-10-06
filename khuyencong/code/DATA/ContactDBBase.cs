using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class ContactDBBase : ShopOnlineDBBase
    {
        public static ContactDBBase Create()
        {
            return (ContactDBBase)Activator.CreateInstance(typeof(ContactDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateContact(Contact Contact);

        #endregion

        #region READ STATEMENTs

        public abstract Contact GetContact(int ContactId);
        public abstract IEnumerable<Contact> GetContactsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Contact> GetAllContactsPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Contact> GetAllContactsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Contact> GetAllContacts( int categoryId,int status);
       


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteContactDyn(string where);
        public abstract int DeleteContact(int ContactId);
        public abstract int DeleteContacts(string lstContactIds);

        #endregion

    }
}
