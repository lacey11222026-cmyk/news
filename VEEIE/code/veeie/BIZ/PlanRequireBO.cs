using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using PlanRequire = DATA.PlanRequire;
namespace BIZ
{
    public class PlanRequireBO
    {
       

        #region CREATE
        public int CreateUpdatePlanRequire(PlanRequire PlanRequire)
        {
            
            int returnVal = PlanRequireDBBase.Create().CreateUpdatePlanRequire(PlanRequire);
          
            return returnVal;
        }
        public int UpdateStatus(int PlanRequireId)
        {
            try
            {
                return PlanRequireDBBase.Create().UpdateStatus(PlanRequireId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanRequireBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int PlanRequireId, bool upOrder,int id)
        {
            try
            {
                return PlanRequireDBBase.Create().UpdateOrder(PlanRequireId, upOrder, id);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanRequireBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public PlanRequire GetPlanRequire(int PlanRequireId)
        {
            try
            {
                return PlanRequireDBBase.Create().GetPlanRequire(PlanRequireId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "PlanRequireBO", "GetPlanRequire");
                return null;
            }
        }


        public List<PlanRequire> GetList(int id)
        {
            var data = PlanRequireDBBase.Create().GetList(id);
            if (data == null)
                return null;

            return data.ToList();
        }

      

        #endregion




      
    }
}
