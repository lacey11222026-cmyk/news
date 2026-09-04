using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ManufactoryDBSproc: ManufactoryDBBase
    {
        #region Overrides of ManufactoryDBBase

        public override int CreateUpdateManufactory ( Manufactory manufactory )
        {
            try
            {
                int _id = manufactory.Id;
                string _title = manufactory.Title;
                string _description = manufactory.Description;
                string _image = manufactory.Image;
                string _website = manufactory.Website;
                byte _published = manufactory.Published;
                int _ordering = manufactory.Ordering;
                string _params = manufactory.Params;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Manufactory_InsertUpdate ( _id, _title, _description, _image, _website, _published, _ordering, _params );

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ManufactoryDBSproc", "CreateUpdateManufactory");
                return -1;
            }
        }

        public override Manufactory GetManufactory ( int manuFactoryId )
        {
            var select = "*";
            var where = "Id = " + manuFactoryId;
            var order = string.Empty;

            return GetManufactorysDyn ( select, where, order ).FirstOrDefault ();
        }

        public override IEnumerable<Manufactory> GetAllManufactoriesPaged ( int pageIndex, int pageSize, ref int totalRecords, short? published )
        {
            var select = string.Empty;
            var where = string.Empty;
            if ( published >= 0 )
                where += "Published = " + published;
            var orderBy = "Ordering ASC";

            return GetAllManufactorysPagedDyn ( select, where, orderBy, pageIndex, pageSize, ref totalRecords );
        }

        public IEnumerable<Manufactory> GetAllManufactorysPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                string _select = select;
                string _where = where;
                string _orderBy = orderBy;
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var list = datacontext.sp_Manufactory_SelectPagedDynamic ( _select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecords );
                    return list;
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ManufactoryDBSproc", "GetAllManufactorysPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Manufactory> GetManufactorysDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_Manufactory_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "ManufactoryDBSproc", "GetManufactorysDyn");
                return null;
            }
        }

        public override IEnumerable<Manufactory> GetAllManufactories ( string title )
        {
            var select = "*";
            var where = string.Empty;
            if ( !string.IsNullOrEmpty ( title ) )
                where += "Title LIKE N'%" + title + "%' ";
            var order = "Id DESC";

            return GetManufactorysDyn ( select, where, order );

        }

        public override int DeleteManufactoryDyn ( string where )
        {
            try
            {
                string _where = where;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_Manufactory_DeleteDynamic ( _where );
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle ( exp );
                return -1;
            }
        }

        public override int DeleteManufactory ( int manuFactoryId )
        {
            string where = "Id = " + manuFactoryId;
            return DeleteManufactoryDyn ( where );
        }

        public override int DeleteManufactories ( string listManufactoryId )
        {
            string where = "Id IN (" + listManufactoryId + ")";
            return DeleteManufactoryDyn ( where );
        }

        #endregion
    }
}
