using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using BIZ.Entity;
using UTILS;
using CMS.Models;

namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,Channel,NewsPublish")]
    public class AdminChannelController : Controller
    {
        //
        // GET: /AdminChannel/

        public ActionResult Index()
        {
            return View();
        }
        #region Channel

        public ActionResult Channel(int status = -1, int page = 1)
        {

            ViewBag.StatusList = new List<EnumInfo> { new EnumInfo { Value = -1, Text = "--Tất cả--" }, new EnumInfo { Value = 1, Text = "Hoạt động" }, new EnumInfo { Value = 0, Text = "Khóa" } };
            int total = 0;
            int pagesize = 20;
            var lstdata = new ChannelBO().GetAllChannelFullsPaged(page, pagesize, ref total, status);

            var model = new ChannelModel
            {
                listdata = lstdata,
                pageIndex = page,
                pageSize = pagesize,
                total = total

            };
            ViewBag.Title = "Quản trị kênh tin";
            return View(model);
        }
        public ActionResult ChannelAdd()
        {

            var obj = new DATA.Channel { Published = 1 };

            ViewBag.imageUrl = Utils.GetTempUrl(HttpContext.User.Identity.Name);

            ViewBag.Title = "Tạo mới kênh tin";
            return View(obj);
        }

        [Authorize(Roles = "Administrator,Channel")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChannelAdd(DATA.Channel obj)
        {
            var tempImage = obj.Image;
            if (!string.IsNullOrEmpty(obj.Image))
            {
                obj.Image = "1.jpg";

            }
            //inser news
            int id = new ChannelBO().CreateUpdateChannel(obj);


            //copy file
            if (!string.IsNullOrEmpty(tempImage))
            {
                Utils.MoveFile(Utils.GetTempPath(HttpContext.User.Identity.Name), tempImage, Utils.GetNewsImagePath(id, "Channel"), "1.jpg");

            }
            return RedirectToAction("Channel", "AdminChannel");
        }

        [Authorize(Roles = "Administrator,Channel")]
        public ActionResult ChannelEdit(int id)
        {
            var obj = new ChannelBO().GetChannel(id);
            if (obj == null)
            {
                Response.Redirect(Url.Action("AssesDenied", "Admin"));


            }
            ViewBag.Title = "Sửa kênh tin";
            ViewBag.imageUrl = Utils.GetImageUrl(id, "Channel", false);
            return View(obj);
        }

        [Authorize(Roles = "Administrator,Channel")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChannelEdit(DATA.Channel obj)
        {
            new ChannelBO().CreateUpdateChannel(obj);
            return RedirectToAction("Channel", "AdminChannel");
        }

        [Authorize(Roles = "Administrator,Channel")]
        [ChildActionOnly]
        public ActionResult FormChannelAdd(DATA.Channel obj)
        {

            return PartialView(obj);
        }
        [Authorize(Roles = "Administrator,Channel")]
        public ActionResult ChannelNews(int channelId, string name)
        {
            int total = 0;
            
            var lstnews = new ContentBO().GetTopContentByChannelId(channelId, 1, 30, ref total);
            if (lstnews == null)
                lstnews = new List<CONTENT_FULL>();
            ViewData["SelectedNews"] = new SelectList(lstnews, "Id", "Title");
            ViewBag.Title = "Danh sách tin- kênh tin: " + name;
            return View();
        }
        [Authorize(Roles = "Administrator,Channel")]
        [HttpPost]
        public ActionResult ChannelNewsAdd(int channelId, long newsId)
        {
            string results = "0";
            var obj = new DATA.Channel_Data { ChannelId = channelId, ContentId = newsId };
            if (new Channel_DataBO().CreateUpdateChannel_Data(obj) >= 0)
                results = "1";
            return Json(results);

        }

        [Authorize(Roles = "Administrator,Channel")]
        [HttpPost]
        public ActionResult ChannelNewsDelete(int channelId, long newsId)
        {
            string results = "0";
            //var obj = new DATA.Channel_Data { ChannelId = channelId, ContentId = newsId };
            if (new Channel_DataBO().DeleteByCId(channelId, newsId) >= 0)
                results = "1";
            return Json(results);

        }

        #endregion
        #region "Hotnews"


        [Authorize(Roles = "Administrator,Channel,NewsPublish")]
        public ActionResult ConfigHotNews(int site = 0)
        {
            ViewBag.site = site;
            ViewBag.SiteList = new List<EnumInfo> { new EnumInfo { Value = 0, Text = "tietkiemnangluong.com.vn" }, new EnumInfo { Value = 1, Text = "Trang ATGT" } };
            var key = "HotChannel";
            if(site>0)
                key = "HotChannel_" + site;
            ViewBag.lstNews = "";
            var lstchannel2 = new List<DATA.Channel>();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                lstchannel2 = new ChannelBO().GetChannelByIds(configValue.ConfigValue, 5, true);

            }
            if (lstchannel2 == null)
                lstchannel2 = new List<DATA.Channel>();
            ViewData["SelectedNews"] = new SelectList(lstchannel2, "Id", "Name");


            var total = 0;
            var lstchannel = new ChannelBO().GetAllChannelFullsPaged(1, 30, ref total, 1);
            if(lstchannel==null)
                lstchannel = new List<DATA.Channel>();
            ViewData["AvailableNews"] = new SelectList(lstchannel, "Id", "Name");

            ViewBag.Title = "Cấu hình kênh tin nổi bật";
            return View();
        }
        [HttpPost]
        public ActionResult SaveConfigHotNews(string svalue, int site = 0)
        {
            var results = "true";

            try
            {
                var key = "HotChannel";
                if (site > 0)
                    key = "HotChannel_" + site;
                if (new SystemConfigBO().SetByKey(key, svalue) >= 0)
                {
                    Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);
                }


            }
            catch (System.Exception ex)
            {

                results = ex.Message;

            }
            return Json(results);
        }
        #endregion
    }
}
