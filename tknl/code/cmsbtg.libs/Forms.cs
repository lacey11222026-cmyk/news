using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public  class Forms
    {
        private DBHelper db = null;
        public Forms()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Forms()
        {

        }

        public virtual void Dispose()
        {

        }
                
        public void Insert(string FormName, string FormDisplayName, int FormTypeID, int FormControlID, string FormPart, string FormDescription, int FormSkin, int FormStatus )
        {
            string RewriteUrl = Common.UCS2Convert(FormName).Replace(" ", "-");
            while (RewriteUrl.IndexOf("--") > -1)
            {
                RewriteUrl = RewriteUrl.Replace("--", "-");
            }
            SqlCommand oCommand = new SqlCommand("sp_Forms_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FormName", FormName));
            oCommand.Parameters.Add(new SqlParameter("@FormDisplayName", FormDisplayName));
            oCommand.Parameters.Add(new SqlParameter("@FormTypeID", FormTypeID ));
            oCommand.Parameters.Add(new SqlParameter("@FormControlID", FormControlID));
            oCommand.Parameters.Add(new SqlParameter("@FormPart", FormPart));
            oCommand.Parameters.Add(new SqlParameter("@FormDescription", FormDescription));
            oCommand.Parameters.Add(new SqlParameter("@FormSkin", FormSkin));
            oCommand.Parameters.Add(new SqlParameter("@FormStatus", FormStatus));


            //output.Direction = ParameterDirection.Output;
            //oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            //return (int)output.Value;
        }
        public void Update(int ID,string FormName, string FormDisplayName, int FormTypeID, int FormControlID, string FormPart, string FormDescription, int FormSkin,int FormStatus)
        {
            string RewriteUrl = Common.UCS2Convert(FormName).Replace(" ", "-");
            while (RewriteUrl.IndexOf("--") > -1)
            {
                RewriteUrl = RewriteUrl.Replace("--", "-");
            }
            SqlCommand oCommand = new SqlCommand("sp_Forms_Update");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            oCommand.Parameters.Add(new SqlParameter("@FormName", FormName));
            oCommand.Parameters.Add(new SqlParameter("@FormDisplayName", FormDisplayName));
            oCommand.Parameters.Add(new SqlParameter("@FormTypeID", FormTypeID));
            oCommand.Parameters.Add(new SqlParameter("@FormControlID", FormControlID));
            oCommand.Parameters.Add(new SqlParameter("@FormPart", FormPart));
            oCommand.Parameters.Add(new SqlParameter("@FormDescription", FormDescription));
            oCommand.Parameters.Add(new SqlParameter("@FormSkin", FormSkin));
            oCommand.Parameters.Add(new SqlParameter("@FormStatus", FormStatus));
            db.ExecuteNonQuery(oCommand);
            
        }
        public void Delete(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_Forms_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", ID));
            db.ExecuteNonQuery(oCommand);
        }
        public void DeleteFormID(int ID)
        {
            SqlCommand oCommand = new SqlCommand("sp_Forms_DeleteFormID");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FormID", ID));
            db.ExecuteNonQuery(oCommand);
        }
        public DataTable GetForms()
        {
            return db.getDataTableSP("sp_Forms_GetAll");
        }
       
        public DataRow GetInfo(int ID)
        {
            DataTable dtRet = db.getDataTableSP("sp_Forms_GetID", new SqlParameter("@ID", ID));
            if (dtRet == null || dtRet.Rows.Count == 0)
                return null;
            return dtRet.Rows[0];
        }
        public DataTable GetLayoutForms(string WhereCondition, string OrderByExpression)
        {

            SqlCommand oCommand = new SqlCommand("sp_Layout_Forms_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@WhereCondition", WhereCondition));
            oCommand.Parameters.Add(new SqlParameter("@OrderByExpression", OrderByExpression));
            DataTable dtRet = db.getDataTable(oCommand);
            return dtRet;
        }
        
    }
}
