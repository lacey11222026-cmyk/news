using BIZ;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Sale,Report")]
    public class AdminPlanController : Controller
    {
        //
        // GET: /AdminPlan/

        public ActionResult ManagePlan()
        {
            ViewBag.Title = "Quản trị báo cáo dự án";
            return View();
        }

        public ActionResult ListPlan(int? status)
        {

            var data = new List<Plan>();

            data = new PlanBO().GetList(HttpContext.User.Identity.Name);

            return PartialView(data);
        }

        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new Plan
            {
                Status = 1,
            };

            if (PageID > 0)
            {
                model = new PlanBO().GetPlan(PageID);
            }

            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật dự án";
            }
            else
            {
                ViewBag.Title = "Thêm mới dự án";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(Plan Plan)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Plan.Order = Convert.ToInt32(Plan.Order);
                Plan.UserName = HttpContext.User.Identity.Name;
                var result = new PlanBO().CreateUpdatePlan(Plan);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Plan.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                        ReturnData.Description = "Thêm mới Thành Công";


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

        [HttpPost]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = new PlanBO().UpdateOrder(Id, SortOrder, HttpContext.User.Identity.Name);
                if (updateResult > 0)
                {
                    return Json(new { ResponseCode = updateResult, Msg = "Cập nhật thứ tự thành công" });
                }
                else
                {
                    return Json(new { ResponseCode = -1, Msg = "Cập nhật thứ tự không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }
        }
        [HttpPost]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new PlanBO().UpdateStatus(id);
                    if (result >= 0)
                    {

                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định trang cần active";
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

        public ActionResult Detail(string Id, string Name)
        {
            ViewBag.Id = Id;
            ViewBag.Name = Name;
            return View();
        }
        public ActionResult ListPlanStuck(string planId)
        {

            var data = new List<PlanStuck>();

            data = new PlanStuckBO().GetList(int.Parse(Utils.Base64Decode(planId)));

            return PartialView(data);
        }
        public ActionResult AddPlanStuck(string planId)
        {
            var model = new PlanStuck
            {
                PlanId = int.Parse(Utils.Base64Decode(planId)),
                Status = 1,
            };
            return PartialView("PlanStuckDetail", model);
        }
        public ActionResult PlanStuckDetail(int Id)
        {
            var model = new PlanStuckBO().GetPlanStuck(Id);
            return PartialView(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult PlanStuckSaveData(PlanStuck Plan)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Plan.Order = Convert.ToInt32(Plan.Order);
                //Plan.UserName = HttpContext.User.Identity.Name;
                var result = new PlanStuckBO().CreateUpdatePlanStuck(Plan);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Plan.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                        ReturnData.Description = "Thêm mới Thành Công";


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
        [HttpPost]
        public JsonResult PlanStuckUpdateSortOrder(int Id, bool SortOrder, int PlanId)
        {
            try
            {
                var updateResult = new PlanStuckBO().UpdateOrder(Id, SortOrder, PlanId);
                if (updateResult > 0)
                {
                    return Json(new { ResponseCode = updateResult, Msg = "Cập nhật thứ tự thành công" });
                }
                else
                {
                    return Json(new { ResponseCode = -1, Msg = "Cập nhật thứ tự không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }
        }
        [HttpPost]
        public JsonResult PlanStuckUpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new PlanStuckBO().UpdateStatus(id);
                    if (result >= 0)
                    {

                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định trang cần active";
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

        public ActionResult ListPlanRequire(string planId)
        {

            var data = new List<PlanRequire>();

            data = new PlanRequireBO().GetList(int.Parse(Utils.Base64Decode(planId)));

            return PartialView(data);
        }
        public ActionResult AddPlanRequire(string planId)
        {
            var model = new PlanRequire
            {
                PlanId = int.Parse(Utils.Base64Decode(planId)),
                Status = 1,
            };
            return PartialView("PlanRequireDetail", model);
        }
        public ActionResult PlanRequireDetail(int Id)
        {
            var model = new PlanRequireBO().GetPlanRequire(Id);
            return PartialView(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult PlanRequireSaveData(PlanRequire Plan)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Plan.Order = Convert.ToInt32(Plan.Order);
                //Plan.UserName = HttpContext.User.Identity.Name;
                var result = new PlanRequireBO().CreateUpdatePlanRequire(Plan);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Plan.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                        ReturnData.Description = "Thêm mới Thành Công";


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
        [HttpPost]
        public JsonResult PlanRequireUpdateSortOrder(int Id, bool SortOrder, int PlanId)
        {
            try
            {
                var updateResult = new PlanRequireBO().UpdateOrder(Id, SortOrder, PlanId);
                if (updateResult > 0)
                {
                    return Json(new { ResponseCode = updateResult, Msg = "Cập nhật thứ tự thành công" });
                }
                else
                {
                    return Json(new { ResponseCode = -1, Msg = "Cập nhật thứ tự không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }
        }
        [HttpPost]
        public JsonResult PlanRequireUpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new PlanRequireBO().UpdateStatus(id);
                    if (result >= 0)
                    {

                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định trang cần active";
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

        public ActionResult ListPlanItem(string planId)
        {

            var data = new List<PlanItem>();

            data = new PlanItemBO().GetList(int.Parse(Utils.Base64Decode(planId)));

            return PartialView(data);
        }
        public ActionResult AddPlanItem(string planId)
        {
            var model = new PlanItemModel
            {
                PlanId = int.Parse(Utils.Base64Decode(planId)),
                Name="",
                Status = 1,
                Item1=new PlanItemData(),
                Item2 = new PlanItemData(),
                Item3 = new PlanItemData(),
            };
            return PartialView("PlanItemDetail", model);
        }
        public ActionResult PlanItemDetail(int Id)
        {
            var data = new PlanItemBO().GetPlanItem(Id);
            var model = new PlanItemModel
            {
                Id = data.Id,
                Name = data.Name,
                PlanId = data.PlanId,
                Status = data.Status,
            };
            model.Item1= JsonConvert.DeserializeObject<PlanItemData>(data.Config1);
            model.Item2 = JsonConvert.DeserializeObject<PlanItemData>(data.Config2);
            model.Item3 = JsonConvert.DeserializeObject<PlanItemData>(data.Config3);
            return PartialView(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult PlanItemSaveData(PlanItemModel data)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));
                data.Item1 = new PlanItemData(data.Currency1, data.CurrencyRate1, data.PlanYear1, data.PlanCurrent1, data.PlanQ1, data.BalanceYear1, data.Balance1);
                data.Item2 = new PlanItemData(data.Currency2, data.CurrencyRate2, data.PlanYear2, data.PlanCurrent2, data.PlanQ2, data.BalanceYear2, data.Balance2);
                data.Item3 = new PlanItemData(data.Currency3, data.CurrencyRate3, data.PlanYear3, data.PlanCurrent3, data.PlanQ3, data.BalanceYear3, data.Balance3);
                var obj = new PlanItem
                {
                    Id = data.Id,
                    Name = data.Name,
                    PlanId = data.PlanId,
                    Status = data.Status,
                    Config1 = Utils.ConvertToJson(data.Item1, string.Empty),
                    Config2 = Utils.ConvertToJson(data.Item2, string.Empty),
                    Config3 = Utils.ConvertToJson(data.Item3, string.Empty),
                };

                var result = new PlanItemBO().CreateUpdatePlanItem(obj);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (obj.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                        ReturnData.Description = "Thêm mới Thành Công";


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
        [HttpPost]
        public JsonResult PlanItemUpdateSortOrder(int Id, bool SortOrder, int PlanId)
        {
            try
            {
                var updateResult = new PlanItemBO().UpdateOrder(Id, SortOrder, PlanId);
                if (updateResult > 0)
                {
                    return Json(new { ResponseCode = updateResult, Msg = "Cập nhật thứ tự thành công" });
                }
                else
                {
                    return Json(new { ResponseCode = -1, Msg = "Cập nhật thứ tự không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }
        }
        [HttpPost]
        public JsonResult PlanItemUpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new PlanItemBO().UpdateStatus(id);
                    if (result >= 0)
                    {

                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định trang cần active";
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
