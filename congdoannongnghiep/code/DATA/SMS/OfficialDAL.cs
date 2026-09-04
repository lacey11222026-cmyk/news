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
    public class OfficialDAL
    {
       
        public static List<Official> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Official>("sp_tbvanban_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Official>();
            }
        }
        public static List<Official> GetTopLastestDocuments(int status)
        {
            var select = "Top 5000 *";
            var where = "1=1  ";
            //if (status > 0)
            //{
               
            //    where += " OfficialStatus=" + status;
            //}
            var orderBy = "IdVanBan ASC";

            return SelectDynamic(select, where, orderBy);
        }

        public static List<Official2> SelectDynamic2(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Official2>("sp_tbtintuc_SelectDynamic", pars);

                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                return new List<Official2>();
            }
        }
        public static List<Official2> GetTop(int id)
        {
            var select = "Top 5000 *";
            var where = "1=1  ";
            //if (status > 0)
            //{

            //    where += " OfficialStatus=" + status;
            //}
            var orderBy = "IdTintuc ASC";

            return SelectDynamic2(select, where, orderBy);
        }
    }
}
