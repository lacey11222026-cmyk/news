using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ManufactoryDBBase: ShopOnlineDBBase
    {
        public static ManufactoryDBBase Create ()
        {
            return ( ManufactoryDBBase ) Activator.CreateInstance ( typeof ( ManufactoryDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateManufactory ( Manufactory manuFactory );

        #endregion

        #region READ STATEMENTs

        public abstract Manufactory GetManufactory ( int manuFactoryId );
        public abstract IEnumerable<Manufactory> GetAllManufactoriesPaged ( int pageIndex, int pageSize, ref int totalRecords, short? published );
        public abstract IEnumerable<Manufactory> GetManufactorysDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Manufactory> GetAllManufactories ( string title );

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteManufactoryDyn ( string where );

        public abstract int DeleteManufactory ( int manuFactoryId );

        public abstract int DeleteManufactories ( string listManufactoryId );

        #endregion



    }
}
