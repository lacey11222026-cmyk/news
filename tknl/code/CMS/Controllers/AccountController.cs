using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CMS.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.Url = Server.UrlDecode(returnUrl);
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LogOn(string UserName, string Password, string returnUrl)
        {
            //if (ModelState.IsValid)
            //{
                Password = UTILS.Utils.MD5Encrypt(Password);
                MembershipUser user = Membership.GetUser(UserName);
        
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
                        Response.Cookies.Add(cookie);
                        //var cookieExpires = Convert.ToDouble(ConfigurationManager.AppSettings["CookieExpires"]);
                        //if (cookieExpires == 0)
                          var  cookieExpires = 1440;
                        cookie.Expires = DateTime.Now.AddMinutes(cookieExpires);


                        user.LastActivityDate = DateTime.Now;
                        Membership.UpdateUser(user);
                        //FormsAuthentication.SetAuthCookie(UserName, false);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        return Redirect(returnUrl);
                        //return RedirectToAction("Index", "Admin");
                    }
                }

                //ModelState.AddModelError("", "Tài khoản truy cập không hợp lệ.");
            //}
            return View();
        }
        public ActionResult LogOff()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
           
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserUserChangePassword(string Password, string NewsPassword)
        {

            string results = "";
            Password = UTILS.Utils.MD5Encrypt(NewsPassword);
            NewsPassword = UTILS.Utils.MD5Encrypt(Password);
            MembershipUser mUser = Membership.GetUser(HttpContext.User.Identity.Name, false);
            MembershipUser _mUser = mUser;
            _mUser.UnlockUser();
            if (_mUser.GetPassword()!=Password)
            {
                if(_mUser.ChangePassword(Password, NewsPassword))
                {
                    results = "Đổi mật khẩu thành công!";
                }
                else
                {
                    results = "Đổi mật khẩu thất bại!";
                }
            }
            else
            {
                results = "Mật khẩu không đúng!";
            }
                
            return Json(results);

        }

    }
}
