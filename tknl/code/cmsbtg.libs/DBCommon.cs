using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

/// <summary>
/// Summary description for DBCommon
/// </summary>
namespace cms.libs
{
    public sealed class DBCommon
    {
        public static string ClientIP
        {
            get
            {
                string IP = "";
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (IP == "")
                {
                    IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return IP;
            }
        }

        public static string UrlRoot
        {
            get
            {
                string sRet = "";
                if (ConfigurationManager.AppSettings["rootPath"] == null)
                {
                    sRet = System.Web.HttpContext.Current.Request.ApplicationPath;
                }
                else
                {
                    sRet = ConfigurationManager.AppSettings["rootPath"];
                }
                if (!sRet.EndsWith("/"))
                    sRet = sRet + "/";
                return sRet;
            }
        }

        public string XSSFilter(string sValue)
        {
            string sTemp = "?=:/._-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string sOut = "";
            for (int i = 0; i < sValue.Length; i++)
            {
                if (sTemp.IndexOf(sValue[i]) >= 0)
                {
                    sOut += sValue[i];
                }
            }

            return sOut;
        }

        public static String UCS2Convert(String sContent)
        {
            sContent = sContent.Trim();
            String sUTF8Lower = "a|á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ|đ|e|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ|i|í|ì|ỉ|ĩ|ị|o|ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ|u|ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự|y|ý|ỳ|ỷ|ỹ|ỵ";

            String sUTF8Upper = "A|Á|À|Ả|Ã|Ạ|Ă|Ắ|Ằ|Ẳ|Ẵ|Ặ|Â|Ấ|Ầ|Ẩ|Ẫ|Ậ|Đ|E|É|È|Ẻ|Ẽ|Ẹ|Ê|Ế|Ề|Ể|Ễ|Ệ|I|Í|Ì|Ỉ|Ĩ|Ị|O|Ó|Ò|Ỏ|Õ|Ọ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ|U|Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự|Y|Ý|Ỳ|Ỷ|Ỹ|Ỵ";

            String sUCS2Lower = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|d|e|e|e|e|e|e|e|e|e|e|e|e|i|i|i|i|i|i|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|u|y|y|y|y|y|y";

            String sUCS2Upper = "A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|D|E|E|E|E|E|E|E|E|E|E|E|E|I|I|I|I|I|I|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|U|U|U|U|U|U|U|U|U|U|U|U|Y|Y|Y|Y|Y|Y";

            String[] aUTF8Lower = sUTF8Lower.Split(new Char[] { '|' });

            String[] aUTF8Upper = sUTF8Upper.Split(new Char[] { '|' });

            String[] aUCS2Lower = sUCS2Lower.Split(new Char[] { '|' });

            String[] aUCS2Upper = sUCS2Upper.Split(new Char[] { '|' });

            Int32 nLimitChar;

            nLimitChar = aUTF8Lower.GetUpperBound(0);

            for (int i = 1; i <= nLimitChar; i++)
            {
                sContent = sContent.Replace(aUTF8Lower[i], aUCS2Lower[i]);

                sContent = sContent.Replace(aUTF8Upper[i], aUCS2Upper[i]);
            }
            string sUCS2regex = @"[A-Za-z0-9- ]";
            string sEscaped = new Regex(sUCS2regex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture).Replace(sContent, string.Empty);
            if (string.IsNullOrEmpty(sEscaped))
                return sContent;
            sEscaped = sEscaped.Replace("[", "\\[");
            sEscaped = sEscaped.Replace("]", "\\]");
            sEscaped = sEscaped.Replace("^", "\\^");
            string sEscapedregex = @"[" + sEscaped + "]";
            return new Regex(sEscapedregex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture).Replace(sContent, string.Empty);
        }

        public static string StripHtml(string Html)
        {
            if (string.IsNullOrEmpty(Html))
            {
                return string.Empty;
            }
            //Stripts the <script> tags from the Html
            string scriptregex = @"<scr" + @"ipt[^>.]*>[\s\S]*?</sc" + @"ript>";
            System.Text.RegularExpressions.Regex scripts = new System.Text.RegularExpressions.Regex(scriptregex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            string scriptless = scripts.Replace(Html, " ");

            //Stripts the <style> tags from the Html
            string styleregex = @"<style[^>.]*>[\s\S]*?</style>";
            System.Text.RegularExpressions.Regex styles = new System.Text.RegularExpressions.Regex(styleregex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            string styleless = styles.Replace(scriptless, " ");
            //Strips the HTML tags from the Html
            System.Text.RegularExpressions.Regex objRegExp = new System.Text.RegularExpressions.Regex("<(.|\n)+?>", RegexOptions.IgnoreCase);

            //Replace all HTML tag matches with the empty string
            string strOutput = objRegExp.Replace(styleless, " ");

            // Convert &&amp;amp;eacute; to &amp;eacute; (e') so French words are indexable
            // ## UNDOCUMENTED ## this line is new in Version 2, but was not documented
            // in the article... I may explain it when writing about Version 3...
            ExtendedHtmlUtility ExtHtml = new ExtendedHtmlUtility();
            strOutput = ExtHtml.HtmlEntityDecode(strOutput, false);
            // The above line can be safely commented out on most English pages
            // since it's unlikely any 'important' characters would be HtmlEncoded

            //Replace all < and > with &lt; and &gt;
            strOutput = strOutput.Replace("<", "&lt;");
            strOutput = strOutput.Replace(">", "&gt;");

            objRegExp = null;
            return strOutput;
        }

        /// <summary>
        /// chuyển sang không dấu
        /// </summary>
        /// <param name="accented"></param>
        /// <returns></returns>

        public static string StripDiacriticsFile(string accented)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = accented.Normalize(System.Text.NormalizationForm.FormD);
            string content = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            
            return content;
        }

        public static string AutoTagHTML(string source)
        {
            try
            {
                string result;
                result = StripHtml(source.ToLower()).Replace("\"", ",");
                result = result.Replace("'", ",");
                result = result.Replace(".", ",");
                result = result.Replace("?", ",");
                result = result.Replace("!", ",");
                do
                {
                    result = result.Replace("  ", " ");
                } while (result.IndexOf("  ") > 0);
                string[] aresult = result.Split(" ".ToCharArray());
                string sDon_Am = "cần|về|quá|vì|bị|do|làm|nhưng|cùng|một|hai|ba|như|sau|không|mà|các|lên|hoặc|giành|này|nhận|ngày|từ|thay|đều|vừa|gì|theo|cho|mới|của|sẽ|trên|và|đang|theo|của|rất|muốn|có|được|với|cả|đến|những|tại|ở|là|của|khi|còn|cũng|vì|có|trong|theo|tại|vào|";
                for (int i = 0; i < aresult.Length; i++)
                {
                    if ((sDon_Am.IndexOf(aresult[i] + "|") >= 0))
                    {
                        aresult[i] = ",";
                    }
                }

                result = "";
                for (int i = 0; i < aresult.Length; i++)
                {
                    result = result + " " + aresult[i];
                }
                aresult = result.Split(",".ToCharArray());

                result = "";
                string sTmp = "";
                for (int i = aresult.Length - 1; i > 0; i--)
                {
                    sTmp = aresult[i].Trim();
                    while (sTmp.StartsWith(","))
                    {
                        sTmp = sTmp.Remove(0, 1);
                    }
                    while (sTmp.EndsWith(","))
                    {
                        sTmp = sTmp.Remove(sTmp.Length - 1, 1);
                    }
                    if (sTmp.Trim().Length > 2) result = result + ", " + sTmp.Trim();
                }
                while (result.StartsWith(","))
                {
                    result = result.Remove(0, 1);
                }
                while (result.EndsWith(","))
                {
                    result = result.Remove(result.Length - 1, 1);
                }
                return result.Trim();
            }
            catch
            {
                return source;
            }
        }

        public static string SubString(string sSource, int length)
        {
            if (string.IsNullOrEmpty(sSource))
                return string.Empty;
            if (sSource.Length <= length)
                return sSource;

            string mSource = sSource;
            int nLength = length;

            int m = mSource.Length;
            while (nLength > 0 && mSource[nLength].ToString() != " ")
            {
                nLength--;
            }
            mSource = mSource.Substring(0, nLength);
            return mSource + "...";
        }

        public static string MakeRewriteUrl(string RootPath, string rewriteUrl, int cateId)
        {
            while (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            while (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId);
            return RootPath + m_UrlUtils.Encode() + "/" + rewriteUrl + "/index.htm";
        }

        public static string MakeNextPageRewriteUrl(string RootPath, string rewriteUrl, int cateId, int CurrPage)
        {
            while (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            while (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId, CurrPage);
            return RootPath + m_UrlUtils.Encode() + "/" + rewriteUrl + "/index.htm";
        }

        public static string MakeMobileRewriteUrl(string RootPath, string rewriteUrl, int cateId)
        {
            while (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            while (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId);
            return RootPath + m_UrlUtils.Encode() + "/" + rewriteUrl + "/index.html";
        }

        public static string MakeRewriteUrlv1(string RootPath, string rewriteUrl, int cateId, long itemId)
        {
            if (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            if (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId, itemId);
            //return "/thethao/" + m_UrlUtils.Encode() + "/" + rewriteUrl + ".htm";
            return RootPath + rewriteUrl + "-" + m_UrlUtils.EncodeV1() + ".html";
        }

        public static string MakeRewriteUrl(string RootPath, string rewriteUrl, int cateId, long itemId)
        {
            if (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            if (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId, itemId);
            //return "/thethao/" + m_UrlUtils.Encode() + "/" + rewriteUrl + ".htm";
            return RootPath + rewriteUrl + "-" + m_UrlUtils.Encode() + ".html";
        }

        public static string MakeMobileRewriteUrl(string RootPath, string rewriteUrl, int cateId, long itemId)
        {
            if (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            if (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId, itemId);
            return RootPath + m_UrlUtils.Encode() + "/" + rewriteUrl + ".html";
        }

        public static bool isMobileBrowser2()
        {
            bool ismobile = false;
            HttpContext context = HttpContext.Current;
            string u = context.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|meego.+mobile|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                ismobile = true;
            }
            return ismobile;
        }

        public static bool isMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                    new[]
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "sie-", "sec-", "htc",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi",
                    "moto", "iphone"
                };

                //Loop through each item in the list created above
                //and check if the header contains that text
                foreach (string s in mobiles)
                {
                    if (context.Request.ServerVariables["HTTP_USER_AGENT"].
                                                        ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string MakeSlideShowRewriteUrl(string RootPath, string rewriteUrl, int cateId, long itemId)
        {
            if (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            if (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId, itemId);
            return RootPath + m_UrlUtils.Encode() + "/" + rewriteUrl + "/slide-show/1/index.htm";
        }
 
        public static string MakeSlideShowRewriteUrlnews(string RootPath, string rewriteUrl, int cateId, long itemId)
        {
            if (rewriteUrl.StartsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(0, 1);
            }
            if (rewriteUrl.EndsWith("/"))
            {
                rewriteUrl = rewriteUrl.Remove(rewriteUrl.Length - 1, 1);
            }
            UrlUtils m_UrlUtils = new UrlUtils(cateId, itemId);
           // return RootPath + m_UrlUtils.Encode() + "/" + rewriteUrl + "/slide-show/1/index.htm";
            return RootPath + rewriteUrl + "-" + m_UrlUtils.EncodeV1() + ".html";
        }
        public static string StripDiacritics(string accented)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = accented.Normalize(System.Text.NormalizationForm.FormD);
            string content = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            content = content.Replace("/", "-");
            content = content.Replace(".", "-");
            content = content.Replace("--", "-");
            content = content.Replace(" ", "-");
            content = content.Replace("?", "-");
            content = content.Replace("\"", "-");
            content = content.Replace("'", "-");
            content = content.Replace("!", "-");
            content = content.Replace("&", "-");
            content = content.Replace(":", "-");
            return content;
        }
    }
}