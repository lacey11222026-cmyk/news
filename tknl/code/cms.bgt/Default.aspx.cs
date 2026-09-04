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

    public partial class _default : System.Web.UI.Page
    {
        public string UrlRoot = Constants.ROOT_PATH;
        private UserValidation m_UserValidation = new UserValidation();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!m_UserValidation.IsSigned())
            //{
            //    Response.Redirect(UrlRoot + "default.aspx", true);
            //}

            /*List<WorkflowTask> l_WorkflowTask = new WorkflowTask().GetByUserIdAndPartId(m_UserValidation.LoginID, m_UserValidation.PartID);

            if (l_WorkflowTask != null && l_WorkflowTask.Count > 0)
            {
                WorkflowTask m_WorkflowTask = l_WorkflowTask[l_WorkflowTask.Count - 1];
                string Url = UrlRoot;
                Url = UrlRoot + "discuss/index.htm";
                if (Url != UrlRoot)
                {
                    Response.Redirect(Url, true);
                }
            }*/
        }
    }
}