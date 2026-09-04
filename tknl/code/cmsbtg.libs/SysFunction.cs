using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.UI.WebControls;

namespace cms.libs
{
    public class SysFunction
    {
         private DBHelper db = null;
        public SysFunction()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~SysFunction()
        {

        }

        public virtual void Dispose()
        {

        }
        public DataTable GetAll()
        {
            return db.getDataTableSP("sp_SysFunction_GetAll");
        }
        public DataTable GetByUserID(int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_SysFunction_GetByUserID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            return db.getDataTable(oCommand);
        }
    }
}
