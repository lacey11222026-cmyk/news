using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ShopDBBase: ShopOnlineDBBase
    {
        public static ShopDBBase Create ()
        {
            return ( ShopDBBase ) Activator.CreateInstance ( typeof ( ShopDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateShop ( Shop manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Shop  GetShop(int Id);
        public abstract IEnumerable<Shop> GetAllPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published);
        public abstract IEnumerable<Shop> GetShopsDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Shop> GetTopLastest(int top,int type);

        #endregion

        #region DELETE STATEMENTs

        

        public abstract int DeleteShop ( int manuFactoryId );

      

        #endregion



    }
}
