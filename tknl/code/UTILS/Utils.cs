using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace UTILS
{
    public class Utils
    {
        public static string IntToLetters(int value)
        {
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char) ('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }
public static bool CheckValidMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return true;
            //if (CheckScriptTag(mobile))
            //    return false;
            string patternDienThoai = @"^[0]\d{9}$";

            Regex myRegexDienThoai = new Regex(patternDienThoai);

            Match mDienThoai = myRegexDienThoai.Match(mobile);

            if (!mDienThoai.Success)
            {
                return false;
            }

            return true;
        }
        public static int GetTelCo(string Mobile)
        {
            var prefix = SubstringByWord(Mobile, 3);
            if (!CheckValidMobile(Mobile))
                return 0;
            switch (prefix)
            {
                //VTT
                case "086":
                case "096":
                case "097":
                case "098":
                case "032":
                case "033":
                case "034":
                case "035":
                case "036":
                case "037":
                case "038":
                case "039":
                    return 1;
                //VMS
                case "089":
                case "090":
                case "093":
                case "070":
                case "079":
                case "077":
                case "076":
                case "078":


                    return 2;
                //VNP
                case "088":
                case "091":
                case "094":
                case "083":
                case "084":
                case "085":
                case "081":
                case "082":
                //VNM
                case "056":
                case "058":
                case "092":
                //GMB
                case "099":
                case "059":
                    return 3;
            }
            return 0;
        }
        public static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }
        public static string FormatKeywordSearch(string input)
        {
            var rt = RemoveAllHtmlTags(input);
            rt = RemoveSqlInjection(rt);
            rt = SubstringByWord(rt, 100);

            return rt.Replace("&", "%26").Trim();
        }
        public static string RemoveSqlInjection(string stringValue)
        {
            if (null == stringValue)
                return stringValue;
            stringValue = RegexReplace(stringValue, "-{2,}", "-");
            stringValue = RegexReplace(stringValue, @"[*/]+", string.Empty);
            stringValue = RegexReplace(stringValue, @"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
            return stringValue;

        }
        private static string RegexReplace(string stringValue, string matchPattern, string toReplaceWith)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith);
        }

        private static string RegexReplace(string stringValue, string matchPattern, string toReplaceWith, RegexOptions regexOptions)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith, regexOptions);
        }
        public static string RemoveAllHtmlTags(object input)
        {
            var output = string.Format("{0}", input);
            if (string.IsNullOrEmpty(output)) return string.Empty;

            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            output = HttpUtility.HtmlDecode(output);
            output = htmlRegex.Replace(output, string.Empty);
            return output.Replace("&", "%26").Trim();
        }
        public static string SubstringByWord(string input, int length)
        {
            string str = input.Trim();

            try
            {
                if (str.Length > length)
                {
                    str = str.Substring(0, length);
                    str = str.Substring(0, str.LastIndexOf(" "));
                }
            }
            catch { }

            return str;
        }
        public static string MD5Encrypt(string plainText)
        {
            UTF8Encoding encoding1 = new UTF8Encoding();
            MD5CryptoServiceProvider provider1 = new MD5CryptoServiceProvider();
            byte[] buffer1 = encoding1.GetBytes(plainText);
            byte[] buffer2 = provider1.ComputeHash(buffer1);
            return BitConverter.ToString(buffer2).Replace("-", "").ToLower();
        }
        public static string Base64Encode(object plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText.ToString());
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string FormatMoney(object input)
        {
            var m = Convert.ToInt32(input ?? "0");
            return m.ToString("#,#", CultureInfo.InvariantCulture).Replace(",", ".");
        }
        public static string SubString(string input, int len)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            if (!input.Contains(" ")) return input + "...";
            if (len > input.Length) return input;
            return input.Substring(0, input.Substring(0, len).LastIndexOf(" ")) + "...";
        }
        public static string FormatDateForDocument(DateTime? input)
        {
            if (input == null)
                return "";
            if (input.Value.Year >= 9999 || input.Value.Year <= 1900)
                return "";
            return input.Value.ToString("dd/MM/yyyy");
            return "";
        }

        public static string formatDateofWeek(DayOfWeek input)
        {
            switch (input)
            {
                case DayOfWeek.Monday:
                    return "Thứ hai";
                case DayOfWeek.Tuesday:
                    return "Thứ ba";
                case DayOfWeek.Wednesday:
                    return "Thứ tư";
                case DayOfWeek.Thursday:
                    return "Thứ năm";
                case DayOfWeek.Friday:
                    return "Thứ sáu";
                case DayOfWeek.Saturday:
                    return "Thứ bảy";


                default:
                    return "Chủ nhật";

            }
        }
        public static string StripHtmlTag(string input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(input);
            return doc.DocumentNode.InnerText;
        }
        public static string ConvertToRewriteLink(string text)
        {

            string strReturn = ReplaceVietnameseChar(text);

            // giữ lại dấu '-' và '_'
            strReturn = strReturn.Replace("-", " ").Replace("_", " ");

            // xóa các ký tự đặc biệt
            strReturn = ReplaceSpecificChar(strReturn, "-");

            strReturn = strReturn.Replace(" ", "-");
            strReturn = strReturn.Replace("--", "-");

            return strReturn.ToLower();
        }
        public static string ReplaceSpecificChar(string input, string repalce)
        {
            var pattern = @"[\W\s]";
            var output = Regex.Replace(input, pattern, repalce);
            output = output.Trim().Replace(" ", repalce);
            return output;
        }

        public static string FormatUrlRewriteByType(int id, string title, int categoryType, string link = "")
        {
            try
            {

                string entity = "Other";

                if (categoryType == (byte)UTILS.Constants.CategoryType.Product)
                {
                    entity = "Products";
                }
                else if (categoryType == (byte)UTILS.Constants.CategoryType.News)
                {
                    entity = "Articles";

                }
                else if (categoryType == (byte)UTILS.Constants.CategoryType.Album)
                {

                    entity = "Albums";
                }
                else if (categoryType == (byte)UTILS.Constants.CategoryType.Intro)
                {
                    entity = "Intro";


                }
                else if (categoryType == (byte)UTILS.Constants.CategoryType.Doc)
                {
                    entity = "Documents";


                }
                return FormatUrlRewrite(id, title, entity, link);
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "FormatUrlRewriteByType");
                return null;
            }
        }
        //public static string ATGTListCate = ConfigurationManager.AppSettings["ATGTListCate"] ?? ",0,";
        //public static string ATGTListCateName = ConfigurationManager.AppSettings["ATGTListCateName"] ?? ",an toàn giao thông,csgt kể chuyện,văn hóa giao thông,lái xe an toàn,gương sáng giao thông,";
        public static string StiteUrl = ConfigurationManager.AppSettings["SiteUrl"] ?? "http://tietkiemnangluong.com.vn/";
        //public static string ATGT_SiteUrl = ConfigurationManager.AppSettings["ATGT_SiteUrl"] ?? "http://atgt.tietkiemnangluong.com.vn/";
        public static string FormatUrlRewrite(long id, string title, string entity, string link = "")
        {
            try
            {
                string _url = "/";
                var isEnableURLRewrite = int.Parse(ConfigurationManager.AppSettings["EnableURLRewrite"].ToString());
                var _title = ConvertToRewriteLink(title);
                switch (entity)
                {
                    case "Documents":

                        _url += "van-ban.html";

                        return _url;
                    case "DocumentDetail":

                        _url += "Document/Detail/" + id + "";

                        return _url;
                    case "Default":
                        if (isEnableURLRewrite > 0)
                        {

                            _url += "home.html";
                        }
                        else
                        {
                            _url += entity + ".aspx";
                        }
                        return _url;
                    case "Error":
                        if (isEnableURLRewrite > 0)
                        {

                            _url += "error.html";
                        }
                        else
                        {
                            _url += entity + ".aspx";
                        }
                        return _url;
                    
                    case "ArticleDetail":
                        if (isEnableURLRewrite > 0)
                        {
                            //if (ATGTListCateName.Contains("," + link.ToLower() + ","))
                            //{
                            //    _url = ATGT_SiteUrl + "tin-tuc/" + ConvertToRewriteLink(link) + "/t" + id + "/" + _title + ".html";

                            //}
                            //else
                            //{
                                _url = StiteUrl + "tin-tuc/" + ConvertToRewriteLink(link) + "/t" + id + "/" + _title ;
                            //}

                        }
                        else
                        {
                            _url += entity + ".aspx?aid=" + id + "";
                        }
                        return _url;
                    case "Articles":
                        if (isEnableURLRewrite > 0)
                        {
                            if (id > 0)
                            {
                                //if (ATGTListCate.Contains("," + id + ","))
                                //{
                                //    _url = ATGT_SiteUrl + "tin-tuc/c" + id + "/" + _title + ".html";
                                //    if (id == 3)
                                //    {
                                //        _url = ATGT_SiteUrl;
                                //    }
                                //}
                                //else
                                //{
                                    _url = StiteUrl + "tin-tuc/c" + id + "/" + _title ;
                                //}




                            }
                            else
                                _url += "tin-tuc.htmll";
                        }
                        else
                        {
                            if (id > 0)
                                _url += entity + ".aspx?cid=" + id + "";
                            else
                                _url += entity + ".aspx";
                        }
                        return _url;


                    case "Intro":
                        if (isEnableURLRewrite > 0)
                        {
                            if (id > 0)
                                _url += "gioi-thieu/c" + id + "/" + _title ;
                            else
                                _url += "gioithieu.html";
                        }
                        else
                        {
                            if (id > 0)
                                _url += entity + ".aspx?cid=" + id + "";
                            else
                                _url += entity + ".aspx";
                        }
                        return _url;
                    default:
                        return link;
                }

                return _url;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "FormatUrlRewrite");
                return null;
            }
        }


        public static string GetAppSettingValue(string key)
        {
            try
            {
                var _value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(_value))
                    return string.Empty;

                return _value;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "GetAppSettingValue");
                return null;
            }
        }

        public static bool SetAppSettingValue(string key, string value, string path)
        {
            try
            {

                // Open App.Config of executable
                System.Configuration.Configuration config =

                  WebConfigurationManager.OpenWebConfiguration(path);

                AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");

                // Add an Application Setting.
                appSettings.Settings.Remove(key);
                appSettings.Settings.Add(key, value);

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Full);

                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "SetAppSettingValue");
                return false;
            }


        }

        public static bool CheckNew(DateTime createDate)
        {
            try
            {
                var newProductDays = Convert.ToInt32(ConfigurationManager.AppSettings["NewProductDayCount"]);
                if ((DateTime.Now - createDate).Days <= newProductDays)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string FormatTextValue(string txtValue)
        {
            try
            {
                if (string.IsNullOrEmpty(txtValue))
                    return string.Empty;

                if (IsNumber(txtValue))
                    return txtValue;

                var _textValue = txtValue;
                _textValue = _textValue.ToLower().Trim();

                char[] delimiterChars = { ' ', '\t' };

                string[] words = _textValue.Split(delimiterChars);

                var newText = string.Empty;
                foreach (string s in words)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    newText += ToUpperFirstChar(s) + ' ';
                }

                newText = newText.Trim();

                return newText;
            }
            catch (Exception)
            {
                return txtValue;
            }

        }

        public static string ToUpperFirstChar(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        //public static string ToUpperFirstChar(string input)
        //{
        //    if (string.IsNullOrEmpty(input))
        //        return input;

        //    string output = input;
        //    string _firtChar = output.Substring(0, 1);
        //    _firtChar = _firtChar.ToUpper();
        //    return _firtChar + output.Remove(0, 1);
        //}

        public static string FormatLengthString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var output = input;
            if (output.Length <= maxLength)
                return output;

            output = input.Substring(0, maxLength - 6) + " ... ";
            return output;
        }


        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 20/09/2011 01:59 AM
        /// todo: get image url by rule
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="isThumb">if set to <c>true</c> [is thumb].</param>
        /// <returns></returns>
        /// 

        public static string GetImageUrl(long id, string entity, bool isThumb)
        {
            if (id == 0 || string.IsNullOrEmpty(entity))
                //return "/Images/nophoto.jpg";
                return ConfigurationManager.AppSettings["NoPhotoUrl"];

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(ConfigurationManager.AppSettings["UploadUrl"]).Append(entity).Append("/").Append(Convert.ToInt32(id) / 100000).Append("/").Append(Convert.ToInt32(id) / 100).Append("/").Append(id).Append("/");

            if (isThumb)
                strBuilder.Append("Thumb/");

            return strBuilder.ToString();
        }
        public static string GetTempUrl(string username)
        {
            if (string.IsNullOrEmpty(username))
                //return "/Images/nophoto.jpg";
                return ConfigurationManager.AppSettings["NoPhotoUrl"];

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(ConfigurationManager.AppSettings["UploadUrl"]).Append("/Temp/").Append(username).Append("/");



            return strBuilder.ToString();
        }
        public static string SeparateDouble(double _double, string separation)
        {
            //var _price = price;
            if (_double <= 0)
                return "0";

            var _separation = separation;
            if (string.IsNullOrEmpty(separation))
                _separation = ".";

            var strDouble = _double.ToString();
            var length = strDouble.Length;

            for (var i = length; i > 0; i = i - 3)
            {
                if (i != length)
                    strDouble = strDouble.Insert(i, _separation);
            }

            return strDouble;
        }
        public static string ReplaceWordChars( string text)
        {

            var s = text;

            // smart single quotes and apostrophe

            s = Regex.Replace(s, "[\u2018\u2019\u201A]", "'");

            // smart double quotes

            s = Regex.Replace(s, "[\u201C\u201D\u201E]", "\"");

            // ellipsis

            s = Regex.Replace(s, "\u2026", "...");

            // dashes

            s = Regex.Replace(s, "[\u2013\u2014]", "-");

            // circumflex

            s = Regex.Replace(s, "\u02C6", "^");

            // open angle bracket

            s = Regex.Replace(s, "\u2039", "<");

            // close angle bracket

            s = Regex.Replace(s, "\u203A", ">");

            // spaces

            s = Regex.Replace(s, "[\u02DC\u00A0]", " ");



            return s;

        }
        public static int GetNumberChac(string content)
        {
            try
            {
                content =ReplaceWordChars(RemoveAllHtmlTags(content)) ;
                return content.Split(' ').Length;
            }
            catch
            {

                return 0;
            }
        }
        public static string ConvertDoubleToVND(double _double, int roundUnit, string unit)
        {
            //var _price = price;
            if (_double <= 0)
                return "0";

            var _unit = unit;
            if (string.IsNullOrEmpty(unit))
                _unit = " triệu";

            return Math.Ceiling(_double / roundUnit).ToString() + _unit;
        }



        public static string GetNewsImagePath(long id, string EntityName = "Article")
        {

            StringBuilder strBuilder = new StringBuilder();
            // divided 1000000 files in folder               
            strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(EntityName).Append("\\").Append(Convert.ToInt32(id) / 100000).Append("\\").Append(Convert.ToInt32(id) / 100).Append("\\").Append(id).Append("\\");
            var upload_path = strBuilder.ToString();
            return upload_path;
        }

        public static string GetTempPath(string username)
        {

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append("\\Temp\\").Append(username).Append("\\");

            return strBuilder.ToString();
        }
        public static string GetEditorPath(string username)
        {

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append("User\\").Append(username).Append("\\").Append(DateTime.Now.Year.ToString()).Append("\\").Append(DateTime.Now.Month.ToString()).Append("\\").Append(DateTime.Now.Day.ToString()).Append("\\");

            return strBuilder.ToString();
        }

        public static string MoveFile(string from_path, string from_file, string to_path, string to_file)
        {
            try
            {
                if (!Directory.Exists(from_path))
                    Directory.CreateDirectory(from_path);

                if (!Directory.Exists(to_path))
                    Directory.CreateDirectory(to_path);
                Directory.Move(from_path + from_file, to_path + to_file);
                var strReturn = "success|" + to_file;

                return strReturn;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "MoveFile");
                return "fail|exception";
            }
        }


        /// created by: manhcuong.phung 
        /// date: 18/08/2011 02:16 PM
        /// todo: upload http posted file , using rules folder divied for upload path, return file url
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">The id.</param>
        /// <param name="entityName"></param>
        /// <param name="file_ext">The file_ext.</param>
        /// <param name="file_prefix">The file_prefix.</param>
        /// <param name="hasThumb"></param>
        /// <returns></returns>
        public static string Upload(HttpRequest request, int id, string entityName, string file_ext, string file_prefix, bool hasThumb)
        {
            try
            {
                if (request == null)
                    return "fail|request_error";

                HttpPostedFile httpPostedFile = request.Files[0];
                //int id = request
                if (httpPostedFile == null)
                    return "fail|file_not_exist";

                var allow_content_type = ConfigurationManager.AppSettings["ImageContenType"];
                if (!string.IsNullOrEmpty(allow_content_type))
                {
                    var content_type = httpPostedFile.ContentType;
                    if (allow_content_type.IndexOf(content_type) == -1)
                        return "fail|denied_content_type";
                }

                //if ( id <= 0 )
                // return "fail|id_not_exist";

                StringBuilder strBuilder = new StringBuilder();
                if (id > 0)
                {
                    // divided 1000000 files in folder               
                    strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(entityName).Append("\\").Append(id / 100000).Append("\\").Append(id / 100).Append("\\").Append(id).Append("\\");
                }

                string upload_path = strBuilder.ToString();
                //sring thumb_upload_path = upload_path + "thumb\\";

                // if folder not exist => create folder follow rule
                if (!Directory.Exists(upload_path))
                    Directory.CreateDirectory(upload_path);

                // full file upload path
                //var numfile = 1;
                //var files = Directory.GetFiles(upload_path)  ;
                //if (files != null)
                //{

                //    try
                //    {
                //        var lastfile = files[files.Length - 1].Split('\\');
                //        var lastname = lastfile[lastfile.Length - 1].Split('.');
                //        numfile = int.Parse(lastfile[lastfile.Length - 1].Split('.')[0]) + 1;

                //    }
                //    catch
                //    {

                //        numfile = 1;
                //    }
                //}
                //var file_name = numfile.ToString() ;
                var file_name = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();
                strBuilder = new StringBuilder();
                strBuilder.Append(file_name).Append(".").Append(file_ext);

                var file_path = upload_path + strBuilder;
                // save posted file to server                
                httpPostedFile.SaveAs(file_path);

                // create thumbnail
                //if (hasThumb)
                //{
                //    if (!Directory.Exists(thumb_upload_path))
                //        Directory.CreateDirectory(thumb_upload_path);

                //    string file_thumb_path = thumb_upload_path + strBuilder;
                //    var thumbnail = GenerateThumbnail(file_path);
                //    thumbnail.Save(file_thumb_path);
                //}

                var strReturn = "success|" + file_name + "." + file_ext;

                return strReturn;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Upload");
                return "fail|exception";
            }
        }
        public static string UploadURL(HttpRequest request, string url, string entityName, string file_ext, string file_prefix, bool hasThumb)
        {
            try
            {
                if (request == null)
                    return "fail|request_error";

                HttpPostedFile httpPostedFile = request.Files[0];
                //int id = request
                if (httpPostedFile == null)
                    return "fail|file_not_exist";

                var allow_content_type = ConfigurationManager.AppSettings["ImageContenType"];
                if (!string.IsNullOrEmpty(allow_content_type))
                {
                    var content_type = httpPostedFile.ContentType;
                    if (allow_content_type.IndexOf(content_type) == -1)
                        return "fail|denied_content_type";
                }

                //if ( id <= 0 )
                // return "fail|id_not_exist";

                StringBuilder strBuilder = new StringBuilder();


                // divided 1000000 files in folder               
                //strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(url).Append("\\");


                string upload_path = url;
                string thumb_upload_path = upload_path + "thumb\\";

                // if folder not exist => create folder follow rule
                if (!Directory.Exists(upload_path))
                    Directory.CreateDirectory(upload_path);

                // full file upload path

                //var file_name = numfile.ToString();
                var file_name = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();
                strBuilder = new StringBuilder();
                strBuilder.Append(file_name).Append(".").Append(file_ext);

                var file_path = upload_path + strBuilder;
                // save posted file to server                
                httpPostedFile.SaveAs(file_path);

                // create thumbnail
                //if (hasThumb)
                //{
                //    if (!Directory.Exists(thumb_upload_path))
                //        Directory.CreateDirectory(thumb_upload_path);

                //    string file_thumb_path = thumb_upload_path + strBuilder;
                //    var thumbnail = GenerateThumbnail(file_path);
                //    thumbnail.Save(file_thumb_path);
                //}

                var strReturn = "success|" + file_name + "." + file_ext;

                return strReturn;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "UploadURL");
                return "fail|exception";
            }
        }

        public static string UploadBanner(HttpRequest request, string entityName, string position, string file_ext, bool hasThumb)
        {
            try
            {
                if (request == null)
                    return "fail|request_error";

                HttpPostedFile httpPostedFile = request.Files[0];
                //int id = request
                if (httpPostedFile == null)
                    return "fail|file_not_exist";

                var allow_content_type = ConfigurationManager.AppSettings["ImageContenType"];
                if (!string.IsNullOrEmpty(allow_content_type))
                {
                    var content_type = httpPostedFile.ContentType;
                    if (allow_content_type.IndexOf(content_type) == -1)
                        return "fail|denied_content_type";
                }

                StringBuilder strBuilder = new StringBuilder();

                // divided 1000000 files in folder               
                strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(entityName).Append("\\");


                string upload_path = strBuilder.ToString();
                string thumb_upload_path = upload_path + "thumb\\";

                // if folder not exist => create folder follow rule
                if (!Directory.Exists(upload_path))
                    Directory.CreateDirectory(upload_path);

                // full file upload path
                var file_name = position;
                strBuilder = new StringBuilder();
                strBuilder.Append(file_name).Append(".").Append(file_ext);

                var file_path = upload_path + strBuilder;
                // delete current file 
                if (File.Exists(file_path))
                    File.Delete(file_path);
                // save posted file to server                
                httpPostedFile.SaveAs(file_path);

                // create thumbnail
                if (hasThumb)
                {
                    if (!Directory.Exists(thumb_upload_path))
                        Directory.CreateDirectory(thumb_upload_path);

                    string file_thumb_path = thumb_upload_path + strBuilder;
                    var thumbnail = GenerateThumbnail(file_path);
                    thumbnail.Save(file_thumb_path);
                }

                var strReturn = "success|" + file_name + "." + file_ext;

                return strReturn;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "UploadBanner");
                return "fail|exception";
            }
        }


        public static string DeleteFiles(HttpRequest request, int id, string entityName)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                // divided 1000000 files in folder               
                strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(entityName).Append("\\").Append(id / 100000).Append("\\").Append(id / 100).Append("\\").Append(id).Append("\\");

                string upload_path = strBuilder.ToString();
                //string thumb_upload_path = upload_path + "thumb\\";

                // if folder not exist => create folder follow rule
                if (!Directory.Exists(upload_path))
                    return "fail";
                //if (!Directory.Exists(thumb_upload_path))
                //    return "fail";

                // full file upload path               
                var file_path = upload_path;
                //var file_thumb_path = thumb_upload_path + fileName;

                // delete
                Directory.Delete(file_path, true);
                //File.Delete ( file_thumb_path );
                //var thumbnail = Crop ( file_path, 100, 100, 50, 50 );
                //thumbnail.Save ( file_thumb_path );              
                return "success";
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "DeleteFiles");
                return "fail";
            }
        }

        public static string DeleteFilePath(HttpRequest request, string path, string fileName, string entityName)
        {
            try
            {
                //StringBuilder strBuilder = new StringBuilder();
                // divided 1000000 files in folder               
                //strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(entityName).Append("\\").Append(id / 100000).Append("\\").Append(id / 100).Append("\\").Append(id).Append("\\");

                string upload_path = path;
                string thumb_upload_path = upload_path + "thumb\\";

                // if folder not exist => create folder follow rule
                if (!Directory.Exists(upload_path))
                    return "fail";
                //if (!Directory.Exists(thumb_upload_path))
                //    return "fail";

                // full file upload path               
                var file_path = upload_path + fileName;
                var file_thumb_path = thumb_upload_path + fileName;

                // delete
                File.Delete(file_path);
                //delte ảnh thumb
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(upload_path);
                foreach (System.IO.FileInfo file in directory.GetFiles())
                {
                    if (file.Name.Contains(fileName + "."))
                    {
                        file.Delete();
                    }

                }

                return "success";
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Utils", "DeleteFile:entityName= " + entityName + " |fileName= " + fileName);
                return "fail";
            }
        }
        public static string DeleteFile(HttpRequest request, int id, string fileName, string entityName)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                // divided 1000000 files in folder               
                strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append(entityName).Append("\\").Append(id / 100000).Append("\\").Append(id / 100).Append("\\").Append(id).Append("\\");

                string upload_path = strBuilder.ToString();
                string thumb_upload_path = upload_path + "thumb\\";

                // if folder not exist => create folder follow rule
                if (!Directory.Exists(upload_path))
                    return "fail";
                //if (!Directory.Exists(thumb_upload_path))
                //    return "fail";

                // full file upload path               
                var file_path = upload_path + fileName;
                var file_thumb_path = thumb_upload_path + fileName;

                // delete
                File.Delete(file_path);
                //delte ảnh thumb
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(upload_path);
                foreach (System.IO.FileInfo file in directory.GetFiles())
                {
                    if (file.Name.Contains(fileName + "."))
                    {
                        file.Delete();
                    }

                }

                return "success";
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Utils", "DeleteFile:entityName= " + entityName + " |fileName= " + fileName + " | id= " + id);
                return "fail";
            }
        }



        public static Image Crop(string img, int width, int height, int x, int y)
        {
            try
            {
                Image image = Image.FromFile(img);
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                bmp.SetResolution(80, 60);

                Graphics gfx = Graphics.FromImage(bmp);
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gfx.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
                // Dispose to free up resources
                image.Dispose();
                bmp.Dispose();
                gfx.Dispose();

                return bmp;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ImageCrop");
                return null;
            }
        }

        public static Image GenerateThumbnail(string file_path)
        {
            try
            {
                Image image = null;
                // Check if textbox has a value
                if (!string.IsNullOrEmpty(file_path))
                    image = Image.FromFile(file_path);
                // Check if image exists)
                if (image != null)
                {
                    var width = 200;
                    var height = 200;

                    if (image.Width < 200)
                        width = image.Width;


                    if (image.Height < 200)
                        height = image.Height;


                    var percent = ((float)image.Width / (float)image.Height);

                    if (percent > 1)
                    {

                        height = (int)Math.Ceiling((float)width / percent);
                    }
                    else
                    {
                        width = (int)Math.Ceiling(height * percent);

                    }

                    return image.GetThumbnailImage(width, height, null, new IntPtr());
                }

                return null;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "GenerateThumbnail");
                return null;
            }
        }

        //public void GenerateThumbnail()
        //{
        //    var bitmap;
        //    try
        //    {
        //        bitmap = new Bitmap(newWidth, newHeight);
        //        using (Graphics g = Graphics.FromImage(bitmap))
        //        {
        //            g.SmoothingMode = SmoothingMode.HighQuality;
        //            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //            g.CompositingQuality = CompositingQuality.HighQuality;
        //            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            g.DrawImage(oldImage,
        //                new Rectangle(0, 0, newWidth, newHeight),
        //                clipRectangle, GraphicsUnit.Pixel);
        //        }
        //        //done with drawing on "g"
        //        return bitmap;//IDisposable
        //    }
        //    catch
        //    {
        //        if (bitmap != null) bitmap.Dispose();
        //        throw;
        //    }
        //}

        public Stream GenerateThumbnail(Stream stream, string ext)
        {
            try
            {
                Image image = null;

                // Check if textbox has a value
                if (stream != null)
                {
                    image = System.Drawing.Image.FromStream(stream, true, true);
                    //ImageConverter imageConverter = new System.Drawing.ImageConverter();
                    //image = imageConverter.ConvertFrom(stream) as Image;

                    //stream.Seek(0, SeekOrigin.Begin);
                    //image = Image.FromStream(stream);
                }

                // Check if image exists)
                if (image != null)
                {
                    var width = 100;

                    if (image.Width < 100)
                        width = image.Width;

                    var height = 100;
                    if (image.Height < 100)
                        height = image.Height;

                    Image newImage = image.GetThumbnailImage(width, height, null, new IntPtr());

                    ImageFormat imageFormat;
                    switch (ext)
                    {
                        case "jpg":
                            imageFormat = ImageFormat.Jpeg;
                            break;
                        case "bmp":
                            imageFormat = ImageFormat.Bmp;
                            break;
                        case "png":
                            imageFormat = ImageFormat.Png;
                            break;
                        case "gif":
                            imageFormat = ImageFormat.Gif;
                            break;
                        case "ico":
                            imageFormat = ImageFormat.Icon;
                            break;
                        default:
                            imageFormat = ImageFormat.Jpeg;
                            break;
                    }

                    var _stream = ToStream(newImage, imageFormat);
                    return _stream;
                }



                return null;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "GenerateThumbnail");
                return null;
            }
        }

        protected Stream ToStream(Image image, ImageFormat formaw)
        {
            var __stream = new System.IO.MemoryStream();
            image.Save(__stream, formaw);
            __stream.Position = 0;
            return __stream;
        }


        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 07/04/2011 04:40 AM
        /// todo: convert a object to json format
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static string ConvertToJson(object obj, string objName)
        {
            JavaScriptDateTimeConverter datetimeformat = new JavaScriptDateTimeConverter();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (!string.IsNullOrEmpty(objName))
                    stringBuilder.Append("{").Append(objName).Append(":");

                stringBuilder.Append(JsonConvert.SerializeObject(obj, datetimeformat));

                if (!string.IsNullOrEmpty(objName))
                    stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ConvertToJson");
                return string.Empty;
            }
        }
        public static DateTime ConvertToDate(string strDate, string format)
        {

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = format;
            DateTime objDate = Convert.ToDateTime(strDate, dtfi);
            return objDate;
        }

        public static double ConvertToDateTimeStamp(DateTime inputDateTime)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (inputDateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return span.TotalSeconds;
        }

        public static bool IsNumber(string value)
        {
            int number1;
            return int.TryParse(value, out number1);
        }

        public static bool IsFloat(string value)
        {
            float number1;
            return float.TryParse(value, out number1);
        }

        public static Dictionary<string, string> ParseStringJson(string json)
        {
            Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dictionary;
        }



        /// <summary>
        /// Created by:
        /// Date:
        /// Description : remove vietnamese char to latin char
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceVietnameseChar(string s)
        {
            if (s == null)
                return String.Empty;
            // replace specification character
            s = s.Trim().ToLower();
            s = s.Replace('á', 'a');
            s = s.Replace('à', 'a');
            s = s.Replace('ả', 'a');
            s = s.Replace('ã', 'a');
            s = s.Replace('ạ', 'a');
            s = s.Replace('ă', 'a');
            s = s.Replace('ắ', 'a');
            s = s.Replace('ằ', 'a');
            s = s.Replace('ẳ', 'a');
            s = s.Replace('ẵ', 'a');
            s = s.Replace('ặ', 'a');
            s = s.Replace('â', 'a');
            s = s.Replace('ấ', 'a');
            s = s.Replace('ầ', 'a');
            s = s.Replace('ẩ', 'a');
            s = s.Replace('ẫ', 'a');
            s = s.Replace('ậ', 'a');
            s = s.Replace('é', 'e');
            s = s.Replace('è', 'e');
            s = s.Replace('ẻ', 'e');
            s = s.Replace('ẽ', 'e');
            s = s.Replace('ẹ', 'e');
            s = s.Replace('ê', 'e');
            s = s.Replace('ế', 'e');
            s = s.Replace('ề', 'e');
            s = s.Replace('ể', 'e');
            s = s.Replace('ễ', 'e');
            s = s.Replace('ệ', 'e');
            s = s.Replace('í', 'i');
            s = s.Replace('ì', 'i');
            s = s.Replace('ỉ', 'i');
            s = s.Replace('ĩ', 'i');
            s = s.Replace('ị', 'i');
            s = s.Replace('ó', 'o');
            s = s.Replace('ò', 'o');
            s = s.Replace('ỏ', 'o');
            s = s.Replace('õ', 'o');
            s = s.Replace('ọ', 'o');
            s = s.Replace('ô', 'o');
            s = s.Replace('ố', 'o');
            s = s.Replace('ồ', 'o');
            s = s.Replace('ổ', 'o');
            s = s.Replace('ỗ', 'o');
            s = s.Replace('ộ', 'o');
            s = s.Replace('ơ', 'o');
            s = s.Replace('ớ', 'o');
            s = s.Replace('ờ', 'o');
            s = s.Replace('ở', 'o');
            s = s.Replace('ỡ', 'o');
            s = s.Replace('ợ', 'o');
            s = s.Replace('ú', 'u');
            s = s.Replace('ù', 'u');
            s = s.Replace('ủ', 'u');
            s = s.Replace('ũ', 'u');
            s = s.Replace('ụ', 'u');
            s = s.Replace('ư', 'u');
            s = s.Replace('ứ', 'u');
            s = s.Replace('ừ', 'u');
            s = s.Replace('ử', 'u');
            s = s.Replace('ữ', 'u');
            s = s.Replace('ự', 'u');
            s = s.Replace('ý', 'y');
            s = s.Replace('ỳ', 'y');
            s = s.Replace('ỷ', 'y');
            s = s.Replace('ỹ', 'y');
            s = s.Replace('ỵ', 'y');
            s = s.Replace('đ', 'd');
            return s;
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
        public static string EmbedAudio(string url, string width, string height)
        {
            var result = "";
            result += "<embed width=\"" + width + "\" height=\"" + height + "\"";
            result += "flashvars=\"file=" + url + "&amp;volume=60&amp;repeat=false&amp;bufferlength=10&amp;";
            result += "\"";
            result += " allowscriptaccess=\"always\" allowfullscreen=\"true\" wmode=\"transparent\" quality=\"hight\"";
            result += "src=\"/flash/flvplayer.swf\" type=\"application/x-shockwave-flash\" name=\"flvplayer\" id=\"flvplayer\"></embed> ";
            return result;
        }
        
        //public static string EmbedAudio(string url, string width, string height)
        //{
        //    var result = "";
        //    result += "<object width=\"" + width + "\" height=\"" + height + "\" name=\"dewplayerbub\" id=\"dewplayerbub\" ";
        //    result += "data=\"/flash/dewplayer-bubble.swf\" type=\"application/x-shockwave-flash\">";
        //    result += "<param value=\"/flash/dewplayer-bubble.swf\" name=\"movie\"><param value=\"mp3=" + url + "\" name=\"flashvars\"></object>";


        //    return result;
        //}
    }

    public class Global
    {
        private static Configuration _configuration;

        public static Configuration Configuration
        {
            get
            {
                //not Setting class is not init
                if (_configuration == null)
                    _configuration = new Configuration();
                return _configuration;
            }
        }
    }

}
