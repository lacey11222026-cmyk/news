using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class TechProcessDBSproc : TechProcessDBBase
    {
        public override int CreateUpdateTechProcess(TechProcess TechProcess)
        {
            try
            {
                int? _id = TechProcess.Id;

                string _categoryid = TechProcess.Topic;

                string _createdBy = TechProcess.CreatedBy;
                string _name = TechProcess.Name;
                string _code = TechProcess.Code;
                string _description = TechProcess.Scale;
                string _result = TechProcess.Result;

                string _organ = TechProcess.Organ;

                string _status = TechProcess.Status;
                string _Accept = TechProcess.Code2;


                string _FromDate = TechProcess.FromDate;

                string _ToDate = TechProcess.FilePath;

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_TechProcess_InsertUpdate(_id, _categoryid, _name, _description, _createdBy, _status, _result, _Accept, _code, _organ, _ToDate, _FromDate);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<TechProcess> GetTopLastestTechProcesss(int top)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "";

            var orderBy = "[Order] DESC, Id DESC";

            return GetTechProcesssDyn(select, where, orderBy);
        }
        public override TechProcess GetTechProcess(int TechProcessId)
        {
            var select = "*";
            var where = "Id = " + TechProcessId;
            var orderBy = string.Empty;

            var results = GetTechProcesssDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<TechProcess> GetTechProcesssDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_TechProcess_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }



        public override IEnumerable<TechProcess> GetAllTechProcesssPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_TechProcess_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
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
        public override int UpdateOrder(int Id, bool upOrder)
        {
            try
            {

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_TechProcess_UpdateSortOrder(Id, upOrder);
                    return 1;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override IEnumerable<TechProcess> GetTechProcesssByFilter(string keyword, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "[Order] DESC, Id DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Name LIKE N'%" + keyword + "%' ";
                where += "OR Code LIKE N'%" + keyword + "%' )";

            }


            return GetAllTechProcesssPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }


        public override int DeleteTechProcessDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_TechProcess_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }


        public override int DeleteTechProcess(int TechProcessId) { var where = "Id =" + TechProcessId; return DeleteTechProcessDyn(where); }
        public override int DeleteTechProcesss(string lstTechProcessIds) { var where = "Id IN (" + lstTechProcessIds + ")"; return DeleteTechProcessDyn(where); }


    }
}
