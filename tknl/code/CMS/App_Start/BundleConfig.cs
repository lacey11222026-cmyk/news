using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/scripts/site").Include(
                     //"~/Scripts/jquery-1.9.1.min.js",
                     //"~/Scripts/jquery-ui.1.10.min.js",
                     "~/Scripts/jquery-paging.js",
                     "~/Scripts/jQuery-jCache.js",
                      "~/Scripts/jquery-jtemplates.js",
                     "~/Scripts/tiny_mce/tiny_mce.js",
                     "~/Scripts/ajaxupload.3.6.js",
                     "~/Scripts/jquery.query.js",
                      "~/Administrator/Scripts/admin.constant.js",
                     "~/Administrator/Scripts/admin.utils.js",
                     "~/Scripts/site.lib.encode.js",
                     "~/Scripts/jquery-services-110405.js",
                     "~/Administrator/Scripts/highcharts.js",
                     "~/Styles/admin/js/custom_blue.js",
                     "~/Scripts/jquery-ui-timepicker-addon.js"
                     ));
            bundles.Add(new StyleBundle("~/style").Include(
                 "~/Styles/admin/css/blue/screen.css",
                 "~/Styles/admin/css/blue/datepicker.css",
                 "~/Styles/admin/css/tipsy.cs",
                 "~/Styles/admin/js/visualize/visualize.css",
                 "~/Styles/admin/js/jwysiwyg/jquery.wysiwyg.css",
                 "~/Styles/admin/js/fancybox/jquery.fancybox-1.3.0.css",
                 "~/js/jAlert/jquery.alerts.css",
                 "~/Styles/admin/css/tipsy.css",
                 "~/Styles/admin/css/blue/extend.css",
                 "~/Styles/jquery-ui-1.8.16.admin.css"
             ));
            BundleTable.EnableOptimizations = false;
        }
    }
}