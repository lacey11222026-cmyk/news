using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class ReaderDiscuss
    {
        private DBHelper db = null;
        public ReaderDiscuss()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~ReaderDiscuss()
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

        public List<ReaderDiscuss> GetPagedPassed(int DiscussId, int GuestId, int Status, DateTime FromDate, DateTime ToDate, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            SqlCommand oCommand = new SqlCommand("sp_ReaderDiscuss_GetPagedPassed");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            oCommand.Parameters.Add(new SqlParameter("@FromDate", FromDate));
            oCommand.Parameters.Add(new SqlParameter("@ToDate", ToDate));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", CurrPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", RecordPerPage));
            SqlParameter output = new SqlParameter("@TotalRecord", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            List<ReaderDiscuss> lRet = db.GetList<ReaderDiscuss>(oCommand);
            TotalRecord = (int)output.Value;
            return lRet;
        }
        public List<ReaderDiscuss> GetPagedPending(int DiscussId, int GuestId, int Status, DateTime FromDate, DateTime ToDate, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            SqlCommand oCommand = new SqlCommand("sp_ReaderDiscuss_GetPagedPending");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@GuestId", GuestId));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            oCommand.Parameters.Add(new SqlParameter("@FromDate", FromDate));
            oCommand.Parameters.Add(new SqlParameter("@ToDate", ToDate));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", CurrPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", RecordPerPage));
            SqlParameter output = new SqlParameter("@TotalRecord", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            List<ReaderDiscuss> lRet = db.GetList<ReaderDiscuss>(oCommand);
            TotalRecord = (int)output.Value;
            return lRet;
        }
        public ReaderDiscuss Get()
        {
            SqlCommand oCommand = new SqlCommand("sp_ReaderDiscuss_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", Id));
            List<ReaderDiscuss> lRet = db.GetList<ReaderDiscuss>(oCommand);
            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }
    }
}
