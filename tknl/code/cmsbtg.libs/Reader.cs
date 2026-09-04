using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class Reader
    {
         private DBHelper db = null;
        public Reader()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Reader()
        {

        }

        public virtual void Dispose()
        {

        }
        public long ReaderId
        {
            get;
            set;
        }
        public string FullName
        {
            get;
            set;
        }
        public int Gender
        {
            get;
            set;
        }
        public int Age
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string Job
        {
            get;
            set;
        }
        public int DiscussId
        {
            get;
            set;
        }
        public long Insert()
        {
            SqlParameter output = new SqlParameter("@ReaderId", SqlDbType.BigInt);
            output.Direction = ParameterDirection.Output;
            SqlCommand oCommand = new SqlCommand("Insert_Reader");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FullName", FullName));
            oCommand.Parameters.Add(new SqlParameter("@Gender", Gender));
            oCommand.Parameters.Add(new SqlParameter("@Age", Age));
            oCommand.Parameters.Add(new SqlParameter("@Address", Address));
            oCommand.Parameters.Add(new SqlParameter("@Job", Job));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return Convert.ToInt64(output.Value);
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("Insert_Reader");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FullName", FullName));
            oCommand.Parameters.Add(new SqlParameter("@Gender", Gender));
            oCommand.Parameters.Add(new SqlParameter("@Age", Age));
            oCommand.Parameters.Add(new SqlParameter("@Address", Address));
            oCommand.Parameters.Add(new SqlParameter("@Job", Job));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("Delete_Reader");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ReaderId", ReaderId));
            db.ExecuteNonQuery(oCommand);
        }
        public Reader Get()
        {
            SqlCommand oCommand = new SqlCommand("Reader_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ReaderId", ReaderId));
            List<Reader> lRet = db.GetList<Reader>(oCommand);
            if (lRet != null && lRet.Count > 0)
                return lRet[0];
            return null;
        }
        public List<Reader> GetPaged(string Keyword, int DiscussId, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            SqlCommand oCommand = new SqlCommand("sp_Reader_GetPaged");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Keyword", Keyword));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", CurrPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", RecordPerPage));

            SqlParameter output = new SqlParameter("@TotalRecord", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            List<Reader> lRet = db.GetList<Reader>(oCommand);
            TotalRecord = Convert.ToInt32(output.Value);
            return lRet;
        }
    }
}
