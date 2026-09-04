using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
using System.CodeDom.Compiler;
namespace DATA.ContentDB
{
    public class Project2DAL
    {
        public static List<Project2> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Project2>("sp_Project2_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<Project2>();
            }
        }
        public static List<Project2> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Project2>("sp_Project2_SelectDynamic", pars);

                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                return new List<Project2>();
            }
        }
        public static int UpdateView(int Id)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@Id", Id);

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Project2UpdateView", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int InsertUpdate(Project2 functions)
        {
            try
            {
                var pars = new SqlParameter[25];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Name", functions.Name);
                pars[2] = new SqlParameter("@Location", functions.Location);
                pars[3] = new SqlParameter("@Description", functions.Description);
                pars[4] = new SqlParameter("@Config", functions.Config);
                pars[5] = new SqlParameter("@Unit", functions.Unit);
                pars[6] = new SqlParameter("@UnitIInfo", functions.UnitIInfo);
                pars[7] = new SqlParameter("@Status", functions.Status);
                pars[8] = new SqlParameter("@Organ", functions.Organ);
                pars[9] = new SqlParameter("@Total", functions.Total);
                pars[10] = new SqlParameter("@Currency", functions.Currency);
                pars[11] = new SqlParameter("@Detail", functions.Detail);
                pars[12] = new SqlParameter("@Source", functions.Source);
                pars[13] = new SqlParameter("@Type", functions.Type);
                pars[14] = new SqlParameter("@SubType", functions.SubType);
                pars[15] = new SqlParameter("@Progress", functions.Progress);
                pars[16] = new SqlParameter("@LegalStatus", functions.LegalStatus);
                pars[17] = new SqlParameter("@Impact", functions.Impact);
                pars[18] = new SqlParameter("@Document", functions.Document);
                pars[19] = new SqlParameter("@Rule1", functions.Rule1);
                pars[20] = new SqlParameter("@Rule2", functions.Rule2);
                pars[21] = new SqlParameter("@Rule3", functions.Rule3);
                pars[22] = new SqlParameter("@Rule4", functions.Rule4);
                pars[23] = new SqlParameter("@Username", functions.Username);
                pars[24] = new SqlParameter("@ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
              
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_Project2_InsertUpdate", pars);
                return Convert.ToInt32(pars[24].Value);
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

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_Project2_Delete", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static Project2 GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static List<Project2> GetSearch(int status, int type,string lang, string keyword, int pageIndex, int pageSize, ref int totalRecords,string fromdate= "", string todate = "",int  subtype=0)
        {
            var select = "[Id] ,[Name] ,[Location] ,[Type] ,[SubType] ,[Unit] ,[UnitIInfo] ,[Organ] ,[Total] ,[Currency] ,[CreateTime] ,[SendTime] ,[Detail] ,[Source] ,[Progress] ,[LegalStatus] ,[Impact] ,[Document] ,[Username] ,[Status] ,[ViewCount]";



            var where = string.Empty;
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                // where += "( Email LIKE N'%" + keyword + "%' ";
                //where += "OR SignedBy LIKE N'%" + keyword + "%' ";
                //where += "OR SignedByDesc LIKE N'%" + keyword + "%' ";
                where += "Name LIKE N'%" + keyword + "%' ";
            }
            //if (!string.IsNullOrEmpty(username))
            //{
            //    if (!string.IsNullOrEmpty(where))
            //        where += " AND ";
            //    where +=
            //       " Username =" + "'" + username + "'";
            //}
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Type =" + type;
            }
            if (subtype > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [SubType] =" + subtype;
            }
            if (status >-1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }

            if (!string.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                   " UserName =" + "'" + lang + "'";
            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);


                where +=
                    "and (convert(nvarchar(23),CreateTime,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            //NLogLogger.DebugMessage(where);
            return SelectDynamicPage(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        
        public static List<Project2> TopProject(int top,int status,string keyword,string lang="")
        {
            var select = $"top ({top}) [Id] ,[Name] ,[Location] ,[Type] ,[SubType] ,[Unit] ,[UnitIInfo] ,[Organ] ,[Total] ,[Currency] ,[CreateTime] ,[SendTime] ,[Detail] ,[Source] ,[Progress] ,[LegalStatus] ,[Impact] ,[Document] ,[Username] ,[Status] ,[ViewCount]";



            var where = string.Empty;
            var orderBy = "[Id] DESC";
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
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
                   " UserName =" + "'" + lang + "'";
            }
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            return SelectDynamic(select, where, orderBy);
        }

    }
}