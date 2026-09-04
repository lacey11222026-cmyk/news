using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class PlanItemDBBase: ShopOnlineDBBase
    {
        public static PlanItemDBBase Create ()
        {
            return ( PlanItemDBBase ) Activator.CreateInstance ( typeof ( PlanItemDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePlanItem ( PlanItem manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder,int id);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract PlanItem  GetPlanItem(int Id);
       
        public abstract IEnumerable<PlanItem> GetPlanItemsDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<PlanItem> GetList(int id);

        #endregion




    }
}
