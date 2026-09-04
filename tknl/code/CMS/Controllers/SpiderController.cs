using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BIZ;
using BIZ.Entity;
using CMS.Models;
using DATA;
using UTILS;
using Constants = UTILS.Constants;
using System.Web.Security;

namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,NewsEdit,NewsPublish")]
    public class SpiderController : Controller
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
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            string results;


            new GoNewsBO().DeleteGoNews(Id);
            results = "true";
            return Json(results);
        }

        public ActionResult Index(int categoryId = 0, int page = 1, string createdby = "-1", string fromdate = "", string todate = "", string title = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }

            //var lstcate = MvcApplication.StaticCategoryList.Where(x => x.Published == 1 && x.ParentId == 0);
            List<CATEGORY_FULL> listcategory=_staticCategoryList;
            //listcategory = MvcApplication.StaticCategoryList.Where(x => x.Published == 1 && x.ParentId == 0).ToList();

           
            ViewBag.categoryId = categoryId;
            ViewBag.createdby = createdby;
            ViewBag.todate = todate;
            ViewBag.fromdate = fromdate;
            ViewBag.title = title;
            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = _startDate;

            ViewBag.title = "Quản trị tin crawl";

            int total = 0;
            var lstnews = new GoNewsBO().FilterGoNews(categoryId, "", page, 40, ref total, fromdate, todate);
            var model = new GoNewsModel { CategoryId = categoryId, pageIndex = page, pageSize = 40, listdata = lstnews, total = total };
            return View(model);

        }
        public ActionResult Detail(int id)
        {
            var model = new GoNewsBO().GetGoNews(id);
            return View(model);
        }
        public ActionResult Edit(int id)
        {

            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            ViewBag.isEditer = HttpContext.User.IsInRole("NewsEdit");
            var model = new GoNewsBO().GetGoNews(id);

            if (String.IsNullOrEmpty(model.News_Image2) || !model.News_Image2.Contains(".jpg"))
            {
                model.News_Image = model.News_Image2;
            }
            else
            {
                if (!model.News_Image.Contains(".jpg"))
                    model.News_Image = string.Empty;

            }
            ViewBag.imageUrl = Utils.GetTempUrl(HttpContext.User.Identity.Name);
            ViewBag.title = "Biên tập tin crawl";
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(GoNew obj, int actionType, string keywords, string param, string sPublishedTime)
        {
            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");
            ViewBag.isEditer = HttpContext.User.IsInRole("NewsEdit");
            var newsobj = new CONTENT_FULL
            {
                PublishDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Type = 1,
                CategoryId = obj.News_CategoryId,
                Contents = obj.News_Content,
                CreatedBy = "Spider",
                Image = "1.jpg",
                Title = obj.News_Title,
                IntroText = obj.News_Description,
                Keywords = keywords,
                Params = param,

            };

            if (!string.IsNullOrEmpty(sPublishedTime))
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                newsobj.PublishDate = DateTime.ParseExact(sPublishedTime, "dd/MM/yyyy HH:mm", culture);
            }


            if (String.IsNullOrEmpty(newsobj.Contents))
                newsobj.Contents = " ";
            if (String.IsNullOrEmpty(newsobj.Keywords))
                newsobj.Keywords = " ";

            var firstOrDefault = _staticCategoryList.FirstOrDefault(x => x.Id == newsobj.CategoryId);
            if (firstOrDefault != null)
                newsobj.CategoryPathway = firstOrDefault.Pathway;
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                Type = actionType,
                ItemId = newsobj.Id,
                Contents = newsobj.Contents,
                Url = newsobj.Url,
                Title = newsobj.Title,
            };
            //check action type
            switch (actionType)
            {


                case (int)Constants.NewsAction.Publish:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Xuất bản tin từ nguồn crawl";
                        newsobj.Status = (int)Constants.NewsStatus.Publish;
                    }
                    else

                        return RedirectToAction("AssesDenied", "Admin");
                    break;
                case (int)Constants.NewsAction.SendPublish:
                    if (ViewBag.isAdmin)
                    {
                        lognewsobj.Description = "Tạo mới tin từ nguồn crawl";
                        newsobj.Status = (int)Constants.NewsStatus.PublishWait;
                    }
                    else
                    {
                        if (ViewBag.isEditer)
                        {
                            lognewsobj.Description = "Gửi xuất bản từ nguồn crawl";
                            newsobj.Status = (int)Constants.NewsStatus.Editting;
                        }
                        else
                        {
                            return RedirectToAction("AssesDenied", "Admin");
                        }
                    }


                    break;

                case (int)Constants.NewsAction.Save:

                    lognewsobj.Description = "Tạo mới tin từ nguồn crawl";
                    newsobj.Status = (int)Constants.NewsStatus.Draft;

                    break;

            }
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
            //update news
            var id = new ContentBO().CreateUpdateContent(newsobj);
            if (id >= 0)
            {
                //insert log
                lognewsobj.ItemId = id;
                new ContentLogBO().CreateUpdateContentLog(lognewsobj);

                new GoNewsBO().DeleteGoNews(obj.Id);
                //copy file
                if (!string.IsNullOrEmpty(obj.News_Image))
                {
                    if (!obj.News_Image.Contains("http://"))
                    {
                        Utils.MoveFile(Utils.GetTempPath(HttpContext.User.Identity.Name), obj.News_Image, Utils.GetNewsImagePath(id), "1.jpg");
                    }
                    else
                    {
                        //download image 

                        Action<string, string> send = DownloadImage;
                        //Action<string, string> send = (string fromPath, string uri) =>
                        //{
                        //    DownloadImage(fromPath, uri);
                        //    //call o day
                        //};
                        var asynSend = send.BeginInvoke(Utils.GetNewsImagePath(id), obj.News_Image, null, null);
                    }


                }

            }

            //if (actionType == (int)Constants.NewsAction.Reject || actionType == (int)Constants.NewsAction.Publish || actionType == (int)Constants.NewsAction.Delete)
            //{ }
            return Json(id.ToString());
        }
        private void DownloadImage(string fromPath, string uri)
        {
            var webClient = new WebClient();
            if (!Directory.Exists(fromPath))
                Directory.CreateDirectory(fromPath);
            webClient.DownloadFile(uri, fromPath + "1.jpg");
        }
       
        [ChildActionOnly]
        public ActionResult FormEdit(GoNew obj)
        {
            var listcategory = _staticCategoryList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
            ViewBag.CategoryList = listcategory;


            ViewBag.isEditer = HttpContext.User.IsInRole("NewsEdit");
            ViewBag.isAdmin = HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("NewsPublish");

            ViewBag.Author = HttpContext.User.Identity.Name;
            var lstaccount = GetUserByNewsRole();
            ViewBag.AccountList = lstaccount;
            return PartialView(obj);
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
        [ChildActionOnly]
        public ActionResult FormReference()
        {
            var listcategory = _staticCategoryList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Review(long Id)
        {
            return PartialView();
        }
    }
}
