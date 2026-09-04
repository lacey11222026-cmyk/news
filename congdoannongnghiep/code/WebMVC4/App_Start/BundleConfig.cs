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

            //bundles.Add(new ScriptBundle("~/sitescript").Include(
            //          //"~/Content2/js/jquery.js",
            //          //"~/Content2/js/bootstrap.js",
            //          "~/Content2/js/jquery.mmenu.min.all.js",
            //          "~/Content2/js/jquery.flexisel.js",
            //           "~/Content2/js/jquery.matchHeight.js",
            //           "~/Content2/js/script.js"
            //          ));
            //bundles.Add(new StyleBundle("~/Content/site.css").Include(
            //  "~/Content2/css/bootstrap.min.css",
            //  "~/Content2/css/common.css",
            //  "~/Content2/css/style.css",
            //  "~/Content2/css/jquery.mmenu.all.css",
            //  "~/Content2/css/extend.css"

            //  ));
            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                     "~/Administrator/Scripts/jquery.metisMenu.js"
               ));
        }
    }
}