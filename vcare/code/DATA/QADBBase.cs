using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class QADBBase: ShopOnlineDBBase
    {
        public static QADBBase Create ()
        {
            return ( QADBBase ) Activator.CreateInstance ( typeof ( QADBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateQA ( QA manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract QA  GetQA(int Id);
        public abstract IEnumerable<QA> GetAllPaged(int pageIndex, int pageSize, ref int totalRecords, int? published);
        public abstract IEnumerable<QA> GetQAsDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<QA> GetTopLastest(int top);

        #endregion

        #region DELETE STATEMENTs

        

        public abstract int DeleteQA ( int manuFactoryId );

      

        #endregion



    }
}
