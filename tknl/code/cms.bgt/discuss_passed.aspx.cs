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
using cms.libs;
using System.Globalization;
using System.Data.SqlTypes;

namespace CMS
{


    public partial class discuss_passed : System.Web.UI.Page
    {
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
        public string UrlRoot = Constants.ROOT_PATH;
        private ReaderDiscuss m_ReaderDiscuss;
        private int DiscussId = 0;
        private int GuestId = 0;
        private int Status = -1;
        private DateTime FromDate = (DateTime)SqlDateTime.MinValue;
        private DateTime ToDate = (DateTime)SqlDateTime.MaxValue;
        private int RecordPerPage = 30;
        private WorkflowTask m_Workflows;
        private int TaskID = 0;
        private UserValidation m_UserValidation = new UserValidation();
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
            //if (TaskID == 0 || !m_UserValidation.CheckTask(TaskID))
            //{
            //    Response.Redirect(UrlRoot + "discuss.aspx", true);
            //}

            if (!this.IsPostBack)
            {

                DateTime CrDate = DateTime.Now;
                txtDateFrom.SelectedDate = CrDate.AddDays(-15);
                txtDateTo.SelectedDate = CrDate;
                List<Discuss> l_Discuss = new Discuss().GetAll(1);
                l_Discuss.Insert(0, new Discuss() { Title = "Toàn bộ", DiscussId = 0 });
                if (l_Discuss != null && l_Discuss.Count > 0)
                {
                    ddlDiscuss.DataSource = l_Discuss;
                    ddlDiscuss.DataBind();

                    DiscussId = int.Parse(ddlDiscuss.SelectedValue);
                    List<Guest> lstGuest = new Guest() { DiscussId = DiscussId }.GetByDiscussId();
                    lstGuest.Insert(0, new Guest() { FullName = "Toàn bộ", GuestId = 0 });
                    ddlGuest.DataSource = lstGuest;
                    ddlGuest.DataTextField = "FullName";
                    ddlGuest.DataValueField = "GuestId";
                    ddlGuest.DataBind();


                    UsersDiscuss m_u_discuss = null;
                    if (m_UserValidation.CheckSysFunction(1))
                    {
                        /*ddlDiscuss.Items.Insert(0, new ListItem(" Toàn bộ ", "0"));
                        ddlDiscuss.SelectedIndex = 0;
                        ddlGuest.Items.Insert(0, new ListItem(" Toàn bộ ", "0"));
                        ddlGuest.SelectedIndex = 0;*/
                    }
                    else
                    {
                        m_u_discuss = new UsersDiscuss() { UserId = m_UserValidation.LoginID }.Get();
                        ddlDiscuss.SelectedValue = m_u_discuss.DisscussId.ToString();
                        DiscussId = int.Parse(ddlDiscuss.SelectedValue);
                        lstGuest = new Guest() { DiscussId = DiscussId }.GetByDiscussId();
                        ddlGuest.DataSource = lstGuest;
                        ddlGuest.DataTextField = "FullName";
                        ddlGuest.DataValueField = "GuestId";
                        ddlGuest.DataBind();
                        ddlDiscuss.Enabled = false;
                        if (lstGuest.Find(m => m.GuestId == m_u_discuss.GuestId) != null)
                        {
                            ddlGuest.SelectedValue = m_u_discuss.GuestId.ToString();
                            ddlGuest.Enabled = false;
                        }
                    }
                    UserTask m_userTask = new UserTask().GetByUserIDMaxStatus(m_UserValidation.LoginID);
                    if (m_userTask.TaskID == 1)
                    {
                        ddlDiscuss.Enabled = false;
                        ddlGuest.Enabled = false;
                    }
                    BindData();
                }
            }
        }
        protected void BindData()
        {
            m_Workflows = new WorkflowTask();
            m_Workflows.Id = TaskID;
            m_Workflows = m_Workflows.Get();
            LTL_HEADER.Text = "Danh sách đã " + m_Workflows.Description.ToLower();
            Status = m_Workflows.MoveUp;
            DiscussId = int.Parse(ddlDiscuss.SelectedValue);
            GuestId = int.Parse(ddlGuest.SelectedValue);
            if (txtDateFrom.SelectedDate.Value != null)
            {
                FromDate = txtDateFrom.SelectedDate.Value;
            }
            if (txtDateTo.SelectedDate.Value != null)
            {
                ToDate = txtDateTo.SelectedDate.Value.AddDays(1);
            }
            int TotalRecord = 0;
            m_ReaderDiscuss = new ReaderDiscuss();
            rptData.DataSource = m_ReaderDiscuss.GetPagedPassed(DiscussId, GuestId,Status, FromDate, ToDate, CurrPage, RecordPerPage, out TotalRecord);
            rptData.DataBind();
            if (TotalRecord > 0)
            {
                ltlTotal.Visible = dlPaper.Visible = dlPaper1.Visible = true;
                ltlTotal1.Text = ltlTotal.Text = "Tìm thấy " + TotalRecord.ToString() + "  bản ghi.";
                var TotalPage = TotalRecord / RecordPerPage;
                if (TotalRecord % RecordPerPage > 0)
                    TotalPage += 1;
                DataTable dtPaper = Paper.MakeDataPaper(TotalPage, CurrPage, 5);
                dlPaper.DataSource = dtPaper;
                dlPaper.DataBind();
                dlPaper1.DataSource = dtPaper;
                dlPaper1.DataBind();
            }
            else
            {
                ltlTotal1.Text = "Tìm thấy 0  bản ghi.";
                ltlTotal.Visible = dlPaper.Visible = dlPaper1.Visible = false;

            }
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                var cbxHeaderSelect = (CheckBox)e.Item.FindControl("cbxHeaderSelect");
                cbxHeaderSelect.Attributes.Add("style", "cursor:pointer");
                cbxHeaderSelect.Attributes.Add("onclick", "SelectAll('" + cbxHeaderSelect.ClientID + "', 'cbxSelect')");
            }
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                var trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                CheckBox cbxSelect = (CheckBox)e.Item.FindControl("cbxSelect");
                HiddenField hiddenID = (HiddenField)e.Item.FindControl("hiddenID");
                Literal ltlQuestion = (Literal)e.Item.FindControl("ltlQuestion");
                Literal ltlAnswer = (Literal)e.Item.FindControl("ltlAnswer");
                ImageButton iBtnMoveUp = (ImageButton)e.Item.FindControl("iBtnMoveUp");
                ImageButton iBtnMoveDown = (ImageButton)e.Item.FindControl("iBtnMoveDown");
                ImageButton iBtnDelete = (ImageButton)e.Item.FindControl("iBtnDelete");
                HyperLink hlEdit = (HyperLink)e.Item.FindControl("hlEdit");
                LinkButton hlUpdate = (LinkButton)e.Item.FindControl("hlUpdate");
                
