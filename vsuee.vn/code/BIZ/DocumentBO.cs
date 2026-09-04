using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;
using Document = DATA.Document;
namespace BIZ
{
    public class DocumentBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_DOCUMENT;

        protected delegate void DelegateFlushAllDocumentCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateDocument(Document Document)
        {
            
            int returnVal = DocumentDBBase.Create().CreateUpdateDocument(Document);
            if (Document.EffectiveDate.GetValueOrDefault().Year > 3000)
            {
                Document.EffectiveDate = null;

            }
            if (Document.ExpiryDate.GetValueOrDefault().Year > 3000)
            {
                Document.ExpiryDate = null;

            }
            if (returnVal != -1)
            {
                //UpdateCache(Document);
                FlushAllDocumentCache(strGroupKeyCached);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Document by Document id
        /// </summary>
        /// <param name="DocumentId">The Document id.</param>
        /// <returns></returns>
        //public Document GetDocument(int DocumentId)
        //{
        //    return DocumentDBBase.Create().GetDocument(DocumentId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Document by id => add to local cache
        /// </summary>
        /// <param name="DocumentId">The Document id.</param>
        /// <returns></returns>
        public Document GetDocument(int DocumentId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_DOCUMENT + DocumentId;
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<Document>(cachedata.ToString());

                var item = DocumentDBBase.Create().GetDocument(DocumentId);
                if (item.EffectiveDate.GetValueOrDefault().Year > 3000)
                {
                    item.EffectiveDate = null;

                }
                if (item.ExpiryDate.GetValueOrDefault().Year > 3000)
                {
                    item.ExpiryDate = null;

                }
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        public DOCUMENT_FULL GetDocumentFull(int DocumentId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_DOCUMENT + DocumentId;

                //var item = (DOCUMENT_FULL)LocalCaching.GetData(strKeyCached);
                //if (item != null)
                //    return item;

                var content = GetDocument(DocumentId);

                var item = new DOCUMENT_FULL
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                    EffectiveDate = content.EffectiveDate,
                    ExpiryDate = content.ExpiryDate,
                    FilePath = content.FilePath,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    SignedBy = content.SignedBy,
                    SignedByDesc = content.SignedByDesc,
                    Hits = content.Hits,
                    //Private = content.Private
                };
                if (content.EffectiveDate.GetValueOrDefault().Year > 3000)
                {
                    item.EffectiveDate = null;

                }
                if (content.ExpiryDate.GetValueOrDefault().Year > 3000)
                {
                    item.ExpiryDate = null;

                }
                //RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        private IEnumerable<Document> GetTopLastestDocuments(int top, int categoryId)
        {
            var result = DocumentDBBase.Create().GetTopLastestDocuments(top, categoryId);
            if (result == null)
                return null;
            return result;
        }
        public List<DOCUMENT_FULL> GetTopLastestDocumentsFull(int top, int categoryId = 0)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + top + "_category" + categoryId;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var lstItem = (List<DOCUMENT_FULL>)LocalCaching.GetData(strKeyCached);

            //if (lstItem != null && lstItem.Count > 0)
            //    return lstItem;
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(cachedata.ToString());
            var lstItemBase = GetTopLastestDocuments(top, categoryId);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<DOCUMENT_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new DOCUMENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryPathway=content.CategoryId
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                    FilePath = content.FilePath,
                   
                    CreatedBy = content.CreatedBy,
                    SignedBy = content.SignedBy,
                    SignedByDesc = content.SignedByDesc,
                    CreatedDate = content.CreatedDate,

                    PublishDate = content.PublishDate,
                    EffectiveDate = content.EffectiveDate,
                    ExpiryDate = content.ExpiryDate,
                    Status = content.Status,
                    Hits = content.Hits,
                    SiteId = content.SiteId,
                    //Private = content.Private
                    // Style = content.Style 

                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
            }

            return lstItem;
        }
        public List<DOCUMENT_FULL> GetDocumentsSearchPaged(string title, int categoryId, int status, int pageIndex, int pageSize, string fromdate, string todate, ref int totalRecords)
        {
            var Documents = DocumentDBBase.Create().GetDocumentsSearch(title, categoryId, status, pageIndex, pageSize,fromdate,todate, ref totalRecords);
            if (Documents == null)
                return null;

            List<DOCUMENT_FULL> albumFulls = new List<DOCUMENT_FULL>();
            foreach (var content in Documents)
            {
                DOCUMENT_FULL DocumentsFull = new DOCUMENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryPathway=content.CategoryId
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                    FilePath = content.FilePath,

                    CreatedBy = content.CreatedBy,
                    SignedBy = content.SignedBy,
                    SignedByDesc = content.SignedByDesc,
                    CreatedDate = content.CreatedDate,

                    PublishDate = content.PublishDate,
                    EffectiveDate = content.EffectiveDate,
                    ExpiryDate = content.ExpiryDate,
                    Status = content.Status,
                    Hits = content.Hits,
                    //Private = content.Private
                    // Style = content.Style 


                };

                albumFulls.Add(DocumentsFull);
            }
            return albumFulls.ToList();
        }
        public List<Document> GetDocumentsPaged(string title, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var Documents = DocumentDBBase.Create().GetDocumentsByFilter(title, categoryId, status, pageIndex, pageSize, ref totalRecords);
            if (Documents == null)
                return null;

            return Documents.ToList();
        }
        public List<DOCUMENT_FULL> GetDocumentsFuLLPaged(string title, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_title" + title + "_status" + status;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_title" + title + "_status" + status + "total";
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(cachedata.ToString());
            }
            var Documents = GetDocumentsPaged(title, categoryId, status, pageIndex, pageSize, ref totalRecords);

