using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class SurveyDBBase : ShopOnlineDBBase
    {
        public static SurveyDBBase Create()
        {
            return (SurveyDBBase)Activator.CreateInstance(typeof(SurveyDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateSurvey(Survey Survey);

        #endregion

        #region READ STATEMENTs

        public abstract Survey GetSurvey(int SurveyId);
        public abstract IEnumerable<Survey> GetSurveysDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Survey> GetAllSurveysPaged(int pageIndex, int pageSize, ref int totalRecords, int status);
        public abstract IEnumerable<Survey> GetAllSurveysPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Survey> GetAllSurveys(int top,int status,int category,string title);
        public abstract IEnumerable<Survey> GetSurveyByIds(string ids, int top);


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteSurveyDyn(string where);
        public abstract int DeleteSurvey(int SurveyId);
        public abstract int DeleteSurveys(string lstSurveyIds);

        #endregion

    }
}
