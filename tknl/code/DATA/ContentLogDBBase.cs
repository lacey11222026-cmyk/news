using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class ContentLogDBBase : ShopOnlineDBBase
    {
        public static ContentLogDBBase Create()
        {
            return (ContentLogDBBase)Activator.CreateInstance(typeof(ContentLogDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateContentLog(ContentLog ContentLog);

        #endregion

        #region READ STATEMENTs

        public abstract List<ContentLog> GetContentLog(long ContentId,int type);
        public abstract IEnumerable<ContentLog> GetContentLogsDyn(string select, string where, string orderBy);
        public abstract  ContentLog GetById(long id);

        public abstract IEnumerable<ContentLog> GetAllPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);

        public abstract IEnumerable<ContentLog> GetByFilter(string UserName, int type, long itemid, string itemName, int pageIndex, int pageSize, ref int totalRecords, string fromdate = "", string todate = "");



        #endregion



    }
}
