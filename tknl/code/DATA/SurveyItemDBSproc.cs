using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class SurveyItemDBSproc : SurveyItemDBBase
    {
        public override int CreateUpdateSurveyItem(SurveyItem SurveyItem)
        {
            try
            {
                int? _id = SurveyItem.Id;
                int? _surveyId = SurveyItem.SurveyId;
                string _Content = SurveyItem.Content;

                int _count = SurveyItem.Count;
                int? _status = SurveyItem.Status;
                int? _order = SurveyItem.Order;

                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_SurveyItem_InsertUpdate(_id, _Content, _surveyId, _count, _status, _order);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdateSurveyItem");
                return -1;
            }
        }

        public override SurveyItem GetSurveyItem(int SurveyItemId)
        {
            var select = "*";
            var where = "Id = " + SurveyItemId;
            var orderBy = string.Empty;

            var results = GetSurveyItemsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<SurveyItem> GetSurveyItemsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_SurveyItem_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetSurveyItemsDyn select=" + select + "| where" + where);
                return null;
            }
        }


        public override IEnumerable<SurveyItem> GetBySurveyId(int surveyId, int status)
        {
            var select = "*";
            var where = string.Empty;
            string orderBy = " [Order] ASC";


            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
            if (surveyId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " SurveyId =" + surveyId;
            }

            return GetSurveyItemsDyn(select, where, orderBy);
        }


        public override int UpdateDyn(string update, string where)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_SurveyItem_UpdateDynamic(update, where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int UpdateStatus(int Id, int status)
        {
            var update = " Set Status= " + status;
            var where = "Id =" + Id;
            return UpdateDyn(update, where);

        }
        public override int CountAdd(int Id)
        {
            var update = " Set Count= Count+1 ";
            var where = "Id =" + Id;
            return UpdateDyn(update, where);

        }
        public override int DeleteSurveyItemDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_SurveyItem_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override int DeleteSurveyItem(int surveyItemId) { var where = "Id =" + surveyItemId; return DeleteSurveyItemDyn(where); }
        public override int DeleteSurveyItems(string lstSurveyItemIds) { var where = "Id IN (" + lstSurveyItemIds + ")"; return DeleteSurveyItemDyn(where); }

    }
}
