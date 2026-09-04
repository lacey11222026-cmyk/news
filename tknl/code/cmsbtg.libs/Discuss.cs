using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace cms.libs
{
    public class Discuss
    {
        private DBHelper db = null;
        public Discuss()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~Discuss()
        {

        }

        public virtual void Dispose()
        {

        }
        public int DiscussId
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string SubTitle
        {
            get;
            set;
        }
        public string Summary
        {
            get;
            set;
        }
        public string TopicDiscussion
        {
            get;
            set;
        }
        public string ImgPath
        {
            get;
            set;
        }
        public DateTime DateCreate
        {
            get;
            set;
        }
        public int SiteId
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public int Status
        {
            get;
            set;
        }
        public int SaveArticle
        {
            get;
            set;
        }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int Insert()
        {
            SqlCommand oCommand = new SqlCommand("Insert_Discuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Title", Title));
            oCommand.Parameters.Add(new SqlParameter("@SubTitle", SubTitle));
            oCommand.Parameters.Add(new SqlParameter("@Summary", Summary));
            oCommand.Parameters.Add(new SqlParameter("@TopicDiscussion", TopicDiscussion));
            oCommand.Parameters.Add(new SqlParameter("@ImgPath", ImgPath));
            oCommand.Parameters.Add(new SqlParameter("@DateCreate", DateCreate));
            oCommand.Parameters.Add(new SqlParameter("@SiteId", SiteId));
            oCommand.Parameters.Add(new SqlParameter("@Type", Type));
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            oCommand.Parameters.Add(new SqlParameter("@SaveArticle", SaveArticle));
            oCommand.Parameters.Add(new SqlParameter("@StartTime", StartTime));
            oCommand.Parameters.Add(new SqlParameter("@EndTime", EndTime));
            SqlParameter output = new SqlParameter("@DiscussId", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("Update_Discuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@Title", Title));
            oCommand.Parameters.Add(new SqlParameter("@SubTitle", SubTitle));
            oCommand.Parameters.Add(new SqlParameter("@Summary", Summary));
            oCommand.Parameters.Add(new SqlParameter("@TopicDiscussion", TopicDiscussion));
            oCommand.Parameters.Add(new SqlParameter("@ImgPath", ImgPath));
            oCommand.Parameters.Add(new SqlParameter("@StartTime", StartTime));
            oCommand.Parameters.Add(new SqlParameter("@EndTime", EndTime));
            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("Delete_Discuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
            new Guest { DiscussId = DiscussId }.DeleteByDiscussId();
        }
        public Discuss Get()
        {
            SqlCommand oCommand = new SqlCommand("Discuss_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            List<Discuss> lRet = db.GetList<Discuss>(oCommand);
            if (lRet != null && lRet.Count > 0)
                return lRet[0];
            return null;
        }
        public List<Discuss> GetAll(int Status)
        {
            SqlCommand oCommand = new SqlCommand("sp_Discuss_GetAll");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Status", Status));
            return db.GetList<Discuss>(oCommand);
        }

        public List<Discuss> GetAllPaged(int status, int currPage, int recordPerPage)
        {
            SqlCommand oCommand = new SqlCommand("sp_Discuss_GetPagedPassed");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Status", status));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", currPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", recordPerPage));

            return db.GetList<Discuss>(oCommand);
        }
        
        public Discuss GetFirst()
        {
            SqlCommand oCommand = new SqlCommand("Discuss_GetFirst");
            oCommand.CommandType = CommandType.StoredProcedure;
            List<Discuss> lRet = db.GetList<Discuss>(oCommand);
            if (lRet != null && lRet.Count > 0)
                return lRet[0];
            return null;
        }
        public void UpdateStatus()
        {
            SqlCommand oCommand = new SqlCommand("sp_Discuss_UpdateStatus");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
        }

        

        public string getRewrite()
        {
            return DBCommon.StripDiacritics(this.Title);
        }
    }
}
