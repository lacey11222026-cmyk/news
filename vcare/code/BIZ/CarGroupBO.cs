using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using CarGroup = DATA.CarGroup;
namespace BIZ
{
    public class CarGroupBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CarGroup;

        protected delegate void DelegateFlushAllCarGroupCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateCarGroup(CarGroup CarGroup)
        {

            int returnVal = CarGroupDBBase.Create().CreateUpdateCarGroup(CarGroup);
            if (returnVal != -1)
            {
                UpdateCache(CarGroup);
                FlushAllCarGroupCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get CarGroup by CarGroup id
        /// </summary>
        /// <param name="CarGroupId">The CarGroup id.</param>
        /// <returns></returns>
        //public CarGroup GetCarGroup(int CarGroupId)
        //{
        //    return CarGroupDBBase.Create().GetCarGroup(CarGroupId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get CarGroup by id => add to local cache
        /// </summary>
        /// <param name="CarGroupId">The CarGroup id.</param>
        /// <returns></returns>
        public CarGroup GetCarGroup(int CarGroupId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_CARGROUP + CarGroupId;

                var item = (CarGroup)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = CarGroupDBBase.Create().GetCarGroup(CarGroupId);

               
                LocalCaching.Add(strKeyCached, itemBase);

                return itemBase;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        
      
        public List<CarGroup> GetTopLastestCarGroup(int status=1)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_CARGROUP + status ;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<CarGroup>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            lstItem = CarGroupDBBase.Create().GetTopCarGroups(status).ToList();
            if (lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, lstItem);
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }

       
      






        #endregion

        #region UPDATE

        public void UpdateCache(CarGroup CarGroup)
        {
            var strKeyCached = Constants.CACHE_KEY_CARGROUP + CarGroup.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, CarGroup, null, null);

        }

        #endregion

        #region DELETE

      

        public int DeleteCarGroup(int id)
        {
            var returnVal = CarGroupDBBase.Create().DeleteCarGroup(id);
            if (returnVal != -1)
                FlushAllCarGroupCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllCarGroupCache(string containKey)
        {
            DelegateFlushAllCarGroupCache delegateFlushAllCarGroupCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCarGroupCache.BeginInvoke(strGroupKeyCached, containKey, null, null);


        }

    }
}
