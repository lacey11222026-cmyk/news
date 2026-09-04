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
        private IEnumerable<Product> GetTopContentByIds(string ids, int top)
        {
            var result = ProductDBBase.Create().GetTopContentByIds(ids, top);
            if (result == null)
                return null;
            return result;
        }
        public List<Product_Full> GetTopProductByIdsFulls(string ids, int top, bool isArragne = false)
        {

            //var lstItemBase =new List<Product_Full>();
            var lstItemBase = GetTopContentByIds(ids, top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<Product_Full>();
            if (isArragne)
            {

                var listIds = ids.Split(',').ToList();
                foreach (var itemid in listIds)
                {
                    if (!string.IsNullOrEmpty(itemid))
                    {
                        foreach (var content in lstItemBase)
                        {
                            if (content.Id == long.Parse(itemid))
                            {
                                var item = new Product_Full()
                                {
                                    Id = content.Id,
                                    Name = content.Name,
                                    DescriptImage = content.DescriptImage,
                                    PriceReal = content.PriceReal,
                                    Price = content.Price
                                   
                                };
                                lstItem.Add(item);
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (var content in lstItemBase)
                {
                    var item = new Product_Full()
                    {
                        Id = content.Id,
                        Name = content.Name,
                        DescriptImage = content.DescriptImage,
                        PriceReal = content.PriceReal,
                        Price = content.Price
                    };

                    lstItem.Add(item);
                }
            }


           
            return lstItem;
        }
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
                    W = content.W,
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


        public List<Product> GetTopProduct(int top, int categoryId, int manufactoryId, int? published, bool? isHot, bool? isNew, string lang = "")
        {
            var data = ProductDBBase.Create().GetTopLastest(top, categoryId, manufactoryId,published, isHot, isNew,lang);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Product> GetProductsPaged(string keyword, int categoryId,int manufactoryId, int pageIndex, int pageSize, ref int totalRecords, int? published, bool? isHot, bool? isNew, string lang = "",double min=0, double max =0,int orderType=0)
        {
            var data = ProductDBBase.Create().GetAllPaged( keyword ,categoryId, manufactoryId,pageIndex, pageSize, ref  totalRecords, published,  isHot,isNew,lang,min,max, orderType);
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
