using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace cms.libs
{
    public class LOGs
    {
        private DBHelper db = null;
        public LOGs()
        {
            db = new DBHelper(Config.SQLConn);
        }

        ~LOGs()
        {

        }

        public virtual void Dispose()
        {

        }
        public int LogId
        {
            get;
            set;
        }
        public int LogUser
        {
            get;
            set;
        }
        public string LogDesc
        {
            get;
            set;
        }
        public string LogIP
        {
            get;
            set;
        }
        public DateTime LogTime
        {
            get;
            set;
        }
       
        public void Add(int LogUser, string LogDesc, string LogIP)
        {
            SqlCommand cmd = new SqlCommand("Log_Add");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@LogUser", LogUser));
            cmd.Parameters.Add(new SqlParameter("@LogDesc", LogDesc));
            cmd.Parameters.Add(new SqlParameter("@LogIP", LogIP));
            db.ExecuteNonQuery(cmd);
        }
        public void Delete()
        {
            SqlCommand cmd = new SqlCommand("Log_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@LogId", this.LogId));
            db.ExecuteNonQuery(cmd);
        }
       
    }

}
