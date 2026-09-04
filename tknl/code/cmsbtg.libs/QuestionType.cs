using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cms.libs
{
    public class QuestionType
    {
        private DBHelper db = null;
        public QuestionType()
        {
            db = new DBHelper(Config.SQLConn);
        }
        ~QuestionType()
        {

        }

        public virtual void Dispose()
        {

        }
        public int CategoryID
        {
            get;
            set;
        }
        public string CategoryName
        {
            get;
            set;
        }
        public string CategoryRewrite
        {
            get;
            set;
        }
        public int CategoryOrder
        {
            get;
            set;
        }
        public QuestionType Get()
        {
            SqlCommand oCommand = new SqlCommand("sp_QuestionType_Get");
            oCommand.CommandType = CommandType.StoredProcedure;
            oCommand.Parameters.Add(new SqlParameter("@Id", CategoryID));
            List<QuestionType> lRet = db.GetList<QuestionType>(oCommand);
            if (lRet == null || lRet.Count == 0)
                return null;
            return lRet[0];
        }
        public List<QuestionType> GetAll()
        {
            SqlCommand oCommand = new SqlCommand("sp_QuestionType_GetAll");
            oCommand.CommandType = CommandType.StoredProcedure;
            return db.GetList<QuestionType>(oCommand);
        }
    }
}
