using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Product = DATA.Product;
namespace BIZ
{
    public class ProductBO
    {
       

        #region CREATE
        public int CreateUpdateProduct(Product Product)
        {
            
            int returnVal = ProductDBBase.Create().CreateUpdateProduct(Product);
          
            return returnVal;
        }
        public int UpdateStatus(int ProductId)
        {
            try
            {
                return ProductDBBase.Create().UpdateStatus(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "UpdateStatus");
                return -1;
            }
        }
        public int SetHot(int ProductId)
        {
            try
            {
                return ProductDBBase.Create().SetHot(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "SetHot");
                return -1;
            }
        }
        public int SetNew(int ProductId)
        {
            try
            {
                return ProductDBBase.Create().SetNew(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "SetNew");
                return -1;
            }
        }
        public int SetSell(int ProductId)
        {
            try
            {
                return ProductDBBase.Create().SetSell(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "SetSell");
                return -1;
            }
        }
        public int UpdateOrder(int ProductId, bool upOrder)
        {
            try
            {
                return ProductDBBase.Create().UpdateOrder(ProductId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "UpdateOrder");
                return -1;
            }
        }
        public int UpdateOrderTop(int ProductId)
        {
            try
            {
                return ProductDBBase.Create().UpdateOrderTop(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ
        public Product_Full GetProductFull(int ProductId)
        {
            try
            {
                var content = ProductDBBase.Create().GetProduct(ProductId);
                var item = new Product_Full
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Album = content.Album,
                    AvailableSell = content.AvailableSell,
                    CategoryPathway = content.CategoryPathway,
                    Config = content.Config,
                    CreatedTime = content.CreatedTime,
                    DescriptImage = content.DescriptImage,
                    Description = content.Description,
                    Intro = content.Intro,
                    IsHot = content.IsHot,
                    IsNew = content.IsNew,
                    Language = content.Language,
                    ManufactoryId = content.ManufactoryId,
                    Name = content.Name,
                    OrderSort = content.OrderSort,
                    Price = content.Price,
                    PriceReal = content.PriceReal,
                    Status = content.Status,
                    Tech = content.Tech,
                    Size = content.Size,
                    Volumn = content.Volumn,
                    Url = content.Url,
                    UpdateTime = content.UpdateTime,
                };
                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "GetProduct");
                return null;
            }
        }
        public Product_Full GetProductFull(string Url)
        {
            try
            {
                var content = ProductDBBase.Create().GetByUrl(Url);
                var item = new Product_Full
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Album = content.Album,
                    AvailableSell = content.AvailableSell,
                    CategoryPathway = content.CategoryPathway,
                    Config = content.Config,
                    CreatedTime = content.CreatedTime,
                    DescriptImage = content.DescriptImage,
                    Description = content.Description,
                    Intro = content.Intro,
                    IsHot = content.IsHot,
                    IsNew = content.IsNew,
                    Language = content.Language,
                    ManufactoryId = content.ManufactoryId,
                    Name = content.Name,
                    OrderSort = content.OrderSort,
                    Price = content.Price,
                    PriceReal = content.PriceReal,
                    Status = content.Status,
                    Tech = content.Tech,
                    Size = content.Size,
                    Volumn = content.Volumn,
                    Url = content.Url,

                };
                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "GetProduct");
                return null;
            }
        }
        public Product GetProduct(int ProductId)
        {
            try
            {
                return ProductDBBase.Create().GetProduct(ProductId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ProductBO", "GetProduct");
                return null;
            }
        }


        public List<Product> GetTopProduct(int top, int categoryId,  int manufactoryId, int size, int v, int id, decimal price,int notManuId=-1)
        {
            var data = ProductDBBase.Create().GetTopLastest(top, categoryId, manufactoryId, size, v, id, price,notManuId);
            if (data == null)
                return null;
            return data.ToList();
        }

        public List<Product> GetProductsPaged(string keyword, int categoryId,int manufactoryId,string manufactory,int s,int v, int pageIndex, int pageSize, ref int totalRecords, int? published, bool? isHot, bool? isNew, int model,int min=0,int max=0,int orderType=0)
        {
            var data = ProductDBBase.Create().GetAllPaged( keyword ,categoryId, manufactoryId, manufactory,s,v, pageIndex, pageSize, ref  totalRecords, published,  isHot,isNew, model, min,max, orderType);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Product> GetProductsPagedFontEnd(string keyword, int categoryId, int manufactoryId, string manufactory, int s, int v, int pageIndex, int pageSize, ref int totalRecords, int? published, bool? isHot, bool? isNew, int model, int min = 0, int max = 0, int orderType = 0,int carId=0)
        {
           
            //if(!string.IsNullOrEmpty(manufactory))
            //{
                //var childmanu = "";
                //var listIds = manufactory.Split(',').ToList();
                //foreach (var itemid in listIds)
                //{
                //    if (!string.IsNullOrEmpty(itemid))
                //    {
                //        if (Config.ParentManu.Contains("," + itemid + ","))
                //        {

                //            foreach (var item in new ManufactoryBO().GetAllManufactoryFulls(int.Parse(itemid), -1, 1))
                //            {
                //                childmanu += item.Id + ",";
                //            }
                //        }
                //        manufactory += childmanu;
                //    }

                //}
                   // }    
            var data = ProductDBBase.Create().GetAllPaged(keyword, categoryId, manufactoryId, manufactory, s,v, pageIndex, pageSize, ref totalRecords, 1, isHot, isNew,model, min, max, orderType, carId);
            if (data == null)
                return null;

            return data.ToList();
        }


        #endregion



        #region DELETE

        public int DeleteProducts(string listIds)
        {
            var returnVal = ProductDBBase.Create().DeleteManufactories(listIds);
         
            return returnVal;
        }

        public int DeleteProduct(int id)
        {
            var returnVal = ProductDBBase.Create().DeleteProduct(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
