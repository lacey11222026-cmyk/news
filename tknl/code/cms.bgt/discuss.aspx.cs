using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using cms.libs;

namespace CMS
{

    public partial class discuss : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        private int PartId = 26;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_UserValidation.CheckSysFunction(6))
            {
                Response.Redirect(UrlRoot + "default.aspx", true);
            }
            if (!m_UserValidation.IsSigned())
            {
                Response.Redirect("~/login.aspx?url=" + Server.UrlEncode(Request.RawUrl), true);
            }
            List<WorkflowTask> l_WorkflowTask = new WorkflowTask().GetByUserIdAndPartId(m_UserValidation.LoginID, PartId);

            if (l_WorkflowTask == null || l_WorkflowTask.Count == 0)
                return;
            WorkflowTask m_WorkflowTask = l_WorkflowTask[0];
            string Url = UrlRoot;
            if (m_WorkflowTask.AddnewEnabled)
            {
                Url = UrlRoot + "discuss/" + m_WorkflowTask.Id.ToString() + "/add/index.htm";
            }
            else
            {
                Url = UrlRoot + "discuss/" + m_WorkflowTask.Id.ToString() + "/pending/index.htm";
            }

            Response.Redirect(Url, true);
        }
    }
}

