using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Local
{
    public partial class LoginGoogle : System.Web.UI.Page
    {
        public string UrlLogin { get; set; }

        public string RedirectUri = UTILS.Config.ApplicationUrl + "LoginGoogle.aspx";

        public string Code
        {
            get
            {
                var c = Request.QueryString["code"];
                if (string.IsNullOrEmpty(c))
                    return "";
                return c;
            }
        }

        public string State
        {
            get
            {
                var s = Request.QueryString["state"];
                if (string.IsNullOrEmpty(s))
                    return "";
                return s;
            }
        }

        private string GoogleTokenKey = "GoogleTokenKey";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    var datatoken = GoogleApiService.GetAccessToken(Code, RedirectUri);

                    if (datatoken != null)
                    {
                        var userInfo = GoogleApiService.GetUserInfo(datatoken.access_token);
                        if (userInfo != null)
                        {
                            var email = userInfo.email;
                            //email = "huongtt@vuit.org.vn";
                            var userName = "";
                            try
                            {
                                userName = Membership.GetUserNameByEmail(email);
                            }
                            catch (Exception)
                            {
                            }
                            if (UTILS.Config.AdminAcount.Contains("," + email + ","))
                            {
                                userName = "quantri";
                            }
                            if (!string.IsNullOrEmpty(userName))
                            {
                                MembershipUser user = Membership.GetUser(userName);

                                if (user != null)

                                {
                                    HttpCookie cookie =
                                    FormsAuthentication.GetAuthCookie(userName, false);

                                    FormsAuthenticationTicket ft =
                                            FormsAuthentication.Decrypt(cookie.Value);

                                    //Cutom user data
                                    string userData = userName;
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
                                        Note = "Đăng nhập bằng email: " + email,
                                        Type = 1

                                    };
                                    //Ghi log
                                    Action<ContentLog> send = InsertContentLog;
                                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                                    Response.Redirect(State);
                                }
                            }
                            
                            else
                            {
                                if (email.Contains("vuit.org.vn") )
                                {
                                    userName = email.Replace("vuit.org.vn@", "");
                                    HttpCookie cookie =
                                   FormsAuthentication.GetAuthCookie(userName, false);

                                    FormsAuthenticationTicket ft =
                                            FormsAuthentication.Decrypt(cookie.Value);

                                    //Cutom user data
                                    string userData = userName;
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
                                    Response.Redirect(State);
                                }
                            }


                            return;

                        }
                        return;

                    }
                    return;
                }

                GoogleAuth();

            }
        }
        protected void GoogleAuth()
        {
            UrlLogin = GoogleApiService.GetUrlLogin(RedirectUri, HttpUtility.UrlEncode(State));

            Response.Redirect(UrlLogin);
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}