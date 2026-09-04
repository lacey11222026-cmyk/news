using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class PlanDBSproc : PlanDBBase
    {
        #region Overrides of PlanDBBase

        public override int CreateUpdatePlan(Plan manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string Investor = manufactory.Investor;
                string WorkPlan = manufactory.WorkPlan;
                string Result = manufactory.Result;
                string Code = manufactory.Code;
                string Problem = manufactory.Problem;
                string UserName = manufactory.UserName;
                double Total = manufactory.Total;
                double Total1 = manufactory.Total1;
                double Total2 = manufactory.Total2;
                double Power = manufactory.Power;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Plan_InsertUpdate(_id, _name, Code, Investor, Result, WorkPlan, Problem, UserName, Total,Total1,Total2,Power, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                   
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanDBSproc", "CreateUpdatePlan");
                return -1;
            }
        }
       
        public override Plan GetPlan(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlansDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Plan> GetList(string username)
        {
            var select = " *";
            
            var where = "";
            if(!string.IsNullOrEmpty(username))
            {

                where = " UserName =" + "'" + username + "'";
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetPlansDyn(select, where, orderBy);
        }
      

        public override IEnumerable<Plan> GetPlansDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Plan_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "PlanDBSproc", "GetPlansDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder,string username)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Plan_UpdateSortOrder(Id, upOrder, username);
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
                     datacontext.SP_Plan_UpdateStatus(Id, ref responeCode);
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
