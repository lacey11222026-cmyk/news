using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using HotNews = DATA.HotNews;
namespace BIZ
{
    public class HotNewsBO
    {
       

        #region CREATE
        public int CreateUpdateHotNews(HotNews HotNews)
        {
            
            int returnVal = HotNewsDBBase.Create().CreateUpdateHotNews(HotNews);
          
            return returnVal;
        }
        public int UpdateStatus(int HotNewsId)
        {
            try
            {
                return HotNewsDBBase.Create().UpdateStatus(HotNewsId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "HotNewsBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int HotNewsId, bool upOrder)
        {
            try
            {
                return HotNewsDBBase.Create().UpdateOrder(HotNewsId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "HotNewsBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public HotNews GetHotNews(int HotNewsId)
        {
            try
            {
                return HotNewsDBBase.Create().GetHotNews(HotNewsId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "HotNewsBO", "GetHotNews");
                return null;
            }
        }


        public List<HotNews> GetTopHotNews(int top,string key,int status)
        {
            var data = HotNewsDBBase.Create().GetTopLastest(top, key, status);
            if (data == null)
                return null;

            return data.ToList();
        }

        
       
       

        #endregion



        #region DELETE

       

        public int DeleteHotNews(int id)
        {
            var returnVal = HotNewsDBBase.Create().DeleteHotNews(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
