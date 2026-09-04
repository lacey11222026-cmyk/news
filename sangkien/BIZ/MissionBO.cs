using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;
using Mission = DATA.Mission;
namespace BIZ
{
    public class MissionBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_MISSION;

        protected delegate void DelegateFlushAllMissionCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateMission(Mission Mission)
        {
            
            int returnVal = MissionDBBase.Create().CreateUpdateMission(Mission);
            
            if (returnVal != -1)
            {
                //UpdateCache(Mission);
                FlushAllMissionCache(strGroupKeyCached);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Mission by Mission id
        /// </summary>
        /// <param name="MissionId">The Mission id.</param>
        /// <returns></returns>
        //public Mission GetMission(int MissionId)
        //{
        //    return MissionDBBase.Create().GetMission(MissionId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Mission by id => add to local cache
        /// </summary>
        /// <param name="MissionId">The Mission id.</param>
        /// <returns></returns>
        public Mission GetMission(int MissionId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_MISSION + MissionId;
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<Mission>(cachedata.ToString());

                var item = MissionDBBase.Create().GetMission(MissionId);
                
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        public MISSION_FULL GetMissionFull(int MissionId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_MISSION + MissionId;

                //var item = (MISSION_FULL)LocalCaching.GetData(strKeyCached);
                //if (item != null)
                //    return item;

                var content = GetMission(MissionId);

                var item = new MISSION_FULL
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                   
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                  
                    Organ = content.Organ,
                    Result = content.Result,
                    //Private = content.Private
                    FromDate = content.FromDate,
                    ToDate = content.ToDate,

                };
                
                //RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        private IEnumerable<Mission> GetTopLastestMissions(int top, int categoryId)
        {
            var result = MissionDBBase.Create().GetTopLastestMissions(top, categoryId);
            if (result == null)
                return null;
            return result;
        }
        public List<MISSION_FULL> GetTopLastestMissionsFull(int top, int categoryId = 0)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_MISSIONS + top + "_category" + categoryId;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var lstItem = (List<MISSION_FULL>)LocalCaching.GetData(strKeyCached);

            //if (lstItem != null && lstItem.Count > 0)
            //    return lstItem;
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<MISSION_FULL>>(cachedata.ToString());
            var lstItemBase = GetTopLastestMissions(top, categoryId);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<MISSION_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new MISSION_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryPathway=content.CategoryId
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                   // FilePath = content.FilePath,
                   
                    CreatedBy = content.CreatedBy,
                    //SignedBy = content.SignedBy,
                    Organ = content.Organ,
                    CreatedDate = content.CreatedDate,

                    PublishDate = content.PublishDate,
                    //EffectiveDate = content.EffectiveDate,
                    //ExpiryDate = content.ExpiryDate,
                    Status = content.Status,
                    Result = content.Result,
                    //Private = content.Private
                    Accept = content.Accept,
                      FromDate = content.FromDate,
                    ToDate = content.ToDate,
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
            }

            return lstItem;
        }
       
        public List<MISSION_FULL> GetMissionsFuLLPaged(string title, int categoryId, int status,int createdBy, int year, int pageIndex, int pageSize, ref int totalRecords)
        {
            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_MISSIONS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_title" + title + "_status" + status + "_cr" + createdBy + "_year" + year;
            string strKeyCachedTotal = strKeyCached + "total";
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<MISSION_FULL>>(cachedata.ToString());
            }
            var Missions = GetMissionsPaged(title, categoryId, status, createdBy, year, pageIndex, pageSize,ref totalRecords);

            if (Missions == null)
                return null;
            List<MISSION_FULL> albumFulls = new List<MISSION_FULL>();
            foreach (var content in Missions)
            {
                MISSION_FULL MissionsFull = new MISSION_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryPathway=content.CategoryId
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                    

                    CreatedBy = content.CreatedBy,
                    //SignedBy = content.SignedBy,
                    Organ = content.Organ,
                    CreatedDate = content.CreatedDate,

                    PublishDate = content.PublishDate,

                    FromDate = content.FromDate,
                    ToDate = content.ToDate,

                    Status = content.Status,
                    Result = content.Result,
                    Accept = content.Accept 


                };
               
                albumFulls.Add(MissionsFull);
            }
            RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(albumFulls));
            RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
            return albumFulls.ToList();
        }

        private IEnumerable<Mission>  GetMissionsPaged(string title, int categoryId, int status, int createdBy, int year, int pageIndex, int pageSize, ref int totalRecords)
        {
            return MissionDBBase.Create().GetMissionsByFilter(title, categoryId, status, pageIndex, pageSize, year, createdBy, ref totalRecords);
        }


        #endregion

        #region UPDATE
        public int ViewAdd(long id)
        {

            var returnVal = MissionDBBase.Create().ViewAdd(id);
            
            return returnVal;

        }
        

        #endregion

        #region DELETE

        public int DeleteMissions(string listIds)
        {
            var returnVal = MissionDBBase.Create().DeleteMissions(listIds);
            if (returnVal != -1)
                FlushAllMissionCache(strGroupKeyCached);
            return returnVal;
        }

        public int DeleteMission(int id)
        {
            var returnVal = MissionDBBase.Create().DeleteMission(id);
            if (returnVal != -1)
                FlushAllMissionCache(strGroupKeyCached);
            return returnVal;
        }

        #endregion

        public void FlushAllMissionCache(string containKey)
        {
            //DelegateFlushAllMissionCache delegateFlushAllMissionCache = LocalCaching.RemoveContainKeyInGroupKey;
            //delegateFlushAllMissionCache.BeginInvoke(strGroupKeyCached, containKey, null, null);

            RedisCaching.RemoveGroup(containKey);
        }

    }
}
