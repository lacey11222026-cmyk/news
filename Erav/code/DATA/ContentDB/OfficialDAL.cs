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

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<Official>("sp_tblOfficial_SelectDynamic", pars);
               
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
            var where = "[CateOfficialID]>=0 and Status=1 and isDelete=0  ";
            //if (status > 0)
            //{
               
            //    where += " OfficialStatus=" + status;
            //}
            var orderBy = "ID ASC";

            return SelectDynamic(select, where, orderBy);
        }

        public static List<OfficialFile> SelectDynamicFile(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<OfficialFile>("sp_tblOfficialFile_SelectDynamic", pars);

                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                return new List<OfficialFile>();
            }
        }
        public static List<OfficialFile> GetFile(int Id)
        {
            var select = "Top 10000 *";
            var where = "";
            if (Id > 0)
            {

                where += " OfficialID=" + Id;
            }
            var orderBy = "OfficialFileID ASC";

            return SelectDynamicFile(select, where, orderBy);
        }
    }
}
