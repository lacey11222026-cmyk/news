using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class ProductBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_PRODUCT;
        readonly string strGroupKeyProductAttributeCached = Constants.CACHE_GROUPKEY_PRODUCTATTRIBUTE;

        protected delegate void DelegateFlushAllProductCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateFlushAllProductAttributeCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE UPDATE

        private int CreateUpdateProduct(Product product)
        {
            return ProductDBBase.Create().CreateUpdateProduct(product);
        }

        public int CreateUpdateProduct(PRODUCT_FULL productFull)
        {
            Product product = productFull.ConvertToBase();
            int returnValue = CreateUpdateProduct(product);
            if (returnValue != -1)
            {
                UpdateCache(productFull);
                FlushAllProductCache(string.Empty);
            }

            return returnValue;
        }

        public int UpdateAttributes(int productId, string attributes)
        {
            Product product = GetProduct(productId);
            if (product == null)
                return -1;

            product.Attributes = attributes;

            return CreateUpdateProduct(product);
        }


        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get product by product id
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        private Product GetProduct(int productId)
        {
            return ProductDBBase.Create().GetProduct(productId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get product by id => add to local cache
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public PRODUCT_FULL GetProductFull(int productId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_PRODUCT + productId;

                var item = (PRODUCT_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = GetProduct(productId);

                item = new PRODUCT_FULL
                {

                    Id = itemBase.Id,
                    CategoryId = itemBase.CategoryId,
                    Title = itemBase.Title,
                    Name = itemBase.Name,
                    Alias = itemBase.Alias,
                    ProductCode = itemBase.ProductCode,
                    IntroText = itemBase.IntroText,
                    FullText = itemBase.FullText,
                    CategoryPathway = itemBase.CategoryPathway,
                    Images = itemBase.Images,
                    Thumbnail = itemBase.Thumbnail,
                    Price = itemBase.Price,
                    PriceModifyDate = itemBase.PriceModifyDate,
                    Attributes = itemBase.Attributes,
                    CreatedBy = itemBase.CreatedBy,
                    CreatedDate = itemBase.CreatedDate,
                    ModifiedBy = itemBase.ModifiedBy,
                    ModifiedDate = itemBase.ModifiedDate,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,
                    Hits = itemBase.Hits,
                    Count = itemBase.Count,
                    Params = itemBase.Params,
                    ManufactoryId = itemBase.ManufactoryId
                };

                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public string GetProductFull_JSON(int productId)
        {
            var productFull = GetProductFull(productId);

            if (productFull == null)
                return null;

            return UTILS.Utils.ConvertToJson(GetProductFull(productId), string.Empty);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of attributes have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllProductsPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PRODUCTS_PAGED_JSON + pageIndex + "_" + pageSize;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<PRODUCT_FULL> products = GetAllProductFullPaged(pageIndex, pageSize, ref totalRecords);

            if (products == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(products, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return json;
        }
        public string GetFilterProductsPaged_JSON(int pageIndex, int pageSize, string title, int categoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PRODUCTS_PAGED_JSON + pageIndex + "_" + pageSize + "_" + title + "_" + categoryId;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<PRODUCT_FULL> products = FilterProductFulls(title, categoryId, pageIndex, pageSize, ref totalRecords);

            if (products == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(products, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return json;
        }
        private IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var listProduct = ProductDBBase.Create().GetAllProductsPaged(pageIndex, pageSize, ref totalRecords);
            if (listProduct == null)
                return null;
            return listProduct.ToList();
        }

        private IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords, short published)
        {
            var listProduct = ProductDBBase.Create().GetAllProductsPaged(pageIndex, pageSize, ref totalRecords, published);
            if (listProduct == null)
                return null;
            return listProduct.ToList();
        }

        public List<Product> GetAllProductsByCateogryPaged(int pageIndex, int pageSize, ref int totalRecords, short published, int categoryId)
        {
            var listProduct = ProductDBBase.Create().GetAllProductsPaged(pageIndex, pageSize, ref totalRecords, published, categoryId);
            if (listProduct == null)
                return null;
            return listProduct.ToList();
        }

        public List<PRODUCT_FULL> GetAllProductFullPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                var lstItemBase = GetAllProductsPaged(pageIndex, pageSize, ref totalRecords);
                if (lstItemBase == null)
                    return null;

                List<PRODUCT_FULL> lstItem = new List<PRODUCT_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {

                        Id = itemBase.Id,
                        CategoryId = itemBase.CategoryId,
                        Title = itemBase.Title,
                        Name = itemBase.Name,
                        Alias = itemBase.Alias,
                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        FullText = itemBase.FullText,
                        CategoryPathway = itemBase.CategoryPathway,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail,
                        Price = itemBase.Price,
                        PriceModifyDate = itemBase.PriceModifyDate,
                        Attributes = itemBase.Attributes,
                        CreatedBy = itemBase.CreatedBy,
                        CreatedDate = itemBase.CreatedDate,
                        ModifiedBy = itemBase.ModifiedBy,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Hits = itemBase.Hits,
                        Count = itemBase.Count,
                        Params = itemBase.Params,
                        ManufactoryId = itemBase.ManufactoryId

                    };

                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public List<PRODUCT_FULL> GetAllProductFullPaged(int pageIndex, int pageSize, ref int totalRecords, short published)
        {
            try
            {
                var lstItemBase = GetAllProductsPaged(pageIndex, pageSize, ref totalRecords, published);
                if (lstItemBase == null)
                    return null;

                List<PRODUCT_FULL> lstItem = new List<PRODUCT_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {

                        Id = itemBase.Id,
                        CategoryId = itemBase.CategoryId,
                        Title = itemBase.Title,
                        Name = itemBase.Name,
                        Alias = itemBase.Alias,
                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        FullText = itemBase.FullText,
                        CategoryPathway = itemBase.CategoryPathway,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail,
                        Price = itemBase.Price,
                        PriceModifyDate = itemBase.PriceModifyDate,
                        Attributes = itemBase.Attributes,
                        CreatedBy = itemBase.CreatedBy,
                        CreatedDate = itemBase.CreatedDate,
                        ModifiedBy = itemBase.ModifiedBy,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Hits = itemBase.Hits,
                        Count = itemBase.Count,
                        Params = itemBase.Params,
                        ManufactoryId = itemBase.ManufactoryId

                    };

                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public List<PRODUCT_FULL> GetAllProductFullByCategoryPaged(int pageIndex, int pageSize, ref int totalRecords, short published, int categoryId)
        {
            try
            {
                string keyCache = Constants.CACHE_KEY_ALL_PRODUCT_PAGED + pageIndex + "_" + pageSize + "_" + published + "_" + categoryId;
                //string groupKeyCache = Constants.CACHE_GROUPKEY_PRODUCT;

                var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

                var lstItems = (List<PRODUCT_FULL>)LocalCaching.GetData(keyCache);
                if (lstItems != null && lstItems.Count > 0)
                    return lstItems;

                var lstItemBase = GetAllProductsByCateogryPaged(pageIndex, pageSize, ref totalRecords, published, categoryId);
                if (lstItemBase == null)
                    return null;

                lstItems = new List<PRODUCT_FULL>();
                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {
                        Id = itemBase.Id,
                        CategoryId = itemBase.CategoryId,
                        Title = itemBase.Title,
                        Name = itemBase.Name,
                        Alias = itemBase.Alias,
                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        FullText = itemBase.FullText,
                        CategoryPathway = itemBase.CategoryPathway,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail,
                        Price = itemBase.Price,
                        PriceModifyDate = itemBase.PriceModifyDate,
                        Attributes = itemBase.Attributes,
                        CreatedBy = itemBase.CreatedBy,
                        CreatedDate = itemBase.CreatedDate,
                        ModifiedBy = itemBase.ModifiedBy,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Hits = itemBase.Hits,
                        Count = itemBase.Count,
                        Params = itemBase.Params,
                        ManufactoryId = itemBase.ManufactoryId

                    };

                    lstItems.Add(item);
                }

                if (lstItems.Count > 0)
                {
                    LocalCaching.Add(keyCache, lstItems);
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                }

                return lstItems;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        private IEnumerable<Product> FilterProducts(string title, int categoryId, int pageIndex, int pageSize, ref int totalRecords)
        {
            var products = ProductDBBase.Create().GetFilterProducts(title, categoryId, pageIndex, pageSize, ref  totalRecords);
            if (products == null)
                return null;
            return products.ToList();
        }

        public List<PRODUCT_FULL> FilterProductFulls(string title, int categoryId, int pageIndex, int pageSize, ref int totalRecords)
        {
            var products = FilterProducts(title, categoryId, pageIndex, pageSize, ref  totalRecords);
            if (products == null)
                return null;
            List<PRODUCT_FULL> lstProductFulls = new List<PRODUCT_FULL>();
            foreach (var product in products)
            {
                PRODUCT_FULL productFull = new PRODUCT_FULL()
                {

                    Id = product.Id,
                    CategoryId = product.CategoryId,
                    Title = product.Title,
                    Name = product.Name,
                    Alias = product.Alias,
                    ProductCode = product.ProductCode,
                    IntroText = product.IntroText,
                    FullText = product.FullText,
                    CategoryPathway = product.CategoryPathway,
                    Images = product.Images,
                    Thumbnail = product.Thumbnail,
                    Price = product.Price,
                    PriceModifyDate = product.PriceModifyDate,
                    Attributes = product.Attributes,
                    CreatedBy = product.CreatedBy,
                    CreatedDate = product.CreatedDate,
                    ModifiedBy = product.ModifiedBy,
                    ModifiedDate = product.ModifiedDate,
                    Published = product.Published,
                    Ordering = product.Ordering,
                    Hits = product.Hits,
                    Count = product.Count,
                    Params = product.Params,
                    ManufactoryId = product.ManufactoryId
                };

                lstProductFulls.Add(productFull);
            }

            return lstProductFulls;
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 07/09/2011 10:55 PM
        /// todo: get top product group by category (site - homepage display)        
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        private IEnumerable<Product> GetTopProductsByCategory(int top, int categoryId)
        {
            var result = ProductDBBase.Create().GetTopProductsByCategory(top, categoryId);
            if (result == null)
                return null;

            return result.ToList();
        }

        public List<PRODUCT_FULL> GetTopProductFullsByCategory(int top, int categoryId)
        {
            try
            {
                string keyCache = Constants.CACHE_KEY_TOP_PRODUCTS_BY_CATEGORY + "_" + top + "_" + categoryId;

                var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

                List<PRODUCT_FULL> lstItem = (List<PRODUCT_FULL>)LocalCaching.GetData(keyCache);
                if (lstItem != null && lstItem.Count > 0)
                    return lstItem;

                var lstItemBase = GetTopProductsByCategory(top, categoryId);
                if (lstItemBase == null)
                    return null;

                lstItem = new List<PRODUCT_FULL>();
                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {
                        Id = itemBase.Id,
                        CategoryId = itemBase.CategoryId,
                        Title = itemBase.Title,
                        Name = itemBase.Name,
                        Alias = itemBase.Alias,
                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        FullText = itemBase.FullText,
                        CategoryPathway = itemBase.CategoryPathway,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail,
                        Price = itemBase.Price,
                        PriceModifyDate = itemBase.PriceModifyDate,
                        Attributes = itemBase.Attributes,
                        CreatedBy = itemBase.CreatedBy,
                        CreatedDate = itemBase.CreatedDate,
                        ModifiedBy = itemBase.ModifiedBy,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Hits = itemBase.Hits,
                        Count = itemBase.Count,
                        Params = itemBase.Params,
                        ManufactoryId = itemBase.ManufactoryId

                    };

                    lstItem.Add(item);
                }

                if (lstItem.Count > 0)
                {
                    LocalCaching.Add(keyCache, lstItem);
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        private IEnumerable<Product> GetAllProductsByCategory(int categoryId, byte published)
        {
            var result = ProductDBBase.Create().GetAllProducts(string.Empty, categoryId, published);
            if (result == null)
                return null;

            return result.ToList();

        }

        public List<PRODUCT_FULL> GetAllProductFullsByCategory(int categoryId, byte published)
        {
            try
            {
                string keyCache = Constants.CACHE_KEY_ALL_PRODUCTS_BYCATEGORY_BYPUBLISHED + categoryId + "_" + published;

                var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

                List<PRODUCT_FULL> lstItem = (List<PRODUCT_FULL>)LocalCaching.GetData(keyCache);
                if (lstItem != null && lstItem.Count > 0)
                    return lstItem;

                var lstItemBase = GetAllProductsByCategory(categoryId, published);
                if (lstItemBase == null)
                    return null;

                lstItem = new List<PRODUCT_FULL>();
                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {
                        Id = itemBase.Id,
                        CategoryId = itemBase.CategoryId,
                        Title = itemBase.Title,
                        Name = itemBase.Name,
                        Alias = itemBase.Alias,
                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        FullText = itemBase.FullText,
                        CategoryPathway = itemBase.CategoryPathway,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail,
                        Price = itemBase.Price,
                        PriceModifyDate = itemBase.PriceModifyDate,
                        Attributes = itemBase.Attributes,
                        CreatedBy = itemBase.CreatedBy,
                        CreatedDate = itemBase.CreatedDate,
                        ModifiedBy = itemBase.ModifiedBy,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Hits = itemBase.Hits,
                        Count = itemBase.Count,
                        Params = itemBase.Params,
                        ManufactoryId = itemBase.ManufactoryId
                    };

                    lstItem.Add(item);
                }

                if (lstItem.Count > 0)
                {
                    LocalCaching.Add(keyCache, lstItem);
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }

        }
        public List<PRODUCT_FULL> GetTopProductsByIds(List<string> ids, int top, bool isArragne = false)
        {
            string _ids;
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string id in ids)
            {
                stringBuilder.Append(id).Append(",");
            }

            _ids = stringBuilder.ToString();
            _ids = _ids.TrimEnd(',');
            return GetTopProductsByIds(_ids, top, isArragne);

        }
        public List<PRODUCT_FULL> GetTopProductsByIds(string[] ids, int top, bool isArragne = false)
        {
            string _ids;
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string id in ids)
            {
                stringBuilder.Append(id).Append(",");
            }

            _ids = stringBuilder.ToString();
            _ids = _ids.TrimEnd(',');
            return GetTopProductsByIds(_ids, top, isArragne);

        }
        public List<PRODUCT_FULL> GetTopProductsByIds(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_PRODUCTS_BYIDS + top + "_lst" + ids + isArragne;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<PRODUCT_FULL>)LocalCaching.GetData(strKeyCached);
            //var lstItem = new List<CATEGORY_FULL> ();
            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            var lstItemBase = ProductDBBase.Create().GetTopProductsByIds(ids, top);

            if (lstItemBase == null)
                return null;

            lstItem = new List<PRODUCT_FULL>();
            if (isArragne)
            {

                var listIds = ids.Split(',').ToList();
                foreach (var itemid in listIds)
                {
                    if (!string.IsNullOrEmpty(itemid))
                    {
                        foreach (var itemBase in lstItemBase)
                        {
                            if (itemBase.Id == long.Parse(itemid))
                            {
                                var item = new PRODUCT_FULL()
                                {
                                    Id = itemBase.Id,
                                    Title = itemBase.Title,

                                    ProductCode = itemBase.ProductCode,
                                    IntroText = itemBase.IntroText,
                                    Images = itemBase.Images,
                                    Thumbnail = itemBase.Thumbnail

                                };

                                lstItem.Add(item);
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {
                        Id = itemBase.Id,
                        Title = itemBase.Title,

                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail

                    };

                    lstItem.Add(item);
                }
            }
            return lstItem;
        }



        public List<PRODUCT_FULL> GetAllProducts(int published)
        {
            try
            {
                string keyCache = Constants.CACHE_KEY_ALL_PRODUCTS_BY_PUBLISHED + published;

                var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

                List<PRODUCT_FULL> lstItem = (List<PRODUCT_FULL>)LocalCaching.GetData(keyCache);
                if (lstItem != null && lstItem.Count > 0)
                    return lstItem;

                var lstItemBase = ProductDBBase.Create().GetAllProducts(published);
                if (lstItemBase == null)
                    return null;

                lstItem = new List<PRODUCT_FULL>();
                foreach (var itemBase in lstItemBase)
                {
                    var item = new PRODUCT_FULL()
                    {
                        Id = itemBase.Id,
                        CategoryId = itemBase.CategoryId,
                        Title = itemBase.Title,
                        Name = itemBase.Name,
                        Alias = itemBase.Alias,
                        ProductCode = itemBase.ProductCode,
                        IntroText = itemBase.IntroText,
                        FullText = itemBase.FullText,
                        CategoryPathway = itemBase.CategoryPathway,
                        Images = itemBase.Images,
                        Thumbnail = itemBase.Thumbnail,
                        Price = itemBase.Price,
                        PriceModifyDate = itemBase.PriceModifyDate,
                        Attributes = itemBase.Attributes,
                        CreatedBy = itemBase.CreatedBy,
                        CreatedDate = itemBase.CreatedDate,
                        ModifiedBy = itemBase.ModifiedBy,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Hits = itemBase.Hits,
                        Count = itemBase.Count,
                        Params = itemBase.Params,
                        ManufactoryId = itemBase.ManufactoryId
                    };

                    lstItem.Add(item);
                }

                if (lstItem.Count > 0)
                {
                    LocalCaching.Add(keyCache, lstItem);
                    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }



        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 15/09/2011 09:16 AM
        /// todo: get min price and max price
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public Dictionary<string, double> GetPriceLimitRange(int categoryId)
        {
            var lstProducts = GetAllProductFullsByCategory(categoryId, 1);
            var maxPrice = (from p in lstProducts orderby p.Price descending select p.Price).FirstOrDefault();
            var minPrice = (from p in lstProducts orderby p.Price ascending select p.Price).FirstOrDefault();
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            dictionary.Add("Max", maxPrice);
            dictionary.Add("Min", minPrice);

            return dictionary;
        }


        public List<int> GetListProductIdsByAttributeValue(int attributeId, string[] values)
        {
            try
            {
                // get attribute filter info
                var attribute = new AttributeBO().GetAttributeFull(attributeId);
                var fitlerType = attribute.FilterType;
                var filterDataType = attribute.DataType;
                var fitlerEntity = attribute.FilterEntity;

                var lstProductAttributes = new ProductAttributeBO().GetProductAttributesByAttribute(attributeId);
                List<PRODUCTATTRIBUTE_FULL> _lstProductAttributes = new List<PRODUCTATTRIBUTE_FULL>();

                switch (fitlerType)
                {
                    case (byte)UTILS.Constants.FilterType.ByValue:
                        if (filterDataType == (byte)UTILS.Constants.FilterDataType.String)
                        {
                            _lstProductAttributes = (from p in lstProductAttributes where Utils.FormatTextValue(p.TextValue) == Utils.FormatTextValue(values[0]) select p).ToList();
                        }
                        else if (filterDataType == (byte)UTILS.Constants.FilterDataType.Double)
                        {
                            double numberValue = 0;
                            if (Utils.IsNumber(values[0]))
                                numberValue = Convert.ToDouble(values[0]);
                            _lstProductAttributes = (from p in lstProductAttributes where p.NumbericValue == numberValue select p).ToList();
                        }

                        break;
                    case (byte)UTILS.Constants.FilterType.ByRange:
                        double min = 0;
                        double max = 0;
                        if (Utils.IsNumber(values[0]))
                            min = Convert.ToDouble(values[0]);
                        if (Utils.IsNumber(values[0]))
                            max = Convert.ToDouble(values[1]);
                        if (max < min)
                            break;
                        _lstProductAttributes = (from p in lstProductAttributes where p.NumbericValue >= min && p.NumbericValue < max select p).ToList();
                        break;
                    case (byte)UTILS.Constants.FilterType.ByMultiValue:
                        var listValues = values.ToList();

                        if (filterDataType == (byte)UTILS.Constants.FilterDataType.String)
                        {
                            _lstProductAttributes = (from p in lstProductAttributes where listValues.IndexOf(Utils.FormatTextValue(p.TextValue)) != -1 select p).ToList();
                        }
                        else if (filterDataType == (byte)UTILS.Constants.FilterDataType.Double)
                        {
                            _lstProductAttributes = (from p in lstProductAttributes where listValues.IndexOf(Convert.ToString(p.NumbericValue)) != -1 select p).ToList();
                        }
                        else if (filterDataType == (byte)UTILS.Constants.FilterDataType.Bit)
                        {
                            _lstProductAttributes = (from p in lstProductAttributes where p.NumbericValue == 1 select p).ToList();
                        }

                        break;
                }

                List<int> lstProductIds = new List<int>();

                if (_lstProductAttributes.Count <= 0)
                    return lstProductIds;

                foreach (var _lstProductAttribute in _lstProductAttributes)
                {
                    if (_lstProductAttribute != null)
                        lstProductIds.Add(_lstProductAttribute.ProductId);
                }

                return lstProductIds;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }

        #endregion

        #region DELETE

        public int DeleteProduct(int productId)
        {
            int returnVal;
            // Delete product attribute relationship
            returnVal = ProductAttributeDBBase.Create().DeleteProductAttributeByProductId(productId);
            if (returnVal == -1)
                return -1;

            new ProductAttributeBO().FlushAllProductAttributeCache(string.Empty);

            returnVal = ProductDBBase.Create().DeleteProduct(productId);
            if (returnVal != -1)
                FlushAllProductCache(string.Empty);
            return returnVal;
        }

        public int DeleteProducts(string listId)
        {
            // Delete product attribute relationship
            int returnVal = ProductAttributeDBBase.Create().DeleteProductAttributeByProductId(listId);
            if (returnVal == -1)
                return -1;

            new ProductAttributeBO().FlushAllProductAttributeCache(string.Empty);

            returnVal = ProductDBBase.Create().DeleteProducts(listId);
            if (returnVal != -1)
                FlushAllProductCache(string.Empty);
            return returnVal;
        }

        #endregion

        #region Extend

        public Dictionary<string, string> ParseProductAttributes(string strProductAttributes)
        {
            return Utils.ParseStringJson(strProductAttributes);
        }

        public void FlushAllProductCache(string containKey)
        {
            // remove product cache
            DelegateFlushAllProductCache delegateFlushAllProductCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllProductCache.BeginInvoke(strGroupKeyCached, containKey, null, null);

            // remove product attribute cache
            DelegateFlushAllProductAttributeCache delegateFlushAllProductAttributeCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllProductAttributeCache.BeginInvoke(strGroupKeyProductAttributeCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        public void UpdateCache(PRODUCT_FULL productFull)
        {
            var strKeyCached = Constants.CACHE_KEY_PRODUCT + productFull.Id;
            //LocalCaching.Add ( strKeyCached, productFull );
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, productFull, null, null);
        }

        #endregion

    }
}
