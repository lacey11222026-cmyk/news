using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class PartDBBase : ShopOnlineDBBase
    {
        public static PartDBBase Create()
        {
            return (PartDBBase)Activator.CreateInstance(typeof(PartDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePart(Part Part);

        #endregion

        #region READ STATEMENTs

        public abstract Part GetPart(int PartId);
        public abstract IEnumerable<Part> GetPartsDyn(string select, string where, string orderBy);

        public abstract IEnumerable<Part> GetAllPartsPaged(int status, string code, int pageIndex, int pageSize,
            ref int totalRecords);
        public abstract IEnumerable<Part> GetAllPartsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Part> GetAllParts( int status,string code);
       


        #endregion

        #region DELETE STATEMENTs

      
        public abstract int DeletePart(int PartId);
       

        #endregion

    }
}
