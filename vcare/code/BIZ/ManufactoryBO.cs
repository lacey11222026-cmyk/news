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
            FlushAllManufactoryCache(string.Empty);

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
                    ParentId = itemBase.ParentId,
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

       

        private List<Manufactory> GetAllManufactories(int ParentId,int CategoryId,int status)
        {
            var listManufactory = ManufactoryDBBase.Create().GetAllManufactories(ParentId, CategoryId, status);
            if (listManufactory == null)
                return null;
            return listManufactory.ToList();
        }

        public List<MANUFACTORY_FULL> GetAllManufactoryFulls(int ParentId,int CategoryId, int status)
        {
            string keyCache = Constants.CACHE_KEY_ALL_MANUFACTORIES+"_cate"+ CategoryId +"_cate"+ParentId+status;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstItems = (List<MANUFACTORY_FULL>)LocalCaching.GetData(keyCache);
            if (lstItems != null && lstItems.Count > 0)
                return lstItems;
   
            var lstItemBase = GetAllManufactories(ParentId,CategoryId, status); 

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
                    ParentId = itemBase.ParentId,
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


       
      
        public int UpdateStatus(int ProductId)
        {
            try
            {
                ManufactoryDBBase.Create().UpdateStatus(ProductId);
                FlushAllManufactoryCache(string.Empty);
                return 1;
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
