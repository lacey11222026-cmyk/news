using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
namespace DATA.ContentDB
{
    public class IdeaDAL
    {
        public static List<Idea> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Idea>("sp_Idea_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<Idea>();
            }
        }
        public static List<Idea> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Idea>("sp_Idea_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Idea>();
            }
        }
        public static int Delete(int Id)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Id", Id);

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Idea_Delete", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int InsertUpdate(Idea functions)
        {
            try
            {
                var pars = new SqlParameter[16];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Name", functions.Name);
                pars[2] = new SqlParameter("@Code", functions.Code);
                pars[3] = new SqlParameter("@No", functions.No);
                pars[4] = new SqlParameter("@PublishDate", functions.PublishDate);
                pars[5] = new SqlParameter("@FilePath", functions.FilePath);
                pars[6] = new SqlParameter("@Proposer", functions.Proposer);
                pars[7] = new SqlParameter("@Status", functions.Status);
                pars[8] = new SqlParameter("@Unit", functions.Unit);
                pars[9] = new SqlParameter("@Mark", functions.Mark);
                pars[10] = new SqlParameter("@Effective", functions.Effective);
                pars[11] = new SqlParameter("@Followers", functions.Followers);
                pars[12] = new SqlParameter("@Progress", functions.Progress);
                pars[13] = new SqlParameter("@ProgressPercent", functions.ProgressPercent);
                pars[14] = new SqlParameter("@Result", functions.Result);
                pars[15] = new SqlParameter("@Region", functions.Region);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_Idea_InsertUpdate", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
       
        public static Idea GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static List<Idea> GetSearch(int status,int progress, int year,string keyword,string unit, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "Id ASC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
               
                //where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += " Name LIKE N'%" + keyword + "%'";

            }
            if (!string.IsNullOrEmpty(unit))
            {
                keyword = Utils.FormatKeywordSearch(unit);

                //where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += " Unit LIKE N'%" + unit + "%'";

            }


            //if (categoryId > 0)
            //{
            //    if (!string.IsNullOrEmpty(where))
            //        where += " AND ";

            //    where += " CategoryPathway Like '%," + categoryId + ",%' ";
            //}
            if (year > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Year(PublishDate) =" + year;
            }
            if (status > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (progress > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Progress =" + progress;
            }
            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }


    }
}
