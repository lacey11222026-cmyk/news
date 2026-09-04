using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UTILS;

namespace DATA
{
    public class ContentLogDBSproc : ContentLogDBBase
    {
        public override int CreateUpdateContentLog(ContentLog ContentLog)
        {
            try
            {
                long? _id = ContentLog.Id;
                long? _contentId = ContentLog.ItemId;
                int? _Type = ContentLog.Type;
               int? _ItemtType = ContentLog.ItemtType;
               string ItemName = ContentLog.ItemName;
                string _reason = ContentLog.Note;
                string _description = ContentLog.Description;
                string _userName = ContentLog.UserName;
                string _contents = ContentLog.Contents;
                string _introtext = ContentLog.IntroText;
                string _url = ContentLog.Url;

                string _title = ContentLog.Title;
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_ContentLog_InsertUpdate(_id, _userName, _Type, _contentId, _ItemtType, ItemName, _reason, _description,_introtext, _contents,_url,_title);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdateContentLog");
                return -1;
            }
        }

        public override List<ContentLog> GetContentLog(long ItemId,int type)
        {
            var select = "Id,UserName,ItemId,ItemName,Type,Note,Description,CreateTime";
            var where = "ItemId = " + ItemId;
            var orderBy = "Id DESC";

            var results = GetContentLogsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.ToList();
        }
        public override ContentLog GetById(long id)
        {
            var select = "*";
            var where = "Id = " + id;
            var orderBy = "Id DESC";

            var results = GetContentLogsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public override IEnumerable<ContentLog> GetContentLogsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_ContentLog_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllContentLogsPagedDyn select=" + select + "| where" + where);
                return null;
            }
        }


        public override IEnumerable<ContentLog> GetAllPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_ContentLog_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
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

        public override IEnumerable<ContentLog> GetByFilter(string UserName, int itemtType, long itemid, string title, int pageIndex, int pageSize, ref int totalRecords, string fromdate = "", string todate = "")
        {
            var select = "*";

            UserName = Utils.FormatKeywordSearch(UserName);
            title = Utils.FormatKeywordSearch(title);
            var where = "1=1 ";
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " ( ItemName LIKE N'%" + title + "%') ";
            }

            if (!string.IsNullOrEmpty(UserName))
            {

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " UserName =" + "'" + UserName + "'";


            }

            if (itemtType > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " ItemtType=" + itemtType.ToString();
            }
            if (itemid > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " ItemId=" + itemid.ToString();
            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);


                where +=
                    " (convert(nvarchar(23),[CreateTime],121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            return GetAllPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }







    }
}
