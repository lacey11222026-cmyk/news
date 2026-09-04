using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;

namespace BIZ
{
    public class CategoryBO
    {
        string strGroupKeyCached = Constants.CACHE_GROUPKEY_CATEGORY;
        protected delegate void DelegateFlushAllCategoryCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        public int CreateUpdateCategory(Category category)
        {
            return CategoryDBBase.Create().CreateUpdateCategory(category);
        }
        public int UpdateContent(Category itemBase)
        {

            var categoryFull = new CATEGORY_FULL
            {
                Id = itemBase.Id,
                ParentId = itemBase.ParentId,
                Pathway = itemBase.Pathway,
                Image = itemBase.Image,
                Name = itemBase.Name,
                Link = itemBase.Link,
                Description = itemBase.Description,
                Contents = itemBase.Contents,
                CreateDate = itemBase.CreateDate,
                ModifiedDate = itemBase.ModifiedDate,
                Published = itemBase.Published,
                Ordering = itemBase.Ordering,
                Language = itemBase.Language,
                Params = itemBase.Params,
                Type = itemBase.Type
            };
            var result = CategoryDBBase.Create().UpdateContent(itemBase.Id, itemBase.Contents, itemBase.Published, itemBase.Ordering, itemBase.Params);
            if (result != -1)
            {
                //UpdateCache(categoryFull);
                //FlushAllCategoryCache(string.Empty);
                FlushAllCategoryCache(strGroupKeyCached);
            }
            return result;
        }
        public void FlushAllCategoryCache(string containKey)
        {
            // remove product cache
            RedisCaching.RemoveGroup(containKey);

        }
        public int CreateUpdateCategory(CATEGORY_FULL categoryFull)
        {
            Category category = categoryFull.ConvertToBase();

            var result = CreateUpdateCategory(category);
            if (result != -1)
            {
                //UpdateCache(categoryFull);
                //FlushAllCategoryCache(string.Empty);
                FlushAllCategoryCache(strGroupKeyCached);
            }
            return result;
        }

        #endregion

