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
using DATA;
using WebMVC4.Models;
using UTILS;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Banner")]
    public class AdminBannerController : Controller
    {
        //
        // GET: /AdminBanner/
        //private List<CATEGORY_FULL> _staticCategoryList;
        //private List<CATEGORY_FULL> _staticCategoryByUserList;
        //protected override void Initialize(RequestContext requestContext)
        //{

        //    _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.WebSite);
        //    _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, requestContext.HttpContext.User.Identity.Name,
        //                                                                       requestContext.HttpContext.User.IsInRole("Administrator"));
        //    base.Initialize(requestContext);

        //}
        public ActionResult Index()
        {
            //var lstCate = _staticCategoryByUserList;

            //if(lstCate.Count==0)
            //    return RedirectToAction("Index2", "Admin");
            //lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Khoa học công nghệ" });
            //ViewBag.CategoryList = lstCate;
            return View();
        }
        public ActionResult ListBanner(int? regionId, int? status, int? type)
        {
            int RegionId = regionId == null ? -1 : (int)regionId;
            int Status = status == null ? -1 : (int)status;
            int Type = type == null ? -1 : (int)type;
            var data = new BannerBO().GetTopLastestBanners(0, RegionId, Status, Type);
            return PartialView(data);
        }
        public ActionResult GetBannerDetail(int Id = 0)
        {
            var model = new Banner { Id = 0, Type = 1, Order = 1 };
            if (Id > 0)
            {
                model = new BannerBO().GetBanner(Id);
                if (model == null)
                    return RedirectToAction("Index");
                //if(!HttpContext.User.IsInRole("Administrator"))
                //{
                //    var listcategory = _staticCategoryByUserList;
                //    if (listcategory.Find(x => x.Id == model.Type) == null)
                //    {
                //        return RedirectToAction("Index");
                //    }
                //}
                
                ViewBag.Title = "Cập nhật banner";
            }
            else
            {
                ViewBag.Title = "Thêm mới banner";
            }
            //var lstCate = _staticCategoryByUserList;
            //lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Khoa học công nghệ" });
            //ViewBag.CategoryList = lstCate;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SaveData(Banner banner)
        {
            var ReturnData = new ReturnData();
            try
            {
                var result = new BannerBO().CreateUpdateBanner(banner);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Banner,
                        ItemId = banner.Id,
                        ItemName = banner.Name,
                        Note = "Xóa banner",
                        Type = 1

                    };
                    if (banner.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update Banner";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới Banner";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new BannerBO().GetBanner(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Banner,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt banner",
                            Type = 1

                        };
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            lognewsobj.Note = "Khóa banner";
                        }
                        new BannerBO().CreateUpdateBanner(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật banner Thành Công";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định văn bản cần thao tác";
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
        public ActionResult Delete(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var result = new BannerBO().DeleteBanner(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Banner,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa banner",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa banner Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Bài Viết không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định bài viết cần xóa";
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
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