            if (Documents == null)
                return null;
            List<DOCUMENT_FULL> albumFulls = new List<DOCUMENT_FULL>();
            foreach (var content in Documents)
            {
                DOCUMENT_FULL DocumentsFull = new DOCUMENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryPathway=content.CategoryId
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                    FilePath = content.FilePath,

                    CreatedBy = content.CreatedBy,
                    SignedBy = content.SignedBy,
                    SignedByDesc = content.SignedByDesc,
                    CreatedDate = content.CreatedDate,

                    PublishDate = content.PublishDate,

                    EffectiveDate = content.EffectiveDate,
                    ExpiryDate = content.ExpiryDate,
                
                    Status = content.Status,
                    Hits = content.Hits
                    // Style = content.Style 


                };
                if (content.EffectiveDate.GetValueOrDefault().Year>3000)
                {
                    DocumentsFull.EffectiveDate = null;
                    
                }
                if (content.ExpiryDate.GetValueOrDefault().Year > 3000)
                {
                    DocumentsFull.ExpiryDate = null;

                }
                albumFulls.Add(DocumentsFull);
            }
            RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(albumFulls));
            RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
            return albumFulls.ToList();
        }

        public List<DOCUMENT_FULL> GetPageLastestDoccumentFull(int categoryId, int pageIndex, int pageSize, ref int totalRecords)
        {

            string strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId;
            string strKeyCachedTotal = strGroupKeyCached + "_"+Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "total";
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(cachedata.ToString());
            }
                
            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //{
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            //    LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            //}
            //var lstItem = (List<DOCUMENT_FULL>)LocalCaching.GetData(strKeyCached);
            ////var lstItem = new List<CATEGORY_FULL> ();
            //if (lstItem != null && lstItem.Count > 0)
            //{
            //    totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
            //    return lstItem;
            //}
            var lstItem = new List<DOCUMENT_FULL>();
            var documents = GetDocumentsPaged(String.Empty, categoryId, 1, pageIndex, pageSize, ref totalRecords);
            if (documents == null)
                return null;

            foreach (var content in documents)
            {
                DOCUMENT_FULL DocumentsFull = new DOCUMENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryPathway=content.CategoryId
                    Name = content.Name,
                    Code = content.Code,
                    Description = content.Description,
                    FilePath = content.FilePath,

                    CreatedBy = content.CreatedBy,
                    SignedBy = content.SignedBy,
                    SignedByDesc = content.SignedByDesc,
                    CreatedDate = content.CreatedDate,

                    PublishDate = content.PublishDate,
                    EffectiveDate = content.EffectiveDate,
                    ExpiryDate = content.ExpiryDate,
                    Status = content.Status,
                    Hits = content.Hits
                    // Style = content.Style 


                };


                lstItem.Add(DocumentsFull);
            }
            if (lstItem.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, lstItem);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }
            return lstItem.ToList();
        }
        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of Documents have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetDocumentsPaged_JSON(string title, int categoryId, int status, int pageIndex, int pageSize)
        {
            string keyCache = strGroupKeyCached + "_" + Constants.CACHE_KEY_DOCUMENTS_PAGED_JSON + "_pageindex" + pageIndex + "_pagesize" + pageSize + "_title" + title + "_categoryId" + categoryId + "_status" + status;
            string groupKeyCache = Constants.CACHE_GROUPKEY_DOCUMENT;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<DOCUMENT_FULL> Documents = GetDocumentsFuLLPaged(title, categoryId, status, pageIndex, pageSize, ref totalRecords);

            if (Documents == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(Documents, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
            }

            return json;
        }








        #endregion

        #region UPDATE
        public int ViewAdd(long id)
        {

            var returnVal = DocumentDBBase.Create().ViewAdd(id);
            
            return returnVal;

        }
        

        #endregion

        #region DELETE

        public int DeleteDocuments(string listIds)
        {
            var returnVal = DocumentDBBase.Create().DeleteDocuments(listIds);
            if (returnVal != -1)
                FlushAllDocumentCache(strGroupKeyCached);
            return returnVal;
        }

        public int DeleteDocument(int id)
        {
            var returnVal = DocumentDBBase.Create().DeleteDocument(id);
            if (returnVal != -1)
                FlushAllDocumentCache(strGroupKeyCached);
            return returnVal;
        }

        #endregion

        public void FlushAllDocumentCache(string containKey)
        {
            //DelegateFlushAllDocumentCache delegateFlushAllDocumentCache = LocalCaching.RemoveContainKeyInGroupKey;
            //delegateFlushAllDocumentCache.BeginInvoke(strGroupKeyCached, containKey, null, null);

            RedisCaching.RemoveGroup(containKey);
        }

    }
}
