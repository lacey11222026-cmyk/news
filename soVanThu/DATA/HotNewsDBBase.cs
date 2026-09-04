using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class HotNewsDBBase: ShopOnlineDBBase
    {
        public static HotNewsDBBase Create ()
        {
            return ( HotNewsDBBase ) Activator.CreateInstance ( typeof ( HotNewsDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateHotNews ( HotNews manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract HotNews  GetHotNews(int Id);
        
        public abstract IEnumerable<HotNews> GetHotNewssDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<HotNews> GetTopLastest(int top,string key,int status);

        #endregion

        #region DELETE STATEMENTs

        

        public abstract int DeleteHotNews ( int manuFactoryId );

      

        #endregion



    }
}
