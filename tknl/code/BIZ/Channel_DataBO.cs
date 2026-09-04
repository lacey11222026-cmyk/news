using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Newtonsoft.Json;

namespace BIZ
{
    public class Channel_DataBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CHANNEL_DATA;
        protected delegate void DelegateFlushAllChannel_DataCache(string strGroupKeyCached, string containKey);

        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        //public int CreateUpdateChannel_Data(Channel_Data Channel_Data)
        //{
        //    return Channel_DataDBBase.Create().CreateUpdateChannel_Data(Channel_Data);
        //}

        public int CreateUpdateChannel_Data(Channel_Data Channel_Data)
        {
            //Channel_Data Channel_Data = Channel_DataFull.ConvertToBase();
            int returnVal = Channel_DataDBBase.Create().CreateUpdateChannel_Data(Channel_Data);
            if (returnVal != -1)
            {
                UpdateCache(Channel_Data);
                FlushAllChannel_DataCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Channel_Data by Channel_Data id
        /// </summary>
        /// <param name="Channel_DataId">The Channel_Data id.</param>
        /// <returns></returns>
        public Channel_Data GetChannel_Data(int Channel_DataId)
        {
            return Channel_DataDBBase.Create().GetById(Channel_DataId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Channel_Data by id => add to local cache
        /// </summary>
        /// <param name="Channel_DataId">The Channel_Data id.</param>
        /// <returns></returns>
        public Channel_Data GetChannel_DataFull(int Channel_DataId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_CHANNEL_DATA + Channel_DataId;

                var data = LocalCaching.GetData(strKeyCached);
                if (data != null)
                    return JsonConvert.DeserializeObject<Channel_Data>(data.ToString());

                var item = GetChannel_Data(Channel_DataId);


                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "GetChannel_DataFull");
                return null;
            }
        }

      
        public List<Channel_Data> GetByContentId(long contentId)
        {
            string strKeyCached = Constants.CACHE_KEY_TOP_BYCONTENT_CHANNEL_DATA + contentId;
            
            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
            {
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
               
            }

            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Channel_Data>>(data.ToString());
            var lstItem = Channel_DataDBBase.Create().GetByContentId(contentId).ToList();
            if (lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
               
            }
            return lstItem.ToList();
        }
        
        public List<Channel_Data> GetAllChannelDataFullsPaged(int channelId,int pageIndex, int pageSize, ref int totalRecords)
        {
            string strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_CHANNEL_DATA + pageIndex + "_pagesize" + pageSize + "_channelId" + channelId;
            string strKeyCachedTotal = Constants.CACHE_KEY_TOP_LASTEST_CHANNEL_DATA + pageIndex + "_pagesize" + pageSize + "_channelId" + channelId + "_total";

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
            {
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            }
            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
            {
                totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
                return JsonConvert.DeserializeObject<List<Channel_Data>>(data.ToString());
            }

            var lstItem = Channel_DataDBBase.Create().GetAllChannel_DatasPaged(channelId,pageIndex, pageSize, ref totalRecords).ToList();
            if (lstItem!=null && lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                LocalCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            }
            return lstItem.ToList();
        }



        #endregion

        #region UPDATE

        public void UpdateCache(Channel_Data channelDataFull)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_CHANNEL_DATA + channelDataFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(channelDataFull), null, null);

        }

        #endregion

        #region DELETE

       

        public int DeleteByChannelId(int id)
        {
            var returnVal = Channel_DataDBBase.Create().DeleteByChannelId(id);
            if (returnVal != -1)
                FlushAllChannel_DataCache(string.Empty);
            return returnVal;
        }
        public int DeleteByCId(int channelId,long contentId)
        {
            var returnVal = Channel_DataDBBase.Create().DeleteById(channelId,contentId);
            if (returnVal != -1)
                FlushAllChannel_DataCache(string.Empty);
            return returnVal;
        }
        #endregion

        public void FlushAllChannel_DataCache(string containKey)
        {
            DelegateFlushAllChannel_DataCache delegateFlushAllChannel_DataCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllChannel_DataCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }
    }
}
