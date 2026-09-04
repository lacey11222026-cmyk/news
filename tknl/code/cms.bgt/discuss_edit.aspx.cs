using System;
using System.Collections.Generic;
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
using Telerik.Web.UI;
using cms.libs;

namespace CMS
{


    public partial class discuss_edit : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        protected string mediaUrl = Config.mediaUrl + "resources/swf/flv/flvplayer.swf";
        protected long ReaderDiscussId = 0;
        private int TaskID = 0;
        private int StatusCurrent = 0;
        private WorkflowTask m_WorkflowTask;
        ReaderDiscuss m_ReaderDiscuss;
        private UserTask m_userTask;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.CheckSysFunction(6))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (Request.QueryString["TaskId"] != null)
            {
                TaskID = int.Parse(Request.QueryString["TaskId"]);
            }
            else
            {
                Response.Redirect("~/discuss.aspx", true);
            }
            if (Request.QueryString["ReaderDiscussId"] != null)
            {
                ReaderDiscussId = long.Parse(Request.QueryString["ReaderDiscussId"]);
            }
            else
            {
                Response.Redirect("~/discuss/" + TaskID + "/pending/index.htm", true);
            }
            DialogDefinition insertGroupboxDialogDefinition =
                new DialogDefinition(UrlRoot + "controls/insertgroupbox.ascx", new DialogParameters());
            insertGroupboxDialogDefinition.Modal = true;
            insertGroupboxDialogDefinition.VisibleTitlebar = true;
            insertGroupboxDialogDefinition.VisibleStatusbar = true;
            insertGroupboxDialogDefinition.Width = Unit.Pixel(680);
            insertGroupboxDialogDefinition.Height = Unit.Pixel(550);
            insertGroupboxDialogDefinition.VisibleStatusbar = false;
            insertGroupboxDialogDefinition.ReloadOnShow = true;
            insertGroupboxDialogDefinition.Title = "Insert Groupbox";
            RadContent.AddDialogDefinition("CustomInsertGroupbox", insertGroupboxDialogDefinition);

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
                linkTag.Text += "   .reToolbar.Default .CustomInsertGroupbox\n";
                linkTag.Text += "   {\n";
                linkTag.Text += "       background-image: url(   '" + UrlRoot + "icons/InsertGroupbox.gif' );\n";
                linkTag.Text += "   }\n";
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
                ddlDiscuss.DataSource = new Discuss().GetAll(1);
                ddlDiscuss.DataBind();                

                 m_ReaderDiscuss = new ReaderDiscuss { Id = ReaderDiscussId }.Get();
                if (m_ReaderDiscuss == null)
                {
                    Response.Redirect("~/discuss/" + TaskID + "/pending/index.htm", true);
                }
                ltlReaderInfo.Text = m_ReaderDiscuss.FullName + ", " + (m_ReaderDiscuss.Gender == 1 ? "Nam - " : "Nữ - ") + m_ReaderDiscuss.Age;
                txtQuestion.Text = m_ReaderDiscuss.Question;
                RadContent.Content = m_ReaderDiscuss.Answer;
                hfCurrStatus.Value = m_ReaderDiscuss.Status.ToString();
                try
                {
                    ddlDiscuss.SelectedValue = m_ReaderDiscuss.DiscussId.ToString();
                }
                catch (Exception ex) { }
                int DiscussId = int.Parse(ddlDiscuss.SelectedValue);
                List<Guest> lstGuest = new Guest() { DiscussId = DiscussId }.GetByDiscussId();

                dllGuest.DataSource = lstGuest;
                dllGuest.DataTextField = "FullName";
                dllGuest.DataValueField = "GuestId";
                dllGuest.DataBind();
                UsersDiscuss m_u_discuss;
                if (!m_UserValidation.CheckSysFunction(1))
                {
                    m_u_discuss = new UsersDiscuss() { UserId = m_UserValidation.LoginID }.Get();
                    ddlDiscuss.SelectedValue = m_u_discuss.DisscussId.ToString();
                    ddlDiscuss.Enabled = false;
                    if (m_u_discuss.GuestId > 0)
                    {
                        if (lstGuest.Find(m => m.GuestId == m_u_discuss.GuestId) != null)
                        {
                            dllGuest.SelectedValue = m_u_discuss.GuestId.ToString();
                            dllGuest.Enabled = false;
                        }
                    }
                }
                

                WorkflowTask m_WorkflowTask = new WorkflowTask { Id = TaskID }.Get();
                if (m_WorkflowTask.MoveUp < Constants.MaxStatus)
                {
                    btnUpdateAndMoveUp.Text = "Gửi lên";
                }
                else
                {
                    btnUpdateAndMoveUp.Text = "Xuất bản";
                }
                if (m_WorkflowTask.MoveDown >= 0)
                {
                    btnMoveDown.Text = "Trả lại";
                }
                else
                {
                    btnMoveDown.Enabled = false;
                }

                m_userTask = new UserTask().GetByUserIDMaxStatus(m_UserValidation.LoginID);
                if (m_userTask.TaskID >= 2)
                    Button1.Visible = true;
                else
                    Button1.Visible = false;
            }
        }
        private void UpdateData(int Status)
        {
            new DiscussDetail
            {
                Id = ReaderDiscussId,
                Question = txtQuestion.Text,
                DiscussId = int.Parse(ddlDiscuss.SelectedValue),
                GuestId = int.Parse(dllGuest.SelectedValue),
                Answer = RadContent.Content,
                Status = Status
            }.Update();

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                m_WorkflowTask = new WorkflowTask();
                m_WorkflowTask.Id = TaskID;
                m_WorkflowTask = m_WorkflowTask.Get();
                m_userTask = new UserTask().GetByUserIDMaxStatus(m_UserValidation.LoginID);
                if (int.Parse(hfCurrStatus.Value) == 4 && m_userTask.TaskID >= 3)
                    UpdateData(int.Parse(hfCurrStatus.Value));
                else
                    UpdateData(m_WorkflowTask.Status);
                Response.Redirect("~/discuss/" + TaskID + "/pending/index.htm", true);
            }
            catch (Exception ex)
            {
                ltlError.Text = "Có lỗi xảy ra trong quá trình cập nhật thông tin: " + ex.ToString();
            }

        }
        protected void btnUpdateAndMoveUpMax_Click(object sender, EventArgs e)
        {
            try
            {
                m_userTask = new UserTask().GetByUserIDMaxStatus(m_UserValidation.LoginID);
                if (m_userTask.TaskID >= 3)
                    m_userTask.TaskID = 4;
                UpdateData(m_userTask.TaskID);
                Response.Redirect("~/discuss/" + TaskID + "/pending/index.htm", true);
            }
            catch (Exception ex)
            {
                ltlError.Text = "Có lỗi xảy ra trong quá trình cập nhật thông tin: " + ex.ToString();
            }
        }
        protected void btnUpdateAndMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                m_WorkflowTask = new WorkflowTask { Id = TaskID }.Get();
                UpdateData(m_WorkflowTask.MoveUp);
                Response.Redirect("~/discuss/" + TaskID + "/pending/index.htm", true);
            }
            catch (Exception ex)
            {
                ltlError.Text = "Có lỗi xảy ra trong quá trình cập nhật thông tin: " + ex.ToString();
            }
        }
        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                m_WorkflowTask = new WorkflowTask { Id = TaskID }.Get();

                UpdateData(m_WorkflowTask.MoveDown);
                Response.Redirect("~/discuss/" + TaskID + "/pending/index.htm", true);
            }
            catch (Exception ex)
            {
                ltlError.Text = "Có lỗi xảy ra trong quá trình cập nhật thông tin: " + ex.ToString();
            }
        }
        protected void ddlDiscuss_SelectedIndexChanged(object sender, EventArgs e)
        {
            int DiscussId = int.Parse(ddlDiscuss.SelectedValue);
            List<Guest> lstGuest = new Guest() { DiscussId = DiscussId }.GetByDiscussId();

            dllGuest.DataSource = lstGuest;
            dllGuest.DataTextField = "FullName";
            dllGuest.DataValueField = "GuestId";
            dllGuest.DataBind();
        }

        protected void ddlDiscuss_DataBound(object sender, EventArgs e)
        {
            
        }
    }
}