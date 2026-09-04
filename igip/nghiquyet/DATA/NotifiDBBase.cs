using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class NotifiDBBase: ShopOnlineDBBase
    {
        public static NotifiDBBase Create ()
        {
            return ( NotifiDBBase ) Activator.CreateInstance ( typeof ( NotifiDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateNotifi ( Notifi manuFactory );

        #endregion

        #region READ STATEMENTs

       
        public abstract IEnumerable<Notifi> GetNotifisDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Notifi> GetNotifi(string CreateUser, int ExpireDate);

        #endregion




    }
}
