using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class PlanItemDBSproc : PlanItemDBBase
    {
        #region Overrides of PlanItemDBBase

        public override int CreateUpdatePlanItem(PlanItem manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string _Config1 = manufactory.Config1;
                string Config2 = manufactory.Config2;
                string Config3 = manufactory.Config3;
               
                int? PlanId = manufactory.PlanId;
               
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanItem_InsertUpdate(_id, _name, _Config1, Config2, Config3, PlanId, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                   
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanItemDBSproc", "CreateUpdatePlanItem");
                return -1;
            }
        }
       
        public override PlanItem GetPlanItem(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlanItemsDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<PlanItem> GetList(int  Id)
        {
            var select = " *";
            
            var where = "";
            if(Id>0)
            {

                where = " PlanId =" + Id;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlanItemsDyn(select, where, orderBy);
        }
      

        public override IEnumerable<PlanItem> GetPlanItemsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_PlanItem_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanItemDBSproc", "GetPlanItemsDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder,int id)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanItem_UpdateSortOrder(Id, id,upOrder );
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
                     datacontext.SP_PlanItem_UpdateStatus(Id, ref responeCode);
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
