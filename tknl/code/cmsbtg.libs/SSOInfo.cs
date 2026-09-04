using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Security;

namespace cms.libs
{
    public class SSOInfo
    {
        private string _CookieUserInfo = "eb4a3c8426d5a29404e4e148cc92af13";

        private RijndaelEnhanced rijndaelKey;
        private int _LoginExpires = 15;     //15 phut
        private string _SSODomain = Config.SSODomain;

        private int _LoginID = 0;
        private string _LoginName = "";
        private string _LoginSessionID = "";
        private string _LoginIP = "";
        private bool _LoginIsExpires = true;

        public int LoginID
        {
            get { return _LoginID; }
            set { _LoginID = value; }
        }

        public string LoginName
        {
            get { return _LoginName; }
            set { _LoginName = value; }
        }

        public string LoginSessionID
        {
            get { return _LoginSessionID; }
            set { _LoginSessionID = value; }
        }

        public string LoginIP
        {
            get { return _LoginIP; }
            set { _LoginIP = value; }
        }

        public void TimeExpiresSetting(int TimeExpiresSetting)
        {
            _LoginExpires = TimeExpiresSetting;
        }

        public SSOInfo()
        {
            rijndaelKey = new RijndaelEnhanced(Getkey(), "@1B2c3D4e5F6g7H8");
        }

        public SSOInfo(string key)
        {
            rijndaelKey = new RijndaelEnhanced(key, "@1B2c3D4e5F6g7H8");
        }

        public virtual bool IsSigned()
        {
            this.Get();
            bool bLoged = (this._LoginName.Length > 0 && this._LoginIP.Length > 0 && !this._LoginIsExpires);
            if (bLoged)
            {
                this.SetCookieUserInfo(this._LoginID, this._LoginName, this._LoginSessionID);
            }
            return bLoged;
        }

        public void Set()
        {
            this.SetCookieUserInfo(_LoginID, _LoginName, HttpContext.Current.Session.SessionID);
        }

        public void Set(int UserID)
        {
            this._LoginID = UserID;
            this.SetCookieUserInfo(_LoginID, _LoginName, HttpContext.Current.Session.SessionID);
        }

        public void Set(string UserName)
        {
            this._LoginName = UserName;
            this.SetCookieUserInfo(_LoginID, _LoginName, HttpContext.Current.Session.SessionID);
        }

        public void Set(int UserID, string UserName)
        {
            this._LoginID = UserID;
            this._LoginName = UserName;
            this.SetCookieUserInfo(_LoginID, _LoginName, HttpContext.Current.Session.SessionID);
        }

        public SSOInfo Get()
        {
            string[] CookieValueArray = GetCookieUserInfo();
            if (CookieValueArray[0] != "")
                this._LoginID = int.Parse(CookieValueArray[0]);
            this._LoginName = CookieValueArray[1];
            this._LoginSessionID = CookieValueArray[2];
            this._LoginIP = CookieValueArray[3];
            if (!CookieValueArray[4].Equals(""))
            {
                if (Convert.ToDateTime(CookieValueArray[4]) > DateTime.Now)
                {
                    this._LoginIsExpires = false;
                }
            }
            return this;
        }

        public virtual void SignOut()
        {
            DelCookieUserInfo();
        }

        //ghi thong tin UserInfo
        public void SetCookie(string CookieName, string CookieValue)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieName] != null)
                System.Web.HttpContext.Current.Response.Cookies.Set(System.Web.HttpContext.Current.Request.Cookies[CookieName]);
            else
                System.Web.HttpContext.Current.Response.Cookies.Set(new HttpCookie(CookieName, ""));

            System.Web.HttpContext.Current.Response.Cookies[CookieName].Value = rijndaelKey.Encrypt(CookieValue);
            if (!_SSODomain.Equals(""))
            {
                System.Web.HttpContext.Current.Response.Cookies[CookieName].Path = "/";
                System.Web.HttpContext.Current.Response.Cookies[CookieName].Domain = _SSODomain;
            }
            System.Web.HttpContext.Current.Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(_LoginExpires);
            System.Web.HttpContext.Current.Response.Cookies[CookieName].HttpOnly = true;
        }

        public string GetCookie(string CookieName)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                return rijndaelKey.Decrypt(System.Web.HttpContext.Current.Request.Cookies[CookieName].Value.ToString());
            }
            else
                return "";
        }

        private void SetCookieUserInfo(int UserID, string UseNname, string SessionID)
        {
            string CookieValue = "";
            CookieValue = UserID.ToString();
            CookieValue += "|" + UseNname;
            CookieValue += "|" + SessionID;
            CookieValue += "|" + getIP();
            CookieValue += "|" + DateTime.Now.AddMinutes(_LoginExpires);

            if (System.Web.HttpContext.Current.Request.Cookies[_CookieUserInfo] != null)
                System.Web.HttpContext.Current.Response.Cookies.Set(System.Web.HttpContext.Current.Request.Cookies[_CookieUserInfo]);
            else
                System.Web.HttpContext.Current.Response.Cookies.Set(new HttpCookie(_CookieUserInfo, ""));

            System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].Value = rijndaelKey.Encrypt(CookieValue);
            if (!_SSODomain.Equals(""))
            {
                System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].Path = "/";
                System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].Domain = _SSODomain;
            }
            System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].Expires = DateTime.Now.AddMinutes(_LoginExpires);

            System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].HttpOnly = true;
        }

        private string[] GetCookieUserInfo()
        {
            string[] CookieValue = new string[0];
            if (System.Web.HttpContext.Current.Request.Cookies[_CookieUserInfo] != null)
            {
                try
                {
                    CookieValue = rijndaelKey.Decrypt(System.Web.HttpContext.Current.Request.Cookies[_CookieUserInfo].Value.ToString()).Split(new char[] { '|' });
                }
                catch
                {
                    CookieValue = "|||||".Split(new char[] { '|' });
                }
            }
            else
                CookieValue = "|||||".Split(new char[] { '|' });
            return CookieValue;
        }

        private void DelCookieUserInfo()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[_CookieUserInfo] != null)
            {
                if (!_SSODomain.Equals(""))
                {
                    System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].Domain = _SSODomain;
                }
                System.Web.HttpContext.Current.Response.Cookies[_CookieUserInfo].Expires = DateTime.Now.AddMonths(-1);
            }
        }

        //==================================================================

        private string getIP()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]) && HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != "unknown")
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "unknown")
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]) && HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != "unknown")
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        //private string getIP()
        //{
        //    string IP = "";
        //    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //    {
        //        IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    }
        //    if (IP == "")
        //    {
        //        IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    return IP;
        //}
        private string Getkey()
        {
            return Key.sKey;
        }
    }
}