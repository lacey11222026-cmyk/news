using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Newtonsoft.Json;

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
        /// <param name="contactId">The Contact id.</param>
        /// <returns></returns>
        public Contact GetContact(int contactId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_CONTACT + contactId;

                var data = LocalCaching.GetData(strKeyCached);
                if (data != null)
                    return JsonConvert.DeserializeObject<Contact>(data.ToString());
                   

                var contact = ContactDBBase.Create().GetContact(contactId);



                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(contact));

                return contact;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e,"GetContact");
                return null;
            }
        }

        public List<Contact> GetAllContactsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var contacts = ContactDBBase.Create().GetAllContactsPaged(pageIndex, pageSize, ref totalRecords);
            if (contacts == null)
                return null;

            return contacts.ToList();
        }

       
        

        public List<Contact_FULL> GetContactsByCategory(int categoryId,int status)
        {
            string strKeyCached = Constants.CACHE_KEY_ALL_CONTACTS_BYCATEGORY + categoryId + "_" + status;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_Contact;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(strKeyCached) == -1)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Contact_FULL>>(data.ToString());

            var contacts = ContactDBBase.Create().GetAllContacts(categoryId, status);

            if (contacts == null)
                return null;

       

            var lstCachedContacts = new List<Contact_FULL>();
            foreach (var Contact in contacts)
            {
                Contact_FULL contactFull = new Contact_FULL()
                {

                    Id = Contact.Id,
                    CategoryId = Contact.CategoryId,
                    Name = Contact.Name,
                    Role = Contact.Role,
                    CategoryPathway = Contact.CategoryPathway,
                    Mail = Contact.Mail,
                    Mobile = Contact.Mobile,
                    Published = Contact.Published
                    
                };

                lstCachedContacts.Add(contactFull);
            }

            if (lstCachedContacts.Count > 0)
            {
                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstCachedContacts));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstCachedContacts;
        }

    


        #endregion

        #region UPDATE

        public void UpdateCache(Contact contact)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_CONTACT + contact.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(contact), null, null);

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
