using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace cms.libs
{
    public partial class WorkflowTask
    {
        private DBHelper db = null;
        public WorkflowTask()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~WorkflowTask()
        { }
        public int Id
        {
            get;
            set;
        }

        public int WorkflowId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string RewriteUrl
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int HotOrder
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public int MoveUp
        {
            get;
            set;
        }

        public int MoveDown
        {
            get;
            set;
        }

        public bool AddnewEnabled
        {
            get;
            set;
        }

        public bool EditEnabled
        {
            get;
            set;
        }

        public bool DeleteEnabled
        {
            get;
            set;
        }
        public int Insert()
        {
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@WorkflowId", this.WorkflowId));
            oCommand.Parameters.Add(new SqlParameter("@Name", this.Name));
            oCommand.Parameters.Add(new SqlParameter("@RewriteUrl", this.RewriteUrl));
            oCommand.Parameters.Add(new SqlParameter("@Description", this.Description));
            oCommand.Parameters.Add(new SqlParameter("@HotOrder", this.HotOrder));
            oCommand.Parameters.Add(new SqlParameter("@Status", this.Status));
            oCommand.Parameters.Add(new SqlParameter("@MoveUp", this.MoveUp));
            oCommand.Parameters.Add(new SqlParameter("@MoveDown", this.MoveDown));
            oCommand.Parameters.Add(new SqlParameter("@AddnewEnabled", this.AddnewEnabled));
            oCommand.Parameters.Add(new SqlParameter("@EditEnabled", this.EditEnabled));
            oCommand.Parameters.Add(new SqlParameter("@DeleteEnabled", this.DeleteEnabled));
            SqlParameter output = new SqlParameter("@Id", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_Update");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", this.Id));
            oCommand.Parameters.Add(new SqlParameter("@WorkflowId", this.WorkflowId));
            oCommand.Parameters.Add(new SqlParameter("@Name", this.Name));
            oCommand.Parameters.Add(new SqlParameter("@RewriteUrl", this.RewriteUrl));
            oCommand.Parameters.Add(new SqlParameter("@Description", this.Description));
            oCommand.Parameters.Add(new SqlParameter("@HotOrder", this.HotOrder));
            oCommand.Parameters.Add(new SqlParameter("@Status", this.Status));
            oCommand.Parameters.Add(new SqlParameter("@MoveUp", this.MoveUp));
            oCommand.Parameters.Add(new SqlParameter("@MoveDown", this.MoveDown));
            oCommand.Parameters.Add(new SqlParameter("@AddnewEnabled", this.AddnewEnabled));
            oCommand.Parameters.Add(new SqlParameter("@EditEnabled", this.EditEnabled));
            oCommand.Parameters.Add(new SqlParameter("@DeleteEnabled", this.DeleteEnabled));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", this.Id));
            db.ExecuteNonQuery(oCommand);
        }
        public WorkflowTask Get()
        {
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", this.Id));
            List<WorkflowTask> lRet = db.GetList<WorkflowTask>(oCommand);

            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }
        public WorkflowTask GetByStatus(int WorkflowId, int Status)
        { 
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_GetByStatus");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@WorkflowId", WorkflowId));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            List<WorkflowTask> lRet = db.GetList<WorkflowTask>(oCommand);

            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }
        public List<WorkflowTask> GetByWorkflowId()
        {
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_GetByWorkflowId");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@WorkflowId", this.WorkflowId));
            return db.GetList<WorkflowTask>(oCommand);
        }
        public List<WorkflowTask> GetByUserIdAndPartId(int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("sp_WorkflowTask_GetByUserIdAndPartId");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            return db.GetList<WorkflowTask>(oCommand);
        }
    }
}

