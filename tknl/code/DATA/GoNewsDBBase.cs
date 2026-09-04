using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class GoNewsDBBase: ShopOnlineDBBase
    {
        public static GoNewsDBBase Create ()
        {
            return ( GoNewsDBBase ) Activator.CreateInstance ( typeof ( GoNewsDBSproc ) );
        }

        

        #region READ STATEMENTs

        public abstract GoNew GetGoNews ( int attributeId );
        public abstract IEnumerable<GoNew> GetGoNewssDyn(string select, string where, string orderBy);
        public abstract IEnumerable<GoNew> GetAllGoNewssPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<GoNew> GetAllGoNewssPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        //public abstract IEnumerable<GoNew> GetAllGoNewss(string name, int categoryId, int groupId);
        public abstract IEnumerable<GoNew> GetAllGoNewssByFilter(int categoryId,string lstcate, int pageIndex, int pageSize,
                                                                 ref int totalRecords, string fromdate, string todate);


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteGoNewsDyn ( string where );
        public abstract int DeleteGoNews ( int attributeId );
        public abstract int DeleteGoNewss ( string lstGoNewsIds );

        #endregion
    }
}
