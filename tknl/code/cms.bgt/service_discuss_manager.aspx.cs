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

    public partial class service_discuss_manager : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.CheckSysFunction(3))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (!this.IsPostBack)
            {
                BindData();
            }
        }
        protected void BindData()
        {
            rptData.DataSource = new Discuss().GetAll(-1);
            rptData.DataBind();
        }
        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                CheckBox cbxHeaderSelect = (CheckBox)e.Item.FindControl("cbxHeaderSelect");
                cbxHeaderSelect.Attributes.Add("style", "cursor:pointer");
                cbxHeaderSelect.Attributes.Add("onclick", "SelectAll('" + cbxHeaderSelect.ClientID + "', 'cbxSelect')");
            }
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                HtmlTableRow trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                CheckBox cbxSelect = (CheckBox)e.Item.FindControl("cbxSelect");
                HiddenField hiddenID = (HiddenField)e.Item.FindControl("hiddenID");
                Literal ltlName = (Literal)e.Item.FindControl("ltlName");
                ImageButton iBtnStatus = (ImageButton)e.Item.FindControl("iBtnStatus");
                HyperLink hlEdit = (HyperLink)e.Item.FindControl("hlEdit");
                Discuss item = (Discuss)e.Item.DataItem;
                if (item != null)
                {
                    if (e.Item.ItemIndex % 2 == 1)
                        trItem.Attributes.Add("class", "alter");
                    else
                        trItem.Attributes.Add("class", "item");
                    cbxSelect.Attributes.Add("style", "cursor:pointer");

                    hiddenID.Value = item.DiscussId.ToString();
                    ltlName.Text = item.Title;
                    iBtnStatus.ImageUrl = item.Status == 0 ? UrlRoot + "icons/offline.gif" : UrlRoot + "icons/online.gif";
                    iBtnStatus.CommandArgument = item.DiscussId.ToString();
                    hlEdit.NavigateUrl = "~/discuss/edit/" + item.DiscussId.ToString() + ".htm";
                }
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem rptItem in rptData.Items)
            {
                CheckBox cbxSelect = (CheckBox)rptItem.FindControl("cbxSelect");
                if (cbxSelect.Checked)
                {
                    HiddenField hiddenID = (HiddenField)rptItem.FindControl("hiddenID");
                    new Discuss { DiscussId = int.Parse(hiddenID.Value) }.Delete();
                }
            }
            BindData();
        }
        protected void rptData_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                ImageButton iBtnStatus = (ImageButton)e.Item.FindControl("iBtnStatus");
                iBtnStatus.CausesValidation = false;
                iBtnStatus.Click += new ImageClickEventHandler(iBtnStatusClick);
            }
        }
        protected void iBtnStatusClick(object sender, EventArgs e)
        {
            ImageButton iBtn = (ImageButton)sender;
            if (iBtn != null)
            {
                new Discuss { DiscussId = int.Parse(iBtn.CommandArgument) }.UpdateStatus();
                BindData();
            }
        }
    }
}