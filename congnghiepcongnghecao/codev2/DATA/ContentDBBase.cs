using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ContentDBBase : ShopOnlineDBBase
    {
        public static ContentDBBase Create()
        {
            return (ContentDBBase)Activator.CreateInstance(typeof(ContentDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateContent(Content content);
        public abstract int Mark(long Id, float Mark);
        public abstract int ViewAdd(long Id,int CategoryId);
        #endregion

        #region READ STATEMENTs
        public abstract IEnumerable<LogView> GetLogViewDyn(string select, string where, string orderBy, string groupby);
        public abstract IEnumerable<LogView> GetTopViewsContent(int top, string fromdate, string todate);
        public abstract IEnumerable<LogView> GetTopViewsCate(int top, string fromdate, string todate);
        public abstract Content GetContent(int contentId);
        public abstract IEnumerable<Content> GetContentsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Content> GetAllContentsPaged(int pageIndex, int pageSize, ref int totalRecords);

        public abstract IEnumerable<MarkST> GetContentsMarkDyn(string select, string where, string orderBy,
                                                                string groupby);
        public abstract IEnumerable<Content> GetAllContentsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Content>  GetFilterContents(int pageIndex, int pageSize, string title, int categoryId, List<int> lstcate, int status, string createdby, ref int totalRecords, string fromdate = "", string todate = "", string lststatus = "", string alias = "",string orderBy= "PublishDate DESC");

        public abstract IEnumerable<MarkST> GetSTContentMark(string title, int categoryId, int status, string createdby,
                                                             string fromdate = "", string todate = "");
        public abstract IEnumerable<Content> GetFilterContentsFrontend(int pageIndex, int pageSize, string title, int categoryId, ref int totalRecords, string fromdate, string todate, string lstNotId, string lang, int type,int site);
        public abstract IEnumerable<Content> GetTopLastestContents(int top, int categoryId, int site);
        public abstract IEnumerable<Content> GetTopViewContents(int top, int categoryId, string title, string fromdate, string todate);
        public abstract IEnumerable<Content> GetTopContentByIds(string ids, int top);
        public abstract List<Statistic> GetReport(int categoryid, int year);

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteContentDyn(string where);
        public abstract int DeleteContent(int contentId);
        public abstract int DeleteContents(string lstContentIds);

        #endregion
    }
}
