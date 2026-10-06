using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class CategoryDBBase : ShopOnlineDBBase
    {
        public static CategoryDBBase Create()
        {
            return (CategoryDBBase)Activator.CreateInstance(typeof(CategoryDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateCategory(Category category);
        public abstract int UpdateContent(int cateid, string content, byte published, byte ordering, string language);

        #endregion

        #region READ STATEMENTs

        public abstract Category GetCategory(int categoryId);
        public abstract IEnumerable<Category> GetAllCategoriesPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Category> GetAllCategoriesPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Category> GetCategoriesDyn(string select, string where, string orderBy);

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 22/08/2011 06:31 PM
        /// todo: get categories by string of list id separate by ,
        /// </summary>
        /// <param name="listId">The list id.</param>
        /// <returns></returns>
        public abstract IEnumerable<Category> GetCategories(string listId);
        public abstract IEnumerable<Category> GetCategories();
        public abstract IEnumerable<Category> GetCategories(int categoryType);
        public abstract IEnumerable<Category> GetAllRootCategories();
        public abstract IEnumerable<Category> GetAllChildCategories(int categoryId);
        //public abstract IEnumerable<Category> GetAllHomepageCategories();
        public abstract IEnumerable<Category> GetAllCategoriesByPosition(byte position);

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteCategoryDyn(string where);

        public abstract int DeleteCategory(int categoryId);

        public abstract int DeleteCategories(string listCategoryId);

        #endregion

    }
}
