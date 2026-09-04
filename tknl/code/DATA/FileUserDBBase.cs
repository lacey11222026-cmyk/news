using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class FileUserDBBase: ShopOnlineDBBase
    {
        public static FileUserDBBase Create ()
        {
            return ( FileUserDBBase ) Activator.CreateInstance ( typeof ( FileUserDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateFileUser ( FileUser FileUser );

        #endregion

        #region READ STATEMENTs

        public abstract FileUser GetFileUser ( long FileUserId );
      
        public abstract IEnumerable<FileUser> GetFileUsersDyn(string select, string where, string orderBy);

        public abstract IEnumerable<FileUser> GetFileUsersByFilter(int top, string title, string filetype, string username,
                                                                   string fromdate = "", string todate = "");
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteFileUserDyn ( string where );
        public abstract int DeleteFileUser ( long FileUserId );
        public abstract int DeleteFileUsers ( string lstFileUserIds );
        

        #endregion
    }
}
