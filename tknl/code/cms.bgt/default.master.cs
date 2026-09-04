using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using cms.libs;

namespace CMS
{
    public partial class master_default : System.Web.UI.MasterPage
    {
        private const int _itemW = 100;
        public string UrlRoot = Constants.ROOT_PATH;
        private static UserValidation m_UserValidation = new UserValidation();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "..:Content Management System:..";
            Literal linkTag = new Literal();
            linkTag.Text = string.Format(@"<link href=""{0}css/layout.css"" rel=""stylesheet"" type=""text/css"" />
        <link href=""{0}css/css.css"" rel=""stylesheet"" type=""text/css"" />
        <link href=""{0}css/style_repeater.css"" rel=""stylesheet"" type=""text/css"" />
        <link href=""{0}css/paper.css"" rel=""stylesheet"" type=""text/css"" />
        ", UrlRoot);
            this.Page.Header.Controls.Add(linkTag);

            if (!this.IsPostBack)
            {
                this.Page.DataBind();
                if (m_UserValidation.IsSigned())
                {
                    ltlUserID.Text = m_UserValidation.LoginName;
                    hlChangePwd.InnerText = "Đổi mật khẩu";
                    hlChangePwd.HRef = "~/system/user/changepwd.htm";
                    hlSignOut.InnerText = "Thoát";
                    hlSignOut.HRef = "~/login.aspx?act=out";
                }
                else
                {
                    //Response.Redirect(ConfigurationManager.AppSettings["CMS_URL"]+"login.aspx?url="+Server.UrlEncode(Request.RawUrl), true)
                   string sUrl = Server.UrlEncode(ConfigurationManager.AppSettings["CMS_URL"] + Request.CurrentExecutionFilePath);//neu url la sub domain
                   // string sUrl = Server.UrlEncode(ConfigurationManager.AppSettings["SITE_URL"] + Request.CurrentExecutionFilePath);//neu url ko la subdomain
                    
                   Response.Redirect(UrlRoot + "login.aspx?url=" + sUrl, true);
                }
                Languages lang = new Languages(Server.MapPath(UrlRoot + "xmls/vn.xml"));
                DataTable dt = lang.GetPage("MENU_ADMIN_HEADER", "priority ASC");

                if (dt.Rows.Count == 0)
                    return;
                string abc = Request.CurrentExecutionFilePath;
                abc = abc.Replace(UrlRoot, string.Empty);
                string sCurr = Request.Url.AbsoluteUri;
                string sHtml = string.Empty;
                int i = 0;
                bool bActive = false;
                foreach (DataRow dr in dt.Rows)
                {
                    string caption = dr["caption"].ToString();
                    string link = Constants.ROOT_PATH + dr["url"].ToString();
                    if (sCurr.IndexOf(dr["key"].ToString()) > 0 && !bActive)
                    {
                        sHtml += "<td style=\"width: 4px; height: 20px\" class=\"spacerTab\" nowrap>&nbsp;</td>";
                        sHtml += "<td id='left" + i + "' style=\"width: 4px; height: 20px\" nowrap valign=\"top\" class=\"selTabLeft\">";
                        sHtml += "<img style=\"border:0px\" src=\"" + UrlRoot + "css/selectedTab_leftCorner.gif\" width=\"4\" height=\"3\" alt=\"\"></td>";
                        sHtml += "<td style=\"width:" + _itemW + "px; height: 20px;\" align=\"center\" nowrap valign=\"middle\" class=\"selTabCenter\" >" + caption + "</td>";
                        sHtml += "<td id='right" + i + "' style=\"width: 4px; height: 20px\" nowrap align=\"right\" valign=\"top\" nowrap class=\"selTabRight\">";
                        sHtml += "<img src=\"" + UrlRoot + "css/selectedTab_rightCorner.gif\" width=\"4\" height=\"3\" alt=\"\" style=\"border:0px\"></td>";
                        bActive = true;
                    }
                    else
                    {
                        sHtml += "<td style=\"width: 4px; height: 20px\" class=\"spacerTab\" nowrap>&nbsp;</td>";
                        sHtml += "<td id='left" + i + "' style=\"width: 4px; height: 20px\" nowrap valign=\"top\" class=\"deSTabLeft\">";
                        sHtml += "<img src=\"" + UrlRoot + "css/unSelectedTab_leftCorner.gif\" width=\"4\" height=\"3\" alt=\"\" style=\"border:0px\" /></td>";
                        sHtml += "<td style=\"width:" + _itemW + "px; height: 20px;cursor:hand;cursor:pointer;\" align=\"center\" nowrap valign=\"middle\" class=\"deSTabCenter\" onclick='window.location = \"" + link + "\"' onmouseover=\"this.className='hoverTabCenter';document.getElementById('left" + i + "').className='hoverTabLeft';document.getElementById('right" + i + "').className='hoverTabRight';\" onmouseout=\"this.className='deSTabCenter';document.getElementById('left" + i + "').className='deSTabLeft';document.getElementById('right" + i + "').className='deSTabRight';\" onkeypress=\"__keyPress(event, '" + link + "');\">" + caption + "</td>";
                        sHtml += "<td id='right" + i + "' style=\"width: 4px; height: 20px\" nowrap align=\"right\" valign=\"top\" class=\"deSTabRight\">";
                        sHtml += "<img src=\"" + UrlRoot + "css/unSelectedTab_rightCorner.gif\" style=\"width: 4px; height: 3px; border: 0px\" alt=\"\" /></td>";
                    }
                    i++;
                }
                ltlMenu.Text = sHtml;
            }
        }
    }
}