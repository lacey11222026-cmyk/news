using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class UserTask
    {
        private DBHelper db = null;
        public UserTask()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~UserTask()
        {

        }

        public virtual void Dispose()
        {

        }
        public int UserID
        {
            get;
            set;
        }
        public int PartID
        {
            get;
            set;
        }
        public int TaskID
        {
            get;
            set;
        }
        public void Insert(int TaskID, int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@TaskID", TaskID));
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete(int TaskID, int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@TaskID", TaskID));
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            db.ExecuteNonQuery(oCommand);
        }
        public void DeleteByUserID(int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_DeleteByUserID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            db.ExecuteNonQuery(oCommand);
        }
        public void DeleteByPartID(int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("spUserTask_DeleteByPartID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            db.ExecuteNonQuery(oCommand);
        }
        public void DeleteByTaskID(int TaskID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_DeleteByWorkflowID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@TaskID", TaskID));
            db.ExecuteNonQuery(oCommand);
        }
        public DataTable GetByTaskID(int UserID, int TaskID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_GetByTaskID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@TaskID", TaskID));
            return db.getDataTable(oCommand);
        }
        public DataTable GetByUserID(int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_GetByUserID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            return db.getDataTable(oCommand);
        }

        public UserTask GetByUserIDMaxStatus(int UserID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_GetByUserIDMaxStatus");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            List<UserTask> dtRet = db.GetList<UserTask>(oCommand);

            if (dtRet.Count > 0)
                return dtRet[0]; 
             return null;
        }

        public DataTable GetByPartID(int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_GetByPartID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            return db.getDataTable(oCommand);
        }

        public DataRow GetInfo(int TaskID, int UserID, int PartID)
        {
            SqlCommand oCommand = new SqlCommand("sp_UserTask_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@TaskID", TaskID));
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@PartID", PartID));
            DataTable dtRet = db.getDataTable(oCommand);
            if (dtRet == null || dtRet.Rows.Count == 0)
                return null;
            return dtRet.Rows[0];
        }

        public UserTask GetByTaskID(int TaskID)
        {
            throw new NotImplementedException();
        }
    }
}
