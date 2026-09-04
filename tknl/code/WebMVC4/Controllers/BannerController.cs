using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;

namespace WebMVC4.Controllers
{
    public class BannerController : Controller
    {
        //
        // GET: /Banner/
        //[Authorize]
        public ActionResult Index(int Id=0)
        {
            return View();
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerRight1(int site = 0, int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site);
                    

                }
               
                return PartialView(lstBanner);

            }
            if (currentCategoryId >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site);
                   
                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 3, status, site);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerRight2(int site = 0, int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site);


                }

                return PartialView(lstBanner);

            }
            if (currentCategoryId >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site);

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 10, status, site);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerRight3(int site = 0, int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site);


                }

                return PartialView(lstBanner);

            }
            if (currentCategoryId >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site);

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 11, status, site);
            return PartialView(lstBanner);
        }
        [ChildActionOnly]
        public ActionResult BannerRight4(int site = 0, int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site);


                }

                return PartialView(lstBanner);

            }
            if (currentCategoryId >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site);

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 12, status, site);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerRight5(int site = 0, int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site);


                }

                return PartialView(lstBanner);

            }
            if (currentCategoryId >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site);

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 13, status, site);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerTop(int site = 0, int currentCategoryId=0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());

                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, categoryId);
                if(lstBanner.Count<=0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site);
                    //var objcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == categoryId).FirstOrDefault();

                    //if(objcate!=null)
                    //{
                    //    lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, objcate.ParentId.Value);
                    //    if (lstBanner.Count <= 0)
                    //    {
                    //        var objparrentcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == objcate.ParentId.Value).FirstOrDefault();

                    //        if (objparrentcate != null)
                    //        {
                    //            lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, objparrentcate.ParentId.Value);
                    //        }

                    //    }
                    //}

                }
                return PartialView(lstBanner);

            }
            if (currentCategoryId >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId>0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0,1, status, site);
                    //var objcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == currentCategoryId).FirstOrDefault();

                    //if (objcate != null)
                    //{
                    //    lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, objcate.ParentId.Value);
                    //    if (lstBanner.Count <= 0)
                    //    {
                    //        var objparrentcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == objcate.ParentId.Value).FirstOrDefault();

                    //        if (objparrentcate != null)
                    //        {
                    //            lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site, objparrentcate.ParentId.Value);
                    //        }

                    //    }
                    //}

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 1, status, site);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerCenter1(bool IsMobile=false, int site = 0,int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            ViewBag.IsMobile = IsMobile;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site);
                    //var objcate = new CategoryBO().GetCategoryFull(currentCategoryId);

                    //if (objcate != null)
                    //{
                    //    lstBanner = new BannerBO().GetTopLastestBanners(0, 2, status, site, objcate.ParentId.Value);
                    //    if (lstBanner.Count <= 0)
                    //    {
                    //        var objparrentcate = new CategoryBO().GetCategoryFull(objcate.ParentId.Value);

                    //        if (objparrentcate != null)
                    //        {
                    //            lstBanner = new BannerBO().GetTopLastestBanners(0, 2, status, site, objparrentcate.ParentId.Value);
                    //        }

                    //    }
                    //}

                }
                return PartialView(lstBanner);

            }
            if (site >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site);
                    //var objcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == currentCategoryId).FirstOrDefault();

                    //if (objcate != null)
                    //{
                    //    lstBanner = new BannerBO().GetTopLastestBanners(0, 2, status, site, objcate.ParentId.Value);
                    //    if (lstBanner.Count <= 0)
                    //    {
                    //        var objparrentcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == objcate.ParentId.Value).FirstOrDefault();

                    //        if (objparrentcate != null)
                    //        {
                    //            lstBanner = new BannerBO().GetTopLastestBanners(0,2, status, site, objparrentcate.ParentId.Value);
                    //        }

                    //    }
                    //}

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 4, status, site);
            return PartialView(lstBanner);
            
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerCenter2(int site = 0,int currentCategoryId = 0)
        {
            var lstBanner = new List<Banner>();
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
                var categoryId = int.Parse(ControllerContext.ParentActionViewContext.RouteData.Values["Id"].ToString());
                lstBanner = new BannerBO().GetTopLastestBanners(0, 5, status, site, categoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0,5, status, site);
                    //var objcate = new CategoryBO().GetCategoryFull(currentCategoryId);

                    //if (objcate != null)
                    //{
                    //    lstBanner = new BannerBO().GetTopLastestBanners(0, 2, status, site, objcate.ParentId.Value);
                    //    if (lstBanner.Count <= 0)
                    //    {
                    //        var objparrentcate = new CategoryBO().GetCategoryFull(objcate.ParentId.Value);

                    //        if (objparrentcate != null)
                    //        {
                    //            lstBanner = new BannerBO().GetTopLastestBanners(0, 2, status, site, objparrentcate.ParentId.Value);
                    //        }

                    //    }
                    //}

                }
                return PartialView(lstBanner);

            }
            if (site >= 10000)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 5, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 5, status, site);
                }
                return PartialView(lstBanner);
            }
            if (currentCategoryId > 0)
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 5, status, site, currentCategoryId);
                if (lstBanner.Count <= 0)
                {
                    lstBanner = new BannerBO().GetTopLastestBanners(0, 5, status, site);
                    //var objcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == currentCategoryId).FirstOrDefault();

                    //if (objcate != null)
                    //{
                    //    lstBanner = new BannerBO().GetTopLastestBanners(0, 2, status, site, objcate.ParentId.Value);
                    //    if (lstBanner.Count <= 0)
                    //    {
                    //        var objparrentcate = MvcApplication.StaticCategoryAllList.Where(x => x.Id == objcate.ParentId.Value).FirstOrDefault();

                    //        if (objparrentcate != null)
                    //        {
                    //            lstBanner = new BannerBO().GetTopLastestBanners(0,2, status, site, objparrentcate.ParentId.Value);
                    //        }

                    //    }
                    //}

                }
                return PartialView(lstBanner);
            }
            lstBanner = new BannerBO().GetTopLastestBanners(0, 5, status, site);
            return PartialView(lstBanner);
        }
       // [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerCenter3(bool IsMobile= false, int site = 0)
        {
            var status = 1;
            if (ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower() == "banner")
            {
                status = -1;
            }
            ViewBag.IsMobile = IsMobile;
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 6, status, site);
            return PartialView(lstBanner);
        }
    }
}
