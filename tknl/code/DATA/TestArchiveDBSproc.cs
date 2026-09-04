using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class TestArchiveDBSproc: TestArchiveDBBase
    {
        #region Overrides of TestArchiveDBBase

        public override int CreateUpdateTestArchive ( TestArchive manufactory )
        {
            try
            {
                int _id = manufactory.Id;
                string _archive = manufactory.Archive;
                string _questions = manufactory.Questions;
                string _fulname = manufactory.FulName;
                string _location = manufactory.Location;
                string _mobile = manufactory.Mobile;
                string Role = manufactory.Role;
                int Note = manufactory.Note.GetValueOrDefault();
                int? _mark = manufactory.Mark;
                int? _TestTime = manufactory.TestTime;
                int? _resgitorId = manufactory.RegistorId;
                int? _status = manufactory.Status;
                DateTime? _startTime = manufactory.StartTime;
                DateTime? _endTime = manufactory.EndTime;
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_TestArchive_InsertUpdate(_id, _mobile, _fulname, _location, _resgitorId, _startTime, _endTime, _questions, _archive,_mark, _status, _TestTime,Role,Note);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<TestArchiveTop> SelectTop()
        {
            try
            {
              
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_TestArchive_SelectTop().ToArray();
                   

                    return results;
                }

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override IEnumerable<TestArchive> GetAllTestArchivesPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_TestArchive_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
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
        public override IEnumerable<TestArchive> GetTestArchivesDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_TestArchive_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
       
        public override IEnumerable<TestArchive> GetByRegistorId(int id, string mobile, int pageIndex, int pageSize, ref int totalRecords,int OrderType,int status)
        {
            var select = "*";
            var where = "";

            if (id > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "  RegistorId=" + id;
            }
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "  Status=" + status;
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " ( Mobile="  + "'" + mobile + "' OR [Role] LIKE N'%" + mobile + "%' ) ";
            }
            var order = "Id DESC";
            select = " [Id] ,[Mobile],[Location] ,[Role],[Note]  ,[FulName] ,[RegistorId],[StartTime] ,[EndTime] ,[Questions] ,[Archive],[Mark] ,[Status] ,[CreatedDate] ,DATEDIFF(millisecond , [StartTime] , [EndTime] )  as  [TestTime]";


            if (OrderType == 2)
            {
                            
                order = "Mark DESC, Num ASC,DATEDIFF ( millisecond , [StartTime] , [EndTime] )   ASC";
            }

            return GetAllTestArchivesPagedDyn(select, where, order,pageIndex,pageSize,ref totalRecords);

        }
        public override TestArchive GetById(int id)
        {
            var select = "*";
           
            var where = "  Id=" + id;

         
            var order = "Id DESC";

            return GetTestArchivesDyn(select, where, order).FirstOrDefault();

        }
        public override List<TestArchive> GetByMobile(int id,string mobile)
        {
            try
            {
                var select = "[Id] ,[Mobile],[Location],[Role],[Note]  ,[FulName] ,[RegistorId],[StartTime] ,[EndTime] ,[Questions] ,[Archive],[Mark] ,[Status] ,[CreatedDate] ,DATEDIFF(millisecond , [StartTime] , [EndTime] )  as  [TestTime]";
                var where = "";

                if (id > 0)
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += "  RegistorId=" + id;
                }

                if (!string.IsNullOrEmpty(mobile))
                {
                    if (!string.IsNullOrEmpty(where))
                        where += " AND ";

                    where += "  Mobile='" + mobile+"'";
                }
                var order = "Id DESC";

                return GetTestArchivesDyn(select, where, order).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public override List<TestArchiveReport> Report(int id)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_TestArchive_Report(id).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }

        }
        public override int DeleteDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_TestArchive_DeleteDynamic(where);

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
