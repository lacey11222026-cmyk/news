using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class SupportDBBase : ShopOnlineDBBase
    {
        public static SupportDBBase Create()
        {
            return (SupportDBBase)Activator.CreateInstance(typeof(SupportDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateSupport(Support support);

        #endregion

        #region READ STATEMENTs

        public abstract Support GetSupport(int supportId);
        public abstract IEnumerable<Support> GetSupportsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Support> GetAllSupportsPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Support> GetAllSupportsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Support> GetAllSupports(string name, int categoryId);
        public abstract IEnumerable<Support> GetTopSupports(int top, int published);


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteSupportDyn(string where);
        public abstract int DeleteSupport(int supportId);
        public abstract int DeleteSupports(string lstSupportIds);

        #endregion

    }
}
