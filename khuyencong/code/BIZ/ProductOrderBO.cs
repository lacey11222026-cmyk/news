using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class ProductOrderBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_PRODUCTORDER;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE

        public int CreateUpdateProductOrder(ProductOrder productOrder)
        {
            return ProductOrderDBBase.Create().CreateUpdateProductOrder(productOrder);
        }

        public int CreateUpdateProductOrder(PRODUCTORDER_FULL productOrderFull)
        {
            ProductOrder productOrder = productOrderFull.ConvertToBase();
            int returnVal = CreateUpdateProductOrder(productOrder);
            if (returnVal != -1)
            {
                UpdateCache(productOrderFull);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get productOrder by productOrder id
        /// </summary>
        /// <param name="productOrderId">The productOrder id.</param>
        /// <returns></returns>
        public ProductOrder GetProductOrder(int productOrderId)
        {
            return ProductOrderDBBase.Create().GetProductOrder(productOrderId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get productOrder by id => add to local cache
        /// </summary>
        /// <param name="productOrderId">The productOrder id.</param>
        /// <returns></returns>
        public PRODUCTORDER_FULL GetProductOrderFull(int productOrderId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_PRODUCTORDER + productOrderId;

                var item = (PRODUCTORDER_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = GetProductOrder(productOrderId);

                item = new PRODUCTORDER_FULL
                {

                    Id = itemBase.Id,
                    ProductId = itemBase.ProductId,
                    ProductCode = itemBase.ProductCode,
                    ProductTitle = itemBase.ProductTitle,
                    ProductPrice = itemBase.ProductPrice,
                    //UserId = itemBase.UserId,
                    UserName = itemBase.UserName,
                    UserEmail = itemBase.UserEmail,
                    UserPhone = itemBase.UserPhone,
                    UserMobile = itemBase.UserMobile,
                    UserAddress = itemBase.UserAddress,
                    OrderDate = itemBase.OrderDate,
                    OrderDateStamp = itemBase.OrderDateStamp,
                    State = itemBase.State,
                    Published = itemBase.Published,
                    // Ordering = itemBase.Ordering,
                    // Params = itemBase.Params,
                };

                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e,"GetProductOrderFull");
                return null;
            }
        }

        public List<ProductOrder> GetAllProductOrdersPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var productOrders = ProductOrderDBBase.Create().GetAllProductOrdersPaged(pageIndex, pageSize, ref totalRecords);
            if (productOrders == null)
                return null;

            return productOrders.ToList();
        }

        public List<PRODUCTORDER_FULL> GetAllProductOrderFullsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var productOrders = GetAllProductOrdersPaged(pageIndex, pageSize, ref totalRecords);
            List<PRODUCTORDER_FULL> productOrderFulls = new List<PRODUCTORDER_FULL>();
            foreach (var productOrder in productOrders)
            {
                PRODUCTORDER_FULL productOrderFull = new PRODUCTORDER_FULL()
                {
                    Id = productOrder.Id,
                    ProductId = productOrder.ProductId,
                    ProductCode = productOrder.ProductCode,
                    ProductTitle = productOrder.ProductTitle,
                    ProductPrice = productOrder.ProductPrice,
                    //UserId = itemBase.UserId,
                    UserName = productOrder.UserName,
                    UserEmail = productOrder.UserEmail,
                    UserPhone = productOrder.UserPhone,
                    UserMobile = productOrder.UserMobile,
                    UserAddress = productOrder.UserAddress,
                    OrderDate = productOrder.OrderDate,
                    OrderDateStamp = productOrder.OrderDateStamp,
                    State = productOrder.State,
                    Published = productOrder.Published,
                    // Ordering = itemBase.Ordering,
                    // Params = itemBase.Params,
                };

                productOrderFulls.Add(productOrderFull);
            }

            return productOrderFulls;

        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of productOrders have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllProductOrdersPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PRODUCTORDERS_PAGED_JSON + pageIndex + pageSize;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_PRODUCTORDER;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<PRODUCTORDER_FULL> productOrders = GetAllProductOrderFullsPaged(pageIndex, pageSize, ref totalRecords);

            if (productOrders == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(productOrders, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return json;
        }

        public List<ProductOrder> FilterProductOrders(string title, int categoryId)
        {
            var productOrders = ProductOrderDBBase.Create().GetAllProductOrders(title, categoryId);
            if (productOrders == null)
                return null;
            return productOrders.ToList();
        }

        public List<PRODUCTORDER_FULL> FilterProductOrderFulls(string title, int categoryId)
        {
            var productOrders = FilterProductOrders(title, categoryId);
            if (productOrders == null)
                return null;
            List<PRODUCTORDER_FULL> lstProductOrderFulls = new List<PRODUCTORDER_FULL>();
            foreach (var productOrder in productOrders)
            {
                PRODUCTORDER_FULL productOrderFull = new PRODUCTORDER_FULL()
                {
                    Id = productOrder.Id,
                    ProductId = productOrder.ProductId,
                    ProductCode = productOrder.ProductCode,
                    ProductTitle = productOrder.ProductTitle,
                    ProductPrice = productOrder.ProductPrice,
                    //UserId = itemBase.UserId,
                    UserName = productOrder.UserName,
                    UserEmail = productOrder.UserEmail,
                    UserPhone = productOrder.UserPhone,
                    UserMobile = productOrder.UserMobile,
                    UserAddress = productOrder.UserAddress,
                    OrderDate = productOrder.OrderDate,
                    OrderDateStamp = productOrder.OrderDateStamp,
                    State = productOrder.State,
                    Published = productOrder.Published,
                };

                lstProductOrderFulls.Add(productOrderFull);
            }

            return lstProductOrderFulls;
        }

        public List<PRODUCTORDER_FULL> GetProductOrdersByCategory(int categoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PRODUCTORDERS_BYCATEGORY + categoryId;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_PRODUCTORDER;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstCachedProductOrders = (List<PRODUCTORDER_FULL>)LocalCaching.GetData(keyCache);

            if (lstCachedProductOrders != null)
                return lstCachedProductOrders;

            var productOrders = FilterProductOrders(string.Empty, categoryId);

            if (productOrders == null)
                return null;

            var publishedProductOrders = (from p in productOrders where p.Published == 1 select p).ToList();

            lstCachedProductOrders = new List<PRODUCTORDER_FULL>();
            foreach (var productOrder in publishedProductOrders)
            {
                PRODUCTORDER_FULL productOrderFull = new PRODUCTORDER_FULL()
                {
                    Id = productOrder.Id,
                    ProductId = productOrder.ProductId,
                    ProductCode = productOrder.ProductCode,
                    ProductTitle = productOrder.ProductTitle,
                    ProductPrice = productOrder.ProductPrice,
                    //UserId = itemBase.UserId,
                    UserName = productOrder.UserName,
                    UserEmail = productOrder.UserEmail,
                    UserPhone = productOrder.UserPhone,
                    UserMobile = productOrder.UserMobile,
                    UserAddress = productOrder.UserAddress,
                    OrderDate = productOrder.OrderDate,
                    OrderDateStamp = productOrder.OrderDateStamp,
                    State = productOrder.State,
                    Published = productOrder.Published,
                };

                lstCachedProductOrders.Add(productOrderFull);
            }

            if (lstCachedProductOrders.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedProductOrders);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstCachedProductOrders;
        }

        #endregion

        #region UPDATE

        public void UpdateCache(PRODUCTORDER_FULL productOrderFull)
        {
            var strKeyCached = Constants.CACHE_KEY_PRODUCTORDER + productOrderFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, productOrderFull, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteProductOrders(string listIds)
        {

            var returnVal = ProductOrderDBBase.Create().DeleteProductOrders(listIds);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteProductOrder(int id)
        {
            var returnVal = ProductOrderDBBase.Create().DeleteProductOrder(id);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public void FlushAllCache(string containKey)
        {
            DelegateFlushAllCache delegateFlushAllCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        #endregion
    }
}
