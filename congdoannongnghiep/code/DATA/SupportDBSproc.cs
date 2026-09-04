using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class SupportDBSproc : SupportDBBase
    {
        public override int CreateUpdateSupport(Support support)
        {
            try
            {
                int _id = support.Id;
                int? _categoryid = support.CategoryId;
                string _supporter = support.Supporter;
                string _yahoo = support.Yahoo;
                string _skype = support.Skype;
                string _mail = support.Mail;
                string _phone = support.Phone;
                string _mobile = support.Mobile;
                byte? _published = support.Published;
                byte? _ordering = support.Ordering;
                string _params = support.Params;

                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Support_InsertUpdate(_id, _categoryid, _supporter, _yahoo, _skype, _mail, _phone, _mobile, _published, _ordering, _params);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return -1;
            }
        }

        public override Support GetSupport(int supportId)
        {
            var select = "*";
            var where = "Id = " + supportId;
            var orderBy = string.Empty;

            var results = GetSupportsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Support> GetSupportsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Support_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return null;
            }
        }

        public override IEnumerable<Support> GetAllSupportsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Ordering ASC";

            return GetAllSupportsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Support> GetAllSupportsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext dc = DataContext)
                {
                    var results = dc.sp_Support_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return null;
            }
        }

        public override IEnumerable<Support> GetAllSupports(string name, int categoryId)
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(name))
                where += "Supporter LIKE N'%" + name + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryId =" + categoryId;
            }

            return GetSupportsDyn(select, where, orderBy);
        }

        public override IEnumerable<Support> GetTopSupports(int top, int published)
        {
            var select = string.Empty;
            var where = string.Empty;
            var orderBy = "Ordering ASC,Supporter ASC";


            if (top > 0)
            {
                select = "TOP (" + top + ") *";
            }

            if (published > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Published =" + published;
            }



            return GetSupportsDyn(select, where, orderBy);
        }

        public override int DeleteSupportDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Support_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return -1;
            }
        }

        public override int DeleteSupport(int supportId) { var where = "Id =" + supportId; return DeleteSupportDyn(where); }
        public override int DeleteSupports(string lstSupportIds) { var where = "Id IN (" + lstSupportIds + ")"; return DeleteSupportDyn(where); }

    }
}
