using Card.Data.DTO;
using Card.Data.Service;
using Card.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Card.CMS.Filter;
using Card.CMS.Models;
using Card.Data.Api;
using System.Configuration;

namespace Card.CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService _userservice;
        private readonly IAccountTokenService _accounttokenservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;
       
        private readonly ITransactionsService _transervice;
       
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } set { } }
        public HomeController(IAccountTokenService accounttokenservice, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice, ITransactionsService transervice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;
            _transervice = transervice;
            _accounttokenservice = accounttokenservice;
          
        }
        public ActionResult ErrorPermission()
        {


            return View();
        }
        public ActionResult ErrorNotPage()
        {


            return View();
        }

        public ActionResult Index()
        {


            if (CurrentUser == null)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult Header()
        {
          
            var user = _userservice.SelectByUserID(CurrentUser.UserID);
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
                    functions = _functionservice.GetListFunctionByUserID(userinfo.UserID);
                    Session[SessionsManager.SESSION_USERFUNCTIONS] = _userroleservice.UserFunction_GetByUserID(userinfo.UserID);

                    //functions = _userroleservice.GetListFunctionByID(userinfo.Type);
                    //Session[SessionsManager.SESSION_USERFUNCTIONS] = _userroleservice.UserFunction_GetByUserID(userinfo.UserID);
                }
            }
            Session[SessionsManager.SESSION_FUNCTIONS] = functions;
            return PartialView(functions);
        }
    }

}