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
                                                                               requestContext.HttpContext.User.IsInRole("Comment"));
            base.Initialize(requestContext);

        }
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish,Comment")]
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
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish,Comment")]
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
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish,Comment")]
        public ActionResult ConfigHotNewsForCate(int categoryId = 0)
        {
            var lstCate = _staticCategoryByUserList;
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Trang chủ" });
            ViewBag.CategoryList = lstCate;
            ViewBag.CategoryList2 = lstCate;
            if (categoryId == 0)
            {
                categoryId = lstCate.FirstOrDefault().Id;
            }
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
        [ChildActionOnly]
        public ActionResult AlbumUploadContent(string myuploader, int id)
        {
            using (var uploader = new CuteWebUI.MvcUploader(System.Web.HttpContext.Current))
            {
                // set value Uploader
                uploader.UploadUrl = Response.ApplyAppPathModifier("~/Post/AlbumUploadHandler.ashx?id=" + id.ToString());
                uploader.Name = "myuploader";
                uploader.AllowedFileExtensions = "*.jpg";
                uploader.ManualStartUpload = true;
                uploader.MaxFilesLimit = 50;
                //uploader.MaxSizeKB = 102400;
                uploader.MultipleFilesUpload = true;
                uploader.InsertButtonID = "uploadbutton";
                uploader.CancelAllMsg = "Ngừng toàn bộ Upload";
                uploader.CancelUploadMsg = "Ngừng Upload";
                //prepair html code for the view
                ViewData["uploaderhtml"] = new HtmlString(uploader.Render());

                //if it's HTTP POST:
                if (!string.IsNullOrEmpty(myuploader))
                {
                    var processedfiles = (from strguid in myuploader.Split('/')
                                          select new Guid(strguid)
                                              into fileguid
                                          select uploader.GetUploadedFile(fileguid)
                                                  into file
                                          where file != null
                                          where Path.GetExtension(file.FileName).Equals(".jpg")
                                          select file.FileName).ToList();

                    if (processedfiles.Count > 0)
                    {
                        ViewData["UploadedMessage"] = string.Join(",", processedfiles.ToArray()) + " đã upload thành công";
                    }
                }

            }
            return PartialView();
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
