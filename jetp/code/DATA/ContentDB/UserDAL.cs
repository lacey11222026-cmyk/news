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
    public class UserDAL
    {
        public static List<User> SelectDynamicPage(string select, string where, string order, int CurrPage, int PageSize, ref int TotalRecord)
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
                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<User>("sp_User_SelectPagedDynamic", pars);
                TotalRecord = Convert.ToInt32(pars[5].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                TotalRecord = 0;
                return new List<User>();
            }
        }
        public static List<User> SelectDynamic(string select, string where, string order)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@SelectQuery", select);
                pars[1] = new SqlParameter("@WhereCondition", where);
                pars[2] = new SqlParameter("@OrderByExpression", order);

                var list = new DBHelper(Configuration.HomeConnectionString).GetListSP<User>("sp_User_SelectDynamic", pars);
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<User>();
            }
        }
        public static int Reg(User functions)
        {
            try
            {
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@Id", functions.Id);
                pars[1] = new SqlParameter("@UserName", functions.UserName);
                pars[2] = new SqlParameter("@Email", functions.Email);
                pars[3] = new SqlParameter("@Mobile", functions.Mobile);
                pars[4] = new SqlParameter("@Password", functions.Password);
                pars[5] = new SqlParameter("@Organ", functions.Organ);
                pars[6] = new SqlParameter("@FistName", functions.FistName);
                pars[7] = new SqlParameter("@LastName", functions.LastName);
                pars[8] = new SqlParameter("@ResponseCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("sp_User_Reg", pars);
                return Convert.ToInt32(pars[8].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int Authentication(string username, string password)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_Username", username);
                pars[1] = new SqlParameter("@_Password", password);
                pars[2] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_User_Login", pars);

                return Convert.ToInt32(pars[2].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int UpdateUserDynamic(string where, string updatest)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@UpdateCondition", updatest);
                pars[1] = new SqlParameter("@WhereCondition", where);
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_User_UpdateDynamic", pars);
                return 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return -99;
            }
        }
        public static int ChangePassword(string UserName, string PasswordOld, string PasswordNew)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_UserName", UserName);
                pars[1] = new SqlParameter("@_PasswordOld", PasswordOld);
                pars[2] = new SqlParameter("@_PasswordNew", PasswordNew);
                pars[3] = new SqlParameter("@_ResponseCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Configuration.HomeConnectionString).ExecuteNonQuerySP("SP_User_ChangePassword", pars);
                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return -99;
            }
        }
        public static User GetDetail(int DocumentId)
        {
            var select = "*";
            var where = "Id = " + DocumentId;
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public static User GetByUserName(string DocumentId)
        {
            var select = "*";
            var where = " UserName =" + "'" + DocumentId + "'";
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public static User GetByEmail(string DocumentId)
        {
            var select = "*";
            var where = " [Email] =" + "'" + DocumentId + "'";
            var orderBy = string.Empty;

            var results = SelectDynamic(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
    }
}
