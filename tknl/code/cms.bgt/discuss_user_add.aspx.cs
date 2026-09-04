using cms.libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS2012
{
    public partial class discuss_user_add : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        private DataTable dataAnswer
        {
            get
            {
                return (DataTable)ViewState[this.ClientID + "_dataAnswer"];
            }
            set
            {
                ViewState[this.ClientID + "_dataAnswer"] = value;
            }
        }
        protected string mediaUrl = Config.mediaUrl + "resources/swf/flv/flvplayer.swf";
        private DataView dataView
        {
            get
            {
                DataView source = new DataView(dataAnswer);
                return source;
            }
        }
        private DataRow FindRow(long ID)
        {
            DataRow[] rows = dataAnswer.Select(string.Format("ID = '{0}'", ID));
            if (rows.Length > 0)
                return rows[0];
            else
                return null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.CheckSysFunction(3))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            DialogDefinition insertImageDialogDefinition = new DialogDefinition(UrlRoot + "controls/imagemanager.ascx", new DialogParameters());
            insertImageDialogDefinition.Modal = true;
            insertImageDialogDefinition.VisibleTitlebar = true;
            insertImageDialogDefinition.VisibleStatusbar = true;
            insertImageDialogDefinition.Width = Unit.Pixel(680);
            insertImageDialogDefinition.Height = Unit.Pixel(550);
            insertImageDialogDefinition.VisibleStatusbar = false;
            insertImageDialogDefinition.ReloadOnShow = true;
            insertImageDialogDefinition.Title = "Nhập ảnh";
            RadContent.AddDialogDefinition("CustomImageManager", insertImageDialogDefinition);

            DialogDefinition insertFlashDialogDefinition = new DialogDefinition(UrlRoot + "controls/flashmanager.ascx", new DialogParameters());
            insertFlashDialogDefinition.Modal = true;
            insertFlashDialogDefinition.VisibleTitlebar = true;
            insertFlashDialogDefinition.VisibleStatusbar = true;
            insertFlashDialogDefinition.Width = Unit.Pixel(680);
            insertFlashDialogDefinition.Height = Unit.Pixel(510);
            insertFlashDialogDefinition.VisibleStatusbar = false;
            insertFlashDialogDefinition.ReloadOnShow = true;
            insertFlashDialogDefinition.Title = "Nhập Flash";
            RadContent.AddDialogDefinition("CustomFlashManager", insertFlashDialogDefinition);

            DialogDefinition insertMediaDialogDefinition = new DialogDefinition(UrlRoot + "controls/mediamanager.ascx", new DialogParameters());
            insertMediaDialogDefinition.Modal = true;
            insertMediaDialogDefinition.VisibleTitlebar = true;
            insertMediaDialogDefinition.VisibleStatusbar = true;
            insertMediaDialogDefinition.Width = Unit.Pixel(680);
            insertMediaDialogDefinition.Height = Unit.Pixel(510);
            insertMediaDialogDefinition.VisibleStatusbar = false;
            insertMediaDialogDefinition.ReloadOnShow = true;
            insertMediaDialogDefinition.Title = "Nhập Clip";
            RadContent.AddDialogDefinition("CustomMediaManager", insertMediaDialogDefinition);
            if (!this.IsPostBack)
            {
                Literal linkTag = new Literal();
                linkTag.Text = "<style type=\"text/css\">\n";
                linkTag.Text += "   .reToolbar.Default .CustomImageManager\n";
                linkTag.Text += "   {\n";
                linkTag.Text += "       background-image: url(   '" + UrlRoot + "icons/ImageManager.gif' );\n";
                linkTag.Text += "   }\n";
                linkTag.Text += "   .reToolbar.Default .CustomMediaManager\n";
                linkTag.Text += "  {\n";
                linkTag.Text += "      background-image: url(   '" + UrlRoot + "icons/MediaManager.gif' );\n";
                linkTag.Text += "  }\n";
                linkTag.Text += "  .reToolbar.Default .CustomFlashManager\n";
                linkTag.Text += "  {\n";
                linkTag.Text += "       background-image: url(   '" + UrlRoot + "icons/FlashManager.gif' );\n";
                linkTag.Text += "   }\n";
                linkTag.Text += "</style>\n";
                Page.Header.Controls.Add(linkTag);
                dataAnswer = new DataTable();
                dataAnswer.Columns.Add("ID", typeof(int));
                dataAnswer.Columns.Add("FullName", typeof(string));
                dataAnswer.Columns.Add("Gender", typeof(string));
                dataAnswer.Columns.Add("Job", typeof(string));
                dataAnswer.Columns["ID"].AutoIncrement = true;
                dataAnswer.Columns["ID"].AutoIncrementSeed = 1;
                dataAnswer.Columns["ID"].AutoIncrementStep = 1;
            }
        }
        protected void BindSelected()
        {
            if (dataView != null)
            {
                int n = dataView.Count;
                rptGuest.DataSource = dataView;
                rptGuest.DataBind();
            }
        }
        protected void rptGuest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                CheckBox cbxHeaderRemove = (CheckBox)e.Item.FindControl("cbxHeaderRemove");
                cbxHeaderRemove.Attributes.Add("style", "cursor:pointer");
                cbxHeaderRemove.Attributes.Add("onclick", "SelectAll('" + cbxHeaderRemove.ClientID + "', 'fieldsetSelected')");
            }
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                HtmlTableRow trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                CheckBox cbxRemove = (CheckBox)e.Item.FindControl("cbxRemove");
                HiddenField hiddenID = (HiddenField)e.Item.FindControl("hiddenID");
                Literal ltlOrderNo = (Literal)e.Item.FindControl("ltlOrderNo");
                TextBox txtFullName = (TextBox)e.Item.FindControl("txtFullName");
                DropDownList ddlGender = (DropDownList)e.Item.FindControl("ddlGender");
                TextBox txtJob = (TextBox)e.Item.FindControl("txtJob");
                DataRowView item = (DataRowView)e.Item.DataItem;
                if (item != null)
                {
                    if (e.Item.ItemIndex % 2 == 1)
                        trItem.Attributes.Add("class", "alter");
                    else
                        trItem.Attributes.Add("class", "item");
                    cbxRemove.Attributes.Add("style", "cursor:pointer");

                    hiddenID.Value = item["ID"].ToString();
                    ltlOrderNo.Text = (e.Item.ItemIndex + 1).ToString();
                    txtFullName.Text = item["FullName"].ToString();
                    try
                    {
                        ddlGender.SelectedValue = item["Gender"].ToString();
                    }
                    catch
                    { }
                    txtJob.Text = item["Job"].ToString();
                }
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            long ID;
            foreach (RepeaterItem rptItem in rptGuest.Items)
            {
                CheckBox cbxRemove = (CheckBox)rptItem.FindControl("cbxRemove");
                if (cbxRemove.Checked)
                {
                    HiddenField hiddenID = (HiddenField)rptItem.FindControl("hiddenID");
                    ID = long.Parse(hiddenID.Value);
                    DataRow dr = FindRow(ID);
                    if (dr != null)
                    {
                        dataAnswer.Rows.Remove(dr);
                    }
                }
            }
            BindSelected();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Discuss m_Discuss = new Discuss
            {
                Title = txtTitle.Text,
                TopicDiscussion = RadContent.Content.Trim(),
                DateCreate = DateTime.Now,
                SiteId = 1,
                Type = 0,
                Status = 1
            };
            int DiscussId = m_Discuss.Insert();
            Guest m_Guest;
            foreach (RepeaterItem rptItem in rptGuest.Items)
            {
                TextBox txtFullName = (TextBox)rptItem.FindControl("txtFullName");
                DropDownList ddlGender = (DropDownList)rptItem.FindControl("ddlGender");
                TextBox txtJob = (TextBox)rptItem.FindControl("txtJob");
                m_Guest = new Guest
                {
                    FullName = txtFullName.Text,
                    Gender = int.Parse(ddlGender.SelectedValue),
                    Job = txtJob.Text,
                    UserId = m_UserValidation.LoginID,
                    DiscussId = DiscussId
                };
                m_Guest.Insert();
            }
            hfResult.Value = "1";
        }
        protected void btnGuestAdd_Click(object sender, EventArgs e)
        {
            DataRow newRow = dataAnswer.NewRow();
            newRow["FullName"] = txtFullName.Text;
            newRow["Gender"] = ddlGender.SelectedValue;
            newRow["Job"] = txtJob.Text;
            dataAnswer.Rows.Add(newRow);
            BindSelected();
        }
    }
}