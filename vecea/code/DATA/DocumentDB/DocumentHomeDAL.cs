using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
namespace DATA.DocumentDB
{
    public class DocumentHomeDAL
    {
        public List<DocumentHome> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
        {
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);
                pars[3] = new SqlParameter("@PageIndex", CurrPage);
                pars[4] = new SqlParameter("@PageSize", PageSize);
                pars[5] = new SqlParameter("@TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<DocumentHome>("sp_Document_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<DocumentHome>();
            }
        }
        public List<DocumentHome> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<DocumentHome>("sp_Document_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<DocumentHome>();
            }
        }
        public List<DocumentHome> GetTopLastestDocuments(int top, int categoryId)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            var orderBy = "Id DESC";

            return SelectDynamic(select, where, orderBy);
        }
        public  DocumentHome GetDocument(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public List<DocumentHome> GetDocumentsSearch(string keyword, int categoryId, int status, int pageIndex, int pageSize, string fromdate, string todate, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "PublishDate DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Name LIKE N'%" + keyword + "%' ";
                where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += "OR Code LIKE N'%" + keyword + "%' )";

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

            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }

            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

    }
}
