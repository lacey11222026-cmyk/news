using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace cms.libs
{
    public class Workflows
    {
       
        private DBHelper db = null;
        public Workflows()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Workflows()
        {

        }

        public virtual void Dispose()
        {

        }
        public int Id
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

        public bool IsDefault
        {
            get;
            set;
        }
        public short ViewPolicy
        {
            get;
            set;
        }
        public int Status
        {
            get;
            set;
        }
        public int Insert()
        {
            SqlCommand oCommand = new SqlCommand("sp_Workflow_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Name", this.Name));
            oCommand.Parameters.Add(new SqlParameter("@Description", this.Description));
            oCommand.Parameters.Add(new SqlParameter("@IsDefault", this.IsDefault));
            oCommand.Parameters.Add(new SqlParameter("@ViewPolicy", this.ViewPolicy));
            oCommand.Parameters.Add(new SqlParameter("@Status", this.Status));

            SqlParameter output = new SqlParameter("@Id", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("sp_Workflow_Update");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", this.Id));
            oCommand.Parameters.Add(new SqlParameter("@Name", this.Name));
            oCommand.Parameters.Add(new SqlParameter("@Description", this.Description));
            oCommand.Parameters.Add(new SqlParameter("@IsDefault", this.IsDefault));
            oCommand.Parameters.Add(new SqlParameter("@ViewPolicy", this.ViewPolicy));
            oCommand.Parameters.Add(new SqlParameter("@Status", this.Status));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("sp_Workflow_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", this.Id));
            db.ExecuteNonQuery(oCommand);
        }
        public Workflows Get()
        {
            SqlCommand oCommand = new SqlCommand("sp_Workflow_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", this.Id));
            List<Workflows> lRet = db.GetList<Workflows>(oCommand);

            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }

        public List<Workflows> GetAll()
        {
            SqlCommand oCommand = new SqlCommand("sp_Workflow_GetAll");
            oCommand.CommandType = CommandType.StoredProcedure;
            List<Workflows> lRet = db.GetList<Workflows>(oCommand);

            return lRet;
        }
        public DataTable GetByPartID(int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("sp_Workflow_GetByPartID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            return db.getDataTable(oCommand);
        }
        public DataTable GetAllStatus()
        {
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("text");
            dtRet.Columns.Add("value");
            DataRow dr = dtRet.NewRow();
            dr["text"] = "N/A";
            dr["value"] = "-1";
            dtRet.Rows.Add(dr);
            int MaxStatus = Constants.MaxStatus;
            for (int i = 0; i <= MaxStatus; i++)
            {
                dr = dtRet.NewRow();
                dr["text"] = i.ToString();
                dr["value"] = i.ToString();
                dtRet.Rows.Add(dr);
            }
            return dtRet;
        }
    }
}
