using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using PlanStuck = DATA.PlanStuck;
namespace BIZ
{
    public class PlanStuckBO
    {
       

        #region CREATE
        public int CreateUpdatePlanStuck(PlanStuck PlanStuck)
        {
            
            int returnVal = PlanStuckDBBase.Create().CreateUpdatePlanStuck(PlanStuck);
          
            return returnVal;
        }
        public int UpdateStatus(int PlanStuckId)
        {
            try
            {
                return PlanStuckDBBase.Create().UpdateStatus(PlanStuckId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanStuckBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int PlanStuckId, bool upOrder,int id)
        {
            try
            {
                return PlanStuckDBBase.Create().UpdateOrder(PlanStuckId, upOrder, id);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanStuckBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public PlanStuck GetPlanStuck(int PlanStuckId)
        {
            try
            {
                return PlanStuckDBBase.Create().GetPlanStuck(PlanStuckId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanStuckBO", "GetPlanStuck");
                return null;
            }
        }


        public List<PlanStuck> GetList(int id)
        {
            var data = PlanStuckDBBase.Create().GetList(id);
            if (data == null)
                return null;

            return data.ToList();
        }

      

        #endregion




      
    }
}
