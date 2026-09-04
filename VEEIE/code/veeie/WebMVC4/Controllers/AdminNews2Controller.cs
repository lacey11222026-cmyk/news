using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

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
            return View();
        }

        public ActionResult ListNews(int? cateId, int? status, string fromDate, string endDate, string title, string createdBy, int? currentPage, int? pageSize)
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
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            int CateId = cateId == null ? -1 : (int)cateId;
            int Status = status == null ? -1 : (int)status;

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
                data = new ContentBO().GetFilterContentFullsPaged(CurrPage, RecordPerPage, title, CateId, null, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus);
            }
            else
            {
                //xem đc tin xuất bản của mình
                if (Status == 1)
                {
                    createdBy = HttpContext.User.Identity.Name;
                }
                data = new ContentBO().GetFilterContentFullsPaged(CurrPage, RecordPerPage, title, CateId, lstCate, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus);
            }

            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;
            ViewBag.CategoryList = _staticCategoryByUserList;
            return PartialView(data);
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

           
            return View();
        }

        public ActionResult ListNews2(int? cateId, int? status, string fromDate, string endDate, string title, string createdBy, int? currentPage, int? pageSize)
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
                data = new ContentBO().GetFilterContentFullsPaged(currPage, RecordPerPage, title, CateId, null, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus, "");
            }
            else
            {
                //xem đc tin xuất bản của mình
                //if (Status == 1)
                //{
                //    createdBy = HttpContext.User.Identity.Name;
                //}
                data = new ContentBO().GetFilterContentFullsPaged(currPage, RecordPerPage, title, CateId, lstCate, Status, createdBy, ref TotalRecord, fromDate, endDate, lststatus, "");
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

            var model = new CONTENT_FULL { PublishDate = DateTime.Now, Id = 0, Alias = HttpContext.User.Identity.Name, CategoryId = 0, Hits = 0, CreatedBy = HttpContext.User.Identity.Name,FileParam=new FileInfo() };
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
                if(model.FileParam==null)
                {
                    model.FileParam=   new FileInfo();
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
            }
            else
            {
                ViewBag.Title = "Thêm mới tin tức";
                listcategory = listcategory.Where(x => x.Published == 1).ToList();
            }
            ViewBag.CategoryList = listcategory;
            return View(model);
        }

        public ActionResult Crawl()
        {
            ViewBag.Title = "Crawl tin tức";
            return View();
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
                    notiobj.Title = String.Format("<b>{1}</b> tạo bài viết <b>{0}</b>", newsobj.Title, HttpContext.User.Identity.Name);
                    break;
                case "publish":

                    newsobj.Status = 4;
                    lognewsobj.Note = "Xuất bản";

                    //lấy tài khoản xuất bản+admin+thằng viết
                    notiobj.Role = GetUserByNewsRole(newsobj.CreatedBy);
                    notiobj.Title = String.Format("<b>{1}</b> xuất bản bài viết <b>{0}</b>", newsobj.Title, HttpContext.User.Identity.Name);
                    break;
                case "reject":
                    if (newsobj.Status == 4)
                    {
                        lognewsobj.Note = "Hạ bài";
                        newsobj.Status = 2;//đợi xuất bản
                        notiobj.Title = String.Format("<b>{1}</b> hạ bài viết <b>{0}</b>", newsobj.Title, HttpContext.User.Identity.Name);
                        //lấy tài khoản xuất bản+admin+thằng viết
                        notiobj.Role = GetUserByNewsRole(newsobj.CreatedBy);
                    }
                    else
                    {
                        lognewsobj.Note = "Trả lại biên tập";
                        newsobj.Status = 1;//đang biên tập
                        notiobj.Title = String.Format("<b>{1}</b> trả lại bài viết <b>{0}</b>", newsobj.Title, HttpContext.User.Identity.Name);
                        // lấy tài khoản thằng viết
                        notiobj.Role = string.Format(",{0},", newsobj.CreatedBy);
                    }


                    break;
                case "send":

                    lognewsobj.Note = "Gửi xuất bản";
                    newsobj.Status = 2;
                    // lấy tài khoản xuất bản+admin
                    notiobj.Role = GetUserByNewsRole("");
                    notiobj.Title = String.Format("<b>{1}</b> gửi xuất bản bài viết <b>{0}</b>", newsobj.Title, HttpContext.User.Identity.Name);
                    break;

                case "save":

                    lognewsobj.Note = "Lưu bài";
                    //lấy tài khoản xuất bản+admin+thằng viết
                    notiobj.Role = GetUserByNewsRole(newsobj.CreatedBy);
                    notiobj.Title = String.Format("<b>{1}</b> lưu bài viết <b>{0}</b>", newsobj.Title, HttpContext.User.Identity.Name);
                    break;
            }
            //update news
            var result = new ContentBO().CreateUpdateContent(newsobj);
            if (result >= 0)
            {
                if (lognewsobj.ItemId==0)
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
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
        private void InsertNotifi(Notifi sobj)
        {
            new NotifiBO().Create(sobj);
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
