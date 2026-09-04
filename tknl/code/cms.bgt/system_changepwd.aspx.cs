using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
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

    public partial class system_changepwd : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private int UserID;
        private Users m_Users = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UserID = int.Parse((Request.QueryString["UserId"] == null) ? "0" : Request.QueryString["UserId"]);
            }
            catch
            {
                UserID = 0;
            }
            if (!this.IsPostBack)
            {
                m_Users = new Users();
                txtUserName.Attributes.Add("style", "width:200px");
                txtUserName.Text = m_Users.GetInfo(UserID)["Name"].ToString();
                txtNewPassword.Attributes.Add("style", "width:200px");
                txtConfirmNewPassword.Attributes.Add("style", "width:200px");
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            m_Users = new Users();
            DataRow m_User_Info = m_Users.GetInfo(UserID);
            bool b = m_Users.ChangePwd(m_User_Info["Name"].ToString(), m_User_Info["Password"].ToString(), Encrypt.MD5(txtNewPassword.Text.Trim()));
            if (!b)
                ltlError.Text = "Lỗi trong quá trình đổi mật khẩu!";
            else
                ltlError.Text = "Bạn đã đổi mật khẩu thành công!";
        }
    }
}