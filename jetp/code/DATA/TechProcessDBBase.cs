using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class TechProcessDBBase : ShopOnlineDBBase
    {
        public static TechProcessDBBase Create()
        {
            return (TechProcessDBBase)Activator.CreateInstance(typeof(TechProcessDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateTechProcess(TechProcess TechProcess);

        #endregion

        #region READ STATEMENTs

        public abstract TechProcess GetTechProcess(int TechProcessId);

        public abstract IEnumerable<TechProcess> GetAllTechProcesssPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);

        public abstract IEnumerable<TechProcess> GetTechProcesssByFilter(string keyword, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<TechProcess> GetTechProcesssDyn(string select, string where, string orderBy);
        public abstract IEnumerable<TechProcess> GetTopLastestTechProcesss(int top);


        #endregion

        #region DELETE STATEMENTs
        public abstract int UpdateOrder(int Id, bool upOrder);
        public abstract int DeleteTechProcessDyn(string where);
        public abstract int DeleteTechProcess(int TechProcessId);
        public abstract int DeleteTechProcesss(string lstTechProcessIds);

        #endregion
    }
}
