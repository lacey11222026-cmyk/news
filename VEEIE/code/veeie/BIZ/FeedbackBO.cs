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
       

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE


        public int CreateUpdateFeedback(Feedback Feedback)
        {

            int returnVal = FeedbackDBBase.Create().CreateUpdateFeedback(Feedback);
            
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
               

                var Feedback = FeedbackDBBase.Create().GetFeedback(FeedbackId);




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

      

        #endregion

        #region DELETE

        public int DeleteFeedbacks(string listIds)
        {

            var returnVal = FeedbackDBBase.Create().DeleteFeedbacks(listIds);
           
            return returnVal;
        }

        public int DeleteFeedback(int id)
        {
            var returnVal = FeedbackDBBase.Create().DeleteFeedback(id);
           
            return returnVal;
        }

       
        #endregion
    }
}
