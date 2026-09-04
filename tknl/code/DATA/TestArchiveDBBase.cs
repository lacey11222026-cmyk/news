using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class TestArchiveDBBase : ShopOnlineDBBase
    {
        public static TestArchiveDBBase Create()
        {
            return (TestArchiveDBBase)Activator.CreateInstance(typeof(TestArchiveDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateTestArchive(TestArchive manuFactory);

        #endregion

        #region READ STATEMENTs

        public abstract IEnumerable<TestArchive> GetAllTestArchivesPagedDyn(string select, string where, string orderBy,
            int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<TestArchive> GetTestArchivesDyn(string select, string where, string orderBy);
        public abstract List<TestArchive> GetByMobile(int id, string mobile);

        public abstract TestArchive GetById(int id);
        public abstract List<TestArchiveReport> Report(int id);

        public abstract IEnumerable<TestArchive> GetByRegistorId(int id, string mobile, int pageIndex, int pageSize,
            ref int totalRecords, int OrderType,int status);
        public abstract int DeleteDyn(string where);
        public abstract int Delete(int documentId);

        public abstract IEnumerable<TestArchiveTop> SelectTop();
        #endregion




    }
}
