using System.Configuration;
using System.Globalization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ.Entity;
using DATA;
using UTILS;
using CMS.Helper;
using CMS.Models;
using Constants = UTILS.Constants;
namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,NewsEdit,NewsPublish,NewsCreate")]
    public class AdminNews2Controller : Controller
    {
        //
        // GET: /AdminNews2/
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
            if (HttpContext.User.IsInRole("NewsEdit"))
            {
                Response.Redirect("/AdminNews2/NewsByBT");
            }
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
            {
                Response.Redirect("/AdminNews2/NewsByXB");
            }

            if (HttpContext.User.IsInRole("NewsCreate"))
            {
                Response.Redirect("/AdminNews2/NewsByCTV");
            }
            return View();
        }
        public List<EnumInfo> GetUserByNewsRole()
        {
            var list1 = Roles.GetUsersInRole("Administrator");
            var list2 = Roles.GetUsersInRole("NewsEdit");
            var list3 = Roles.GetUsersInRole("NewsPublish");
            var list4 = Roles.GetUsersInRole("NewsCreate");

            var result = new List<EnumInfo>();

            if (list1 != null)
            {
                foreach (var item in list1)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            if (list2 != null)
            {
                foreach (var item in list2)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            if (list3 != null)
            {
                foreach (var item in list3)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            if (list4 != null)
            {
                foreach (var item in list4)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            return result;
        }
        public ContentResult AutoCompleteUser(string searchText)
        {
            var lstaccount = GetUserByNewsRole().Select(x => x.Text).Distinct().ToList();
            var filteredaccount = lstaccount.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList();

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var jsonString = jsonSerializer.Serialize(filteredaccount).ToString();
            return Content(jsonString);

        }

        #region "PV"
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish,NewsCreate")]
        public ActionResult NewsByCTV(int CategoryId = 0, int Status = (int)Constants.NewsStatus.Draft, int page = 1, string Order = "ModifyDate", string title = "", int type = -1, string fromdate = "", string todate = "")
        {

            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.Type = type;
            ViewBag.TypeList = new List<EnumInfo>
                                   {
                                       new EnumInfo {Value = -1, Text = "--Tất cả--"},
                                       new EnumInfo {Value = 1, Text = "Tin bài"},
                                       new EnumInfo {Value = 2, Text = "Video"},
                                       
                                       new EnumInfo {Value = 3, Text = "Phóng sự ảnh"}
                                   };

            ViewBag.Keyword = title;
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = CategoryId;
            ViewBag.Order = Order;
            ViewBag.OrderList = new List<EnumInfo> { new EnumInfo { SValue = "ModifyDate", Text = "Thời gian cập nhật" }, new EnumInfo { SValue = "PublishDate", Text = "Thời gian xuất bản" }, new EnumInfo { SValue = "Hits", Text = "Lượt xem" } };
            int total = 0;
            int pagesize = 20;
            ViewBag.Status = Status;
            ViewBag.StatusList = new List<EnumInfo>
                                     {
                                         new EnumInfo { Value = -1, Text = "--Tất cả--" },
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Draft, Text = "Đang viết" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Reject, Text = "Bị trả lại" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.EditWait, Text = "Chờ biên tập" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Publish, Text = "Đã xuất bản" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Disable, Text = "Thùng rác" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.EditReject, Text = "Trả biên tập" },
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Editting, Text = "Đang biên tập" },
                                         new EnumInfo { Value = (int)Constants.NewsStatus.PublishWait, Text = "Đợi xuất bản" }
                                     };
            var lstStatus = "";
            //if (Status == -1)
            //{
            //    lstStatus = (int)Constants.NewsStatus.Draft + "," + (int)Constants.NewsStatus.EditWait + "," + (int)Constants.NewsStatus.Reject + "," +
            //                (int)Constants.NewsStatus.Publish + "," + (int)Constants.NewsStatus.Disable;
            //}
            var lstdata = new List<CONTENT_FULL>();
            if (User.IsInRole("Administrator"))
            {
                lstdata = new ContentBO().GetFilterContentFullsPaged(page, pagesize, title, CategoryId, null, Status, HttpContext.User.Identity.Name, ref total, type, fromdate, todate, lstStatus, Order + " DESC");
            }
            else
            {
                lstdata = new ContentBO().GetFilterContentFullsPaged(page, pagesize, title, CategoryId, listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList(), Status, HttpContext.User.Identity.Name, ref total, type, fromdate, todate, lstStatus, Order + " DESC");
            }


            var model = new NewsModel
            {
                listdata = lstdata,
                pageIndex = page,
                pageSize = pagesize,
                total = total

            };
            Session["ReturnUrl"] = HttpUtility.UrlEncode(Request.RawUrl.ToString());
            ViewBag.Title = "Quản trị tin tức";
            //ViewBag.Title = "Quản trị tin tức : tin " + HtmlHelpers.GetNewsStatusName(Status, ViewBag.StatusList);
            return View(model);
        }
        #endregion
        #region "BT"
        [Authorize(Roles = "Administrator,NewsEdit,NewsPublish")]
        public ActionResult NewsByBT(int CategoryId = 0, int Status = (int)Constants.NewsStatus.EditWait, int page = 1, string author = "", string Order = "ModifyDate", string title = "", int type = -1, string fromdate = "", string todate = "")
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.Keyword = title;
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = CategoryId;
            ViewBag.Order = Order;
            ViewBag.OrderList = new List<EnumInfo>
                                    {
                                        new EnumInfo { SValue = "ModifyDate", Text = "Thời gian cập nhật" },
                                        new EnumInfo { SValue = "PublishDate", Text = "Thời gian xuất bản" }, 
                                        new EnumInfo { SValue = "Hits", Text = "Lượt xem" }
                                    };
            ViewBag.Type = type;
            ViewBag.TypeList = new List<EnumInfo>
                                   {
                                       new EnumInfo {Value = -1, Text = "--Tất cả--"},
                                       new EnumInfo {Value = 1, Text = "Tin bài"},
                                       new EnumInfo {Value = 2, Text = "Video"},
                                       
                                       new EnumInfo {Value = 3, Text = "Phóng sự ảnh"}
                                   };
            ViewBag.Author = author;
            var lstaccount = GetUserByNewsRole();
            lstaccount.Insert(0, new EnumInfo { SValue = "-1", Text = "--Tất cả--" });
            ViewBag.AccountList = lstaccount;
            int total = 0;
            int pagesize = 20;
            ViewBag.Status = Status;
            ViewBag.StatusList = new List<EnumInfo>
                                     {
                                         new EnumInfo { Value = -1, Text = "--Tất cả--" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.EditWait, Text = "Chờ biên tập" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Editting, Text = "Đang biên tập" },
                                         new EnumInfo { Value = (int)Constants.NewsStatus.PublishWait, Text = "Đợi xuất bản" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.EditReject, Text = "Trả biên tập" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Reject, Text = "Trả phóng viên" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Publish, Text = "Đã xuất bản" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Disable, Text = "Thùng rác" }
                                     };
            var lstStatus = "";
            if (Status == -1)
            {
                lstStatus = (int)Constants.NewsStatus.Publish + "," + (int)Constants.NewsStatus.EditWait + "," + (int)Constants.NewsStatus.PublishWait + "," + (int)Constants.NewsStatus.EditReject + "," + (int)Constants.NewsStatus.Editting + "," + (int)Constants.NewsStatus.Reject + "," +
                            (int)Constants.NewsStatus.Disable;
            }
            var lstdata = new List<CONTENT_FULL>();
            if (User.IsInRole("Administrator"))
            {
                lstdata = new ContentBO().GetFilterContentFullsPaged(page, pagesize, title, CategoryId, null, Status, author, ref total, type, fromdate, todate, lstStatus, Order + " DESC");
            }
            else
            {
                lstdata = new ContentBO().GetFilterContentFullsPaged(page, pagesize, title, CategoryId, listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList(), Status, author, ref total, type, fromdate, todate, lstStatus, Order + " DESC");
            }


            var model = new NewsModel
            {
                listdata = lstdata,
                pageIndex = page,
                pageSize = pagesize,
                total = total

            };
            Session["ReturnUrl"] = HttpUtility.UrlEncode(Request.RawUrl.ToString());
            ViewBag.Title = "Quản trị tin tức";
            //ViewBag.Title = "Quản trị tin tức : tin " + HtmlHelpers.GetNewsStatusName(Status, ViewBag.StatusList);
            return View(model);
        }

        #endregion
        #region "XB"
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult NewsByXB(int CategoryId = 0, int Status = (int)Constants.NewsStatus.Publish, int page = 1, string author = "", string Order = "ModifyDate", string title = "", int type = -1, string fromdate = "", string todate = "")
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.Keyword = title;
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = CategoryId;
            ViewBag.Order = Order;
            ViewBag.OrderList = new List<EnumInfo> { new EnumInfo { SValue = "ModifyDate", Text = "Thời gian cập nhật" }, new EnumInfo { SValue = "PublishDate", Text = "Thời gian xuất bản" }, new EnumInfo { SValue = "Hits", Text = "Lượt xem" } };

            ViewBag.Author = author;
            var lstaccount = GetUserByNewsRole();
            lstaccount.Insert(0, new EnumInfo { SValue = "-1", Text = "--Tất cả--" });
            ViewBag.AccountList = lstaccount;
            int total = 0;
            int pagesize = 20;
            ViewBag.Status = Status;
            ViewBag.StatusList = new List<EnumInfo>
                                     {
                                         new EnumInfo { Value = -1, Text = "--Tất cả--" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.PublishWait, Text = "Chờ xuất bản" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Publish, Text = "Đã xuất bản" }, 
                                         new EnumInfo { Value = (int)Constants.NewsStatus.EditReject, Text = "Trả lại biên tập" },
                                         new EnumInfo { Value = (int)Constants.NewsStatus.Disable, Text = "Thùng rác" }
                                     };
            ViewBag.Type = type;
            ViewBag.TypeList = new List<EnumInfo>
                                   {
                                       new EnumInfo {Value = -1, Text = "--Tất cả--"},
                                       new EnumInfo {Value = 1, Text = "Tin bài"},
                                       new EnumInfo {Value = 2, Text = "Video"},
                                       
                                       new EnumInfo {Value = 3, Text = "Phóng sự ảnh"}
                                   };
            var lstStatus = "";
            if (Status == -1)
            {
                lstStatus = (int)Constants.NewsStatus.PublishWait + "," + (int)Constants.NewsStatus.Publish + "," + (int)Constants.NewsStatus.EditReject + "," +
                            (int)Constants.NewsStatus.Disable;
            }
            var lstdata = new List<CONTENT_FULL>();
            if (User.IsInRole("Administrator"))
            {
                lstdata = new ContentBO().GetFilterContentFullsPaged(page, pagesize, title, CategoryId, null, Status, author, ref total, type, fromdate, todate, lstStatus, Order + " DESC");
            }
            else
            {
                lstdata = new ContentBO().GetFilterContentFullsPaged(page, pagesize, title, CategoryId, listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList(), Status, author, ref total, type, fromdate, todate, lstStatus, Order + " DESC");
            }

            var model = new NewsModel
            {
                listdata = lstdata,
                pageIndex = page,
                pageSize = pagesize,
                total = total

            };
            Session["ReturnUrl"] = HttpUtility.UrlEncode(Request.RawUrl.ToString());
            ViewBag.Title = "Quản trị tin tức";
            //ViewBag.Title = "Quản trị tin tức : tin " + HtmlHelpers.GetNewsStatusName(Status, ViewBag.StatusList);
            return View(model);
        }
        #endregion

        #region "Form"
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
        private void MoveFile(string from_path, string from_file, string to_path, string to_file)
        {
            Utils.MoveFile(from_path, from_file, to_path, to_file);
        }

        public ActionResult Add()
        {

            var newsobj = new CONTENT_FULL { SPublishedTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm") };
            newsobj.Album = HttpContext.User.Identity.Name;
            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            ViewBag.isEditer = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit");
            ViewBag.isReporter = HttpContext.User.IsInRole("NewsCreate");
            ViewBag.Title = "Thêm mới tin";
            ViewBag.imageUrl = Utils.GetTempUrl(HttpContext.User.Identity.Name);
            return View(newsobj);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(CONTENT_FULL newsobj, int actionType)
        {
            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            ViewBag.isEditer = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit");
            ViewBag.isReporter = HttpContext.User.IsInRole("NewsCreate");
            IFormatProvider culture = new CultureInfo("en-US", true);
            newsobj.PublishDate = DateTime.ParseExact(newsobj.SPublishedTime, "dd/MM/yyyy HH:mm", culture);

            newsobj.CreatedRole = " ";
            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish"))
            {
                newsobj.CreatedRole = "XB";
            }
            else
            {
                if (HttpContext.User.IsInRole("NewsEdit"))
                {
                    newsobj.CreatedRole = "BT";
                }
            }


            newsobj.Hits = 0;
            newsobj.CreatedDate = DateTime.Now;
            //newsobj.PublishedTime = DateTime.Now;
            newsobj.CreatedBy = HttpContext.User.Identity.Name;


            if (String.IsNullOrEmpty(newsobj.Contents))
                newsobj.Contents = " ";
            if (String.IsNullOrEmpty(newsobj.Keywords))
                newsobj.Keywords = " ";

            if (newsobj.CategoryId == null)
            {
                return RedirectToAction("AssesDenied", "Admin");
            }

            var firstOrDefault = _staticCategoryList.FirstOrDefault(x => x.Id == newsobj.CategoryId);
            if (firstOrDefault != null)
            {
                newsobj.CategoryPathway = firstOrDefault.Pathway;
                newsobj.Language = firstOrDefault.Language;
            }
                
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                Type = actionType,
                Contents = newsobj.Contents,
                Url = newsobj.Url,
                Title = newsobj.Title,
                IntroText = newsobj.IntroText
            };
            //check action type
            switch (actionType)
            {

                case (int)Constants.NewsAction.SendPublish:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Tạo mới tin";
                        newsobj.Status = (int)Constants.NewsStatus.PublishWait;
                    }
                    else
                        return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.Save:


                    lognewsobj.Description = "Tạo mới tin";
                    newsobj.Status = (int)Constants.NewsStatus.Draft;


                    break;
                case (int)Constants.NewsAction.SendEdit:
                    if (ViewBag.isReporter)
                    {
                        lognewsobj.Description = "Tạo mới tin và gửi biên tập";
                        newsobj.Status = (int)Constants.NewsStatus.EditWait;
                    }
                    else
                        return RedirectToAction("AssesDenied", "Admin");
                    break;

            }
            if (!string.IsNullOrEmpty(newsobj.TempImage))
            {
                newsobj.Image = "1.jpg";

            }

            //inser news
            long id = new ContentBO().CreateUpdateContent(newsobj);
            if (id >= 0)
            {
                lognewsobj.ItemId = id;

                //insert log
                //new ContentLogBO().CreateUpdateContentLog(lognewsobj);
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);
                //copy file
                if (!string.IsNullOrEmpty(newsobj.TempImage))
                {
                    //copy file tạm ra đường dẫn id của bài
                    Action<string, string, string, string> sendMoveFile = MoveFile;
                    var asynSendMoveFile = sendMoveFile.BeginInvoke(Utils.GetTempPath(HttpContext.User.Identity.Name), newsobj.TempImage, Utils.GetNewsImagePath(id), "1.jpg", null, null);
                    //Utils.MoveFile(Utils.GetTempPath(HttpContext.User.Identity.Name), newsobj.TempImage, Utils.GetNewsImagePath(id), "1.jpg");

                }
            }

            return Json(id.ToString());
        }
        public ActionResult Edit(long id)
        {
            ViewBag.ReturnUrl = Session["ReturnUrl"] != null ? HttpUtility.UrlDecode(Session["ReturnUrl"].ToString()) : "/AdminNews2/";


            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            ViewBag.isEditer = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit");
            ViewBag.isReporter = HttpContext.User.IsInRole("NewsCreate");
            var newsobj = new ContentBO().GetContentFull(id);
            if (newsobj == null)
            {
                Response.Redirect(Url.Action("AssesDenied", "Admin"));
            }
            if (newsobj.Status != 1)
            {
                newsobj.PublishDate = DateTime.Now;
            }
            ViewBag.Title = "Cập nhật tin";
            newsobj.SPublishedTime = newsobj.PublishDate.ToString("dd/MM/yyyy HH:mm");
            // ViewBag.imageUrl = Utils.GetNewsImageUrl(newsobj.Id);

            //Check quyền
            //if (newsobj.ContentType == (int)Constants.ContentType.Video || newsobj.GameTab > 0)
            //     return RedirectToAction("AssesDenied", "Admin");
            switch (newsobj.Status)
            {
                case (int)Constants.NewsStatus.Draft:
                    if (newsobj.CreatedBy != HttpContext.User.Identity.Name)
                        return RedirectToAction("AssesDenied", "Admin");
                    break;
                //tin tra phong vien
                case (int)Constants.NewsStatus.Reject:
                    if (!ViewBag.isAdmin && ViewBag.isReporter)
                    {
                        if (newsobj.CreatedBy != HttpContext.User.Identity.Name)
                        {
                            return RedirectToAction("AssesDenied", "Admin");
                        }
                        else
                        {
                            newsobj.Status = (int)Constants.NewsStatus.Draft;
                            new ContentBO().CreateUpdateContent(newsobj);
                        }
                    }

                    break;
                case (int)Constants.NewsStatus.EditWait:
                    //bien tạp vao edit tin thanh dang bien tap
                    if (ViewBag.isEditer)
                    {
                        newsobj.Status = (int)Constants.NewsStatus.Editting;
                        new ContentBO().CreateUpdateContent(newsobj);

                    }
                    break;
                case (int)Constants.NewsStatus.Editting:
                    if (!ViewBag.isEditer)
                        return RedirectToAction("AssesDenied", "Admin");
                    break;
                //tin tra bien tap
                case (int)Constants.NewsStatus.EditReject:
                    //bien tạp vao edit tin thanh dang bien tap    
                    if (ViewBag.isEditer)
                    {
                        if (!ViewBag.isAdmin)
                        {
                            newsobj.Status = (int)Constants.NewsStatus.Editting;
                            new ContentBO().CreateUpdateContent(newsobj);
                        }

                    }
                    else
                    {
                        return RedirectToAction("AssesDenied", "Admin");

                    }

                    break;
                case (int)Constants.NewsStatus.Publish:
                case (int)Constants.NewsStatus.PublishWait:
                    if (!ViewBag.isAdmin)
                        return RedirectToAction("AssesDenied", "Admin");
                    break;

                case (int)Constants.NewsStatus.Disable:
                    if (newsobj.CreatedBy != HttpContext.User.Identity.Name && ViewBag.isReporter)
                        return RedirectToAction("AssesDenied", "Admin");
                    break;

                default:
                    return RedirectToAction("AssesDenied", "Admin");
                    break;
            }
            ViewBag.imageUrl = Utils.GetImageUrl(id, "Article", false);
            return View(newsobj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(CONTENT_FULL newsobj, int actionType, string note = "")
        {
            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            ViewBag.isEditer = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit");
            ViewBag.isReporter = HttpContext.User.IsInRole("NewsCreate");
            IFormatProvider culture = new CultureInfo("en-US", true);
            newsobj.PublishDate = DateTime.ParseExact(newsobj.SPublishedTime, "dd/MM/yyyy HH:mm", culture);
           
            if (String.IsNullOrEmpty(newsobj.Contents))
                newsobj.Contents = " ";
            if (String.IsNullOrEmpty(newsobj.Keywords))
                newsobj.Keywords = " ";

            var firstOrDefault = _staticCategoryList.FirstOrDefault(x => x.Id == newsobj.CategoryId);
            if (firstOrDefault != null)
            {
                newsobj.CategoryPathway = firstOrDefault.Pathway;
                newsobj.Language = firstOrDefault.Language;
            }
                
           
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                Type = actionType,
                Contents = newsobj.Contents,
                Url = newsobj.Url,
                Note = note,
                Title = newsobj.Title,
                IntroText = newsobj.IntroText,
                ItemId = newsobj.Id
            };
            //check action type
            switch (actionType)
            {
                case (int)Constants.NewsAction.GetBack:

                    lognewsobj.Description = "Lấy lại bài";

                    newsobj.Status = (int)Constants.NewsStatus.Draft;
                    break;

                case (int)Constants.NewsAction.Down:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Gỡ bài";

                        newsobj.Status = (int)Constants.NewsStatus.PublishWait;
                        break;
                    }

                    return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.RejectBT:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Trả lại biên tập";

                        newsobj.Status = (int)Constants.NewsStatus.EditReject;
                        break;
                    }
                    return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.Reject:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Trả lại phóng viên";

                        newsobj.Status = (int)Constants.NewsStatus.Reject;
                        break;
                    }
                    else
                    {
                        if (ViewBag.isEditer)
                        {
                            lognewsobj.Description = "Trả lại phóng viên";
                            newsobj.Status = (int)Constants.NewsStatus.Reject;
                            break;
                        }
                    }

                    return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.Publish:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Xuất bản";
                        newsobj.Status = (int)Constants.NewsStatus.Publish;
                    }
                    else
                        return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.SendPublish:
                    if (ViewBag.isEditer)
                    {
                        lognewsobj.Description = "Gửi Xuất bản";
                        newsobj.Status = (int)Constants.NewsStatus.PublishWait;
                    }
                    else
                        return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.Save:

                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Lưu lại tin";

                        //newsobj.Status = (int)Constants.NewsStatus.Draft;
                        //if (newsobj.CreatedBy != HttpContext.User.Identity.Name)
                        //    newsobj.Status = (int)Constants.NewsStatus.PublishWait;


                    }
                    else
                    {

                        if (ViewBag.isEditer)
                        {
                            lognewsobj.Description = "Lưu lại tin";
                            
                        }
                        else
                        {

                            lognewsobj.Description = "Lưu lại tin";
                           


                        }
                    }

                    break;
                case (int)Constants.NewsAction.Restore:

                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Xuất bản hoàn tác lại tin";
                        newsobj.Status = (int)Constants.NewsStatus.PublishWait;

                    }
                    else
                    {
                        if (ViewBag.isEditer)
                        {
                            lognewsobj.Description = "Biên tập hoàn tác lại tin";
                            newsobj.Status = (int)Constants.NewsStatus.Editting;

                        }
                        else
                        {

                            lognewsobj.Description = "Phóng viên hoàn tác lại tin";
                            newsobj.Status = (int)Constants.NewsStatus.Draft;

                        }
                    }

                    break;
                case (int)Constants.NewsAction.SendEdit:
                    if (ViewBag.isEditer || ViewBag.isReporter)
                    {
                        lognewsobj.Description = "Gửi biên tập";
                        newsobj.Status = (int)Constants.NewsStatus.EditWait;
                    }
                    else
                        return RedirectToAction("AssesDenied", "Admin");

                    break;
                case (int)Constants.NewsAction.Delete:

                    lognewsobj.Description = "Xóa bài";
                    newsobj.Status = (int)Constants.NewsStatus.Disable;


                    break;
            }
            //update news
            if (new ContentBO().CreateUpdateContent(newsobj) >= 0)
            {
                //insert log
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);
            }
            //if (actionType == (int)Constants.NewsAction.Reject || actionType == (int)Constants.NewsAction.Publish || actionType == (int)Constants.NewsAction.Delete)
            //{ }
            return RedirectToAction("Index", "AdminNews2");
            //return View();
        }

        [ChildActionOnly]
        public ActionResult FormEdit(CONTENT_FULL obj)
        {
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
            if (listcategory.Find(x => x.Id == obj.CategoryId) == null)
            {
                return RedirectToAction("AssesDenied", "Admin");
            }
            ViewBag.CategoryList = listcategory;
            ViewBag.StatusList = new List<EnumInfo> { new EnumInfo { Value = (int)Constants.NewsStatus.Draft, Text = "Đang viết" }, new EnumInfo { Value = (int)Constants.NewsStatus.EditWait, Text = "Đợi biên tập" }, new EnumInfo { Value = (int)Constants.NewsStatus.PublishWait, Text = "Chờ xuất bản" }, new EnumInfo { Value = (int)Constants.NewsStatus.Publish, Text = "Xuất bản" }, new EnumInfo { Value = (int)Constants.NewsStatus.Reject, Text = "Bị trả lại" }, new EnumInfo { Value = (int)Constants.NewsStatus.EditReject, Text = "Bị trả lại" }, new EnumInfo { Value = (int)Constants.NewsStatus.Editting, Text = "Đang biên tập" }, new EnumInfo { Value = (int)Constants.NewsStatus.Disable, Text = "Bị xóa" } };
            var lstaccount = GetUserByNewsRole();
            ViewBag.AccountList = lstaccount;
            ViewBag.isEditer = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsEdit");
            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            return PartialView(obj);
        }
        [ChildActionOnly]
        public ActionResult FormAdd(CONTENT_FULL obj)
        {
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
            ViewBag.CategoryList = listcategory;

            var lstaccount = GetUserByNewsRole();
            ViewBag.AccountList = lstaccount;

            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            // ViewBag.isReporter = HttpContext.User.IsInRole("NewsCreate");
            return PartialView(obj);
        }
        [ChildActionOnly]
        public ActionResult FormReference()
        {
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            return PartialView();
        }

        
        public ActionResult Review(long Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            return PartialView(newsobj);
        }

        public ActionResult Review2(long id)
        {
            var obj = new ContentLogBO().GetById(id);

            return View(obj);
        }
        [ChildActionOnly]
        public ActionResult VideoReview(long Id)
        {
            ViewBag.Id = Id;
            return PartialView();
        }
        #endregion


        #region "Hotnews"


        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigHotNews(string keyword = "", int categoryId = 0)
        {
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            if (User.IsInRole("Administrator") || User.IsInRole("NewsPublish"))
            {
                listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Trang chủ" });
                listcategory.Insert(1, new CATEGORY_FULL { Id = OtherPage.EngPage, Name = "Trang chủ Tiếng Anh" });
            }

            ViewBag.CategoryList = listcategory;
            if (categoryId == 0 && !User.IsInRole("Administrator") && !User.IsInRole("NewsPublish"))
            {
                return RedirectToAction("ConfigHotNews", new { categoryId = listcategory.FirstOrDefault().Id });
            }
            var key = "HotNews";
            if (categoryId > 0)
                key += "_" + categoryId;
            ViewBag.lstNews = "";
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
                ViewBag.lstNews = configValue.ConfigValue;
            ViewBag.categoryId = categoryId;
            ViewBag.Title = "Cấu hình tin nổi bật";
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        [HttpPost]
        public ActionResult SaveConfigHotNews(string svalue, int categoryId = 0)
        {
            var results = "true";

            try
            {
                var key = "HotNews";
                if (categoryId > 0)
                    key += "_" + categoryId;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                     new ContentBO().FlushAllContentCache(BIZ.Constants.CACHE_GROUPKEY_CONTENT);
                    // Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);
                }


            }
            catch (System.Exception ex)
            {

                results = ex.Message;

            }
            return Json(results);
        }

        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigHotNewsATGT(string keyword = "", int categoryId = 3)
        {

            var listcategory = new List<CATEGORY_FULL>();
            //listcategory.AddRange(MvcApplication.StaticATGT_CategoryList);
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            if (User.IsInRole("Administrator") || User.IsInRole("NewsPublish"))
            {
                listcategory.Insert(0, new CATEGORY_FULL { Id = 3, Name = "Trang chủ ATGT" });
            }

            ViewBag.CategoryList = listcategory;
            if (categoryId == 0 && !User.IsInRole("Administrator") && !User.IsInRole("NewsPublish"))
            {
                return RedirectToAction("ConfigHotNews", new { categoryId = listcategory.FirstOrDefault().Id });
            }
            var key = "HotNewsATGT";
            if (categoryId > 3)
                key += "_" + categoryId;
            ViewBag.lstNews = "";
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
                ViewBag.lstNews = configValue.ConfigValue;
            ViewBag.categoryId = categoryId;
            ViewBag.Title = "Cấu hình tin nổi bật trang ATGT";
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        [HttpPost]
        public ActionResult SaveConfigHotNewsATGT(string svalue, int categoryId = 3)
        {
            var results = "true";

            try
            {
                var key = "HotNewsATGT";
                if (categoryId > 3)
                    key += "_" + categoryId;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                     new ContentBO().FlushAllContentCache(BIZ.Constants.CACHE_GROUPKEY_CONTENT);
                    // Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);
                }


            }
            catch (System.Exception ex)
            {

                results = ex.Message;

            }
            return Json(results);
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigTopViewNews(string keyword = "", int categoryId = 0)
        {
            var listcategory = new List<CATEGORY_FULL>();


            if (User.IsInRole("Administrator") || User.IsInRole("NewsPublish"))
            {
                listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Trang chủ" });
                listcategory.Insert(1, new CATEGORY_FULL { Id = OtherPage.EngPage, Name = "Trang chủ Tiếng Anh" });
            }

            ViewBag.CategoryList = listcategory;
          
            var key = "TopViewNews";
            if (categoryId > 0)
                key += "_" + categoryId;
            ViewBag.lstNews = "";
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
                ViewBag.lstNews = configValue.ConfigValue;
            ViewBag.categoryId = categoryId;
            ViewBag.Title = "Cấu hình tin xem nhiều";
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        [HttpPost]
        public ActionResult SaveConfigTopViewNews(string svalue, int categoryId = 0)
        {
            var results = "true";

            try
            {
                var key = "TopViewNews";
                if (categoryId > 0)
                    key += "_" + categoryId;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                     new ContentBO().FlushAllContentCache(BIZ.Constants.CACHE_GROUPKEY_CONTENT);
                    // Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);
                }
            }
            catch (System.Exception ex)
            {
                results = ex.Message;

            }
            return Json(results);
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        public ActionResult ConfigTopViewNewsATGT(string keyword = "", int categoryId = 0)
        {
            var listcategory = new List<CATEGORY_FULL>();
            //listcategory.AddRange(MvcApplication.StaticATGT_CategoryList);
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            if (User.IsInRole("Administrator"))
            {
                listcategory.Insert(0, new CATEGORY_FULL { Id = 3, Name = "An toàn giao thông" });
            }

            ViewBag.CategoryList = listcategory;
            if (categoryId == 0 && !User.IsInRole("Administrator"))
            {
                return RedirectToAction("ConfigHotNews", new { categoryId = listcategory.FirstOrDefault().Id });
            }
            var key = "TopViewNewsATGT";
            if (categoryId > 0)
                key += "_" + categoryId;
            ViewBag.lstNews = "";
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
                ViewBag.lstNews = configValue.ConfigValue;
            ViewBag.categoryId = categoryId;
            ViewBag.Title = "Cấu hình tin xem nhiều trang ATGT";
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        [HttpPost]
        public ActionResult SaveConfigTopViewNewsATGT(string svalue, int categoryId = 0)
        {
            var results = "true";

            try
            {
                var key = "TopViewNewsATGT";
                if (categoryId > 3)
                    key += "_" + categoryId;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                     new ContentBO().FlushAllContentCache(BIZ.Constants.CACHE_GROUPKEY_CONTENT);
                    // Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);
                }
            }
            catch (System.Exception ex)
            {
                results = ex.Message;

            }
            return Json(results);
        }
        public ActionResult ConfigHotVideo(int site = 0)
        {
            ViewBag.site = site;
            //hard code site=1 thi chon chuyen muc goc la 3
            if (site == 1)
            {
                ViewBag.categoryId = int.Parse(ConfigurationManager.AppSettings["ATGT_Cate"]);
            }
            ViewBag.SiteList = new List<EnumInfo> { new EnumInfo { Value = 0, Text = "tietkiemnangluong.com.vn" }, new EnumInfo { Value = 1, Text = "Trang ATGT" } };
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            if (User.IsInRole("Administrator"))
            {
                listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            }

            ViewBag.CategoryList = listcategory;

            var key = "HotVideo";
            if (site > 0)
                key = "HotVideo_" + site;
            var lstNews = new List<CONTENT_FULL>();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                lstNews = new ContentBO().GetTopContentByIdsFulls(configValue.ConfigValue, 15, true);

            }
            if (lstNews == null)
                lstNews = new List<CONTENT_FULL>();
            ViewData["SelectedNews"] = new SelectList(lstNews, "Id", "Title");
            ViewBag.Title = "Cấu hình video nổi bật";
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        [HttpPost]
        public ActionResult SaveConfigHotVideo(string svalue, int site = 0)
        {
            var results = "true";

            try
            {
                var key = "HotVideo";
                if (site > 0)
                    key = "HotVideo_" + site;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                     new ContentBO().FlushAllContentCache(BIZ.Constants.CACHE_GROUPKEY_CONTENT);
                }


            }
            catch (System.Exception ex)
            {

                results = ex.Message;

            }
            return Json(results);
        }

        public ActionResult ConfigHotPhoto(int site = 0)
        {
            ViewBag.site = site;
            //hard code site=1 thi chon chuyen muc goc la 3
            if (site == 1)
            {
                ViewBag.categoryId = int.Parse(ConfigurationManager.AppSettings["ATGT_Cate"]);
            }
            ViewBag.SiteList = new List<EnumInfo> { new EnumInfo { Value = 0, Text = "tietkiemnangluong.com.vn" }, new EnumInfo { Value = 1, Text = "Trang ATGT" } };
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            if (User.IsInRole("Administrator"))
            {
                listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            }

            ViewBag.CategoryList = listcategory;

            var key = "HotPhoto";
            if (site > 0)
                key = "HotPhoto_" + site;
            var lstNews = new List<CONTENT_FULL>();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                lstNews = new ContentBO().GetTopContentByIdsFulls(configValue.ConfigValue, 30, true);

            }
            if (lstNews == null)
                lstNews = new List<CONTENT_FULL>();
            ViewData["SelectedNews"] = new SelectList(lstNews, "Id", "Title");
            ViewBag.Title = "Cấu hình tin ảnh nổi bật";
            return View();
        }
        [Authorize(Roles = "Administrator,NewsPublish")]
        [HttpPost]
        public ActionResult SaveConfigHotPhoto(string svalue, int site = 0)
        {
            var results = "true";

            try
            {
                var key = "HotPhoto";
                if (site > 0)
                    key = "HotPhoto_" + site;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                    //Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);
                     new ContentBO().FlushAllContentCache(BIZ.Constants.CACHE_GROUPKEY_CONTENT);
                }


            }
            catch (System.Exception ex)
            {

                results = ex.Message;

            }
            return Json(results);
        }
        #endregion

        #region"History"
        public ActionResult History(long id, string NewsTitle)
        {
            ViewBag.NewsTitle = NewsTitle;
            var model = new ContentLogBO().GetContentLogsByContentId(id);
            return View(model);
        }
        #endregion
    }
}
