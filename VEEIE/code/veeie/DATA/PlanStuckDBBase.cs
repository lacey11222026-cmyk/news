using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class PlanStuckDBBase: ShopOnlineDBBase
    {
        public static PlanStuckDBBase Create ()
        {
            return ( PlanStuckDBBase ) Activator.CreateInstance ( typeof ( PlanStuckDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePlanStuck ( PlanStuck manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder,int id);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract PlanStuck  GetPlanStuck(int Id);
       
        public abstract IEnumerable<PlanStuck> GetPlanStucksDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<PlanStuck> GetList(int id);

        #endregion




    }
}
