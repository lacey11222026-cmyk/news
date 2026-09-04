using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class TestQuestionDBBase: ShopOnlineDBBase
    {
        public static TestQuestionDBBase Create ()
        {
            return ( TestQuestionDBBase ) Activator.CreateInstance ( typeof ( TestQuestionDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateTestQuestion ( TestQuestion manuFactory );

        #endregion

        #region READ STATEMENTs

       
        public abstract IEnumerable<TestQuestion> GetTestQuestionsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<TestQuestion> GetTestQuestion();
        public abstract IEnumerable<TestQuestion> GetByRegistorId(int id);

        public abstract TestQuestion GetById(int Id);
        public abstract IEnumerable<TestQuestion> GetByRegistorId(int id,int status, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<TestQuestion> GetAllTestQuestionsPagedDyn(string select, string where,
            string orderBy, int pageIndex, int pageSize, ref int totalRecords);

        public abstract int DeleteDyn(string where);
        public abstract int Delete(int documentId);

        #endregion




    }
}
