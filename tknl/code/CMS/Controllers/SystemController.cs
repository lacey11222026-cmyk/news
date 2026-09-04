using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BIZ;
using BIZ.Entity;
using CMS.Models;
using Constants = UTILS.Constants;

namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SystemController : Controller
    {
        //
        // GET: /System/

        public ActionResult Users(string searchtext = null)
        {
            MembershipUserCollection userCollection;
            userCollection = String.IsNullOrEmpty(searchtext) ? Membership.GetAllUsers() : Membership.FindUsersByName(searchtext);
            var model = new UserModel { listuser = userCollection, searchtext = searchtext };
            ViewBag.searchtext = searchtext;
            ViewBag.Title = "Quản trị người dùng";
            return View(model);
        }
        [HttpPost]
        public ActionResult UserAppproved(string username)
        {
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

            }
            Membership.UpdateUser(user);

            return Json(results);

        }
        [HttpPost]
        public ActionResult UserDelete(string username)
        {
            string results = "0";
            if (Membership.DeleteUser(username))
                results = "1";
            return Json(results);

        }
        public ActionResult UserEdit(string username)
        {
            ViewBag.Title = "Thông tin tài khoản";
            MembershipUser user = Membership.GetUser(username);

            var roles = Roles.GetAllRoles();
            var userRoles = System.Web.Security.Roles.GetRolesForUser(username);
            var view = new ViewUserDetail
            {

                user = user,
                rolenames = roles,
                user_roles = userRoles

            };
            return View(view);
        }

        [ChildActionOnly]
        public ActionResult UserEditCategory(string username)
        {

            var listcategory = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.None).Where(x => x.Type == 2 || x.Type == 5).ToList();
            if (listcategory == null)
                listcategory = new List<CATEGORY_FULL>();
            ViewData["AvailableNews"] = new SelectList(listcategory, "Id", "Name");


            var lstCate = new List<CATEGORY_FULL>();
            var usercategoryobj = new PublisherCategoryBO().GetByUserName(username);
            if (usercategoryobj != null)
            {
                if (!string.IsNullOrEmpty(usercategoryobj.CategoryPath))
                {
                    foreach (var item in listcategory)
                    {
                        if (usercategoryobj.CategoryPath.Contains("," + item.Id + ","))
                        {
                            var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name };
                            lstCate.Add(x1);
                        }
                    }
                }
            }
            ViewBag.username = username;
            ViewData["SelectedNews"] = new SelectList(lstCate, "Id", "Name");
            return PartialView();
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
        [HttpPost]
        public ActionResult UserChangPass(string username, string newPassword)
        {
            const string results = "";
            //newPassword = UTILS.Utils.MD5Encrypt(newPassword);
            MembershipUser mUser = Membership.GetUser(username, false);
            MembershipUser _mUser = mUser;
            if (_mUser != null)
            {
                _mUser.UnlockUser();
                if (!string.IsNullOrEmpty(newPassword))
                {
                    newPassword = UTILS.Utils.MD5Encrypt(newPassword);
                    _mUser.ChangePassword(_mUser.GetPassword(), newPassword);
                }
            }
            return Json(results);

        }
        [HttpPost]
        public ActionResult UserSetRole(string username, string rolename)
        {


            var rolenames = new string[1];
            rolenames[0] = rolename;

            var usernames = new string[1];
            usernames[0] = username;
            string results;
            if (Roles.FindUsersInRole(rolename, username).Length == 1)
            {
                Roles.RemoveUsersFromRoles(usernames, rolenames);
                results = "Xóa quyền thành công";
            }
            else
            {
                Roles.AddUsersToRoles(usernames, rolenames);
                results = "Thêm quyền thành công";
            }

            //var list = Roles.GetUsersInRole("roleName").Select(Membership.GetUser).ToList()
            return Json(results);

        }
        public ActionResult UserAdd()
        {

            ViewBag.Title = "Thêm mới người dùng";
            var roles = System.Web.Security.Roles.GetAllRoles();
            return View(roles);
        }
        [HttpPost]
        public ActionResult UserAdd(string username, string newPassword, string email, string lstrole)
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
                string[] strUserRoles = lstrole.Remove(lstrole.Length - 1, 1).Split('|');
                if (strUserRoles.Any())
                {
                    // Add user to roles
                    System.Web.Security.Roles.AddUserToRoles(newUser.UserName, strUserRoles);
                }
            }

            return Json(results);

        }
        public ActionResult Configuration()
        {


            ViewBag.Title = "Thiết lập hệ thống";
            return View();
        }
    }
}
