using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Net;

namespace cms.libs
{
    public class UserValidation : SSOInfo
    {
        private string CookiePartIDName = "PartID";
        private string CookieProcessIDName = "ProcessID";
        private int _PartID;
        public int PartID
        {
            get
            {
                return _PartID;
            }
            set
            {
                SetCookie(CookiePartIDName, value.ToString());
            }
        }
        private int _ProcessID;
        public int ProcessID
        {
            get
            {
                return _ProcessID;
            }
            set
            {
                SetCookie(CookieProcessIDName, value.ToString());
            }
        }

        public UserValidation()
        {
            Get();
            string sValue = GetCookie(CookiePartIDName);

            if (sValue == "")
            {
                if (LoginID > 0)
                {
                    DataTable dt_Parts = new Parts().GetForUserID(this.LoginID);
                    if (dt_Parts != null && dt_Parts.Rows.Count > 0)
                    {
                        sValue = dt_Parts.Rows[0]["ID"].ToString();
                        SetCookie(CookiePartIDName, sValue);
                    }
                }
            }
            else
            {
                SetCookie(CookiePartIDName, sValue);
            }

            if (sValue == "")
                _PartID = 0;
            else
                _PartID = int.Parse(sValue);

            sValue = GetCookie(CookieProcessIDName);
            if (sValue == "")
            {
                if (_PartID > 0)
                {
                    DataRow Part_Info = new Parts().GetInfo(_PartID);
                    if (Part_Info != null)
                    {
                        sValue = Part_Info["WorkflowID"].ToString();
                        SetCookie(CookieProcessIDName, sValue);
                    }
                }
            }
            else
            {
                SetCookie(CookieProcessIDName, sValue);
            }
            if (sValue == "")
                _ProcessID = 0;
            else
                _ProcessID = int.Parse(sValue);

        }
        public override void SignOut()
        {
            base.SignOut();
            base.SetCookie(CookiePartIDName, "");
            base.SetCookie(CookieProcessIDName, "");
        }
        public bool CheckSysFunction(int FunctionID)
        {
            DataRow dr = new SysFunc_User().GetInfo(FunctionID, this.LoginID);
            return dr == null ? false : true;
        }
        public bool CheckTask(int TaskID)
        {
            DataRow dr = new UserTask().GetInfo(TaskID, this.LoginID, this.PartID);
            return dr == null ? false : true;
        }

    }
}
