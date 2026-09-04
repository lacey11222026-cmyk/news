using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ProductDBSproc : ProductDBBase
    {
        #region Overrides of ProductDBBase

        public override int CreateUpdateProduct(Product manufactory)
        {
            int? responecode = 0;
            try
            {
                int _id = manufactory.Id;
                int? _manufactoryId = manufactory.ManufactoryId;
                string _album = manufactory.Album;
                string _name = manufactory.Name;
                string _description = manufactory.Description;
                string _intro = manufactory.Intro;
                string _tech = manufactory.Tech;
                string _service = manufactory.Language;
                string _image = manufactory.DescriptImage;
                string _config = manufactory.Config;
                int? _status = manufactory.Status;
                int? _order = manufactory.OrderSort;
                bool? _ishot = manufactory.IsHot;
                bool? _isnew = manufactory.IsNew;
                bool? _availableSell = manufactory.AvailableSell;
                int? _cateId = manufactory.CategoryId;
                string _catepath = manufactory.CategoryPathway;
                Decimal? _price = manufactory.Price;
                Decimal? _priceReal = manufactory.PriceReal;
                double? w = manufactory.W;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Product_InsertUpdate(_id, _name, _description, _intro, _tech, _service, _image, _album, _config, _ishot, _isnew, _price, _priceReal, _availableSell, _status, _order, _manufactoryId, _cateId, _catepath,w, ref responecode);
                    return responecode.GetValueOrDefault();
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ProductDBSproc", "CreateUpdateProduct");
                return -1;
            }
        }
        public override IEnumerable<Product> GetTopContentByIds(string ids, int top)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            if (ids.EndsWith(","))
                ids = ids.Remove(ids.Length - 1);

            var select = " Id,Name,DescriptImage,PriceReal,Price";
            if (top > 0)
                select = "TOP(" + top + ") Id,Name,DescriptImage,PriceReal,Price";
            var where = "Id IN (" + ids + ") AND Status = 1";
            var orderBy = "[OrderSort] DESC, Id DESC";

            return GetProductsDyn(select, where, orderBy);
        }
        public override Product GetProduct(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetProductsDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Product> GetTopLastest(int top, int categoryId, int manufactoryId, int? published,bool? isHot,bool? isNew,string lang="")
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if (manufactoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "ManufactoryId = " + manufactoryId;
            }
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
            if (!String.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " Language =" + "'" + lang.ToLowerInvariant() + "'";
            }
            if (isHot.GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " IsHot= 1";
            }
            if (isNew.GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " IsNew= 1";
            }
            var orderBy = "[OrderSort] DESC, Id DESC";

            return GetProductsDyn(select, where, orderBy);
        }
        public override IEnumerable<Product> GetAllPaged(string keyword, int categoryId, int manufactoryId, int pageIndex, int pageSize, ref int totalRecords, int? published, bool? isHot, bool? isNew, string lang = "", double min =0, double max =0,int orderType=0)
        {
            var select = string.Empty;
            var where = string.Empty;
            keyword = Utils.FormatKeywordSearch(keyword);

            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CategoryPathway Like '%," + categoryId + ",%' ";
            }
            if (!String.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " Language =" + "'" + lang.ToLowerInvariant() + "'";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "( Name LIKE N'%" + keyword + "%' ";
                where += "OR Description LIKE N'%" + keyword + "%' )";

            }
            if (manufactoryId >0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "ManufactoryId = " + manufactoryId;
            }
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
            if (isHot.GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " IsHot= 1";
            }
            if (isNew.GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " IsNew= 1";
            }
            if (min > 0 && max > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += " (W<= "+ max + " AND W>= "+min +")";
            }
            var orderBy = "[OrderSort] DESC, ID DESC";
            if (orderType > 0)
            {
                switch (orderType)
                {
                    case 1:
                        orderBy = "[Price] ASC";
                        break;
                    case 2:
                        orderBy = "[Price] DESC";
                        break;

                }
            }
            return GetAllProductsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public IEnumerable<Product> GetAllProductsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
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
                ExHandler.Handle(exp, "ProductDBSproc", "GetAllProductsPagedDyn");
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
                ExHandler.Handle(exp, "ProductDBSproc", "GetProductsDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Product_UpdateSortOrder(Id, upOrder);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int UpdateOrderTop(int Id)
        {
            try
            {

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Product_UpdateSortOrderTop(Id,true);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int SetHot(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Product_SetHot(Id, ref responeCode);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int SetNew(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Product_SetNew(Id, ref responeCode);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int SetSell(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Product_SetSell(Id, ref responeCode);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.SP_Product_UpdateStatus(Id, ref responeCode);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
        public override int DeleteProductDyn(string where)
        {
            try
            {
                string _where = where;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_ProductDeleteDynamic(_where);
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override int DeleteProduct(int manuFactoryId)
        {
            string where = "Id = " + manuFactoryId;
            return DeleteProductDyn(where);
        }

        public override int DeleteManufactories(string listProductId)
        {
            string where = "Id IN (" + listProductId + ")";
            return DeleteProductDyn(where);
        }

        #endregion
    }
}
