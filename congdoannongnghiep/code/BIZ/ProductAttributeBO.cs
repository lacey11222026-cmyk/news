using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using ProductAttribute = DATA.ProductAttribute;

namespace BIZ
{
    public class ProductAttributeBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_PRODUCTATTRIBUTE;
        protected delegate void DelegateFlushAllProductAttributeCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);


        #region CREATE UPDATE

        public int CreateUpdateProductAttribute(ProductAttribute productAttribute)
        {
            return ProductAttributeDBBase.Create().CreateUpdateProductAttribute(productAttribute);
        }

        public int CreateUpdateProductAttribute(PRODUCTATTRIBUTE_FULL productAttributeFull)
        {
            ProductAttribute productAttribute = productAttributeFull.ConvertToBase();
            int returnValue = CreateUpdateProductAttribute(productAttribute);
            if (returnValue != -1)
            {
                UpdateCache(productAttributeFull);
                FlushAllProductAttributeCache(string.Empty);
            }

            return returnValue;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get productAttribute by productAttribute id
        /// </summary>
        /// <param name="productAttributeId">The productAttribute id.</param>
        /// <returns></returns>
        public ProductAttribute GetProductAttribute(int productAttributeId)
        {
            return ProductAttributeDBBase.Create().GetProductAttribute(productAttributeId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get productAttribute by id => add to local cache
        /// </summary>
        /// <param name="productAttributeId">The productAttribute id.</param>
        /// <returns></returns>
        public PRODUCTATTRIBUTE_FULL GetProductAttributeFull(int productAttributeId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_PRODUCTATTRIBUTE + productAttributeId;

                var item = (PRODUCTATTRIBUTE_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = GetProductAttribute(productAttributeId);

                item = new PRODUCTATTRIBUTE_FULL
                {
                    Id = itemBase.Id,
                    AttributeId = itemBase.AttributeId,
                    ProductId = itemBase.ProductId,
                    NumbericValue = itemBase.NumbericValue,
                    TextValue = itemBase.TextValue,
                    Ordering = itemBase.Ordering,
                    Params = itemBase.Params
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

        public List<PRODUCTATTRIBUTE_FULL> GetProductAttributesByAttribute(int attributeId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ALL_PRODUCTATTRIBUTES_BY_ATTRIBUTE + attributeId;

                var lstItems = (List<PRODUCTATTRIBUTE_FULL>)LocalCaching.GetData(strKeyCached);
                if (lstItems != null)
                    return lstItems;

                var lstItemBases = ProductAttributeDBBase.Create().GetAllProductAttributes(attributeId);

                lstItems = new List<PRODUCTATTRIBUTE_FULL>();
                foreach (var itemBase in lstItemBases)
                {
                    PRODUCTATTRIBUTE_FULL item = new PRODUCTATTRIBUTE_FULL()
                    {
                        Id = itemBase.Id,
                        AttributeId = itemBase.AttributeId,
                        ProductId = itemBase.ProductId,
                        NumbericValue = itemBase.NumbericValue,
                        TextValue = itemBase.TextValue,
                        Ordering = itemBase.Ordering,
                        Params = itemBase.Params
                    };

                    lstItems.Add(item);

                }

                if (lstItems.Count > 0)
                    LocalCaching.Add(strKeyCached, lstItems);

                return lstItems;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }

        }

        public Dictionary<string, double> GetNumbericValueLimitRange(int attributeId)
        {
            var lstProductAttributes = GetProductAttributesByAttribute(attributeId);
            var max = (from p in lstProductAttributes orderby p.NumbericValue descending select p.NumbericValue).FirstOrDefault();
            var min = (from p in lstProductAttributes orderby p.NumbericValue ascending select p.NumbericValue).FirstOrDefault();
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            dictionary.Add("Max", Convert.ToDouble(max));
            dictionary.Add("Min", Convert.ToDouble(min));
            return dictionary;
        }

        public List<PRODUCTATTRIBUTE_FULL> GetProductAttributesByProduct(int productId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ALL_PRODUCTATTRIBUTES_BY_PRODUCT + productId;

                var lstItems = (List<PRODUCTATTRIBUTE_FULL>)LocalCaching.GetData(strKeyCached);
                if (lstItems != null)
                    return lstItems;

                var lstItemBases = ProductAttributeDBBase.Create().GetAllProductAttributesByProduct(productId);

                lstItems = new List<PRODUCTATTRIBUTE_FULL>();
                foreach (var itemBase in lstItemBases)
                {
                    PRODUCTATTRIBUTE_FULL item = new PRODUCTATTRIBUTE_FULL()
                    {
                        Id = itemBase.Id,
                        AttributeId = itemBase.AttributeId,
                        ProductId = itemBase.ProductId,
                        NumbericValue = itemBase.NumbericValue,
                        TextValue = itemBase.TextValue,
                        Ordering = itemBase.Ordering,
                        Params = itemBase.Params
                    };

                    lstItems.Add(item);

                }

                if (lstItems.Count > 0)
                    LocalCaching.Add(strKeyCached, lstItems);

                return lstItems;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }

        }

        #endregion

        #region DELETE

        public int DeleteProductAttribute(int productAttributeId)
        {
            var returnVal = ProductAttributeDBBase.Create().DeleteProductAttribute(productAttributeId);
            if (returnVal != -1)
                FlushAllProductAttributeCache(string.Empty);
            return returnVal;
        }

        #endregion

        #region Extend

        public void FlushAllProductAttributeCache(string containKey)
        {
            DelegateFlushAllProductAttributeCache delegateFlushAllProductAttributeCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllProductAttributeCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
        }

        public void UpdateCache(PRODUCTATTRIBUTE_FULL productAttributeFull)
        {
            var strKeyCached = Constants.CACHE_KEY_PRODUCTATTRIBUTE + productAttributeFull.Id;
            //LocalCaching.Add ( strKeyCached, productAttributeFull );
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, productAttributeFull, null, null);
        }

        #endregion

    }
}
