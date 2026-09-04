using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Car.Data.Service
{
    public interface IPlansService
    {
          int CreateUpdatePlan(Plan manuFactory);
          int UpdateOrder(int Id, bool upOrder, string username);
          int UpdateStatus(int Id);
          Plan GetPlan(int Id);
          List<Plan> GetPlansDyn(string select, string where, string orderBy);
        List<Plan> GetList(string username, int year, int type,int status);
    }
    public class PlansService : MainDataContextBase, IPlansService
    {
        public int CreateUpdatePlan(Plan manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;

                string _name = manufactory.Name;
                string Investor = manufactory.Investor + "";
                string WorkPlan = manufactory.WorkPlan + "";
                string Result = manufactory.Result+"";
                string Result2 = manufactory.Result2 + "";
                string Result3 = manufactory.Result3 + "";
                string Result4 = manufactory.Result4 + "";
                string Code = manufactory.Code;
                string Problem = manufactory.Problem + "";
                string UserName = manufactory.UserName;
                double Total = manufactory.Total;
                double Total1 = manufactory.Total1;
                int? year = manufactory.Year;
                int? type = manufactory.Type;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Plan_InsertUpdate(_id, _name, Code, Investor, Result, Result2, Result3, Result4, WorkPlan, Problem, UserName, Total, Total1, year, type, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();

                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

        public Plan GetPlan(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPlansDyn(select, where, order).FirstOrDefault();
        }
        public List<Plan> GetList(string username, int year, int type,int status)
        {
            var select = " *";

            var where = "";
            if (!string.IsNullOrEmpty(username))
            {

                where += " UserName =" + "'" + username + "'";
            }
            if (year > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Year = " + year;
            }
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Status = " + status;
            }
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                if (type == 1)
                    where += " Type<=4 ";
                if (type == 2)
                    where += " Type>4 ";
            }
            var orderBy = "[Year] DESC , [Type] DESC, Id DESC";

            return GetPlansDyn(select, where, orderBy);
        }


        public List<Plan> GetPlansDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Plan_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new List<Plan>();
            }
        }
        public int UpdateOrder(int Id, bool upOrder, string username)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Plan_UpdateSortOrder(Id, upOrder, username);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }


        public int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Plan_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

    }
}
