using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using CarSize = DATA.CarSize;
namespace BIZ
{
    public class CarSizeBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CARSIZE;

        protected delegate void DelegateFlushAllCarSizeCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);



        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get CarSize by CarSize id
        /// </summary>
        /// <param name="CarSizeId">The CarSize id.</param>
        /// <returns></returns>
        //public CarSize GetCarSize(int CarSizeId)
        //{
        //    return CarSizeDBBase.Create().GetCarSize(CarSizeId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get CarSize by id => add to local cache
        /// </summary>
        /// <param name="CarSizeId">The CarSize id.</param>
        /// <returns></returns>

        public int UpdateDynamic(string update, string where)
        {

            int returnVal = CarSizeDBBase.Create().UpdateCarSizeDyn(update, where);
            if (returnVal != -1)
            {
                //UpdateCache(CarSize);
                FlushAllCarSizeCache(string.Empty);
            }
            return returnVal;
        }
        public CarSize Get(int id)
        {

            return CarSizeDBBase.Create().Get(id);
        }
        public List<CarSize> GetTopLastestCarSize(int groupid, int size, int status = 1)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_CARSIZE + status + groupid + size;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<CarSize>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            lstItem = CarSizeDBBase.Create().GetTopCarSizes(groupid, size, status).ToList();
            if (lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, lstItem);
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }









        #endregion

        #region UPDATE

        public void UpdateCache(CarSize CarSize)
        {
            var strKeyCached = Constants.CACHE_KEY_ALBUM + CarSize.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, CarSize, null, null);

        }

        #endregion

        #region DELETE




        #endregion

        public void FlushAllCarSizeCache(string containKey)
        {
            DelegateFlushAllCarSizeCache delegateFlushAllCarSizeCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCarSizeCache.BeginInvoke(strGroupKeyCached, containKey, null, null);


        }

    }
}
