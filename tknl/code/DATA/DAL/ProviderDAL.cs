using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
namespace DATA.DAL
{
    public class ProviderDAL
    {
        public static List<Provider> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
        {
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);
                pars[3] = new SqlParameter("@PageIndex", CurrPage);
                pars[4] = new SqlParameter("@PageSize", PageSize);
                pars[5] = new SqlParameter("@TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Configuration.SMSConnectionString).GetListSP<Provider>("sp_Provider_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<Provider>();
            }
        }
        public static List<Provider> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.SMSConnectionString).GetListSP<Provider>("sp_Provider_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Provider>();
            }
        }
        public static int InsertUpdate(Provider functions)
        {
            try
            {
                var pars = new SqlParameter[13];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Images", functions.Images);
                pars[2] = new SqlParameter("@Name", functions.Name);
                pars[3] = new SqlParameter("@Type", functions.Type);
                pars[4] = new SqlParameter("@Address", functions.Address);
                pars[5] = new SqlParameter("@MST", functions.MST);
                pars[6] = new SqlParameter("@Year", functions.Year);
                pars[7] = new SqlParameter("@Cate", functions.Cate);
                pars[8] = new SqlParameter("@Business", functions.Business);
                pars[9] = new SqlParameter("@Office", functions.Office);
                pars[10] = new SqlParameter("@Contact", functions.Contact);

                pars[11] = new SqlParameter("@Represent", functions.Represent);
                pars[12] = new SqlParameter("@Province", functions.Province);
                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("sp_Provider_InsertUpdate", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int UpdateStatus(int Id)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@Id", Id);
            
                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("SP_ProviderUpdateStatus", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int UpdateSortOrder(int Id, bool upOrder)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@Id", Id);
                pars[1] = new SqlParameter("@UpOrder", upOrder);
                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("SP_Provider_UpdateSortOrder", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int Delete(int Id)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Id", Id);

                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("SP_Provider_Delete", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static Provider GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static List<Provider> GetSearch(int status,string keyword, int pageIndex, int pageSize, ref int totalRecords, string Province, string smt="")
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "[Order] DESC, ID DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Contact LIKE N'%" + keyword + "%' ";
                //where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += "OR Name LIKE N'%" + keyword + "%' )";

            }
           
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (!string.IsNullOrEmpty(Province))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Province =N'" + Province + "'";

            }
            if (!string.IsNullOrEmpty(smt))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " MST =N'" + smt + "'";

            }
            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

    }
}
