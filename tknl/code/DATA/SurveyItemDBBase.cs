using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class SurveyItemDBBase : ShopOnlineDBBase
    {
        public static SurveyItemDBBase Create()
        {
            return (SurveyItemDBBase)Activator.CreateInstance(typeof(SurveyItemDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateSurveyItem(SurveyItem SurveyItem);

        #endregion

        #region READ STATEMENTs

        public abstract SurveyItem GetSurveyItem(int SurveyItemId);
        public abstract IEnumerable<SurveyItem> GetSurveyItemsDyn(string select, string where, string orderBy);

        public abstract IEnumerable<SurveyItem> GetBySurveyId(int surveyId, int status);
       


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteSurveyItemDyn(string where);
        public abstract int DeleteSurveyItem(int surveyItemId);
        public abstract int DeleteSurveyItems(string lstSurveyItemIds);
        public abstract int UpdateDyn(string update, string where);
        public abstract int UpdateStatus(int Id, int status);
        public abstract int CountAdd(int Id);

        #endregion

    }
}
