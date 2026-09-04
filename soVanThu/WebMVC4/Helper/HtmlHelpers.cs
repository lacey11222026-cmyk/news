using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTILS;

namespace WebMVC4.Helper
{
    public static class HtmlHelpers
    {
        public static string GetDateDocument(DateTime? obj)
        {
            if (!obj.HasValue)
            {


                return "";
            }
            else
            {
                if (obj.Value.Year < 9000)
                {
                    return obj.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }

            }
        }
        public static string BindChildCategoriesSiteMap(int categoryId, int type)
        {

            var _childCategory = new CategoryBO().GetAllChildCategories(categoryId, 8, false);

            var html = "";

            if (_childCategory != null && _childCategory.Count > 0)
            {

                html += " <ul rel=\"open\">";
                var index = 0;
                foreach (CATEGORY_FULL _child in _childCategory)
                {
                    var url = Utils.FormatUrlRewriteByType(_child.Id, _child.Name, (int)_child.Type, _child.Link);

                    html += " <li><a href=\"" + url + "\">" + _child.Name + "</a></li>";

                    index++;
                }

                html += "</ul>";
            }

            return html;

        }
        public static string BindChildCategories(int categoryId, int type)
        {

            var _childCategory = new CategoryBO().GetAllChildCategories(categoryId, 10, false);

            var html = "";

            if (_childCategory != null && _childCategory.Count > 0)
            {

                html += "<ul class='nav-sub'>";
                var index = 0;
                foreach (CATEGORY_FULL _child in _childCategory)
                {
                    if (_child.Published == 1)
                    {
                        var url = Utils.FormatUrlRewriteByType(_child.Id, _child.Name, (int)_child.Type, _child.Link);

                        if (index == _childCategory.Count - 1)
                        {
                            html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a></li>";
                        }
                        else
                        {
                            html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a></li>";
                        }
                    }


                    index++;
                }

                html += "</ul>";
            }

            return html;

        }
        public static string BindMobileChildCategorie(int categoryId, int type)
        {

            var _childCategory = new CategoryBO().GetAllChildCategories(categoryId, 10, false);

            var html = "";

            if (_childCategory != null && _childCategory.Count > 0)
            {

                html += "<ul class='sub02'>";
                var index = 0;
                foreach (CATEGORY_FULL _child in _childCategory)
                {
                    if (_child.Published == 1)
                    {
                        var url = Utils.FormatUrlRewriteByType(_child.Id, _child.Name, (int)_child.Type, _child.Link);

                        if (index == _childCategory.Count - 1)
                        {
                            html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a></li>";
                        }
                        else
                        {
                            html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a></li>";
                        }
                    }


                    index++;
                }

                html += "</ul>";
            }

            return html;

        }
        public static string EmbedVideo(string url, string image, string width, string height)
        {
            var result = "";
            result += "<embed width=\"" + width + "\" height=\"" + height + "\"";
            result += "flashvars=\"file=" + url + "&amp;volume=60&amp;repeat=false&amp;bufferlength=10&amp;";
            result += "image=" + image + "\"";
            result += " allowscriptaccess=\"always\" allowfullscreen=\"true\" wmode=\"transparent\" quality=\"hight\"";
            result += "src=\"/flash/flvplayer.swf\" type=\"application/x-shockwave-flash\" name=\"flvplayer\" id=\"flvplayer\"></embed> ";
            return result;
        }
        public static string GetRolesByUserName(string userName)
        {
            // Get array of user's roles
            string[] roles = System.Web.Security.Roles.GetRolesForUser(userName);
            if (roles.Count() > 0)
            {
                // Parse array of roles
                string userRoles = String.Empty;
                foreach (string r in roles)
                {
                    userRoles += " | " + GetRoleName(r);
                }
                userRoles = userRoles.Remove(0, 2);
                return userRoles;
            }

            return string.Empty;

        }
        public static string Getimgren(object fileexten, object filname, object hdfCurrentFolder)
        {
            string imgrenurl = "";
            switch (fileexten.ToString())
            {
                case ".jpg":
                    imgrenurl = hdfCurrentFolder + filname.ToString();
                    break;
                case ".doc":
                    imgrenurl = "/Administrator//Content/images/Icon/doc.jpg";
                    break;
                case ".docx":
                    imgrenurl = "/Administrator//Content/images/Icon/doc.jpg";
                    break;
                case ".rar":
                    imgrenurl = "/Administrator//Content/images/Icon/rar.jpg";
                    break;
                case ".zip":
                    imgrenurl = "/Administrator//Content/images/Icon/rar.jpg";
                    break;
                case ".xls":
                    imgrenurl = "/Administrator//Content/images/Icon/exel.jpg";
                    break;
                case ".xlsx":
                    imgrenurl = "/Administrator//Content/images/Icon/exel.jpg";
                    break;
                case ".ppt":
                    imgrenurl = "/Administrator//Content/images/Icon/ppt.jpg";
                    break;
                case ".swf":
                    imgrenurl = "/Administrator//Content/images/Icon/flv.jpg";
                    break;
                case ".flv":
                    imgrenurl = "/Administrator//Content/images/Icon/flv.jpg";
                    break;
                case ".pdf":
                    imgrenurl = "/Administrator//Content/images/Icon/pdf.jpg";
                    break;
                case ".mp3":
                    imgrenurl = "/Administrator//Content/images/Icon/media.jpg";
                    break;
                case ".avi":
                    imgrenurl = "/Administrator//Content/images/Icon/media.jpg";
                    break;
                case ".mp4":
                    imgrenurl = "/Administrator//Content/images/Icon/media.jpg";
                    break;
                Default:
                    imgrenurl = "/Administrator//Content/images/Icon/default.jpg";
                    break;
            }
            return imgrenurl;

        }
        public static string GetRoleName(string RoleName)
        {
            string result = "";
            switch (RoleName.ToLower())
            {

                case "administrator":
                    result = "Quản trị hệ thống";
                    break;
                case "album":
                    result = "Quản lý thư viện ảnh";
                    break;
                case "banner":
                    result = "Quản lý banner";
                    break;
                case "category":
                    result = "Quản lý chuyên mục";
                    break;
                case "competition":
                    result = "Quản lý cuộc thi ảnh";
                    break;
                case "competitioncreate":
                    result = "Quản lý cuộc thi ảnh";
                    break;
                case "contact":
                    result = "Quản lý danh bạ";
                    break;
                case "document":
                    result = "Quản lý văn bản";
                    break;
                case "comment":
                    result = "Quản lý bình luận";
                    break;
                case "report":
                    result = "Báo cáo";
                    break;
                case "guest":
                    result = "Phóng viên";
                    break;
                case "local":
                    result = "Quản trị nội bộ";
                    break;
                case "mark":
                    result = "Chấm nhuận bút";
                    break;
                case "newsedit":
                    result = "Biên tập tin tức";
                    break;
                case "newspublish":
                    result = "Xuất bản tin tức";
                    break;
                case "rate":
                    result = "Cấu hình";
                    break;
                case "customer":
                    result = "Nội dung giới thiệu";
                    break;


                default:
                    result = RoleName;
                    break;
            }
            return result;

        }
        public static string GetRegionName(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Trên";
                    break;
                case 11:
                    result = "Top";
                    break;
                case 2:
                    result = "Phải";
                    break;
                case 3:
                    result = "Trên mobile";
                    break;
                case 13:
                    result = "Botom";
                    break;
                case 4:
                    result = "Trên";
                    break;
                case 5:
                    result = "Phải 2";
                    break;
                case 6:
                    result = "Tin tức";
                    break;
                case 7:
                    result = "Video";
                    break;
                case 15:
                    result = "Center";
                    break;
                //case 6:
                //    result = "dưới trái";
                //    break;
                default:
                    result = "";
                    break;
            }
            return result;

        }
        public static string BindMChildCategories(int categoryId, int type)
        {

            var _childCategory = new CategoryBO().GetAllChildCategories(categoryId, 10, false);

            var html = "";

            if (_childCategory != null && _childCategory.Count > 0)
            {

                html += " <span class=\"ico-dropdown\"></span><ul class=\"nav-mb-sub\">";
                var index = 0;
                foreach (CATEGORY_FULL _child in _childCategory)
                {
                    if (_child.Published == 1)
                    {
                        var url = Utils.FormatUrlRewriteByType(_child.Id, _child.Name, (int)_child.Type, _child.Link);

                        if (index == _childCategory.Count - 1)
                        {
                            html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a></li>";
                        }
                        else
                        {
                            html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a></li>";
                        }
                    }


                    index++;
                }

                html += "</ul>";
            }

            return html;

        }
        public static string GetNewsType(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Biên tập";
                    break;
                case 2:
                    result = "Dịch";
                    break;
                case 3:
                    result = "Trích nguồn";
                    break;


                default:
                    result = "Bài viết";
                    break;
            }
            return result;

        }
        public static string GetNewsStatus(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Đang biên tập";
                    break;
                case 2:
                    result = "Đợi xuất bản";
                    break;
                case 3:
                    result = "Đang viết";
                    break;
                case 4:
                    result = "Đã xuất bản";
                    break;

                default:
                    result = "";
                    break;
            }
            return result;

        }
        public static string GetNewsCategoryName(int? categoryId, List<CATEGORY_FULL> lstdata)
        {
            try
            {
                var obj = lstdata.Where(x => x.Id == categoryId.GetValueOrDefault()).FirstOrDefault();
                if (obj == null)
                    return "N/A";
                return obj.Name.Replace("-+", "");
            }
            catch
            {

                return "N/A";
            }
        }
        public static string MissionCate(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Đề tài";
                    break;
                case 2:
                    result = "Dự án SXTN";
                    break;


                default:
                    result = "N/A";
                    break;
            }
            return result;

        }
        public static string MissionAccept(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Đã nghiệm thu";
                    break;
                case 0:
                    result = "Chưa nghiệm thu";
                    break;


                default:
                    result = "N/A";
                    break;
            }
            return result;

        }
        public static string MissionResult(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Đạt";
                    break;
                case 0:
                    result = "Chưa đạt";
                    break;

                case 2:
                    result = "Chưa nghiệm thu";
                    break;
                default:
                    result = "N/A";
                    break;
            }
            return result;

        }
        public static string MissionAuthor(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Khối viện";
                    break;
                case 2:
                    result = "Khối trường";
                    break;


                default:
                    result = "Đơn vị khác";
                    break;
            }
            return result;

        }
        public static string GetDocType(int Region)
        {
            string result = "";
            switch (Region)
            {

                case 13:
                    result = "Thông tư";
                    break;
                case 11:
                    result = "Quyết định";
                    break;
                case 3:
                    result = "Công văn";
                    break;
                case 1:
                    result = "Chỉ thị";
                    break;
                case 2:
                    result = "Công điện";
                    break;
                case 5:
                    result = "Luật";
                    break;
                case 4:
                    result = "Hiệp định";
                    break;
                case 14:
                    result = "Thông tư liên tịch";
                    break;
                case 6:
                    result = "Nghị định";

                    break;
                case 8:
                    result = "Nghị quyết";
                    break;
                case 9:
                    result = "Pháp lệnh";
                    break;
                case 7:
                    result = "Nghị định thư";
                    break;
                case 15:
                    result = "Văn kiện";
                    break;
                case 12:
                    result = "Thoả thuận";
                    break;
                case 16:
                    result = "Văn bản hợp nhất";
                    break;
                case 17:
                    result = "Bản ghi nhớ";
                    break;
                case 18:
                    result = "Công điện";
                    break;
                case 19:
                    result = "Quy định";
                    break;
                case 20:
                    result = "Thông báo";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;

        }
        public static string GetDocArea(int type)
        {
            string result = "";
            switch (type)
            {

                case 25:
                    result = "Phát điện nhà máy nhiệt điện";
                    break;
                case 28:
                    result = "Phát điện nhà máy phong điện";
                    break;
                case 22:
                    result = "Phát điện nhà máy thuỷ điện";
                    break;
                case 18:
                    result = "Phân phối và bán lẻ điện từ cấp điện áp 0,4 kV trở xuống";
                    break;
                case 20:
                    result = "Phân phối và bán lẻ điện từ cấp điện áp 110 kV trở xuống ";
                    break;
                case 19:
                    result = "Phân phối và bán lẻ điện từ cấp điện áp 22kV trở xuống";
                    break;
                case 24:
                    result = "Phân phối và bán lẻ điện từ cấp điện áp 35 kV trở xuống";
                    break;
                case 21:
                    result = "Phân phối và bán lẻ điện từ cấp điện áp 15 kV trở xuống";
                    break;
                case 17:
                    result = "Tư vấn đầu tư xây dựng điện";
                    break;
                case 16:
                    result = "Truyền tải điện";
                    break;
                case 1:
                    result = "Công nghiệp nặng";
                    break;
                case 4:
                    result = "Công nghiệp hỗ trợ";
                    break;
                case 6:
                    result = "Giấy phép hoạt động điện lực";
                    break;
                case 2:
                    result = "Công nghiệp thực phẩm";
                    break;
                case 7:
                    result = "Mua bán điện";
                    break;
                case 11:
                    result = "Thị trường điện lực";
                    break;
                case 3:
                    result = "Công nghiệp chế biến, chế tạo";
                    break;
                case 9:
                    result = "An toàn - Kỹ thuật điện";
                    break;
                case 10:
                    result = "Tiết kiệm điện";
                    break;
                case 8:
                    result = "Thuỷ điện";
                    break;
                case 12:
                    result = "Kiểm tra, giải quyết tranh chấp - xử lý vi phạm";
                    break;
                case 15:
                    result = "Tư vấn giám sát thi công các công trình điện";
                    break;

                default:
                    result = "";
                    break;
            }
            return result;

        }
        public static string GetDocArea2(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Quy định chung";
                    break;
                case 4:
                    result = "Đầu tư - Quy hoạch";
                    break;
                case 6:
                    result = "Giấy phép hoạt động điện lực";
                    break;
                case 2:
                    result = "Giá điện";
                    break;
                case 7:
                    result = "Mua bán điện";
                    break;
                case 11:
                    result = "Thị trường điện lực";
                    break;
                case 3:
                    result = "Hệ thống điện Quốc gia";
                    break;
                case 9:
                    result = "An toàn - Kỹ thuật điện";
                    break;
                case 10:
                    result = "Tiết kiệm điện";
                    break;
                case 8:
                    result = "Thuỷ điện";
                    break;
                case 12:
                    result = "Kiểm tra, giải quyết tranh chấp - xử lý vi phạm";
                    break;
                case 15:
                    result = "Tư vấn giám sát thi công các công trình điện";
                    break;

                default:
                    result = "";
                    break;
            }
            return result;

        }
        public static string GetSKProgress(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Chưa đánh giá";
                    break;
                case 2:
                    result = "Đang đánh giá";
                    break;
                case 3:
                    result = "Đã đánh giá";
                    break;


                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static string GetSKRegion(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Thí điểm";
                    break;
                case 2:
                    result = "Chính thức";
                    break;
                case 3:
                    result = "Nhân rộng";
                    break;


                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static string GetSKResult(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Đạt";
                    break;
                case 2:
                    result = "Chưa đánh giá";
                    break;
                case 3:


                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static string GetSKStatus(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Đang triển khai";
                    break;
                case 2:
                    result = "Đã hoàn thành";
                    break;
                case 3:
                    result = "Tạm dừng";
                    break;
                case 4:
                    result = "Huỷ bỏ";
                    break;

                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static int GetSKStatusInt(string type)
        {
            int result = 0;
            switch (type)
            {

                case "Đang triển khai":
                    result = 1;
                    break;
                case "Hoàn thành":
                case "Đã hoàn thành":
                    result = 2;
                    break;
                case "Tạm dừng":
                    result = 3;
                    break;
                case "Huỷ bỏ":
                    result = 4;
                    break;

                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public static string GetTTHCArea(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Công nghiệp nặng";
                    break;
                case 2:
                    result = "Công nghiệp thực phẩm";
                    break;
                case 3:
                    result = "Công nghiệp chế biến, chế tạo";
                    break;
                case 4:
                    result = "Công nghiệp hỗ trợ";
                    break;

                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static string GetTTHCAgent(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Bộ Công nghiệp";
                    break;
                case 2:
                    result = "Bộ Công Thương";
                    break;
                case 11:
                    result = "Thủ tướng Chính phủ";
                    break;
                case 5:
                    result = "Bộ Tài chính";
                    break;
                case 15:
                    result = "Cục Công nghiệp";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static string GetDocAgent(int type)
        {
            string result = "";
            switch (type)
            {

                case 1:
                    result = "Bộ Công nghiệp";
                    break;
                case 2:
                    result = "Bộ Công Thương";
                    break;
                case 11:
                    result = "Thủ tướng Chính phủ";
                    break;
                case 5:
                    result = "Bộ Tài chính";
                    break;
                case 15:
                    result = "Cục Công nghiệp";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
        public static string GetItemLogTypeName(int? type)
        {
            string result = "";
            switch (type.GetValueOrDefault())
            {

                case 1:
                    result = "Giới thiệu";
                    break;
                case 2:
                    result = "Tin bài";
                    break;
                case 3:
                    result = "";
                    break;
                case 4:
                    result = "Album";
                    break;
                case 5:
                    result = "Văn bản";
                    break;
                case 6:
                    result = "";
                    break;
                case 7:
                    result = "Banner";
                    break;
                case 8:
                    result = "Bình luận";
                    break;
                case 9:
                    result = "Chuyên mục";
                    break;
                default:
                    result = "Hệ thống";
                    break;
            }
            return result;

        }
    }
}