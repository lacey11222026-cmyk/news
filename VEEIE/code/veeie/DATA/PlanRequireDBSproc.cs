using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class PlanRequireDBSproc : PlanRequireDBBase
    {
        #region Overrides of PlanRequireDBBase

        public override int CreateUpdatePlanRequire(PlanRequire manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
               
                string Description = manufactory.Description;
              
                int? PlanId = manufactory.PlanId;
                string FinishDate = manufactory.FinishDate;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanRequire_InsertUpdate(_id, Description, FinishDate,PlanId, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                   
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanRequireDBSproc", "CreateUpdatePlanRequire");
                return -1;
            }
        }
       
        public override PlanRequire GetPlanRequire(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlanRequiresDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<PlanRequire> GetList(int  Id)
        {
            var select = " *";
            
            var where = "";
            if(Id>0)
            {

                where = " PlanId =" + Id;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlanRequiresDyn(select, where, orderBy);
        }
      

        public override IEnumerable<PlanRequire> GetPlanRequiresDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_PlanRequire_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanRequireDBSproc", "GetPlanRequiresDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder,int id)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_PlanRequire_UpdateSortOrder(Id, id,upOrder );
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
                     datacontext.SP_PlanRequire_UpdateStatus(Id, ref responeCode);
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
