using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ExpertDBBase: ShopOnlineDBBase
    {
        public static ExpertDBBase Create ()
        {
            return ( ExpertDBBase ) Activator.CreateInstance ( typeof ( ExpertDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateExpert ( Expert manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Expert  GetExpert(int Id);
        public abstract IEnumerable<Expert> GetAllPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published, int type, string lang);
        public abstract IEnumerable<Expert> GetExpertsDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Expert> GetTopLastest(int top,int type);

        #endregion

        #region DELETE STATEMENTs

        

        public abstract int DeleteExpert ( int manuFactoryId );

      

        #endregion



    }
}
