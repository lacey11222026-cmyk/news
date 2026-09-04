using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using cms.libs;
using Telerik.Web.UI;

namespace CMS2012.controls
{
    public partial class mp3manager : System.Web.UI.UserControl
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        private string mediaFileTypes = Constants.musicFileTypes;
        private string Keyword = string.Empty;
        private DateTime FromDate = (DateTime)SqlDateTime.MinValue;
        private DateTime ToDate = (DateTime)SqlDateTime.MaxValue;
        private int RecordPerPage = 10;
        private FileManager m_FileManager;
        private string mediaUrl = Config.mediaUrl;
        private string mediaPath = Config.mediaPath;
        private ConvertTools m_ConvertTools;
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
                txtDateTo.SelectedDate = CrDate;
                tbxKeyword.Attributes.Add("style", "width:100%");
                ltlmediaFilters.Text = "Hệ thống chỉ cập nhật các file có định dạng .mp3";
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
            rptData.DataSource = m_FileManager.GetPaged(m_UserValidation.LoginID, FileManager.FileType.Music, Keyword, FromDate, ToDate, CurrPage, RecordPerPage, out TotalRecord);
            rptData.DataBind();
            if (TotalRecord > 0)
            {
                ltlTotal.Visible = dlPaper.Visible = dlPaper1.Visible = true;
                ltlTotal1.Text = ltlTotal.Text = "Tìm thấy " + TotalRecord.ToString() + "  bản ghi.";
                int TotalPage = TotalRecord / RecordPerPage;
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
            if (e.Item.ItemIndex != -1 && e.Item.ItemType != ListItemType.Separator)
            {
                HtmlTableRow trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                Literal ltlSize = (Literal)e.Item.FindControl("ltlSize");
                Literal ltlCrTime = (Literal)e.Item.FindControl("ltlCrTime");
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
                    hlName.Attributes.Add("onmouseover", "doTooltip(event,'" + FilePath + item.Name + ".jpg" + "','" + item.Name + "','#ffffff','#ff0000')");
                    hlName.Attributes.Add("onmouseout", "hideTip()");
                    hlName.Attributes.Add("onclick", "ShowInfo('" + FilePath + item.Name + "','" + item.Width.ToString() + "','" + item.Height.ToString() + "','" + item.Description + "')");

                    ltlSize.Text = item.Length;
                    ltlCrTime.Text = CrTime.ToString("dd/MM/yyyy HH:mm:ss");
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
            string NameWithoutExtension;
            string Length = "";
            string Description = tbxDescription.Text;
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
                if (mediaFileTypes.IndexOf(Extension) >= 0)
                {
                    Name = oFile.GetName();
                    Name = FileManager.GetUniqueFileName(m_mediaPath, Name);
                    NameWithoutExtension = Name.Replace(Extension, string.Empty);

                    oFile.SaveAs(m_mediaPath + Name, true);
                    m_ConvertTools = new ConvertTools();

                    FileInfo flvInfo = new FileInfo(m_mediaPath + NameWithoutExtension + Extension);

                    long fileSize = flvInfo.Length;
                    if (fileSize > 1000000) Length = fileSize / 1000000 + " Mb";
                    else if (fileSize > 1000) Length = fileSize / 1000 + " Kb";
                    else Length = fileSize + " b";
                    int fWidth = 200;
                    int fHeight = 25;
                    m_FileManager.Name = NameWithoutExtension + Extension;
                    m_FileManager.Description = "";
                    m_FileManager.Width = fWidth;
                    m_FileManager.Height = fHeight;
                    m_FileManager.Length = Length;
                    m_FileManager.Type = (int)FileManager.FileType.Music;
                    m_FileManager.UserID = m_UserValidation.LoginID;
                    m_FileManager.Insert();
                }
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
                sb.AppendLine("arg.returnExtension = '400|300|3|3|1|" + ddlImgAlign.SelectedValue + "|" + Description + "';");
                sb.AppendLine("oWindow.Close(arg);");
                sb.AppendLine("</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "returnValue", sb.ToString());
            }
        }
    }
}