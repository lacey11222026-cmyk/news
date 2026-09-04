using BIZ;
using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Constants = UTILS.Constants;
using WebMVC4.Models;
using UTILS;
using Newtonsoft.Json;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminCategoryController : Controller
    {
        //
        // GET: /AdminCategory/
        protected override void Initialize(RequestContext requestContext)
        {

            base.Initialize(requestContext);

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddEdit(int Id = 0)
        {
            ViewBag.CategoryId = Id;
            ViewBag.ImageUrl = UTILS.Utils.GetImageUrl(Id, UTILS.EntityName.Category, false);
            return View();
        }
        public ActionResult GetCategoryDetail(int Id = 0)
        {
            var category = new CATEGORY_FULL { Id = 0, Ordering = 1, Type = 2 };
            if (Id > 0)
            {
                category = new CategoryBO().GetCategoryFull(Id);
                if (category == null)
                    return RedirectToAction("Index");
            }
            //var category = new CATEGORY_FULL { Ordering = 0 };
            var listdata = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.None);

            var listcategory = new List<CATEGORY_FULL>();
            foreach (var item in listdata)
            {
                if (item.ParentId > 0)
                {

                    var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name, Pathway = item.Pathway };
                    if (item.NodeLevel != 0)
                    {
                        for (var i = 1; i < item.NodeLevel; i++)
                        {
                            x1.Name = "-+ " + x1.Name;
                        }
                    }

                    var pindex = listcategory.Select((Value, Index) => new { Value, Index }).Where(x => x.Value.Id == x1.ParentId).FirstOrDefault();
                    if (pindex != null)
                    {
                        listcategory.Insert(pindex.Index + 1, x1);

                    }


                }
                else
                {
                    listcategory.Add(item);
                }
            }

            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Nhóm gốc--" });
            if (Id == 0)
            {
                ViewBag.CategoryList = listcategory;
                ViewBag.Title = "Thêm mới chuyên mục";
                category.Param = new CategoryParam();
            }
            else
            {
                ViewBag.Title = "Cập nhật chuyên mục";
                try
                {
                    category.Param = JsonConvert.DeserializeObject<CategoryParam>(category.Params);
                }
                catch
                {

                    category.Param = new CategoryParam();
                }
                ViewBag.CategoryList = listcategory.Where(x => x.Id != Id).ToList();
            }
            return View(category);
        }
        public ActionResult Intro()
        {
            return View();
        }
        public ActionResult IntroAddEdit(int Id = 0)
        {
            var model = new CATEGORY_FULL { Id = 0, Ordering = 1 };
            if (Id > 0)
            {
                model = new CategoryBO().GetCategoryFull(Id);
                if (model == null)
                    return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public JsonResult SaveDataIntro(CATEGORY_FULL doc)
        {
            var ReturnData = new ReturnData();
            try
            {
                //doc.Params = Utils.ConvertToJson(doc.Param, string.Empty);

                var model = new CategoryBO().GetCategoryFull(doc.Id);
                model.Language = doc.Language;
                model.Contents = doc.Contents;
                model.Ordering = doc.Ordering;
                model.Published = doc.Published;
                // model.Params = doc.Params;

                var result = new CategoryBO().UpdateContent(model);

                if (result >= 0)
                {
                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Cate,
                        ItemId = doc.Id,
                        ItemName = model.Name,
                        Note = "",
                        Type = 1

                    };
                    ReturnData.Description = "Cập nhật Thành Công";
                    lognewsobj.Note = "Update nội dungchuyên mục";
                    ReturnData.ResponseCode = 0;

                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveData(CATEGORY_FULL doc)
        {
            var ReturnData = new ReturnData();
            try
            {
                doc.Params = Utils.ConvertToJson(doc.Param, string.Empty);
                var model = doc;
                if (doc.Id > 0)
                {
                    model = new CategoryBO().GetCategoryFull(doc.Id);
                    model.Language = doc.Language;
                    model.Name = doc.Name;
                    model.Description = doc.Description;
                    model.ParentId = doc.ParentId;
                    model.Link = doc.Link;
                    model.Type = doc.Type;
                    model.Ordering = doc.Ordering;
                    model.Published = doc.Published;
                    model.Params = doc.Params;
                }
                var result = new CategoryBO().CreateUpdateCategory(model);

                if (result >= 0)
                {
                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Cate,
                        ItemId = doc.Id,
                        ItemName = doc.Name,
                        Note = "",
                        Type = 1

                    };
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update chuyên mục";
                        ReturnData.ResponseCode = 0;
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới chuyên mục";
                        ReturnData.ResponseCode = 1;
                    }

                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        public ActionResult Banner()
        {
            return View();
        }
        public ActionResult BannerAddEdit(int Id = 0)
        {
            ViewBag.Id = Id;

            return View();
        }
        public ActionResult Support()
        {
            return View();
        }
        public ActionResult SupportAddEdit(int Id = 0)
        {
            ViewBag.Id = Id;

            return View();
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
