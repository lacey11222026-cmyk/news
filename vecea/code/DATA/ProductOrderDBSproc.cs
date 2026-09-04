using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class ProductOrderDBSproc : ProductOrderDBBase
    {
        public override int CreateUpdateProductOrder(ProductOrder productOrder)
        {
            try
            {
                int _id = productOrder.Id;
                int? _productid = productOrder.ProductId;
                string _productcode = productOrder.ProductCode;
                string _producttitle = productOrder.ProductTitle;
                double? _productprice = productOrder.ProductPrice;
                //int? _userid = productOrder.UserId;
                string _username = productOrder.UserName;
                string _useremail = productOrder.UserEmail;
                string _userphone = productOrder.UserPhone;
                string _usermobile = productOrder.UserMobile;
                string _useraddress = productOrder.UserAddress;
                System.DateTime? _orderdate = productOrder.OrderDate;
                double? _orderdatestamp = productOrder.OrderDateStamp;
                byte? _state = productOrder.State;
                byte? _published = productOrder.Published;
                //byte? _ordering = productOrder.Ordering;
                //string _params = productOrder.Params;

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_ProductOrder_InsertUpdate(_id, _productid, _productcode, _producttitle, _productprice, _username, _useremail, _userphone, _usermobile, _useraddress, _orderdate, _orderdatestamp, _state, _published);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override ProductOrder GetProductOrder(int productOrderId)
        {
            var select = "*";
            var where = "Id = " + productOrderId;
            var orderBy = string.Empty;

            var results = GetProductOrdersDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<ProductOrder> GetProductOrdersDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_ProductOrder_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return null;
            }
        }

        public override IEnumerable<ProductOrder> GetAllProductOrdersPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Id DESC";

            return GetAllProductOrdersPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<ProductOrder> GetAllProductOrdersPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_ProductOrder_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return null;
            }
        }

        public override IEnumerable<ProductOrder> GetAllProductOrders(string name, int categoryId)
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(name))
                where += "Title LIKE N'%" + name + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryId =" + categoryId;
            }

            return GetProductOrdersDyn(select, where, orderBy);
        }

        public override int DeleteProductOrderDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_ProductOrder_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override int DeleteProductOrder(int productOrderId) { var where = "Id =" + productOrderId; return DeleteProductOrderDyn(where); }
        public override int DeleteProductOrders(string lstProductOrderIds) { var where = "Id IN (" + lstProductOrderIds + ")"; return DeleteProductOrderDyn(where); }


    }
}
