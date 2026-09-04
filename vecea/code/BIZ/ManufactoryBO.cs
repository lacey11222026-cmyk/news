using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class ManufactoryBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_MANUFACTORY;
        protected delegate void DelegateFlushAllManufactoryCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE UPDATE

        public int CreateUpdateManufactory(Manufactory manuFactory)
        {
            return ManufactoryDBBase.Create().CreateUpdateManufactory(manuFactory);
        }

        public int CreateUpdateManufactory(MANUFACTORY_FULL manuFactoryFull)
        {
            Manufactory manuFactory = manuFactoryFull.ConvertToBase();
            int returnValue = CreateUpdateManufactory(manuFactory);
            if (returnValue != -1)
            {
                UpdateCache(manuFactoryFull);
                FlushAllManufactoryCache(string.Empty);
            }

            return returnValue;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get manuFactory by manuFactory id
        /// </summary>
        /// <param name="manuFactoryId">The manuFactory id.</param>
        /// <returns></returns>
        public Manufactory GetManufactory(int manuFactoryId)
        {
            return ManufactoryDBBase.Create().GetManufactory(manuFactoryId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get manuFactory by id => add to local cache
        /// </summary>
        /// <param name="manuFactoryId">The manuFactory id.</param>
        /// <returns></returns>
        public MANUFACTORY_FULL GetManufactoryFull(int manuFactoryId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_MANUFACTORY + manuFactoryId;

                var item = (MANUFACTORY_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = GetManufactory(manuFactoryId);

                item = new MANUFACTORY_FULL
                {
                    Id = itemBase.Id,
                    Title = itemBase.Title,
                    Description = itemBase.Description,
                    Image = itemBase.Image,
                    Website = itemBase.Website,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,
                    Params = itemBase.Params,
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

        public string GetManufactoryFull_JSON(int manuFactoryId)
        {
            var manuFactoryFull = GetManufactoryFull(manuFactoryId);

            if (manuFactoryFull == null)
                return null;

            return UTILS.Utils.ConvertToJson(GetManufactoryFull(manuFactoryId), string.Empty);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of attributes have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllManufactoriesPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALL_MANUFACTORIES_PAGED_JSON + pageIndex + "_" + pageSize;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<MANUFACTORY_FULL> manuFactorys = GetAllManufactoryFullPaged(pageIndex, pageSize, ref totalRecords);

            if (manuFactorys == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(manuFactorys, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return json;
        }

        private List<Manufactory> GetAllManufactories()
        {
            var listManufactory = ManufactoryDBBase.Create().GetAllManufactories(string.Empty);
            if (listManufactory == null)
                return null;
            return listManufactory.ToList();
        }

        public List<MANUFACTORY_FULL> GetAllManufactoryFulls()
        {
            string keyCache = Constants.CACHE_KEY_ALL_MANUFACTORIES;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstItems = (List<MANUFACTORY_FULL>)LocalCaching.GetData(keyCache);
            if (lstItems != null && lstItems.Count > 0)
                return lstItems;
   
            var lstItemBase = GetAllManufactories(); ;

            if (lstItemBase == null)
                return null;
            lstItems = new List<MANUFACTORY_FULL>();

            foreach (var itemBase in lstItemBase)
            {
                MANUFACTORY_FULL item = new MANUFACTORY_FULL()
                {
                    Id = itemBase.Id,
                    Title = itemBase.Title,
                    Description = itemBase.Description,
                    Image = itemBase.Image,
                    Website = itemBase.Website,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,
                    Params = itemBase.Params,
                };

                lstItems.Add(item);
            }

            if (lstItems.Count > 0)
            {
                LocalCaching.Add(keyCache, lstItems);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstItems;
        }


        private List<Manufactory> GetAllManufactoriesPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var listManufactory = ManufactoryDBBase.Create().GetAllManufactoriesPaged(pageIndex, pageSize, ref totalRecords, null);
            if (listManufactory == null)
                return null;
            return listManufactory.ToList();
        }

        private List<Manufactory> GetAllManufactoriesPaged(int pageIndex, int pageSize, ref int totalRecords, short published)
        {
            var listManufactory = ManufactoryDBBase.Create().GetAllManufactoriesPaged(pageIndex, pageSize, ref totalRecords, published);
            if (listManufactory == null)
                return null;
            return listManufactory.ToList();
        }


        public List<MANUFACTORY_FULL> GetAllManufactoryFullPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                var lstItemBase = GetAllManufactoriesPaged(pageIndex, pageSize, ref totalRecords);
                if (lstItemBase == null)
                    return null;

                List<MANUFACTORY_FULL> lstItem = new List<MANUFACTORY_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new MANUFACTORY_FULL()
                    {

                        Id = itemBase.Id,
                        Title = itemBase.Title,
                        Description = itemBase.Description,
                        Image = itemBase.Image,
                        Website = itemBase.Website,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Params = itemBase.Params,

                    };

                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public List<MANUFACTORY_FULL> GetAllManufactoryFullPaged(int pageIndex, int pageSize, ref int totalRecords, short published)
        {
            try
            {
                var lstItemBase = GetAllManufactoriesPaged(pageIndex, pageSize, ref totalRecords, published);
                if (lstItemBase == null)
                    return null;

                List<MANUFACTORY_FULL> lstItem = new List<MANUFACTORY_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new MANUFACTORY_FULL()
                    {

                        Id = itemBase.Id,
                        Title = itemBase.Title,
                        Description = itemBase.Description,
                        Image = itemBase.Image,
                        Website = itemBase.Website,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Params = itemBase.Params,
                    };

                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }


        public List<Manufactory> FilterManufactories(string title)
        {
            var manuFactorys = ManufactoryDBBase.Create().GetAllManufactories(title);
            if (manuFactorys == null)
                return null;
            return manuFactorys.ToList();
        }

        public List<MANUFACTORY_FULL> FilterManufactoryFulls(string title)
        {
            var manuFactorys = FilterManufactories(title);
            if (manuFactorys == null)
                return null;
            List<MANUFACTORY_FULL> lstManufactoryFulls = new List<MANUFACTORY_FULL>();
            foreach (var manuFactory in manuFactorys)
            {
                MANUFACTORY_FULL manuFactoryFull = new MANUFACTORY_FULL()
                {

                    Id = manuFactory.Id,
                    Title = manuFactory.Title,
                    Description = manuFactory.Description,
                    Image = manuFactory.Image,
                    Website = manuFactory.Website,
                    Published = manuFactory.Published,
                    Ordering = manuFactory.Ordering,
                    Params = manuFactory.Params,
                };

                lstManufactoryFulls.Add(manuFactoryFull);
            }

            return lstManufactoryFulls;
        }

        #endregion

        #region DELETE

        public int DeleteManufactory(int manuFactoryId)
        {
            int returnVal = ManufactoryDBBase.Create().DeleteManufactory(manuFactoryId);
            if (returnVal != -1)
                FlushAllManufactoryCache(string.Empty);
            return returnVal;
        }

        public int DeleteManufactories(string listId)
        {
            var returnVal = ManufactoryDBBase.Create().DeleteManufactories(listId);
            if (returnVal != -1)
                FlushAllManufactoryCache(string.Empty);
            return returnVal;
        }

        #endregion

        #region Extend

        public void FlushAllManufactoryCache(string containKey)
        {
            DelegateFlushAllManufactoryCache delegateFlushAllManufactoryCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllManufactoryCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        public void UpdateCache(MANUFACTORY_FULL manuFactoryFull)
        {
            var strKeyCached = Constants.CACHE_KEY_MANUFACTORY + manuFactoryFull.Id;
            //LocalCaching.Add ( strKeyCached, manuFactoryFull );
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, manuFactoryFull, null, null);
        }

        #endregion
    }
}
