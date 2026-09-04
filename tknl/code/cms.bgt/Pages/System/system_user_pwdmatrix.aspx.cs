using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

using cms.libs;

namespace CMS2012
{
    public partial class system_user_pwdmatrix : Page
    {
        public string UrlRoot = DBCommon.UrlRoot;
        private UserValidation m_UserValidation = new UserValidation();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (m_UserValidation.IsSigned() == false)
                Response.Redirect(UrlRoot);
            CheckQuyen();

            if (!IsPostBack)
            {
                Parts m_Parts = new Parts();
                DataTable dtPart = m_Parts.GetAll();
                foreach (DataRow dr in dtPart.Rows)
                {
                    ddlPart.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
                }
                ddlPart.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void CheckQuyen()
        {
            if (m_UserValidation.IsSigned() == false)
                Response.Redirect(UrlRoot);
            if (CheckPermission(m_UserValidation.LoginName) == false)
            {
                Response.Write("Bạn không có quyền truy cập");
                Response.End();
                return;
            }
        }

        protected bool CheckPermission(string AccountName)
        {
            bool tt = false;
            if (AccountName == "huonghx")
                tt = true;
            return tt;
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            string Keyword = txtUsername.Text;
            int PartID = Convert.ToInt32(ddlPart.SelectedValue);
            short Status = Convert.ToInt16(1);
            int _TotalRecord = 0;
            var item = users.GetPagedbyList("", PartID, Status, 1, 100, out _TotalRecord);
            CheckQuyen();

            foreach (var item1 in item)
            {
                GetMatrix(item1.FullName, item1.Name);
            }
        }

        private bool checkEmail(string userName)
        {
            bool f = false;
            Users user = new Users().GetbyUserName(userName);
            if (user == null)
                f = false;
            else
                f = true;

            return f;
        }

        private void GetMatrix(string fullname, string username)
        {
            //grvpass.Width = 213;
            //var newpass = string.Empty;
            //Rowselect = string.Empty;
            var _securityCode = "";
            var data = new DataTable();
            for (var index = 1; index < 10; index++)
                data.Columns.Add(new DataColumn("column" + index));
            var listMaTrix = GetListMaTrix(9, 9, 1);//9 hang - 9 cot - 2 ky tu
            foreach (var variable in listMaTrix)
            {
                var row = data.NewRow();
                var arr = variable.Split('|');
                for (var index = 0; index < 9; index++)
                {
                    _securityCode += arr[index];
                    row["column" + (index + 1)] = arr[index];
                }
                _securityCode += "|";
                data.Rows.Add(row);
            }
            if (new Users().UpdateMatrix(username, DateTime.Now.ToString("yyyyMMdd") + "#" + _securityCode))
            {
                FillToImage(data, fullname, username);
            }
            else
                ltlMsg.Text = "Cập nhật thất bại";
        }

        public List<string> GetListMaTrix(int col, int row, int value)
        {
            const string key = "123456789abcdefgjijklmnpqrstuvxyzABCDEFGHIJKLMNPQRSTUVXYZ";
            var keyLenght = key.Length;
            var list = new List<string>();
            var random = new Random();
            for (var index = 0; index < col; index++)
            {
                var text = string.Empty;
                for (var rowindex = 0; rowindex < row; rowindex++)
                {
                    var s = string.Empty;
                    for (var i = 0; i < value; i++)
                        s = s + key[random.Next(keyLenght)];
                    text += s + "|";
                }
                text = text.Substring(0, text.Length - 1);
                list.Add(text);
            }
            return list;
        }

        private void FillToImage(DataTable dataMatrix, string txtFullName, string txtUsername)
        {
            string path = Server.MapPath("~/ImgMatrix") + "\\";
            string email = txtUsername.Trim();
            string fullName = txtFullName.Trim();

            var data = dataMatrix;//(DataTable)Session[SessionManager.SESSION_MATRIX];
            var finalBitmap = new Bitmap(path + "root.png");
            var g = Graphics.FromImage(finalBitmap);

            g.DrawString(email, new Font("Tahoma", 12), Brushes.Black, new PointF(60, 60));
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString("Tài khoản:" + fullName, new Font("Tahoma", 12), Brushes.Black, new PointF(60, 80));
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString("Ngày có hiệu lực:" + DateTime.Now, new Font("Tahoma", 9), Brushes.Black, new PointF(60, 335));
            g.DrawString("Ngày hết hạn:" + DateTime.Now.AddDays(90), new Font("Tahoma", 9), Brushes.Black, new PointF(60, 360));

            //g.Save();
            var strFullPathToMyFile = Path.Combine(path + email + ".jpg", path + email + ".jpg");
            finalBitmap.Save(strFullPathToMyFile, ImageFormat.Jpeg);

            var dataGrid = new DataGridView { Height = 201, Width = 183 };
            dataGrid.Font = new Font("Tahoma", 9);
            var columnArr = new DataGridViewColumn[9];
            for (var colIndex = 0; colIndex < 9; colIndex++)
            {
                columnArr[colIndex] = new DataGridViewTextBoxColumn { Width = 20 };
            }
            dataGrid.Columns.AddRange(columnArr);
            for (var i = 0; i < 8; i++)
                dataGrid.Rows.Add(new DataGridViewRow().SetValues(new object[8]));

            for (var index = 0; index < 9; index++)
                for (var columnIndex = 0; columnIndex < 9; columnIndex++)
                    dataGrid.Rows[index].Cells[columnIndex].Value = data.Rows[index][columnIndex];
            dataGrid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGrid.RowsDefaultCellStyle.SelectionBackColor = Color.White;
            dataGrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            //dataGrid.DataSource = bindingSource1;
            //bindingSource1.DataSource = data;
            //dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            //dataGrid.AutoGenerateColumns = true;
            //dataGrid.DataSource = data;
            dataGrid.Rows[0].Cells[0].Selected = false;
            dataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGrid.BackgroundColor = Color.WhiteSmoke;
            dataGrid.RowHeadersVisible = false;
            dataGrid.ColumnHeadersVisible = false;
            dataGrid.ForeColor = Color.Black;
            dataGrid.DrawToBitmap(finalBitmap, new Rectangle(60, 120, 183, 201));
            finalBitmap.Save(strFullPathToMyFile, ImageFormat.Jpeg);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
        }
    }
}