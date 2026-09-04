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
using Telerik.Web.UI;

namespace CMS
{
    public partial class controls_Service_menu : System.Web.UI.UserControl
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private RadPanelItem item;
        private UserValidation m_UserValidation = new UserValidation();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Languages lang = new Languages(Server.MapPath(UrlRoot + "xmls/vn.xml"));
                DataTable dt = lang.GetPage("ADMIN_SERVICE_PAGES", "priority ASC");
                string sCurr = Request.Url.AbsoluteUri;
                DataTable dtChild = null;
                bool bExpanded = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (m_UserValidation.CheckSysFunction(Convert.ToInt32(dr["FuncID"])))
                    {
                        item = new RadPanelItem();
                        item.Text = dr["caption"].ToString();
                        item.Value = dr["key"].ToString();
                        if (sCurr.IndexOf(item.Value) > 0 && !bExpanded)
                        {
                            item.Expanded = true;
                            bExpanded = true;
                        }
                        dtChild = lang.GetPage("ADMIN_SERVICE_" + dr["key"].ToString().ToUpper() + "_PAGES", "priority ASC");
                        if (dtChild != null && dtChild.Rows.Count > 0)
                        {
                            string sCaption = "";
                            string sUrl = "";
                            foreach (DataRow drChild in dtChild.Rows)
                            {
                                sCaption = drChild["caption"].ToString();
                                if (drChild["url"].ToString() == "#")
                                {
                                    sUrl = Request.RawUrl + "#";
                                }
                                else if (drChild["url"].ToString().StartsWith("http://") || drChild["url"].ToString().StartsWith("https://"))
                                {
                                    sUrl = drChild["url"].ToString();
                                }
                                else
                                {
                                    sUrl = UrlRoot + drChild["url"].ToString();
                                }
                                item.Items.Add(new RadPanelItem(sCaption, sUrl));
                            }
                        }

                        PanelMenu.Items.Add(item);
                    }
                }
            }
        }
    }
}