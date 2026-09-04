using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ShopDBSproc : ShopDBBase
    {
        #region Overrides of ShopDBBase

        public override int CreateUpdateShop(Shop manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string _address = manufactory.Address;
                string _description = manufactory.Description;
                string _phone = manufactory.Phone;
                string _cityName = manufactory.CityName;
                string _long = manufactory.Longitude;
                string _lat = manufactory.Latitude;
                int? _cityId = manufactory.CityId;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Shop_InsertUpdate(_id, _name, _address, _description,_phone, _long, _lat, _cityId,0, _cityName, "", _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ShopDBSproc", "CreateUpdateShop");
                return -1;
            }
        }
       
        public override Shop GetShop(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetShopsDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Shop> GetTopLastest(int top,int type)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            if(type>0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CityId=" + type.ToString();
            }
            var orderBy = "[Order] DESC, Id DESC";

            return GetShopsDyn(select, where, orderBy);
        }
        public override IEnumerable<Shop> GetAllPaged(string keyword,int pageIndex, int pageSize, ref int totalRecords, int? published)
        {
            var select = string.Empty;
            var where = string.Empty;
           
           
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( [Name] LIKE N'%" + keyword + "%' ";
                where += "OR [Address] LIKE N'%" + keyword + "%' ";
                where += "OR [Description] LIKE N'%" + keyword + "%' ";
                where += "OR [Phone] LIKE N'%" + keyword + "%' )";

            }
            var orderBy = "[Order] DESC, ID DESC";

            return GetAllShopsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public IEnumerable<Shop> GetAllShopsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                string _select = select;
                string _where = where;
                string _orderBy = orderBy;
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var list = datacontext.sp_Shop_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ShopDBSproc", "GetAllShopsPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Shop> GetShopsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Shop_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ShopDBSproc", "GetShopsDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Shop_UpdateSortOrder(Id, upOrder);
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
                     datacontext.SP_Shop_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      

        public override int DeleteShop(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Shop_Delete(Id, ref responeCode);
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
