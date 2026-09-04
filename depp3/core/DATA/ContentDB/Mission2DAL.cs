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
    public class Mission2DAL
    {
        public static List<Mission2> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Mission2>("sp_Mission2_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<Mission2>();
            }
        }
        public static List<Mission2> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Mission2>("sp_Mission2_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Mission2>();
            }
        }
        public static int InsertUpdate(Mission2 functions)
        {
            try
            {
                var pars = new SqlParameter[11];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Name", functions.Name);
                pars[2] = new SqlParameter("@CategoryId", functions.CategoryId);
                pars[3] = new SqlParameter("@Description", functions.Description);
                pars[4] = new SqlParameter("@CreatedBy", functions.CreatedBy);
                pars[5] = new SqlParameter("@PublishDate", functions.PublishDate);
                pars[6] = new SqlParameter("@Result", functions.Result);
                pars[7] = new SqlParameter("@Status", functions.Status);

                pars[8] = new SqlParameter("@Accept", functions.Accept);
                pars[9] = new SqlParameter("@Code", functions.Code);
                pars[10] = new SqlParameter("@Organ", functions.Organ);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_Mission2_InsertUpdate", pars);
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
            
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Mission2UpdateStatus", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static Mission2 GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static List<Mission2> GetTop()
        {
            var select = "*";
            var where = "Status =1 ";
            var orderBy = "CategoryId ASC";

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results;
        }
        public static List<Mission2> GetSearch(int status,string keyword, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Description LIKE N'%" + keyword + "%' ";
                //where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += "OR Name LIKE N'%" + keyword + "%' )";

            }



            //if (categoryId > 0)
            //{
            //    if (!string.IsNullOrEmpty(where))
            //        where += " AND ";

            //    where += " CategoryPathway Like '%," + categoryId + ",%' ";
            //}
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }

            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

    }
}
