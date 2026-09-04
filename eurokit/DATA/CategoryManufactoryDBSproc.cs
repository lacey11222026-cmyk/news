using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CategoryManufactoryDBSproc: CategoryManufactoryDBBase
    {
        #region Overrides of CategoryManufactoryDBBase

        public override int CreateUpdateCategoryManufactory ( CategoryManufactory manufactory )
        {
            try
            {
                int _id = manufactory.Id;
                int? _cateid = manufactory.CategoryId;
                int? _manid = manufactory.ManufactoryId;
               

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CategoryManufactory_Insert (_cateid, _manid);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CategoryManufactoryDBSproc", "CreateUpdateCategoryManufactory");
                return -1;
            }
        }


        public override List<CategoryManufactory> GetByManuId(int cateid)
        {

            using (ShopOnlineDataContext datacontext = DataContext)
            {
                var list = datacontext.SP_CategoryManufactory_GetByManufactoryId(cateid).ToList();

                return list;
            }
        }
        public override List<CategoryManufactory> GetByCateId ( int cateid )
        {

            using (ShopOnlineDataContext datacontext = DataContext)
            {
                var list = datacontext.SP_CategoryManufactory_GetByCategoryId(cateid).ToList();
                
                return list;
            }
        }

       

        public override int DeleteCategoryManufactoryDyn ( string where )
        {
            try
            {
                string _where = where;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_CategoryManufactory_DeleteDynamic ( _where );
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle ( exp );
                return -1;
            }
        }

        public override int DeleteById ( int manuFactoryId )
        {
            string where = "Id = " + manuFactoryId;
            return DeleteCategoryManufactoryDyn ( where );
        }

        public override int DeleteManufactories ( string listCategoryManufactoryId )
        {
            string where = "Id IN (" + listCategoryManufactoryId + ")";
            return DeleteCategoryManufactoryDyn ( where );
        }

        #endregion
    }
}
