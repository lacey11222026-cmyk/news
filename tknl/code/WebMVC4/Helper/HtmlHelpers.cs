using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTILS;
using Constants = UTILS.Constants;
namespace WebMVC4.Helper
{
    public static class HtmlHelpers
    {
        public static int GetNumberQuestion(int id)
        {
            try
            {
                var obj = new TestQuestionBO().GetByRegistorId(id);
                if (obj == null)
                    return 0;
                return obj.Count();
            }
            catch
            {

                return 0;
            }
        }
        public static string GetTimeTest(DateTime? EndTime, DateTime? StartTime)
        {
            try
            {
                TimeSpan span = (EndTime.GetValueOrDefault() - StartTime.GetValueOrDefault());

                return String.Format("{0}:{1}",
                    span.Minutes + span.Hours * 60, span.Seconds);
            }
            catch
            {

                return "";
            }
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
        public static string GetActionType(int actionType)
        {
            switch (actionType)
            {

                case (int)Constants.NewsAction.Reject:
                    return "Trả lại biên tập";

                    break;
                case (int)Constants.NewsAction.Publish:

                    return "Xuất bản";
                    break;
                case (int)Constants.NewsAction.SendPublish:

                    return "Gửi thư ký tòa soạn";
                    break;
                case (int)Constants.NewsAction.Save:

                    return "Lưu lại tin";
                    break;
                case (int)Constants.NewsAction.SendEdit:

                    return "Gửi biên tập";
                    break;
                case (int)Constants.NewsAction.Delete:


                    return "Gỡ bài";
                    break;
            }
            return "Không xác định";
        }
        public static string GetNewsStatusName(int Status, List<EnumInfo> lstdata)
        {
            try
            {
                var obj = lstdata.Where(x => x.Value == Status).FirstOrDefault();
                if (obj == null)
                    return "";
                return obj.Text;
            }
            catch
            {

                return "N/A";
            }
        }
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
        public static string GetNewsCategoryName(int categoryId, List<CATEGORY_FULL> lstdata)
        {
            try
            {
                var obj = lstdata.Where(x => x.Id == categoryId).FirstOrDefault();
                if (obj == null)
                    return "N/A";
                var objParent = lstdata.Where(x => x.Id == obj.ParentId && x.Id>0).FirstOrDefault();
                if (objParent == null)
                    return obj.Name;
                return objParent.Name.Replace("-+", "") + "-->" + obj.Name.Replace("-+", "");
            }
            catch
            {

                return "N/A";
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

            var _childCategory = new CategoryBO().GetAllChildCategories(categoryId, 8, false);
            if (_childCategory != null && _childCategory.Count > 0)
                _childCategory = _childCategory.Where(x => x.Published == 1).ToList();
            var html = "";

            if (_childCategory != null && _childCategory.Count > 0)
            {

                html += "<ul>";
                var index = 0;
                foreach (CATEGORY_FULL _child in _childCategory)
                {
                    var url = Utils.FormatUrlRewriteByType(_child.Id, _child.Name, (int)_child.Type, _child.Link);

                    if (index == _childCategory.Count - 1)
                    {
                        html += "<li> <a href=\"" + url + "\">" + _child.Name + "</a></li>";
                    }
                    else
                    {
                        html += " <li><a  href=\"" + url + "\">" + _child.Name + "</a> </li>  ";
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
                case "newscreate":
                    result = "Phóng viên";
                    break;
                case "local":
                    result = "Phóng viên";
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
                case "sale":
                    result = "Quản trị hỏi đáp";
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
                    result = "Cuối";
                    break;
                case 2:
                    result = "Phả 1i";
                    break;
                case 3:
                    result = "Phải";
                    break;
                case 4:
                    result = "Giữa 1";
                    break;
                case 5:
                    result = "Giữa 2";
                    break;
                case 6:
                    result = "Giữa 3";
                    break;
                case 8:
                    result = "liên kết";
                    break;
                case 10:
                    result = "Phải2";
                    break;
                case 11:
                    result = "Phải3";
                    break;
                case 12:
                    result = "Phải4";
                    break;
                case 13:
                    result = "Phải5";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;

        }
        public static string GetNewsStatus(int? Region)
        {
            string result = "";
            switch (Region.GetValueOrDefault())
            {

                case 4:
                    result = "Đang biên tập";
                    break;
                case 2:
                    result = "Đợi xuất bản";
                    break;
                case 3:
                    result = "Đang viết";
                    break;
                case 1:
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
        public static string GetCategoryName(int id)
        {
            var cateobj = new CategoryBO().GetCategoryFull(id);
            if (cateobj != null)
                return cateobj.Name;

            return string.Empty;

        }

        public static string GetSourceName(string url)
        {
            if (url.Contains("dantri.com"))
                return "Báo Dân Trí";
            if (url.Contains("vtc.vn"))
                return "Báo VTC News";

            if (url.Contains("vietnamnet"))
                return "Báo Việt Nam Net";
            if (url.Contains("vnexpress.net"))
                return "Báo VN Express";

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
        public static string GetBanerType(int type)
        {
            switch (type)
            {

                case 1:
                    return "Ảnh";

                    break;
                case 2:

                    return "Flash";

            }
            return "Không xác định";
        }
        public static string GetBanerRegion(int region)
        {
            switch (region)
            {

                case 1:
                    return "Chính";

                    break;
                case 2:

                    return "Phải 1";
                    break;
                case 3:

                    return "Phải 2";
                    break;
                case 4:

                    return "Giữa 1";
                    break;
                case 5:

                    return "Giữa 2";
                    break;
                case 6:


                    return "Bottom";
                    break;
            }
            return "Không xác định";
        }
    }
}