        #region READ
        public List<CATEGORY_FULL> GetCategoryByUserName(List<CATEGORY_FULL> staticCategoryList, string username, bool isAdmin)
        {
            if (staticCategoryList == null)
                return null;
            var lstcate = new List<CATEGORY_FULL>();
            lstcate.AddRange(staticCategoryList);
            var listcategory = new List<CATEGORY_FULL>();

            if (isAdmin)
            {

                foreach (var item in lstcate)
                {
                    if (item.ParentId > 0)
                    {
                        var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name, Pathway = item.Pathway, Published = item.Published };
                        if (item.NodeLevel != 0)
                        {
                            for (var i = 1; i < item.NodeLevel; i++)
                            {
                                x1.Name = "-+ " + x1.Name;
                            }
                        }

                        var pindex = listcategory.Select((Value, Index) => new { Value, Index }).FirstOrDefault(x => x.Value.Id == x1.ParentId);

                        if (pindex != null)
                        {
                            listcategory.Insert(pindex.Index + 1, x1);

                        }
                        else
                        {
                            listcategory.Add(item);
                        }
                    }
                    else
                    {
                        listcategory.Add(item);
                    }
                }
            }
            else
            {
                var usercategoryobj = new PublisherCategoryBO().GetByUserName(username);

                if (!string.IsNullOrEmpty(usercategoryobj.CategoryPath) && usercategoryobj.CategoryPath!=",")
                {
                    foreach (var item in lstcate)
                    {

                        //thằng con thì ko check
                        if (item.ParentId != 0 && item.ParentId != 4)
                        {
                            //if (usercategoryobj.CategoryPath.Contains("," + item.Id + ","))
                            //{
                            var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name, Pathway = item.Pathway, Published = item.Published };
                            if (item.NodeLevel != 0)
                            {
                                for (var i = 1; i < item.NodeLevel; i++)
                                {
                                    x1.Name = "-+ " + x1.Name;
                                }
                            }

                            var pindex = listcategory.Select((Value, Index) => new { Value, Index }).FirstOrDefault(x => x.Value.Id == x1.ParentId);

                            if (pindex != null)
                            {
                                listcategory.Insert(pindex.Index + 1, x1);

                            }
                            //}
                        }
                        else
                        {
                            if (usercategoryobj.CategoryPath.Contains("," + item.Id + ","))

                                listcategory.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in lstcate)
                    {
                        if (item.ParentId > 0)
                        {
                            var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name, Pathway = item.Pathway, Published = item.Published };
                            if (item.NodeLevel != 0)
                            {
                                for (var i = 1; i < item.NodeLevel; i++)
                                {
                                    x1.Name = "-+ " + x1.Name;
                                }
                            }

                            var pindex = listcategory.Select((Value, Index) => new { Value, Index }).FirstOrDefault(x => x.Value.Id == x1.ParentId);
                            if (pindex != null)
                            {
                                listcategory.Insert(pindex.Index + 1, x1);

                            }
                        }
                        else
                        {
                            listcategory.Add(item);
                        }
                    }
                }


            }
            return listcategory;
        }
        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get category by category id
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public Category GetCategory(int categoryId)
        {
            return CategoryDBBase.Create().GetCategory(categoryId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get category by id => add to local cache
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public CATEGORY_FULL GetCategoryFull(int categoryId)
        {
            try
            {
                if (categoryId == 0) return null;
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_CATEGORY + categoryId;

                //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                //if (listGroupKey == null)
                //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<CATEGORY_FULL>(cachedata.ToString());

                //var item = (CATEGORY_FULL)LocalCaching.GetData(strKeyCached);
                //if (item != null)
                //    return item;

                var itemBase = GetCategory(categoryId);

                var item = new CATEGORY_FULL
                {
                    Id = itemBase.Id,
                    ParentId = itemBase.ParentId,
                    Pathway = itemBase.Pathway,
                    Image = itemBase.Image,
                    Name = itemBase.Name,
                    Link = itemBase.Link,
                    Description = itemBase.Description,
                    Contents = itemBase.Contents,
                    CreateDate = itemBase.CreateDate,
                    ModifiedDate = itemBase.ModifiedDate,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,
                    Language = itemBase.Language,
                    Params = itemBase.Params,
                    Type = itemBase.Type
                };

                //LocalCaching.Add(strKeyCached, item);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));
                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public string GetCategoryFull_Json(int categoryId)
        {
            var categoryFull = GetCategoryFull(categoryId);

            if (categoryFull == null)
                return null;

            return UTILS.Utils.ConvertToJson(GetCategoryFull(categoryId), string.Empty);
        }

        public List<Category> GetAllCategories(int categoryType)
        {
            var listCategory = CategoryDBBase.Create().GetCategories(categoryType);

            if (listCategory == null)
                return null;

            return listCategory.ToList();
        }
        public static string fgetcontent(string content, int isgetcontent)
        {
            if (isgetcontent == 1)
                return content;
            return String.Empty;
        }
        public List<CATEGORY_FULL> GetAllCategoriesFull(UTILS.Constants.CategoryType categoryType)
        {
            try
            {

                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_ALL_CATEGORIES + (int)categoryType;

                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<List<CATEGORY_FULL>>(cachedata.ToString());

                //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                //if (listGroupKey == null)
                //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

                //var lstItem = (List<CATEGORY_FULL>)LocalCaching.GetData(strKeyCached);
                ////var lstItem = new List<CATEGORY_FULL> ();
                //if (lstItem != null && lstItem.Count > 0)
                //    return lstItem;

                var lstItemBase = GetAllCategories((int)categoryType);
                // reorder 
                var lstItemBase_temp = from p in lstItemBase orderby p.ParentId, p.Ordering ascending select p;
                lstItemBase = lstItemBase_temp.ToList();

                if (lstItemBase.Count == 0)
                    return null;

                var lstItem = new List<CATEGORY_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new CATEGORY_FULL()
                    {
                        Id = itemBase.Id,
                        ParentId = itemBase.ParentId,
                        Pathway = itemBase.Pathway,

                        Name = itemBase.Name,
                        Language = itemBase.Language,
                        Description = itemBase.Description,
                        Contents = itemBase.Contents,
                        CreateDate = itemBase.CreateDate,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,

                        Params = itemBase.Params,
                        Type = itemBase.Type
                    };

                    lstItem.Add(item);
                }

                if (lstItem.Count > 0)
                {
                    //LocalCaching.Add(strKeyCached, lstItem);
                    RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                    //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public List<Category> GetAllCategoriesPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var listCategory = CategoryDBBase.Create().GetAllCategoriesPaged(pageIndex, pageSize, ref totalRecords);

            if (listCategory == null)
                return null;

            return listCategory.ToList();
        }

        public List<CATEGORY_FULL> GetAllCategoriesFullPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_ALL_CATEGORIES_PAGED + pageIndex + pageSize;

                //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                //if (listGroupKey == null)
                //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

                //var lstItem = (List<CATEGORY_FULL>)LocalCaching.GetData(strKeyCached);
                ////var lstItem = new List<CATEGORY_FULL> ();
                //if (lstItem != null && lstItem.Count > 0)
                //    return lstItem;
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<List<CATEGORY_FULL>>(cachedata.ToString());

                var lstItemBase = GetAllCategoriesPaged(pageIndex, pageSize, ref totalRecords);
                if (lstItemBase == null || lstItemBase.Count == 0)
                    return null;

                var lstItem = new List<CATEGORY_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new CATEGORY_FULL()
                    {
                        Id = itemBase.Id,
                        ParentId = itemBase.ParentId,
                        Pathway = itemBase.Pathway,

                        Name = itemBase.Name,
                        Link = itemBase.Link,
                        Description = itemBase.Description,

                        CreateDate = itemBase.CreateDate,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,
                        Language = itemBase.Language,
                        Params = itemBase.Params,
                        Type = itemBase.Type
                    };

                    lstItem.Add(item);
                }

                if (lstItem.Count > 0)
                {
                    //LocalCaching.Add(strKeyCached, lstItem);
                    //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                    RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                }

                return lstItem;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        public string GetAllCategoriesFullPaged_Json(int pageIndex, int pageSize)
        {
            var totalRecords = 0;
            var lstCategoryFull = GetAllCategoriesFullPaged(pageIndex, pageSize, ref totalRecords);
            if (lstCategoryFull == null || lstCategoryFull.Count == 0)
                return null;
            return Utils.ConvertToJson(lstCategoryFull, string.Empty);
        }

        public List<CATEGORY_FULL> GetAllChildCategories(int categoryId, int numberTake, bool isRandom)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_ALL_CHILDCATEGORIES + categoryId + "_numbertak" + numberTake + "_" + isRandom.ToString();

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var lstItem = (List<CATEGORY_FULL>)LocalCaching.GetData(strKeyCached);
            var cachedata = RedisCaching.GetData(strKeyCached);
            var lstItem = new List<CATEGORY_FULL>();
            if (cachedata != null)
                lstItem = JsonConvert.DeserializeObject<List<CATEGORY_FULL>>(cachedata.ToString());
            //var lstItem = new List<CATEGORY_FULL> ();
            IEnumerable<CATEGORY_FULL> lstItemBase_temp = lstItem;
            Random rand;
            if (lstItem != null && lstItem.Count > 0)
            {
                if (isRandom)
                {
                    rand = new Random();
                    // random and take 3 category
                    lstItemBase_temp = (from p in lstItem orderby rand.Next() select p);
                }

                if (numberTake > 0)
                {
                    lstItemBase_temp = lstItemBase_temp.Take(numberTake);
                }

                if (lstItemBase_temp == null)
                    return lstItem;

                lstItem = lstItemBase_temp.ToList();

                return lstItem;
            }

            var lstItemBase = CategoryDBBase.Create().GetAllChildCategories(categoryId);

            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<CATEGORY_FULL>();

            foreach (var itemBase in lstItemBase)
            {
                var item = new CATEGORY_FULL()
                {
                    Id = itemBase.Id,
                    ParentId = itemBase.ParentId,
                    Pathway = itemBase.Pathway,

                    Name = itemBase.Name,
                    Link = itemBase.Link,
                    Description = itemBase.Description,

                    CreateDate = itemBase.CreateDate,
                    ModifiedDate = itemBase.ModifiedDate,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,

                    Params = itemBase.Params,
                    Language = itemBase.Language,
                    Type = itemBase.Type
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, lstItem);
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
            }

            lstItemBase_temp = lstItem;

            if (isRandom)
            {
                rand = new Random();
                // random and take 3 category
                lstItemBase_temp = (from p in lstItem orderby rand.Next() select p);
            }

            if (numberTake > 0)
            {
                lstItemBase_temp = lstItemBase_temp.Take(numberTake);
            }

            if (lstItemBase_temp == null)
                return lstItem;

            lstItem = lstItemBase_temp.ToList();

            return lstItem;


        }

        public List<Category> GetAllRootCategories()
        {
            var lstRootCategory = CategoryDBBase.Create().GetAllRootCategories();

            if (lstRootCategory == null)
                return null;

            return lstRootCategory.ToList();
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 22/08/2011 10:17 PM
        /// todo: get level of category node by parse pathway, 0 is root, 
        /// </summary>
        /// <param name="pathway">The pathway.</param>
        /// <returns></returns>
        public int GetNodeLevel(string pathway)
        {
            try
            {
                if (string.IsNullOrEmpty(pathway))
                    return 0;

                var _pathWay = FormatPathway(pathway);
                if (_pathWay == "0")
                    return 0;

                if (!string.IsNullOrEmpty(pathway))
                {
                    string[] nodes = _pathWay.Split(',');
                    return nodes.Count() - 2;
                }

                return 0;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return 0;
            }
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 22/08/2011 11:31 PM
        /// todo: get parentid pathway 
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public string GetPathway(int categoryId)
        {
            if (categoryId == 0)
                return string.Empty;

            var category = GetCategoryFull(categoryId);
            if (category == null)
                return string.Empty;
            // reformat pathway
            var pathway = FormatPathway(category.Pathway);

            return pathway;
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/09/2011 12:18 AM
        /// todo: get list category by list categoryid split from pathway (using for category breadcrumb modules)
        /// </summary>
        public List<CATEGORY_FULL> GetAllCategoriesByPathway(int categoryId)
        {
            List<CATEGORY_FULL> lstCategoryFulls = new List<CATEGORY_FULL>();

            string pathWay = GetPathway(categoryId);
            // concat current categoryId
            if (string.IsNullOrEmpty(pathWay))
                pathWay = categoryId.ToString();


            string[] arrCategoryId = pathWay.Split(',');

            foreach (var _categoryId in arrCategoryId)
            {
                if (Utils.IsNumber(_categoryId))
                {
                    CATEGORY_FULL categoryFull = GetCategoryFull(Convert.ToInt32(_categoryId));
                    if (categoryFull != null)
                        lstCategoryFulls.Add(categoryFull);
                }
            }

            return lstCategoryFulls;
        }



        public int GetParentId(int categoryId)
        {
            if (categoryId == 0)
                return -1;

            var category = GetCategoryFull(categoryId);
            if (category == null)
                return -1;

            // reformat pathway
            var parentId = category.ParentId;
            return int.Parse(parentId.ToString());
        }



        public string GetTitle(int categoryId)
        {
            if (categoryId == 0)
                return string.Empty;

            var category = GetCategoryFull(categoryId);
            if (category == null)
                return string.Empty;

            // reformat pathway
            return category.Name;
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/09/2011 02:20 PM
        /// todo: get all categories , which is allowed to show in homepage
        /// </summary>
        /// <returns></returns>
        public List<Category> GetAllHomepageCategories()
        {
            var result = CategoryDBBase.Create().GetAllCategoriesByPosition((byte)UTILS.Constants.CategoryPosition.Homepage);
            if (result == null)
                return null;

            return result.ToList();
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/09/2011 02:30 PM
        /// todo: this function is used to get data from cache with key , if dont have in cache => get from database and set data to cache with key
        /// </summary>        
        /// <returns></returns>
        public List<CATEGORY_FULL> GetAllHomepageCategoryFulls()
        {
            try
            {

                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_ALL_HOMEPAGE_CATEGORIES;

                //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                //if (listGroupKey == null)
                //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                var cachedata = RedisCaching.GetData(strKeyCached);
                var lstItem = new List<CATEGORY_FULL>();
                if (cachedata != null)
                    lstItem = JsonConvert.DeserializeObject<List<CATEGORY_FULL>>(cachedata.ToString());
                //var lstItem = (List<CATEGORY_FULL>)LocalCaching.GetData(strKeyCached);
                //var lstItem = new List<CATEGORY_FULL> ();
                IEnumerable<CATEGORY_FULL> lstItemBase_temp;
                Random rand;
                if (lstItem != null && lstItem.Count > 0)
                {
                    rand = new Random();
                    // random and take 3 category
                    lstItemBase_temp = (from p in lstItem orderby rand.Next() select p).Take(3);

                    if (lstItemBase_temp == null)
                        return lstItem;

                    lstItem = lstItemBase_temp.ToList();
                    return lstItem;
                }

                var lstItemBase = GetAllHomepageCategories();


                if (lstItemBase.Count == 0)
                    return null;

                lstItem = new List<CATEGORY_FULL>();

                foreach (var itemBase in lstItemBase)
                {
                    var item = new CATEGORY_FULL()
                    {
                        Id = itemBase.Id,
                        ParentId = itemBase.ParentId,
                        Pathway = itemBase.Pathway,

                        Name = itemBase.Name,
                        Link = itemBase.Link,
                        Description = itemBase.Description,

                        CreateDate = itemBase.CreateDate,
                        ModifiedDate = itemBase.ModifiedDate,
                        Published = itemBase.Published,
                        Ordering = itemBase.Ordering,

                        Params = itemBase.Params,
                        Type = itemBase.Type
                    };

                    lstItem.Add(item);
                }

                if (lstItem.Count > 0)
                {
                    //LocalCaching.Add(strKeyCached, lstItem);
                    //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                    RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                }

                rand = new Random();
                // random and take 3 category
                lstItemBase_temp = (from p in lstItem orderby rand.Next() select p).Take(3);
                if (lstItemBase_temp == null)
                    return lstItem;
                lstItem = lstItemBase_temp.ToList();
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
        /// date: 08/09/2011 02:30 PM
        /// todo: this function is used to get data from cache with key , if dont have in cache => get from database and set data to cache with key
        /// </summary>        
        /// <returns></returns>
        public List<CATEGORY_FULL> GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition categoryPosition, int numberTake, bool isRandom)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_ALL_CATEGORIES_BY_POSITION + (byte)categoryPosition;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var lstItem = (List<CATEGORY_FULL>)LocalCaching.GetData(strKeyCached);
            var cachedata = RedisCaching.GetData(strKeyCached);
            var lstItem = new List<CATEGORY_FULL>();
            if (cachedata != null)
                lstItem = JsonConvert.DeserializeObject<List<CATEGORY_FULL>>(cachedata.ToString());
            //var lstItem = new List<CATEGORY_FULL> ();
            IEnumerable<CATEGORY_FULL> lstItemBase_temp = lstItem;
            Random rand;
            if (lstItem != null && lstItem.Count > 0)
            {
                if (isRandom)
                {
                    rand = new Random();
                    // random and take 3 category
                    lstItemBase_temp = (from p in lstItem orderby rand.Next() select p);
                }

                if (numberTake > 0)
                {
                    lstItemBase_temp = lstItemBase_temp.Take(numberTake);
                }

                if (lstItemBase_temp == null)
                    return lstItem;

                lstItem = lstItemBase_temp.ToList();

                return lstItem;
            }

            var lstItemBase = CategoryDBBase.Create().GetAllCategoriesByPosition((byte)categoryPosition);

            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<CATEGORY_FULL>();

            foreach (var itemBase in lstItemBase)
            {
                var item = new CATEGORY_FULL()
                {
                    Id = itemBase.Id,
                    ParentId = itemBase.ParentId,
                    Pathway = itemBase.Pathway,
                    Name = itemBase.Name,
                    Link = itemBase.Link,
                    Description = itemBase.Description,
                    CreateDate = itemBase.CreateDate,
                    ModifiedDate = itemBase.ModifiedDate,
                    Published = itemBase.Published,
                    Ordering = itemBase.Ordering,
                    Language = itemBase.Language,
                    Params = itemBase.Params,
                    Type = itemBase.Type
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, lstItem);
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
            }

            lstItemBase_temp = lstItem;

            if (isRandom)
            {
                rand = new Random();
                // random and take 3 category
                lstItemBase_temp = (from p in lstItem orderby rand.Next() select p);
            }

            if (numberTake > 0)
            {
                lstItemBase_temp = lstItemBase_temp.Take(numberTake);
            }

            if (lstItemBase_temp == null)
                return lstItem;

            lstItem = lstItemBase_temp.ToList();

            return lstItem;
        }


        #endregion

        #region UPDATE

        public void UpdateCache(CATEGORY_FULL categoryFull)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_CATEGORY + categoryFull.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, categoryFull, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteCategory(int categoryId)
        {
            var result = CategoryDBBase.Create().DeleteCategory(categoryId);
            if (result != -1)
                FlushAllCategoryCache(strGroupKeyCached);
            return result;
        }

        public int DeleteCategories(string listId)
        {
            var result = CategoryDBBase.Create().DeleteCategories(listId);
            if (result != -1)
                FlushAllCategoryCache(strGroupKeyCached);
            return result;
        }

        #endregion

        public static string FormatPathway(string pathway)
        {
            if (string.IsNullOrEmpty(pathway))
                return pathway;
            var _pathway = pathway.Replace("0/", "");
            _pathway = _pathway.TrimStart('/').TrimEnd('/').Trim();
            return _pathway;
        }


        //public void FlushAllCategoryCache(string containKey)
        //{
        //    //DelegateFlushAllCategoryCache delegateFlushAllCategoryCache = LocalCaching.RemoveContainKeyInGroupKey;
        //    //delegateFlushAllCategoryCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
        //    //LocalCaching.RemoveContainKeyInGroupKey(strGroupKeyCached, containKey);

        //    RedisCaching.RemoveGroup(containKey);
        //}

        public bool InList(List<CATEGORY_FULL> categoryFulls, int categoryId)
        {
            var categoryFullsTemp = (from p in categoryFulls where p.Id == categoryId select p);
            if (categoryFullsTemp.Count() > 0)
                return true;
            return false;
        }

    }
}
