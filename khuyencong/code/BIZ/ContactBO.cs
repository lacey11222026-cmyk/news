using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class ContactBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CONTACT;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE

      
        public int CreateUpdateContact(Contact Contact)
        {
            
            int returnVal = ContactDBBase.Create().CreateUpdateContact(Contact);
            if (returnVal != -1)
            {
                UpdateCache(Contact);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

       
       

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Contact by id => add to local cache
        /// </summary>
        /// <param name="ContactId">The Contact id.</param>
        /// <returns></returns>
        public Contact GetContact(int ContactId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_CONTACT + ContactId;

                var item = (Contact)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var Contact = ContactDBBase.Create().GetContact(ContactId);



                LocalCaching.Add(strKeyCached, Contact);

                return Contact;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e,"GetContact");
                return null;
            }
        }

        public List<Contact> GetAllContactsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var Contacts = ContactDBBase.Create().GetAllContactsPaged(pageIndex, pageSize, ref totalRecords);
            if (Contacts == null)
                return null;

            return Contacts.ToList();
        }

       

       
       

       

        

        public List<Contact_FULL> GetContactsByCategory(int categoryId,int status)
        {
            string keyCache = Constants.CACHE_KEY_ALL_CONTACTS_BYCATEGORY + categoryId+"_"+status;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_Contact;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstCachedContacts = (List<Contact_FULL>)LocalCaching.GetData(keyCache);

            if (lstCachedContacts != null)
                return lstCachedContacts;

            var Contacts = ContactDBBase.Create().GetAllContacts(categoryId, status);

            if (Contacts == null)
                return null;

       

            lstCachedContacts = new List<Contact_FULL>();
            foreach (var Contact in Contacts)
            {
                Contact_FULL ContactFull = new Contact_FULL()
                {

                    Id = Contact.Id,
                    CategoryId = Contact.CategoryId,
                    Name = Contact.Name,
                    Yahoo = Contact.Yahoo,
                    CategoryPathway = Contact.CategoryPathway,
                    Mail = Contact.Mail,
                    Mobile = Contact.Mobile,
                    Published = Contact.Published
                    
                };

                lstCachedContacts.Add(ContactFull);
            }

            if (lstCachedContacts.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedContacts);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstCachedContacts;
        }

    


        #endregion

        #region UPDATE

        public void UpdateCache(Contact Contact)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_CONTACT + Contact.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, Contact, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteContacts(string listIds)
        {

            var returnVal = ContactDBBase.Create().DeleteContacts(listIds);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteContact(int id)
        {
            var returnVal = ContactDBBase.Create().DeleteContact(id);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public void FlushAllCache(string containKey)
        {
            DelegateFlushAllCache delegateFlushAllCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        #endregion
    }
}
