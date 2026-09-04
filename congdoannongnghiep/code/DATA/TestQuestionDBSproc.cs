using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class TestQuestionDBSproc: TestQuestionDBBase
    {
        #region Overrides of TestQuestionDBBase

        public override int CreateUpdateTestQuestion ( TestQuestion manufactory )
        {
            try
            {
                int _id = manufactory.Id;
                string _contents = manufactory.Contents;
                string _title = manufactory.Title;
                string _answers = manufactory.Answers;
                string _explain = manufactory.Explain;
                int? _mark = manufactory.Mark;
                int? _type = manufactory.Type;
                int? _status = manufactory.Status;
                int? _registorId = manufactory.RegistorId;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_TestQuestion_InsertUpdate(_id, _title, _contents, _mark, _type, _registorId, _explain, _answers, _status);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        

        public override IEnumerable<TestQuestion> GetTestQuestionsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_TestQuestion_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override IEnumerable<TestQuestion> GetAllTestQuestionsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_TestQuestion_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override IEnumerable<TestQuestion> GetByRegistorId(int id,int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";
            var where = "";

            if (id > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where = "  RegistorId=" + id;
            }


            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where = "  Status=" + status;
            }
                
            var order = "Id DESC";

            return GetAllTestQuestionsPagedDyn(select, where, order, pageIndex, pageSize,ref totalRecords);

        }
        public override IEnumerable<TestQuestion> GetTestQuestion( )
        {
            var select = "*";
            var where = "[Status]=1 ";
    
            var order = "Id DESC";

            return GetTestQuestionsDyn ( select, where, order );

        }
        public override TestQuestion GetById(int Id)
        {
            var select = "*";
            var where = "[Id]=" + Id;

            var order = "Id DESC";

            return GetTestQuestionsDyn(select, where, order).FirstOrDefault();

        }
        public override IEnumerable<TestQuestion> GetByRegistorId(int id)
        {
            var select = "*";
            var where = "[Status]=1 ";

            if(id>0)
                where += "And  RegistorId="+ id;
            var order = "Id DESC";

            return GetTestQuestionsDyn(select, where, order);

        }
        public override int DeleteDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_TestQuestion_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }


        public override int Delete(int documentId) { var where = "Id =" + documentId; return DeleteDyn(where); }
        

        #endregion
    }
}
