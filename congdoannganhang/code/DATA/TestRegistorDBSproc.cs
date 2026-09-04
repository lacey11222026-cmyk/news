using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class TestRegistorDBSproc: TestRegistorDBBase
    {
        #region Overrides of TestRegistorDBBase

        public override int CreateUpdateTestRegistor ( TestRegistor manufactory )
        {
            try
            {
                int _id = manufactory.Id;
                string _title = manufactory.Title;
                string _desciption = manufactory.Desciption;
                DateTime? _endTime = manufactory.EndTime;
                DateTime? _startTime = manufactory.StartTime;
                int? _number = manufactory.NumberQuestion;
                int? _status = manufactory.Status;
                int? _testTime = manufactory.TestTime;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_TestRegistor_InsertUpdate(_id,_title, _desciption, _startTime, _endTime, _status,_number, _testTime);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        

        public override IEnumerable<TestRegistor> GetTestRegistorsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_TestRegistor_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        public override IEnumerable<TestRegistor> GetAllTestRegistorsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_TestRegistor_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
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
        public override IEnumerable<TestRegistor> GetTestRegistor( )
        {
            var select = "*";
            var where = "";
    
            var order = "Id DESC";

            return GetTestRegistorsDyn ( select, where, order );

        }
        public override TestRegistor GetById(int Id)
        {
            var select = "*";
            var where = "[Id]="+ Id;

            var order = "Id DESC";

            return GetTestRegistorsDyn(select, where, order).FirstOrDefault();

        }
        public override IEnumerable<TestRegistor> GetAll(string keyword,int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";
            var where = "";
            keyword = Utils.FormatKeywordSearch(keyword);
            if (!string.IsNullOrEmpty(keyword))
                where += " ( Title LIKE N'%" + keyword + "%') ";
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status=" + status.ToString();
            }
            var order = "Id DESC";

            return GetAllTestRegistorsPagedDyn(select, where, order, pageIndex, pageSize,ref totalRecords);

        }
        public override IEnumerable<TestRegistor> GetAll()
        {
            var select = "*";
            var where = "";
           
            var order = "Id DESC";

            return GetTestRegistorsDyn(select, where, order);

        }
        public override int DeleteDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_TestRegistor_DeleteDynamic(where);

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
