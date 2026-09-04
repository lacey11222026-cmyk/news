using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class PlanRequireDBBase: ShopOnlineDBBase
    {
        public static PlanRequireDBBase Create ()
        {
            return ( PlanRequireDBBase ) Activator.CreateInstance ( typeof ( PlanRequireDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePlanRequire ( PlanRequire manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder,int id);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract PlanRequire  GetPlanRequire(int Id);
       
        public abstract IEnumerable<PlanRequire> GetPlanRequiresDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<PlanRequire> GetList(int id);

        #endregion




    }
}
