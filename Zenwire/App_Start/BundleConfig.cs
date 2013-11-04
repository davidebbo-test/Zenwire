
using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;


namespace Zenwire
{
    [ExcludeFromCodeCoverage]
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/jquery-2.0.3.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Scripts/bootstrap-timepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/syndicate").Include(
                        "~/Scripts/syndicate/respond.min.js",
                        "~/Scripts/syndicate/retina.js",
                        "~/Scripts/syndicate/jquery.easing.js",
                        "~/Scripts/syndicate/jquery.fitvids.min.js",
                        "~/Scripts/syndicate/jquery.nicescroll.min.js",
                        "~/Scripts/syndicate/jquery.touchwipe.min.js",
                        "~/Scripts/syndicate/skrollr.js",
                        "~/Scripts/syndicate/toucheffects.js",
                        "~/Scripts/syndicate/modals.js",
                        "~/Scripts/syndicate/jquery.form.js",
                        "~/Scripts/syndicate/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/zenwire").Include("~/Scripts/zenwire-*"));
            
            bundles.Add(new ScriptBundle("~/bundles/guideline").Include(
                        "~/Scripts/syndicate/guideline.main.js",
                        "~/Scripts/syndicate/guideline.steps.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                        "~/Content/bootstrap.min.css",
                        "~/Content/syndicate-styles.css",
                        "~/Content/bootstrap-extensions/bootstrap-*"));

            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include(
                       "~/Content/fullcalendar/fullcalendar.css",
                       "~/Content/fullcalendar/fullcalendar.print.css"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                       "~/Scripts/jquery-ui-1.10.3.min.js",
                       "~/Scripts/jquery.ui.widget.min.js",
                       "~/Scripts/calendar/fullcalendar.js"));
        }
    }
}