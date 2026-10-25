using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Card.Data;
using Card.Utility;
namespace DATA.ContentDB
{
    public class VanThuDAL
    {
        
        public static int Delete(int id)
        {
            try { new DBHelper(Config.MainConnectionString).ExecuteNonQuerySP("SP_VanThu_Delete", new[] { new SqlParameter("@_Id", id) }); return 1; }
            catch (Exception ex) { NLogLogger.PublishException(ex); return -99; }
        }
        public static List<VanThu> SelectDynamic(string select, string where, string order)
        {
            try { return new DBHelper(Config.MainConnectionString).GetListSP<VanThu>("sp_VanThu_SelectDynamic", new[] { new SqlParameter("@SelectQuery", select), new SqlParameter("@WhereCondition", where), new SqlParameter("@OrderByExpression", order) }); }
            catch (Exception ex) { NLogLogger.PublishException(ex); return new List<VanThu>(); }
        }
        public static List<VanThu> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
        {
            try
            {
                var pars = new[] { new SqlParameter("@SelectQuery", select), new SqlParameter("@WhereCondition", where), new SqlParameter("@OrderByExpression", order), new SqlParameter("@PageIndex", CurrPage), new SqlParameter("@PageSize", PageSize), new SqlParameter("@TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output } };
                var result = new DBHelper(Config.MainConnectionString).GetListSP<VanThu>("sp_VanThu_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value); return result;
            }
            catch (Exception ex) { NLogLogger.PublishException(ex); TotalRecord = 0; return new List<VanThu>(); }
        }
        public static VanThu GetDetail(int id) { return SelectDynamic("*", "Id = " + id, "").FirstOrDefault(); }
    }
}
