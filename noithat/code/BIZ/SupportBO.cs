using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class SupportBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_SUPPORT;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE

        public int CreateUpdateSupport(Support support)
        {
            return SupportDBBase.Create().CreateUpdateSupport(support);
        }

        public int CreateUpdateSupport(SUPPORT_FULL supportFull)
        {
            Support support = supportFull.ConvertToBase();
            int returnVal = CreateUpdateSupport(support);
            if (returnVal != -1)
            {
                UpdateCache(supportFull);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get support by support id
        /// </summary>
        /// <param name="supportId">The support id.</param>
        /// <returns></returns>
        public Support GetSupport(int supportId)
        {
            return SupportDBBase.Create().GetSupport(supportId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get support by id => add to local cache
        /// </summary>
        /// <param name="supportId">The support id.</param>
        /// <returns></returns>
        public SUPPORT_FULL GetSupportFull(int supportId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_SUPPORT + supportId;

                var item = (SUPPORT_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var support = GetSupport(supportId);

                item = new SUPPORT_FULL
                {


                    Id = support.Id,
                    CategoryId = support.CategoryId,
                    Supporter = support.Supporter,
                    Yahoo = support.Yahoo,
                    Skype = support.Skype,
                    Mail = support.Mail,
                    Phone = support.Phone,
                    Mobile = support.Mobile,
                    Published = support.Published,
                    Ordering = support.Ordering,
                    Params = support.Params,
                };

                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e,"GetSupportFull");
                return null;
            }
        }

        public List<Support> GetAllSupportsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var supports = SupportDBBase.Create().GetAllSupportsPaged(pageIndex, pageSize, ref totalRecords);
            if (supports == null)
                return null;

            return supports.ToList();
        }

        public List<SUPPORT_FULL> GetAllSupportFullsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var supports = GetAllSupportsPaged(pageIndex, pageSize, ref totalRecords);
            List<SUPPORT_FULL> supportFulls = new List<SUPPORT_FULL>();
            foreach (var support in supports)
            {
                SUPPORT_FULL supportFull = new SUPPORT_FULL()
                {
                    Id = support.Id,
                    CategoryId = support.CategoryId,
                    Supporter = support.Supporter,
                    Yahoo = support.Yahoo,
                    Skype = support.Skype,
                    Mail = support.Mail,
                    Phone = support.Phone,
                    Mobile = support.Mobile,
                    Published = support.Published,
                    Ordering = support.Ordering,
                    Params = support.Params,
                };

                supportFulls.Add(supportFull);
            }

            return supportFulls;

        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of supports have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllSupportsPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALL_SUPPORTS_PAGED_JSON + pageIndex +"_"+ pageSize;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_SUPPORT;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<SUPPORT_FULL> supports = GetAllSupportFullsPaged(pageIndex, pageSize, ref totalRecords);

            if (supports == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(supports, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return json;
        }

        public List<Support> FilterSupports(string title, int categoryId)
        {
            var supports = SupportDBBase.Create().GetAllSupports(title, categoryId);
            if (supports == null)
                return null;
            return supports.ToList();
        }

        public List<SUPPORT_FULL> FilterSupportFulls(string title, int categoryId)
        {
            var supports = FilterSupports(title, categoryId);
            if (supports == null)
                return null;
            List<SUPPORT_FULL> lstSupportFulls = new List<SUPPORT_FULL>();
            foreach (var support in supports)
            {
                SUPPORT_FULL supportFull = new SUPPORT_FULL()
                {

                    Id = support.Id,
                    CategoryId = support.CategoryId,
                    Supporter = support.Supporter,
                    Yahoo = support.Yahoo,
                    Skype = support.Skype,
                    Mail = support.Mail,
                    Phone = support.Phone,
                    Mobile = support.Mobile,
                    Published = support.Published,
                    Ordering = support.Ordering,
                    Params = support.Params,
                };

                lstSupportFulls.Add(supportFull);
            }

            return lstSupportFulls;
        }

        public List<SUPPORT_FULL> GetSupportsByCategory(int categoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_SUPPORTS_BYCATEGORY + categoryId;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_SUPPORT;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstCachedSupports = (List<SUPPORT_FULL>)LocalCaching.GetData(keyCache);

            if (lstCachedSupports != null)
                return lstCachedSupports;

            var supports = FilterSupports(string.Empty, categoryId);

            if (supports == null)
                return null;

            var publishedSupports = (from p in supports where p.Published == 1 orderby p.Ordering ascending select p).ToList();

            lstCachedSupports = new List<SUPPORT_FULL>();
            foreach (var support in publishedSupports)
            {
                SUPPORT_FULL supportFull = new SUPPORT_FULL()
                {

                    Id = support.Id,
                    CategoryId = support.CategoryId,
                    Supporter = support.Supporter,
                    Yahoo = support.Yahoo,
                    Skype = support.Skype,
                    Mail = support.Mail,
                    Phone = support.Phone,
                    Mobile = support.Mobile,
                    Published = support.Published,
                    Ordering = support.Ordering,
                    Params = support.Params,
                };

                lstCachedSupports.Add(supportFull);
            }

            if (lstCachedSupports.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedSupports);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstCachedSupports;
        }

        public List<SUPPORT_FULL> GetTopSupports(int top, bool isRandom)
        {
            Random rand = new Random();
            string keyCache = Constants.CACHE_KEY_TOP_SUPPORTS + top;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_SUPPORT;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstCachedSupports = (List<SUPPORT_FULL>)LocalCaching.GetData(keyCache);

            if (lstCachedSupports != null)
            {
                if (isRandom)
                {
                    rand = new Random();
                    var _lstCachedSupports = ( from p in lstCachedSupports orderby rand.Next() select p ).ToList();
                    return _lstCachedSupports;
                }

                return lstCachedSupports;
            }

            var supports = SupportDBBase.Create().GetTopSupports(top, 1);

            if (supports == null)
                return null;

            var publishedSupports = (from p in supports where p.Published == 1 orderby p.Ordering ascending select p).ToList();

            lstCachedSupports = new List<SUPPORT_FULL>();
            foreach (var support in publishedSupports)
            {
                SUPPORT_FULL supportFull = new SUPPORT_FULL()
                {

                    Id = support.Id,
                    CategoryId = support.CategoryId,
                    Supporter = support.Supporter,
                    Yahoo = support.Yahoo,
                    Skype = support.Skype,
                    Mail = support.Mail,
                    Phone = support.Phone,
                    Mobile = support.Mobile,
                    Published = support.Published,
                    Ordering = support.Ordering,
                    Params = support.Params,
                };

                lstCachedSupports.Add(supportFull);
            }

            if (lstCachedSupports.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedSupports);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

                if (isRandom)
                {
                    rand = new Random();
                    var _lstCachedSupports = (from p in lstCachedSupports orderby rand.Next() select p).ToList();
                    return _lstCachedSupports;
                }
            }


            return lstCachedSupports;
        }


        #endregion

        #region UPDATE

        public void UpdateCache(SUPPORT_FULL supportFull)
        {
            var strKeyCached = Constants.CACHE_KEY_SUPPORT + supportFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, supportFull, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteSupports(string listIds)
        {

            var returnVal = SupportDBBase.Create().DeleteSupports(listIds);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteSupport(int id)
        {
            var returnVal = SupportDBBase.Create().DeleteSupport(id);
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
