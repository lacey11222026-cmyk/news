using System.Web;
using System.Web.Optimization;

namespace WebEN
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/scripts/site.js").Include(

                     "~/Scripts/site.constant.js",
                     "~/Scripts/site.utils.js",
                      "~/Scripts/site.lib.string.js",
                     "~/Scripts/site.lib.encode.js",
                     "~/Scripts/jquery.jcarousel.min.js",
                     "~/Scripts/jquery.query.js"
                     
                     ));
        }
    }
}