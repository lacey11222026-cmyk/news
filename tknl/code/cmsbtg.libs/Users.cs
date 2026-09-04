using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cms.libs
{
    public class Users
    {
        private DBHelper db = null;

        public Users()
        {
            db = new DBHelper(Config.SQLConn);
        }

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Telephone
        {
            get;
            set;
        }

        public int Gender
        {
            get;
            set;
        }

        public string Comments
        {
            get;
            set;
        }

        public DateTime DateBirth
        {
            get;
            set;
        }

        public DateTime LastLogon
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public string Passwordmatrix
        {
            get;
            set;
        }

        public int Insert(string Name, string Password, string FullName, string Address, string Email, string Telephone, int Gender, string Comments, DateTime DateBirth, int Status)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Name", Name));
            oCommand.Parameters.Add(new SqlParameter("@Password", Password));
            oCommand.Parameters.Add(new SqlParameter("@FullName", FullName));
            oCommand.Parameters.Add(new SqlParameter("@Address", Address));
            oCommand.Parameters.Add(new SqlParameter("@Email", Email));
            oCommand.Parameters.Add(new SqlParameter("@Telephone", Telephone));
            oCommand.Parameters.Add(new SqlParameter("@Gender", Gender));
            oCommand.Parameters.Add(new SqlParameter("@Comments", Comments));
            oCommand.Parameters.Add(new SqlParameter("@DateBirth", DateBirth));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            SqlParameter output = new SqlParameter("@ID", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }

        public void Update(int ID, string Name, string FullName, string Address, string Email, string Telephone, int Gender, string Comments, DateTime DateBirth, int Status)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_Update");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            oCommand.Parameters.Add(new SqlParameter("@Name", Name));
            oCommand.Parameters.Add(new SqlParameter("@FullName", FullName));
            oCommand.Parameters.Add(new SqlParameter("@Address", Address));
            oCommand.Parameters.Add(new SqlParameter("@Email", Email));
            oCommand.Parameters.Add(new SqlParameter("@Telephone", Telephone));
            oCommand.Parameters.Add(new SqlParameter("@Gender", Gender));
            oCommand.Parameters.Add(new SqlParameter("@Comments", Comments));
            oCommand.Parameters.Add(new SqlParameter("@DateBirth", DateBirth));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            db.ExecuteNonQuery(oCommand);
        }

        public void UpdateStatus(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_UpdateStatus");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            db.ExecuteNonQuery(oCommand);
        }

        public bool ChangePwd(string UserName, string OldPassword, string NewPassword)
        {
            DataRow m_User_Info = GetInfo(UserName);
            if (m_User_Info == null || m_User_Info["Password"].ToString() != OldPassword)
                return false;
            try
            {
                SqlCommand oCommand = new SqlCommand("sp_User_ChangePwd");
                oCommand.CommandType = CommandType.StoredProcedure;
                oCommand.Parameters.Add(new SqlParameter("@Name", UserName));
                oCommand.Parameters.Add(new SqlParameter("@Password", NewPassword));
                db.ExecuteNonQuery(oCommand);
                return true;
            }
            catch { return false; }
        }

        public void Delete(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            db.ExecuteNonQuery(oCommand, new SqlParameter("@ID", ID));
        }

        public DataRow GetInfo(int ID)
        {
            DataTable dtRet = db.getDataTableSP("sp_User_Get", new SqlParameter("@ID", ID));
            if (dtRet == null || dtRet.Rows.Count == 0)
                return null;
            return dtRet.Rows[0];
        }

        public Users Get(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            List<Users> lRet = db.GetList<Users>(oCommand);
            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }

        public bool UpdateMatrix(string username, string Matrix)
        {
            try
            {
                Users m_User = new Users();
                m_User = m_User.GetbyUserName(username);

                DBHelper db = new DBHelper(Config.SQLConn);
                SqlCommand oCommand = new SqlCommand("sp_Users_UpdateMatrix");
                oCommand.CommandType = CommandType.StoredProcedure;
                oCommand.Parameters.Add(new SqlParameter("@UserName", username));
                oCommand.Parameters.Add(new SqlParameter("@Matrix", Matrix));
                db.ExecuteNonQuery(oCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Users GetbyUserName(string UserName)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_GetForUserName");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserName", UserName));
            List<Users> lRet = db.GetList<Users>(oCommand);
            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }

        public DataRow GetInfo(string UserName)
        {
            DataTable dtRet = db.getDataTableSP("sp_User_GetForUserName", new SqlParameter("@UserName", UserName));
            if (dtRet == null || dtRet.Rows.Count == 0)
                return null;
            return dtRet.Rows[0];
        }

        public List<Users> GetPagedbyList(string Keyword, int PartID, int Status, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_GetPaged");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Keyword", Keyword));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", CurrPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", RecordPerPage));

            SqlParameter output = new SqlParameter("@TotalRecord", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            List<Users> lRet = db.GetList<Users>(oCommand);
            if (lRet == null || lRet.Count == 0)
            {
                TotalRecord = 0;
                return null;
            }
            else
            {
                TotalRecord = Convert.ToInt32(output.Value);
                return lRet;
            }
        }

        public DataTable GetPaged(string Keyword, int PartID, int Status, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_GetPaged");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Keyword", Keyword));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", CurrPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", RecordPerPage));

            SqlParameter output = new SqlParameter("@TotalRecord", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            DataTable dtRet = db.getDataTable(oCommand);
            TotalRecord = Convert.ToInt32(output.Value);
            return dtRet;
        }

        public DataTable Get_User_GetDynamic(string WhereCondition, string OrderByExpression)
        {
            SqlCommand oCommand = new SqlCommand("sp_User_GetDynamic");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@WhereCondition", WhereCondition));
            oCommand.Parameters.Add(new SqlParameter("@OrderByExpression", OrderByExpression));
            return db.getDataTable(oCommand);
        }
    }
}