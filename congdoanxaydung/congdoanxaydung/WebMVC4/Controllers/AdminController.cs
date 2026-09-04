using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMVC4.Models;
using Constants = UTILS.Constants;
namespace WebMVC4.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index2()
        {

            return View();
        }
        
        public ActionResult MenuBar()
        {

            return PartialView();
        }
        public ActionResult Header()
        {

            return PartialView();
        }
       
        public ActionResult MenuLeft()
        {

            return PartialView();
        }
        [Authorize]
        public ActionResult MultiUpload(int Month = 0)
        {
            if (Month == 0)
            {
                Month = int.Parse(DateTime.Now.Month.ToString());

            }
            var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + Month.ToString() + "\\";

            DirectoryInfo sdir = new DirectoryInfo(folder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var lstImgFiles = sdir.GetFiles().Where(x => x.Name.Split('.').Length == 2).OrderByDescending(x => x.CreationTime).Take(40).ToList();
            ViewBag.Month = Month;
            ViewBag.hdfCurrentFolder = ConfigurationManager.AppSettings["UploadUrl"] + "User/" + HttpContext.User.Identity.Name + "/" + DateTime.Now.Year.ToString() + "/" + Month.ToString() + "/";
            return View(lstImgFiles);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult FileDelete(string filename, string month)
        {
            string results = "0";
            try
            {
                var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + month.ToString() + "\\";
                string strfile = folder + filename;
                System.IO.File.Delete(strfile);
                results = "1";
            }
            catch
            {

                results = "0";
            }
            return Json(results);

        }
      
        
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
            //newPassword = UTILS.Utils.MD5Encrypt(newPassword);
            string results = "";
            MembershipUser mUser = Membership.GetUser(User.Identity.Name, false);
            MembershipUser _mUser = mUser;
            _mUser.UnlockUser();
            if (!string.IsNullOrEmpty(newPassword))
            {
                newPassword = UTILS.Utils.MD5Encrypt(newPassword);
                _mUser.ChangePassword(_mUser.GetPassword(), newPassword);

            }
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                ItemtType = (int)Constants.CategoryType.System,
                ItemId = 0,
                ItemName = HttpContext.User.Identity.Name,
                Note = "Đổi mật khẩu",
                Type = 1

            };
            //Ghi log
            Action<ContentLog> send = InsertContentLog;
            var asynSend = send.BeginInvoke(lognewsobj, null, null);
            return Json(results);

        }

        public ActionResult PopupManagerImages()
        {
            ViewBag.Month = DateTime.Now.Month;
            return PartialView();
        }
        public ActionResult ListImages(int? currPage,int Month = 0 )
        {
            int CurrPage = 1;
            CurrPage = currPage == null ? 1 : currPage.GetValueOrDefault();
            int pagesize = 20;

            var totalRecord = 0;
          
            if (Month == 0)
            {
                Month = int.Parse(DateTime.Now.Month.ToString());

            }
            var folder =  ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + Month.ToString() + "\\";

            DirectoryInfo sdir = new DirectoryInfo(folder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var lstImgFiles = sdir.GetFiles().Where(x => x.Name.Split('.').Length == 2).OrderByDescending(x => x.CreationTime).ToList();
            totalRecord = lstImgFiles.Count();
            int PageCount = (int)(totalRecord / pagesize);
            if (totalRecord > 0)
            {
                if (totalRecord % pagesize > 0)
                    PageCount += 1;
            }
            ViewBag.PageNumber = currPage;
            ViewBag.PageCount = PageCount;
            ViewBag.Month = Month;
            ViewBag.hdfCurrentFolder = ConfigurationManager.AppSettings["UploadUrl"] + "User/" + HttpContext.User.Identity.Name + "/" + DateTime.Now.Year.ToString() + "/" + Month.ToString() + "/";

            lstImgFiles = lstImgFiles.Skip(pagesize * (CurrPage - 1)).Take(pagesize).ToList();
            return PartialView(lstImgFiles);
        }
        public ActionResult PopUpFileInfo()
        {
           
            return PartialView();
        }
        public ActionResult SaveFileUpload()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpeg", ".jpg", ".jpe", ".bmp", ".png", ".gif", ".ico", ".tiff", ".tif", ".svg", ".svgz", ".doc", ".docx", ".txt", ".pdf", ".rtf", ".xlsx", ".xls", ".csv", ".ppt", ".zip", ".zipx", ".tar", ".gz", ".z", ".rar" };
                    var checkextension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(checkextension))
                    {
                        return Json(new { Message = fName });
                    }
                    //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                    //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                    //var pathString = System.Configuration.ConfigurationManager.AppSettings["mediaPath"];
                    //DateTime CrTime = DateTime.Now;
                    //pathString = pathString + CrTime.ToString("yyyy/MM/dd") + "/";
                    var folder =  ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() ;

                    var fileName1 = UTILS.Utils.ReplaceVietnameseChar(Path.GetFileNameWithoutExtension(file.FileName)).Replace(" ", "_");
                    var extend1 = Path.GetExtension(file.FileName);
                    bool isExists = System.IO.Directory.Exists(folder);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(folder);

                    var path = string.Format("{0}\\{1}", folder, fileName1 + extend1);
                    if (System.IO.File.Exists(path))
                    {
                        path = string.Format("{0}\\{1}", folder, fileName1 + new Random().Next(10000) + extend1);
                    }
                    file.SaveAs(path);

                }

            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
        public ActionResult TopNotify()
        {
            var username = HttpContext.User.Identity.Name;
            var expriDate = int.Parse(DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"));
            var lstnoti = new NotifiBO().GetNotifi(username, expriDate);
            var lstnotRead = new NotiReadBO().GetNotiRead(username, expriDate);
            var lstnotUnRead = "";
            var TotalNoti = 0;
            foreach (var item in lstnoti)
            {
                if (!lstnotRead.Where(x => x.NotiId == item.Id).Any())
                {
                    lstnotUnRead += item.Id + ";";
                    TotalNoti++;

                }
            }
            ViewBag.TotalNoti = TotalNoti;
            ViewBag.lstnotUnRead = lstnotUnRead;
            return PartialView(lstnoti);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ReadMoti(string noti)
        {
            var ReturnData = new ReturnData();
            var username = HttpContext.User.Identity.Name;
            var ExpriDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            new NotiReadBO().ReadMulti(ExpriDate, username, noti);
            return Json(ReturnData);
        }
        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult ListNotify(int? currentPage, int? pageSize)
        {
            
           
            int currPage = currentPage == null ? 1 : (int)currentPage;
            int recordPerPage = pageSize == null ? 30 : (int)pageSize;
            var username = HttpContext.User.Identity.Name;
            var expriDate = int.Parse(DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"));
            var lstnoti = new NotifiBO().GetNotifi(username, expriDate);
            ViewBag.TotalRecord = lstnoti.Count();
            ViewBag.PageSize = recordPerPage;
            ViewBag.CurrentPage = currPage;

            var data= lstnoti.Skip(recordPerPage * (currPage - 1)).Take(recordPerPage).ToList();
            ViewBag.Title = "Danh sách tin nhắn";
            return PartialView(data);
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
