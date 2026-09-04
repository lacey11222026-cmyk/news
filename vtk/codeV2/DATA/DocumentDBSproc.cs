using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class DocumentDBSproc: DocumentDBBase
    {
        public override int CreateUpdateDocument ( Document Document )
        {
            try
            {
                int? _id = Document.Id;
                
                int? _categoryid = Document.CategoryId;
                string _categoryPathway = Document.CategoryPathway;
                string _createdBy = Document.CreatedBy;
                string _name = Document.Name;
                string _code = Document.Code;
                string _description = Document.Description;
                int? _hits = Document.Hits;

                string _signedBy = Document.SignedBy;
                string _signedByDesc = Document.SignedByDesc;
                
                string _filepath = Document.FilePath;
                byte? _status = Document.Status;

                DateTime? _publishDate = Document.PublishDate;
                DateTime? _createdDate = Document.CreatedDate;
                DateTime? _effectiveDate = Document.EffectiveDate;
                DateTime? _expiryDate = Document.ExpiryDate;
                //int? _private = Document.Private;
                int _SiteId = Document.SiteId;
                string _SiteUrl = Document.SiteUrl;


                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Document_InsertUpdate(_id, _categoryid, _name, _categoryPathway, _description, _createdBy, _publishDate, _createdDate, _effectiveDate, _expiryDate, _status, _hits, _filepath, _code, _signedBy, _signedByDesc, _SiteId,_SiteUrl);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<Document> GetTopLastestDocuments(int top, int categoryId)
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
            var orderBy = "PublishDate DESC";

            return GetDocumentsDyn(select, where, orderBy);
        }
        public override Document GetDocument ( int DocumentId )
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = GetDocumentsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Document> GetDocumentsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Document_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

      

        public override IEnumerable<Document> GetAllDocumentsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_Document_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecord );

                    return results;
                }

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }


        public override IEnumerable<Document> GetDocumentsSearch(string keyword, int categoryId, int status, int pageIndex, int pageSize, string fromdate, string todate, ref int totalRecords)
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

            return GetAllDocumentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Document> GetDocumentsByFilter(string keyword, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";


            keyword = Utils.FormatKeywordSearch(keyword);
            var where = string.Empty;
            var orderBy = "PublishDate DESC";

            if (!string.IsNullOrEmpty(keyword))
            where += "Name LIKE N'%" + keyword + "%' ";
            if ( categoryId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if ( status >= 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " Status =" + status;
            }

            return GetAllDocumentsPagedDyn ( select, where, orderBy ,pageIndex, pageSize, ref totalRecords);
        }

        public override int DeleteDocumentDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Document_DeleteDynamic ( where );

            }
            catch ( Exception exp )
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
                    //return datacontext.sp_Document_ViewAdd(Id);
                    return 1;

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override int DeleteDocument ( int DocumentId ) { var where = "Id =" + DocumentId; return DeleteDocumentDyn ( where ); }
        public override int DeleteDocuments ( string lstDocumentIds ) { var where = "Id IN (" + lstDocumentIds + ")"; return DeleteDocumentDyn ( where ); }


    }
}
