using Antlr.Runtime.Misc;
using Card.Data.DTO;
using Card.Data.Service;
using Card.CMS.Filter;
using Card.CMS.Models;

using Card.Utility;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATA.ContentDB;

namespace Card.CMS.Controllers
{
    public class DocumentController : Controller
    {
        public UserFunction Permission { get { return ((UserFunction)Session[SessionsManager.SESSION_PERMISSION]); } }
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } set { } }
        private readonly IContentsService _contentservice;
        private readonly ICommentsService _commentservice;
        private readonly IUsersService _userservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IGroupsService _groupservice;
        private readonly IUserRoleService _userroleservice;
        private readonly ITransactionsService _transervice;
        public DocumentController(IContentsService contentservice, ICommentsService commentservice, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IGroupsService groupservice, IUserRoleService userroleservice, ITransactionsService transervice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;
            _groupservice = groupservice;
            _transervice = transervice;
            _contentservice = contentservice;
            _commentservice = commentservice;
        }
        // Dán vào Controller hiện tại. BẮT BUỘC nối hai helper phân quyền với hệ thống đăng nhập thật.
        [PermissionFilter(FunctionCode = FunctionCode.OrderList)]
        public ActionResult Index()
        {
            ViewBag.fromDate = new DateTime(DateTime.Today.Year, 1, 1);
            ViewBag.toDate = DateTime.Today;

            bool canViewAll = false;         // TODO: chỉ Tổng hợp/Lãnh đạo có quyền
            if (CurrentFullUser.Group == 13)
                canViewAll = true;
            ViewBag.canViewAll = canViewAll;
            return View();
        }

        [HttpGet]
        [PermissionFilter(FunctionCode = FunctionCode.OrderList)]
        public ActionResult ListDocument(string title, int? currentPage, int? pageSize, int? group, string fromDate, string endDate)
        {
            int page = Math.Max(1, currentPage ?? 1);
            int size = Math.Max(1, Math.Min(200, pageSize ?? 50));
            int requestedGroup = group ?? -1;
            int ownGroupId = CurrentFullUser.Group; // TODO: triển khai từ session/claims thật
            bool canViewAll = false;         // TODO: chỉ Tổng hợp/Lãnh đạo có quyền
            if (CurrentFullUser.Group == 13)
                canViewAll = true;
            if (ownGroupId <= 0 && !canViewAll) return new HttpStatusCodeResult(403);
            if (!canViewAll && requestedGroup >= 0 && requestedGroup != ownGroupId)
                return new HttpStatusCodeResult(403);
            int effectiveGroup = canViewAll ? requestedGroup : ownGroupId;
            int total = 0;
            var data = VanThuSearchDAL.GetSearch(title, effectiveGroup, page, size, ref total, fromDate, endDate);

            ViewBag.currentGroupId = CurrentFullUser.Group;

            ViewBag.TotalRecord = total;
            ViewBag.PageSize = size;
            ViewBag.CurrentPage = page;
            ViewBag.canViewAll = canViewAll;
            return PartialView("ListDocument", data);
        }
        // Định nghĩa GetAuthenticatedGroupId() và CanViewAllVanThu() theo hệ thống đăng nhập của bạn.
        // Bảo vệ GetDocumentDetail(id) bằng cùng nguyên tắc phân quyền và khóa chỉnh sửa IsCompleted.

        // Đưa hai action sau vào Controller hiện tại(giữ namespace và using sẵn có).
        // using System; using DATA.ContentDB;
        [PermissionFilter(FunctionCode = FunctionCode.OrderList)]
        public ActionResult GetDocumentDetail(int Id = 0)
        {
            var model = Id > 0 ? VanThuDAL.GetDetail(Id) : new VanThu { Id = 0, Step = 1 };
            if (model == null) return HttpNotFound();

            if(Id>0)
            {
                if (model.GroupId!=CurrentFullUser.Group)
                    return RedirectToAction("ErrorPermission", "Home");
            }    
           
            if (!model.Step.HasValue || model.Step < 1 || model.Step > 7) model.Step = 1;
            ViewBag.Title = Id == 0 ? "Thêm mới đơn thư" : "Cập nhật đơn thư - Bước " + model.Step;
            // TODO: kiểm tra quyền truy cập theo GroupId của người dùng trước khi trả View.
            return View("GetDocumentDetail", model);
        }

        // Đưa vào Controller hiện tại; thay GetGroupIdFromAuthenticatedUser bằng hệ thống quyền thực tế.
        // Không đọc GroupId từ form. GET GetDocumentDetail cũng phải kiểm tra GroupId.
        [ValidateAntiForgeryToken]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.OrderList)]
        public JsonResult SaveData(VanThu doc, string direction)
        {
            try
            {
                int? groupId = CurrentFullUser.Group; // TODO: map user -> GroupId từ server
                if (!groupId.HasValue) return Json(new { ResponseCode = -403, Description = "Chưa xác định được đơn vị" });

                if (doc.Id == 0)
                {


                    // Lấy ngày báo cáo từ form
                    DateTime ngayBaoCao;

                    string ngayBaoCaoStr = Request.Form["NgayBaoCao"];
                    string SoCvDenStr = Request.Form["SoCvDen"];

                    if (!DateTime.TryParseExact(
                        ngayBaoCaoStr,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out ngayBaoCao))
                    {
                        return Json(new
                        {
                            ResponseCode = -600,
                            Description = "Ngày báo cáo không hợp lệ"
                        });
                    }

                    doc.NgayBaoCao = ngayBaoCao;
                    doc.ReportCode = Helper.HtmlHelpers.GetGroupCode(CurrentFullUser.Group) + "-" + SoCvDenStr + "-" + ngayBaoCao.ToString("yyyy");
                }


                int currentStep = doc.Step ?? 1;
                int id = VanThuStepDAL.SaveStep(doc, currentStep, direction, Request.Form, groupId, doc.ReportCode);
                int nextStep = direction == "next" ? Math.Min(7, currentStep + 1) : direction == "back" ? Math.Max(1, currentStep - 1) : currentStep;
                return Json(new { ResponseCode = 1, Id = id, Step = nextStep, Description = "Lưu thành công" });
            }
            catch (UnauthorizedAccessException) { return Json(new { ResponseCode = -403, Description = "Không có quyền thao tác hồ sơ" }); }
            catch (FormatException ex) { return Json(new { ResponseCode = -600, Description = ex.Message }); }
            catch (Exception ex) { NLogLogger.PublishException(ex); return Json(new { ResponseCode = -99, Description = "Không lưu được hồ sơ. Vui lòng kiểm tra dữ liệu hoặc tải lại trang." }); }
        }


        
    }
}