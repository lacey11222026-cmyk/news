using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class OrganDBBase: ShopOnlineDBBase
    {
        public static OrganDBBase Create ()
        {
            return ( OrganDBBase ) Activator.CreateInstance ( typeof ( OrganDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateOrgan ( Organ manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Organ  GetOrgan(int Id);
        public abstract IEnumerable<Organ> GetAllPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published, int type, string lang);
        public abstract IEnumerable<Organ> GetOrgansDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Organ> GetTopLastest(int top,int type);

        #endregion

        #region DELETE STATEMENTs

        

        public abstract int DeleteOrgan ( int manuFactoryId );

      

        #endregion



    }
}
