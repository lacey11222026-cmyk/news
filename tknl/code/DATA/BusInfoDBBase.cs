using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class BusInfoDBBase : ShopOnlineDBBase
    {
        public static BusInfoDBBase Create()
        {
            return (BusInfoDBBase)Activator.CreateInstance(typeof(BusInfoDBSproc));
        }

        #region READ STATEMENTs

        public abstract BusInfo GetBusInfo(int BusInfoId);
        public abstract IEnumerable<BusInfo> GetBusInfosDyn(string select, string where, string orderBy);
        public abstract IEnumerable<BusInfo> GetAllBusInfosPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<BusInfo> GetAllBusInfosPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<BusInfo> GetAllBusInfos(int cityId, int status);

        #endregion
    }
}
