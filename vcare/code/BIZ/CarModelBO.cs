using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using CarModel = DATA.CarModel;
namespace BIZ
{
    public class CarModelBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CarModel;

        protected delegate void DelegateFlushAllCarModelCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateCarModel(CarModel CarModel)
        {

            int returnVal = CarModelDBBase.Create().CreateUpdateCarModel(CarModel);
            if (returnVal != -1)
            {
                UpdateCache(CarModel);
                FlushAllCarModelCache(string.Empty);
            }
            return returnVal;
        }
        public int UpdateDynamic(string update,string where)
        {

            int returnVal = CarModelDBBase.Create().UpdateCarModelDyn(update,where);
            if (returnVal != -1)
            {
                //UpdateCache(CarModel);
                FlushAllCarModelCache(string.Empty);
            }
            return returnVal;
        }
        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get CarModel by CarModel id
        /// </summary>
        /// <param name="CarModelId">The CarModel id.</param>
        /// <returns></returns>
        //public CarModel GetCarModel(int CarModelId)
        //{
        //    return CarModelDBBase.Create().GetCarModel(CarModelId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get CarModel by id => add to local cache
        /// </summary>
        /// <param name="CarModelId">The CarModel id.</param>
        /// <returns></returns>
        public CarModel GetCarModel(int CarModelId)
        {
            try
            {
                //var strKeyCached = Constants.CACHE_KEY_CARMODEL + CarModelId;

                //var item = (CarModel)LocalCaching.GetData(strKeyCached);
                //if (item != null)
                //    return item;

                var itemBase = CarModelDBBase.Create().GetCarModel(CarModelId);

               
                //LocalCaching.Add(strKeyCached, itemBase);

                return itemBase;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public CarModel GetByUrl(string url)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_CARMODEL + url;

                var item = (CarModel)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = CarModelDBBase.Create().GetByUrl(url);


                LocalCaching.Add(strKeyCached, itemBase);

                return itemBase;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public List<CarModel> GetTopLastestCarModel(int groupid=0,int status=1)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_CARMODEL + status+ groupid;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<CarModel>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            lstItem = CarModelDBBase.Create().GetTopCarModels(groupid, status).ToList();
            if (lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, lstItem);
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }

       
      






        #endregion

        #region UPDATE

        public void UpdateCache(CarModel CarModel)
        {
            var strKeyCached = Constants.CACHE_KEY_ALBUM + CarModel.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, CarModel, null, null);

        }

        #endregion

        #region DELETE

      

        public int DeleteCarModel(int id)
        {
            var returnVal = CarModelDBBase.Create().DeleteCarModel(id);
            if (returnVal != -1)
                FlushAllCarModelCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllCarModelCache(string containKey)
        {
            DelegateFlushAllCarModelCache delegateFlushAllCarModelCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCarModelCache.BeginInvoke(strGroupKeyCached, containKey, null, null);


        }

    }
}
