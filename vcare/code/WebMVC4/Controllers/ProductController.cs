using BIZ;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, int ManuId = -1, int size = -1, int v = -1, int Page = 1, int type = 0, int orderType = 5, string view = "grid", string fManu = "")
        {

            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");


            ViewBag.CategoryId = cateobj.Id;
            ViewBag.CateName = cateobj.Name;
            ViewBag.ParentCategoryId = cateobj.ParentId;


            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name;
            if (size > 0)
            {
                siteTitle += " " + new CarSizeBO().Get(size).Name;
            }
            if (v > 0)
            {
                siteTitle += " " + new CarSizeBO().Get(v).Name;
            }

            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.SiteDescription = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle;


            if (Page > 1)
            {
                ViewBag.SiteTitle = siteTitle + " Trang-" + Page.ToString() + " | " + Resources.Global.SiteTitle;
                ViewBag.SiteDescription = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            }
            var PageSize = 12;
            int Total = 0;
            int max = 0;
            int min = 0;
            switch (type)
            {
                case 1:
                    min = 1;
                    max = 100000;
                    break;
                case 2:
                    min = 100000;
                    max = 500000;
                    break;
                case 3:
                    min = 500000;
                    max = 1000000;
                    break;
                case 4:
                    min = 1000000;
                    max = 2000000;
                    break;
                case 5:
                    min = 2000000;
                    max = 1000000000;
                    break;

            }
            string manu = "";
            if (ManuId > 0)
            {

                if (Config.ParentManu.Contains("," + ManuId + ","))
                {
                    manu = "," + ManuId + ",";

                    foreach (var item in new ManufactoryBO().GetAllManufactoryFulls(ManuId, -1, 1))
                    {
                        manu += item.Id + ",";
                    }
                }

            }
            else
            {
                manu = fManu;
            }
            ViewBag.fManu = fManu;
            var data = new ProductBO().GetProductsPagedFontEnd("", CategoryId, ManuId, manu, size, v, Page, PageSize, ref Total, 1, null, null, -1, min, max, orderType);
            if(data!=null)
            {
                var album = new ImageParam();
                foreach (var item in data)
                {
                    try
                    {
                        album = JsonConvert.DeserializeObject<ImageParam>(item.Album);

                    }
                    catch
                    {

                        album = new ImageParam();
                    }
                    item.Album = "";
                    if (!string.IsNullOrEmpty(album.Path1))
                        item.Album = album.Path1;
                }
               
            }    
            var model = new ProductModel { Cate = cateobj, listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId, ManufactoryId = ManuId, View = view };
            if (cateobj.ParentId > 0)
            {
                model.CateParrent = new CategoryBO().GetCategoryFull(cateobj.ParentId.GetValueOrDefault());
            }
            ViewBag.Type = type;
            model.Size = size;
            model.V = v;
            model.OrderType = orderType;
            model.ManufactoryId = ManuId;
            var lsstManu = new List<MANUFACTORY_FULL>();
           
            if (cateobj.ParentId == 0)
            {
                lsstManu = new ManufactoryBO().GetAllManufactoryFulls(-1, CategoryId, -1);
            }
            else
            {

                lsstManu = new ManufactoryBO().GetAllManufactoryFulls(-1, int.Parse(cateobj.Pathway.Split(',')[1]), -1);

            }
            model.ListManu = lsstManu;
            // model.ListManu = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1);
            if (ManuId > 0)
            {
                model.Manu = model.ListManu.FirstOrDefault(x => x.Id == ManuId);
                siteTitle += " " + model.Manu.Title;
            }
            else
            {
                model.Manu = new MANUFACTORY_FULL { Id = -1, ParentId = -1, Title = "" };
            }
            model.listcate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x => x.Published == 1 && x.ParentId > 0).ToList();
            //if (CategoryId == 2 || CategoryId == 8 || model.Cate.ParentId == 2)
            //{
            //    model.ListSize = new CarSizeBO().GetTopLastestCarSize(-1, -1, 1);
            //}

            return View(model);
        }
        [LocalizationActionFilter]
        public ActionResult Search(string query, int Page = 1)
        {

            var siteTitle = "Tìm kiếm từ khóa " + query + " | ";

            ViewBag.SiteDescription = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            //ViewBag.Keywords =  ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle + Resources.Global.SiteTitle;

            ViewBag.SiteDescription = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            //ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle + Resources.Global.SiteTitle;
            if (Page > 1)
            {
                ViewBag.SiteTitle = siteTitle + " | " + Resources.Global.SiteTitle + " Trang-" + Page.ToString();
                ViewBag.SiteDescription = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]) + " Trang-" + Page.ToString();
            }


            ViewBag.Keyword = query;
            var PageSize = 48;
            int Total = 0;
            var albums = new ProductBO().GetProductsPaged(query,-1, -1, "", -1, -1, Page, PageSize, ref Total,1,null,null,-1,0,0,3);
            var Model = new ProductModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total };
            
            return View(Model);
        }
        public ActionResult CarDetail(int Id, int orderType = 3, string view = "grid", string fManu = "", int type = 0)
        {
            var carobj = new CarModelBO().GetCarModel(Id);
            var carGroup = new CarGroupBO().GetCarGroup(carobj.GroupId);
            ViewBag.Title = carGroup.Name + " " + carobj.Name;
            if (carobj.Name.Contains(carGroup.Name))
                ViewBag.Title = carobj.Name;

            ViewBag.SiteTitle = "Bình ắc quy & lốp cho ô tô " + ViewBag.Title;
            ViewBag.fManu = fManu;
            int total = 0;
            int max = 0;
            int min = 0;
            switch (type)
            {
                case 1:
                    min = 1;
                    max = 500000;
                    break;
                case 2:
                    min = 500000;
                    max = 1000000;
                    break;
                case 3:
                    min = 1000000;
                    max = 1500000;
                    break;
                case 4:
                    min = 1500000;
                    max = 2000000;
                    break;
                case 5:
                    min = 2000000;
                    max = 1000000000;
                    break;

            }
            var data = new ProductBO().GetProductsPagedFontEnd("", -1, -1, fManu, -1, -1, 1, 100, ref total, 1, null, null, -1, min, max, orderType, Id);
            var dataft = new ProductBO().GetProductsPagedFontEnd("", -1, -1, "", -1, -1, 1, 100, ref total, 1, null, null, -1, 0, 0, 3, Id);

            ViewBag.Type = type;
            var model = new ProductModel { CarModel = carobj, listdata = data, View = view, OrderType = orderType };
            model.ListManu = null;
            if (dataft != null)
            {
                var lstManuName = dataft.GroupBy(x => x.ManufactoryId.GetValueOrDefault()).Select(a=>a.Key);
                var lstManu = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1);
                var lstfManu = new List<MANUFACTORY_FULL>();
                if (lstManuName != null)
                {
                    foreach (var manuId in lstManuName)
                    {
                        if (lstManu.Exists(x => x.Id == manuId))
                        {
                            var manu = lstManu.FirstOrDefault(x => x.Id == manuId);
                            if(manu.ParentId==0)
                            {
                                if (!lstfManu.Exists(x => x.Id == manu.Id))
                                {
                                    lstfManu.Add(manu);
                                }    
                            }
                            else
                            {
                                var manuParent= lstManu.FirstOrDefault(x => x.Id == manu.ParentId);
                                if (!lstfManu.Exists(x => x.Id == manu.ParentId))
                                {
                                    lstfManu.Add(manuParent);
                                }
                            }
                        }
                    }
                    lstfManu.OrderBy(x => x.Ordering);
                    model.ListManu = lstfManu;
                }
            }
            
            return View(model);

        }
        [LocalizationActionFilter]
        public ActionResult Detail(string Url)
        {
            var productobj = new ProductBO().GetProductFull(Url);
            if (productobj==null)
                return RedirectToAction("Error", "Home");
            try
            {
                productobj.ImageParam = JsonConvert.DeserializeObject<ImageParam>(productobj.Album);
            }
            catch
            {

                productobj.ImageParam = new ImageParam();
            }
            if (productobj.ImageParam == null)
            {
                productobj.ImageParam = new ImageParam();
            }
            try
            {
                productobj.ProParam = JsonConvert.DeserializeObject<ProParam>(productobj.Config);
            }
            catch
            {

                productobj.ProParam = new ProParam();
            }
            if (productobj.ProParam == null)
            {
                productobj.ProParam = new ProParam();
            }
            if (productobj.Status.GetValueOrDefault() != 1)
                return RedirectToAction("Error", "Home");

            var metaDescription = productobj.Name;
            var siteTitle = productobj.Name;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.SiteDescription = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle;

            if (HttpContext.Request.Url != null)
                ViewBag.SiteImage = "http://" + HttpContext.Request.Url.Host + productobj.DescriptImage;
            //ViewBag.CurrentCategoryId = productobj.CategoryId;

            var manuobj = new ManufactoryBO().GetManufactoryFull(productobj.ManufactoryId.GetValueOrDefault());
            if (manuobj == null)
                manuobj = new MANUFACTORY_FULL { Id = -1 };

            var cateobj = new CategoryBO().GetCategoryFull(productobj.CategoryId.GetValueOrDefault());
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            var title = cateobj.Name;
            if (productobj.ManufactoryId > 0)
                title += " " + manuobj.Title;
            ViewBag.Title = title;
            var model = new ProductDetailModel
            {
                Detail = productobj,
                Manu = manuobj,
                Cate = cateobj

            };
            if (cateobj.ParentId > 0)
            {
                model.CateParrent = new CategoryBO().GetCategoryFull(cateobj.ParentId.GetValueOrDefault());
            }
            //ViewBag.CateName = cateobj.Name;
            //ViewBag.ManuName = manuobj.Title;

            return View(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult FooterMenu(int id, string name)
        {
            var cate = new CATEGORY_FULL { Id = id, Name = name, Type = 0, Link = "" };
            var lstmanu = new CategoryManufactoryBO().GetByCateId(cate.Id);
            //var lstcate = new CategoryBO().GetAllChildCategories(cate.Id, 10, false);

            var model = new ProductModel
            {
                Cate = cate,
                //listmanu = lstmanu,
                //listcate = lstcate.Where(x => x.Published == 1).ToList()
            };
            return PartialView(model);
        }

        public ActionResult HomeMenu(int id, string name)
        {
            try
            {
                var cate = new CATEGORY_FULL { Id = id, Name = name, Type = 0, Link = "" };
                var lstmanu = new CategoryManufactoryBO().GetByCateId(-1);
                var lstcate = new CategoryBO().GetAllChildCategories(cate.Id, 10, false);
                if (lstcate != null)
                    lstcate = lstcate.Where(x => x.Published == 1).ToList();
                var model = new ProductModel
                {
                    Cate = cate,
                    //listmanu = lstmanu,
                    listcate = lstcate
                };
                return PartialView(model);
            }
            catch
            {

                return PartialView(null);
            }
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult HotProduct(int top)
        {

            var data = new ProductBO().GetTopProduct(top, -1, -1, -1, 1, -1, -1);
            if (data != null)
            {
                var album = new ImageParam();
                foreach (var item in data)
                {
                    try
                    {
                        album = JsonConvert.DeserializeObject<ImageParam>(item.Album);

                    }
                    catch
                    {

                        album = new ImageParam();
                    }
                    item.Album = "";
                    if (!string.IsNullOrEmpty(album.Path1))
                        item.Album = album.Path1;
                }

            }
            var model = new ProductModel
            {
                listdata = data
            };
          
            
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopProduct(int top, int categoryId, int manufactoryId, bool? ishot, bool? isnew)
        {

            var data = new ProductBO().GetTopProduct(top, categoryId, manufactoryId, -1, -1, -1, -1);
            if (data != null)
            {
                var album = new ImageParam();
                foreach (var item in data)
                {
                    try
                    {
                        album = JsonConvert.DeserializeObject<ImageParam>(item.Album);

                    }
                    catch
                    {

                        album = new ImageParam();
                    }
                    item.Album = "";
                    if (!string.IsNullOrEmpty(album.Path1))
                        item.Album = album.Path1;
                }

            }
            var model = new ProductModel
            {
                listdata = data
            };
            if (categoryId > 0)
            {
                var cateobj = new CategoryBO().GetCategoryFull(categoryId);
                //var lstmanu = new CategoryManufactoryBO().GetByCateId(categoryId);
                model.Cate = cateobj;
                //model.listmanu = lstmanu;
            }
            return PartialView(model);
        }
        public ActionResult LoadDataGrid(List<Product> data)
        {
            return PartialView(data);
        }
        public ActionResult LoadProduct(int Id)
        {
            var productobj = new ProductBO().GetProductFull(Id);
            try
            {
                productobj.ImageParam = JsonConvert.DeserializeObject<ImageParam>(productobj.Album);
            }
            catch
            {

                productobj.ImageParam = new ImageParam();
            }
            if (productobj.ImageParam == null)
            {
                productobj.ImageParam = new ImageParam();
            }
            if (productobj.Status.GetValueOrDefault() != 1)
                return RedirectToAction("Error", "Home");



            var manuobj = new ManufactoryBO().GetManufactoryFull(productobj.ManufactoryId.GetValueOrDefault());
            if (manuobj == null)
                manuobj = new MANUFACTORY_FULL { Id = -1 };

            var cateobj = new CategoryBO().GetCategoryFull(productobj.CategoryId.GetValueOrDefault());


            var model = new ProductDetailModel
            {
                Detail = productobj,
                Manu = manuobj,
                Cate = cateobj

            };
            return PartialView(model);
        }
        public ActionResult LoadDataList(List<Product> data)
        {
            return PartialView(data);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopManu(int CategoryId, int ManuId, string CateName)
        {
            var data = new List<MANUFACTORY_FULL>();
            if (CategoryId == 0)
            {
                data = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, -1);
            }
            else
            {
                //var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                //var id = cateobj.ParentId > 0 ? cateobj.ParentId.GetValueOrDefault() : CategoryId;
                var lstdata = new CategoryManufactoryBO().GetByCateId(CategoryId);
                if (lstdata == null)
                {
                    data = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, -1);
                }
                else
                {
                    foreach (var item in lstdata)
                    {
                        data.Add(new MANUFACTORY_FULL { Id = item.ManufactoryId.GetValueOrDefault(), Title = item.ManufactoryName });
                    }
                }
            }
            ViewBag.ManuId = ManuId;
            ViewBag.CateUrl = Utils.FormatUrlRewriteByType(CategoryId, CateName, 0, "");
            return PartialView(data);
        }

        public ActionResult Range(int type, string manufactory, string national, string waterType, string flag)
        {
            ViewBag.Type = type;
            ViewBag.Manufactory = manufactory;
            ViewBag.National = national;
            ViewBag.WaterType = waterType;
            ViewBag.Flag = flag;
            var lstdata = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, -1);
            return PartialView(lstdata.Where(x => x.Published == 1).ToList());
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RelateProduct(int id, int categoryId)
        {
            try
            {
                
                var data = new ProductBO().GetTopProduct(6, categoryId, -1, -1, -1, id, 0);
                if(data==null||data.Count()<3)
                {
                    var cate = new CategoryBO().GetCategoryFull(categoryId);
                    data = new ProductBO().GetTopProduct(6, cate.ParentId.GetValueOrDefault(), -1, -1, -1, id, 0);
                }    
                if (data != null)
                {
                    var album = new ImageParam();
                    foreach (var item in data)
                    {
                        try
                        {
                            album = JsonConvert.DeserializeObject<ImageParam>(item.Album);

                        }
                        catch
                        {

                            album = new ImageParam();
                        }
                        item.Album = "";
                        if (!string.IsNullOrEmpty(album.Path1))
                            item.Album = album.Path1;
                    }

                }
                return PartialView(data);
            }
            catch (Exception)
            {

                return PartialView(null);
            }
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RelateProduct2(int id, int categoryId, int manuid, int size, int v, decimal price)
        {
            try
            {
                var lstdata = new ProductBO().GetTopProduct(12, categoryId, -1, size, v, id, price + 5000000, manuid);

                return PartialView(lstdata);
            }
            catch (Exception)
            {

                return PartialView(null);
            }
        }
        [OutputCache(Duration = 120, VaryByParam = "nonse", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var data = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, -1);
            //model.Manufactory = lstmanu.Where(x => x.Published == 1).ToList();
            return PartialView(data);
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public JsonResult GetCarModel(int Id)
        {
            var data = new CarModelBO().GetTopLastestCarModel(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public JsonResult GetCarSize(int size)
        {
            var data = new CarSizeBO().GetTopLastestCarSize(-1, size, 1);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
