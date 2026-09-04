using System.Web;
using System.Web.Optimization;

namespace WebMVC4
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Administrator/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/sitecss").Include(
                    "~/Content/css/*.css"
                    ));
            bundles.Add(new ScriptBundle("~/Content/sitejs").Include(

                        "~/Content/js/*.js"
                       ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Administrator/Styles/css").Include(
                     "~/Administrator/Styles/bootstrap.min.css",
                     "~/Administrator/Styles/site.css",
                     "~/Administrator/Styles/sb-admin.css",
                     "~/Administrator/Styles/plugins/validation/bootstrapValidator.css",
                     "~/Administrator/Styles/plugins/datetimepicker/bootstrap-datetimepicker.css",
                     "~/Administrator/Styles/extend.css"
                     ));

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
                     "~/Administrator/Scripts/sb-admin.js"
                     //"~/Administrator/Scripts/dropzone/dropzone.js"

                     ));
            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                     "~/Administrator/Scripts/jquery.metisMenu.js"
               ));
        }
    }
}