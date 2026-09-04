using BIZ;
using BIZ.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,CompetitionCreate")]
    public class AdminAlbumImageController : Controller
    {
        //
        // GET: /AdminAlbumImage/

       
        public ActionResult Index()
        {
            ViewBag.IsCompetition = false;
            ViewBag.IsCompetitionCreate = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("Competition"))
                ViewBag.IsCompetition = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("CompetitionCreate"))
                ViewBag.IsCompetitionCreate = true;
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(ConfigurationManager.AppSettings["UploadUrl"]).Append(UTILS.EntityName.Album).Append("/");
            ViewBag.ImageUrl = strBuilder.ToString();
            return View();
        }
        public ActionResult ListAlbum(string order, int? status, int? type, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 250 : (int)pageSize;
            //int Order = order == null ? 1 : (int)order;
            int Type = type == null ? -1 : (int)type;
            int Status = status == null ? -100 : (int)status;

            var data = new AlbumImageBO().GetAlbumsFuLLPaged(title, -1, Status, Type,CurrPage, RecordPerPage, ref TotalRecord, "","",order);
            foreach (var item in data)
            {
                try
                {
                    item.Album = JsonConvert.DeserializeObject<List<AlbumImageInfo>>(item.Description);
                }
                catch
                {
                    item.Album = new List<AlbumImageInfo>();
                }
            }
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;
            return PartialView(data);
        }
        public ActionResult Download()
        {
            var data = new AlbumImageBO().GetTopLastestAlbumsFull(1000, -1);
            foreach(var item in data)
            {
                if(item.Status>0)
                {
                    var strBuilder = new StringBuilder();
                    // divided 1000000 files in folder               
                    strBuilder.Append(Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("\\AlbumImage\\").Append(item.Code).Append("\\");
                    var desc = "<div><b>" + item.Code + " - " + item.Author + " - " + item.GroupName + "</b></div>";
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        DownloadImage(strBuilder.ToString(), "http://vnubw.org.vn/" + item.Image, "doanvien.jpg");
                        SaveHtmlFile(strBuilder.ToString(), "<div style='width:900px;margin:0 auto'><div> <img src='doanvien.jpg' style='max-width:900px;'></div>" + desc +
                     item.Description + "</div>", "doanvien.html");
                    }
                    if (!string.IsNullOrEmpty(item.Image2))
                    {
                        DownloadImage(strBuilder.ToString(), "http://vnubw.org.vn/" + item.Image2, "congdoan.jpg");
                        SaveHtmlFile(strBuilder.ToString(), "<div style='width:900px;margin:0 auto'><div> <img src='congdoan.jpg'  style='max-width:900px;'></div>" + desc +
                            item.Description2 + "</div>", "congdoan.html");
                    }
                 
                   
                 
                
                }
            }
            return View();
        }
        private void DownloadImage(string fromPath, string uri,string file)
        {
            var webClient = new WebClient();
            if (!Directory.Exists(fromPath))
                Directory.CreateDirectory(fromPath);
            webClient.DownloadFile(uri, fromPath + file);
        }
        private void SaveHtmlFile(string fromPath, string content, string file)
        {
            var webClient = new WebClient();
            if (!Directory.Exists(fromPath))
                Directory.CreateDirectory(fromPath);
            System.IO.File.WriteAllText(fromPath+ file, content);
        }
        public ActionResult AddEdit(int Id = 0)
        {
            var model = new AlbumImage_FULL { Id = 0, PublishDate = DateTime.Now,Status=-1 };
            model.Album = new List<AlbumImageInfo>();
            ViewBag.FileImage = new List<SelectListItem>();
            if (Id > 0)
            {

                model = new AlbumImageBO().GetAlbum(Id);
                try
                {
                    model.Album = JsonConvert.DeserializeObject<List<AlbumImageInfo>>(model.Description);
                }catch
                {
                    model.Album = new List<AlbumImageInfo>();
                }
               
                if (model == null)
                    return RedirectToAction("Index");
               
               

                ViewBag.Title = "Cập nhật Album";
            }
            else
            {
                ViewBag.Title = "Thêm mới Album";
            }
            //ViewBag.IsCompetition = false;
            //ViewBag.IsCompetitionCreate = false;
            //if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("Competition"))
            //    ViewBag.IsCompetition = true;
            //if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("CompetitionCreate"))
            //    ViewBag.IsCompetitionCreate = true;
            //ViewBag.Id = Id;
            //ViewBag.ImageUrl = UTILS.Utils.GetImageUrl(Id, UTILS.EntityName.Album, false);
            if (string.IsNullOrEmpty(model.Description))
                model.Description = " ";
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public JsonResult SaveData(AlbumImage_FULL doc)
        {
            var ReturnData = new ReturnData();
            try
            {
                doc.Param = HttpContext.User.Identity.Name;
               
                //doc.CategoryId = 154;
                doc.CategoryPathway = ",154,";
                if (doc.Id > 0)
                {
                    doc.Code = String.Format("{0}{1}", "MS", doc.Id);
                }
               
                //doc.Description = Utils.ConvertToJson(Services, string.Empty);
                IFormatProvider culture = new CultureInfo("en-US", true);
                doc.PublishDate = DateTime.ParseExact(doc.SPublishDate, "dd/MM/yyyy HH:mm", culture);
                var result = new AlbumImageBO().CreateUpdateAlbum(doc);


                if (result >= 0)
                {
                   
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                      
                        ReturnData.ResponseCode = 0;
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                      
                        ReturnData.ResponseCode = 1;
                        doc.Id = result;
                        //var ms = result.ToString();
                        //if (result < 10)
                        //    ms = "0" + ms;
                        //doc.Code = String.Format("{0}{1}", "MS", ms);
                        //new AlbumImageBO().CreateUpdateAlbum(doc);

                    }

                 
                    
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateStatus(string _id, int  Status)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new AlbumImageBO().GetAlbum(Id);
                    if (obj != null)
                    {
                        obj.Status = Status;
                        new AlbumImageBO().CreateUpdateAlbum(obj);
                     

                        ReturnData.Description = "Cập nhật album Thành Công";
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
                    var result = new AlbumImageBO().DeleteAlbum(Id);
                    if (result >= 0)
                    {
                       
                      

                        ReturnData.Description = "Xóa album Thành Công";
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

    }
}
