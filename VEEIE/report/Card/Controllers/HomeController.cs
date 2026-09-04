using Car.Data.DTO;
using Car.Data.Service;
using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car.CMS.Filter;
using Car.CMS.Models;
using Car.Data.Api;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;

namespace Car.CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService _userservice;
       
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;
       
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } }
        public HomeController(IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;
           
        }
        public ActionResult Index()
        {
            //string Content = "{xin chao|hello|hi|} Cuong {cam on|thanks}";
            //Content = StringUtils.FomatSMSContent(Content);
            //NLogLogger.DebugMessage(Content);
            //ServerProcess.GetProfile(11000);
            if (CurrentUser == null)
                return RedirectToAction("Login", "Account");
            var user = _userservice.SelectByUserID(CurrentUser.UserID);
            if (user == null)
                return RedirectToAction("Login", "Account");
            //ViewBag.Order = user.StatusOrder;
            Session[SessionsManager.SESSION_USER_FULL] = user;
            return View(user);
        }


        public ActionResult ErrorPermission()
        {


            return View();
        }
        public ActionResult ErrorNotPage()
        {


            return View();
        }


        public ActionResult Header()
        {
           // var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            var user = _userservice.SelectByUserID(CurrentUser.UserID);
            return PartialView(user);
        }
        public ActionResult Balance(Users user)
        {
            
            ViewBag.Balance = user.Balance+ user.BalanceHold;
            ViewBag.BalanceHold = user.BalanceHold>0? user.BalanceHold:0;
            return PartialView();
        }
        public ActionResult StatusBar(Users user = null)
        {
            if (user != null)
            {

                return PartialView(user);
            }
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            user = _userservice.SelectByUserID(userinfo.UserID);
            ViewBag.Balance = user.Balance;
            return PartialView(user);
        }
      
        public ActionResult Menu()
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];

            var functions = (List<Functions>)Session[SessionsManager.SESSION_FUNCTIONS];
            if (userinfo == null)
            {
                return PartialView(null);
            }
            if (functions == null)
            {
                if (userinfo.Type == 1)
                {
                    Session[SessionsManager.SESSION_FUNCTIONS] = _functionservice.GetListFunctionBySystemID(0);
                    Session[SessionsManager.SESSION_USERFUNCTIONS] = new List<UserFunction>();
                }
                else
                {
                    /*bo quyen theo user*/
                    //functions= _functionservice.GetListFunctionByUserID(userinfo.UserID); 
                    //Session[SessionsManager.SESSION_USERFUNCTIONS] = _userroleservice.UserFunction_GetByUserID(userinfo.UserID);

                    functions = _userroleservice.GetListFunctionByID(userinfo.Type);
                    Session[SessionsManager.SESSION_USERFUNCTIONS] = _userroleservice.GroupFunction_GetByID(userinfo.Type);
                }
            }
            Session[SessionsManager.SESSION_FUNCTIONS] = functions;
            return PartialView(functions);
        }


        public ActionResult PopupManagerImages()
        {
            ViewBag.Month = DateTime.Now.Month;
            return PartialView();
        }
        public ActionResult ListImages(int? currPage, int Month = 0)
        {
            int CurrPage = 1;
            CurrPage = currPage == null ? 1 : currPage.GetValueOrDefault();
            int pagesize = 20;

            var totalRecord = 0;

            if (Month == 0)
            {
                Month = int.Parse(DateTime.Now.Month.ToString());

            }
            var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + CurrentUser.Username + "\\" + DateTime.Now.Year.ToString() + "\\" + Month.ToString() + "\\";

            DirectoryInfo sdir = new DirectoryInfo(folder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var lstImgFiles = sdir.GetFiles().OrderByDescending(x => x.CreationTime).ToList();
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
            ViewBag.hdfCurrentFolder = ConfigurationManager.AppSettings["UploadUrl"] + "User/" + CurrentUser.Username + "/" + DateTime.Now.Year.ToString() + "/" + Month.ToString() + "/";

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

                    //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                    //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                    //var pathString = System.Configuration.ConfigurationManager.AppSettings["mediaPath"];
                    //DateTime CrTime = DateTime.Now;
                    //pathString = pathString + CrTime.ToString("yyyy/MM/dd") + "/";
                    var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + CurrentUser.Username + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString();

                    var fileName1 = StringUtils.ReplaceVietnameseChar(Path.GetFileNameWithoutExtension(file.FileName)).Replace(" ", "_");
                    // fileName1 = UTILS.Utils.SubStringFile(fileName1, 30);
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

    }
}