                ReaderDiscuss item = (ReaderDiscuss)e.Item.DataItem;
                if (item != null)
                {
                    if (e.Item.ItemIndex % 2 == 1)
                        trItem.Attributes.Add("class", "alter");
                    else
                        trItem.Attributes.Add("class", "item");
                    cbxSelect.Attributes.Add("style", "cursor:pointer");

                    hiddenID.Value = item.Id.ToString();
                    ltlQuestion.Text = "(" + item.FullName + ", " + (item.Gender == 1 ? "Nam - " : "Nữ - ") + item.Age + "): " + item.Question;
                    ltlAnswer.Text = item.Answer;

                    iBtnMoveUp.Enabled = false;
                    iBtnMoveUp.ImageUrl = UrlRoot + "icons/up.gif";
                    iBtnMoveUp.Attributes.Add("style", "width:16px;height:16px");
                    iBtnMoveUp.CommandArgument = item.Id.ToString();

                    iBtnMoveDown.Enabled = Status == Constants.MaxStatus;
                    iBtnMoveDown.ImageUrl = UrlRoot + "icons/down.gif";
                    iBtnMoveDown.Attributes.Add("style", "width:16px;height:16px");
                    iBtnMoveDown.CommandArgument = item.Id.ToString();

                    iBtnDelete.Enabled = Status == Constants.MaxStatus;
                    iBtnDelete.ImageUrl = UrlRoot + "icons/delete.gif";
                    iBtnDelete.CommandArgument = item.Id.ToString();

                    hlUpdate.Enabled = Status == Constants.MaxStatus;
                    hlUpdate.CommandArgument = item.Id.ToString();

                    hlEdit.Enabled = Status == Constants.MaxStatus ? true : false;
                    hlEdit.NavigateUrl = "~/discuss/" + TaskID.ToString() + "/edit/" + item.Id.ToString() + "/index.htm";

                }
            }
        }
        protected void rptData_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                ImageButton iBtnMoveUp = (ImageButton)e.Item.FindControl("iBtnMoveUp");
                iBtnMoveUp.CausesValidation = false;
                iBtnMoveUp.Click += new ImageClickEventHandler(iBtnMoveUp_Click);

                ImageButton iBtnMoveDown = (ImageButton)e.Item.FindControl("iBtnMoveDown");
                iBtnMoveDown.CausesValidation = false;
                iBtnMoveDown.Click += new ImageClickEventHandler(iBtnMoveDown_Click);

                ImageButton iBtnDelete = (ImageButton)e.Item.FindControl("iBtnDelete");
                iBtnDelete.CausesValidation = false;
                iBtnDelete.Attributes.Add("onclick", "return confirm(\"Bạn có thực sự muốn xóa?\");");
                iBtnDelete.Click += new ImageClickEventHandler(iBtnDelete_Click);

                LinkButton hlUpdate = (LinkButton)e.Item.FindControl("hlUpdate");
                hlUpdate.CausesValidation = false;
                hlUpdate.Click += new EventHandler(ibtnLinkUpdate_Click);
            }
        }

        protected void iBtnMoveUp_Click(object sender, EventArgs e)
        {
            ImageButton iBtn = (ImageButton)sender;
            if (iBtn != null)
            {
                m_Workflows = new WorkflowTask();
                m_Workflows.Id = TaskID;
                m_Workflows = m_Workflows.Get();
                new DiscussDetail { Id = long.Parse(iBtn.CommandArgument), Status = m_Workflows.MoveUp }.UpdateStatus();
                BindData();
            }
        }
        protected void iBtnMoveDown_Click(object sender, EventArgs e)
        {
            ImageButton iBtn = (ImageButton)sender;
            if (iBtn != null)
            {
                m_Workflows = new WorkflowTask();
                m_Workflows.Id = TaskID;
                m_Workflows = m_Workflows.Get();
                new DiscussDetail { Id = long.Parse(iBtn.CommandArgument), Status = m_Workflows.Status }.UpdateStatus();
                BindData();
            }
        }
        protected void iBtnDelete_Click(object sender, EventArgs e)
        {
            ImageButton iBtn = (ImageButton)sender;
            if (iBtn != null)
            {
                long Id = long.Parse(iBtn.CommandArgument);
                new DiscussDetail { Id = Id }.Delete();
                BindData();
            }
        }
        protected void ibtnLinkUpdate_Click(object sender, EventArgs e)
        {
            LinkButton ibtn = (LinkButton)sender;
            if (ibtn != null)
            {
                int id = int.Parse(ibtn.CommandArgument);
                new DiscussDetail { Id = id, DateAnswer = DateTime.Now }.UpdateCreateTime();
            }
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrPage = 1;
            BindData();
        }
        protected void ddlDiscuss_SelectedIndexChanged(object sender, EventArgs e)
        {
            int DiscussId = int.Parse(ddlDiscuss.SelectedValue);
            List<Guest> lstGuest = new Guest() { DiscussId = DiscussId }.GetByDiscussId();
            lstGuest.Insert(0, new Guest() { FullName = "Toàn bộ", GuestId = 0 });
            ddlGuest.DataSource = lstGuest;
            ddlGuest.DataTextField = "FullName";
            ddlGuest.DataValueField = "GuestId";
            ddlGuest.DataBind();

            CurrPage = 1;
            BindData();
        }
        protected void ddlGuest_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrPage = 1;
            BindData();
        }
    }

}