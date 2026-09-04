using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CategoryDBSproc : CategoryDBBase
    {
        #region Overrides of CategoryDBBase
        public override int UpdateContent(int cateid, string content, byte published, byte ordering, string language)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Category_UpdateContent(cateid, content, published, ordering, language);
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryDBBase", "UpdateContent cateid= " + cateid);
                return -1;
            }
        }
        public override int CreateUpdateCategory(Category category)
        {
            try
            {
                int? _id = category.Id;
                int? _parentid = category.ParentId;
                string _pathway = " ";
                
                string _name = category.Name;
                string _link = category.Link;
                string _description = category.Description;
                string _contents = category.Contents;
                string _language = category.Language;
                System.DateTime? _createdate = category.CreateDate;
                System.DateTime? _modifieddate = category.ModifiedDate;
                byte? _published = category.Published;
                byte? _ordering = category.Ordering;
               
                string _params = category.Params;
                byte? _type = category.Type;

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Category_InsertUpdate(_id, _parentid, _pathway, _name, _link, _description, _contents, _createdate, _modifieddate, _published, _ordering, _params, _type, _language);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryDBBase", "CreateUpdateCategory");
                return -1;
            }
        }

        public override Category GetCategory(int categoryId)
        {
            try
            {
                int? _categoryId = categoryId;
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Category_Select(_categoryId).FirstOrDefault();
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp,"CategoryProc","Getcategory, CategoryId="+ categoryId.ToString()  );
                return null;
            }
        }

        public override IEnumerable<Category> GetAllCategoriesPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var list = datacontext.sp_Category_SelectPaged(_pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetAllCategoriesPaged");
                return null;
            }
        }

        public override IEnumerable<Category> GetAllCategoriesPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
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
                    var list = datacontext.sp_Category_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetAllCategoriesPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Category> GetCategoriesDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetCategoriesDyn");
                return null;
            }
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 22/08/2011 06:31 PM
        /// todo: get categories by string of list id separate by ,
        /// </summary>
        /// <param name="listId">The list id.</param>
        /// <returns></returns>
        public override IEnumerable<Category> GetCategories(string listId)
        {
            try
            {
                var _listId = listId;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var select = "Id,Name,ParentId,Pathway,Params";
                    var where = "Id IN(" + _listId + ")";
                    var orderBy = "Ordering ASC";
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetCategories:listId= " + listId);
                return null;
            }
        }

        public override IEnumerable<Category> GetCategories()
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var select = "Id,Name,ParentId,Pathway,Published,Ordering,Type,Params";
                    var where = string.Empty;
                    var orderBy = "ParentId ASC,Ordering ASC";
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetCategories");
                return null;
            }
        }

        public override IEnumerable<Category> GetCategories(int categoryType)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var select = "Id,Name,ParentId,Pathway,Published,Language,Ordering,Type,Contents,Params,Link";
                    var where = string.Empty;
                    if (categoryType != -1)
                    {
                        where = "Type = " + categoryType;
                    }
                    var orderBy = "[Language] DESC, Ordering ASC";
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetCategories:categoryType= " + categoryType.ToString());
                return null;
            }
        }

        public override IEnumerable<Category> GetAllRootCategories()
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var select = "Id,Name,ParentId,Pathway";
                    var where = "ParentId = 0";
                    var orderBy = "[Language] DESC, Ordering ASC";
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetAllRootCategories");
                return null;
            }
        }

        public override IEnumerable<Category> GetAllChildCategories(int categoryId)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var select = "Id,Name,ParentId,Pathway,Published,Language,Ordering,Type";
                    var where = "ParentId=" + categoryId + " AND Id!=" + categoryId;
                    //var where = "Pathway Like '%,"+ categoryId+",%' AND Id!="+categoryId ;
                    var orderBy = "[Language] DESC, Ordering ASC";
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetAllChildCategories,categoryId="+categoryId.ToString()  );
                return null;
            }
        }

        //public override IEnumerable<Category> GetAllHomepageCategories()
        //{
        //    try
        //    {
        //        using (ShopOnlineDataContext datacontext = DataContext)
        //        {
        //            var select = "Id,Title,Image";
        //            var where = "Params LIKE '%\"IsHomepage\":1%'";
        //            var orderBy = "Ordering ASC";
        //            return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        ExHandler.Handle(exp);
        //        return null;
        //    }
        //}

        public override IEnumerable<Category> GetAllCategoriesByPosition(byte position,string lang)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var select = "Id,ParentId,Name,Pathway,Image,Type,Published,Link,Params";
                    var where = "Published = 1 AND ";
                    switch (position)
                    {
                        case 0:
                            where += "Params LIKE '%\"IsHomepage\":true%'";
                            break;
                        case 1:
                            where += "Params LIKE '%\"IsRightCol\":1%'";
                            break;
                        case 2:
                            where += "Params LIKE '%IsMainMenu:true%'";
                            break;
                        case 3:
                            where += "Params LIKE '%\"IsTopMenu\":1%'";
                            break;
                        case 4:
                            where += "Params LIKE '%\"IsFooter\":true%'";
                            break;
                    }
                    if(!String.IsNullOrEmpty(lang))
                    {
                        where += " AND Language =" + "'" + lang.ToLowerInvariant() + "'";
                    }
                    var orderBy = "Ordering ASC";
                    return datacontext.sp_Category_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CategoryProc", "GetAllCategoriesByPosition");
                return null;
            }
        }

        public override int DeleteCategoryDyn(string where)
        {
            try
            {
                string _where = where;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Category_DeleteDynamic(_where);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                if (exp is SqlException)
                {
                    var sqlException = exp as SqlException;
                    switch (sqlException.Number)
                    {
                        case 547:
                            return -2;
                        default:
                            return -1;
                    }
                }

                return -1;
            }
        }

        public override int DeleteCategory(int categoryId)
        {
            string where = "Id = " + categoryId;
            return DeleteCategoryDyn(where);
        }

        public override int DeleteCategories(string listCategoryId)
        {
            string where = "Id IN (" + listCategoryId + ")";
            return DeleteCategoryDyn(where);
        }

        #endregion
    }
}
