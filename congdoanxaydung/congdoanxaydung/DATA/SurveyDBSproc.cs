using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class SurveyDBSproc : SurveyDBBase
    {
        public override int CreateUpdateSurvey(Survey Survey)
        {
            try
            {
                int? _id = Survey.Id;
                string _content = Survey.Content;
                string _title = Survey.Title;
                string _cate = Survey.CategoryPath;
                DateTime _beginDate = Survey.BeginDate;
                DateTime _endDate = Survey.EndDate;
                int? _type = Survey.Type;
                int _status = Survey.Status;
               

                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Survey_InsertUpdate(_id, _title, _content, _cate,_beginDate, _endDate, _status, _type);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdateSurvey");
                return -1;
            }
        }

        public override Survey GetSurvey(int SurveyId)
        {
            var select = "*";
            var where = "Id = " + SurveyId;
            var orderBy = string.Empty;

            var results = GetSurveysDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Survey> GetSurveysDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Survey_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllSurveysPagedDyn select=" + select + "| where" + where);
                return null;
            }
        }

        public override IEnumerable<Survey> GetAllSurveysPaged(int pageIndex, int pageSize, ref int totalRecords,int status)
        {
            string select = "*";
            var where = "";
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            string orderBy = "Id DESC";

            return GetAllSurveysPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override IEnumerable<Survey> GetSurveyByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);

            var select = " *";
            if (top > 0)
                select = "TOP(" + top + ") *";
            var where = "Id IN (" + ids + ") AND Status = 1";
            var orderBy = "Id DESC";

            return GetSurveysDyn(select, where, orderBy);
        }
        public override IEnumerable<Survey> GetAllSurveysPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext dc = DataContext)
                {
                    var results = dc.sp_Survey_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllSurveysPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Survey> GetAllSurveys(int top,int status, int category,string title)
        {
            var select = "*";
            if (top > 0)
                select = "TOP(" + top + ") * ";
            var where = string.Empty;
            string orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "[Title] LIKE N'%" + title + "%' ";

            }
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (category >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPath Like '%," + category + ",%' ";
            }

            return GetSurveysDyn(select, where, orderBy);
        }

       

        public override int DeleteSurveyDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Survey_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      
        public override int DeleteSurvey(int SurveyId) { var where = "Id =" + SurveyId; return DeleteSurveyDyn(where); }
        public override int DeleteSurveys(string lstSurveyIds) { var where = "Id IN (" + lstSurveyIds + ")"; return DeleteSurveyDyn(where); }

    }
}
