using DATA.SMS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UTILS;


namespace DATA.ContentDB
{
    public class TestLocationDAL
    {
        public List<TestLocation> GetList()
        {
            string select = "*";
            return this.SelectDynamic(select, "1=1", "[Name] ASC");
        }

        public List<TestLocation> SelectDynamic(string select, string where, string order)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@SelectQuery", select), new SqlParameter("@WhereCondition", where), new SqlParameter("@OrderByExpression", order) };
                return new DBHelper(Configuration.HomeConnectionString).GetListSP<TestLocation>("sp_TestLocation_SelectDynamic", parameters);
            }
            catch (Exception exception1)
            {
                NLogLogger.PublishException(exception1);
                return new List<TestLocation>();
            }
        }
    }

}
