using BIZ;
using DATA;
using DATA.ContentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using UTILS;
using static System.Collections.Specialized.BitVector32;
using static UTILS.Cryptography;
using WebMVC4.Models;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.DynamicData;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Security.Cryptography;
using WebMVC4.Helper;
using System.Web.Razor.Parser;
namespace WebMVC4.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("IndexEn", userinfo);
            }
            //NLogLogger.DebugMessage(GeneratePassword(8));
            return View(userinfo);
        }
        public ActionResult ChangePassword()
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("ChangePasswordEn", userinfo);
            }
            return View(userinfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string Password, string NewPassword)
        {
            try
            {
                var lang = WorkContext.GetLanguage();
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(NewPassword))
                {
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });

                }


                var result = UserDAL.ChangePassword(Session[SessionsManager.SESSION_USERNAME].ToString(), Encrypt.MD5(Password.Trim()), Encrypt.MD5(NewPassword.Trim()));


                if (result > 0)
                {


                    return Json(new { success = true, statusCode = 1, msg = "Success" });
                }
                if (result == -3)
                {
                    if(lang=="vi-vn")
                    {
                        return Json(new { success = false, statusCode = -1, msg = "Mật khẩu cũ không đúng" });
                    }
                    else
                    {
                        return Json(new { success = false, statusCode = -1, msg = "Old password is incorrect" });
                    }
                    
                }
                if (lang == "vi-vn")
                {
                    return Json(new { success = false, statusCode = -1, msg = "Hệ thống đang bận. Vui lòng quay lại sau" });
                }
                else
                {
                    return Json(new { success = false, statusCode = -1, msg = "The system is busy. Please come back later." });
                }
                
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "System Busy" });
            }
        }

        public ActionResult Header()
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return PartialView(userinfo);
        }
        public ActionResult Verify(string username, string code)
        {
            ViewBag.status = -1;
            var lang = WorkContext.GetLanguage();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(code))
            {
                ViewBag.message = "Dữ liệu không hợp lệ";
                if (lang != "vi-vn")
                    ViewBag.message = "Invalid data";
                return View();
            }
            var m_Users = UserDAL.GetByUserName(username);
            if (m_Users == null)
            {
                ViewBag.message = "Tài khoản không tồn tại";
                if (lang != "vi-vn")
                    ViewBag.message = "Account does not exist";
                return View();
            }
            if (m_Users.Status == 1)
            {
                ViewBag.message = "Tài khoản đã xác minh. Bạn có thể đăng nhập ngay";
                if (lang != "vi-vn")
                    ViewBag.message = "Account verified successfully. You can log in now.";
                ViewBag.status = 1;
                return View();
            }
            if (m_Users.Mobile == code)
            {
                m_Users.Status = 1;
                ViewBag.message = "Tài khoản đã xác minh thành công. Bạn có thể đăng nhập ngay";
                if (lang != "vi-vn")
                    ViewBag.message = "Account verified successfully. You can log in now\"";
                ViewBag.status = 1;
                var where = " [Id] = " + m_Users.Id.ToString();
                var update = "[Status] =1";
                var result = UserDAL.UpdateUserDynamic(where, update);
                return View();
            }
            if (m_Users.Mobile != code)
            {
                ViewBag.message = "Mã xác minh không hợp lệ";
                if (lang != "vi-vn")
                    ViewBag.message = "Invalid verification code";
                return View();
            }
            return View();
        }
        public ActionResult Login(string act)
        {
            //TelegramNotify.SendWarning("1223735562", $"Thừa đơn xyz tài khoản 123 mã thẻ 456 số tiền 500000/100000 ");
            if (!string.IsNullOrEmpty(act) && act == "out")
            {
                ///m_UserValidation.SignOut();
                Session.Abandon();
                Session.RemoveAll();
                ExpireAllCookies();
                Response.Redirect("~/", true);
            }

            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("LoginEn");
            }
            return View();
        }
        public ActionResult ResetPassword()
        {
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("ResetPasswordEn");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword( string Email)
        {
            try
            {
                if ( string.IsNullOrEmpty(Email))
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });

                var lang = WorkContext.GetLanguage();

                var m_Users = UserDAL.GetByEmail(Email);
                if (m_Users != null && m_Users.Id > 0)
                {

                    if (m_Users.Email != Email)
                    {
                        if (lang == "vi-vn")
                        {
                            return Json(new { success = false, statusCode = -2, msg = "Email không chính xác" });
                        }
                        else
                        {
                            return Json(new { success = false, statusCode = -2, msg = "Email is incorrect" });
                        }
                       
                    }

                    if (Session["sendmail3"] == null)
                    {
                        Session["sendmail3"] = "1";
                    }
                    var countsession = Convert.ToInt32(Session["sendmail3"].ToString());
                    if (countsession > 3)
                    {
                        if (lang == "vi-vn")
                        {
                            return Json(new { success = false, statusCode = 1, msg = "Thao tác quá nhiều. Vui lòng quay lại sau" });
                        }
                        else
                        {
                            return Json(new { success = false, statusCode = 1, msg = "Too many operations. Please come back later." });
                        }
                     
                    }
                    var newpass = GeneratePassword(8);
                    var where = " [Id] = " + m_Users.Id.ToString();
                    var update = "[Password] =" + "'" + Encrypt.MD5(newpass) + "'";
                    var result = UserDAL.UpdateUserDynamic(where, update);
                    Session["sendmail3"] = (countsession + 1).ToString();

                   

                    string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailpw.html"));
                    string mailbody = String.Format(mailform, m_Users.FistName, newpass, m_Users.UserName);
                    string mailsubject = "Mật khẩu mới của bạn";

                    if (lang != "vi-vn")
                    {
                        mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailpwen.html"));
                        mailbody = String.Format(mailform, m_Users.FistName, newpass, m_Users.UserName);
                        mailsubject = "Your new password";
                    }
                    Action<string, string, string> send = (string subject, string body, string email) =>
                    {
                        EmailService.SendMail(subject, body, email);

                    };
                    send.BeginInvoke(mailsubject, mailbody, m_Users.Email, null, null);
                    if (lang == "vi-vn")
                    {
                        return Json(new { success = true, statusCode = 1, msg = $"Lấy lại mật khẩu cho tài khoản {m_Users.UserName} thành công" });
                    }
                    else
                    {
                        return Json(new { success = true, statusCode = 1, msg = $"Password recovered for account {m_Users.UserName}  successfully" });

                    }
                   

                }
                if (lang == "vi-vn")
                {
                    return Json(new { success = false, statusCode = -1, msg = "Tên đăng nhâp không tồn tại" });
                }
                else
                {
                    return Json(new { success = false, statusCode = -1, msg = "Tên đang nhâp không tồn tại" });
                }
               
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "System Busy" });
            }
        }
        private void ExpireAllCookies()
        {
            if (HttpContext != null)
            {
                int cookieCount = Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = Request.Cookies[i];
                    if (cookie != null)
                    {
                        var expiredCookie = new HttpCookie(cookie.Name)
                        {
                            Expires = DateTime.Now.AddDays(-1),
                            Domain = cookie.Domain
                        };
                        Response.Cookies.Add(expiredCookie); // overwrite it
                    }
                }

                // clear cookies server side
                Request.Cookies.Clear();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            try
            {
                var lang = WorkContext.GetLanguage();
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });


                var password = Encrypt.MD5(Password.Trim());

                int checkLogin = UserDAL.Authentication(Username.Trim(), password);
                var msg = "Username hoặc Password không đúng";
                if (lang != "vi-vn")
                    msg = "Incorrect username or password";
                if (checkLogin > 0)
                {
                    var m_Users = UserDAL.GetByUserName(Username);
                    if (m_Users != null && m_Users.Id > 0)
                    {

                        if (m_Users.Status != 1)
                        {
                            Session["UserNameVerify"] = m_Users.UserName;
                            return Json(new { success = false, statusCode = -2, msg = "Tài khoản chưa xác minh" });
                        }

                        Session[SessionsManager.SESSION_USERID] = m_Users.Id;
                        Session[SessionsManager.SESSION_USERNAME] = m_Users.UserName;
                        var userinfo = new UserSession
                        {
                            UserID = m_Users.Id,
                            Username = m_Users.UserName,
                            FirstName = m_Users.FistName,
                            Email = m_Users.Email,
                            Mobile = m_Users.Mobile,
                            LastName = m_Users.LastName,
                            Organ = m_Users.Organ
                        };

                        Session[SessionsManager.SESSION_USER] = userinfo;
                        //Session[SessionsManager.SESSION_USER_FULL] = m_Users;
                        //Session[SessionsManager.SESSION_TOKEN] = ServerProcess.GetUserTokenCache(m_Users.UserAPI, m_Users.PasswordAPI);
                        string SessionID = Session.SessionID;


                        return Json(new { success = true, statusCode = 1, msg = "Đăng Nhập Thành Công" });

                    }

                    return Json(new { success = false, statusCode = -1, msg = msg });
                }
                return Json(new { success = false, statusCode = -1, msg = msg });
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "System Busy" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify()
        {
            var username = Session["UserNameVerify"].ToString();
            var user = UserDAL.GetByUserName(username);
            if (user != null && user.Id > 0)
            {
                if (Session["sendmail"] == null)
                {
                    Session["sendmail"] = "1";
                }
                var countsession = Convert.ToInt32(Session["sendmail"].ToString());
                if (countsession > 3)
                {
                    return Json(new { success = false, statusCode = 1, msg = "Success" });
                }
                Session["sendmail"] = (countsession + 1).ToString();



                string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailvr.html"));
                string mailbody = String.Format(mailform, user.FistName, "https://jetp.moit.gov.vn/User/Verify?username=" + user.UserName + "&code=" + user.Mobile);
                string mailsubject = "Xác minh tài khoản của bạn";
                var lang = WorkContext.GetLanguage();
                if (lang != "vi-vn")
                {
                    mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailvren.html"));
                    mailbody = String.Format(mailform, user.FistName, "https://jetp.moit.gov.vn/User/Verify?username=" + user.UserName + "&code=" + user.Mobile);
                    mailsubject = "Please verify your email address";
                }
                Action<string, string, string> send = (string subject, string body, string email) =>
                {
                    EmailService.SendMail(subject, body, email);

                };
                send.BeginInvoke(mailsubject, mailbody, user.Email, null, null);

                return Json(new { success = true, statusCode = 1, msg = "Success" });
            }
            return Json(new { success = true, statusCode = 1, msg = "Success" });
        }
        public ActionResult Register()
        {
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("RegisterEn");
            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reg(string Username, string Password, string FistName, string LastName, string Email, string Mobile, string Organ)
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(FistName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Email))
                {
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });

                }
                var user = new User
                {
                    UserName = Username,
                    Password = Encrypt.MD5(Password.Trim()),
                    FistName = FistName,
                    LastName = LastName,
                    Email = Email,
                    Mobile = GenerateRandomNumberCode(5),
                    Organ = Organ,
                };
                var lang = WorkContext.GetLanguage();
                var reg = UserDAL.Reg(user);
                if (reg > 0)
                {
                    string mailsubject = "Xác minh tài khoản của bạn";

                    string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailvr.html"));
                    string mailbody = String.Format(mailform, user.FistName, "https://jetp.moit.gov.vn/User/Verify?username=" + user.UserName + "&code=" + user.Mobile);



                    if (lang != "vi-vn")
                    {
                        mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailvren.html"));
                        mailbody = String.Format(mailform, user.FistName, "https://jetp.moit.gov.vn/User/Verify?username=" + user.UserName + "&code=" + user.Mobile);
                        mailsubject = "Please verify your email address";
                    }


                    Action<string, string, string> send = (string subject, string body, string email) =>
                    {
                        EmailService.SendMail(subject, body, email);

                    };
                    send.BeginInvoke(mailsubject, mailbody, user.Email, null, null);

                    return Json(new { success = true, statusCode = 1, msg = "Success" });
                }
                else
                {
                    if (reg == -51)
                    {
                        if (lang == "vi-vn")
                        {
                            return Json(new { success = false, statusCode = -1, msg = "Tài khoản đã tồn tại" });
                        }
                        else
                        {
                            return Json(new { success = false, statusCode = -1, msg = "Account already exists" });
                        }
                        
                    }
                    if (reg == -52)
                    {
                        if (lang == "vi-vn")
                        {
                            return Json(new { success = false, statusCode = -1, msg = "Email đã tồn tại" });
                        }
                        else
                        {
                            return Json(new { success = false, statusCode = -1, msg = "Email already exists" });
                        }
                       
                    }
                }
                if (lang == "vi-vn")
                {
                    return Json(new { success = false, statusCode = -1, msg = "Hệ thống đang bận. Vui lòng quay lại sau" });
                }
                else
                {
                    return Json(new { success = false, statusCode = -1, msg = "System Busy" });
                }
               
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "System Busy" });

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(string FistName, string LastName, string Organ)
        {
            try
            {
                if (string.IsNullOrEmpty(FistName) || string.IsNullOrEmpty(LastName))
                {
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });

                }
                FistName = Utils.FormatKeywordSearch(FistName);
                LastName = Utils.FormatKeywordSearch(LastName);
                Organ = Utils.FormatKeywordSearch(Organ);
                var where = " [Id] = " + Session[SessionsManager.SESSION_USERID].ToString();
                var update = "[FistName] =" + "N'" + FistName + "'";
                update += " ,[LastName] =" + "N'" + LastName + "'";
                update += " ,[Organ] =" + "N'" + Organ + "'";
                var result = UserDAL.UpdateUserDynamic(where, update);


                if (result > 0)
                {
                    var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
                    userinfo.FirstName = FistName;
                    userinfo.LastName = LastName;
                    userinfo.Organ = Organ;

                    return Json(new { success = true, statusCode = 1, msg = "Success" });
                }


                return Json(new { success = false, statusCode = -1, msg = "System Busy" });
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "System Busy" });
            }
        }
        public ActionResult Project()
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("ProjectEn", userinfo);
            }
            return View(userinfo);
        }
        public ActionResult AddProject(int Id = 0)
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
           ViewBag.listLocation = new TestLocationBO().GetAllCache();
            var obj = new UserProjectFull();
            if (Id > 0)
            {
                var project = UserProjectDAL.GetDetail(Id);
                if (project == null)
                {
                    return RedirectToAction("Project");
                }
                if (project.Username != Session[SessionsManager.SESSION_USERNAME].ToString())
                {
                    return RedirectToAction("Project");
                }
                if (project.Status != 0)
                {
                    return RedirectToAction("Project");
                }
                obj.Id = project.Id;
                obj.Name = project.Name;
                obj.Location = project.Location;
                obj.Type = project.Type;
                obj.SubType = project.SubType;
                obj.Unit = project.Unit;
                obj.UnitIInfo = project.UnitIInfo;
                obj.Organ = project.Organ;
                obj.Total = project.Total;
                obj.Currency = project.Currency;
                obj.Detail = project.Detail;
                obj.Source = project.Source;
                obj.Progress = project.Progress;
                obj.LegalStatus = project.LegalStatus;
                obj.Description = project.Description;
                obj.Impact = project.Impact;
                obj.Document = project.Document;
                obj.Rule1 = project.Rule1;
                obj.Rule2 = project.Rule2;
                obj.Rule3 = project.Rule3;
                obj.Rule4 = project.Rule4;
                obj.Config = project.Config;
                obj.Username = project.Username;
                obj.Status = project.Status;
                obj.ProjectConfig = JsonConvert.DeserializeObject<UserProjectConfig>(obj.Config);
                if (string.IsNullOrEmpty(obj.ProjectConfig.TADetail))
                {
                    obj.ProjectConfig.TADetail = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Time))
                {
                    obj.ProjectConfig.Time = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Finish))
                {
                    obj.ProjectConfig.Finish = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Support))
                {
                    obj.ProjectConfig.Support = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Rate))
                {
                    obj.ProjectConfig.Rate = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Revenue))
                {
                    obj.ProjectConfig.Revenue = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Finance))
                {
                    obj.ProjectConfig.Finance = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.UnitDev))
                {
                    obj.ProjectConfig.UnitDev = " ";
                }

                if (string.IsNullOrEmpty(obj.ProjectConfig.Role))
                {
                    obj.ProjectConfig.Role = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Email))
                {
                    obj.ProjectConfig.Email = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Mobile))
                {
                    obj.ProjectConfig.Mobile = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Fullname))
                {
                    obj.ProjectConfig.Fullname = " ";
                }
                if (obj.Type == 2)
                {
                    if (WorkContext.GetLanguage() != "vi-vn")
                    {
                        return View("AddProject2En", obj);
                    }
                    return View("AddProject2", obj);
                }
            }
            else
            {
                obj.Currency = "";
                obj.Id = 0;
                obj.Description = "";
                obj.Rule1 = "";
                obj.Rule2 = "";
                obj.Rule3 = "";
                obj.Rule4 = "";
                obj.ProjectConfig = new UserProjectConfig { TA = -1 };
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("AddProjectEn", obj);
            }
            return View(obj);
        }
        public ActionResult AddProject2(int Id = 0)
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.listLocation = new TestLocationBO().GetAllCache();
            var obj = new UserProjectFull();
            if (Id > 0)
            {
                var project = UserProjectDAL.GetDetail(Id);
                if (project == null)
                {
                    return RedirectToAction("Project");
                }
                if (project.Username != Session[SessionsManager.SESSION_USERNAME].ToString())
                {
                    return RedirectToAction("Project");
                }
                if (project.Status != 0)
                {
                    return RedirectToAction("Project");
                }
                obj.Id = project.Id;
                obj.Name = project.Name;
                obj.Location = project.Location;
                obj.Type = project.Type;
                obj.SubType = project.SubType;
                obj.Unit = project.Unit;
                obj.UnitIInfo = project.UnitIInfo;
                obj.Organ = project.Organ;
                obj.Total = project.Total;
                obj.Currency = project.Currency;
                obj.Detail = project.Detail;
                obj.Source = project.Source;
                obj.Progress = project.Progress;
                obj.LegalStatus = project.LegalStatus;
                obj.Description = project.Description;
                obj.Impact = project.Impact;
                obj.Document = project.Document;
                obj.Rule1 = project.Rule1;
                obj.Rule2 = project.Rule2;
                obj.Rule3 = project.Rule3;
                obj.Rule4 = project.Rule4;
                obj.Config = project.Config;
                obj.Username = project.Username;
                obj.Status = project.Status;
                obj.ProjectConfig = JsonConvert.DeserializeObject<UserProjectConfig>(obj.Config);
                if (obj.Type == 1)
                {
                    if (WorkContext.GetLanguage() != "vi-vn")
                    {
                        return View("AddProjectEn", obj);
                    }
                    return View("AddProject", obj);
                }
            }
            else
            {
                obj.Currency = "";
                obj.Id = 0;
                obj.Description = "";
                obj.Rule1 = "";
                obj.Rule2 = "";
                obj.Rule3 = "";
                obj.Rule4 = "";

                obj.ProjectConfig = new UserProjectConfig { TA = -1 };
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("AddProject2En", obj);
            }
            return View(obj);
        }
        public ActionResult Detail(int Id)
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.listLocation = new TestLocationBO().GetAllCache();

            var obj = new UserProjectFull();

            var project = UserProjectDAL.GetDetail(Id);
            if (project == null)
            {
                return RedirectToAction("Project");
            }
            if (project.Username != Session[SessionsManager.SESSION_USERNAME].ToString())
            {
                return RedirectToAction("Project");
            }

            obj.Id = project.Id;
            obj.Name = project.Name;
            obj.Location = project.Location;
            obj.Type = project.Type;
            obj.SubType = project.SubType;
            obj.Unit = project.Unit;
            obj.UnitIInfo = project.UnitIInfo;
            obj.Organ = project.Organ;
            obj.Total = project.Total;
            obj.Currency = project.Currency;
            obj.Detail = project.Detail;
            obj.Source = project.Source;
            obj.Progress = project.Progress;
            obj.LegalStatus = project.LegalStatus;
            obj.Description = project.Description;
            obj.Impact = project.Impact;
            obj.Document = project.Document;
            obj.Rule1 = project.Rule1;
            obj.Rule2 = project.Rule2;
            obj.Rule3 = project.Rule3;
            obj.Rule4 = project.Rule4;
            obj.Config = project.Config;
            obj.Username = project.Username;
            obj.Status = project.Status;
            obj.ProjectConfig = JsonConvert.DeserializeObject<UserProjectConfig>(obj.Config);
            if (string.IsNullOrEmpty(obj.ProjectConfig.TADetail))
            {
                obj.ProjectConfig.TADetail = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Time))
            {
                obj.ProjectConfig.Time = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Finish))
            {
                obj.ProjectConfig.Finish = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Support))
            {
                obj.ProjectConfig.Support = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Rate))
            {
                obj.ProjectConfig.Rate = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Revenue))
            {
                obj.ProjectConfig.Revenue = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Finance))
            {
                obj.ProjectConfig.Finance = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.UnitDev))
            {
                obj.ProjectConfig.UnitDev = " ";
            }

            if (string.IsNullOrEmpty(obj.ProjectConfig.Role))
            {
                obj.ProjectConfig.Role = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Email))
            {
                obj.ProjectConfig.Email = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Mobile))
            {
                obj.ProjectConfig.Mobile = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.Fullname))
            {
                obj.ProjectConfig.Fullname = " ";
            }
            if (obj.Type == 2)
            {
                if (WorkContext.GetLanguage() != "vi-vn")
                {
                    return View("Detail2En", obj);
                }
                return View("Detail2", obj);
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("DetailEn", obj);
            }
            return View(obj);
        }
        public ActionResult ManageProject()
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (WorkContext.GetLanguage() != "vi-vn")
            {
                return View("ManageProjectEn", userinfo);
            }
            return View();
        }
        public ActionResult ListProject(int? status, string keyword)
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return new EmptyResult();
            }
            var data = new List<UserProject>();

            //int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;

            data = UserProjectDAL.TopProject(1000, Session[SessionsManager.SESSION_USERNAME].ToString(), Status, keyword);


            return PartialView(data);
        }
        public ActionResult ListProjectEn(int? status, string keyword)
        {
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo == null)
            {
                return new EmptyResult();
            }
            var data = new List<UserProject>();

            //int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;

            data = UserProjectDAL.TopProject(1000, Session[SessionsManager.SESSION_USERNAME].ToString(), Status, keyword);


            return PartialView(data);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(UserProject Project, UserProjectConfig ProjectConfig)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Project.Username = Session[SessionsManager.SESSION_USERNAME].ToString();

                Project.Config = Utils.ConvertToJson(ProjectConfig, string.Empty);
                var result = UserProjectDAL.InsertUpdate(Project);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Project.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                        ReturnData.Description = "Thêm mới Thành Công";

                    if(Project.Status==1)
                    {
                        var lang = WorkContext.GetLanguage();
                        var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
                        string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailgui.html"));
                      
                        string mailsubject = "Thông báo xác nhận đề xuất dự án";

                        if (lang != "vi-vn")
                        {
                            mailform = System.IO.File.ReadAllText(Server.MapPath("/Content2/MailFormat/mailguien.html"));
                           
                            mailsubject = "Project Proposal Submission Confirmation";
                        }
                        Action<string, string, string> send = (string subject, string body, string email) =>
                        {
                            EmailService.SendMail(subject, body, email);

                        };
                        send.BeginInvoke(mailsubject, mailform, userinfo.Email, null, null);
                    }    
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
        private string GenerateRandomNumberCode(int length)
        {
            Random rnd = new Random();
            string result = "";

            for (int i = 0; i < length; i++)
            {
                result += rnd.Next(0, 10); // số từ 0 đến 9
            }

            return result;
        }
        public string GeneratePassword(int length)
        {
            if (length < 4)
                throw new ArgumentException("Password length must be at least 4 to include all character types.");

            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!#%^*";

            string all = upper + lower + digits + special;

            var result = new char[length];

            // Đảm bảo mỗi loại ký tự xuất hiện ít nhất 1 lần
            result[0] = upper[RandomCharIndex(upper.Length)];
            result[1] = lower[RandomCharIndex(lower.Length)];
            result[2] = digits[RandomCharIndex(digits.Length)];
            result[3] = special[RandomCharIndex(special.Length)];

            // Điền phần còn lại ngẫu nhiên từ toàn bộ tập
            for (int i = 4; i < length; i++)
                result[i] = all[RandomCharIndex(all.Length)];

            // Xáo trộn để tránh predictable pattern (vị trí 0–3)
            //Shuffle(result);

            return new string(result);
        }

        // Lấy chỉ số ngẫu nhiên an toàn
        public int RandomCharIndex(int max)
        {
             var rng = RandomNumberGenerator.Create();
            byte[] buffer = new byte[4];
            int value;

            do
            {
                rng.GetBytes(buffer);
                value = BitConverter.ToInt32(buffer, 0) & int.MaxValue;
            }
            while (value >= (int.MaxValue - (int.MaxValue % max)));

            return value % max;
        }

        // Xáo trộn Fisher-Yates
        public void Shuffle(char[] array)
        {
            var rng = RandomNumberGenerator.Create();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = RandomCharIndex(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        //static string GeneratePassword(int length)
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        //    char[] result = new char[length];

        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        byte[] buffer = new byte[sizeof(uint)];

        //        for (int i = 0; i < length; i++)
        //        {
        //            rng.GetBytes(buffer);
        //            uint num = BitConverter.ToUInt32(buffer, 0);
        //            result[i] = chars[(int)(num % (uint)chars.Length)];
        //        }
        //    }

        //    return new string(result);
        //}
    }


}

