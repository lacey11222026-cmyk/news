using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BIZ;
using BIZ.Entity;
using DATA;
using CMS.Models;
using Constants = UTILS.Constants;
using UTILS;
using Newtonsoft.Json;

namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,Category,Banner,Survey")]
    public class AdminCategoryController : Controller
    {

        private List<CATEGORY_FULL> _staticCategoryList;
        private List<CATEGORY_FULL> _staticCategoryByUserList;
        protected override void Initialize(RequestContext requestContext)
        {

            _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.None);
            _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, requestContext.HttpContext.User.Identity.Name,
                                                                               true);
            base.Initialize(requestContext);

        }
        #region Category
        [Authorize(Roles = "Administrator,Category")]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản trị chuyên mục";
            return View();
        }
        [Authorize(Roles = "Administrator,Category")]
        public ActionResult AddEdit(int id = 0)
        {
            var category = new CATEGORY_FULL { Ordering = 0 };
            var listdata = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.None);
            var listcategory = new List<Category>();
            foreach (var item in listdata)
            {
                if (item.ParentId > 0)
                {

                    var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name };
                    if (item.NodeLevel != 0)
                    {
                        for (var i = 1; i < item.NodeLevel; i++)
                        {
                            x1.Name = "-+ " + x1.Name;
                        }
                    }

                    int pindex = listcategory.Select((Value, Index) => new { Value, Index }).Where(x => x.Value.Id == x1.ParentId).FirstOrDefault().Index;
                    listcategory.Insert(pindex + 1, x1);


                }
                else
                {
                    listcategory.Add(item);
                }
            }

            listcategory.Insert(0, new Category { Id = 0, Name = "--Nhóm gốc--" });
            if (id == 0)
            {
                ViewBag.CategoryList = listcategory;

            }
            else
            {
                ViewBag.CategoryList = listcategory.Where(x => x.Id != id).ToList();
            }

            ViewBag.Title = "Thêm mới chuyên mục";
            if (id > 0)
            {
                ViewBag.Title = "Cập nhật chuyên mục";
                category = new CategoryBO().GetCategoryFull(id);
                try
                {
                    category.Param = JsonConvert.DeserializeObject<CategoryParam>(category.Params);
                }
                catch
                {

                    category.Param = new CategoryParam();
                }
            }
            ViewBag.TypeCateList = new List<EnumInfo> { new EnumInfo { Value = 2, Text = "Tin tức" }, new EnumInfo { Value = 1, Text = "Giới thiệu" }, new EnumInfo { Value = 5, Text = "Văn bản" },new EnumInfo { Value = 3, Text = "Khác" } };
            ViewBag.LanguageList = new List<EnumInfo> { new EnumInfo { SValue = "vi-vn", Text = "Tiếng Việt" }, new EnumInfo { SValue = "en-us", Text = "Tiếng Anh" } };
            return View(category);
        }
        [Authorize(Roles = "Administrator,Category")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddEdit(CATEGORY_FULL category)
        {
            category.Params = Utils.ConvertToJson(category.Param, string.Empty);
            category.ModifiedDate = DateTime.Now;

            if (category.Id > 0)
            {
                new CategoryBO().CreateUpdateCategory(category);
            }
            else
            {
                category.CreateDate = DateTime.Now;
                new CategoryBO().CreateUpdateCategory(category);
            }
            //Utils.SetAppSettingValue("EnableURLRewrite", "1", Request.ApplicationPath);

            return RedirectToAction("Index", "AdminCategory");
        }
      
       
        #endregion

        #region Intro-Support

        public ActionResult Intro()
        {
            return View();
        }

        public ActionResult IntroAddEdit(int Id = 0)
        {
            ViewBag.CategoryId = Id;
            var category = new CategoryBO().GetCategoryFull(Id);
            try
            {
                category.Param = JsonConvert.DeserializeObject<CategoryParam>(category.Params);
            }
            catch
            {

                category.Param = new CategoryParam();
            }
            return View(category);
        }
         [ValidateInput(false)]
        [HttpPost]
        public ActionResult IntroAddEdit(CATEGORY_FULL category)
        {
            //category.Params = Utils.ConvertToJson(category.Param, string.Empty);
            category.ModifiedDate = DateTime.Now;

            if (category.Id > 0)
            {
                new CategoryBO().UpdateContentFull(category);
            }
            
            return RedirectToAction("Intro", "AdminCategory");
        }
        public ActionResult Support()
        {
            return View();
        }
        public ActionResult SupportAddEdit(int id = 0)
        {
            ViewBag.Id = id;

            return View();
        }
        #endregion
        #region Banner
        [Authorize(Roles = "Administrator,Banner")]
        public ActionResult Banner(int region = -1, int status = -1, int site = 0,int categoryId=0)
        {
            ViewBag.CategoryId = categoryId;
            var CateList = new List<CATEGORY_FULL>();
            if(site==0)
            {
                CateList = _staticCategoryByUserList;
                CateList.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Trang chủ" });
                CateList.Insert(1, new CATEGORY_FULL { Id = OtherPage.EngPage, Name = "Trang chủ Tiếng Anh" });
                //CateList.Insert(1, new CATEGORY_FULL { Id = OtherPage.VideoPage, Name = "Trang Video" });
                //CateList.Insert(2, new CATEGORY_FULL { Id = OtherPage.PhotoPage, Name = "Trang phóng sự ảnh" });
            }

            
            else
            {
                //CateList.AddRange(MvcApplication.StaticATGT_CategoryList);
                if (CateList.Where(x=>x.Id==0).Count()==0)
                {
                    CateList.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Trang chủ" });
                    //CateList.Insert(1, new CATEGORY_FULL { Id = OtherPage.ATGTVideoPage, Name = "Trang Video" });
                    //CateList.Insert(2, new CATEGORY_FULL { Id = OtherPage.ATGTPhotoPage, Name = "Trang phóng sự ảnh" });
                }
                
            }
            ViewBag.CateList = CateList;
            ViewBag.Region = region;
            ViewBag.Satus = status;
            ViewBag.site = site;
            ViewBag.SiteList = new List<EnumInfo> { new EnumInfo { Value = 0, Text = "tietkiemnangluong.com.vn" }, new EnumInfo { Value = 1, Text = "Trang Carbon" } };
            var lstBanner = new BannerBO().GetTopLastestBanners(0, region, status, site, categoryId);
            ViewBag.RegionList = new List<EnumInfo> { new EnumInfo { Value = -1, Text = "--Tất cả--" },  new EnumInfo { Value = 3, Text = "Phải" }, new EnumInfo() { Value = 4, Text = "Giữa 1" }, new EnumInfo { Value = 5, Text = "Giữa 2" }, new EnumInfo { Value = 6, Text = "Giữa 3" },new EnumInfo { Value = 1, Text = "Cuối" }, };
            //ViewBag.RegionList = new List<EnumInfo> { new EnumInfo { Value = -1, Text = "--Tất cả--" }, new EnumInfo { Value = 1, Text = "Chính" }, new EnumInfo { Value = 2, Text = "Phải" }, new EnumInfo { Value = 3, Text = "Dưới" } };

            ViewBag.StatusList = new List<EnumInfo> { new EnumInfo { Value = -1, Text = "--Tất cả--" }, new EnumInfo { Value = 1, Text = "Hoạt động" }, new EnumInfo { Value = 0, Text = "Khóa" } };
            ViewBag.Title = "Quản trị Banner";
            return View(lstBanner);
        }
        [Authorize(Roles = "Administrator,Banner")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult BannerAddEdit(Banner banner)
        {
            new BannerBO().CreateUpdateBanner(banner);

            return RedirectToAction("Banner", "AdminCategory");
        }
        [Authorize(Roles = "Administrator,Banner,NewsPublish")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult BannerDelete(int id)
        {



            string results = "0";

            if (new BannerBO().DeleteBanner(id) >= 0)
            {
                results = "1";
            }
            //var list = Roles.GetUsersInRole("roleName").Select(Membership.GetUser).ToList()
            return Json(results);

        }
        #endregion
        #region Survey
        [Authorize(Roles = "Administrator,Survey")]
        public ActionResult Survey(int page = 1, int status = -1)
        {
            int total = 0;
            var pageSize = 20;
            var lstdata = new SurveyBO().GetAllSurveysPaged(page, pageSize, ref  total, status);
            var model = new SurveyModel
            {
                listdata = lstdata,
                pageIndex = page,
                pageSize = pageSize,
                total = total

            };
            ViewBag.Title = "Quản trị khảo sát";
            ViewBag.Status = status;
            ViewBag.StatusList = new List<EnumInfo> { new EnumInfo { Value = -1, Text = "--Tất cả--" }, new EnumInfo { Value = 1, Text = "Duyệt" }, new EnumInfo { Value = 0, Text = "Chưa duyệt" } };
            return View(model);
        }
        [Authorize(Roles = "Administrator,Survey")]
        [HttpPost]
        public ActionResult SurveyAppproved(int Id, int Status)
        {
            string results;
            var obj = new SurveyBO().GetSurvey(Id);
            obj.Status = Status;

            new SurveyBO().CreateUpdateSurvey(obj);
            results = "true";
            return Json(results);

        }
        [Authorize(Roles = "Administrator,Survey")]
        [HttpPost]
        public ActionResult SurveyDelete(int Id)
        {
            string results;


            new SurveyBO().DeleteSurvey(Id);
            results = "true";
            return Json(results);

        }
        [Authorize(Roles = "Administrator,Survey")]
        [HttpPost]
        public ActionResult SurveyItemAppproved(int Id, int Status)
        {
            string results;
            //var obj = new SurveyBO().GetSurvey(Id);
            //obj.Status = Status;

            new SurveyItemBO().UpdateStatus(Id, Status);
            results = "true";
            return Json(results);

        }
        [Authorize(Roles = "Administrator,Survey")]
        [HttpPost]
        public ActionResult SurveyItemDelete(int Id)
        {
            string results;


            new SurveyItemBO().DeleteSurveyItem(Id);
            results = "true";
            return Json(results);

        }
        [Authorize(Roles = "Administrator,Survey")]
        public ActionResult SurveyAdd()
        {

            var obj = new DATA.Survey { Status = 1, Type = 1, CategoryPath = " ", BeginDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3) };


            ViewBag.Title = "Tạo mới khảo sát";

            return View(obj);
        }
        [Authorize(Roles = "Administrator,Survey")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SurveyAdd(Survey obj, string SBeginDate, string SEndDate)
        {
            if (!string.IsNullOrEmpty(SBeginDate))
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                obj.BeginDate = DateTime.ParseExact(SBeginDate, "dd/MM/yyyy", culture);
            }
            if (!string.IsNullOrEmpty(SEndDate))
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                obj.EndDate = DateTime.ParseExact(SEndDate, "dd/MM/yyyy", culture);
            }
            int id = new SurveyBO().CreateUpdateSurvey(obj);
            return RedirectToAction("Survey", "AdminCategory");
        }


        [Authorize(Roles = "Administrator,Survey")]
        public ActionResult SurveyEdit(int Id)
        {
            var obj = new SurveyBO().GetSurvey(Id);
            ViewBag.Title = "Cập nhật khảo sát";
            return View(obj);
        }
        [Authorize(Roles = "Administrator,Survey")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SurveyEdit(Survey obj, string SBeginDate, string SEndDate)
        {
            if (!string.IsNullOrEmpty(SBeginDate))
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                obj.BeginDate = DateTime.ParseExact(SBeginDate, "dd/MM/yyyy", culture);
            }
            if (!string.IsNullOrEmpty(SEndDate))
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                obj.EndDate = DateTime.ParseExact(SEndDate, "dd/MM/yyyy", culture);
            }
            int id = new SurveyBO().CreateUpdateSurvey(obj);
            return RedirectToAction("Survey", "AdminCategory");
        }
        [Authorize(Roles = "Administrator,Survey")]
        [ChildActionOnly]
        public ActionResult SurveyCategory(string categoryPath)
        {
            var listAvailable = _staticCategoryByUserList;
            if (listAvailable == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
            ViewData["AvailableNews"] = new SelectList(listAvailable, "Id", "Name");

            var listSelected = new List<CATEGORY_FULL>();


            if (!string.IsNullOrEmpty(categoryPath))
            {
                var listcategory = _staticCategoryByUserList;
                foreach (var item in listcategory)
                {
                    if (categoryPath.Contains("," + item.Id + ","))
                    {
                        var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name };
                        listSelected.Add(x1);
                    }
                }
            }

            ViewData["SelectedNews"] = new SelectList(listSelected, "Id", "Name");
            return PartialView();
        }
        [Authorize(Roles = "Administrator,Survey")]
        public ActionResult ConfigHotSurvey(int site = 0)
        {
            ViewBag.site = site;
            ViewBag.SiteList = new List<EnumInfo> { new EnumInfo { Value = 0, Text = "tietkiemnangluong.com.vn" }, new EnumInfo { Value = 1, Text = "Trang ATGT" } };
            var key = "HotSurvey";
            if (site > 0)
                key = "HotSurvey_" + site;
            ViewBag.lstNews = "";
            var lstchannel2 = new List<DATA.Survey>();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                lstchannel2 = new SurveyBO().GetSurveyByIds(configValue.ConfigValue, 30, true);

            }
            if (lstchannel2 == null)
                lstchannel2 = new List<DATA.Survey>();
            ViewData["SelectedNews"] = new SelectList(lstchannel2, "Id", "Title");



            ViewBag.Title = "Cấu hình khảo sát nổi bật";
            return View();
        }
        [Authorize(Roles = "Administrator,Survey")]
        [HttpPost]
        public ActionResult SaveConfigHotSurvey(string svalue, int site = 0)
        {
            var results = "true";

            try
            {
                var key = "HotSurvey";
                if (site > 0)
                    key = "HotSurvey_" + site;
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
        [Authorize(Roles = "Administrator,Survey")]
        [ChildActionOnly]
        public ActionResult FormSurvey(DATA.Survey obj)
        {
            ViewBag.SBeginDate = obj.BeginDate.ToString("dd/MM/yyyy");
            ViewBag.SEndDate = obj.EndDate.ToString("dd/MM/yyyy");
            return PartialView(obj);
        }
        [Authorize(Roles = "Administrator,Survey")]
        public ActionResult SurveyItem(int Id)
        {
            var obj = new SurveyBO().GetSurvey(Id);
            var lstdata = new SurveyItemBO().GetSurveyItemsBy(Id, -1);
            ViewBag.Title = "Danh sách câu trả lời khảo sát:" + obj.Title;
            ViewBag.SurveyId = Id;
            return View(lstdata);
        }
        [Authorize(Roles = "Administrator,Survey")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SurveyItemAddEdit(SurveyItem obj)
        {
            new SurveyItemBO().CreateUpdateSurveyItem(obj);
            return RedirectToAction("SurveyItem", "AdminCategory", new { Id = obj.SurveyId });
        }
        #endregion

    
    }
}
