using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CategoryManufactoryDBBase: ShopOnlineDBBase
    {
        public static CategoryManufactoryDBBase Create ()
        {
            return ( CategoryManufactoryDBBase ) Activator.CreateInstance ( typeof ( CategoryManufactoryDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateCategoryManufactory( CategoryManufactory manuFactory );

        #endregion

        #region READ STATEMENTs

        public abstract List<CategoryManufactory> GetByManuId(int cateid);

        public abstract List<CategoryManufactory> GetByCateId(int cateid);
       

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteCategoryManufactoryDyn ( string where );

        public abstract int DeleteById( int manuFactoryId );

        public abstract int DeleteManufactories( string listCategoryManufactoryId );

        #endregion



    }
}
