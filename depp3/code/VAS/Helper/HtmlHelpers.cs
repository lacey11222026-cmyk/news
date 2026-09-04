using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTILS;
using Constants = UTILS.Constants;
namespace VAS.Helper
{
    public static class HtmlHelpers
    {
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
                    userRoles += " | " + r;
                }
                userRoles = userRoles.Remove(0, 2);
                return userRoles;
            }

            return string.Empty;

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