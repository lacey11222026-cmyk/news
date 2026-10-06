using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class ProductDBBase : ShopOnlineDBBase
    {
        public static ProductDBBase Create()
        {
            return (ProductDBBase)Activator.CreateInstance(typeof(ProductDBSproc));
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateProduct(Product product);

        #endregion

        #region READ STATEMENTs

        public abstract Product GetProduct(int productId);
        public abstract IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords, short published);
        public abstract IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords, short published, int categoryId);
        public abstract IEnumerable<Product> GetAllProductsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Product> GetProductsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Product> GetAllProducts(int published);
        public abstract IEnumerable<Product> GetFilterProducts(string name, int categoryId, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Product> GetAllProducts(string name, int categoryId, byte published);
        public abstract IEnumerable<Product> GetAllProductsPagedByPriceRange(int categoryId, double fromPrice, double toPrice, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Product> GetTopProductsByIds(string ids, int top);
        public abstract IEnumerable<Product> GetTopProductsByCategory(int top, int categoryId);


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteProductDyn(string where);

        public abstract int DeleteProduct(int productId);

        public abstract int DeleteProducts(string listProductId);

        #endregion
    }
}
