using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Banner = DATA.Banner;
using Newtonsoft.Json;
namespace BIZ
{
    public class BannerBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_BANNER;

        protected delegate void DelegateFlushAllBannerCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateBanner(Banner banner)
        {

            int returnVal = BannerDBBase.Create().CreateUpdateBanner(banner);
            if (returnVal != -1)
            {
                //UpdateCache(banner);
                FlushAllBannerCache(strGroupKeyCached);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Banner by Banner id
        /// </summary>
        /// <param name="BannerId">The Banner id.</param>
        /// <returns></returns>
        //public Banner GetBanner(int BannerId)
        //{
        //    return BannerDBBase.Create().GetBanner(BannerId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Banner by id => add to local cache
        /// </summary>
        /// <param name="BannerId">The Banner id.</param>
        /// <returns></returns>
        public Banner GetBanner(int BannerId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_Banner + BannerId;
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<Banner>(cachedata.ToString());

                var item = BannerDBBase.Create().GetBanner(BannerId);

                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));
                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "BannerBO", "GetBanner");
                return null;
            }
        }
      
        public List<Banner> GetTopLastestBanners(int top, int region = -1,int status=-1,int site=0,int categoryId=0)
        {
            var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_Banners + top + "_region" + region + "_region" + region + "_status" + status + "_site" + site + "_categoryId" + categoryId;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<List<Banner>>(data.ToString());

            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<Banner>>(cachedata.ToString());

            var lstItemBase = BannerDBBase.Create().GetTopLastestBanners(top, region, status, site,categoryId).ToList();
            if (lstItemBase.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
            }

            return lstItemBase;
        }
     
       








        #endregion

        #region UPDATE

        //public void UpdateCache(Banner banner)
        //{
        //    var strKeyCached = Constants.CACHE_KEY_Banner + banner.Id;
        //    DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
        //    delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(banner), null, null);

        //}

        #endregion

        #region DELETE

        public int DeleteBanners(string listIds)
        {
            var returnVal = BannerDBBase.Create().DeleteBanners(listIds);
            if (returnVal != -1)
                FlushAllBannerCache(strGroupKeyCached);
            return returnVal;
        }

        public int DeleteBanner(int id)
        {
            var returnVal = BannerDBBase.Create().DeleteBanner(id);
            if (returnVal != -1)
                FlushAllBannerCache(strGroupKeyCached);
            return returnVal;
        }

        #endregion

        public void FlushAllBannerCache(string containKey)
        {
            // DelegateFlushAllBannerCache delegateFlushAllBannerCache = LocalCaching.RemoveContainKeyInGroupKey;
            //delegateFlushAllBannerCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            RedisCaching.RemoveGroup(containKey);

        }

    }
}
