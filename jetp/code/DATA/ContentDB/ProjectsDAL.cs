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
    public class ProjectsDAL
    {
        public static List<Projects> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Projects>("sp_Project_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<Projects>();
            }
        }
        public static List<Projects> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Projects>("sp_Project_SelectDynamic", pars);

                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                return new List<Projects>();
            }
        }
        public static int InsertUpdate(Projects functions)
        {
            try
            {
                var pars = new SqlParameter[20];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Name", functions.Name);
                pars[2] = new SqlParameter("@Image", functions.Image);
                pars[3] = new SqlParameter("@Description", functions.Description);
                pars[4] = new SqlParameter("@Config", functions.Config);
                pars[5] = new SqlParameter("@Contact", functions.Contact);
                pars[6] = new SqlParameter("@ProcessTime", functions.ProcessTime);
                pars[7] = new SqlParameter("@Status", functions.Status);
                pars[8] = new SqlParameter("@StartTime", functions.StartTime);
                pars[9] = new SqlParameter("@SupportGroup", functions.SupportGroup);
                pars[10] = new SqlParameter("@Result", functions.Result);
                pars[11] = new SqlParameter("@Target", functions.Target);
                pars[12] = new SqlParameter("@Region", functions.Region);
                pars[13] = new SqlParameter("@Type", functions.Type);
                pars[14] = new SqlParameter("@SystemType", functions.SystemType);
                pars[15] = new SqlParameter("@ProposedCapital", functions.ProposedCapital);
                pars[16] = new SqlParameter("@Capital", functions.Capital);
                pars[17] = new SqlParameter("@Language", functions.Language);
                pars[18] = new SqlParameter("@MinimumTaget", functions.MinimumTaget);
                pars[19] = new SqlParameter("@Sponsor", functions.Sponsor);

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_Project_InsertUpdate", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int UpdateOrder(int Id, bool upOrder,string lang)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@Id", Id);
                pars[1] = new SqlParameter("@UpOrder", upOrder);
                pars[2] = new SqlParameter("@Language", lang);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Project_UpdateSortOrder", pars);
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

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Project_Delete", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static Projects GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static List<Projects> GetSearch(int status, int type,string lang, string keyword, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "[Order] DESC, Id DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                // where += "( Email LIKE N'%" + keyword + "%' ";
                //where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += "Name LIKE N'%" + keyword + "%' ";
            }
            if (!string.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                   " Language =" + "'" + lang + "'";
            }
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Type =" + type;
            }
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " SystemType =" + status;
            }
            //NLogLogger.DebugMessage(where);
            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public static List<Projects> TopProject(int top,string lang)
        {
            var select = $"top ({top}) *";



            var where = string.Empty;
            var orderBy = "[Order] DESC, Id DESC";

            if (!string.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                   " Language =" + "'" + lang + "'";
            }
            
            return SelectDynamic(select, where, orderBy);
        }

    }
}