using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Car.Data.Service
{
    public interface IPaysService
    {
          int CreateUpdatePay(Pay manuFactory);
          int UpdateOrder(int Id, bool upOrder, string username);
          int UpdateStatus(int Id);
          Pay GetPay(int Id);
          List<Pay> GetPaysDyn(string select, string where, string orderBy);
        List<Pay> GetList(string username,int status);
    }
    public class PaysService : MainDataContextBase, IPaysService
    {
        public int CreateUpdatePay(Pay manufactory)
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
                    datacontext.SP_Pay_InsertUpdate(_id, _name, Investor, UserName, Total, Total1, Total2, Total3, Total4, year, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();

                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -1;
            }
        }

        public Pay GetPay(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetPaysDyn(select, where, order).FirstOrDefault();
        }
        public List<Pay> GetList(string username,int status)
        {
            var select = " *";

            var where = "";
            if (!string.IsNullOrEmpty(username))
            {

                where += " UserName =" + "'" + username + "'";
            }
            
            var orderBy = "[Year] ASC, Id DESC";

            return GetPaysDyn(select, where, orderBy);
        }


        public List<Pay> GetPaysDyn(string select, string where, string orderBy)
        {
            try
            {
                using (MainDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Pay_SelectDynamic(select, where, orderBy).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }
        public int UpdateOrder(int Id, bool upOrder, string username)
        {
            try
            {

                using (MainDataContext datacontext = DataContext)
                {
                    datacontext.SP_Pay_UpdateSortOrder(Id, upOrder, username);
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
                    datacontext.SP_Pay_UpdateStatus(Id, ref responeCode);
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
