using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using PlanItem = DATA.PlanItem;
namespace BIZ
{
    public class PlanItemBO
    {
       

        #region CREATE
        public int CreateUpdatePlanItem(PlanItem PlanItem)
        {
            
            int returnVal = PlanItemDBBase.Create().CreateUpdatePlanItem(PlanItem);
          
            return returnVal;
        }
        public int UpdateStatus(int PlanItemId)
        {
            try
            {
                return PlanItemDBBase.Create().UpdateStatus(PlanItemId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanItemBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int PlanItemId, bool upOrder,int id)
        {
            try
            {
                return PlanItemDBBase.Create().UpdateOrder(PlanItemId, upOrder, id);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanItemBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public PlanItem GetPlanItem(int PlanItemId)
        {
            try
            {
                return PlanItemDBBase.Create().GetPlanItem(PlanItemId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanItemBO", "GetPlanItem");
                return null;
            }
        }


        public List<PlanItem> GetList(int id)
        {
            var data = PlanItemDBBase.Create().GetList(id);
            if (data == null)
                return null;

            return data.ToList();
        }

      

        #endregion




      
    }
}
