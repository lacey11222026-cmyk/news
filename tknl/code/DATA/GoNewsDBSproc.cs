using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class GoNewsDBSproc: GoNewsDBBase
    {
        
        public override GoNew GetGoNews ( int attributeId )
        {
            var select = "*";
            var where = "Id = " + attributeId;
            var orderBy = string.Empty;

            var results = GetGoNewssDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<GoNew> GetGoNewssDyn(string select, string where, string orderBy)
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_GoNews_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "GoNewsDBSproc", "GetGoNewssDyn: select" + select);
                return null;
            }
        }

        public override IEnumerable<GoNew> GetAllGoNewssPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Id DESC";

            return GetAllGoNewssPagedDyn ( select, where, orderBy, pageIndex, pageSize, ref totalRecords );
        }

        public override IEnumerable<GoNew> GetAllGoNewssPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_GoNews_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecord );

                    return results;
                }

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "GoNewsDBSproc", "GetAllGoNewssPagedDyn: select" + select);
                return null;
            }
        }


        public override IEnumerable<GoNew> GetAllGoNewssByFilter(int categoryId,string lstcate, int pageIndex, int pageSize, ref int totalRecords,string fromdate,string todate)
        {
            var select = "*";
            var where = "[News_Deleted]=0 ";
            var orderBy = "[News_CreatedDate] DESC";

            if ( categoryId > 0 )
            {
                if ( !string.IsNullOrEmpty ( where ) )
                    where += " AND ";

                where += " [News_CategoryId] =" + categoryId;
            }
            else
            {
                if (!string.IsNullOrEmpty(lstcate))
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";
                    where += " [News_CategoryId] in(" + lstcate+")";
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
                    "(convert(nvarchar(23),Crawl_UpdatedDate,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }



            return GetAllGoNewssPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override int DeleteGoNewsDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_GoNews_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "DeleteGoNewsDyn");
                return -1;
            }
        }

        public override int DeleteGoNews ( int attributeId ) { var where = "Id =" + attributeId; return DeleteGoNewsDyn ( where ); }
        public override int DeleteGoNewss ( string lstGoNewsIds ) { var where = "Id IN (" + lstGoNewsIds + ")"; return DeleteGoNewsDyn ( where ); }


    }
}
