using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class PlanDBBase: ShopOnlineDBBase
    {
        public static PlanDBBase Create ()
        {
            return ( PlanDBBase ) Activator.CreateInstance ( typeof ( PlanDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePlan ( Plan manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder, string username);
     

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Plan  GetPlan(int Id);
       
        public abstract IEnumerable<Plan> GetPlansDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Plan> GetList(string username);

        #endregion




    }
}
