using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Attribute = DATA.Attribute;
namespace BIZ
{
    public class AttributeBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_ATTRIBUTE;
        readonly string strGroupKeyProductAttributeCached = "Constants.CACHE_GROUPKEY_PRODUCTATTRIBUTE";

        protected delegate void DelegateFlushAllAttributeCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateFlushAllProductAttributeCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        public int CreateUpdateAttribute(Attribute attribute)
        {
            return AttributeDBBase.Create().CreateUpdateAttribute(attribute);
        }

        public int CreateUpdateAttribute(ATTRIBUTE_FULL attributeFull)
        {
            Attribute attribute = attributeFull.ConvertToBase();
            int returnVal = CreateUpdateAttribute(attribute);
            if (returnVal != -1)
            {
                UpdateCache(attributeFull);
                FlushAllAttributeCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get attribute by attribute id
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        /// <returns></returns>
        public Attribute GetAttribute(int attributeId)
        {
            return AttributeDBBase.Create().GetAttribute(attributeId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get attribute by id => add to local cache
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        /// <returns></returns>
        public ATTRIBUTE_FULL GetAttributeFull(int attributeId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ATTRIBUTE + attributeId;

                var item = (ATTRIBUTE_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = GetAttribute(attributeId);

                item = new ATTRIBUTE_FULL
                {
                    Id = itemBase.Id,
                    GroupId = itemBase.GroupId,
                    CategoryId = itemBase.CategoryId,
                    Title = itemBase.Title,
                    FilterType = itemBase.FilterType,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,
                    Params = itemBase.Params,
                    DataType = itemBase.DataType,
                    Unit = itemBase.Unit,
                    Filter = itemBase.Filter
                };

                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "AttributeBO", "GetAttributeFull");
                return null;
            }
        }

        public List<Attribute> GetAllAttributesPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var attributes = AttributeDBBase.Create().GetAllAttributesPaged(pageIndex, pageSize, ref totalRecords);
            if (attributes == null)
                return null;

            return attributes.ToList();
        }

        public List<ATTRIBUTE_FULL> GetAllAttributeFullsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var attributes = GetAllAttributesPaged(pageIndex, pageSize, ref totalRecords);
            if (attributes == null)
                return null;

            List<ATTRIBUTE_FULL> attributeFulls = new List<ATTRIBUTE_FULL>();
            foreach (var attribute in attributes)
            {
                ATTRIBUTE_FULL attributeFull = new ATTRIBUTE_FULL()
                                               {

                                                   Id = attribute.Id,
                                                   GroupId = attribute.GroupId,
                                                   CategoryId = attribute.CategoryId,
                                                   Title = attribute.Title,
                                                   FilterType = attribute.FilterType,
                                                   Published = attribute.Published,
                                                   Ordering = attribute.Ordering,
                                                   Params = attribute.Params,
                                                   DataType = attribute.DataType,
                                                   Unit = attribute.Unit,
                                                   Filter = attribute.Filter
                                               };

                attributeFulls.Add(attributeFull);
            }

            return attributeFulls;

        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of attributes have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllAttributesPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALL_ATTRIBUTES_PAGED_JSON + pageIndex + pageSize;
            string groupKeyCache = Constants.CACHE_GROUPKEY_ATTRIBUTE;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<ATTRIBUTE_FULL> attributes = GetAllAttributeFullsPaged(pageIndex, pageSize, ref totalRecords);

            if (attributes == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(attributes, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
            }

            return json;
        }

        public List<Attribute> FilterAttributes(string title, int categoryId, int groupId)
        {
            var attributes = AttributeDBBase.Create().GetAllAttributes(title, categoryId, groupId);
            if (attributes == null)
                return null;
            return attributes.ToList();
        }

        public List<ATTRIBUTE_FULL> FilterAttributeFulls(string title, int categoryId, int groupId)
        {
            var attributes = FilterAttributes(title, categoryId, groupId);
            if (attributes == null)
                return null;
            List<ATTRIBUTE_FULL> lstAttributeFulls = new List<ATTRIBUTE_FULL>();
            foreach (var attribute in attributes)
            {
                ATTRIBUTE_FULL attributeFull = new ATTRIBUTE_FULL()
                                               {

                                                   Id = attribute.Id,
                                                   GroupId = attribute.GroupId,
                                                   CategoryId = attribute.CategoryId,
                                                   Title = attribute.Title,
                                                   FilterType = attribute.FilterType,
                                                   Published = attribute.Published,
                                                   Ordering = attribute.Ordering,
                                                   Params = attribute.Params,
                                                   DataType = attribute.DataType,
                                                   Unit = attribute.Unit,
                                                   Filter = attribute.Filter
                                               };

                lstAttributeFulls.Add(attributeFull);
            }

            return lstAttributeFulls;
        }

        public List<ATTRIBUTE_FULL> GetAllAttributesByCategory(int categoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_ATTRIBUTES_BYCATEGORY + categoryId;
            string groupKeyCache = Constants.CACHE_GROUPKEY_ATTRIBUTE;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var lstCachedAttributes = (List<ATTRIBUTE_FULL>)LocalCaching.GetData(keyCache);

            if (lstCachedAttributes != null)
                return lstCachedAttributes;

            var attributes = FilterAttributes(string.Empty, categoryId, 0);

            if (attributes == null)
                return null;

            var publishedAttributes = (from p in attributes where p.Published == 1 select p).ToList();

            lstCachedAttributes = new List<ATTRIBUTE_FULL>();
            foreach (var attribute in publishedAttributes)
            {
                ATTRIBUTE_FULL attributeFull = new ATTRIBUTE_FULL()
                {
                    Id = attribute.Id,
                    GroupId = attribute.GroupId,
                    CategoryId = attribute.CategoryId,
                    Title = attribute.Title,
                    FilterType = attribute.FilterType,
                    Published = attribute.Published,
                    Ordering = attribute.Ordering,
                    Params = attribute.Params,
                    DataType = attribute.DataType,
                    Unit = attribute.Unit,
                    Filter = attribute.Filter
                };

                lstCachedAttributes.Add(attributeFull);
            }

            if (lstCachedAttributes.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedAttributes);
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
            }

            return lstCachedAttributes;
        }

        public List<ATTRIBUTE_FULL> GetAllAttributesToFilter(int categoryId)
        {
            var attributes = GetAllAttributesByCategory(categoryId);
            var _attributes = (from p in attributes where p.FilterType != (int)UTILS.Constants.FilterType.NoFilter && p.DataType != (int)UTILS.Constants.FilterDataType.Bit select p).ToList();
            return _attributes;
        }

        public List<ATTRIBUTE_FULL> GetAllAttributesToFilter(int categoryId, int filterDataType)
        {
            var attributes = GetAllAttributesByCategory(categoryId);
            var _attributes = (from p in attributes where p.FilterType != (int)UTILS.Constants.FilterType.NoFilter && p.DataType == (int)UTILS.Constants.FilterDataType.Bit select p).ToList();
            return _attributes;
        }

        #endregion

        #region UPDATE

        public void UpdateCache(ATTRIBUTE_FULL attributeFull)
        {
            var strKeyCached = Constants.CACHE_KEY_ATTRIBUTE + attributeFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, attributeFull, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteAttributes(string listIds)
        {
            var returnVal = AttributeDBBase.Create().DeleteAttributes(listIds);
            if (returnVal != -1)
                FlushAllAttributeCache(string.Empty);
            return returnVal;
        }

        public int DeleteAttribute(int id)
        {
            var returnVal = AttributeDBBase.Create().DeleteAttribute(id);
            if (returnVal != -1)
                FlushAllAttributeCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllAttributeCache(string containKey)
        {
            DelegateFlushAllAttributeCache delegateFlushAllAttributeCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllAttributeCache.BeginInvoke(strGroupKeyCached, containKey, null, null);

            DelegateFlushAllProductAttributeCache delegateFlushAllProductAttributeCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllProductAttributeCache.BeginInvoke(strGroupKeyProductAttributeCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

    }
}
