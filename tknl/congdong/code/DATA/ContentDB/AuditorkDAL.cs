using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
namespace DATA.ContentDB
{
    public class AuditorDAL
    {
        public static List<Auditor> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Auditor>("sp_Auditor_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<Auditor>();
            }
        }
        public static List<Auditor> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Auditor>("sp_Auditor_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Auditor>();
            }
        }
        public static int InsertUpdate(Auditor functions)
        {
            try
            {
                var pars = new SqlParameter[22];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Email", functions.Email);
                pars[2] = new SqlParameter("@Title", functions.Title);
                pars[3] = new SqlParameter("@Type", functions.Type);
                pars[4] = new SqlParameter("@No", functions.No);
                pars[5] = new SqlParameter("@FullName", functions.FullName);
                pars[6] = new SqlParameter("@BirthDay", functions.BirthDay);
                pars[7] = new SqlParameter("@Status", functions.Status);
                pars[8] = new SqlParameter("@Passport", functions.Passport);
                pars[9] = new SqlParameter("@Nation", functions.Nation);
                pars[10] = new SqlParameter("@Order", functions.Order);
                pars[11] = new SqlParameter("@Level", functions.Level);
                pars[12] = new SqlParameter("@Organ", functions.Organ);
                pars[13] = new SqlParameter("@MSDN", functions.MSDN);
                pars[14] = new SqlParameter("@Role", functions.Role);
                pars[15] = new SqlParameter("@Config", functions.Config);
                pars[16] = new SqlParameter("@Address", functions.Address);
                pars[17] = new SqlParameter("@Group", functions.Group);
                pars[18] = new SqlParameter("@Mobile", functions.Mobile);
                pars[19] = new SqlParameter("@Cate", functions.Cate);
                pars[20] = new SqlParameter("@Images", functions.Images);
                pars[21] = new SqlParameter("@Province", functions.Province);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_Auditor_InsertUpdate", pars);
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
            
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_AuditorUpdateStatus", pars);
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
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Auditor_UpdateSortOrder", pars);
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

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Auditor_Delete", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static Auditor GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static List<Auditor> GetSearch(int status,string keyword, int pageIndex, int pageSize, ref int totalRecords,int cate,string Province)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "[Order] DESC, ID DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Address LIKE N'%" + keyword + "%' ";
            
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += "OR [FullName] LIKE N'%" + keyword + "%' )";

            }
           
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (cate >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Cate =" + cate;
            }
            if (!string.IsNullOrEmpty(Province))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Province =N'" + Province +"'";

            }
           // NLogLogger.DebugMessage(where);
            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

    }
}
