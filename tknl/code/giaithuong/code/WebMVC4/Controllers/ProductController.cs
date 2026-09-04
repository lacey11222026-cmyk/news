using BIZ;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UTILS;

using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index(int CategoryId,   string q, int Page = 1)
        {

            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
           
            ViewBag.CategoryId = cateobj.Id;
            ViewBag.CateName = cateobj.Name;
            ViewBag.ParentCategoryId = cateobj.ParentId;


            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;



            var PageSize = 20;
            int Total = 0;
            int max = 0;
            int min = 0;

            var data = new ProductBO().GetProductsPaged(q, CategoryId, -1, Page, PageSize, ref Total, 1, null, null, "", min, max, 1);
            if (data != null)
            {
                foreach (var item in data)
                {
                    item.ProductParam = JsonConvert.DeserializeObject<ProductParam>(item.Config);
                }
            }
            var model = new ProductModel { Cate = cateobj, listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId, ManufactoryId = -1 };
            model.listcate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);
            ViewBag.q = q;
            return View(model);
        }
        public ActionResult DownloadFile(int Id)
        {
            var newsobj = new ProductBO().GetProductFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            WebClient myWebClient = new WebClient();
            myWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            byte[] myDataBuffer = myWebClient.DownloadData(newsobj.QRImage);
            return File(myDataBuffer, "image/png", newsobj.ProductParam.Model);
            //return Redirect(newsobj.QRImage);
        }
       
        public ActionResult Search(string q,int categoryId=-1, int Page = 1)
        {

            var siteTitle = "Tìm kiếm từ khóa " + q + " | ";

            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            //ViewBag.Keywords =  ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            //ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Description = siteTitle;


            ViewBag.Keyword = q;
            ViewBag.categoryId = categoryId;
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAlbumShow"]);
            int Total = 0;
            var data = new ProductBO().GetProductsPaged(q, categoryId, -1, Page, PageSize, ref Total, 1, null, null, "");
            if (data != null)
            {
                foreach (var item in data)
                {
                    item.ProductParam = JsonConvert.DeserializeObject<ProductParam>(item.Config);
                }
            }
            var Model = new ProductModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            Model.listcate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);
            return View(Model);
        }

        public ActionResult Detail(int Id, string Title,string CateName)
        {
            var productobj = new ProductBO().GetProductFull(Id);
            productobj.ProductParam = JsonConvert.DeserializeObject<ProductParam>(productobj.Config);
            productobj.ImageParam = JsonConvert.DeserializeObject<List<ProductFileInfo>>(productobj.Album);
            
            //if (Title != Utils.ConvertToRewriteLink(productobj.Name))
            //    return RedirectToAction("Detail", "Product", new { Id = Id, Title = Utils.ConvertToRewriteLink(productobj.Name) });
            var metaDescription = productobj.Name;
            var siteTitle = productobj.Name;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;

            if (HttpContext.Request.Url != null)
                ViewBag.SiteImage =  productobj.DescriptImage;
            //ViewBag.CurrentCategoryId = productobj.CategoryId;

          

            var cateobj = new CategoryBO().GetCategoryFull(productobj.CategoryId.GetValueOrDefault());
            if (cateobj == null)
                return RedirectToAction("Error", "Home");

            var model = new ProductDetailModel
            {
                Detail = productobj,
                Cate = cateobj

            };
            if (cateobj.ParentId > 0)
            {
                model.CateParrent = new CategoryBO().GetCategoryFull(cateobj.ParentId.GetValueOrDefault());
            }
            //ViewBag.CateName = cateobj.Name;
            //ViewBag.ManuName = manuobj.Title;
            ViewBag.MailShare = String.Format("https://mail.google.com/mail/u/0/?ui=2&view=cm&fs=1&tf=1&su={0}&body={1}", HttpUtility.UrlEncode(productobj.Name), HttpUtility.UrlEncode(productobj.Description));
            model.listcate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);

            Action<int, int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, productobj.CategoryId.GetValueOrDefault(), null, null);
            return View(model);
        }
        private void ViewAdd(int Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id, CategoryId);
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopProduct(int top, int categoryId, int manufactoryId, bool? ishot, bool? isnew)
        {

            var lstdata = new ProductBO().GetTopProduct(top, categoryId, manufactoryId, 1, ishot, isnew, "");
            if (lstdata != null)
            {
                foreach (var item in lstdata)
                {
                    item.ProductParam = JsonConvert.DeserializeObject<ProductParam>(item.Config);
                }
            }
            var model = new ProductModel
            {
                listdata = lstdata
            };
            if (categoryId > 0)
            {
                var cateobj = new CategoryBO().GetCategoryFull(categoryId);

                model.Cate = cateobj;

            }
            return PartialView(model);
        }




        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RelateProduct(int id, int categoryId)
        {
            try
            {
                var lstdata = new ProductBO().GetTopProduct(10, categoryId, -1, 1, true, null, "");
                //lstdata = lstdata?.Where(x => x.Id != id).ToList();
                if (lstdata != null)
                {
                    foreach (var item in lstdata)
                    {
                        item.ProductParam = JsonConvert.DeserializeObject<ProductParam>(item.Config);
                    }
                }
                return PartialView(lstdata);
            }
            catch (Exception)
            {

                return PartialView(null);
            }
        }

    }
}
