using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Document = DATA.Document;
using Newtonsoft.Json;
namespace BIZ
{
    public class DocumentBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_DOCUMENT;

        protected delegate void DelegateFlushAllDocumentCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        public int CreateUpdateDocument(Document content)
        {
            return DocumentDBBase.Create().CreateUpdateDocument(content);
        }
        public int CreateUpdateDocument(DOCUMENT_FULL documentFull)
        {

            Document content = documentFull.ConvertToBase();
            int returnVal = CreateUpdateDocument(content);
            if (returnVal != -1)
            {
               // UpdateCache(documentFull);
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
             

                return  DocumentDBBase.Create().GetDocument(DocumentId);


            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "DocumentBO", "GetDocument");
                return null;
            }
        }
        public DOCUMENT_FULL GetDocumentFull(int DocumentId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_DOCUMENT + DocumentId;

                //var data = LocalCaching.GetData(strKeyCached);
                //if (data != null)
                //    return JsonConvert.DeserializeObject<DOCUMENT_FULL>(data.ToString());
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<DOCUMENT_FULL>(cachedata.ToString());

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
                    Private = content.Private
                };

                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));
                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "DocumentBO", "GetDocumentFull ,contentId= " + DocumentId);
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

            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(data.ToString());
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
                    Private = content.Private
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
            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_p" + pageSize + "_c" + categoryId+"_s"+status+"_k"+title;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_p" + pageSize + "_c" + categoryId + "_s" + status + "_k" + title + "total";
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
            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //{
            //    totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
            //    return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(data.ToString());
            //}
            var documents = DocumentDBBase.Create().GetDocumentsSearch(title, categoryId, status, pageIndex, pageSize,fromdate,todate, ref totalRecords);
            if (documents == null)
                return null;

            List<DOCUMENT_FULL> albumFulls = new List<DOCUMENT_FULL>();
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
                    Hits = content.Hits,
                    Private = content.Private
                    // Style = content.Style 


                };

                albumFulls.Add(DocumentsFull);
            }
            if (albumFulls.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(albumFulls));
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                //LocalCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                //LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(albumFulls));
                RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
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
            
            var documents = GetDocumentsPaged(title, categoryId, status, pageIndex, pageSize, ref totalRecords);

            if (documents == null)
                return null;
            var albumFulls = new List<DOCUMENT_FULL>();
            foreach (var content in documents)
            {
                DOCUMENT_FULL documentsFull = new DOCUMENT_FULL()
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

                albumFulls.Add(documentsFull);
            }

            return albumFulls.ToList();
        }

        public List<DOCUMENT_FULL> GetPageLastestDoccumentFull(int categoryId, int pageIndex, int pageSize, ref int totalRecords)
        {

            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_DOCUMENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "total";

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //{
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            //    LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            //}
            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //{
            //    totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
            //    return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(data.ToString());
            //}
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<DOCUMENT_FULL>>(cachedata.ToString());
            }
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
            if (lstItem != null && lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
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

        //public void UpdateCache(DOCUMENT_FULL document)
        //{
        //    var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_DOCUMENT + document.Id;
        //    DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
        //    delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(document), null, null);

        //}

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
