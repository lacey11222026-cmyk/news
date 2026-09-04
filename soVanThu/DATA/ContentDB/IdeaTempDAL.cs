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
    public class IdeaTempDAL
    {
        public static List<IdeaTemp> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<IdeaTemp>("sp_IdeaTemp_SelectDynamic", pars);

                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                return new List<IdeaTemp>();
            }
        }
        public static List<IdeaTemp> GetTop()
        {
            var select = "Top 1000 *";
            var where = "1=1";
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results;
        }
        public static List<IdeaTemp> GetList(string joinId)
        {
            var select = "Top 1000 *";
            var where = "Id IN (" + joinId + ")";
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results;
        }
        public static IdeaTemp GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static int Delete(string Id)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@WhereCondition", Id);

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_IdeaTemp_DeleteDynamic", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int InsertUpdate(IdeaTemp functions)
        {
            try
            {
                var pars = new SqlParameter[17];
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
                pars[12] = new SqlParameter("@Type", functions.Type);
                pars[13] = new SqlParameter("@ProgressPercent", functions.ProgressPercent);
                pars[14] = new SqlParameter("@Result", functions.Result);
                pars[15] = new SqlParameter("@Region", functions.Region);
                pars[16] = new SqlParameter("@Description", functions.Description);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_IdeaTemp_InsertUpdate", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
    }
}
