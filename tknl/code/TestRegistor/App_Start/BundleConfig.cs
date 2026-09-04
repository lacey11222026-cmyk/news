using System.Web;
using System.Web.Optimization;

namespace TestRegistor
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/sitescript").Include(
                     //"~/Content2/js/jquery.js",
                     //"~/Content2/js/bootstrap.js",
                     "~/Content/js/jquery.mmenu.min.all.js",
                     "~/Content/js/bootbox.min.js",
                      "~/Content/js/wow.js",
                      "~/Content/js/script.js",
                      "~/Content/js/formvalidate.js"
                     ));
            bundles.Add(new StyleBundle("~/Content/sitecss").Include(
           "~/Content/css/bootstrap.min.css",
           "~/Content/css/common.css",
           "~/Content/css/style.css",
            "~/Content/css/animate.css",
           "~/Content/css/jquery.mmenu.all.css",
           "~/Content/css/extend.css"

           ));
        }
    }
}
