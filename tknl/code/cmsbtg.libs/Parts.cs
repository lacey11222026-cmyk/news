using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace cms.libs
{
    public class Parts
    {
     
        private DBHelper db = null;
        public Parts()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Parts()
        {

        }

        public virtual void Dispose()
        {
      //      [ID]
      //,[Name]
      //,[Telephone]
      //,[ContactUser]
      //,[Description]
      //,[WorkflowID]
      //,[Status]
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
        public string Telephone
        {
            get;
            set;
        }
        public string ContactUser
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int WorkflowID
        {
            get;
            set;
        }
        public int Status
        {
            get;
            set;
        }
        public int Insert(string Name, string Telephone, string ContactUser, string Description, int WorkflowID,int Status)
        {
            SqlCommand oCommand = new SqlCommand("sp_Part_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Name", Name));
            oCommand.Parameters.Add(new SqlParameter("@Telephone", Telephone));
            oCommand.Parameters.Add(new SqlParameter("@ContactUser", ContactUser));
            oCommand.Parameters.Add(new SqlParameter("@Description", Description));
            oCommand.Parameters.Add(new SqlParameter("@WorkflowID", WorkflowID));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            SqlParameter output = new SqlParameter("@ID", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }
        public void Update(int ID, string Name, string Telephone, string ContactUser, string Description, int WorkflowID)
        {
            SqlCommand oCommand = new SqlCommand("sp_Part_Update");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            oCommand.Parameters.Add(new SqlParameter("@Name", Name));
            oCommand.Parameters.Add(new SqlParameter("@Telephone", Telephone));
            oCommand.Parameters.Add(new SqlParameter("@ContactUser", ContactUser));
            oCommand.Parameters.Add(new SqlParameter("@Description", Description));
            oCommand.Parameters.Add(new SqlParameter("@WorkflowID", WorkflowID));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_Part_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            db.ExecuteNonQuery(oCommand);
        }
        public void UpdateStatus(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_Part_UpdateStatus");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            db.ExecuteNonQuery(oCommand);
        }
        public DataRow GetInfo(int ID)
        {
            DataTable dtRet = db.getDataTableSP("sp_Part_Get", new SqlParameter("@ID", ID));
            if (dtRet == null || dtRet.Rows.Count == 0)
                return null;
            return dtRet.Rows[0];
        }
        public DataTable GetAll()
        {
            return db.getDataTableSP("sp_Part_GetAll");
        }
        public DataTable GetAllActive()
        {
            return db.getDataTableSP("sp_Part_GetAll_Active");
        }
        
        public List<Parts> GetAllToList()
        {
            SqlCommand oCommand = new SqlCommand("sp_Part_GetAll");
            oCommand.CommandType = CommandType.StoredProcedure;
            return db.GetList<Parts>(oCommand);
        }
        public DataTable GetForUserID(int UserID)
        {
            return db.getDataTableSP("sp_Part_GetForUserID", new SqlParameter("@UserID", UserID));
        }
        public DataTable GetDataUserID(int UserID)
        {
            return db.getDataTableSP("sp_Part_GetDataUserID", new SqlParameter("@UserID", UserID));
        }

    }
}
