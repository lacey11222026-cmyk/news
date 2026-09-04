using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class PlanStuckDBSproc : PlanStuckDBBase
    {
        #region Overrides of PlanStuckDBBase

        public override int CreateUpdatePlanStuck(PlanStuck manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string Description = manufactory.Description;
                string Organ = manufactory.Organ;
                string Result1 = manufactory.Result1;
                string Result2 = manufactory.Result2;
                int? PlanId = manufactory.PlanId;
                string FinishDate = manufactory.FinishDate;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanStuck_InsertUpdate(_id, _name, Description, Organ, FinishDate, Result1, Result2, PlanId, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                   
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanStuckDBSproc", "CreateUpdatePlanStuck");
                return -1;
            }
        }
       
        public override PlanStuck GetPlanStuck(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlanStucksDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<PlanStuck> GetList(int  Id)
        {
            var select = " *";
            
            var where = "";
            if(Id>0)
            {

                where = " PlanId =" + Id;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlanStucksDyn(select, where, orderBy);
        }
      

        public override IEnumerable<PlanStuck> GetPlanStucksDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_PlanStuck_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanStuckDBSproc", "GetPlanStucksDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder,int id)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanStuck_UpdateSortOrder(Id, id,upOrder );
                    return 1;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
       
       
        public override int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                     datacontext.SP_PlanStuck_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      

       

   

        #endregion
    }
}
