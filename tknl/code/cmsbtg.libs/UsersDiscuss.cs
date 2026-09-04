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
    public class UsersDiscuss
    {
        private DBHelper db = null;

        public UsersDiscuss()
        {
            db = new DBHelper(Config.SQLConn);
        }

        public int UserId
        {
            get;
            set;
        }

        public int DisscussId
        {
            get;
            set;
        }

        public int GuestId
        {
            get;
            set;
        }


        public void Insert()
        {
            SqlCommand oCommand = new SqlCommand("Insert_Update_User_Disscuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserId", UserId));
            oCommand.Parameters.Add(new SqlParameter("@DisscussId", DisscussId));
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            db.ExecuteNonQuery(oCommand);
        }


        public UsersDiscuss Get()
        {
            SqlCommand oCommand = new SqlCommand("Discuss_User_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserId", UserId));
            List<UsersDiscuss> lRet = db.GetList<UsersDiscuss>(oCommand);
            if (lRet != null && lRet.Count > 0)
                return lRet[0];
            return null;

        }
    }
}