using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class PromotionDBSproc : PromotionDBBase
    {
        #region Overrides of PromotionDBBase

        public override int CreateUpdatePromotion(Promotion promotion)
        {
            try
            {
                int _id = promotion.Id;
                int? _categoryid = promotion.CategoryId;
                string _promotioncode = promotion.PromotionCode;
                string _introtext = promotion.IntroText;
                string _fulltext = promotion.FullText;
                System.DateTime? _startdate = promotion.StartDate;
                System.DateTime? _enddate = promotion.EndDate;
                byte? _bonustype = promotion.BonusType;
                int? _bonusvalue = promotion.BonusValue;
                byte? _published = promotion.Published;
                string productId = promotion.ProductId;

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Promotion_InsertUpdate(_id, _categoryid, _promotioncode, _introtext, _fulltext, _startdate, _enddate, _bonustype, _bonusvalue, _published, productId);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return -1;
            }
        }

        public override Promotion GetPromotion(int promotionId)
        {
            var select = "*";
            var where = "Id = " + promotionId;
            var orderBy = string.Empty;

            var results = GetPromotionsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Promotion> GetPromotionsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Promotion_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return null;
            }
        }

        public override IEnumerable<Promotion> GetAllPromotionsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Id DESC";

            return GetAllPromotionsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Promotion> GetAllPromotionsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_Promotion_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
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

        public override IEnumerable<Promotion> GetAllPromotions(string promotionCode, int categoryId,int published)
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(promotionCode))
                where += "PromotionCode LIKE N'%" + promotionCode + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryId =" + categoryId;
            }

            if (published > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Published =" + published;
            }

            return GetPromotionsDyn(select, where, orderBy);
        }

        public override IEnumerable<Promotion> GetAllPromotions(byte published)
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id DESC";

            if (published >= 0)
            {
                where += " Published =" + published;
            }

            return GetPromotionsDyn(select, where, orderBy);
        }

        public override int DeletePromotionDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Promotion_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp); 
                return -1;
            }
        }

        public override int DeletePromotion(int promotionId) { var where = "Id =" + promotionId; return DeletePromotionDyn(where); }
        public override int DeletePromotions(string lstPromotionIds) { var where = "Id IN (" + lstPromotionIds + ")"; return DeletePromotionDyn(where); }


        #endregion
    }
}
