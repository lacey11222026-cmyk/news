using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class DocumentDBBase: ShopOnlineDBBase
    {
        public static DocumentDBBase Create ()
        {
            return ( DocumentDBBase ) Activator.CreateInstance ( typeof ( DocumentDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateDocument ( Document Document );

        #endregion

        #region READ STATEMENTs

        public abstract Document GetDocument ( int DocumentId );
      
        public abstract IEnumerable<Document> GetAllDocumentsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );

        public abstract IEnumerable<Document> GetDocumentsByFilter(string keyword, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Document> GetDocumentsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Document> GetTopLastestDocuments(int top, int categoryId);

        public abstract IEnumerable<Document> GetDocumentsSearch2(string keyword, string code, int agent, int area, int type, int categoryId, int status,
                                                                 int pageIndex, int pageSize, string fromdate,
                                                                 string todate, ref int totalRecords);
        public abstract IEnumerable<Document> GetDocumentsSearch(string keyword,int categoryId, int status,
                                                                int pageIndex, int pageSize, string fromdate,
                                                                string todate, ref int totalRecords);
        #endregion

        #region DELETE STATEMENTs
        public abstract int ViewAdd(long Id);
        public abstract int DeleteDocumentDyn ( string where );
        public abstract int DeleteDocument ( int DocumentId );
        public abstract int DeleteDocuments ( string lstDocumentIds );

        #endregion
    }
}
