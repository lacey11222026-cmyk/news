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
        public ActionResult Index(int CategoryId, string CateName, int ManuId = 0, int Page = 1, int type = 0,int orderType=0)
        {

            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (CategoryId == 0)
                cateobj = new CATEGORY_FULL { Id = 0, Name = "Sản phẩm", ParentId = 0 };
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            //if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
            //    return RedirectToAction("Index", "Product", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

            ViewBag.CategoryId = cateobj.Id;
            ViewBag.CateName = cateobj.Name;
            ViewBag.ParentCategoryId = cateobj.ParentId;


            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.SiteDescription = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle + Resources.Global.SiteTitle;

            //var lstManu = new List<CategoryManufactory>();
            //lstManu = new CategoryManufactoryBO().GetByCateId(cateobj.Id);

            if (Page > 1)
            {
                ViewBag.SiteTitle = siteTitle + " Trang-" + Page.ToString() + " | " + Resources.Global.SiteTitle;
                ViewBag.SiteDescription = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            }
            var PageSize = 24;
            int Total = 0;
            int max = 0;
            int min = 0;
            switch (type)
            {
                case 1:
                    min = 1;
                    max = 5000000;
                    break;
                case 2:
                    min = 5000000;
                    max = 10000000;
                    break;
                case 3:
                    min = 10000000;
                    max = 20000000;
                    break;
                case 4:
                    min = 20000000;
                    max = 30000000;
                    break;
                case 5:
                    min = 30000000;
                    max = 40000000;
                    break;
                case 6:
                    min = 40000000;
                    max = 1140000000;
                    break;
            }
            var data = new ProductBO().GetProductsPaged("", CategoryId, ManuId, Page, PageSize, ref Total, 1, null, null, "", min, max, orderType);
            var model = new ProductModel { Cate = cateobj, listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId, ManufactoryId = ManuId };
            if (cateobj.ParentId > 0)
            {
                model.CateParrent = new CategoryBO().GetCategoryFull(cateobj.ParentId.GetValueOrDefault());
            }
            ViewBag.Type = type;
            ViewBag.OrderType = orderType;
            ViewBag.ManuId = ManuId;
            return View(model);
        }
        [LocalizationActionFilter]
        public ActionResult Search(string q, int Page = 1)
        {

            var siteTitle = "Tìm kiếm từ khóa " + q + " | ";

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


            ViewBag.Keyword = q;
            var PageSize = 24;
            int Total = 0;
            var albums = new ProductBO().GetProductsPaged(q, 0, -1, Page, PageSize, ref Total, 1, null, null, "");
            var Model = new ProductModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total };
            ViewBag.BodyClass = "wrap product-page";
            return View(Model);
        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id, string Title)
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
            if (Title != Utils.ConvertToRewriteLink(productobj.Name))
                return RedirectToAction("Detail", "Product", new { Id = Id, Title = Utils.ConvertToRewriteLink(productobj.Name) });
            var metaDescription = productobj.Name;
            var siteTitle = productobj.Name;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.SiteDescription = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle;

            if (HttpContext.Request.Url != null)
                ViewBag.SiteImage = "http://"+HttpContext.Request.Url.Host +productobj.DescriptImage;
            //ViewBag.CurrentCategoryId = productobj.CategoryId;

            //var manuobj = new ManufactoryBO().GetManufactoryFull(productobj.ManufactoryId.GetValueOrDefault());
            //if (manuobj == null)
            //    return RedirectToAction("Error", "Home");

            var cateobj = new CategoryBO().GetCategoryFull(productobj.CategoryId.GetValueOrDefault());
            if (cateobj == null)
                return RedirectToAction("Error", "Home");

            var model = new ProductDetailModel
            {
                Detail = productobj,
                //Manu = manuobj,
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
                listmanu = lstmanu,
                //listcate = lstcate.Where(x => x.Published == 1).ToList()
            };
            return PartialView(model);
        }
       
        public ActionResult HomeMenu(int id, string name)
        {
            try
            {
                var cate = new CATEGORY_FULL { Id = id, Name = name, Type = 0, Link = "" };
                var lstmanu = new CategoryManufactoryBO().GetByCateId(cate.Id);
                var lstcate = new CategoryBO().GetAllChildCategories(cate.Id, 10, false);
                if (lstcate != null)
                    lstcate = lstcate.Where(x => x.Published == 1).ToList();
                var model = new ProductModel
                {
                    Cate = cate,
                    listmanu = lstmanu,
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
        public ActionResult TopProduct(int top, int categoryId, int manufactoryId, bool? ishot, bool? isnew)
        {

            var lstdata = new ProductBO().GetTopProduct(top, categoryId, manufactoryId, 1, ishot, isnew, WorkContext.GetLanguage());

            var model = new ProductModel
            {
                listdata = lstdata
            };
            if (categoryId > 0)
            {
                var cateobj = new CategoryBO().GetCategoryFull(categoryId);
                var lstmanu = new CategoryManufactoryBO().GetByCateId(categoryId);
                model.Cate = cateobj;
                model.listmanu = lstmanu;
            }
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopManu(int CategoryId, int ManuId,string link)
        {
            var data = new List<MANUFACTORY_FULL>();
            if (CategoryId == 0)
            {
                data = new ManufactoryBO().GetAllManufactoryFulls(-1);
            }
            else
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                //ViewBag.CateUrl = Utils.FormatUrlRewriteByType(CategoryId, cateobj.Name, 0, cateobj.Link);
                ViewBag.CateUrl = link;
                //var id = cateobj.ParentId > 0 ? cateobj.ParentId.GetValueOrDefault() : CategoryId;
                var lstdata = new CategoryManufactoryBO().GetByCateId(CategoryId);
                if (lstdata == null)
                {
                    data = new ManufactoryBO().GetAllManufactoryFulls(-1);
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
           
            return PartialView(data);
        }

        public ActionResult Range(int type)
        {
            ViewBag.Type = type;
            return PartialView();
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RelateProduct(int id, int categoryId, int manuid)
        {
            try
            {
                var lstdata = new ProductBO().GetTopProduct(8, categoryId, manuid, 1, null, null, "");
                lstdata = lstdata?.Where(x => x.Id != id).ToList();
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
            var lstcategory = new CategoryBO().GetAllCategoriesFull(0, "");
            //var lstmanu=new ManufactoryBO().GetAllManufactoryFulls(-1);
            //var model = new CategoryManufactoryModel();
            lstcategory = lstcategory.Where(x => x.Published == 1).ToList();
            //model.Manufactory = lstmanu.Where(x => x.Published == 1).ToList();
            return PartialView(lstcategory);
        }
    }
}
