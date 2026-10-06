using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class NotiReadDBBase: ShopOnlineDBBase
    {
        public static NotiReadDBBase Create ()
        {
            return ( NotiReadDBBase ) Activator.CreateInstance ( typeof ( NotiReadDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int Read( NotiRead manuFactory );
        public abstract int ReadMulti(int expireDate, string userName, string notiIds);
        #endregion

        #region READ STATEMENTs


        public abstract IEnumerable<NotiRead> GetNotiReadsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<NotiRead> GetNotiRead(string CreateUser, int ExpireDate);

        #endregion

      

    }
}
