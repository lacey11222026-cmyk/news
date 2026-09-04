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
    public partial class controls_discuss_menu : System.Web.UI.UserControl
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private RadPanelItem item;
        private UserValidation m_UserValidation = new UserValidation();
        private int TaskID = 0;
        private string _AppCode = "discuss";
        bool bExpanded = false;
        private int PartId = 26;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["TaskId"] != null)
                {
                    TaskID = int.Parse(Request.QueryString["TaskId"]);
                }
                List<WorkflowTask> l_WorkflowTask = new WorkflowTask().GetByUserIdAndPartId(m_UserValidation.LoginID, PartId);

                if (l_WorkflowTask != null && l_WorkflowTask.Count > 0)
                {
                    if (m_UserValidation.CheckSysFunction(1))
                    {
                        item = new RadPanelItem();
                        item.Text = "Thông tin giao lưu";
                        item.Items.Add(new RadPanelItem("Thêm mới giao lưu", "~/discuss/add.htm"));
                        item.Items.Add(new RadPanelItem("Thông tin giao lưu trực tuyến", "~/discuss/index.htm"));

                        string sCurr = Request.Url.AbsoluteUri;
                        if (!bExpanded && (sCurr.IndexOf("/discuss/add") >= 0 || sCurr.IndexOf("/discuss/index") >= 0 || sCurr.IndexOf("/discuss/edit") >= 0))
                        {
                            item.Expanded = true;
                            bExpanded = true;
                        }

                        PanelMenu.Items.Add(item);
                    }
                    //if (m_UserValidation.CheckSysFunction(6))
                    //{
                    item = new RadPanelItem();
                    item.Text = "Câu hỏi giao lưu";

                    foreach (WorkflowTask m_Info in l_WorkflowTask)
                    {
                        /*item = new RadPanelItem();
                        item.Text = m_Info.Name;
                        string RewriteUrl = m_Info.RewriteUrl;
                        item.Value = RewriteUrl;
                        */
                        if (!bExpanded && m_Info.Id == TaskID)
                        {
                            item.Expanded = true;
                            bExpanded = true;
                        }
                        if (m_Info.AddnewEnabled)
                        {
                            item.Items.Add(new RadPanelItem("Câu hỏi mới",
                                                            "~/" + _AppCode + "/" + m_Info.Id.ToString() +
                                                            "/add/index.htm"));
                        }

                        if (m_Info.Status > -1)
                        {
                            if (l_WorkflowTask.Count >= 2)
                            {
                                item.Items.Add(new RadPanelItem("Danh sách chờ " + m_Info.Description.ToLower() + "",
                                                                "~/" + _AppCode + "/" + m_Info.Id.ToString() +
                                                                "/pending/index.htm"));
                                if (m_Info.Id == 3)
                                {
                                    item.Items.Add(new RadPanelItem("Danh sách đã " + m_Info.Description.ToLower() + "",
                                                                    "~/" + _AppCode + "/" + m_Info.Id.ToString() +
                                                                    "/passed/index.htm"));
                                }
                                if (m_Info.Id == 2 && l_WorkflowTask.Count == 2)
                                {
                                    item.Items.Add(new RadPanelItem("Danh sách đã " + m_Info.Description.ToLower() + "",
                                                                    "~/" + _AppCode + "/" + m_Info.Id.ToString() +
                                                                    "/passed/index.htm"));
                                }
                            }
                            else
                            {
                                item.Items.Add(new RadPanelItem("Danh sách chờ " + m_Info.Description.ToLower() + "",
                                                                "~/" + _AppCode + "/" + m_Info.Id.ToString() +
                                                                "/pending/index.htm"));
                                item.Items.Add(new RadPanelItem("Danh sách đã " + m_Info.Description.ToLower() + "",
                                                                "~/" + _AppCode + "/" + m_Info.Id.ToString() +
                                                                "/passed/index.htm"));
                            }
                        }

                    }
                    PanelMenu.Items.Add(item);
                    // }
                }
            }
        }
    }
}