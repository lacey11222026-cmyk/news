using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using BIZ;
using Constants = UTILS.Constants;
using BIZ.Entity;
using System.Web.Routing;
using DATA;
using WebMVC4.Models;
using UTILS;
using System.Web.Security;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using FileInfo = BIZ.Entity.FileInfo;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,NewsEdit,NewsPublish")]
    public class AdminNews2Controller : Controller
    {
        private List<CATEGORY_FULL> _staticCategoryList;
        private List<CATEGORY_FULL> _staticCategoryByUserList;
        protected override void Initialize(RequestContext requestContext)
        {

            _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, requestContext.HttpContext.User.Identity.Name,
                                                                               requestContext.HttpContext.User.IsInRole("Administrator"));
            base.Initialize(requestContext);

        }

        public ActionResult Index()
        {
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;

            var fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            var lstCate = _staticCategoryByUserList;
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;

            var lstChannel = new ChannelBO().GetFilter("", 0);
            lstChannel.Insert(0, new Channel { Name = "-Chọn kênh tin-", Id = 0 });
            ViewBag.lstChannel = lstChannel;
            return View();
        }

        public ActionResult ListNews(int? cateId, int? status, string fromDate, string endDate, string title, string createdBy, int? currentPage, int? pageSize,int? channelId)
        {
            //ViewBag.Createdby = HttpContext.User.Identity.Name;
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;


            var data = new List<CONTENT_FULL>();

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int currPage = currentPage ?? 1;
            int RecordPerPage = pageSize ?? 20;
            int CateId = cateId ?? -1;
            int Status = status ?? -1;
            int ChannelId = channelId ?? -1;
            var lststatus = "2,4";
            var listcategory = _staticCategoryByUserList;
            var lstCate = listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList();
            if (HttpContext.User.IsInRole("Administrator"))
            {
                lstCate = null;
            }
            if (!ViewBag.IsNewsPublish)
            {
                lststatus = "1,4,2";
                //Status = 1;
                createdBy = HttpContext.User.Identity.Name;
                data = new ContentBO().GetFilterContentFullsPaged(currPage, RecordPerPage, title, CateId, null, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus,"",ChannelId);
            }
            else
            {
                //xem đc tin xuất bản của mình
                if (Status == 1)
                {
                    createdBy = HttpContext.User.Identity.Name;
                }
                data = new ContentBO().GetFilterContentFullsPaged(currPage, RecordPerPage, title, CateId, lstCate, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus, "", ChannelId);
            }

            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = currPage;
            ViewBag.CategoryList = _staticCategoryByUserList;
            return PartialView(data);
        }
        public ActionResult GetNewsDetail(int Id = 0)
        {
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;

            var model = new CONTENT_FULL { PublishDate = DateTime.Now, Id = 0, Alias = HttpContext.User.Identity.Name, CategoryId = 0, Hits = 0, CreatedBy = HttpContext.User.Identity.Name, FileParam = new FileInfo()};
            ViewBag.UserName = HttpContext.User.Identity.Name;
            var lstChannel = new ChannelBO().GetFilter("", 0);
            lstChannel.Insert(0,new Channel {Name = "-Chọn kênh tin-",Id=0});

            var lstdata = Membership.GetAllUsers();
            //ExHandler.Handle(new Exception(), "User", "User" + lstdata.Count);
            List<AccountInfo> lstuser = new List<AccountInfo>();
            foreach (MembershipUser item in lstdata)
            {
                lstuser.Add(new AccountInfo { Value = item.UserName, Text = item.UserName });
            }
            ViewBag.UserList = lstuser;
            var listcategory = _staticCategoryByUserList;

            
            if (Id > 0)
            {
                model = new ContentBO().GetContentFull(Id);
                if (model.Status != 4)
                {
                    model.PublishDate = DateTime.Now;
                }
                try
                {
                    model.FileParam = JsonConvert.DeserializeObject<FileInfo>(model.Thumbnail);
                }
                catch
                {

                    model.FileParam = new FileInfo();
                }
                if (model.FileParam == null)
                {
                    model.FileParam = new FileInfo();
                }
                if (model == null)
                    return RedirectToAction("Index");
                var isCheckPermission = CheckPermission(model, ViewBag.IsNewsPublish, ViewBag.IsNewsEdit, HttpContext.User.Identity.Name);
                if (!isCheckPermission)
                {
                    return RedirectToAction("Index");
                }

                if (listcategory.Find(x => x.Id == model.CategoryId) == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Title = "Cập nhật tin tức";
               
                lstChannel = lstChannel.Where(x => x.Published == 1 || x.Id == model.ChannelId).ToList();
                listcategory = listcategory.Where(x => x.Published == 1||x.Id==model.CategoryId.Value).ToList();
            }
            else
            {
                lstChannel = lstChannel.Where(x => x.Published == 1).ToList();
                ViewBag.Title = "Thêm mới tin tức";
                listcategory = listcategory.Where(x => x.Published == 1).ToList();
            }
            ViewBag.CategoryList = listcategory;
            lstChannel.Insert(0,new Channel {Id = 0,Name="--Chọn kênh tin--"});
            ViewBag.lstChannel = lstChannel;
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveData(CONTENT_FULL newsobj, string actionType)
        {
            var ReturnData = new ReturnData();
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;
            IFormatProvider culture = new CultureInfo("en-US", true);
            newsobj.PublishDate = DateTime.ParseExact(newsobj.SPublishDate, "dd/MM/yyyy HH:mm", culture);

            if (String.IsNullOrEmpty(newsobj.Contents))
                newsobj.Contents = " ";

            if (String.IsNullOrEmpty(newsobj.Url))
                newsobj.Url = "";
            if (String.IsNullOrEmpty(newsobj.Params))
                newsobj.Params = "";
            newsobj.Thumbnail = Utils.ConvertToJson(newsobj.FileParam, string.Empty);
            newsobj.CreatedDate = DateTime.Now;
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                Type = 1,
                ItemtType = (int)Constants.CategoryType.News,
                ItemName = newsobj.Title,
                ItemId = int.Parse(newsobj.Id.ToString())
            };
            var notiobj = new Notifi
            {
                CreateUser = HttpContext.User.Identity.Name,
                ExpireDate = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                Link = Url.Action("Index")
            };

            //check action type
            switch (actionType.ToLower())
            {
                case "add":


                    //nếu tạo mới chuyển sang đang biên tập
                    lognewsobj.Note = "Tạo mới bài viết";
                    newsobj.Status = 1;
                    //nếu quyền xuất bản tạo mới sang chờ xuất bản luôn
                    if (ViewBag.IsNewsPublish)
                    {
                        newsobj.Status = 2;

                    }
                    else
                    {
                        newsobj.Status = 1;//đang biên tập
                    }
                    //lấy tài khoản xuất bản+admin
                    notiobj.Role = GetUserByNewsRole("");
                    notiobj.Title = $"<b>{HttpContext.User.Identity.Name}</b> tạo bài viết <b>{newsobj.Title}</b>";
                    break;
                case "publish":

                    newsobj.Status = 4;
                    lognewsobj.Note = "Xuất bản";

                    //lấy tài khoản xuất bản+admin+thằng viết
                    notiobj.Role = GetUserByNewsRole(newsobj.CreatedBy);
                    notiobj.Title = $"<b>{HttpContext.User.Identity.Name}</b> xuất bản bài viết <b>{newsobj.Title}</b>";
                    break;
                case "reject":
                    if (newsobj.Status == 4)
                    {
                        lognewsobj.Note = "Hạ bài";
                        newsobj.Status = 2;//đợi xuất bản
                        notiobj.Title = $"<b>{HttpContext.User.Identity.Name}</b> hạ bài viết <b>{newsobj.Title}</b>";
                        //lấy tài khoản xuất bản+admin+thằng viết
                        notiobj.Role = GetUserByNewsRole(newsobj.CreatedBy);
                    }
                    else
                    {
                        lognewsobj.Note = "Trả lại biên tập";
                        newsobj.Status = 1;//đang biên tập
                        notiobj.Title =
                            $"<b>{HttpContext.User.Identity.Name}</b> trả lại bài viết <b>{newsobj.Title}</b>";
                        // lấy tài khoản thằng viết
                        notiobj.Role = $",{newsobj.CreatedBy},";
                    }


                    break;
                case "send":

                    lognewsobj.Note = "Gửi xuất bản";
                    newsobj.Status = 2;
                    // lấy tài khoản xuất bản+admin
                    notiobj.Role = GetUserByNewsRole("");
                    notiobj.Title =
                        $"<b>{HttpContext.User.Identity.Name}</b> gửi xuất bản bài viết <b>{newsobj.Title}</b>";
                    break;

                case "save":

                    lognewsobj.Note = "Lưu bài";
                    //lấy tài khoản xuất bản+admin+thằng viết
                    notiobj.Role = GetUserByNewsRole(newsobj.CreatedBy);
                    notiobj.Title = $"<b>{HttpContext.User.Identity.Name}</b> lưu bài viết <b>{newsobj.Title}</b>";
                    break;
            }
            //update news
            var result = new ContentBO().CreateUpdateContent(newsobj);
            if (result >= 0)
            {
                if (lognewsobj.ItemId == 0)
                {
                    lognewsobj.ItemId = int.Parse(result.ToString());
                }

                //insert log
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);

                //insert Noti

                Action<Notifi> sendnoti = InsertNotifi;
                var asynSendNoti = sendnoti.BeginInvoke(notiobj, null, null);
                //new NotifiBO().Create(notiobj);
                ReturnData.ResponseCode = 0;
                if (actionType.ToLower() == "add" || actionType.ToLower() == "save")
                {
                    ReturnData.ResponseCode = result;
                }
                ReturnData.Description = "Cập nhật bài viết Thành Công";
            }
            else
            {
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
            }
            return Json(ReturnData);
            //return View();
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
                    var result = new ContentBO().DeleteContent(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.News,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa bài viết",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa bài viết Thành Công";
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
        [HttpPost]
        
        public ActionResult GetTime()
        {
            
            var ReturnData = new ReturnData();
           
                ReturnData.ResponseCode = -100;
            ReturnData.Description = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                return Json(ReturnData);
            
           
        }
        public ActionResult History(int id, int type, string name)
        {
            //var title = "";
            //switch (type)
            //{
            //    case (int)Constants.CategoryType.News:
            //        var newobj = new ContentBO().GetContentFull(id);
            //        title = newobj.Title;
            //        break;
            //}
            ViewBag.ItemName = name;
            var lstdata = new ContentLogBO().GetContentLogsByContentId(id, type);
            return PartialView(lstdata);
        }
        public ActionResult Reference(string ids)
        {
            var lstCate = _staticCategoryByUserList;
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;
            ViewBag.ids = ids;
            return PartialView();
        }
        public ActionResult Crawl()
        {
            ViewBag.Title = "Crawl tin tức";
            return View();
        }
     
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCrawlData(CONTENT_FULL newsobj)
        {
            var ReturnData = new ReturnData();
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;
            IFormatProvider culture = new CultureInfo("en-US", true);
            newsobj.PublishDate = DateTime.ParseExact(newsobj.SPublishDate, "dd/MM/yyyy HH:mm", culture);

            if (String.IsNullOrEmpty(newsobj.Contents))
                newsobj.Contents = " ";

            if (String.IsNullOrEmpty(newsobj.Url))
                newsobj.Url = "";
            newsobj.Params = "";
            newsobj.Image = "1.jpg";

            newsobj.CreatedBy = HttpContext.User.Identity.Name;
            newsobj.CreatedDate = DateTime.Now;
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                Type = 1,
                ItemtType = (int)Constants.CategoryType.News,
                ItemName = newsobj.Title,
                ItemId = int.Parse(newsobj.Id.ToString())
            };
            var notiobj = new Notifi
            {
                CreateUser = HttpContext.User.Identity.Name,
                ExpireDate = int.Parse(DateTime.Now.ToString("yyyyMMdd")),
                Link = Url.Action("Index")
            };
            newsobj.Hits = 0;
            
            //nếu tạo mới chuyển sang đang biên tập
            lognewsobj.Note = "Tạo mới bài viết";
            newsobj.Status = 1;
            //nếu quyền xuất bản tạo mới sang chờ xuất bản luôn
            if (ViewBag.IsNewsPublish)
            {
                newsobj.Status = 2;

            }
            else
            {
                newsobj.Status = 1;//đang biên tập
            }
            //lấy tài khoản xuất bản+admin
            notiobj.Role = GetUserByNewsRole("");
            notiobj.Title = $"<b>{HttpContext.User.Identity.Name}</b> tạo bài viết <b>{newsobj.Title}</b>";
            //update news
            var result = new ContentBO().CreateUpdateContent(newsobj);
            if (result >= 0)
            {
                if (lognewsobj.ItemId == 0)
                {
                    lognewsobj.ItemId = int.Parse(result.ToString());
                }

                //insert log
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);

                //insert Noti

                Action<Notifi> sendnoti = InsertNotifi;
                var asynSendNoti = sendnoti.BeginInvoke(notiobj, null, null);
                //new NotifiBO().Create(notiobj);


                //download image
                Action<string, string> sendImg = DownloadImage;

                var strBuilder = new StringBuilder();
                // divided 1000000 files in folder               
                strBuilder.Append(Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Article").Append("\\").Append(Convert.ToInt32(result) / 100000).Append("\\").Append(Convert.ToInt32(result) / 100).Append("\\").Append(result).Append("\\");
                //upload_path = strBuilder.ToString();
                var asynSendImg = sendImg.BeginInvoke(strBuilder.ToString(), newsobj.Thumbnail, null, null);

                ReturnData.ResponseCode = result;
                ReturnData.Description = "Cập nhật bài viết Thành Công";
            }
            else
            {
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
            }
            return Json(ReturnData);
            //return View();
        }
        private void DownloadImage(string fromPath, string uri)
        {
            var webClient = new WebClient();
            if (!Directory.Exists(fromPath))
                Directory.CreateDirectory(fromPath);
            webClient.DownloadFile(uri, fromPath + "1.jpg");
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
        private void InsertNotifi(Notifi sobj)
        {
            return;
            //new NotifiBO().Create(sobj);
        }
        private string GetUserByNewsRole(string username)
        {
            var list1 = Roles.GetUsersInRole("Administrator");
            var list2 = Roles.GetUsersInRole("NewsPublish");


            var result = new List<string> { };

            if (list1 != null)
            {
                foreach (var item in list1)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x == item) != null)
                        result.Add(item);
                }
            }
            if (list2 != null)
            {
                foreach (var item in list2)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x == item) != null)
                        result.Add(item);
                }
            }
            if (!string.IsNullOrEmpty(username) && result.Where(x => x == username) != null)
            {
                result.Add(username);
            }
            var stresult = ",";
            foreach (var item in result)
            {
                stresult += item + ",";
            }
            return stresult;
        }
        private bool CheckPermission(CONTENT_FULL data, bool IsNewsPublish, bool IsNewsEdit, string Currentcreatedby)
        {
            if (IsNewsPublish)
            {

                if (data.Status == 2 || data.Status == 4)
                {

                    return true;
                }

                if (data.Status == 1 && data.CreatedBy == Currentcreatedby)
                {

                    return true;
                }
            }

            if (IsNewsEdit)
            {
                if (data.Status == 1)
                {

                    if (data.CreatedBy == Currentcreatedby)
                        return true;
                }

            }
            return false;
        }
    }
}
