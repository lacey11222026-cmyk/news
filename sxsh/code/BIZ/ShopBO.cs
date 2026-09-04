using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Shop = DATA.Shop;
namespace BIZ
{
    public class ShopBO
    {
       

        #region CREATE
        public int CreateUpdateShop(Shop Shop)
        {
            
            int returnVal = ShopDBBase.Create().CreateUpdateShop(Shop);
          
            return returnVal;
        }
        public int UpdateStatus(int ShopId)
        {
            try
            {
                return ShopDBBase.Create().UpdateStatus(ShopId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ShopBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int ShopId, bool upOrder)
        {
            try
            {
                return ShopDBBase.Create().UpdateOrder(ShopId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ShopBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public Shop GetShop(int ShopId)
        {
            try
            {
                return ShopDBBase.Create().GetShop(ShopId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ShopBO", "GetShop");
                return null;
            }
        }


        public List<Shop> GetTopShop(int top,int type)
        {
            var data = ShopDBBase.Create().GetTopLastest(top,type);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Shop> GetShopsPaged(string keyword,int pageIndex, int pageSize, ref int totalRecords, int? published, string lang,int type)
        {
            var data = ShopDBBase.Create().GetAllPaged(keyword,pageIndex, pageSize, ref  totalRecords, published,lang, type);
            if (data == null)
                return null;

            return data.ToList();
        }
       
       

        #endregion



        #region DELETE

       

        public int DeleteShop(int id)
        {
            var returnVal = ShopDBBase.Create().DeleteShop(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
