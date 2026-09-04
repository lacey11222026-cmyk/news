using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.UI.WebControls;

namespace cms.libs
{
    public class SysFunc_User
    {
        private DBHelper db = null;
        public SysFunc_User()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~SysFunc_User()
        {

        }

        public virtual void Dispose()
        {

        }
        public int FuncID
        {
            get;set;
        }
        public int UserID
        {
            get;
            set;
        }
        public void Insert(int FuncID, int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_SysFunc_User_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FuncID", FuncID));
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete(int FuncID, int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_SysFunc_User_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FuncID", FuncID));
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            db.ExecuteNonQuery(oCommand);
        }
        public DataTable GetByUserID(int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_SysFunc_User_GetByUserID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            return db.getDataTable(oCommand);
        }
        public DataRow GetInfo(int FuncID, int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_SysFunc_User_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@FuncID", FuncID));
            DataTable dtRet = db.getDataTable(oCommand);
            if (dtRet == null || dtRet.Rows.Count == 0)
                return null;
            return dtRet.Rows[0];
        }
    }
}
