using System;
using System.Collections;
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

    public partial class keepalive : System.Web.UI.Page
    {
        private UserValidation m_UserValidation = new UserValidation();
        protected void Page_Load(object sender, EventArgs e)
        {
            m_UserValidation.IsSigned();
        }
    }
}