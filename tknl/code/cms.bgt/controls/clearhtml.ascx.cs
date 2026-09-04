using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using cms.libs;

namespace CMS2012.controls
{
    public partial class clearhtml : System.Web.UI.UserControl
    {
        public string UrlRoot = cms.libs.Constants.ROOT_PATH;

        public string content { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        protected void Removetagshtml_Click(object sender, EventArgs e)
        {
            content = StripTagsCharArray(RadContent.Html);
            RadContent.Html = content;
        }
    }
}