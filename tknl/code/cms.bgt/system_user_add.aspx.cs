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
using System.Globalization;
using cms.libs;

namespace CMS
{

    public partial class system_user_add : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private Users m_Users = null;
        private Parts m_Parts = null;
        private UserValidation m_UserValidation = new UserValidation();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.CheckSysFunction(1))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (!this.IsPostBack)
            {
                txtUsername.Attributes.Add("style", "width:250px");
                txtPass.Attributes.Add("style", "width:250px");
                txtConfirmPass.Attributes.Add("style", "width:250px");
                txtFullname.Attributes.Add("style", "width:250px");
                txtEmail.Attributes.Add("style", "width:250px");
                txtDatebirth.Attributes.Add("style", "width:250px");
                txtTel.Attributes.Add("style", "width:250px");
                txtAddress.Attributes.Add("style", "width:250px; height:50px");
                txtDesc.Attributes.Add("style", "width:250px; height:50px");
                DateTime CrDate = DateTime.Now;
                txtDatebirth.SelectedDate = CrDate;

                treePartWorkflow.ShowCheckBoxes = TreeNodeTypes.Leaf;
                treePartWorkflow.ShowLines = true;
                treePartWorkflow.ShowExpandCollapse = false;
                treePartWorkflow.ExpandAll();
                m_Parts = new Parts();
                DataTable dtRet = m_Parts.GetAllActive();
                TreeNode newNode = null;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {

                    foreach (DataRow dr in dtRet.Rows)
                    {
                        newNode = new TreeNode();
                        newNode.Text = dr["Name"].ToString();
                        newNode.Value = dr["ID"].ToString();
                        newNode.PopulateOnDemand = true;
                        newNode.SelectAction = TreeNodeSelectAction.Expand;

                        treePartWorkflow.Nodes.Add(newNode);
                    }
                }
                rptSysFunc.DataSource = new SysFunction().GetAll();
                rptSysFunc.DataBind();

                ddlDiscuss.DataSource = new Discuss().GetAll(1);
                ddlDiscuss.DataBind();

                int DiscussId = int.Parse(ddlDiscuss.SelectedValue);
                List<Guest> lstGuest = new Guest() { DiscussId = DiscussId }.GetByDiscussId();
                lstGuest.Insert(0, new Guest() { FullName = "Toàn bộ", GuestId = 0 });
                dllGuest.DataSource = lstGuest;
                dllGuest.DataTextField = "FullName";
                dllGuest.DataValueField = "GuestId";
                dllGuest.DataBind();

            }
        }
        protected void treePartWorkflow_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            int PartID = int.Parse(e.Node.Value);
            DataRow m_Part_Info = new Parts().GetInfo(PartID);
            WorkflowTask m_WorkflowTask = new WorkflowTask();
            m_WorkflowTask.WorkflowId = Convert.ToInt32(m_Part_Info["WorkflowID"]);
            List<WorkflowTask> l_WorkflowTask = m_WorkflowTask.GetByWorkflowId();
            if (l_WorkflowTask != null && l_WorkflowTask.Count > 0)
            {
                foreach (WorkflowTask dr in l_WorkflowTask)
                {

                    TreeNode newNode = new TreeNode();
                    newNode.Value = dr.Id.ToString();
                    newNode.Text = dr.Name;
                    newNode.PopulateOnDemand = false;

                    newNode.SelectAction = TreeNodeSelectAction.None;
                    if (dr.Id == 1)
                        newNode.Checked = true;

                    e.Node.ChildNodes.Add(newNode);

                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                m_Users = new Users();

                //Xác định xem userName này đã có hay chưa
                DataRow m_User_Info = m_Users.GetInfo(txtUsername.Text.ToString());
                if (m_User_Info != null)
                {
                    ltlError.Text = "Tên truy cập này đã tồn tại, bạn hãy chọn tên truy cập khác!";
                    return;
                }
                int UserID = m_Users.Insert(txtUsername.Text, Encrypt.MD5(txtPass.Text), txtFullname.Text, txtAddress.Text, txtEmail.Text, txtTel.Text, int.Parse(cboGender.SelectedValue), txtDesc.Text, txtDatebirth.SelectedDate.Value, (int)((chkActive.Checked == true) ? 1 : 0));
                int PartID = 0;
                int TaskID = 0;
                UserTask m_UserTask = new UserTask();
                foreach (TreeNode node in treePartWorkflow.Nodes)
                {
                    PartID = int.Parse(node.Value);
                    foreach (TreeNode childnode in node.ChildNodes)
                    {
                        TaskID = int.Parse(childnode.Value);
                        if (childnode.Checked)
                        {
                            m_UserTask.Insert(TaskID, UserID, PartID);

                        }
                    }
                }
                SysFunc_User fUser = new SysFunc_User();
                foreach (RepeaterItem rptItem in rptSysFunc.Items)
                {
                    CheckBox cbxFunctionID = (CheckBox)rptItem.FindControl("cbxFunctionID");
                    if (cbxFunctionID.Checked)
                    {
                        HiddenField hiddenID = (HiddenField)rptItem.FindControl("hiddenID");
                        fUser.Insert(int.Parse(hiddenID.Value), UserID);
                    }
                }

                new UsersDiscuss()
                {
                    UserId = UserID,
                    DisscussId = int.Parse(ddlDiscuss.SelectedValue),
                    GuestId = int.Parse(dllGuest.SelectedValue)
                }.Insert();

                hfResult.Value = "1";
            }
            catch (Exception ex)
            {
                ltlError.Text = "<strong>Có lỗi trong quá trình cập nhật thông tin người dùng: " + ex.ToString() + "</strong>";
                return;
            }

        }
        protected void rptSysFunc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                CheckBox cbxFunctionID = (CheckBox)e.Item.FindControl("cbxFunctionID");
                HiddenField hiddenID = (HiddenField)e.Item.FindControl("hiddenID");
                Literal ltlFunctionName = (Literal)e.Item.FindControl("ltlFunctionName");
                DataRowView item = (DataRowView)e.Item.DataItem;
                if (item != null)
                {
                    cbxFunctionID.Attributes.Add("style", "cursor:pointer");
                    if (item["ID"].ToString() != "1")
                        cbxFunctionID.Checked = true;
                    hiddenID.Value = item["ID"].ToString();
                    ltlFunctionName.Text = item["Name"].ToString();
                }
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

            dllGuest.Items.Insert(0, new ListItem(" Toàn bộ ", "0"));
            dllGuest.SelectedIndex = 0;
        }
    }
}