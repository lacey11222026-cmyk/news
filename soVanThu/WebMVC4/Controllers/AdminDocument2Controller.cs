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
using System.Globalization;
using Newtonsoft.Json;
using DATA.ContentDB;
using System.Diagnostics;
using System.Web.UI.WebControls;
using WebMVC4.Helper;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Document")]
    public class AdminDocument2Controller : Controller
    {

        private List<CATEGORY_FULL> _staticCategoryList;
        protected override void Initialize(RequestContext requestContext)
        {


            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {


            return View();
        }
        public ActionResult Upload()
        {


            return View();
        }
        public ActionResult SaveUpload(List<IdeaTemp> lstdata)
        {
            var ReturnData = new ReturnData();
            int TotalRecord = 0;
            var data = IdeaDAL.GetSearch(-1, -1, -1, "", "", 1, 10000, ref TotalRecord);

            IFormatProvider culture = new CultureInfo("vi-VN", true);

          
            try
            {
                foreach (var item in lstdata)
                {
                   if(!String.IsNullOrEmpty(item.SPublishDate))
                    {
                        item.Type = 0;
                        item.Description = " ";
                        //NLogLogger.DebugMessage(item.SPublishDate);
                        item.PublishDate = DateTime.ParseExact(item.SPublishDate, "M/d/yy", CultureInfo.InvariantCulture);

                        foreach (var itemx in data)
                        {
                            var similarity = Utils.CalculateSimilarity(itemx.Name, item.Name);
                            if (similarity >= 0.7)
                            {

                                item.Type = 1;
                                item.Description = $"Gần giống với sáng kiến {itemx.Code}";
                                break; //
                            }
                        }
                        IdeaTempDAL.InsertUpdate(item);
                        System.Threading.Thread.Sleep(30);
                    }    
                    
                }
               
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Có lỗi trong quá trình xử lý";
            }
            return Json(ReturnData);
        }
        public ActionResult Download(int Id)
        {
            var newsobj = IdeaDAL.GetDetail(Id);
            if (newsobj == null)
                return RedirectToAction("Error", "Home");


            return Redirect(newsobj.FilePath);
        }
        public ActionResult ListDocument(int? year, int? status, int? progress, string unit, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;

            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;
            int Year = year == null ? 0 : (int)year;
            int Status = status == null ? 0 : (int)status;
            int Progress = progress == null ? 0 : (int)progress;
            int TotalRecord = 0;
            var data = IdeaDAL.GetSearch(Status, Progress, Year, title, unit, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
            {
                ViewBag.TotalRecord = TotalRecord;
                //foreach (var  item in data)
                //{
                //    item.FollowersConfig= JsonConvert.DeserializeObject<IdeaConfig>(item.Followers);
                //    item.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(item.Proposer);
                //}
            }
            else
            {
                ViewBag.TotalRecord = 0;
            }



            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        public ActionResult ListDocument2()
        {

            var data = IdeaTempDAL.GetTop();
            return PartialView(data);
        }
        public ActionResult GetDocumentDetail(int Id = 0)
        {
            ViewBag.CategoryList = _staticCategoryList;
            var model = new Idea { Id = 0 };
            if (Id > 0)
            {
                model = IdeaDAL.GetDetail(Id);
                //model.FollowersConfig = JsonConvert.DeserializeObject<IdeaConfig>(model.Followers);
                //model.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(model.Proposer);

                ViewBag.Title = "Cập nhật sáng kiến";
            }
            else
            {
                int TotalRecord = 0;
                var data = IdeaDAL.GetSearch(0, 0, 0, "", "", 1, 1, ref TotalRecord);
                model.Code = String.Format("SK-{0}", (TotalRecord + 1).ToString("D3"));
                model.PublishDate = DateTime.Now;
                model.ProgressPercent = 0;

                ViewBag.Title = "Thêm mới sáng kiến";
            }
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(Idea doc)
        {
            var ReturnData = new ReturnData();
            try
            {

                IFormatProvider culture = new CultureInfo("en-US", true);
                //doc.Followers = Utils.ConvertToJson(doc.FollowersConfig, string.Empty);
                //doc.Proposer = Utils.ConvertToJson(doc.Proposer, string.Empty);
                doc.PublishDate = DateTime.ParseExact(doc.SPublishDate, "dd/MM/yyyy", culture);
                if (string.IsNullOrEmpty(doc.Mark))
                    doc.Mark = " ";
                if (string.IsNullOrEmpty(doc.FilePath))
                    doc.FilePath = " ";
                if (string.IsNullOrEmpty(doc.Followers))
                    doc.Followers = " ";
                if (string.IsNullOrEmpty(doc.Effective))
                    doc.Effective = " ";
                var result = IdeaDAL.InsertUpdate(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Doc,
                        ItemId = doc.Id,
                        ItemName = doc.Name,
                        Note = "Xóa văn bản",
                        Type = 1

                    };
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Cập nhật sáng kiến";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới sáng kiến";
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
        public ActionResult Delete(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
           
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var result = IdeaDAL.Delete(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Doc,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa sáng kiến",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa sáng kiến Thành Công";
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
                ReturnData.Description = "Không xác định sáng kiến cần xóa";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save2(string joinId)
        {
            joinId = joinId.TrimStart(',');

            var ReturnData = new ReturnData();
            try
            {
                var lstdata = IdeaTempDAL.GetList(joinId);
                foreach(var item in lstdata)
                {
                    var itemx = new Idea();
                    itemx.Code = item.Code;
                    itemx.Name = item.Name;
                    itemx.No = item.No;
                    itemx.PublishDate = item.PublishDate;
                    itemx.FilePath = item.FilePath;
                    itemx.Proposer = item.Proposer;
                    itemx.Unit = item.Unit;
                    itemx.Status = HtmlHelpers.GetSKStatusInt(item.Status);
                    itemx.ProgressPercent = int.Parse(item.ProgressPercent.Replace("%",""));
                    itemx.Progress = 1;
                    itemx.Mark = " ";
                    itemx.Region = 1;
                    if (item.Result.Contains("Chưa đánh giá"))
                    {
                        itemx.Result = 2;
                    }
                    if (item.Result.Contains("Đạt"))
                    {
                        itemx.Result = 1;
                        itemx.Mark = item.Result.Replace("Đạt", "").Replace("(", "").Replace(")", "");

                    }
                    itemx.Effective = item.Effective;
                    itemx.Followers = item.Followers;
                    IdeaDAL.InsertUpdate(itemx);
                    System.Threading.Thread.Sleep(30);
                }    
                IdeaTempDAL.Delete("1=1");
                ReturnData.ResponseCode = 1;
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
        public ActionResult Delete2(string joinId)
        {
            joinId = joinId.TrimStart(',');

            var ReturnData = new ReturnData();
            try
            {
                if (!string.IsNullOrEmpty(joinId))
                {
                    var where = "Id IN (" + joinId + ")";
                    var result = IdeaTempDAL.Delete(where);
                    if (result >= 0)
                    {
                       
                       
                        ReturnData.Description = "Xóa  Thành Công";
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
                ReturnData.Description = "Không xác định sáng kiến cần xóa";
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
    }
}
