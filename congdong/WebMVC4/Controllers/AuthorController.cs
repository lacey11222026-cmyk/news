using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATA;
using Newtonsoft.Json;
using UTILS;
using WebMVC4.Models;
using System.Web.Security;
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{
    public class AuthorController : Controller
    {
        //
        // GET: /Author/
        public ActionResult Info(string username)
        {
            var lstUser = new List<AuthorProfile>();
            MembershipUserCollection userCollection = Membership.GetAllUsers();
            foreach (MembershipUser user in userCollection)
            {
                
                try
                {
                    var useritem = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
                    if (!string.IsNullOrEmpty(useritem.Avatar))
                    {
                        useritem.UserName = user.UserName;
                        lstUser.Add(useritem);
                    }
                }
                catch
                {

                }
            }
            lstUser = lstUser.OrderBy(x => x.Order.GetValueOrDefault()).ToList();
            var model = new News2Model
            {
                hotnews = null,
                listUser = lstUser,
                articles = null,
                author = lstUser.FirstOrDefault(x=>x.UserName==username)
            };

            return PartialView(model);
        }
        [LocalizationActionFilter]
        public ActionResult Detail(string username)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            MembershipUser user = Membership.GetUser(username.ToString());
            var useritem = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
            useritem.UserName = username;
            return View(useritem);
        }
        public ActionResult Relate(int CategoryId)
        {
            var PageSize = 6;
            var articles = new ContentBO().GetTopLastestContentFulls(PageSize, CategoryId);
            foreach (var item in articles)
            {
                MembershipUser user = Membership.GetUser(item.Alias);

                var author = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
                item.Avatar = author.Avatar;
                item.FullName = author.FullName;
            }
            return PartialView(articles);
        }
        [LocalizationActionFilter]
        public ActionResult Index(int Page = 1, int Type = 0)
        {


            int CategoryId = 42;
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
           
            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;



            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle ;
            
            var lstNotId = "";
          
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int Total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total, "", "", "", lstNotId);
            foreach(var item in articles)
            {
                MembershipUser user = Membership.GetUser(item.Alias);

                var author = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
                item.Avatar = author.Avatar;
                item.FullName = author.FullName;
            }
            MembershipUserCollection userCollection = Membership.GetAllUsers();
            var lstUser = new List<AuthorProfile>();
            foreach(MembershipUser user in userCollection)
            {
                //MembershipUser user = Membership.GetUser(username.ToString());
                try
                {
                    var useritem = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
                    if (!string.IsNullOrEmpty(useritem.Avatar))
                    {
                        useritem.UserName = user.UserName;
                        lstUser.Add(useritem);
                    }
                }
                catch
                {

                }
            }
            lstUser = lstUser.OrderBy(x => x.Order.GetValueOrDefault()).ToList();
            ViewBag.Total = Total;
            ViewBag.Page = Page;
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = cateobj.Name;


            ViewBag.PageClass = "list";
            var model = new News2Model
            {
                hotnews = null,
                listUser = lstUser,
                articles = articles
            };


            return View(model);
        }


    }
}
