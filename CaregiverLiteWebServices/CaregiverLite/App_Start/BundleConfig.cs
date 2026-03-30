using System;
using System.Web;
using System.Web.Optimization;

namespace CaregiverLite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);
            /*    ====================      CSS     ======================   */
            /*Note: Render in Login,Register, and _Layout*/
            //bundles.Add(new StyleBundle("~/Content/CommanCss").IncludeDirectory(
            //        "~/Content/css", "*.css"));

            //bundles.Add(new StyleBundle("~/Content/CommanCss1").Include(
            //        "~/Content/font-awesome/css/font-awesome.css"));

            //bundles.Add(new StyleBundle("~/Content/CommanCss").Include(
            //    "~/Content/css/bootstrap.min.css",
            //    "~/Content/font-awesome/css/font-awesome.css",
            //    "~/Content/css/dataTables.bootstrap.css",
            //    "~/Content/css/dataTables.responsive.css",
            //    "~/Content/css/dataTables.tableTools.min.css",
            //    "~/Content/css/animate.css",
            //    "~/Content/css/style.css",
            //    "~/Content/css/chosen.css",
            //    "~/Content/css/switchery.css",
            //    "~/Content/css/fullcalendar.css",
            //    "~/Content/css/fullcalendar.print.css"
            //    ));



            bundles.Add(new ScriptBundle("~/bundles/CommanJs").Include(
                "~/Scripts/js/jquery-{version}.js",
                "~/Scripts/js/bootstrap.min.js",
                "~/Scripts/js/jquery.dataTables.js",
                "~/Scripts/js/dataTables.bootstrap.js",
                "~/Scripts/js/dataTables.responsive.js",
                "~/Scripts/js/dataTables.tableTools.min.js",
                "~/Scripts/js/moment.js",
                "~/Scripts/js/moment.min.js",
                "~/Scripts/js/bootstrap-datetimepicker.js",
                "~/Scripts/js/jquery.metisMenu.js",
                "~/Scripts/js/jquery.slimscroll.min.js",
                "~/Scripts/js/jquery.jeditable.js",
                "~/Scripts/js/inspinia.js",
                "~/Scripts/js/chosen.jquery.js",
                "~/Scripts/js/switchery.js",
                "~/Scripts/js/fullcalendar.min.js",
                "~/Scripts/js/jquery.validate.js",
                "~/Scripts/js/jquery.validate.unobtrusive.js",
                "~/Scripts/Config.js",
                "~/Scripts/js/toastr.js",
                "~/Scripts/bootstrap-datetimepicker.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/jquery-ui.js"
                ));

            //bundles.Add(new ScriptBundle("~/bundles/CommanJs").IncludeDirectory(
            //    "~/Scripts/js", "*.js"
            //    ));

        }


        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css");
        }
    }
}
