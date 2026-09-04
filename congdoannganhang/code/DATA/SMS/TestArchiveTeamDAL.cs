using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DATA.SMS
{
    public  class TestArchiveTeamDAL
    {
        public TestArchiveTeam Get(int Id)
        {
            try
            {
                string select = "*";
                

                string where=  "[Id] =" + Id;
                string order = "[Order] ASC";
                return SelectDynamic(select, where, order).FirstOrDefault();
                

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new TestArchiveTeam();
            }
        }
        public int InsertUpdate(TestArchiveTeam obj)
        {
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@Name", obj.Name);
                pars[1] = new SqlParameter("@RegistorId", obj.RegistorId);
                pars[2] = new SqlParameter("@Archive", obj.Archive);
                pars[3] = new SqlParameter("@Questions", obj.Questions);
                pars[4] = new SqlParameter("@Mark", obj.Mark);
                pars[5] = new SqlParameter("@Id", obj.Id);
                pars[6] = new SqlParameter("@ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("SP_TestArchiveTeam_InsertUpdate", pars);
                return Convert.ToInt32(pars[6].Value);

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
      
        public List<TestArchiveTeam> GetList(int RegistorId)
        {
            string select = "*";
            string where = "1=1";
           
            if (RegistorId > 0)
            {

                where += " AND [RegistorId] =" + RegistorId;
            }
            string order = "[Order] ASC";
            return SelectDynamic(select, where, order);
        }
        public List<TestArchiveTeam> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);
               
                var list = new DBHelper(Configuration.SMSConnectionString).GetListSP<TestArchiveTeam>("sp_TestArchiveTeam_SelectDynamic", pars);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<TestArchiveTeam>();
            }
        }
    }
}
