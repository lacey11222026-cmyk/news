using System;
using System.Collections;
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
namespace CMS
{

    public partial class service_discuss_reader : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private int _RecordPerPage = 30;
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
            if (!m_UserValidation.CheckSysFunction(3))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (!m_UserValidation.CheckSysFunction(3))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (!this.IsPostBack)
            {
                ddlDiscuss.Attributes.Add("style", "width:350px");
                tbxKeyword.Attributes.Add("style", "width:350px");
                ddlDiscuss.DataSource = new Discuss().GetAll(1);
                ddlDiscuss.DataBind();
                BindData();
            }
        }
        protected void BindData()
        {
            string Keyword = tbxKeyword.Text;
            int DiscussId = Convert.ToInt32(ddlDiscuss.SelectedValue);
            int _TotalRecord = 0;
            rptData.DataSource = new Reader().GetPaged(Keyword, DiscussId, CurrPage, _RecordPerPage, out _TotalRecord);
            rptData.DataBind();
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
            foreach (RepeaterItem rptItem in rptData.Items)
            {
                CheckBox cbxSelect = (CheckBox)rptItem.FindControl("cbxSelect");
                if (cbxSelect.Checked)
                {
                    HiddenField hiddenID = (HiddenField)rptItem.FindControl("hiddenID");
                    try
                    {
                        new Reader { ReaderId = Int32.Parse(hiddenID.Value) }.Delete();
                    }
                    catch
                    { }
                }
            }
            BindData();
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
                Literal ltlFullName = (Literal)e.Item.FindControl("ltlFullName");
                Literal ltlGender = (Literal)e.Item.FindControl("ltlGender");
                Literal ltlAge = (Literal)e.Item.FindControl("ltlAge");
                Literal ltlAddress = (Literal)e.Item.FindControl("ltlAddress");
                Literal ltlQuestion = (Literal)e.Item.FindControl("ltlQuestion");
                Literal ltlStatus = (Literal)e.Item.FindControl("ltlStatus");
                ImageButton iBtnStatus = (ImageButton)e.Item.FindControl("iBtnStatus");
                Reader item = (Reader)e.Item.DataItem;
                if (item != null)
                {
                    if (e.Item.ItemIndex % 2 == 1)
                        trItem.Attributes.Add("class", "alter");
                    else
                        trItem.Attributes.Add("class", "item");
                    cbxSelect.Attributes.Add("style", "cursor:pointer");

                    hiddenID.Value = item.ReaderId.ToString();
                    ltlFullName.Text = item.FullName;

                    ltlGender.Text = item.Gender == 1 ? "Nam" : "Nữ";
                    ltlAge.Text = item.Age.ToString();
                    ltlAddress.Text = item.Address;
                    string sQuestion = "";
                    List<DiscussDetail> l_DiscussDetail = new DiscussDetail().GetByDiscussIdAndReaderId(Int32.Parse(ddlDiscuss.SelectedValue), item.ReaderId);
                    foreach (DiscussDetail m_DiscussDetail in l_DiscussDetail)
                    {
                        sQuestion += m_DiscussDetail.Question + "<br>";
                    }
                    ltlQuestion.Text = sQuestion;
                    iBtnStatus.CommandArgument = item.ReaderId.ToString();

                }
            }
        }
        protected void ddlDiscuss_SelectedIndexChanged(object sender, EventArgs e)
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
                //new
                //m_Users.UpdateStatus(int.Parse(iBtn.CommandArgument));
                BindData();
            }
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
    }

}