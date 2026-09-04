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
using RestSharp;

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
        public ActionResult Index2()
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

        public ActionResult ListNews2(int? cateId, int? status, string fromDate, string endDate, string title, string createdBy, int? currentPage, int? pageSize, int? channelId)
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
            int Status = 2;
            int ChannelId = channelId ?? -1;
            var lststatus = "";
            var listcategory = _staticCategoryByUserList;
            var lstCate = listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList();
            if (HttpContext.User.IsInRole("Administrator"))
            {
                lstCate = null;
            }
            if (!ViewBag.IsNewsPublish)
            {
                //lststatus = "1,4,2";
                //Status = 1;
                createdBy = HttpContext.User.Identity.Name;
                data = new ContentBO().GetFilterContentFullsPaged(currPage, RecordPerPage, title, CateId, null, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus, "", ChannelId);
            }
            else
            {
                //xem đc tin xuất bản của mình
                //if (Status == 1)
                //{
                //    createdBy = HttpContext.User.Identity.Name;
                //}
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
            var lststatus = "2,4,1";
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
        public ActionResult GetENewsDetail(int Id = 0, int Temp = 1)
        {
            ViewBag.IsNewsEdit = false;
            ViewBag.IsNewsPublish = false;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
                ViewBag.IsNewsPublish = true;
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit"))
                ViewBag.IsNewsEdit = true;

            var model = new CONTENT_FULL { PublishDate = DateTime.Now, Id = 0, Alias = HttpContext.User.Identity.Name, CategoryId = 176, Hits = 0, CreatedBy = HttpContext.User.Identity.Name, FileParam = new FileInfo(), Type = (byte)Temp };
            ViewBag.UserName = HttpContext.User.Identity.Name;
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
                //listcategory = listcategory.Where(x => x.Published == 1 || x.Id == model.CategoryId.Value).ToList();
            }
            else
            {
                ViewBag.Title = "Thêm mới tin tức";
                //int categoryId = 60;
                //categoryId += Temp;
                model.Contents = System.IO.File.ReadAllText(Server.MapPath("/Views/Temp/"+Temp+ ".cshtml"));
                //listcategory = listcategory.Where(x => x.Published == 1).ToList();
            }
            ViewBag.CategoryList = listcategory;
            return View(model);

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
                listcategory = listcategory.Where(x => x.Published == 1 || x.Id == model.CategoryId.Value).ToList();
                lstChannel = lstChannel.Where(x => x.Published == 1 || x.Id == model.ChannelId).ToList();
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

            if (model.CategoryId == 176)
                return View("GetENewsDetail", model);
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
        public ActionResult LoadCrawl(string url)
        {
            ViewBag.Title = "Crawl tin tức";
            var crawlctl = new CrawlBO();
            var webcontent = crawlctl.GetPage(url);
            var newsdes = "";
            var newstitle = "";
            var newscontent = "";
            var newstime = "";
            var newimages = "";
            var remove1 = "";
            var newimageshtml = "";
            var domain = Utils.GetUrlRootOfLink(url);
            switch (domain)
            {
                case "http://congnghiepcongnghecao.com.vn":
                case "http://congnghiepcongnghecao.vn":
                case "http://tietkiemnangluong.com.vn":
                    newsdes = crawlctl.getbyId("introNews", webcontent);
                    newstitle = crawlctl.getbyId("newsTitle", webcontent);
                    newscontent = crawlctl.getbyId("NewsContent", webcontent);
                    newscontent = newscontent.Replace("../../..", domain);
                    newimages = crawlctl.getbyId("igmNews", webcontent);
                    newstime = crawlctl.getbyId("newstime", webcontent);
                    break;
                case "http://nscl.vn":
                    newstitle = crawlctl.getbyclass("post-head", "h1", webcontent);
                    newscontent = crawlctl.getbyclass("text-detail", "div", webcontent);
                    newimages = crawlctl.getattrbyclass("img-thumbnail thumb-0 wp-post-image", "img", "src", webcontent);
                    newsdes = crawlctl.getfirsdom("strong", newscontent).Replace("<strong>", "").Replace("</strong>", "");
                    newscontent = newscontent.Replace(newsdes, "");

                    newimageshtml = "<div style='text-align:center;'>" + crawlctl.getbyclass("wp-caption featured", "div", webcontent) + "</div>";
                    newscontent = newimageshtml + newscontent;

                    newstime = crawlctl.getbyclass("txt", "span", webcontent);
                    newstime = newstime.Replace("Ngày đăng: ", "");
                    break;
                case "http://tapchicongthuong.vn":
                    newstitle = crawlctl.getmetaTag("og:title", webcontent);
                    newscontent = crawlctl.getbyclass("left-bodydetail", "div", webcontent);
                    newsdes = crawlctl.getmetaTag("og:description", webcontent);
                    newimages = crawlctl.getmetaTag("og:image", webcontent);
                    remove1 = new CrawlBO().getbyclassout("tukhoa", "div", newscontent);
                    if (!string.IsNullOrEmpty(remove1))
                        newscontent = newscontent.Replace(remove1, "");

                    newstime = crawlctl.getbyclass("date-detail", "span", webcontent).Split(',')[1].TrimStart();
                    newstime = newstime.Substring(0, 10);
                    break;
                case "http://baocongthuong.com.vn":
                    newstitle = crawlctl.getmetaTag("og:title", webcontent).Split('|')[0];
                    newscontent = crawlctl.getbyclass("content", "div", webcontent);
                    newsdes = crawlctl.getmetaTag("og:description", webcontent);
                    newimages = crawlctl.getmetaTag("og:image", webcontent);

                    remove1 = crawlctl.getbyclassout("__MB_ARTICLE_A", "table", newscontent);
                    if (!string.IsNullOrEmpty(remove1))
                        newscontent = newscontent.Replace(remove1, "");

                    newstime = crawlctl.getbyclass("", "time", webcontent);
                    newstime = newstime.Split('|')[1].TrimStart();
                    break;
                case "http://eprotech.vn":
                    newstitle = crawlctl.getmetaTag("og:title", webcontent);
                    newscontent = crawlctl.getbyclass("content", "div", webcontent);

                    newimageshtml = "<div style='text-align:center;'>" + crawlctl.getbyclass("avatar", "div", webcontent) + "</div>";
                    newscontent = newimageshtml + newscontent;

                    newsdes = crawlctl.getbyclass("teaser", "div", webcontent).Replace("</p>", "").Replace("<p style=\"text-align: justify;\">", "").TrimStart();
                    newsdes = HttpUtility.HtmlDecode(newsdes);

                    newimages = crawlctl.getmetaTag("og:image", webcontent);
                    newimages = newimages.Replace("http://eprotech.vnhttp://eprotech.vn",
                        "http://eprotech.vn");

                    newstime = crawlctl.getbyclass("xdate", "span", webcontent).Split(':')[1].TrimStart();
                    newstime = newstime.Substring(0, 10);
                    break;
                case "http://support.gov.vn":

                    var titlehtml = crawlctl.getbyclass("newdetail_title", "div", webcontent);
                    newstitle = crawlctl.getattr("a", "title", titlehtml);

                    var deshtml = crawlctl.getbyclass("newdetailtomtat", "div", webcontent);
                    webcontent = webcontent.Replace(deshtml, "");
                    //newsdes = crawlctl.getbyclass("content-inner", "div", deshtml).Replace("<p><b>","").Replace("</b></p>", "");
                    newsdes = crawlctl.getattr("b", "", deshtml);
                    newimages = domain + crawlctl.getattr("img", "src", deshtml);
                    newscontent = crawlctl.getbyclass("content-inner", "div", webcontent);
                    newscontent = newscontent.Replace("/images", domain + "/images");

                    var timehtml = crawlctl.getbyclass("solandoctin", "div", webcontent);
                    newstime = crawlctl.getattr("span", "", timehtml).Replace("-", "/");
                    break;
                case "https://www.most.gov.vn":
                    newstitle = crawlctl.getbyclass("News_Detail_Title", "h1", webcontent).TrimStart(); ;
                    newscontent = crawlctl.getbyId("divArticleDescription2", webcontent);
                    //newimages = crawlctl.getattrbyclass("img-thumbnail thumb-0 wp-post-image", "img", "src", webcontent);
                    newsdes = crawlctl.getbyId("divArticleDescription1", webcontent).TrimStart();


                    newimages = domain + crawlctl.getattr("img", "src", newscontent);
                    newscontent = newscontent.Replace("/Images/", domain + "/Images/");

                    newstime = crawlctl.getbyclass("News_Time_Post", "span", webcontent);
                    newstime = newstime.Split(',')[1].TrimStart().Replace("&nbsp;", "").Substring(0, 10);
                    break;
                case "https://vnexpress.net":
                    newstitle = crawlctl.getmetaTag("og:title", webcontent);
                    newscontent = crawlctl.getbyclass("content_detail fck_detail width_common block_ads_connect", "article", webcontent);
                    newsdes = crawlctl.getmetaTag("og:description", webcontent);
                    newimages = crawlctl.getmetaTag("og:image", webcontent);

                    newstime = crawlctl.getbyclass("time left", "span", webcontent);
                    newstime = newstime.Split(',')[1].TrimStart().Replace("&nbsp;", "").Substring(0, 10).Replace("<", "");
                    break;
            }

            var contentcrawl = new ContentBO().GetContentFull(1);
            contentcrawl.Title = newstitle;
            contentcrawl.IntroText = newsdes;
            contentcrawl.Contents = newscontent;
            contentcrawl.Thumbnail = newimages;
            contentcrawl.Params = newstime+ " 10:00";
            contentcrawl.Alias = HttpContext.User.Identity.Name;


            ViewBag.UserName = HttpContext.User.Identity.Name;
            var lstdata = Membership.GetAllUsers();
            //ExHandler.Handle(new Exception(), "User", "User" + lstdata.Count);
            List<AccountInfo> lstuser = new List<AccountInfo>();
            foreach (MembershipUser item in lstdata)
            {
                lstuser.Add(new AccountInfo { Value = item.UserName, Text = item.UserName });
            }
            ViewBag.UserList = lstuser;
            var listcategory = _staticCategoryByUserList;
            ViewBag.CategoryList = listcategory;

            return PartialView(contentcrawl);
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
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetSpeech(string text, string type, int Id)
        {
            try
            {
                text = text.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ").Trim();
                text = text.Replace("\"", "").Replace(":", "");
                string[] textArray1 = new string[] { ConfigurationManager.AppSettings["UploadPath"], @"Audio\\", (Id / 0x1_86a0).ToString(), @"\\", (Id / 100).ToString(), @"\\", Id.ToString(), @"\\" };
                string str = string.Concat(textArray1);
                string str2 = "nambac";
                if (type == "hn-quynhanh")
                {
                    str2 = "nubac";
                }
                string str3 = $"{DateTime.Now.ToString("yyMMddHHmmss")}_{str2}.mp3";
                RestClient client = new RestClient("http://localhost:8087/CardCallback.ashx")
                {
                    Timeout = -1
                };
                RestRequest request = new RestRequest(Method.POST);
                var x = $"{text}";
                request.AddHeader("Content-Type", "application/json");
                var Speechobj = new SpeechInfo
                {
                    text = text,
                    type = type,
                    path = str,
                    filename = str3
                };
                request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(Speechobj), ParameterType.RequestBody);
                //var requestString = $"\"text\":\" {text}\" ,\"type\": \"{type}\",\"filename\": \"{str3}\",\"path\": \"{str}\"";

                //request.AddParameter("application/json", "{" + requestString + "}", ParameterType.RequestBody);
                if (client.Execute(request).Content == "1")
                {
                    string[] textArray2 = new string[] { ConfigurationManager.AppSettings["UploadUrl"], "Audio/", (Id / 0x1_86a0).ToString(), "/", (Id / 100).ToString(), "/", Id.ToString(), "/" };
                    string str5 = string.Concat(textArray2);
                    return base.Json(new
                    {
                        success = true,
                        statusCode = 1,
                        msg = str5 + str3
                    });
                }
            }
            catch (Exception exception1)
            {
                NLogLogger.DebugMessage(exception1.Message.ToString());
            }
            return base.Json(new
            {
                success = false,
                statusCode = -1,
                msg = "Fail"
            });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Deletefile(string file, int Id)
        {

            var filename = file.Split('/').LastOrDefault();
            if (!string.IsNullOrEmpty(filename))
            {
                string[] textArray1 = new string[] { ConfigurationManager.AppSettings["UploadPath"], @"Audio\\", (Id / 0x1_86a0).ToString(), @"\\", (Id / 100).ToString(), @"\\", Id.ToString(), @"\\" };
                string folder = string.Concat(textArray1);
                string strfile = folder + filename;
                System.IO.File.Delete(strfile);
            }
            return base.Json(new
            {
                success = false,
                statusCode = -1,
                msg = "Fail"
            });

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
