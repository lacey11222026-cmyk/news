using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class FeedbackBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_FEEDBACK;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE


        public int CreateUpdateFeedback(Feedback Feedback)
        {

            int returnVal = FeedbackDBBase.Create().CreateUpdateFeedback(Feedback);
            if (returnVal != -1)
            {
                UpdateCache(Feedback);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ



        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Feedback by id => add to local cache
        /// </summary>
        /// <param name="FeedbackId">The Feedback id.</param>
        /// <returns></returns>
        public Feedback GetFeedback(int FeedbackId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_FEEDBACK + FeedbackId;

                var item = (Feedback)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var Feedback = FeedbackDBBase.Create().GetFeedback(FeedbackId);



                LocalCaching.Add(strKeyCached, Feedback);

                return Feedback;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "GetFeedback");
                return null;
            }
        }

        public List<Feedback> GetAllFeedbacksPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var Feedbacks = FeedbackDBBase.Create().GetAllFeedbacksPaged(pageIndex, pageSize, ref totalRecords);
            if (Feedbacks == null)
                return null;

            return Feedbacks.ToList();
        }

        

        public List<Feedback> GetFeedbacksByCategory( int status)
        {

            return FeedbackDBBase.Create().GetAllFeedbacks(status).ToList(); 
           
        }




        #endregion

        #region UPDATE

        public void UpdateCache(Feedback Feedback)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_FEEDBACK + Feedback.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, Feedback, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteFeedbacks(string listIds)
        {

            var returnVal = FeedbackDBBase.Create().DeleteFeedbacks(listIds);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteFeedback(int id)
        {
            var returnVal = FeedbackDBBase.Create().DeleteFeedback(id);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public void FlushAllCache(string containKey)
        {
            DelegateFlushAllCache delegateFlushAllCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        #endregion
    }
}
