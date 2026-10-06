using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class ContactDBSproc : ContactDBBase
    {
        public override int CreateUpdateContact(Contact Contact)
        {
            try
            {
                int? _id = Contact.Id;
                int? _categoryid = Contact.CategoryId;
                string _Name = Contact.Name;
                string _yahoo = Contact.Yahoo;
                string _categoryPathway = Contact.CategoryPathway;
                string _mail = Contact.Mail;
                string _mobile = Contact.Mobile;
                byte? _published = Contact.Published;
               

                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Contact_InsertUpdate(_id, _categoryid, _categoryPathway, _Name, _yahoo, _mail, _mobile, _published);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdateContact");
                return -1;
            }
        }

        public override Contact GetContact(int ContactId)
        {
            var select = "*";
            var where = "Id = " + ContactId;
            var orderBy = string.Empty;

            var results = GetContactsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Contact> GetContactsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Contact_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllContactsPagedDyn select=" + select + "| where" + where);
                return null;
            }
        }

        public override IEnumerable<Contact> GetAllContactsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Name DESC";

            return GetAllContactsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Contact> GetAllContactsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext dc = DataContext)
                {
                    var results = dc.sp_Contact_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllContactsPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Contact> GetAllContacts(int categoryId,int status)
        {
            var select = "*";
            var where = string.Empty;
            string orderBy = "Name DESC";


            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Published =" + status;
            }
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }

            return GetContactsDyn(select, where, orderBy);
        }

       

        public override int DeleteContactDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Contact_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      
        public override int DeleteContact(int ContactId) { var where = "Id =" + ContactId; return DeleteContactDyn(where); }
        public override int DeleteContacts(string lstContactIds) { var where = "Id IN (" + lstContactIds + ")"; return DeleteContactDyn(where); }

    }
}
