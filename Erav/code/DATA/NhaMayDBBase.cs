using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class NhaMayDBBase : ShopOnlineDBBase
    {
        public static NhaMayDBBase Create()
        {
            return (NhaMayDBBase)Activator.CreateInstance(typeof(NhaMayDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateNhaMay(NhaMay NhaMay);

        #endregion

        #region READ STATEMENTs

        public abstract NhaMay GetNhaMay(int NhaMayId);

        public abstract IEnumerable<NhaMay> GetAllNhaMaysPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);

        public abstract IEnumerable<NhaMay> GetNhaMaysByFilter(string title, int pageIndex, int pageSize, ref int totalRecords, int loai = -1, int hinhthuc = -1, int status = -1, string fromdate = "", string todate = "");
        public abstract IEnumerable<NhaMay> GetNhaMaysDyn(string select, string where, string orderBy);
        public abstract IEnumerable<NhaMay> GetTopLastestNhaMays(int top);


        #endregion

        #region DELETE STATEMENTs
        //public abstract int UpdateOrder(int Id, bool upOrder);
        public abstract int DeleteNhaMayDyn(string where);
        public abstract int DeleteNhaMay(int NhaMayId);
        public abstract int DeleteNhaMays(string lstNhaMayIds);

        #endregion
    }
}
