using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class AlbumImageDBSproc: AlbumImageDBBase
    {
        public override int CreateUpdateAlbumImage ( AlbumImage albumImage )
        {
            try
            {
                int? _id = albumImage.Id;

                int? _categoryid = albumImage.CategoryId;
                string _categoryPathway = albumImage.CategoryPathway;
                string _createdBy = albumImage.Author;
                string _title = albumImage.Name;
                string _title2 = albumImage.Name2;
                string _description = albumImage.Description;
                string _description2 = albumImage.Description2;
                int? _type = albumImage.Type;

                string _image = albumImage.Image;
                string _image2 = albumImage.Image2;
                int? _status = albumImage.Status;
                DateTime? _publishDate = albumImage.PublishDate;
                string _param = albumImage.Param;
                string _group = albumImage.GroupName;
                string _code = albumImage.Code;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AlbumImage_InsertUpdate(_id, _categoryid, _title, _title2, _code,_group ,_categoryPathway, _description, _description2, _createdBy, _publishDate, _status, _type,_image, _image2, _param);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<AlbumImage> GetTopAlbumByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);

            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Id IN (" + ids + ") AND Status = 1";
            var orderBy = "Id DESC";

            return GetAlbumsDyn(select, where, orderBy);
        }
        public override IEnumerable<AlbumImage> GetTopLastestAlbums(int top, int categoryId)
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

            return GetAlbumsDyn(select, where, orderBy);
        }
        public override AlbumImage GetAlbum(int AlbumId)
        {
            var select = "*";
            var where = "Id = " + AlbumId;
            var orderBy = string.Empty;

            var results = GetAlbumsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<AlbumImage> GetAlbumsDyn(string select, string where, string orderBy)
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AlbumImage_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }



        public override IEnumerable<AlbumImage> GetAllAlbumsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_AlbumImage_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
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



        public override IEnumerable<AlbumImage> GetAlbumsByFilter(string keyword, int categoryId, int status, int type, int pageIndex, int pageSize, ref int totalRecords, string fromdate, string todate, string orderBy)
        {
            var select = "*";
     
     
      
            var where = string.Empty;
            //var orderBy = "PublishDate DESC";
            if (orderBy == "NEWID()")
            {
                if (pageIndex > 1)
                {
                    orderBy = "PublishDate DESC";
                }
            }
            if (!string.IsNullOrEmpty(keyword))
            {

                where += "( GroupName LIKE N'%" + keyword + "%' ";
                where += "OR Author LIKE N'%" + keyword + "%' ";
            
                where += "OR Code LIKE N'%" + keyword + "%' )"; 

            }
            if ( categoryId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " CategoryId =  " + categoryId;
            }
            if ( status > -100 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                if (status > 0)
                {
                    where += " Status >=" + status;
                }
                
                else
                {
                    where += " Status =  " + status;
                }
            }
            if (type >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Type =" + type;
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
                    " (convert(nvarchar(23),PublishDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            
            return GetAllAlbumsPagedDyn ( select, where, orderBy ,pageIndex, pageSize, ref totalRecords);
        }
       
        public override int DeleteAlbumDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_AlbumImage_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override int UpdateAlbumDyn(string where,string updatestr)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_AlbumImage_UpdateDynamic(updatestr,where);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override int Vote(long albumId, int point)
        {
            try
            {
                var where = "Id =" + albumId;
                var condition = " Set TotalVote=TotalVote+1, Point=Point+" + point + ",TotalVote1=TotalVote1+1, Point1=Point1+" + point;
                return UpdateAlbumDyn (where, condition);
    

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override int Vote2(long albumId, int point)
        {
            try
            {
                var where = "Id =" + albumId;
                var condition = " Set TotalVote=TotalVote+1, Point=Point+" + point + ",TotalVote2=TotalVote2+1, Point2=Point2+" + point;
                return UpdateAlbumDyn(where, condition);


            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override int DeleteAlbum ( int albumId ) { var where = "Id =" + albumId; return DeleteAlbumDyn ( where ); }
        public override int DeleteAlbums ( string lstAlbumIds ) { var where = "Id IN (" + lstAlbumIds + ")"; return DeleteAlbumDyn ( where ); }


    }
}
