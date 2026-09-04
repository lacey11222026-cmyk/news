using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Plan = DATA.Plan;
namespace BIZ
{
    public class PlanBO
    {
       

        #region CREATE
        public int CreateUpdatePlan(Plan Plan)
        {
            
            int returnVal = PlanDBBase.Create().CreateUpdatePlan(Plan);
          
            return returnVal;
        }
        public int UpdateStatus(int PlanId)
        {
            try
            {
                return PlanDBBase.Create().UpdateStatus(PlanId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int PlanId, bool upOrder,string username)
        {
            try
            {
                return PlanDBBase.Create().UpdateOrder(PlanId, upOrder, username);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public Plan GetPlan(int PlanId)
        {
            try
            {
                return PlanDBBase.Create().GetPlan(PlanId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanBO", "GetPlan");
                return null;
            }
        }


        public List<Plan> GetList(string username)
        {
            var data = PlanDBBase.Create().GetList(username);
            if (data == null)
                return null;

            return data.ToList();
        }

      

        #endregion




      
    }
}
