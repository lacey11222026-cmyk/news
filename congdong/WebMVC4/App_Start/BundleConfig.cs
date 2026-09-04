using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace WebMVC4
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public class CssUrlTransform : IBundleTransform
        {
            public void Process(BundleContext context, BundleResponse response)
            {
                Regex exp = new Regex(@"url\([^\)]+\)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                //foreach (FileInfo css in response.Files)
                //{
                //    string cssAppRelativePath = css.FullName.Replace(context.HttpContext.Request.PhysicalApplicationPath, context.HttpContext.Request.ApplicationPath).Replace(Path.DirectorySeparatorChar, '/');
                //    string cssDir = cssAppRelativePath.Substring(0, cssAppRelativePath.LastIndexOf('/'));
                //    response.Content = exp.Replace(response.Content, m => TransformUrl(m, cssDir));
                //}
            }


            private string TransformUrl(Match match, string cssDir)
            {
                string url = match.Value.Substring(4, match.Length - 5).Trim('\'', '"');

                if (url.StartsWith("http://") || url.StartsWith("data:image")) return match.Value;

                if (!url.StartsWith("/"))
                    url = string.Format("{0}/{1}", cssDir, url);

                return string.Format("url({0})", url);
            }

        }
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Administrator/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Administrator/Scripts/bootstrap.min.js",
                     "~/Administrator/Scripts/bootbox.min.js",
                     "~/Administrator/Scripts/jquery.pager.js",
                     "~/Administrator/Scripts/common.js",
                     "~/Administrator/Scripts/utils.js",
                     "~/Administrator/Scripts/plugins/validation/bootstrapValidator.js",
                     "~/Administrator/Scripts/plugins/datetimepicker/moment.js",
                     "~/Administrator/Scripts/plugins/datetimepicker/bootstrap-datetimepicker.js",
                     "~/Administrator/Scripts/plugins/dropdown-hover/bootstrap-dropdownhover.min.js",
                     "~/Administrator/Scripts/jquery.nicescroll.min.js",
                     "~/Administrator/Scripts/scripts.js",
                     "~/Administrator/Scripts/sb-admin.js",
                     "~/Administrator/Scripts/dropzone/dropzone.js"

                     ));
            bundles.Add(new StyleBundle("~/Administrator/Styles/css").Include(
                    "~/Administrator/Styles/bootstrap.min.css",
                    "~/Administrator/Styles/site.css",
                    "~/Administrator/Styles/sb-admin.css",
                    "~/Administrator/Styles/plugins/validation/bootstrapValidator.css",
                    "~/Administrator/Styles/plugins/datetimepicker/bootstrap-datetimepicker.css",
                    "~/Administrator/Styles/extend.css"

                    ));

            bundles.Add(new ScriptBundle("~/sitescript").Include(

                      "~/Content/js/jquery.mmenu.min.all.js",
                      "~/Content/js/jquery.flexisel.js",
                      "~/Content/js/jquery.matchHeight.js"
                      //"~/Content/js/script.js"
                      ));
            bundles.Add(new StyleBundle("~/Content/site.css").Include(
                 "~/Content/css/bootstrap.min.css",
                 "~/Content/css/common.css",
                 "~/Content/css/style.css",
                 "~/Content/css/jquery.mmenu.all.css",
                 "~/Content/css/chantrang.css",
                 "~/Content/css/extend.css"

                 ));
            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                     "~/Administrator/Scripts/jquery.metisMenu.js"
               ));
        }
    }
}