using System;
using System.Collections.Generic;
using Telerik.Web.UI;
using cms.libs;


namespace CMS
{
    public partial class controls_workflow_menu : System.Web.UI.UserControl
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private RadPanelItem item;
        private UserValidation m_UserValidation = new UserValidation();
        private int TaskID;
        private string _AppCode = "article";
        bool bExpanded = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["TaskId"] != null)
                {
                    TaskID = int.Parse(Request.QueryString["TaskId"]);
                }
                ddlPart.Attributes.Add("style", "width:100%");
                ddlPart.DataSource = new Parts().GetForUserID(m_UserValidation.LoginID);
                ddlPart.DataBind();
                try
                {
                    ddlPart.SelectedValue = m_UserValidation.PartID.ToString();
                }
                catch { }
                List<WorkflowTask> l_WorkflowTask = new WorkflowTask().GetByUserIdAndPartId(m_UserValidation.LoginID, m_UserValidation.PartID);

                if (l_WorkflowTask != null && l_WorkflowTask.Count > 0)
                {
                    foreach (WorkflowTask m_Info in l_WorkflowTask)
                    {
                        item = new RadPanelItem();
                        item.Text = m_Info.Name;
                        string RewriteUrl = m_Info.RewriteUrl;
                        item.Value = RewriteUrl;
                        if (!bExpanded && m_Info.Id == TaskID)
                        {
                            item.Expanded = true;
                            bExpanded = true;
                        }
                        if (m_Info.AddnewEnabled)
                        {
                            item.Items.Add(new RadPanelItem("Viết bài", "~/" + _AppCode + "/" + m_Info.Id.ToString() + "/add/index.htm"));
                        }

                        if (m_Info.Status > -1)
                        {
                            item.Items.Add(new RadPanelItem("Danh sách chờ " + m_Info.Description.ToLower() + "", "~/" + _AppCode + "/" + m_Info.Id.ToString() + "/pending/index.htm"));
                            item.Items.Add(new RadPanelItem("Danh sách đã " + m_Info.Description.ToLower() + "", "~/" + _AppCode + "/" + m_Info.Id.ToString() + "/passed/index.htm"));
                        }
                        if (m_Info.Id == 1)
                            item.Items.Add(new RadPanelItem("Danh sách đã xóa", "~/article/" + m_Info.Id.ToString() + "/delete/index.htm"));

                        PanelMenu.Items.Add(item);
                    }
                    if (l_WorkflowTask[l_WorkflowTask.Count - 1].MoveUp == Constants.MaxStatus)
                    {
                        string sCurr = Request.Url.AbsoluteUri;
                        item = new RadPanelItem();
                        if (!bExpanded && sCurr.IndexOf("/comment/") >= 0)
                        {
                            item.Expanded = true;
                            bExpanded = true;
                        }
                        item.Text = "Quản lý bình luận";
                        item.Items.Add(new RadPanelItem("Danh sách mới nhất", "~/article/comment/pending/index.htm"));
                        item.Items.Add(new RadPanelItem("Danh sách chờ duyệt", "~/article/comment/pending_group/index.htm"));
                        item.Items.Add(new RadPanelItem("Danh sách đã duyệt", "~/article/comment/passed/index.htm"));
                        PanelMenu.Items.Add(item);

                        //if (m_UserValidation.PartID == 25)
                        //{
                        //string sCurr = Request.Url.AbsoluteUri;

                        //}
                    }
                    /*string sCurr_ = Request.Url.AbsoluteUri;
                    item = new RadPanelItem();
                    if (!bExpanded && sCurr_.IndexOf("/delete/") >= 0)
                    {
                        item.Expanded = true;
                        bExpanded = true;
                    }
                    item.Text = " Những bài đã xóa";
                    item.Items.Add(new RadPanelItem("Danh sách đã xóa", "~/article/delete/index.htm"));
                    PanelMenu.Items.Add(item);*/
                }

            }
        }

        protected void ddlPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PartID = int.Parse(ddlPart.SelectedValue);
            m_UserValidation.PartID = PartID;
            m_UserValidation.ProcessID = Convert.ToInt32(new Parts().GetInfo(PartID)["WorkflowID"]);
        }
    }
}