using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Car.Utility
{
    public static class StringUtils
    {
        public static string FomatSMSContent(string Content)
        {

            if (Content.Contains("{"))
            {
                string pattern = @"\{(.*?)}";
                var matchList = Regex.Matches(Content, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matchList)
                {
                    if (!match.Success) continue;

                    Content = Content.Replace("{" + match.Groups[1].Value + "}", GetRandomList(match.Groups[1].Value, '|'));
                }
            }


            return Content + GetRandomList(Config.EndSMSText, '|');

        }
        public static string GetRandomList(string Content, char text)
        {
            var arr = Content.Split(text).Where(x => !String.IsNullOrEmpty(x)).ToList();
            var random = new Random();
            int index = random.Next(arr.Count);
            return arr[index];
        }
        public static string FormatTelCo(string Mobile)
        {
            if(Mobile.StartsWith("84"))
            {
                Mobile = Mobile.Substring(2);
                Mobile = $"0{Mobile}";
            }
            if (Mobile.StartsWith("+84"))
            {
                Mobile = Mobile.Substring(3);
                Mobile = $"0{Mobile}";
            }
            //VTT
            if (Mobile.StartsWith("0162"))
            {
                Mobile = Mobile.Substring(4);
                return   $"032{Mobile}" ;
                
            }
            if (Mobile.StartsWith("0163"))
            {
                Mobile = Mobile.Substring(4);
                return $"033{Mobile}";
            }
            if (Mobile.StartsWith("0164"))
            {
                Mobile = Mobile.Substring(4);
                return $"034{Mobile}";
            }
            if (Mobile.StartsWith("0165"))
            {
                Mobile = Mobile.Substring(4);
                return $"035{Mobile}";
            }
            if (Mobile.StartsWith("0166"))
            {
                Mobile = Mobile.Substring(4);
                return $"036{Mobile}";
            }
            if (Mobile.StartsWith("0167"))
            {
                Mobile = Mobile.Substring(4);
                return $"037{Mobile}";
            }
            if (Mobile.StartsWith("0168"))
            {
                Mobile = Mobile.Substring(4);
                return $"038{Mobile}";
            }
            if (Mobile.StartsWith("0169"))
            {
                Mobile = Mobile.Substring(4);
                return $"039{Mobile}";
            }

            //vms
            if (Mobile.StartsWith("0120"))
            {
                Mobile = Mobile.Substring(4);
                return $"070{Mobile}";
            }
            if (Mobile.StartsWith("0121"))
            {
                Mobile = Mobile.Substring(4);
                return $"079{Mobile}";
            }
            if (Mobile.StartsWith("0122"))
            {
                Mobile = Mobile.Substring(4);
                return $"077{Mobile}";
            }
           
            if (Mobile.StartsWith("0126"))
            {
                Mobile = Mobile.Substring(4);
                return $"076{Mobile}";
            }
            if (Mobile.StartsWith("0128"))
            {
                Mobile = Mobile.Substring(4);
                return $"078{Mobile}";
            }
            //vnp
            if (Mobile.StartsWith("0123"))
            {
                Mobile = Mobile.Substring(4);
                return $"083{Mobile}";
            }
            if (Mobile.StartsWith("0124"))
            {
                Mobile = Mobile.Substring(4);
                return $"084{Mobile}";
            }
            if (Mobile.StartsWith("0125"))
            {
                Mobile = Mobile.Substring(4);
                return $"085{Mobile}";
            }
            if (Mobile.StartsWith("0127"))
            {
                Mobile = Mobile.Substring(4);
                return $"081{Mobile}";
            }
            if (Mobile.StartsWith("0129"))
            {
                Mobile = Mobile.Substring(4);
                return $"082{Mobile}";
            }
            return Mobile;
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

                    return 3;
            }
            return 0;
        }
        public static string GetTelCoName(int Telco)
        {
            switch (Telco)

            {
                case 1: return "VTT";
                case 2: return "VMS";
                case 3: return "VNP";
            }
            return "";
        }
        private static string RegexReplace(string stringValue, string matchPattern, string toReplaceWith, RegexOptions regexOptions)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith, regexOptions);
        }
        private static string RegexReplace(string stringValue, string matchPattern, string toReplaceWith)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith);
        }
        public static string RemoveAllHtmlTags(object input)
        {
            var output = string.Format("{ 0}", input);
            if (string.IsNullOrEmpty(output)) return string.Empty;

            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            output = HttpUtility.HtmlDecode(output);
            output = htmlRegex.Replace(output, string.Empty);
            return output.Replace("&", "%26").Trim();
        }
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
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
        public static string FormatKeywordSearch(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            var rt = RemoveAllHtmlTags(input);
            rt = RemoveSqlInjection(rt);
            rt = SubstringByWord(rt, 512);

            return rt.Replace(" & ", "%26").Trim();
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
        public static string RemoveScript(string stringValue)
        {
            //Regex regxScriptRemoval = new Regex(@"<script(.+?)*</script>");
            if (string.IsNullOrEmpty(stringValue))
                return stringValue;
            stringValue = RegexReplace(stringValue, @"<script(.+?)*</script>", string.Empty, RegexOptions.IgnoreCase);
            return RemoveSqlInjection(stringValue);

        }
        public static bool CheckScriptTag(string Output)
        {
            Output = WebUtility.HtmlDecode(Output);
            string pattern = @"^(?:(?!<[^>]*>).)*$";
            var myRegex = new Regex(pattern);
            var m = myRegex.Match(Output);
            if (m.Success)
            {
                return false;
            }

            return true;
        }
        public static bool CheckPassport(string Passport)
        {
            if (string.IsNullOrEmpty(Passport))
                return true;
            if (CheckScriptTag(Passport))
                return false;
            string pattern = @"^[0-9A-Za-z]{8,15}$";
            var myRegex = new Regex(pattern);
            var m = myRegex.Match(Passport);
            if (m.Success)
            {
                return true;
            }

            return false;
        }
        public static bool CheckValidMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return true;
            if (CheckScriptTag(mobile))
                return false;
            string patternDienThoai = @"^[0]\d{9}$";

            Regex myRegexDienThoai = new Regex(patternDienThoai);

            Match mDienThoai = myRegexDienThoai.Match(mobile);

            if (!mDienThoai.Success)
            {
                return false;
            }

            return true;
        }
        public static bool CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;
            if (CheckScriptTag(email))
                return false;
            //Kiểm tra định dạng email

            return Regex.IsMatch(email, @"^([0-9a-z]+[-._+&])*[0-9a-z]+@([-0-9a-z]+[.])+[a-z]{2,6}$", RegexOptions.IgnoreCase);
        }
        public static string InsertCommaNoStyle(string strMoney)
        {
            if (string.IsNullOrEmpty(strMoney))
                return string.Empty;


            int length = strMoney.Length;

            while (length > 3)
            {
                strMoney = strMoney.Insert(length - 3, ".");
                length = strMoney.IndexOf('.');
            }
            return strMoney;
        }
        public static string InsertCommaNoStyle2(long strMoney2)
        {
            string strMoney = strMoney2.ToString();
            if (string.IsNullOrEmpty(strMoney))
                return string.Empty;


            int length = strMoney.Length;

            while (length > 3)
            {
                strMoney = strMoney.Insert(length - 3, ".");
                length = strMoney.IndexOf('.');
            }
            return strMoney;
        }
        public static decimal GetDevide(long item1, long item2)
        {
            if (item2 == 0)
                return 0;
            return (decimal)item1 / item2;
        }
            public static string InsertCommaMark(string strMoney, int type)
        {
            if (string.IsNullOrEmpty(strMoney))
                return string.Empty;

            string sign = "-";
            string classcss = "blue_txt";
            if (type == 2)
            {

                strMoney = strMoney.Replace(sign, "");
                classcss = "red_txt";
            }
            else
            {
                if (strMoney == "0")
                    sign = "";
                else
                    sign = "+";
            }


            int length = strMoney.Length;
            while (length > 3)
            {
                strMoney = strMoney.Insert(length - 3, ".");
                length = strMoney.IndexOf('.');
            }

            return "<span class='" + classcss + "'>" + sign + strMoney + "</span>";
        }
        public static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }
        public static string ReplaceVietnameseChar(string s)
        {
            if (s == null)
                return String.Empty;
            // replace specification character
            //s = s.Trim().ToLower();
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
        public static DateTime GetLastWeeekEnd(DateTime date)
        {
            DateTime mondayOfLastWeek = date.AddDays(-(int)date.DayOfWeek);
            return mondayOfLastWeek;
        }
        public static DateTime GetFirstMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);

        }
        public static DateTime GetLastYear(DateTime date)
        {
            return new DateTime(date.Year - 1, 1, 1);

        }

        /// <summary>
        /// Hàm tạo tên file từ một ên unicode
        /// </summary>
        /// <param name="value">Chuỗi string</param>
        /// <param name="len">độ dài</param>
        /// <returns>string</returns>
        public static string FileNameFromUnicode(string fileNameFull, int len = 150)
        {
            try
            {
                if (string.IsNullOrEmpty(fileNameFull))
                    return string.Empty;

                var fileNameOnly = Path.GetFileNameWithoutExtension(fileNameFull);
                string extension = Path.GetExtension(fileNameFull);
                string path = Path.GetDirectoryName(fileNameFull);

                if (len > 0)
                    fileNameOnly = ShortenByWord(fileNameOnly, len, "");

                fileNameOnly = RemoveUnicode(fileNameOnly);

                var newFileName = Regex.Replace(fileNameOnly, "[^0-9a-zA-Z]+", "_");

                newFileName = Regex.Replace(fileNameOnly, @"[_]{2,}", "_");

                return Path.Combine(path, newFileName + extension);
            }
            catch
            {
                return fileNameFull;
            }
        }

        /// <summary>
        /// Hàm loại bỏ các ký tự unicode sang các kí tự thường.
        /// </summary>
        /// <param name="value">Chuỗi unicode</param>
        /// <returns>string</returns>
        public static string RemoveUnicode(string value)
        {
            value = Regex.Replace(value, @"\s+", "_", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            string nfd = value.Normalize(NormalizationForm.FormD);
            StringBuilder retval = new StringBuilder(nfd.Length);
            foreach (char ch in nfd)
            {
                if (ch >= '\u0300' && ch <= '\u036f') continue;
                if (ch >= '\u1dc0' && ch <= '\u1de6') continue;
                if (ch >= '\ufe20' && ch <= '\ufe26') continue;
                if (ch >= '\u20d0' && ch <= '\u20f0') continue;
                retval.Append(ch);
            }
            return retval.ToString();
        }

        /// <summary>
        /// Hàm cắt ngắn một chuỗi
        /// Nếu nẻ một chữ thì bỏ chữ đó cho đến dấu khoảng cách cuối cùng
        /// </summary>
        /// <param name="sentence">Chuỗi cần cắt</param>
        /// <param name="len">Độ dài</param>
        /// <returns>Chuỗi cộng thêm sau khi cắt ngắn</returns>
        public static string ShortenByWord(string sentence, int len, string expanded = "...")
        {
            if (sentence == null) return string.Empty;
            if (sentence.Length > len)
            {
                sentence = sentence.Substring(0, len);
                // cut a word
                int pos = sentence.LastIndexOf(' ');
                if (pos > 0) sentence = sentence.Substring(0, pos);
                return sentence + expanded;
            }
            return sentence;
        }

    }
}
