using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BIZ;
using Constants = UTILS.Constants;
using BIZ.Entity;
using System.Web.Routing;
using DATA;
using WebMVC4.Models;
using UTILS;
using System.Globalization;
using Newtonsoft.Json;
using DATA.ContentDB;
using System.Diagnostics;
using System.Web.UI.WebControls;
using WebMVC4.Helper;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Document")]
    public class AdminDocument2Controller : Controller
    {

        private List<CATEGORY_FULL> _staticCategoryList;
        protected override void Initialize(RequestContext requestContext)
        {


            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {


            return View();
        }
       
        //Đơn tố cáo
        public ActionResult ListDocument(string title, int? currentPage, int? pageSize, int? group)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int Group = group == null ? -1 : (int)group;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;

            int TotalRecord = 0;
            var data = VanThuDAL.GetSearch( title,1,1, Group, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
            {
                ViewBag.TotalRecord = TotalRecord;
                //foreach (var  item in data)
                //{
                //    item.FollowersConfig= JsonConvert.DeserializeObject<IdeaConfig>(item.Followers);
                //    item.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(item.Proposer);
                //}
            }
            else
            {
                ViewBag.TotalRecord = 0;
            }



            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        public ActionResult Index2()
        {


            return View();
        }

       

        //Đơn khiếu nại
        public ActionResult ListDocument2(string title, int? currentPage, int? pageSize, int? group)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int Group = group == null ? -1 : (int)group;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;

            int TotalRecord = 0;
            var data = VanThuDAL.GetSearch(title, 1, 2, Group, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
            {
                ViewBag.TotalRecord = TotalRecord;
                //foreach (var  item in data)
                //{
                //    item.FollowersConfig= JsonConvert.DeserializeObject<IdeaConfig>(item.Followers);
                //    item.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(item.Proposer);
                //}
            }
            else
            {
                ViewBag.TotalRecord = 0;
            }



            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        //Đơn phàn ánh
        public ActionResult ListDocument3(string title, int? currentPage, int? pageSize, int? group)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int Group = group == null ? -1 : (int)group;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;

            int TotalRecord = 0;
            var data = VanThuDAL.GetSearch(title, 1, 3, Group, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
            {
                ViewBag.TotalRecord = TotalRecord;
                //foreach (var  item in data)
                //{
                //    item.FollowersConfig= JsonConvert.DeserializeObject<IdeaConfig>(item.Followers);
                //    item.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(item.Proposer);
                //}
            }
            else
            {
                ViewBag.TotalRecord = 0;
            }



            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }

        public ActionResult Index4()
        {


            return View();
        }
        //Đơn khác
        public ActionResult ListDocument4(string title, int? currentPage, int? pageSize, int? group)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int Group = group == null ? -1 : (int)group;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;

            int TotalRecord = 0;
            var data = VanThuDAL.GetSearch(title, 1, 4, Group, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
            {
                ViewBag.TotalRecord = TotalRecord;
                //foreach (var  item in data)
                //{
                //    item.FollowersConfig= JsonConvert.DeserializeObject<IdeaConfig>(item.Followers);
                //    item.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(item.Proposer);
                //}
            }
            else
            {
                ViewBag.TotalRecord = 0;
            }



            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        //cong van
        public ActionResult Index5()
        {


            return View();
        }
        //Đơn khác
        public ActionResult ListDocument5(string title, int? currentPage, int? pageSize, int? group)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int Group = group == null ? -1 : (int)group;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;

            int TotalRecord = 0;
            var data = VanThuDAL.GetSearch(title, 2, -1, Group, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
            {
                ViewBag.TotalRecord = TotalRecord;
                //foreach (var  item in data)
                //{
                //    item.FollowersConfig= JsonConvert.DeserializeObject<IdeaConfig>(item.Followers);
                //    item.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(item.Proposer);
                //}
            }
            else
            {
                ViewBag.TotalRecord = 0;
            }



            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }


        public ActionResult Index3()
        {


            return View();
        }

        public ActionResult GetDocumentDetail(int Id = 0,int type=1)
        {
            ViewBag.CategoryList = _staticCategoryList;
            var model = new VanThu { Id = 0 };
            if (Id > 0)
            {
                model = VanThuDAL.GetDetail(Id);

                //model.FollowersConfig = JsonConvert.DeserializeObject<IdeaConfig>(model.Followers);
                //model.ProposerConfig = JsonConvert.DeserializeObject<IdeaConfig>(model.Proposer);
                if(model.IncomingDocumentType==1)
                {
                    if (model.IncomingDocumentDetail == 1)
                    {
                        ViewBag.Title = "Cập nhật đơn tố cáo";
                    }
                    if (model.IncomingDocumentDetail == 2)
                    {
                        ViewBag.Title = "Cập nhật đơn khiếu nại";
                        return View("GetDocumentDetail2",model);
                    }
                    if (model.IncomingDocumentDetail == 3)
                    {
                        ViewBag.Title = "Cập nhật đơn phản ánh, kiến nghị";
                        return View("GetDocumentDetail3", model);
                    }
                    if (model.IncomingDocumentDetail == 3)
                    {
                        ViewBag.Title = "Cập nhật đơn khác";
                        return View("GetDocumentDetail4", model);
                    }
                }
                else
                {
                    ViewBag.Title = "Cập nhật công văn";
                    return View("GetDocumentDetail5", model);
                }
                
            }
            else
            {
              
                ViewBag.Title = "Thêm mới đơn tố cáo";
                if (type == 1)
                {
                    ViewBag.Title = "Thêm mới đơn tố cáo";
                }
                if (type == 2)
                {
                    ViewBag.Title = "Thêm mới đơn khiếu nại";
                    return View("GetDocumentDetail2", model);
                }
                if (type == 3)
                {
                    ViewBag.Title = "Thêm mới đơn phản ánh, kiến nghị";
                    return View("GetDocumentDetail3", model);
                }
                if (type == 4)
                {
                    ViewBag.Title = "Thêm mới đơn khác";
                    return View("GetDocumentDetail4", model);
                }
                if (type == 5)
                {
                    ViewBag.Title = "Thêm mới công văn";
                    return View("GetDocumentDetail5", model);
                }
            }
            return View(model);
        }
        private DateTime? ConvertVanThuDate(string value)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);

            if (string.IsNullOrWhiteSpace(value))
            {
                return Utils.ConvertToDate("01/01/9999", "dd-MM-yyyy");
            }

            return DateTime.ParseExact(
                value.Trim(),
                "dd/MM/yyyy",
                culture
            );
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(
            VanThu doc,

            string SDutyDate,
            string SReceivedDate,
            string SIncomingDocumentDate,
            string SLeaderReceivedDate,
            string SAssignedOfficerReceivedDate,
            string SOutgoingDocumentDate,
            string SProposalSubmissionDate,
            string SGuidanceDocumentDate,
            string STransferSlipDate,
            string SReminderDocumentDate,
            string SReportToInspection2Date,
            string SNonAcceptanceNoticeDate,
            string SAcceptanceDecisionDate,
            string SVerificationDecisionDate,
            string STemporarySuspensionDecisionDate,
            string SSuspensionDecisionDate,
            string SResolutionDecisionDate
        )
        {
            var ReturnData = new ReturnData();

            try
            {
                // Xử lý toàn bộ trường ngày
                doc.DutyDate = ConvertVanThuDate(SDutyDate);

                doc.ReceivedDate = ConvertVanThuDate(SReceivedDate);

                doc.IncomingDocumentDate = ConvertVanThuDate(SIncomingDocumentDate);

                doc.LeaderReceivedDate = ConvertVanThuDate(SLeaderReceivedDate);

                doc.AssignedOfficerReceivedDate =
                    ConvertVanThuDate(SAssignedOfficerReceivedDate);

                doc.OutgoingDocumentDate =
                    ConvertVanThuDate(SOutgoingDocumentDate);

                doc.ProposalSubmissionDate =
                    ConvertVanThuDate(SProposalSubmissionDate);

                doc.GuidanceDocumentDate =
                    ConvertVanThuDate(SGuidanceDocumentDate);

                doc.TransferSlipDate =
                    ConvertVanThuDate(STransferSlipDate);

                doc.ReminderDocumentDate =
                    ConvertVanThuDate(SReminderDocumentDate);

                doc.ReportToInspection2Date =
                    ConvertVanThuDate(SReportToInspection2Date);

                doc.NonAcceptanceNoticeDate =
                    ConvertVanThuDate(SNonAcceptanceNoticeDate);

                doc.AcceptanceDecisionDate =
                    ConvertVanThuDate(SAcceptanceDecisionDate);

                doc.VerificationDecisionDate =
                    ConvertVanThuDate(SVerificationDecisionDate);

                doc.TemporarySuspensionDecisionDate =
                    ConvertVanThuDate(STemporarySuspensionDecisionDate);

                doc.SuspensionDecisionDate =
                    ConvertVanThuDate(SSuspensionDecisionDate);

                doc.ResolutionDecisionDate =
                    ConvertVanThuDate(SResolutionDecisionDate);


                var result = VanThuDAL.InsertUpdate(doc);

                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Doc,
                        ItemId = doc.Id,
                        ItemName = doc.IncomingDocumentSummary,
                        Note = "Đơn",
                        Type = 1
                    };

                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Cập nhật đơn";
                    }
                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới đơn";
                    }

                    // Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else
                {
                    switch (result)
                    {
                        case -51:
                            ReturnData.Description = "Đã có bài viết này";
                            break;

                        case -600:
                            ReturnData.Description = "Tham số truyền vào không hợp lệ";
                            break;

                        default:
                            ReturnData.Description =
                                "Hệ thống đang bận. Vui lòng quay lại sau";
                            break;
                    }
                }

                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                ReturnData.ResponseCode = -99;
                ReturnData.Description =
                    "Hệ thống đang bận. Vui lòng quay lại sau";

                return Json(ReturnData);
            }
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
                    var result = IdeaDAL.Delete(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Doc,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa sáng kiến",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa sáng kiến Thành Công";
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
                ReturnData.Description = "Không xác định sáng kiến cần xóa";
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

        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save2(string joinId)
        {
            joinId = joinId.TrimStart(',');

            var ReturnData = new ReturnData();
            try
            {
                var lstdata = IdeaTempDAL.GetList(joinId);
                foreach(var item in lstdata)
                {
                    var itemx = new Idea();
                    itemx.Code = item.Code;
                    itemx.Name = item.Name;
                    itemx.No = item.No;
                    itemx.PublishDate = item.PublishDate;
                    itemx.FilePath = item.FilePath;
                    itemx.Proposer = item.Proposer;
                    itemx.Unit = item.Unit;
                    itemx.Status = HtmlHelpers.GetSKStatusInt(item.Status);
                    itemx.ProgressPercent = int.Parse(item.ProgressPercent.Replace("%",""));
                    itemx.Progress = 1;
                    itemx.Mark = " ";
                    itemx.Region = 1;
                    if (item.Result.Contains("Chưa đánh giá"))
                    {
                        itemx.Result = 2;
                    }
                    if (item.Result.Contains("Đạt"))
                    {
                        itemx.Result = 1;
                        itemx.Mark = item.Result.Replace("Đạt", "").Replace("(", "").Replace(")", "");

                    }
                    itemx.Effective = item.Effective;
                    itemx.Followers = item.Followers;
                    IdeaDAL.InsertUpdate(itemx);
                    System.Threading.Thread.Sleep(30);
                }    
                IdeaTempDAL.Delete("1=1");
                ReturnData.ResponseCode = 1;
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
        [ValidateAntiForgeryToken]
        public ActionResult Delete2(string joinId)
        {
            joinId = joinId.TrimStart(',');

            var ReturnData = new ReturnData();
            try
            {
                if (!string.IsNullOrEmpty(joinId))
                {
                    var where = "Id IN (" + joinId + ")";
                    var result = IdeaTempDAL.Delete(where);
                    if (result >= 0)
                    {
                       
                       
                        ReturnData.Description = "Xóa  Thành Công";
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
                ReturnData.Description = "Không xác định sáng kiến cần xóa";
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
