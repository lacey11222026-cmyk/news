using BIZ;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UTILS;

namespace WebMVC4.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult LogOn(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.Url = "/";
            }
            ViewBag.Url = Server.UrlDecode(returnUrl);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult LogOn(string UserName, string Password, string returnUrl, string capchar)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });

                if (capchar != Session["Captcha"].ToString())
                {
                    return Json(new { success = false, statusCode = -1, msg = "Mã xác thực không đúng" });

                }
                var data = Membership.GetAllUsers();
                NLogLogger.Info(JsonConvert.SerializeObject(data));
                MembershipUser user = Membership.GetUser(UserName);
                Password = UTILS.Utils.MD5Encrypt(Password);
                if (user != null)
                {
                    var checkLogin = user.GetPassword() == Password ? true : false;
                    if (checkLogin)
                    {


                        // If form login is authentication
                        // Do login with UserData property
                        HttpCookie cookie =
                                FormsAuthentication.GetAuthCookie(UserName, false);

                        FormsAuthenticationTicket ft =
                                FormsAuthentication.Decrypt(cookie.Value);

                        //Cutom user data
                        string userData = UserName;
                        // Declare the new form ticket object
                        FormsAuthenticationTicket newFt =
                                new FormsAuthenticationTicket(
                                        ft.Version,     //version
                                        ft.Name,        //username
                                        ft.IssueDate,   //Issue date
                                        ft.Expiration,  //Expiration date
                                        ft.IsPersistent,
                                        userData,
                                        ft.CookiePath);

                        //re-encrypt the new forms auth ticket that includes the user data
                        string encryptedValue = FormsAuthentication.Encrypt(newFt);

                        //reset the encrypted value of the cookie
                        cookie.Value = encryptedValue;

                        //set the authentication cookie and redirect
                        Response.Cookies.Add(cookie);

                        var cookieExpires = Convert.ToDouble(ConfigurationManager.AppSettings["CookieExpires"]);
                        if (cookieExpires == 0)
                            cookieExpires = 4;
                        cookie.Expires = DateTime.Now.AddHours(cookieExpires);


                        user.LastActivityDate = DateTime.Now;
                        Membership.UpdateUser(user);
                        //FormsAuthentication.SetAuthCookie(UserName, false);
                        var lognewsobj = new ContentLog
                        {
                            UserName = user.UserName,
                            ItemtType = (int)UTILS.Constants.CategoryType.System,
                            ItemId = 0,
                            ItemName = user.UserName,
                            Note = "Đăng nhập",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);
                        return Json(new { success = true, statusCode = 1, msg = "Đăng Nhập Thành Công" });
                        //return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        NLogLogger.Info("username: " + UserName + " pass: " + Password);
                        return Json(new { success = false, statusCode = -1, msg = "Tên đăng nhập hoặc mật khẩu không đúng" });
                    }
                }
                else
                {
                    NLogLogger.Info("username: " + UserName + " pass: " + Password);
                    return Json(new { success = false, statusCode = -2, msg = "Tên đăng nhập hoặc mật khẩu không đúng" });
                }
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "Hệ thống bận vui lòng quay lại sau" });
            }
        }
        public ActionResult LogOff()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
