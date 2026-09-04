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
                    CategoryId = itemBase.CategoryId,
                    Params = itemBase.Params,
                };

                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ManufactoryBO", "GetManufactoryFull: manuFactoryId" + manuFactoryId);
                return null;
            }
        }

       

        private List<Manufactory> GetAllManufactories(int CategoryId,int status)
        {
            var listManufactory = ManufactoryDBBase.Create().GetAllManufactories(CategoryId,-1);
            if (listManufactory == null)
                return null;
            return listManufactory.ToList();
        }

        public List<MANUFACTORY_FULL> GetAllManufactoryFulls(int CategoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_MANUFACTORIES+"_cate"+ CategoryId;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstItems = (List<MANUFACTORY_FULL>)LocalCaching.GetData(keyCache);
            if (lstItems != null && lstItems.Count > 0)
                return lstItems;
   
            var lstItemBase = GetAllManufactories(CategoryId,-1); 

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
                    CategoryId= itemBase.CategoryId,
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


       
        private List<Manufactory> GetAllManufactoriesPaged(int CategoryId,int pageIndex, int pageSize, ref int totalRecords, int published)
        {
            var listManufactory = ManufactoryDBBase.Create().GetAllManufactoriesPaged(CategoryId,pageIndex, pageSize, ref totalRecords, published);
            if (listManufactory == null)
                return null;
            return listManufactory.ToList();
        }
        public int UpdateStatus(int ProductId)
        {
            try
            {
                return ManufactoryDBBase.Create().UpdateStatus(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "Manufactory", "UpdateStatus");
                return -1;
            }
        }
        public int UpdateOrder(int ProductId, bool upOrder)
        {
            try
            {
                return ManufactoryDBBase.Create().UpdateOrder(ProductId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "Manufactory", "UpdateOrder");
                return -1;
            }
        }
        public List<MANUFACTORY_FULL> GetAllManufactoryFullPaged(int CategoryId,int pageIndex, int pageSize, int published, ref int totalRecords)
        {
            try
            {
                var lstItemBase = GetAllManufactoriesPaged(CategoryId,pageIndex, pageSize, ref totalRecords, published);
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
                        CategoryId = itemBase.CategoryId,
                    };

                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ManufactoryBO", "GetAllManufactoryFullPaged: pageIndex" + pageIndex);
                return null;
            }
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
