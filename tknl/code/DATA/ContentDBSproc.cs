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
                string _keywords = content.Keywords;
                string _album = content.Album;
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
                string _createdRole = content.CreatedRole;
                string _params = content.Params;
                string _language = content.Language;
                int _SiteId = content.SiteId;
                int IsHot = content.IsHot;
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_InsertUpdate(_id, _categoryid, _title, _categoryPathway, _alias, _keywords, _album, _introtext, _contents, _image, _thumbnail, _url, _createdby, _createddate, _publishDate, _status, _type, _hits,_createdRole,_language, _params,IsHot,_SiteId);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ContentDBSproc", "CreateUpdateContent ");
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
                ExHandler.Handle(exp, "Mark");
                return -1;
            }
        }
        public override IEnumerable<LogView> GetTopViewsContent(int top, string fromdate, string todate)
        {

            var select = "TOP(" + top + ") [ItemId] as Id, Count(Id) as Total";
            var where = "1=1";
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
                    "and (convert(nvarchar(23),Time,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            var orderBy = "Count(Id) DESC";
            var groupBy = "ItemId";
            return GetLogViewDyn(select, where, orderBy, groupBy);
        }
        public override IEnumerable<LogView> GetTopViewsCate(int top, string fromdate, string todate)
        {

            var select = "TOP(" + top + ") [CategoryId] as Id, Count(Id) as Total";
            var where = "CategoryId>=1";
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
                    "and (convert(nvarchar(23),Time,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            var orderBy = "Count(Id) DESC";
            var groupBy = "CategoryId";
            return GetLogViewDyn(select, where, orderBy, groupBy);
        }
        public override IEnumerable<LogView> GetLogViewDyn(string select, string where, string orderBy, string groupby)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.SP_LogView_SelectDynamic(select, where, orderBy, groupby).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
       
        public override int ViewAdd(long Id,int CategoryId)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Content_ViewAdd(Id, CategoryId);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ViewAdd");
                return -1;
            }
        }
        public override Content GetContent(long contentId)
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
                ExHandler.Handle(exp, "ContentDBSproc", "GetContentsDyn, where= " + where);
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
                ExHandler.Handle(exp, "GetContentsMarkDyn", "GetContentsMarkDyn, where= " + where);
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
                if (string.IsNullOrEmpty(orderBy))
                    orderBy = "PublishDate DESC";
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
                ExHandler.Handle(exp, "ContentDBSproc", "GetAllContentsPagedDyn, where= " + where);
                return null;
            }
        }
        public override IEnumerable<Content> GetAllContentsMarkPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords, ref int totalMark)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                int? _totalMark = totalMark;
                //ExHandler.Handle(new Exception(), "ContentDBSproc", "GetAllContentsPagedDyn, where= " + where);
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_ContentMark_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord,ref _totalMark).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);
                    totalMark = Convert.ToInt32(_totalMark);
                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ContentDBSproc", "GetAllContentsMarkPagedDyn, where= " + where);
                return null;
            }
        }
        public override IEnumerable<Content> GetFilterContentsMark(int pageIndex, int pageSize, string title, int categoryId, int status, string createdby, ref int totalRecords, ref int totalMark, string fromdate = "", string todate = "", string orderBy = "PublishDate DESC")
        {
            title = Utils.FormatKeywordSearch(title);
            var select = " Id,Title,PublishDate,IntroText,Image,Url,CategoryId,Album,Hits,Status,[CreatedBy],[CreatedDate],[Mark],Contents";
            var where = string.Empty;
            //var orderBy = "Ordering ASC";
            //var orderBy = "PublishDate DESC";
            if (!string.IsNullOrEmpty(title))
                where += "(Title LIKE N'%" + title + "%' OR Keywords LIKE N'%" + title + "%'  Or IntroText LIKE N'%" + title + "%'  )";
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
                    if (createdby == "notspider")
                    {
                        if (!string.IsNullOrEmpty(where))
                            where += " AND ";

                        where += " Album <> 'Spider'";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(where))
                            where += " AND ";

                        where += " Album =" + "'" + createdby + "'";
                    }

                }
            }
            if (status >= 0)
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

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            return GetAllContentsMarkPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords,ref totalMark);
        }
        public override IEnumerable<Content> GetFilterContentsFrontend(int pageIndex, int pageSize, string title, int categoryId, ref int totalRecords, string fromdate, string todate, string lstNotId,string lang,int type,int isHot)
        {
            title = Utils.FormatKeywordSearch(title);
            var orderBy = "PublishDate DESC";
            var select = " Id,Title,PublishDate,IntroText,Image,Url,CategoryId,Hits";
            var where = "Status=1 ";
            //var orderBy = "Ordering ASC";
            //var orderBy = "PublishDate DESC";
            if (!string.IsNullOrEmpty(title))
                where += " AND (Title LIKE N'%" + title + "%' OR Keywords LIKE N'%" + title + "%'  Or IntroText LIKE N'%" + title + "%'  )";
            if (categoryId > 0)
            {
              

                where += " AND CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if (isHot > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " IsHot =" + isHot;
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
            if (!string.IsNullOrEmpty(where))
                where += " AND ";
            where +=
               " PublishDate <= getdate()";
            return GetAllContentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override IEnumerable<Content> GetFilterContents(int pageIndex, int pageSize, string title, int categoryId, List<int> lstcate,int status, string createdby, ref int totalRecords, string fromdate = "", string todate = "", string lststatus = "", int type = -1, string orderBy = "PublishDate DESC", int checkTime=0)
        {
            var select = " Id,Title,PublishDate,Album,IntroText,Image,Url,CategoryId,Params,Hits,Status,[CreatedBy],[CreatedDate],[Mark],[IsHot],[SiteId],HitsAudio";
            var where = string.Empty;
            //var orderBy = "Ordering ASC";
            //var orderBy = "PublishDate DESC";
            if (!string.IsNullOrEmpty(title))
                where += "(Title LIKE N'%" + title + "%' OR Keywords LIKE N'%" + title + "%'  Or IntroText LIKE N'%" + title + "%'  )";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                if (categoryId == OtherPage.EngPage|| categoryId == 10002)
                {
                    where += " [Language] =" + "'en-us'";
                }
                else
                {
                    where += " CategoryPathway Like '%," + categoryId + ",%' ";
                }
                
            }
            else
            {
                if (lstcate!=null)
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += " ( ";
                    foreach (var cateid in lstcate.Select((value, i) => new {value, i}))
                    {
                        if(cateid.value>0)
                        {
                            if(cateid.i==0)
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
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Type =" + type;
            }
            if (!string.IsNullOrEmpty(createdby))
            {
                
                if (createdby != "-1")
                {
                    if (createdby == "notspider")
                    {
                        if (!string.IsNullOrEmpty(where))
                            where += " AND ";

                        where += " CreatedBy <> 'Spider'";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(where))
                            where += " AND ";

                        where += " CreatedBy =" + "'" + createdby + "'";
                    }
                   
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

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            if(checkTime==1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                   " PublishDate <= getdate()";
            }    
            //NLogLogger.DebugMessage(where);
            //NLogLogger.DebugMessage(orderBy);
            return GetAllContentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override IEnumerable<MarkST> GetSTContentMark(string title, int categoryId, int status, string createdby, string fromdate = "", string todate = "")
        {
            var select = "Album As CreatedBy,ISNULL(SUM(Mark), 0) as TotalMark,Count(Id) as TotalContent";
            var where = "Album <> 'Spider'";
            //var orderBy = "Ordering ASC";
            var orderBy = "TotalMark";
            var groupby = "Album";
            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Title LIKE N'%" + title + "%' ";

            }
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

                    where += " Album =" + "'" + createdby + "'";
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

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            return GetContentsMarkDyn(select, where, orderBy, groupby);
        }
        public override IEnumerable<Content> GetTopContentByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);



            var select = " Id,Title,PublishDate,IntroText,Image,Url,CategoryId,Params,Hits,Status";
            if (top > 0)
                select = "TOP(" + top + ") Id,Title,PublishDate,IntroText,Image,Url,CategoryId,Params,Hits,Status";
            var where = "Id IN (" + ids + ") AND Status = 1";
            var orderBy = "PublishDate DESC";

            return GetContentsDyn(select, where, orderBy);
        }
        public override IEnumerable<Content> GetTopLastestContents(int top, int categoryId, string lang,int IsHot,string title)
        {
            var select = " Id,Title,PublishDate,IntroText,Image,Url,[Contents],CategoryId,Hits,Params,Status";
            if (top >= 1)
                select = "TOP(" + top + ") Id,Title,PublishDate,IntroText,Image,Url,[Contents],CategoryId,Hits,Params,Status";
            var where = "Status = 1";
            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Title LIKE N'%" + title + "%' ";

            }
            else
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                   " PublishDate <= getdate()";
            }
            if (IsHot > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " IsHot =" + IsHot;
            }
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if (!String.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [Language] =" + "'" + lang + "'";
            }
           ;
            var orderBy = "PublishDate DESC";

            return GetContentsDyn(select, where, orderBy);
        }
        public override IEnumerable<Content> GetTopViewContents(int top, int categoryId, string fromdate = "", string todate = "",string lang="")
        {
            var select = " Id,Title,PublishDate,IntroText,Image,Url,[Contents],CategoryId,Hits,Params,Status";
            if (top >= 1)
                select = "TOP(" + top + ") Id,Title,PublishDate,IntroText,Image,Url,[Contents],CategoryId,Hits,Params,Status";
            var where = "Status = 1";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
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
            if (!String.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [Language] =" + "'" + lang + "'";
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
                ExHandler.Handle(exp, "GetContentReport");
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

                ExHandler.Handle(exp, "ContentDBSproc", "DeleteContentDyn, where= " + where);
                return -1;
            }
        }

        public override int DeleteContent(int contentId) { var where = "Id =" + contentId; return DeleteContentDyn(where); }
        public override int DeleteContents(string lstContentIds) { var where = "Id IN (" + lstContentIds + ")"; return DeleteContentDyn(where); }


    }
}
