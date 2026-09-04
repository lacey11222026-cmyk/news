using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class FeedbackDBBase : ShopOnlineDBBase
    {
        public static FeedbackDBBase Create()
        {
            return (FeedbackDBBase)Activator.CreateInstance(typeof(FeedbackDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateFeedback(Feedback Feedback);

        #endregion

        #region READ STATEMENTs

        public abstract Feedback GetFeedback(int FeedbackId);
        public abstract IEnumerable<Feedback> GetFeedbacksDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Feedback> GetAllFeedbacksPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Feedback> GetAllFeedbacksPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Feedback> GetAllFeedbacks( int status);
       


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteFeedbackDyn(string where);
        public abstract int DeleteFeedback(int FeedbackId);
        public abstract int DeleteFeedbacks(string lstFeedbackIds);

        #endregion

    }
}
