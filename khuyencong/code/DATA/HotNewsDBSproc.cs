using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class HotNewsDBSproc : HotNewsDBBase
    {
        #region Overrides of HotNewsDBBase

        public override int CreateUpdateHotNews(HotNews manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Title;
                string _des = manufactory.Description;
                string _url = manufactory.Url;
                string _key = manufactory.Key;
                string _Image = manufactory.Image;
                
                
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                int? _isBlank = manufactory.IsBlank;

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_HotNews_InsertUpdate(_id, _name, _key, _des, _Image, _url, _order, _status, _isBlank ,ref responecode);
                    return responecode.GetValueOrDefault();
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "HotNewsDBSproc", "CreateUpdateHotNews");
                return -1;
            }
        }
       
        public override HotNews GetHotNews(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetHotNewssDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<HotNews> GetTopLastest(int top,string key,int status)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "[Key] = " + "'" + key + "' ";
            if (status >-1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status =" + status;
            }
            var orderBy = "[Order] DESC, Id DESC";

            return GetHotNewssDyn(select, where, orderBy);
        }
       
     
        public override IEnumerable<HotNews> GetHotNewssDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_HotNews_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "HotNewsDBSproc", "GetHotNewssDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_HotNews_UpdateSortOrder(Id, upOrder);
                    return 1;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
       
       
        public override int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                     datacontext.SP_HotNewsUpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      

        public override int DeleteHotNews(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_HotNews_Delete(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

   

        #endregion
    }
}
