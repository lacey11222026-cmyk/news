using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class AlbumDBSproc: AlbumDBBase
    {
        public override int CreateUpdateAlbum ( Album Album )
        {
            try
            {
                int? _id = Album.Id;
                
                int? _categoryid = Album.CategoryId;
                string _categoryPathway = Album.CategoryPathway;
                string _createdBy = Album.CreatedBy;
                string _title = Album.Title;
                string _description = Album.Description;
                int? _hits = Album.Hits;
                int? _style = Album.Style;
                
                string _images = Album.Images;
                byte? _status = Album.Status;
                DateTime? _publishDate = Album.PublishDate;
                string _param = Album.Param;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Album_InsertUpdate(_id, _categoryid, _title, _categoryPathway, _description, _createdBy, _publishDate, _status, _hits, _images, _style, _param);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<Album> GetTopAlbumByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);

            var select = " Id,Title,PublishDate,Description,Images";
            if (top > 1)
                select = "TOP(" + top + ") Id,Title,PublishDate,Description,Images";
            var where = "Id IN (" + ids + ") AND Status = 1";
            var orderBy = "Id DESC";

            return GetAlbumsDyn(select, where, orderBy);
        }
        public override IEnumerable<Album> GetTopLastestAlbums(int top, int categoryId)
        {
            var select = " Id,Title,PublishDate,Description,Images,Param";
            if (top > 1)
                select = "TOP(" + top + ") Id,Title,PublishDate,Description,Images,Param";
            var where = "Status = 1";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            var orderBy = "Id DESC";

            return GetAlbumsDyn(select, where, orderBy);
        }
        public override Album GetAlbum ( int AlbumId )
        {
            var select = "*";
            var where = "Id = " + AlbumId;
            var orderBy = string.Empty;

            var results = GetAlbumsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Album> GetAlbumsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Album_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

      

        public override IEnumerable<Album> GetAllAlbumsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_Album_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
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

      

        public override IEnumerable<Album> GetAlbumsByFilter(string keyword, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "[Id] ,[Title] ,[Description],[PublishDate] ,[CreatedBy],[Status] ,[CategoryId] ,[CategoryPathway],[Images],[Hits],[Style]";
     
     
      
            var where = string.Empty;
            var orderBy = "PublishDate DESC";

            if (!string.IsNullOrEmpty(keyword))
            where += "Title LIKE N'%" + keyword + "%' ";
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

            return GetAllAlbumsPagedDyn ( select, where, orderBy ,pageIndex, pageSize, ref totalRecords);
        }

        public override int DeleteAlbumDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Album_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
       

        public override int DeleteAlbum ( int AlbumId ) { var where = "Id =" + AlbumId; return DeleteAlbumDyn ( where ); }
        public override int DeleteAlbums ( string lstAlbumIds ) { var where = "Id IN (" + lstAlbumIds + ")"; return DeleteAlbumDyn ( where ); }


    }
}
