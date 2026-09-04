using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Car.Data.Service
{
    public interface IFinancesService
    {
          int CreateUpdateFinance(Finance manuFactory);
          int UpdateOrder(int Id, bool upOrder, string username,int year);
          int UpdateStatus(int Id);
          Finance GetFinance(int Id);
         
        List<Finance> GetList(string username,int status, int year);
    }
    public class FinancesService : MainDataContextBase, IFinancesService
    {
        public int CreateUpdateFinance(Finance manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;

                string _name = manufactory.Name;
                string Investor = manufactory.Investor;
                double? Total = manufactory.Total;
                double? Total1 = manufactory.Total1;
                double? Total2 = manufactory.Total2;
                double? Total3 = manufactory.Total3;
                string UserName = manufactory.UserName;
                double? Total4 = manufactory.Total4;
                int? year = manufactory.Year;
               
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Finance_InsertUpdate(_id, _name, Investor, UserName, Total, Total1, Total2, Total3, Total4, year, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();

                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

        public Finance GetFinance(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetFinancesDyn(select, where, order).FirstOrDefault();
        }
        public List<Finance> GetList(string username,int status,int year)
        {
            var select = " *";

            var where = "";
            if (!string.IsNullOrEmpty(username))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " UserName =" + "'" + username + "'";
            }
            if (year > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Year = " + year;
            }
            var orderBy = "[Order] ASC, Id DESC";

            return GetFinancesDyn(select, where, orderBy);
        }


        public List<Finance> GetFinancesDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Finance_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }
        public int UpdateOrder(int Id, bool upOrder, string username,int year)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Finance_UpdateSortOrder(Id, upOrder, year, username);
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
                    datacontext.SP_Finance_UpdateStatus(Id, ref responeCode);
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
