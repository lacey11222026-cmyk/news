using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class DiscussDetail
    {
        private DBHelper db = null;
        public DiscussDetail()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~DiscussDetail()
        {

        }

        public virtual void Dispose()
        {

        }
        public long Id
        {
            get;
            set;
        }
        public int DiscussId
        {
            get;
            set;
        }
        public string Question
        {
            get;
            set;
        }
        public string Answer
        {
            get;
            set;
        }
        public int GuestId
        {
            get;
            set;
        }
        public DateTime DateQuestion
        {
            get;
            set;
        }
        public DateTime DateAnswer
        {
            get;
            set;
        }
        public int Status
        {
            get;
            set;
        }
        public long ReaderId
        {
            get;
            set;
        }
        public void Insert()
        {
            SqlCommand oCommand = new SqlCommand("Insert_DiscussDeTail");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@Question", Question));
            oCommand.Parameters.Add(new SqlParameter("@Answer", Answer));
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            oCommand.Parameters.Add(new SqlParameter("@DateQuestion", DateQuestion));
            oCommand.Parameters.Add(new SqlParameter("@DateAnswer", DateAnswer));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            oCommand.Parameters.Add(new SqlParameter("@ReaderId", ReaderId));
            db.ExecuteNonQuery(oCommand);
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("Update_DiscussDeTail");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", Id));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@Question", Question));
            oCommand.Parameters.Add(new SqlParameter("@Answer", Answer));
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("Delete_DiscussDeTail");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", Id));
            db.ExecuteNonQuery(oCommand);
        }
        public List<DiscussDetail> GetByDiscussIdAndReaderId(int DiscussId, long ReaderId)
        {
            SqlCommand oCommand = new SqlCommand("sp_DiscussDetail_GetByDiscussIdAndReaderId");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@ReaderId", ReaderId));
            return db.GetList<DiscussDetail>(oCommand);
        }
        public void UpdateStatus()
        {
            SqlCommand oCommand = new SqlCommand("sp_DiscussDetail_UpdateStatus");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@ID", Id));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            db.ExecuteNonQuery(oCommand);
        }

        public void UpdateCreateTime()
        {
            SqlCommand oCommand = new SqlCommand("Update_DiscussDetails_DateTime");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", Id));
            oCommand.Parameters.Add(new SqlParameter("@DateAnswer", DateAnswer));
            db.ExecuteNonQuery(oCommand);
        }
    }
}
