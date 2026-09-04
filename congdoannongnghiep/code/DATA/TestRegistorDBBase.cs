using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class TestRegistorDBBase: ShopOnlineDBBase
    {
        public static TestRegistorDBBase Create ()
        {
            return ( TestRegistorDBBase ) Activator.CreateInstance ( typeof ( TestRegistorDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateTestRegistor ( TestRegistor manuFactory );

        #endregion

        #region READ STATEMENTs

        public abstract IEnumerable<TestRegistor> GetAllTestRegistorsPagedDyn(string select, string where,
            string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<TestRegistor> GetTestRegistorsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<TestRegistor> GetTestRegistor();
        public abstract TestRegistor GetById(int Id);
        public abstract IEnumerable<TestRegistor> GetAll(string keyword, int status, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<TestRegistor> GetAll();
        public abstract int DeleteDyn(string where);
        public abstract int Delete(int documentId);
        #endregion




    }
}
