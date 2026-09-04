using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class FeedbackDBSproc : FeedbackDBBase
    {
        public override int CreateUpdateFeedback(Feedback Feedback)
        {
            try
            {
                int? _id = Feedback.Id;
                
                string _Name = Feedback.Name;
                string _Answer = Feedback.Answer;
                string _ResponedUser = Feedback.ResponedUser;
                string _mail = Feedback.Email;
                string _mobile = Feedback.Mobile;
                string _question = Feedback.Question;
                int? _published = Feedback.Status;
               

                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Feedback_InsertUpdate(_id, _Name, _mail, _mobile, _question, _Answer,_ResponedUser, _published);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdateFeedback");
                return -1;
            }
        }

        public override Feedback GetFeedback(int FeedbackId)
        {
            var select = "*";
            var where = "Id = " + FeedbackId;
            var orderBy = string.Empty;

            var results = GetFeedbacksDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Feedback> GetFeedbacksDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Feedback_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllFeedbacksPagedDyn select=" + select + "| where" + where);
                return null;
            }
        }

        public override IEnumerable<Feedback> GetAllFeedbacksPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Id DESC";

            return GetAllFeedbacksPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Feedback> GetAllFeedbacksPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext dc = DataContext)
                {
                    var results = dc.sp_Feedback_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllFeedbacksPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Feedback> GetAllFeedbacks(int status)
        {
            var select = "*";
            var where = string.Empty;
            string orderBy = "Id ASC";


            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            

            return GetFeedbacksDyn(select, where, orderBy);
        }

       

        public override int DeleteFeedbackDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_Feedback_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      
        public override int DeleteFeedback(int FeedbackId) { var where = "Id =" + FeedbackId; return DeleteFeedbackDyn(where); }
        public override int DeleteFeedbacks(string lstFeedbackIds) { var where = "Id IN (" + lstFeedbackIds + ")"; return DeleteFeedbackDyn(where); }

    }
}
