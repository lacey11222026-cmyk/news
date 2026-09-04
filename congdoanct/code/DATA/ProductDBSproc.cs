using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class ProductDBSproc : ProductDBBase
    {
        #region Overrides of ProductDBBase

        public override int CreateUpdateProduct(Product product)
        {
            try
            {
                int? _id = product.Id;
                int? _categoryid = product.CategoryId;
                string _title = product.Title;
                string _name = product.Name;
                string _alias = product.Alias;
                string _productcode = product.ProductCode;
                string _introtext = product.IntroText;
                string _fulltext = product.FullText;
                string _categoryPathway = product.CategoryPathway;
                string _images = product.Images;
                string _thumbnail = product.Thumbnail;
                double? _price = product.Price;
                System.DateTime _pricemodifydate = product.PriceModifyDate;
                string _attributes = product.Attributes;
                int? _createdby = product.CreatedBy;
                System.DateTime _createddate = product.CreatedDate;
                int? _modifiedby = product.ModifiedBy;
                System.DateTime _modifieddate = product.ModifiedDate;
                byte? _published = product.Published;
                byte? _ordering = product.Ordering;
                int? _hits = product.Hits;
                int? _count = product.Count;
                string _params = product.Params;
                int? _manuFactoryId = product.ManufactoryId;


                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Product_InsertUpdate(_id, _categoryid, _title, _name, _alias, _productcode, _introtext, _fulltext, _categoryPathway, _images, _thumbnail, _price, _pricemodifydate, _attributes, _createdby, _createddate, _modifiedby, _modifieddate, _published, _ordering, _hits, _count, _params, _manuFactoryId);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override Product GetProduct(int productId)
        {
            try
            {
                int? _productId = productId;
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Product_Select(_productId).FirstOrDefault();
            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var list = datacontext.sp_Product_SelectPaged(_pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords, short published)
        {
            var select = string.Empty;
            var where = string.Empty;
            if (published >= 0)
                where += "Published = " + published;
            var orderBy = "Ordering ASC";

            return GetAllProductsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Product> GetAllProductsPaged(int pageIndex, int pageSize, ref int totalRecords, short published, int categoryId)
        {
            var select = "Id,CategoryId,Title,Name,Alias,ProductCode,IntroText,CategoryPathway,Images,Price,CreatedDate,Published,Hits,Count,Params,ManufactoryId";
            var where = string.Empty;

            if (categoryId >= 0)
            {
                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }

            if (!string.IsNullOrEmpty(where))
                where += " AND ";

            if (published >= 0)
                where += "Published = " + published;

            var orderBy = "Id DESC";

            return GetAllProductsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Product> GetAllProductsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
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
                    var list = datacontext.sp_Product_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<Product> GetProductsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Product_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<Product> GetAllProducts(int published)
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id DESC";

            if (published >= 0)
            {
                where += " Published =" + published;
            }

            return GetProductsDyn(select, where, orderBy);
        }

        public override IEnumerable<Product> GetFilterProducts(string name, int categoryId, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";
            var where = string.Empty;
            var orderBy = "Id DESC";

            if (!string.IsNullOrEmpty(name))
                where += "Title LIKE N'%" + name + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }

            return GetAllProductsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Product> GetAllProducts(string name, int categoryId, byte published)
        {
            var select = "Id,CategoryId,Title,Name,Alias,ProductCode,IntroText,CategoryPathway,Images,Price,CreatedDate,Published,Hits,Count,Params,ManufactoryId";
            var where = string.Empty;
            var orderBy = "Ordering ASC";

            if (!string.IsNullOrEmpty(name))
                where += "Title LIKE N'%" + name + "%' ";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where)) where += " AND ";
                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }

            if (!string.IsNullOrEmpty(where))
                where += " AND ";
            where += " Published =" + published;

            return GetProductsDyn(select, where, orderBy);
        }

        public override IEnumerable<Product> GetAllProductsPagedByPriceRange(int categoryId, double fromPrice, double toPrice, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "Id,CategoryId,Title,Name,Alias,ProductCode,IntroText,CategoryPathway,Images,Price,CreatedDate,Published,Hits,Count,Params,ManufactoryId";
            var where = string.Empty;
            var orderBy = "Ordering ASC";

            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }

            if (!string.IsNullOrEmpty(where))
                where += " AND ";
            where += "Price >= " + fromPrice + " AND Price <= " + toPrice;

            if (!string.IsNullOrEmpty(where))
                where += " AND ";

            where += " Published = 1";


            return GetAllProductsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<Product> GetTopProductsByIds(string ids, int top)
        {

            string select;
            if (top > 0)
                select = "TOP(" + top + ") Id,Title,Name,Price,IntroText,CategoryPathway,Images,ManufactoryId,CreatedDate";
            else
                select = " Id,Title,Name,Price,IntroText,CategoryPathway,Images,ManufactoryId";

            string where = "Id IN (" + ids + ") AND Published = 1";
            string orderBy = "Id DESC";

            return GetProductsDyn(select, where, orderBy);
        }

        public override IEnumerable<Product> GetTopProductsByCategory(int top, int categoryId)
        {
            string select = "TOP(" + top + ") Id,Title,Name,Price,IntroText,CategoryPathway,Images,ManufactoryId,CreatedDate";
            if (top < 1)
                select = " Id,Title,Name,Price,IntroText,CategoryPathway,Images,ManufactoryId,CreatedDate";
            string where = "CategoryPathway Like '%," + categoryId + ",%' AND Published = 1";
            string orderBy = "Id DESC";

            return GetProductsDyn(select, where, orderBy);
        }

        public override int DeleteProductDyn(string where)
        {
            try
            {
                string _where = where;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Product_DeleteDynamic(_where);
                }
            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override int DeleteProduct(int productId)
        {
            string where = "Id = " + productId;
            return DeleteProductDyn(where);
        }

        public override int DeleteProducts(string listProductId)
        {
            string where = "Id IN (" + listProductId + ")";
            return DeleteProductDyn(where);
        }

        #endregion
    }
}
