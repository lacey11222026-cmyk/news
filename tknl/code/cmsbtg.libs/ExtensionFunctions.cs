using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text;
namespace cms.libs
{
    public static class ExtensionFunctions
    {
        public static string Xml(this XElement e)
        {
            StringBuilder builder = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(builder));
            e.WriteTo(writer);
            return builder.ToString();
        }
    }
}
