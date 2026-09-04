using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace cms.libs
{
    public class IsMobile
    {
        public string DeviceMobileType { get; set; }

        public int VideoType { get; set; }

        public enum DeviceType
        {
            Desktop,
            Tablet,
            WindowsPhone,
            Phone
        }

        public IsMobile UserAgentToDeviceType(string userAgent)
        {
            string _DeviceType = string.Empty;
            int _VideoType = 0;
            _DeviceType = DeviceType.Desktop.ToString();
            if (userAgent.ToLowerInvariant().Contains("blackberry"))
            {
                _DeviceType = DeviceType.Phone.ToString();
                _VideoType = 5;
            }

            if (userAgent.ToLowerInvariant().Contains("iphone"))
            {
                _DeviceType = DeviceType.Phone.ToString();
                _VideoType = 4;
            }

            if (userAgent.ToLowerInvariant().Contains("windows phone"))
            {
                _DeviceType = DeviceType.WindowsPhone.ToString();
                _VideoType = 1;
            }

            if (userAgent.ToLowerInvariant().Contains("ipad"))
            {
                _DeviceType = DeviceType.Tablet.ToString();
                _VideoType = 2;
            }

            if (userAgent.ToLowerInvariant().Contains("android"))
            {
                _DeviceType = DeviceType.Tablet.ToString();
                string[] a = userAgent.Split(';');
                try
                {
                    if (a[2] != null)
                    {
                        string[] a2 = a[2].Trim().Split(' ');
                        var itemver = a2[1].Substring(0, 3);
                        double version = 0;
                        double.TryParse(itemver, out version);
                        if (version <= 2.1)
                        {
                            _VideoType = 3;
                        }
                        else
                        {
                            _VideoType = 2;
                        }
                    }
                    else
                    {
                        _VideoType = 4;
                    }
                }
                catch (Exception)
                {
                    _VideoType = 4;
                }
            }

            if (userAgent.ToLowerInvariant().Contains("nokia") && !userAgent.ToLowerInvariant().Contains("windows phone"))
            {
                _DeviceType = DeviceType.Phone.ToString();
                _VideoType = 5;
            }

            IsMobile isMobile = new IsMobile();
            isMobile.DeviceMobileType = _DeviceType;
            isMobile.VideoType = _VideoType;
            return isMobile;
        }
    }

    public class Common
    {
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
    }
}