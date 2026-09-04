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
    public class ChannelBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CHANNEL;
        protected delegate void DelegateFlushAllChannelCache(string strGroupKeyCached, string containKey);

        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        //public int CreateUpdateChannel(Channel Channel)
        //{
        //    return ChannelDBBase.Create().CreateUpdateChannel(Channel);
        //}

        public int CreateUpdateChannel(Channel Channel)
        {
            //Channel Channel = ChannelFull.ConvertToBase();
            int returnVal = ChannelDBBase.Create().CreateUpdateChannel(Channel);
            if (returnVal != -1)
            {
                UpdateCache(Channel);
                FlushAllChannelCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Channel by Channel id
        /// </summary>
        /// <param name="channelId">The Channel id.</param>
        /// <returns></returns>
        public Channel GetChannel(int channelId)
        {
            return ChannelDBBase.Create().GetChannel(channelId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Channel by id => add to local cache
        /// </summary>
        /// <param name="channelId">The Channel id.</param>
        /// <returns></returns>
        public Channel GetChannelFull(int channelId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_CHANNEL + channelId;

                var data = LocalCaching.GetData(strKeyCached);
                if (data != null)
                    return JsonConvert.DeserializeObject<Channel>(data.ToString());

                var item = GetChannel(channelId);



                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "GetChannelFull");
                return null;
            }
        }
        
        public List<Channel> GetAllChannelsPaged(string title,int pageIndex, int pageSize, ref int totalRecords)
        {
            var Channels = ChannelDBBase.Create().GetAllChannelsPaged(title, pageIndex, pageSize, ref totalRecords);
            if (Channels == null)
                return null;

            return Channels.ToList();
        }
        public List<Channel> GetFilter(string title, int top)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_CHANNEL + top + "_top" + top + title;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Channel>>(data.ToString());

            var lstItem = ChannelDBBase.Create().GetTopFiller(title, top).ToList();
            
            if (lstItem.Count > 0)
            {


                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }
        public List<Channel> GetChannelByIds(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_CHANNEL_BYIDS + top + "_lst" + ids + isArragne;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Channel>>(data.ToString());

            var lstItemBase = ChannelDBBase.Create().GetChannelByIds(ids, top);
            var lstItem = new List<Channel>();
            if (isArragne)
            {

                var listIds = ids.Split(',').ToList();
                foreach (var itemid in listIds)
                {
                    if (!string.IsNullOrEmpty(itemid))
                    {
                        foreach (var content in lstItemBase)
                        {
                            if (content.Id == long.Parse(itemid))
                            {

                                lstItem.Add(content);
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (var content in lstItemBase)
                {


                    lstItem.Add(content);
                }
            }


            if (lstItem.Count > 0)
            {


                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }
        public List<Channel> GetAllChannelFullsPaged(string title,int pageIndex, int pageSize, ref int totalRecords,int status)
        {
            string strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_CHANNEL + pageIndex + "_pagesize" + pageSize+"_status"+status+"_title"+ title;
            string strKeyCachedTotal = Constants.CACHE_KEY_TOP_LASTEST_CHANNEL + pageIndex + "_pagesize" + pageSize + "_status" + status + "_total" + "_title" + title; 

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
                return JsonConvert.DeserializeObject<List<Channel>>(data.ToString());
            }

            var lstItem = ChannelDBBase.Create().GetAllChannelsPaged(title,pageIndex, pageSize, ref totalRecords, status).ToList();
            if (lstItem.Count > 0)
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

        public void UpdateCache(Channel channelFull)
        {
            var strKeyCached = Constants.CACHE_KEY_CHANNEL + channelFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(channelFull), null, null);

        }

        #endregion

        #region DELETE

        public int DeleteChannels(string listIds)
        {
            var returnVal = ChannelDBBase.Create().DeleteChannels(listIds);
            if (returnVal != -1)
                FlushAllChannelCache(string.Empty);
            return returnVal;
        }

        public int DeleteChannel(int id)
        {
            var returnVal = ChannelDBBase.Create().DeleteChannel(id);
            if (returnVal != -1)
                FlushAllChannelCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllChannelCache(string containKey)
        {
            DelegateFlushAllChannelCache delegateFlushAllChannelCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllChannelCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }
    }
}
