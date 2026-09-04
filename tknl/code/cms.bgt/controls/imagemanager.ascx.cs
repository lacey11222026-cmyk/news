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
using System.IO;
using cms.libs;
using System.Data.SqlTypes;
using System.Globalization;
using Telerik.Web.UI;
using System.Text;

namespace CMS
{
    public partial class controls_imagemanager : System.Web.UI.UserControl
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        private string Keyword = string.Empty;
        private DateTime FromDate = (DateTime)SqlDateTime.MinValue;
        private DateTime ToDate = (DateTime)SqlDateTime.MaxValue;
        private int RecordPerPage = 10;
        private FileManager m_FileManager;
        private string mediaUrl = Config.mediaUrl;
        private string mediaPath = Config.mediaPath;

        //
        private string imgFileTypes = Constants.imageFileTypes;
        public bool AddMetaTags = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.IsSigned())
            {
                Response.Redirect(UrlRoot + "login.aspx?url=" + Server.UrlEncode(Request.RawUrl), true);
            }
            if (AddMetaTags)
            {
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "X-UA-Compatible";
                meta.Content = "IE=7";
                HtmlHead head = (HtmlHead)Page.Header;
                head.Controls.Add(meta);
            }
            if (!this.IsPostBack)
            {
                this.DataBind();
                DateTime CrDate = DateTime.Now;
                txtDateFrom.SelectedDate = CrDate.AddMonths(-2);
                txtDateTo.SelectedDate = CrDate.AddHours(1);
                tbxKeyword.Attributes.Add("style", "width:100%");
                ltlimageFilters.Text = ltlMultiUploadimageFilters.Text = "Hệ thống chỉ cập nhật các file có định dạng sau: " + imgFileTypes;
                BindData(1);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
        }
        protected void BindData(int CurrPage)
        {
            if (tbxKeyword.Text.Trim().Length > 0)
            {
                Keyword = tbxKeyword.Text.Trim();
            }
            if (txtDateFrom.SelectedDate.Value != null)
            {
                FromDate = txtDateFrom.SelectedDate.Value;
            }
            if (txtDateTo.SelectedDate.Value != null)
            {
                ToDate = txtDateTo.SelectedDate.Value;
            }
            m_FileManager = new FileManager();
            int TotalRecord = 0;
            rptData.DataSource = m_FileManager.GetPaged(m_UserValidation.LoginID, FileManager.FileType.Image, Keyword, FromDate, ToDate, CurrPage, RecordPerPage, out TotalRecord);
            rptData.DataBind();
            if (TotalRecord > 0)
            {
                dlPaper.Visible = dlPaper1.Visible = true;
                int TotalPage = TotalRecord / RecordPerPage;
                if (TotalRecord % RecordPerPage > 0)
                    TotalPage += 1;
                DataTable dtPaper = Paper.MakeDataPaper(TotalPage, CurrPage, 3);
                dlPaper.DataSource = dtPaper;
                dlPaper.DataBind();
                dlPaper1.DataSource = dtPaper;
                dlPaper1.DataBind();
            }
            else
            {
                dlPaper.Visible = dlPaper1.Visible = false;

            }
        }
        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                HtmlTableRow trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                Literal ltlSize = (Literal)e.Item.FindControl("ltlSize");
                Literal ltlWidth = (Literal)e.Item.FindControl("ltlWidth");
                Literal ltlHeight = (Literal)e.Item.FindControl("ltlHeight");
                ImageButton iBtnDelete = (ImageButton)e.Item.FindControl("iBtnDelete");
                FileManager item = (FileManager)e.Item.DataItem;
                if (item != null)
                {
                    if (e.Item.ItemIndex % 2 == 1)
                        trItem.Attributes.Add("class", "alter");
                    else
                        trItem.Attributes.Add("class", "item");
                    DateTime CrTime = item.CrTime;
                    string FilePath = mediaUrl + CrTime.ToString("yyyy/MM/dd") + "/";
                    hlName.Text = item.Name;
                    hlName.Attributes.Add("style", "cursor:hand;cursor:pointer");
                    hlName.Attributes.Add("onmouseover", "doTooltip(event,'" + FilePath + item.Name + "" + "','" + item.Name + "','#ffffff','#ff0000')");
                    hlName.Attributes.Add("onmouseout", "hideTip()");

                    string w = "300";
                    string h = "215";
                    if (item.Width < 300)
                        w = item.Width.ToString();

                    if (item.Height < 215)
                        h = item.Height.ToString();
                    hlName.Attributes.Add("onclick", "ShowInfo('" + FilePath + item.Name + "','" + item.Description + "','" + item.Width + "|" + item.Height + "')");

                    ltlSize.Text = item.Length;
                    ltlWidth.Text = item.Width.ToString() + " px";
                    ltlHeight.Text = item.Height.ToString() + " px";
                    iBtnDelete.ImageUrl = UrlRoot + "icons/delete.gif";
                    iBtnDelete.CommandArgument = item.Id.ToString();
                }
            }
        }
        protected void rptData_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {

                ImageButton iBtnDelete = (ImageButton)e.Item.FindControl("iBtnDelete");
                iBtnDelete.CausesValidation = false;
                iBtnDelete.Attributes.Add("onclick", "return confirm(\"Bạn có thực sự muốn xóa?\");");
                iBtnDelete.Click += new ImageClickEventHandler(iBtnDelete_Click);
            }
        }
        protected void iBtnDelete_Click(object sender, EventArgs e)
        {
            ImageButton iBtn = (ImageButton)sender;
            if (iBtn != null)
            {
                int Id = int.Parse(iBtn.CommandArgument);
                FileManager m_FileManager = new FileManager();
                m_FileManager = m_FileManager.Get(Id);
                if (m_FileManager != null)
                {
                    string FilePath = mediaPath + m_FileManager.CrTime.ToString("yyyy/MM/dd") + "/" + m_FileManager.Name;
                    if (File.Exists(FilePath))
                        File.Delete(FilePath);
                    m_FileManager.Delete(Id);
                }
                BindData(1);
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
                BindData(Convert.ToInt32(lbt.CommandArgument));
            }
        }
        protected void btPost_Click(object sender, EventArgs e)
        {
            string Name = "";
            string Extension = "";
            string Length = "";
            string Description = tbxDescription.Text;
            int fWidth = 0;
            int fHeight = 0;
            DateTime CrTime = DateTime.Now;
            string m_mediaPath = mediaPath + CrTime.ToString("yyyy/MM/dd") + "/";
            string m_mediaUrl = mediaUrl + CrTime.ToString("yyyy/MM/dd") + "/";
            if (!Directory.Exists(m_mediaPath))
            {
                Directory.CreateDirectory(m_mediaPath);
            }
            m_FileManager = new FileManager();

            UploadedFile oFile = UploadedFile.FromHttpPostedFile(Request.Files[0]);
            if (oFile.ContentLength > 0)
            {
                Extension = oFile.GetExtension().ToLower();
                if (imgFileTypes.IndexOf(Extension) >= 0)
                {
                    Name = oFile.GetName();
                    long fileSize = oFile.ContentLength;
                    if (fileSize > 1000000) Length = fileSize / 1000000 + " Mb";
                    else if (fileSize > 1000) Length = fileSize / 1000 + " Kb";
                    else Length = fileSize + " b";
                    Name = FileManager.GetUniqueFileName(m_mediaPath, Name);
                    oFile.SaveAs(m_mediaPath + Name, true);

                    System.Drawing.Image img2Scale = System.Drawing.Image.FromFile(m_mediaPath + Name);
                    fWidth = img2Scale.Size.Width;
                    fHeight = img2Scale.Size.Height;


                    m_FileManager.Name = Name;
                    m_FileManager.Description = Description;
                    m_FileManager.Width = fWidth;
                    m_FileManager.Height = fHeight;
                    m_FileManager.Length = Length;
                    m_FileManager.Type = (int)FileManager.FileType.Image;
                    m_FileManager.UserID = m_UserValidation.LoginID;
                    m_FileManager.Insert();
                }
                string iSize = fWidth + "|" + fHeight;
                iSize = tbxW.Text + "|" + tbxH.Text;
                /*if (ddlImgSize.SelectedIndex > 0)
                {
                    iSize = ddlImgSize.SelectedValue;
                }*/
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine("var oWindow = GetRadWindow();");
                sb.AppendLine("var oSendArg = oWindow.Argument;");
                sb.AppendLine("var arg = new Object();");
                sb.AppendLine("if (oSendArg.InstanceId)");
                sb.AppendLine("{");
                sb.AppendLine("arg.InstanceId = oSendArg.InstanceId;");
                sb.AppendLine("}");
                sb.AppendLine("arg.returnValue = '" + m_mediaUrl + Name + "';");
                sb.AppendLine("arg.returnExtension = '" + iSize + "|3|3|1|" + ddlImgAlign.SelectedValue + "|" + Description + "';");
                sb.AppendLine("oWindow.Close(arg);");
                sb.AppendLine("</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "returnValue", sb.ToString());
            }

        }
        protected void btnUploadMulti_Click(object sender, EventArgs e)
        {
            string Name = "";
            string Extension = "";
            string Length = "";
            string Description = "";
            int fWidth = 0;
            int fHeight = 0;
            DateTime CrTime = DateTime.Now;
            string m_mediaPath = mediaPath + CrTime.ToString("yyyy/MM/dd") + "/";
            string m_mediaUrl = mediaUrl + CrTime.ToString("yyyy/MM/dd") + "/";
            if (!Directory.Exists(m_mediaPath))
            {
                Directory.CreateDirectory(m_mediaPath);
            }

            m_FileManager = new FileManager();
            foreach (string fileInputID in Request.Files)
            {
                UploadedFile oFile = UploadedFile.FromHttpPostedFile(Request.Files[fileInputID]);
                if (oFile.ContentLength > 0)
                {
                    Extension = oFile.GetExtension().ToLower();
                    if (imgFileTypes.IndexOf(Extension) >= 0)
                    {
                        Name = oFile.GetName();
                        long fileSize = oFile.ContentLength;
                        if (fileSize > 1000000) Length = fileSize / 1000000 + " Mb";
                        else if (fileSize > 1000) Length = fileSize / 1000 + " Kb";
                        else Length = fileSize + " b";
                        Name = FileManager.GetUniqueFileName(m_mediaPath, Name);
                        oFile.SaveAs(m_mediaPath + Name, true);

                        System.Drawing.Image img2Scale = System.Drawing.Image.FromFile(m_mediaPath + Name);
                        fWidth = img2Scale.Size.Width;
                        fHeight = img2Scale.Size.Height;


                        m_FileManager.Name = Name;
                        m_FileManager.Description = Description;
                        m_FileManager.Width = fWidth;
                        m_FileManager.Height = fHeight;
                        m_FileManager.Length = Length;
                        m_FileManager.Type = (int)FileManager.FileType.Image;
                        m_FileManager.UserID = m_UserValidation.LoginID;
                        m_FileManager.Insert();
                    }
                }
            }
            //Random random = new Random();
            //Response.Redirect(UrlRoot + "common/imagemanager.aspx?params=" + random.NextDouble().ToString());
            RadTabStrip1.SelectedIndex = 0;
            RadPageView1.Selected = true;
            BindData(1);
        }
    }
}