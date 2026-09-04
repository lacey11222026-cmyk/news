using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class PromotionBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_PROMOTION;
        protected delegate void DelegateFlushAllPromotionCache(string strGroupKeyCached, string containKey);

        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        public int CreateUpdatePromotion(Promotion promotion)
        {
            return PromotionDBBase.Create().CreateUpdatePromotion(promotion);
        }

        public int CreateUpdatePromotion(PROMOTION_FULL promotionFull)
        {
            Promotion promotion = promotionFull.ConvertToBase();
            int returnVal = CreateUpdatePromotion(promotion);
            if (returnVal != -1)
            {
                UpdateCache(promotionFull);
                FlushAllPromotionCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get promotion by promotion id
        /// </summary>
        /// <param name="promotionId">The promotion id.</param>
        /// <returns></returns>
        public Promotion GetPromotion(int promotionId)
        {
            return PromotionDBBase.Create().GetPromotion(promotionId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get promotion by id => add to local cache
        /// </summary>
        /// <param name="promotionId">The promotion id.</param>
        /// <returns></returns>
        public PROMOTION_FULL GetPromotionFull(int promotionId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_PROMOTION + promotionId;

                var item = (PROMOTION_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = GetPromotion(promotionId);

                item = new PROMOTION_FULL
                {
                    Id = itemBase.Id,
                    CategoryId = itemBase.CategoryId,
                    PromotionCode = itemBase.PromotionCode,
                    IntroText = itemBase.IntroText,
                    FullText = itemBase.FullText,
                    StartDate = itemBase.StartDate,
                    EndDate = itemBase.EndDate,
                    BonusType = itemBase.BonusType,
                    BonusValue = itemBase.BonusValue,
                    Published = itemBase.Published,
                    ProductId = itemBase.ProductId
                };

                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public List<Promotion> GetAllPromotionsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var promotions = PromotionDBBase.Create().GetAllPromotionsPaged(pageIndex, pageSize, ref totalRecords);
            if (promotions == null)
                return null;

            return promotions.ToList();
        }

        public List<PROMOTION_FULL> GetAllPromotionFullsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var promotions = GetAllPromotionsPaged(pageIndex, pageSize, ref totalRecords);
            List<PROMOTION_FULL> promotionFulls = new List<PROMOTION_FULL>();
            foreach (var promotion in promotions)
            {
                PROMOTION_FULL promotionFull = new PROMOTION_FULL()
                {

                    Id = promotion.Id,
                    CategoryId = promotion.CategoryId,
                    PromotionCode = promotion.PromotionCode,
                    IntroText = promotion.IntroText,
                    FullText = promotion.FullText,
                    StartDate = promotion.StartDate,
                    EndDate = promotion.EndDate,
                    BonusType = promotion.BonusType,
                    BonusValue = promotion.BonusValue,
                    Published = promotion.Published,
                    ProductId = promotion.ProductId
                };

                promotionFulls.Add(promotionFull);
            }

            return promotionFulls;

        }

        public List<Promotion> GetAllPublishedPromotions()
        {
            var promotions = PromotionDBBase.Create().GetAllPromotions(1);
            if (promotions == null)
                return null;

            return promotions.ToList();
        }

        public List<PROMOTION_FULL> GetAllPublishedPromotionFulls()
        {
            try
            {
                string keyCache = Constants.CACHE_KEY_ALL_PUBLISHED_PROMOTIONS;
                string groupKeyCache = Constants.CACHE_GROUPKEY_PROMOTION;

                var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
                if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                    LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

                var lstItems = (List<PROMOTION_FULL>)LocalCaching.GetData(keyCache);
                if (lstItems != null)
                    return lstItems;

                var lstItemBase = GetAllPublishedPromotions();

                if (lstItemBase == null)
                    return null;
                lstItems = new List<PROMOTION_FULL>();
                foreach (var promotion in lstItemBase)
                {
                    PROMOTION_FULL promotionFull = new PROMOTION_FULL()
                    {
                        Id = promotion.Id,
                        CategoryId = promotion.CategoryId,
                        PromotionCode = promotion.PromotionCode,
                        IntroText = promotion.IntroText,
                        FullText = promotion.FullText,
                        StartDate = promotion.StartDate,
                        EndDate = promotion.EndDate,
                        BonusType = promotion.BonusType,
                        BonusValue = promotion.BonusValue,
                        Published = promotion.Published,
                        ProductId = promotion.ProductId
                    };

                    lstItems.Add(promotionFull);
                }

                if (lstItems.Count > 0)
                {
                    LocalCaching.Add(keyCache, lstItems);
                    LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
                }

                return lstItems;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }



        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of promotions have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllPromotionsPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PROMOTIONS_PAGED_JSON + pageIndex + pageSize;
            string groupKeyCache = Constants.CACHE_GROUPKEY_PROMOTION;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<PROMOTION_FULL> promotions = GetAllPromotionFullsPaged(pageIndex, pageSize, ref totalRecords);

            if (promotions == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(promotions, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
            }

            return json;
        }

        public List<Promotion> FilterPromotions(string title, int categoryId)
        {
            var promotions = PromotionDBBase.Create().GetAllPromotions(title, categoryId, -1);
            if (promotions == null)
                return null;
            return promotions.ToList();
        }

        public List<PROMOTION_FULL> FilterPromotionFulls(string title, int categoryId)
        {
            var promotions = FilterPromotions(title, categoryId);
            if (promotions == null)
                return null;
            List<PROMOTION_FULL> lstPromotionFulls = new List<PROMOTION_FULL>();
            foreach (var promotion in promotions)
            {
                PROMOTION_FULL promotionFull = new PROMOTION_FULL()
                {
                    Id = promotion.Id,
                    CategoryId = promotion.CategoryId,
                    PromotionCode = promotion.PromotionCode,
                    IntroText = promotion.IntroText,
                    FullText = promotion.FullText,
                    StartDate = promotion.StartDate,
                    EndDate = promotion.EndDate,
                    BonusType = promotion.BonusType,
                    BonusValue = promotion.BonusValue,
                    Published = promotion.Published,
                    ProductId = promotion.ProductId
                };

                lstPromotionFulls.Add(promotionFull);
            }

            return lstPromotionFulls;
        }

        public List<PROMOTION_FULL> GetPromotionsByCategory(int categoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PROMOTIONS_BYCATEGORY + categoryId;
            string groupKeyCache = Constants.CACHE_GROUPKEY_PROMOTION;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var lstCachedPromotions = (List<PROMOTION_FULL>)LocalCaching.GetData(keyCache);

            if (lstCachedPromotions != null)
                return lstCachedPromotions;

            var promotions = FilterPromotions(string.Empty, categoryId);

            if (promotions == null)
                return null;

            var publishedPromotions = (from p in promotions where p.Published == 1 select p).ToList();

            lstCachedPromotions = new List<PROMOTION_FULL>();
            foreach (var promotion in publishedPromotions)
            {
                PROMOTION_FULL promotionFull = new PROMOTION_FULL()
                {
                    Id = promotion.Id,
                    CategoryId = promotion.CategoryId,
                    PromotionCode = promotion.PromotionCode,
                    IntroText = promotion.IntroText,
                    FullText = promotion.FullText,
                    StartDate = promotion.StartDate,
                    EndDate = promotion.EndDate,
                    BonusType = promotion.BonusType,
                    BonusValue = promotion.BonusValue,
                    Published = promotion.Published,
                };

                lstCachedPromotions.Add(promotionFull);
            }

            if (lstCachedPromotions.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedPromotions);
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
            }

            return lstCachedPromotions;
        }

        public PROMOTION_FULL GetPromotion(string promotionCode)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PROMOTIONS_BYCATEGORY + promotionCode;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var cachedPromotion = (PROMOTION_FULL)LocalCaching.GetData(keyCache);

            if (cachedPromotion != null)
                return cachedPromotion;

            var promotions = PromotionDBBase.Create().GetAllPromotions(promotionCode, -1, 1);

            if (promotions == null || promotions.Count() == 0)
                return null;

            var promotion = promotions.FirstOrDefault();
            if (promotion == null)
                return null;

            cachedPromotion = new PROMOTION_FULL()
            {
                Id = promotion.Id,
                CategoryId = promotion.CategoryId,
                PromotionCode = promotion.PromotionCode,
                IntroText = promotion.IntroText,
                FullText = promotion.FullText,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                BonusType = promotion.BonusType,
                BonusValue = promotion.BonusValue,
                Published = promotion.Published,
            };

            LocalCaching.Add(keyCache, cachedPromotion);
            LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);


            return cachedPromotion;
        }

        public double CheckPromotion(string promotionCode, int productId, int categoryId, double price)
        {
            double promotionPrice = 0;
            var promotionFull = GetPromotion(promotionCode);
            if (promotionFull == null)
                return promotionPrice;

            if (promotionFull.CategoryId == categoryId || promotionFull.ProductId.IndexOf(productId.ToString()) != -1 || promotionFull.CategoryId == 0)
            {
                switch (promotionFull.BonusType)
                {
                    case (int)UTILS.Constants.BonusType.DiscountPercent:
                        promotionPrice = (Convert.ToDouble(promotionFull.BonusValue) / 100) * price;
                        return promotionPrice;
                }


            }

            return promotionPrice;
        }

        #endregion

        #region UPDATE

        public void UpdateCache(PROMOTION_FULL promotionFull)
        {
            var strKeyCached = Constants.CACHE_KEY_PROMOTION + promotionFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, promotionFull, null, null);

        }

        #endregion

        #region DELETE

        public int DeletePromotions(string listIds)
        {
            var returnVal = PromotionDBBase.Create().DeletePromotions(listIds);
            if (returnVal != -1)
                FlushAllPromotionCache(string.Empty);
            return returnVal;
        }

        public int DeletePromotion(int id)
        {
            var returnVal = PromotionDBBase.Create().DeletePromotion(id);
            if (returnVal != -1)
                FlushAllPromotionCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllPromotionCache(string containKey)
        {
            DelegateFlushAllPromotionCache delegateFlushAllPromotionCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllPromotionCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }
    }
}
