using BIZ;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UTILS;
using WebMVC4.Models;
using Constants = UTILS.Constants;
namespace WebMVC4.Controllers
{

    public class SystemController : Controller
    {
        //
        // GET: /System/
        [Authorize(Roles = "Administrator")]
        public ActionResult Users(string searchtext = null)
        {
            MembershipUserCollection userCollection;
            if (String.IsNullOrEmpty(searchtext))
            {

                userCollection = Membership.GetAllUsers();
            }
            else
            {

                userCollection = Membership.FindUsersByName(searchtext);
            }
            var model = new UserModel { listuser = userCollection, searchtext = searchtext };
            ViewBag.searchtext = searchtext;
            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult UserAppproved(string username)
        {
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                ItemtType = (int)Constants.CategoryType.System,
                ItemId = 0,
                ItemName = username,
                Note = "Khóa tài người dùng",
                Type = 1

            };
            string results;
            MembershipUser user = Membership.GetUser(username);
            if (user.IsApproved)
            {
                user.IsApproved = false;
                results = "Khóa tài người dùng thành công";
            }
            else
            {
                user.IsApproved = true;
                results = "Kích hoạt người dùng thành công";
                lognewsobj.Note = "Kích hoạt người dùng";

            }
            Membership.UpdateUser(user);

            //Ghi log
            Action<ContentLog> send = InsertContentLog;
            var asynSend = send.BeginInvoke(lognewsobj, null, null);
            return Json(results);

        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult UserDelete(string username)
        {
            string results = "0";
            if (Membership.DeleteUser(username))
            {
                results = "1";
                var lognewsobj = new ContentLog
                {
                    UserName = HttpContext.User.Identity.Name,
                    ItemtType = (int)Constants.CategoryType.System,
                    ItemId = 0,
                    ItemName = username,
                    Note = "Xóa người dùng",
                    Type = 1

                };
                //Ghi log
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);
            }

            return Json(results);

        }
        [Authorize(Roles = "Administrator")]
        public ActionResult UserEdit(string username)
        {
            var listcategory = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.None).Where(x=>x.ParentId == 0).ToList();
            if (listcategory == null)
                listcategory = new List<CATEGORY_FULL>();
            //listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "Khoa học công nghệ" });
            var usercategoryobj = new PublisherCategoryBO().GetByUserName(username);
            var userCategoryPath = usercategoryobj.CategoryPath == null ? "" : usercategoryobj.CategoryPath;
            MembershipUser user = Membership.GetUser(username);

            var roles = System.Web.Security.Roles.GetAllRoles();
            var userRoles = System.Web.Security.Roles.GetRolesForUser(username);
            var view = new ViewUserDetail
            {

                user = user,
                rolenames = roles,
                user_roles = userRoles,
                lstcate = listcategory,
                userCategoryPath = userCategoryPath

            };
            try
            {
                view.AuthorProfile = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
            }
            catch
            {

                view.AuthorProfile = new AuthorProfile();
            }
            if (view.AuthorProfile == null)
            {
                view.AuthorProfile = new AuthorProfile();
            }
            ViewBag.Title = "Cập nhật người dùng : " + username;
            return View(view);
        }
        [HttpPost]
        public ActionResult SaveUserCategory(string svalue, string username)
        {
            var results = "true";

            try
            {
                new PublisherCategoryBO().SetByUserName(username, svalue);

            }
            catch (System.Exception ex)
            {

                results = ex.Message;

            }
            return Json(results);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult UserChangPass(string username, string newPassword)
        {
            string results = "";

            MembershipUser mUser = Membership.GetUser(username, false);
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
                ItemName = username,
                Note = "Đổi mật khẩu",
                Type = 1

            };
            //Ghi log
            Action<ContentLog> send = InsertContentLog;
            var asynSend = send.BeginInvoke(lognewsobj, null, null);
            return Json(results);

        }
        [ValidateInput(false)]
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult UserChangeEmail(string username, string email, string FullName, string Avatar, string Title, string Description, string Content,int Order)
        {
            string results = "";

            MembershipUser mUser = Membership.GetUser(username, false);
           
            if (!string.IsNullOrEmpty(email))
            {
                mUser.Email = email;
                var profile = new AuthorProfile
                {
                    Avatar = Avatar,
                    FullName = FullName,
                    Description = Description,
                    Content = Content,
                    Title = Title,
                    Order = Order,
                };
                mUser.Comment = Utils.ConvertToJson(profile, string.Empty);

                Membership.UpdateUser(mUser);
                var lognewsobj = new ContentLog
                {
                    UserName = HttpContext.User.Identity.Name,
                    ItemtType = (int)Constants.CategoryType.System,
                    ItemId = 0,
                    ItemName = username,
                    Note = "Đổi Email",
                    Type = 1

                };
                //Ghi log
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);
            }
            
