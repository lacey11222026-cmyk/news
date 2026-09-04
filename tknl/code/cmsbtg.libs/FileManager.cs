using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace cms.libs
{
   
    public class FileManager
    {
        public enum FileType : byte
        {
            Image = 1,
            Music = 2,
            Clip = 3,
            Document = 4,
            Flash = 5,
            ArticleMedia = 6
        }
        private DBHelper db = null;
        public FileManager()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~FileManager()
        {

        }

        public virtual void Dispose()
        {

        }
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Type
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public string Length
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
        public int UserID
        {
            get;
            set;
        }
        public DateTime CrTime
        {
            get;
            set;
        }
       
        public int Insert()
        {
            SqlCommand oCommand = new SqlCommand("sp_FileManager_Insert");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Name", Name));
            oCommand.Parameters.Add(new SqlParameter("@Type", Type));
            oCommand.Parameters.Add(new SqlParameter("@Width", Width));
            oCommand.Parameters.Add(new SqlParameter("@Height", Height));
            oCommand.Parameters.Add(new SqlParameter("@Length", Length));
            oCommand.Parameters.Add(new SqlParameter("@Description", Description));
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@CrTime", DateTime.Now));
            SqlParameter output = new SqlParameter("@ID", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            db.ExecuteNonQuery(oCommand);
            return (int)output.Value;
        }
        public void Delete(int Id)
        {
            SqlCommand oCommand = new SqlCommand("sp_FileManager_Delete");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", Id));
            db.ExecuteNonQuery(oCommand);
        }
        public FileManager Get(int Id)
        {
            SqlCommand oCommand = new SqlCommand("sp_FileManager_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", Id));
            List<FileManager> lRet = db.GetList<FileManager>(oCommand);
            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }
        public List<FileManager> GetPaged(int UserID, FileType Type, string Keyword, DateTime FromDate, DateTime ToDate, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            SqlCommand oCommand = new SqlCommand("sp_FileManager_GetPaged");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
            oCommand.Parameters.Add(new SqlParameter("@Type", Type));
            oCommand.Parameters.Add(new SqlParameter("@Keyword", Keyword));
            oCommand.Parameters.Add(new SqlParameter("@FromDate", FromDate));
            oCommand.Parameters.Add(new SqlParameter("@ToDate", ToDate));
            oCommand.Parameters.Add(new SqlParameter("@CurrPage", CurrPage));
            oCommand.Parameters.Add(new SqlParameter("@RecordPerPage", RecordPerPage));
            SqlParameter output = new SqlParameter("@TotalRecord", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            oCommand.Parameters.Add(output);
            List<FileManager> lRet = db.GetList<FileManager>(oCommand);
            TotalRecord = (int)output.Value;
            return lRet;
        }
        public static string GetUniqueFileName(string strDir, string strFileName)
        {
            //if (strDir != "" && !(strDir.EndsWith("/") || strDir.EndsWith("\\"))) strDir += "/";

            //string strExt = System.IO.Path.GetExtension(strFileName);
            //strFileName = System.IO.Path.GetFileNameWithoutExtension(strFileName);
            //strFileName = strFileName.Replace(" ", "_");
            //strFileName = strFileName + strExt;

            //int file_append = 0;
            //string m_strFileName = strFileName;
            //string m_append = "";
            
            //while (System.IO.File.Exists(strDir + m_strFileName))
            //{
            //    file_append++;
            //    m_append = "(" + file_append.ToString() + ")";
            //    m_strFileName = System.IO.Path.GetFileNameWithoutExtension(strFileName) + m_append + strExt;
            //}
            //return m_strFileName;


            if (strDir != "" && !(strDir.EndsWith("/") || strDir.EndsWith("\\"))) strDir += "/";
            //strFileName = Common.UCS2Convert(strFileName);
            string strExt = System.IO.Path.GetExtension(strFileName);
            strFileName = System.IO.Path.GetFileNameWithoutExtension(strFileName);
            strFileName = Common.UCS2Convert(strFileName);
            strFileName = strFileName.Replace(" ", "_");

            int file_append = 0;
            string m_strFileName = strFileName;
            string m_append = "";

            while (System.IO.Directory.GetFiles(strDir, m_strFileName + ".*").Length >0)
            {
                file_append++;
                m_append = "_" + file_append.ToString();
                m_strFileName = System.IO.Path.GetFileNameWithoutExtension(strFileName) + m_append;
            }
            return m_strFileName + strExt;
        }
    }
}
