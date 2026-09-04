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
    public partial class login : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        public string Url = string.Empty;
        private Users m_Users = null;
        private UserValidation m_UserValidation = new UserValidation();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["act"] != null && Request.QueryString["act"].ToString() == "out")
                {
                    m_UserValidation.SignOut();
                    Response.Redirect("~/default.aspx", true);
                }
            }
            if (Request.QueryString["url"] != null)
            {
                Url = Server.UrlDecode(Request.QueryString["url"].Trim()).ToLower();
            }
            else
                Url = UrlRoot + "workflow.aspx";

            if (!IsPostBack)
            {
                this.Page.DataBind();
                GenPassMatrix();
            }
        }

        private bool IsPasswordStrong(string password)
        {
            return Regex.IsMatch(password, @"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$");
        }

        private string GetPass()
        {
            var list = string.Empty;
            var random = new Random();
            for (var i = 1; i < 10; i++)
            {
                var j = random.Next(1, 9);

                list += string.Format("({0}:{1})", i, j) + "-";
            }
            return list.Substring(0, list.Length - 1);
        }

        private void GenPassMatrix()
        {
            Session["PassMatrix"] = GetPass();
        //    ltlPass.Text = Session["PassMatrix"] as string;
        }

        protected int getDateNumber()
        {
            string day = DateTime.Now.DayOfWeek.ToString();
            if (day == "Monday") return 1;
            if (day == "Tuesday") return 2;
            if (day == "Wednesday") return 3;
            if (day == "Thursday") return 4;
            if (day == "Friday") return 5;
            if (day == "Saturday") return 6;
            return 0;
        }

        private string GetSecurityCode(string code, string origion)
        {
            var value = string.Empty;
            var listmatrix = code.Split('|');//123456789
            var arr = origion.Split('-');//(1:2)
            var index = 0;
            foreach (var column in arr.Select(s => s.Substring(3, 1)))
            {
                value += listmatrix[index].Substring(int.Parse(column) - 1, 1);
                index++;
            }
            return value;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string UserName = txtUser.Value.Trim();
            if (UserName.Length == 0)
            {
                ltlError.Text = "Phải nhập tên truy cập!";
                return;
            }
            string PassWord = txtPass.Value.Trim();
            if (PassWord.Length == 0)
            {
                ltlError.Text = "Phải nhập mật khẩu!"; return;
            }

            //if (!IsPasswordStrong(PassWord))
            //{
            //    ltlError.Text = "Password must be 8 characters and have both letters and numbers."; return;
            //}

            m_Users = new Users();
            Users m_User_Info = m_Users.GetbyUserName(UserName);
            if (m_User_Info == null)
            {
                ltlError.Text = "Thông tin đăng nhập không đúng!";
                return;
            }
            if (m_User_Info.Password.ToString() != Encrypt.MD5(PassWord))
            {
                ltlError.Text = "Bạn nhập sai mật khẩu, vui lòng kiểm tra lại!";
                return;
            }
            if (Convert.ToInt32(m_User_Info.Status) == 0)
            {
                ltlError.Text = "Tài khoản của bạn đã bị khóa!";
                return;
            }
            //if (string.IsNullOrEmpty(m_User_Info.Passwordmatrix))
            //{
            //    ltlError.Text = "Tài khoản chưa được thiết lập ma trận mật khẩu!"; return;
            //}
            //if (Session["PassMatrix"] == null)
            //{
            //    ltlError.Text = "Chưa khởi tạo được mật khẩu!"; return;
            //}
           


           // long date = 0;
           // long.TryParse(m_User_Info.Passwordmatrix.Split('#')[0], out date);
          //  string matrix = m_User_Info.Passwordmatrix.Split('#')[1];
          //  int configDay = 90;

            if (m_User_Info.Status > 0)
            {
                //if (txtPwdMatrix.Text == GetSecurityCode(matrix, Session["PassMatrix"].ToString()))
                //{
                //    if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) - date > configDay)
                //    {
                //        ltlError.Text = "Mật khẩu ma trận đã hết thời hạn!";
                //        return;
                //    }
                //    new LOGs().Add(Convert.ToInt32(m_User_Info.ID), UserName + " đăng nhập vào hệ thống", Constants.getIP());

                //    m_UserValidation.Set(Convert.ToInt32(m_User_Info.ID), UserName);

                //    hfResult.Value = "1";
                //}
                //else
                //{
                //    ltlError.Text = "Sai mật khẩu ma trận!";
                //}
                new LOGs().Add(Convert.ToInt32(m_User_Info.ID), UserName + " đăng nhập vào hệ thống", Constants.getIP());

                m_UserValidation.Set(Convert.ToInt32(m_User_Info.ID), UserName);

                hfResult.Value = "1";
            }
            else
            {
                ltlError.Text = "Tài khoản đang bị khóa!";
            }
        }
    }
}