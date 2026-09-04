using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class Guest
    {
        private DBHelper db = null;
        public Guest()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Guest()
        {

        }

        public virtual void Dispose()
        {

        }
        public int GuestId
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
        public string Job
        {
            get;
            set;
        }
        public int UserId
        {
            get;
            set;
        }
        public int DiscussId
        {
            get;
            set;
        }
        public void Insert()
        {
            SqlCommand oCommand = new SqlCommand("Insert_Guest");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@FullName", FullName));
            oCommand.Parameters.Add(new SqlParameter("@Gender", Gender));
            oCommand.Parameters.Add(new SqlParameter("@Job", Job));
            oCommand.Parameters.Add(new SqlParameter("@UserId", UserId));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("Update_Guest");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            oCommand.Parameters.Add(new SqlParameter("@FullName", FullName));
            oCommand.Parameters.Add(new SqlParameter("@Gender", Gender));
            oCommand.Parameters.Add(new SqlParameter("@Job", Job));
            oCommand.Parameters.Add(new SqlParameter("@UserId", UserId));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("Delete_Guest");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            db.ExecuteNonQuery(oCommand);
        }
        public void DeleteByDiscussId()
        {
            SqlCommand oCommand = new SqlCommand("Delete_GuestByDiscussId");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
        }
        public List<Guest> GetByDiscussId()
        {
            SqlCommand oCommand = new SqlCommand("sp_Guest_GetByDiscussId");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            return db.GetList<Guest>(oCommand);
        }

        public Guest GetById()
        {
            SqlCommand oCommand = new SqlCommand("sp_Guest_GetById");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            List<Guest> lst = db.GetList<Guest>(oCommand);
            if (lst.Count > 0)
                return lst[0];
            return null;
        }
    }
}
