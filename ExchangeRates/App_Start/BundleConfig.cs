using System.Web.Optimization;

namespace ExchangeRates.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/all").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.*",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/bootstrap{version}.js",
                "~/Scripts/utils.js")
                );
            bundles.Add(new StyleBundle("~/Content/all").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/layout.css")
                );
        }
    }
}