            return Json(results);

        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult UserSetRole(string username, string rolename, string roledesc)
        {


            var rolenames = new string[1];
            rolenames[0] = rolename;

            var usernames = new string[1];
            usernames[0] = username;
            string results;
            var lognewsobj = new ContentLog
            {
                UserName = HttpContext.User.Identity.Name,
                ItemtType = (int)Constants.CategoryType.System,
                ItemId = 0,
                ItemName = username,
                Note = "Xóa quyền " + roledesc,
                Type = 1

            };

            if (System.Web.Security.Roles.FindUsersInRole(rolename, username).Length == 1)
            {
                System.Web.Security.Roles.RemoveUsersFromRoles(usernames, rolenames);
                results = "Xóa quyền thành công";
            }
            else
            {
                System.Web.Security.Roles.AddUsersToRoles(usernames, rolenames);
                results = "Thêm quyền thành công";

                lognewsobj.Note = "Thêm quyền " + roledesc;
            }

            //Ghi log
            Action<ContentLog> send = InsertContentLog;
            var asynSend = send.BeginInvoke(lognewsobj, null, null);
            return Json(results);

        }
        [ValidateInput(false)]
        [Authorize(Roles = "Administrator")]
        public ActionResult UserAdd()
        {


            var roles = System.Web.Security.Roles.GetAllRoles();
            
            ViewBag.Title = "Thêm mới người dùng";
            return View(roles);
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult UserAdd(string username, string newPassword, string email, string lstrole,string FullName,string Avatar, string Title, string Description,string Content,int Order)
        {
            username = HttpUtility.UrlDecode(username);
            newPassword = HttpUtility.UrlDecode(newPassword);
            newPassword = UTILS.Utils.MD5Encrypt(newPassword);
            email = HttpUtility.UrlDecode(email);
            lstrole = HttpUtility.UrlDecode(lstrole);
            string results = "Lỗi hệ thống";
            // Create new user
            MembershipUser newUser = Membership.CreateUser(username, newPassword, email);
            if (newUser != null)
            {
                results = "1";
                var lognewsobj = new ContentLog
                {
                    UserName = HttpContext.User.Identity.Name,
                    ItemtType = (int)Constants.CategoryType.System,
                    ItemId = 0,
                    ItemName = username,
                    Note = "Thêm mới người dùng",
                    Type = 1

                };
                //Ghi log
                Action<ContentLog> send = InsertContentLog;
                var asynSend = send.BeginInvoke(lognewsobj, null, null);
                if(!string.IsNullOrEmpty(lstrole))
                {
                    string[] strUserRoles = lstrole.Remove(lstrole.Length - 1, 1).Split('|');
                    if (strUserRoles.Count() > 0)
                    {
                        // Add user to roles
                        System.Web.Security.Roles.AddUserToRoles(newUser.UserName, strUserRoles);
                    }
                }
                MembershipUser mUser = Membership.GetUser(username, false);
                var profile = new AuthorProfile
                {
                    Avatar=Avatar,
                    FullName = FullName,
                    Description= Description,
                    Content = Content,
                    Title = Title,
                    Order=Order,
                };
                mUser.Comment = Utils.ConvertToJson(profile, string.Empty);
                Membership.UpdateUser(mUser);
            }

            return Json(results);

        }
        [Authorize(Roles = "Rate,Administrator")]
        public ActionResult Configuration()
        {


            return View();
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
