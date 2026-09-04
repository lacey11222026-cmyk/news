using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ContentDBSproc : ContentDBBase
    {

        public override int CreateUpdateContent(Content content)
        {
            try
            {
                long? _id = content.Id;
                int? _categoryid = content.CategoryId;
                string _title = content.Title;
                string _categoryPathway = content.CategoryPathway;
                string _alias = content.Alias;
                string _introtext = content.IntroText;
                string _contents = content.Contents;
                string _image = content.Image;
                string _thumbnail = content.Thumbnail;
                string _url = content.Url;
                string _createdby = content.CreatedBy;
                System.DateTime? _createddate = content.CreatedDate;

                System.DateTime? _publishDate = content.PublishDate;
                byte? _status = content.Status;
                byte? _type = content.Type;
                int? _hits = content.Hits;
                string _params = content.Params;

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_InsertUpdate(_id, _categoryid, _title, _categoryPathway, _alias, _introtext, _contents, _image, _thumbnail, _url, _createdby, _createddate, _publishDate, _status, _type, _hits, _params);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override int Mark(long Id, float Mark)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_Mark(Id, Mark);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override int ViewAdd(long Id)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_ViewAdd(Id);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override Content GetContent(int contentId)
        {
            var select = "*";
            var where = "Id = " + contentId;
            var orderBy = string.Empty;

            var results = GetContentsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Content> GetContentsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override IEnumerable<MarkST> GetContentsMarkDyn(string select, string where, string orderBy, string groupby)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_SelectMark(select, where, orderBy, groupby).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override IEnumerable<Content> GetAllContentsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "PublishDate DESC";

            return GetAllContentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Content> GetAllContentsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                //ExHandler.Handle(new Exception(), "ContentDBSproc", "GetAllContentsPagedDyn, where= " + where);
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_Content_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
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

        public override IEnumerable<Content> GetFilterContents(int pageIndex, int pageSize, string title, int categoryId, List<int> lstcate, int status, string createdby, ref int totalRecords,string fromdate="",string todate="", string lststatus = "",string alias="")
        {
            title = Utils.FormatKeywordSearch(title);
            var select = "*";
            var where = string.Empty;
            //var orderBy = "Ordering ASC";
            var orderBy = "PublishDate DESC";
            if (!string.IsNullOrEmpty(title))
                where += "Title LIKE N'%" + title + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";

            }
            else
            {
                if (lstcate != null)
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += " ( ";
                    foreach (var cateid in lstcate.Select((value, i) => new { value, i }))
                    {
                        if (cateid.value > 0)
                        {
                            if (cateid.i == 0)
                            {
                                where += " CategoryPathway Like '%," + cateid.value + ",%' ";
                            }
                            else
                            {
                                where += "OR  CategoryPathway Like '%," + cateid.value + ",%' ";
                            }

                        }
                    }
                    where += " ) ";
                }
            }
            if (!string.IsNullOrEmpty(createdby))
            {
                if (createdby!="-1")
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += " CreatedBy =" + "'" + createdby + "'";
                }
               
            }
            if (!string.IsNullOrEmpty(alias))
            {
                if (alias != "-1")
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += " Alias =" + "'" + alias + "'";
                }

            }
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status =" + status;
            }
            else
            {
                if (!string.IsNullOrEmpty(lststatus))
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";
                    if (lststatus.EndsWith(","))
                        lststatus = lststatus.Remove(lststatus.Length - 1);
                    where += "Status IN (" + lststatus + ")";

                }
            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);


                where +=
                    "and (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            return GetAllContentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override IEnumerable<MarkST> GetSTContentMark(string title, int categoryId, int status, string createdby,string fromdate = "", string todate = "")
        {
            var select = "Alias,Sum(Mark) as TotalMark,Count(Id) as TotalContent";
            var where = string.Empty;
            //var orderBy = "Ordering ASC";
            var orderBy = "TotalMark";
            var groupby = "Alias";
            if (!string.IsNullOrEmpty(title))
                where += "Title LIKE N'%" + title + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if (!string.IsNullOrEmpty(createdby))
            {
                if (createdby != "-1")
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += " Alias =" + "'" + createdby + "'";
                }

            }
            if (status > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status =" + status;
            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);


                where +=
                    " and (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            //ExHandler.Handle(new Exception("mark"), "GetContentsMarkDyn", "GetContentsMarkDyn, where= " + where+ " group= "+groupby +" order= "+orderBy);
            return GetContentsMarkDyn(select, where, orderBy, groupby);
        }
        public override IEnumerable<Content> GetFilterContentsFrontend(int pageIndex, int pageSize, string title, int categoryId, ref int totalRecords, string fromdate, string todate, string lstNotId, string lang, int type)
        {
            title = Utils.FormatKeywordSearch(title);
            var orderBy = "PublishDate DESC";
            var select = " Id,Title,PublishDate,IntroText,Image,Url,CategoryId,Hits";
            var where = "Status=4 ";
            //var orderBy = "Ordering ASC";
            //var orderBy = "PublishDate DESC";
            if (!string.IsNullOrEmpty(title))
                where += "AND Title LIKE N'%" + title + "%' ";
            if (categoryId > 0)
            {


                where += " AND CategoryPathway Like '%," + categoryId + ",%' ";
            }

            if (type > 0)
            {


                where += " AND Type =" + type;
            }
            if (!String.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [Language] =" + "'" + lang + "'";
            }
            if (!string.IsNullOrEmpty(lstNotId))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                if (lstNotId.EndsWith(","))
                    lstNotId = lstNotId.Remove(lstNotId.Length - 1);
                where += "Id Not IN (" + lstNotId + ")";

            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            return GetAllContentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override IEnumerable<Content> GetTopContentByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);

            var select = " Id,Title,PublishDate,IntroText,Image,Url,CategoryId";
            if (top > 0)
                select = "TOP(" + top + ") Id,Title,PublishDate,IntroText,Image,Url,CategoryId";
            var where = "Id IN (" + ids + ") AND Status = 4";
            var orderBy = "PublishDate DESC";

            return GetContentsDyn(select, where, orderBy);
        }
        public override IEnumerable<Content> GetTopLastestContents(int top, int categoryId)
        {
            var select = " Id,Title,PublishDate,CreatedDate,IntroText,Image,Url,[Contents],[Type]";
            if (top >= 1)
                select = "TOP(" + top + ") Id,Title,CreatedDate,PublishDate,IntroText,Image,Url,[Contents],[Type]";
            var where = "Status = 4";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            var orderBy = "PublishDate DESC";

            return GetContentsDyn(select, where, orderBy);
        }
        public override IEnumerable<Content> GetTopViewContents(int top, int categoryId,string title)
        {
            title = Utils.FormatKeywordSearch(title);
            var select = " Id,Title,PublishDate,IntroText,Image,Url";
            if (top >= 1)
                select = "TOP(" + top + ") Id,Title,PublishDate,IntroText,Image,Url";
            var where = "Status = 4";
            if (!string.IsNullOrEmpty(title))
                where += "AND Title LIKE N'%" + title + "%' ";
            if (categoryId > 0)
            {
              
                where += "AND CategoryPathway Like '%," + categoryId + ",%' ";
            }
            var orderBy = "Hits DESC";

            return GetContentsDyn(select, where, orderBy);
        }

        public override List<Statistic> GetReport(int categoryid, int year)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Content_ByReport(categoryid, year).ToList();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override int DeleteContentDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_DeleteDynamic(where);

            }
            catch (Exception exp)
            {

                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override int DeleteContent(int contentId) { var where = "Id =" + contentId; return DeleteContentDyn(where); }
        public override int DeleteContents(string lstContentIds) { var where = "Id IN (" + lstContentIds + ")"; return DeleteContentDyn(where); }


    }
}
