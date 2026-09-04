using cms.libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cmsbtg.libs
{

    public class DBArticles
    {
        private DBHelper db = null;
        public DBArticles()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~DBArticles()
        {

        }
        public virtual void Dispose()
        {

        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int DiscussId { get; set; }
        public string ImgPath { get; set; }

        public int Insert()
        {
            SqlCommand oCommand = new SqlCommand("Insert_Article_Discuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Title", Title));
            oCommand.Parameters.Add(new SqlParameter("@Content", Content));
            oCommand.Parameters.Add(new SqlParameter("@ImgPath", ImgPath));
            oCommand.Parameters.Add(new SqlParameter("@CreateDate", CreateDate));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));

            SqlParameter output = new SqlParameter("@Id", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }
        public void Update()
        {
            SqlCommand oCommand = new SqlCommand("Update_Article_Discuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Title", Title));
            oCommand.Parameters.Add(new SqlParameter("@Content", Content));
            oCommand.Parameters.Add(new SqlParameter("@ImgPath", ImgPath));
            oCommand.Parameters.Add(new SqlParameter("@CreateDate", CreateDate));
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            oCommand.Parameters.Add(new SqlParameter("@Id", ID));

            db.ExecuteNonQuery(oCommand);
        }
        public void Delete()
        {
            SqlCommand oCommand = new SqlCommand("Delete_Article_Discuss");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            db.ExecuteNonQuery(oCommand);
            new Guest { DiscussId = DiscussId }.DeleteByDiscussId();
        }
        public DBArticles Get()
        {
            SqlCommand oCommand = new SqlCommand("Discuss_Get_Article");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            List<DBArticles> lRet = db.GetList<DBArticles>(oCommand);
            if (lRet != null && lRet.Count > 0)
                return lRet[0];
            return null;
        }

        public List<DBArticles> GetListByDiscussIdPaged()
        {
            SqlCommand oCommand = new SqlCommand("Discuss_GetList_Article_ByDiscussId_Paged");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@DiscussId", DiscussId));
            List<DBArticles> lRet = db.GetList<DBArticles>(oCommand);

            return lRet;
        }
    }
}
