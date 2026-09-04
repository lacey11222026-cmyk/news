using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class OrderDBSproc : OrderDBBase
    {
        #region Overrides of OrderDBBase

        public override int InsertProduct(OrderProductMapping manufactory)
        {
            //int? responecode = 0;
            try
            {
                long? _id = manufactory.OrderId;
                int? _status = manufactory.Status;
                int? _couponId = manufactory.ProductId;
                string _productName = manufactory.ProductName;
                int? _amount = manufactory.Amount;
                Decimal? _price = manufactory.Price;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_OrderProductMapping_Insert(_id, _couponId, _productName, _amount, _status, _price);
                    //return responecode.GetValueOrDefault();
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "InsertCoupon");
                return -1;
            }
        }
        public override int InsertCoupon(OrderCouponMapping manufactory)
        {
            //int? responecode = 0;
            try
            {
                long? _id = manufactory.OrderId;
                int? _status = manufactory.Status;
                int? _couponId = manufactory.CouponId;
                string _couponCode = manufactory.CouponCode;
                int? _currencyCosts = manufactory.CurrencyCosts;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_OrderCouponMapping_Insert(_id, _couponId, _couponCode, _status, _currencyCosts);
                    //return responecode.GetValueOrDefault();
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "InsertCoupon");
                return -1;
            }
        }
        public override int Create(Order manufactory)
        {
            
            try
            {
                //long? _id = manufactory.Id;
                string _orderNo = manufactory.OrderNo;
                string _name = manufactory.FullName;
                string _email = manufactory.Email;
                string _mobile = manufactory.Mobile;
                int? _region = manufactory.CustomerRegion;
                string _city = manufactory.CustomerCity;
                string _address = manufactory.CustomerAddress;
                string _ip = manufactory.CustomerIp;
                int? _status = manufactory.Status;
                int? _paymentMethod = manufactory.PaymentMethod;
                 string _comment = manufactory.CustomerComment;
                bool? _delete = manufactory.Deleted;
                string _apiRespone = manufactory.APIRespone;
                string _saleCode = manufactory.SaleCode;
                Decimal? _price = manufactory.OrderPrice;
                Decimal? _orderBilling = manufactory.OrderBilling;
                Decimal? _orderTotalShipping = manufactory.OrderTotalShipping;
                Decimal? _orderTotal = manufactory.OrderTotal;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Order_Create(_orderNo, _paymentMethod, _name, _email, _mobile, _region, _city, _address, _ip, _comment, _price, _orderBilling, _orderTotalShipping,_orderTotal,_apiRespone,_saleCode,_delete,_status);
                    
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "Create");
                return -1;
            }
        }
        public override int Confirm(Order manufactory)
        {
            //int? responecode = 0;
            try
            {
                long? _id = manufactory.Id;
                int? _status = manufactory.Status;
                string _comment = manufactory.CustomerComment;
                string _apiRespone = manufactory.APIRespone;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return  datacontext.sp_Order_Confirm(_id,_status,_apiRespone);
                    //return responecode.GetValueOrDefault();
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "Confirm");
                return -1;
            }
        }
        public override int Update(Order manufactory)
        {
           
            try
            {
                long? _id = manufactory.Id;
                string _name = manufactory.FullName;
                string _email = manufactory.Email;
                string _mobile = manufactory.Mobile;
                int? _region = manufactory.CustomerRegion;
                string _city = manufactory.CustomerCity;
                string _address = manufactory.CustomerAddress;
                string _comment = manufactory.CustomerComment;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Order_UpdateCMS(_id, _name, _email, _mobile, _region,_city,_address,_comment);
                    
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "Update");
                return -1;
            }
        }
        public override Order GetOrder(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetOrdersDyn(select, where, order).FirstOrDefault();
        }
        public override Order GetOrder(string OrderNo)
        {
            var select = "*";
            var where = "OrderNo = " + OrderNo;
            var order = string.Empty;

            return GetOrdersDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Order> GetAllPaged(string keyword, int pageIndex, int pageSize, int? status, string fromdate, string todate, ref int totalRecords)
        {
            var select = string.Empty;
            string where = "[Deleted]= 0 ";
            keyword = Utils.FormatKeywordSearch(keyword);
            
         

            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "( OrderNo LIKE N'%" + keyword + "%' ";
                where += "OR Email LIKE N'%" + keyword + "%' ";
                where += "OR Mobile LIKE N'%" + keyword + "%' ";
                where += "OR FullName LIKE N'%" + keyword + "%' )";

            }
            if (status.HasValue&& status.GetValueOrDefault()>-1000)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + status.GetValueOrDefault();
            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " (convert(nvarchar(23),CreatedTime,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";



            }
            var orderBy = "ID DESC";

            return GetAllOrdersPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public IEnumerable<Order> GetAllOrdersPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
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
                    var list = datacontext.sp_Order_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "GetAllOrdersPagedDyn");
                return null;
            }
        }
        public override List<OrderCouponMapping> GetCouponByOrder(long Id)
        {
            var select = "*";
            var where = "OrderId = " + Id;
            var order = string.Empty;

            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_OrderCouponMapping_SelectDynamic(select, where, order).ToList();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "GetOrdersDyn");
                return null;
            }
        }
        public override List<OrderProductMapping> GetProductByOrder(long Id)
        {
            var select = "*";
            var where = "OrderId = " + Id;
            var order = string.Empty;

            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_OrderProductMapping_SelectDynamic(select, where, order).ToList();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "GetOrdersDyn");
                return null;
            }
        }
        public override IEnumerable<Order> GetOrdersDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Order_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "OrderDBSproc", "GetOrdersDyn");
                return null;
            }
        }
       
        public override int DeleteOrderDyn(string where)
        {
            try
            {
                string _where = where;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_OrderDeleteDynamic(_where);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override int DeleteOrder(int manuFactoryId)
        {
            string where = "Id = " + manuFactoryId;
            return DeleteOrderDyn(where);
        }

        public override int DeleteManufactories(string listOrderId)
        {
            string where = "Id IN (" + listOrderId + ")";
            return DeleteOrderDyn(where);
        }

        #endregion
    }
}
