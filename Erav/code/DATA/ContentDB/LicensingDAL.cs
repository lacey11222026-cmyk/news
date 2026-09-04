using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
namespace DATA.DocumentDB
{
    public class LicensingDAL
    {
       
        public static List<Licensing> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Licensing>("sp_Licensing_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Licensing>();
            }
        }
        public static List<Licensing> GetTopLastestDocuments(int status)
        {
            var select = "Top 2000 *";
            var where = " ";
            //var where = "[ExpiredLicensing]>getdate()";
            //if (status > 0)
            //{

            //    where += " LicensingStatus=" + status;
            //}
            var orderBy = "Id ASC";

            return SelectDynamic(select, where, orderBy);
        }
        

    }
}
