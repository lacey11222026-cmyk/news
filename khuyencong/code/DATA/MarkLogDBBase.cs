using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class MarkLogDBBase : ShopOnlineDBBase
    {
        public static MarkLogDBBase Create()
        {
            return (MarkLogDBBase)Activator.CreateInstance(typeof(MarkLogDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateMarkLog(MarkLog MarkLog);

        #endregion

        #region READ STATEMENTs

        public abstract List<MarkLog> GetMarkLog(long ContentId);
        public abstract IEnumerable<MarkLog> GetMarkLogsDyn(string select, string where, string orderBy);
       



        #endregion

       

    }
}
