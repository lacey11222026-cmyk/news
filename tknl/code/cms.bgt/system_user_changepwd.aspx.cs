using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using cms.libs;

namespace CMS
{
    public partial class system_user_changepwd : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private Users m_Users = null;
        private UserValidation m_UserValidation = new UserValidation();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!m_UserValidation.CheckSysFunction(3))
            //{
            //    Response.Redirect(UrlRoot + "default.aspx", true);
            //}
            txtUserName.Attributes.Add("style", "width:230px");
            txtUserName.Text = m_UserValidation.LoginName;
            txtOldPassword.Attributes.Add("style", "width:230px");
            txtNewPassword.Attributes.Add("style", "width:230px");
            txtConfirmNewPassword.Attributes.Add("style", "width:230px");
        }

        private bool IsPasswordStrong(string password)
        {
            return Regex.IsMatch(password, @"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsPasswordStrong(txtNewPassword.Text.Trim()))
            {
                m_Users = new Users();
                bool b = m_Users.ChangePwd(m_UserValidation.LoginName, Encrypt.MD5(txtOldPassword.Text.Trim()),
                                           Encrypt.MD5(txtNewPassword.Text.Trim()));
                if (!b)
                    ltlError.Text = "Mật khẩu cũ không đúng!";
                else
                    ltlError.Text = "Bạn đã đổi mật khẩu thành công!";
            }
            else
                ltlError.Text = "Password must be 8 characters and have both letters and numbers.";
        }
    }
}