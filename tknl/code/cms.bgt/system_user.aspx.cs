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

    public partial class system_user : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private Parts m_Parts = null;
        private Users m_Users = null;
        private int _RecordPerPage = 20;
        private int _TotalPage = 10;
        private UserValidation m_UserValidation = new UserValidation();

        private int CurrPage
        {
            get
            {
                if (ViewState["CurrPage"] != null)
                {
                    return (int)ViewState["CurrPage"];
                }
                else
                {
                    return 1;
                }

            }
            set
            {
                ViewState["CurrPage"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.CheckSysFunction(1))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (!this.IsPostBack)
            {
                ddlPart.Attributes.Add("style", "width:280px");
                tbxKeyword.Attributes.Add("style", "width:280px");
                ddlStatus.Attributes.Add("style", "width:200px");
                m_Parts = new Parts();
                DataTable dtPart = m_Parts.GetAll();
                foreach (DataRow dr in dtPart.Rows)
                {
                    ddlPart.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
                }
                ddlPart.Items.Insert(0, new ListItem("Tất cả", "0"));

                BindData();
            }
        }
        protected void BindData()
        {
            string Keyword = tbxKeyword.Text;
            int PartID = Convert.ToInt32(ddlPart.SelectedValue);
            short Status = Convert.ToInt16(ddlStatus.SelectedValue);
            int _TotalRecord = 0;
            m_Users = new Users();
            rptUsers.DataSource = m_Users.GetPaged(Keyword, PartID, Status, CurrPage, _RecordPerPage, out _TotalRecord); ;
            rptUsers.DataBind();
            if (_TotalRecord > 0)
            {
                ltlTotal.Visible = dlPaper.Visible = dlPaper1.Visible = btnDelete.Visible = true;
                ltlTotal1.Text = ltlTotal.Text = "Tìm thấy " + _TotalRecord.ToString() + "  bản ghi.";
                _TotalPage = _TotalRecord / _RecordPerPage;
                if (_TotalRecord % _RecordPerPage > 0)
                    _TotalPage += 1;
                DataTable dtPaper = Paper.MakeDataPaper(_TotalPage, CurrPage, 5);
                dlPaper.DataSource = dtPaper;
                dlPaper.DataBind();
                dlPaper1.DataSource = dtPaper;
                dlPaper1.DataBind();
            }
            else
            {
                ltlTotal1.Text = "Tìm thấy 0  bản ghi.";
                ltlTotal.Visible = dlPaper.Visible = dlPaper1.Visible = btnDelete.Visible = false;

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrPage = 1;
            BindData();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem rptItem in rptUsers.Items)
            {
                CheckBox cbxSelect = (CheckBox)rptItem.FindControl("cbxSelect");
                if (cbxSelect.Checked)
                {
                    HiddenField hiddenID = (HiddenField)rptItem.FindControl("hiddenID");
                    try
                    {
                        m_Users = new Users();
                        m_Users.Delete(Int32.Parse(hiddenID.Value));
                    }
                    catch
                    { }
                }
            }
            BindData();
        }
        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                Literal ltlUserId = (Literal)e.Item.FindControl("ltlUserId");
                Literal ltlFullname = (Literal)e.Item.FindControl("ltlFullname");
                Literal ltlEmail = (Literal)e.Item.FindControl("ltlEmail");
                Literal ltlTelephone = (Literal)e.Item.FindControl("ltlTelephone");
                Literal ltlStopDateTime = (Literal)e.Item.FindControl("ltlStopDateTime");
                Literal ltlStatus = (Literal)e.Item.FindControl("ltlStatus");
                ImageButton iBtnStatus = (ImageButton)e.Item.FindControl("iBtnStatus");
                HyperLink hlEdit = (HyperLink)e.Item.FindControl("hlEdit");
                DataRowView item = (DataRowView)e.Item.DataItem;
                if (item != null)
                {
                    if (e.Item.ItemIndex % 2 == 1)
                        trItem.Attributes.Add("class", "alter");
                    else
                        trItem.Attributes.Add("class", "item");
                    cbxSelect.Attributes.Add("style", "cursor:pointer");

                    hiddenID.Value = item["ID"].ToString();
                    ltlUserId.Text = item["Name"].ToString();

                    ltlFullname.Text = item["FullName"].ToString();
                    ltlEmail.Text = item["Email"].ToString();
                    ltlTelephone.Text = item["Telephone"].ToString();
                    iBtnStatus.ImageUrl = (Convert.ToInt32(item["Status"]) == 0) ? UrlRoot + "icons/offline.gif" : UrlRoot + "icons/online.gif";
                    iBtnStatus.CommandArgument = item["ID"].ToString();

                    hlEdit.NavigateUrl = "~/system/user/" + item["ID"].ToString() + "/edit.htm";

                }
            }
        }
        protected void ddlPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrPage = 1;
            BindData();
        }
        protected void dlPaper_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                LinkButton lbtPage = (LinkButton)e.Item.FindControl("lbtPage");
                lbtPage.CausesValidation = false;
                lbtPage.Click += new System.EventHandler(lbtPageClick);
            }
        }
        protected void dlPaper_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                LinkButton lbtPage = (LinkButton)e.Item.FindControl("lbtPage");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                int pType = 0;
                if (dr != null)
                {
                    pType = Convert.ToInt32(dr["Type"]);
                    if (pType == 0)
                    {
                        lbtPage.Text = dr["Text"].ToString();
                        lbtPage.CssClass = dr["CssClass"].ToString();
                        lbtPage.CommandArgument = dr["Page"].ToString();
                    }
                    else
                    {
                        lbtPage.CommandArgument = dr["Page"].ToString();
                        Image img = new Image();
                        img.BorderWidth = Unit.Pixel(0);
                        img.CssClass = "icon_paper";
                        img.ImageUrl = "~/icons/" + dr["Text"].ToString();
                        lbtPage.Controls.Add(img);
                    }
                }
            }
        }
        protected void lbtPageClick(object sender, EventArgs e)
        {
            LinkButton lbt = (LinkButton)sender;
            if (lbt != null)
            {
                CurrPage = int.Parse(lbt.CommandArgument);
                BindData();
            }
        }
        protected void iBtnStatusClick(object sender, EventArgs e)
        {
            ImageButton iBtn = (ImageButton)sender;
            if (iBtn != null)
            {
                m_Users = new Users();
                m_Users.UpdateStatus(int.Parse(iBtn.CommandArgument));
                BindData();
            }
        }
        protected void rptUsers_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                ImageButton iBtnStatus = (ImageButton)e.Item.FindControl("iBtnStatus");
                iBtnStatus.CausesValidation = false;
                iBtnStatus.Click += new ImageClickEventHandler(iBtnStatusClick);
            }
        }
    }
}