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
    public  class SMSLogDAL
    {
        public SMSLog Get(int Id)
        {
            try
            {

                return new DBHelper(Configuration.SMSConnectionString).GetInstanceSP<SMSLog>("SP_SMSLog_Get", new SqlParameter("@Id", Id));

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return new SMSLog();
            }
        }
        public int InsertUpdate(SMSLog obj)
        {
            try
            {
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@Name", obj.Name);
                pars[1] = new SqlParameter("@Admin", obj.Admin);
                pars[2] = new SqlParameter("@Mobile", obj.Mobile);
                pars[3] = new SqlParameter("@Message", obj.Message);
                pars[4] = new SqlParameter("@Ip", obj.Ip);
                pars[5] = new SqlParameter("@Status", obj.Status);
                pars[6] = new SqlParameter("@PartnerCode", obj.PartnerCode);
                pars[7] = new SqlParameter("@Id", obj.Id);
                pars[8] = new SqlParameter("@ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("SP_SMSLog_InsertUpdate", pars);
                return Convert.ToInt32(pars[8].Value);

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public int UpdateStatus(int Id)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@Id", Id);
                pars[1] = new SqlParameter("@ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Configuration.SMSConnectionString).ExecuteNonQuerySP("SP_SMSLog_UpdateStatus", pars);
                return Convert.ToInt32(pars[1].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public List<SMSLog> GetList(int Status, string Name, string Admin,string PartnerCode, int CurrPage, int PageSize, ref int TotalRecord)
        {
            string select = "*";
            string where = "1=1";
            if (!string.IsNullOrEmpty(PartnerCode))
            {
                where += " AND [PartnerCode] ='" + PartnerCode + "' ";
            }
            if (!string.IsNullOrEmpty(Admin))
            {
                where += " AND [Admin] ='" + Admin + "' ";
            }
            if (!string.IsNullOrEmpty(Name))
            {
                where += " ( AND [Name] LIKE N'%" + Name + "%' OR [Mobile] LIKE N'%" + Name + "%' ) ";
            }
            if (Status > -999)
            {

                where += " AND [Status] =" + Status;
            }
            string order = "[Id] DESC";
            return SelectDynamicPage(select, where, order, CurrPage, PageSize, ref TotalRecord);
        }
        public List<SMSLog> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.SMSConnectionString).GetListSP<SMSLog>("sp_SMSLog_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<SMSLog>();
            }
        }
    }
}
