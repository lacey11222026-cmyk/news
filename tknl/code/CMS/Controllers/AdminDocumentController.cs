using System.Globalization;
using System.Web.Routing;
using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Models;
using Constants = UTILS.Constants;
namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,Document")]
    public class AdminDocumentController : Controller
    {
        private List<CATEGORY_FULL> _staticCategoryList;
        private List<CATEGORY_FULL> _staticCategoryByUserList;
        protected override void Initialize(RequestContext requestContext)
        {

            _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.Doc);
            _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, requestContext.HttpContext.User.Identity.Name,
                                                                               requestContext.HttpContext.User.IsInRole("Administrator"));
            base.Initialize(requestContext);

        }
        public ActionResult Index(int CategoryId = 0, int Status = -1, int page = 1, string title = "")
        {
            ViewBag.Title = "Quản trị văn bản";
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
            if (CategoryId == 0)
            {
                CategoryId = listcategory.FirstOrDefault().Id;
            }
            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = CategoryId;
            int total = 0;
            int pagesize = 20;
            ViewBag.Keyword = title;
            ViewBag.Status = Status;
            ViewBag.StatusList = new List<EnumInfo>
                                     {
                                         new EnumInfo {Value = -1, Text = "--Tất cả--"},
                                         new EnumInfo {Value = 1, Text = "Hoạt động"},
                                         new EnumInfo {Value = 0, Text = "Khóa"}
                                     };
            var lstdata = new DocumentBO().GetDocumentsFuLLPaged(title, CategoryId, Status, page, pagesize, ref total);
            var model = new DocumentModel
                            {
                                listdata = lstdata,
                                pageIndex = page,
                                pageSize = pagesize,
                                total = total
                            };

            return View(model);

        }
        public ActionResult AddEdit(int Id = 0)
        {
            ViewBag.Title = "Thêm mới văn bản";
            var model = new DOCUMENT_FULL
                            {
                                Id = 0,
                                CreatedDate = DateTime.Now,
                                Hits = 0,
                                CategoryId = 0,
                                Status = 1,
                                CreatedBy = User.Identity.Name

                            };
            
            if (Id > 0)
            {
                model = new DocumentBO().GetDocumentFull(Id);
                model.SPublishedTime = model.PublishDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                ViewBag.Title = "Cập nhật văn bản";
            }

              

            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }

            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = model.CategoryId;
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            string results;


            new DocumentBO().DeleteDocument(Id);
            results = "true";
            return Json(results);

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddEdit(DOCUMENT_FULL obj)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            obj.PublishDate = DateTime.ParseExact(obj.SPublishedTime, "dd/MM/yyyy", culture);
            var firstOrDefault = _staticCategoryList.FirstOrDefault(x => x.Id == obj.CategoryId);
            if (firstOrDefault != null)
            {
                obj.CategoryPathway = firstOrDefault.Pathway;
                obj.Language = firstOrDefault.Language;
            }
            new DocumentBO().CreateUpdateDocument(obj);

            
                
            return RedirectToAction("Index", "AdminDocument");
        }

    }
}
