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
                int? _categoryId = manufactory.CategoryId;
                int? ParentId = manufactory.ParentId;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Manufactory_InsertUpdate ( _id, _title, _description, _image, _website, _published, _ordering, _categoryId, ParentId, _params );

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

        public override IEnumerable<Manufactory> GetAllManufactories (int parentId,int categoryId,  int published)
        {
            var select = "*";
            var where = "1=1";
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Published = " + published;
            }    
               

            if (categoryId >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "[CategoryId] = " + categoryId;
            }
            if (parentId >-1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "[ParentId] = " + parentId;
            }
            var order = "Ordering ASC";
            return GetManufactorysDyn ( select, where, order );

        }
        public override int UpdateOrder(int Id, bool upOrder)
        {
            try
            {

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Manufactory_UpdateSortOrder(Id, upOrder);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Manufactory_UpdateStatus(Id, ref responeCode);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
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
