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

                html += "<ul class='ulsub-2'>";
                var index = 0;
                foreach (CATEGORY_FULL _child in _childCategory)
                {
                    if(_child.Published==1)
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

                html += "<ul class='nav-mb-sub'>";
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
                html = "<span class=\"ico-dropdown\"></span>" + html;
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
                    userRoles += " | " + GetRoleName( r);
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
                    imgrenurl = "/Administrator/images/Icon/doc.jpg";
                    break;
                case ".docx":
                    imgrenurl = "/Administrator/images/Icon/doc.jpg";
                    break;
                case ".rar":
                    imgrenurl = "/Administrator/images/Icon/rar.jpg";
                    break;
                case ".zip":
                    imgrenurl = "/Administrator/images/Icon/rar.jpg";
                    break;
                case ".xls":
                    imgrenurl = "/Administrator/images/Icon/exel.jpg";
                    break;
                case ".xlsx":
                    imgrenurl = "/Administrator/images/Icon/exel.jpg";
                    break;
                case ".ppt":
                    imgrenurl = "/Administrator/images/Icon/ppt.jpg";
                    break;
                case ".swf":
                    imgrenurl = "/Administrator/images/Icon/flv.jpg";
                    break;
                case ".flv":
                    imgrenurl = "/Administrator/images/Icon/flv.jpg";
                    break;
                case ".pdf":
                    imgrenurl = "/Administrator/images/Icon/pdf.jpg";
                    break;
                case ".mp3":
                    imgrenurl = "/Administrator/images/Icon/media.jpg";
                    break;
                case ".avi":
                    imgrenurl = "/Administrator/images/Icon/media.jpg";
                    break;
                case ".mp4":
                    imgrenurl = "/Administrator/images/Icon/media.jpg";
                    break;
                Default:
                    imgrenurl = "/Administrator/images/Icon/default.jpg";
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
                case "sale":
                    result = "Quản trị dự án đề xuất";
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
                    result = "Dưới 1";
                    break;
                case 13:
                    result = "Botom";
                    break;
                case 4:
                    result = "Trên";
                    break;
                case 5:
                    result = "Dưới 2";
                    break;
                case 6:
                    result = "Top";
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
        public static string ProjectStatus(int Status,string lang)
        {
            string result = "";
            switch (Status)
            {

                case 1:
                    result = "Đang triển khai";
                    break;
                case 2:
                    result = "Chờ duyệt";
                    break;


                default:
                    result = "Hoàn thành";
                    break;
            }
            if(lang!="vi-vn")
            {
                switch (Status)
                {

                    case 1:
                        result = "In Progress";
                        break;
                    case 2:
                        result = "Pending approval";
                        break;


                    default:
                        result = "Completed";
                        break;
                }
            }    
            return result;

        }
        public static string ProjectType(int Status, string lang)
        {
            string result = "";
            switch (Status)
            {

                case 1:
                    result = "Cải thiện khung pháp lý trong chuyển đổi năng lượng";
                    break;
                case 2:
                    result = "Chuyển đổi nhà máy điện than";
                    break;
                case 3:
                    result = "Phát triển ngành năng lượng tái tạo";
                    break;
                case 4:
                    result = "Truyền tải điện và lưu trữ năng lượng";
                    break;
                case 5:
                    result = "Sử dụng năng lượng hiệu quả";
                    break;
                case 6:
                    result = "Chuyển đổi năng lượng trong lĩnh vực giao thông vận tải";
                    break;
                case 7:
                    result = "Đổi mới, phát triển và chuyển giao công nghệ";
                    break;
                default:
                    result = "Đảm bảo quá trình chuyển đổi công bằng";
                    break;
            }
            if (lang != "vi-vn")
            {
                switch (Status)
                {

                    case 1:
                        result = "Improving the regulatory framework for the energy transition";
                        break;
                    case 2:
                        result = "The transition of coal power generation<";
                        break;
                    case 3:
                        result = "Developing the renewable energy industry";
                        break;
                    case 4:
                        result = "Power transmission and energy storage";
                        break;
                    case 5:
                        result = "Energy efficiency";
                        break;
                    case 6:
                        result = "Energy transition in the transportation sector";
                        break;
                    case 7:
                        result = "Innovation, development and technology transfer";
                        break;
                    default:
                        result = "Ensuring a just transition";
                        break;
                }
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
        public static string UserProjectType(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Dự án đầu tư có cấu phần xây dựng";
                    break;
                case 2:
                    result = "Dự án hỗ trợ kỹ thuật";
                    break;


            }
            return result;

        }
        public static string UserProjectTypeEn(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Asset Investment Project";
                    break;
                case 2:
                    result = "Technical assistance project";
                    break;


            }
            return result;

        }
        public static string UserProjectProgress(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Giai đoạn đề xuất ý tưởng";
                    break;
                case 2:
                    result = "Giai đoạn nghiên cứu tiền khả thi";
                    break;
                case 3:
                    result = "Giai đoạn nghiên cứu khả thi";
                    break;
                case 4:
                    result = "Giai đoạn thu mua đất";
                    break;
                case 5:
                    result = "Giai đoạn xin phép";
                    break;
                case 6:
                    result = "Giai đoạn thiết kế kỹ thuật (thiết kế chi tiết)";
                    break;
                case 7:
                    result = "Giai đoạn đàm phán tài chính";
                    break;
                case 8:
                    result = "Giai đoạn thương thảo và ký kết hợp đồng";
                    break;
                case 9:
                    result = "Giai đoạn khởi công xây dựng";
                    break;
                case 10:
                    result = "Giai đoạn hoàn thành xây dựng";
                    break;
                case 11:
                    result = "Giai đoạn vận hành thử nghiệm";
                    break;
                case 12:
                    result = "Giai đoạn vận hành chính thức và bảo trì/bảo dưỡng";
                    break;

            }
            return result;

        }
        public static string UserProjectProgressEn(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Conceptualization stage";
                    break;
                case 2:
                    result = "Pre-feasibility study stage";
                    break;
                case 3:
                    result = "Feasibility study stage";
                    break;
                case 4:
                    result = "Land acquisition stage<";
                    break;
                case 5:
                    result = "Permitting stage";
                    break;
                case 6:
                    result = "Technical design stage (detailed engineering design)";
                    break;
                case 7:
                    result = "Financial negotiation stage";
                    break;
                case 8:
                    result = "Contract negotiation and signing stage";
                    break;
                case 9:
                    result = "Construction commencement stage";
                    break;
                case 10:
                    result = "Construction completion stage";
                    break;
                case 11:
                    result = "Trial operation stage";
                    break;
                case 12:
                    result = "Commercial operation and maintenance stage";
                    break;

            }
            return result;

        }
        public static string UserProjectProgress2(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Giai đoạn đề xuất ý tưởng";
                    break;
                //case 2:
                //    result = "Giai đoạn nghiên cứu khả thi";
                //    break;
                case 2:
                    result = "Giai đoạn thiết kế kỹ thuật";
                    break;
                //case 4:
                //    result = "Giai đoạn huy động nguồn lực và Khởi động";
                //    break;
                case 3:
                    result = "Giai đoạn triển khai";
                    break;
                case 4:
                    result = "Giai đoạn hoàn thành và Kết thúc";
                    break;
              

            }
            return result;

        }
        public static string UserProjectProgress2En(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 1:
                    result = "Idea proposal stage";
                    break;
                //case 2:
                //    result = "Giai đoạn nghiên cứu khả thi";
                //    break;
                case 2:
                    result = "Technical design stage";
                    break;
                //case 4:
                //    result = "Giai đoạn huy động nguồn lực và Khởi động";
                //    break;
                case 3:
                    result = "Implementation stage";
                    break;
                case 4:
                    result = "Completion and closure stage";
                    break;


            }
            return result;

        }
        public static string UserProjectSubType(int Region,int Type)
        {
            string result = "";
            if(Type==1)
            {
                switch (Region)
                {

                    case 1:
                        result = "Các dự án về thúc đẩy chuyển đổi điện than sang năng lượng sạch";
                        break;
                    case 2:
                        result = "Các dự án về phát triển ngành công nghiệp năng lượng tái tạo";
                        break;
                    case 3:
                        result = "Sản xuất năng lượng tái tạo (gió, mặt trời, khác)";
                        break;
                    case 4:
                        result = "Sản xuất và sử dụng hydro xanh và các chất dẫn xuất";
                        break;
                    case 5:
                        result = "Các dự án về truyền tải điện và lưu trữ năng lượng";
                        break;
                    case 6:
                        result = "Các dự án về sử dụng năng lượng tiết kiệm và hiệu quả";
                        break;
                    case 7:
                        result = "Các dự án về chuyển đổi năng lượng xanh, giảm phát thải khí nhà kính của ngành giao thông vận tải";
                        break;

                }
            }
            if (Type == 2)
            {
                switch (Region)
                {

                    case 1:
                        result = "Các dự án hỗ trợ hoàn thiện thể chế, chính sách thúc đẩy chuyển đổi năng lượng, bao gồm nâng cao năng lực";
                        break;
                    case 2:
                        result = "Các dự án đổi mới sáng tạo, phát triển và chuyển giao công nghệ, hỗ trợ xây dựng dự án đầu tư";
                        break;
                    case 3:
                        result = "Các dự án thúc đẩy công bằng trong chuyển đổi năng lượng";
                        break;
                  

                }
            }

            return result;

        }
        public static string UserProjectSubTypeEn(int Region, int Type)
        {
            string result = "";
            if (Type == 1)
            {
                switch (Region)
                {

                    case 1:
                        result = "Projects accelerating the transition from coal power to clean energy";
                        break;
                    case 2:
                        result = "Projects supporting the development of the renewable energy industry";
                        break;
                    case 3:
                        result = "Renewable energy generation (wind, solar, others) projects";
                        break;
                    case 4:
                        result = "Projects on production and utilization of green hydrogen and its derivatives";
                        break;
                    case 5:
                        result = "Projects on power transmission and energy storage";
                        break;
                    case 6:
                        result = "Projects promoting energy efficiency and conservation";
                        break;
                    case 7:
                        result = "Projects on green energy transition and GHG emissions reduction in the transport sector";
                        break;

                }
            }
            if (Type == 2)
            {
                switch (Region)
                {

                    case 1:
                        result = "Projects supporting institutional and policy development for energy transition, including capacity building";
                        break;
                    case 2:
                        result = "Projects on innovation, technology development and transfer, and investment project preparation support";
                        break;
                    case 3:
                        result = "Projects promoting a just energy transition";
                        break;


                }
            }

            return result;

        }
    }
}