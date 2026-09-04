using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class Sites
    {
        private DBHelper db = null;
        public Sites()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Sites()
        {

        }

        public virtual void Dispose()
        {

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
        public string Description
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
        public int OrderNo
        {
            get;
            set;
        }
        public int Status
        {
            get;
            set;
        }
        public Sites Get()
        {
            SqlCommand oCommand = new SqlCommand("sp_Site_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", this.ID));
            List<Sites> lRet= db.GetList<Sites>(oCommand);
            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }
        public List<Sites> GetAll()
        {
            SqlCommand oCommand = new SqlCommand("sp_Site_GetAll");
            oCommand.CommandType = CommandType.StoredProcedure;
            return db.GetList<Sites>(oCommand);
        }
    }
}
