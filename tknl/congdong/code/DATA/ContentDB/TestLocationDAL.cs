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
        public static int InsertUpdate(TestLocation functions)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@Number1", functions.Number1);
                pars[2] = new SqlParameter("@Number2", functions.Number2);
                pars[3] = new SqlParameter("@Number3", functions.Number3);

                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_TestLocation_Update", pars);
                return 1;
                
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
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
