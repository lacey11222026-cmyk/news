using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BIZ;
using Constants = UTILS.Constants;
using BIZ.Entity;
using System.Web.Routing;

namespace WebMVC4.Controllers
{

    public class AdminNewsController : Controller
    {
        //
        // GET: /AdminNews/
        private List<CATEGORY_FULL> _staticCategoryList;
        private List<CATEGORY_FULL> _staticCategoryByUserList;
        protected override void Initialize(RequestContext requestContext)
        {

            _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, requestContext.HttpContext.User.Identity.Name,
                                                                               requestContext.HttpContext.User.IsInRole("Administrator"));
            base.Initialize(requestContext);

        }
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish")]
        public ActionResult Index()
        {
            ViewBag.Createdby = HttpContext.User.Identity.Name;
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;
            return View();
        }
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish")]
        public ActionResult AddEdit(long Id = 0)
        {
            ViewBag.Id = Id;
            ViewBag.ImageUrl = UTILS.Utils.GetImageUrl(Id, UTILS.EntityName.Article, false);
            ViewBag.Createdby = HttpContext.User.Identity.Name;
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigHotNews()
        {
            var lstCate = _staticCategoryByUserList;
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigHotNewsForCate(int categoryId = 0)
        {
            var lstCate = _staticCategoryByUserList;
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Trang chủ" });
            lstCate.Insert(0, new CATEGORY_FULL { Id = 1001, Name = "Tin chạy" });
            ViewBag.CategoryList = lstCate;
            //if (categoryId == 0)
            //{
            //    categoryId = lstCate.FirstOrDefault().Id;
            //}
            ViewBag.categoryId = categoryId;
            var key = "HotNewsForCate";
            //if (categoryId > 0)
            key += "_" + categoryId;
            ViewBag.lstNews = "";
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
                ViewBag.lstNews = configValue.ConfigValue;
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigTopViewNews(int categoryId = 0)
        {
            var lstCate = _staticCategoryByUserList;
            lstCate = lstCate.Where(x => x.Type == 11).ToList();
            ViewBag.CategoryList = lstCate;
            if (categoryId == 0)
            {
                categoryId = lstCate.FirstOrDefault().Id;
            }
            ViewBag.categoryId = categoryId;
            //lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;
            var key = "TopViewNews";
            //if (categoryId > 0)
            key += "_" + categoryId;
            ViewBag.lstNews = "";
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
                ViewBag.lstNews = configValue.ConfigValue;
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigHotVideo()
        {
            //var lstCate = _staticCategoryByUserList;
            //lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            //ViewBag.CategoryList = lstCate;
            return View();
        }

        [Authorize(Roles = "Administrator,Album")]
        public ActionResult Album()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(ConfigurationManager.AppSettings["UploadUrl"]).Append(UTILS.EntityName.Album).Append("/");
            ViewBag.ImageUrl = strBuilder.ToString();
            return View();
        }
        [Authorize(Roles = "Administrator,Album")]
        public ActionResult AlbumAddEdit(int Id = 0)
        {
            ViewBag.Id = Id;
            ViewBag.ImageUrl = UTILS.Utils.GetImageUrl(Id, UTILS.EntityName.Album, false);

            return View();
        }
        public ActionResult History(int id, int type)
        {
            var title = "";
            switch (type)
            {
                case (int)Constants.CategoryType.News:
                    var newobj = new ContentBO().GetContentFull(id);
                    title = newobj.Title;
                    break;
            }
            ViewBag.ItemName = title;
            var lstdata = new ContentLogBO().GetContentLogsByContentId(id, type);
            return View(lstdata);
        }
      


        [Authorize(Roles = "Administrator,Comment")]
        public ActionResult Comment()
        {

            return View();
        }
        [Authorize(Roles = "Administrator,Comment")]
        public ActionResult CommentAddEdit(long Id = 0)
        {
            ViewBag.Id = Id;


            return View();
        }
    }
}
