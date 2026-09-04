using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BIZ;
using BIZ.Entity;
using CMS.Models;

namespace CMS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            ViewBag.Title = "Quản trị hệ thống";
            return View();
        }
        public ActionResult AssesDenied()
        {
            ViewBag.Title = "Quản trị hệ thống";
            return View();
        }
        [ChildActionOnly]
        public ActionResult MenuBar()
        {

            return PartialView();
        }

        [Authorize]
        public ActionResult MultiUpload(string title = "", string account = "", string fromdate = "", string todate = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = DateTime.Now.AddDays(-3);
            //DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }
            if (string.IsNullOrEmpty(account))
                account = User.Identity.Name;

            ViewBag.IsOwner = (account == User.Identity.Name);
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.Keyword = title;
            var lstaccount = GetUserByNewsRole();
            lstaccount.Insert(0, new EnumInfo { SValue = "-1", Text = "--Tất cả--" });
            //lstaccount.Insert(0, new EnumInfo { SValue = User.Identity.Name, Text = "--File của tôi--" });
            ViewBag.AccountList = lstaccount;
            ViewBag.Author = account;
            var lstImgFiles = new FileUserBO().GetFileUsersByFilter(100, title, "", account, fromdate, todate);
            //ViewBag.hdfCurrentFolder = "User/" + HttpContext.User.Identity.Name + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
            ViewBag.Title = "Quản trị hệ thống file";
            return View(lstImgFiles);

        }
        [HttpPost]
        public ActionResult FileDelete(long fileid)
        {
            string results = "0";
            try
            {
                var file = new FileUserBO().GetById(fileid);
                if (file == null)
                    return Json(results);
                if (User.Identity.Name != file.UserName)
                    return Json(results);
                var folder = ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + file.CreateTime.Year.ToString() + "\\" + file.CreateTime.Month.ToString() + "\\" + file.CreateTime.Day.ToString() + "\\";
                string strfile = folder + file.FileName;
                System.IO.File.Delete(strfile);
                new FileUserBO().DeleteFile(fileid);
                results = "1";
            }
            catch
            {

                results = "0";
            }
            return Json(results);

        }
        [HttpPost]
        public ActionResult UpdateFile(long Id, string Contents)
        {
            string results = "0";
            try
            {
                var file = new FileUserBO().GetById(Id);
                if (file == null)
                    return Json(results);
                if (User.Identity.Name != file.UserName)
                    return Json(results);
                file.Keyword = Contents;
                new FileUserBO().CreateUpdateFileUser(file);
                results = "1";
            }
            catch
            {

                results = "0";
            }
            return Json(results);

        }
        [Authorize]
        public ActionResult UserEdit()
        {

            MembershipUser user = Membership.GetUser(User.Identity.Name);

            var roles = Roles.GetAllRoles();
            var userRoles = System.Web.Security.Roles.GetRolesForUser(user.UserName);
            var view = new ViewUserDetail
            {

                user = user,
                rolenames = roles,
                user_roles = userRoles

            };

            ViewBag.Title = "Thông tin tài khoản";
            return View(view);
        }
        [Authorize]
        [HttpPost]
        public ActionResult UserChangPass(string newPassword)
        {
            string results = "";
            MembershipUser mUser = Membership.GetUser(User.Identity.Name, false);
            MembershipUser _mUser = mUser;
            _mUser.UnlockUser();
            if (!string.IsNullOrEmpty(newPassword))
            {
                newPassword = UTILS.Utils.MD5Encrypt(newPassword);
                _mUser.ChangePassword(_mUser.GetPassword(), newPassword);

            }
            return Json(results);

        }
        [ChildActionOnly]
        public ActionResult UploadContent(string myuploader)
        {

            using (var uploader = new CuteWebUI.MvcUploader(System.Web.HttpContext.Current))
            {
                // set value Uploader
                uploader.UploadUrl = Response.ApplyAppPathModifier("~/Post/UploadHandler.ashx");
                uploader.Name = "myuploader";
                uploader.AllowedFileExtensions = "*.jpg,*.png,*.gif,*.doc,*.docx,*.rar,*.zip,*.xls,*.xlsx,*.ppt,*.swf,*.flv,*.mp3,*.avi,*.mp4,*.pdf";
                uploader.ManualStartUpload = true;
                uploader.MaxFilesLimit = 50;
                //uploader.MaxSizeKB = 102400;
                uploader.MultipleFilesUpload = true;
                uploader.InsertButtonID = "uploadbutton";
                uploader.CancelAllMsg = "Ngừng toàn bộ Upload";
                uploader.CancelUploadMsg = "Ngừng Upload";
                //prepair html code for the view
                ViewData["uploaderhtml"] = new HtmlString(uploader.Render());

                //if it's HTTP POST:
                if (!string.IsNullOrEmpty(myuploader))
                {
                    var processedfiles = (from strguid in myuploader.Split('/')
                                          select new Guid(strguid)
                                              into fileguid
                                              select uploader.GetUploadedFile(fileguid)
                                                  into file
                                                  where file != null
                                                  where Path.GetExtension(file.FileName).Equals(".jpg")
                                                  select file.FileName).ToList();

                    if (processedfiles.Count > 0)
                    {
                        ViewData["UploadedMessage"] = string.Join(",", processedfiles.ToArray()) + " đã upload thành công";
                    }
                }

            }
            return PartialView();
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
    }
}
