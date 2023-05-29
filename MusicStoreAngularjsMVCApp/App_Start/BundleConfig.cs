using System.Web;
using System.Web.Optimization;

namespace MusicStoreAngularjsMVCApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/Site.css",
                      "~/Content/fontawesome-free-6.4.0-web/css/all.css"));

            bundles.Add(new ScriptBundle("~/bundles/chosenjs").Include(
                      "~/Library/chosen_v1.8.7/chosen.jquery.min.js"));

            bundles.Add(new StyleBundle("~/bundles/chosencss").Include(
                      "~/Library/chosen_v1.8.7/chosen.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/angularchosenjs").Include(
                      "~/Scripts/angular-chosen-1.4.0/dist/angular-chosen.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzonejs").Include(
                      "~/Library/dropzone@5/dropzone.min.js"));

            bundles.Add(new StyleBundle("~/bundles/dropzonecss").Include(
                      "~/Library/dropzone@5/dropzone.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                      "~/Scripts/angular.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                    "~/Scripts/app/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/guestIndex").Include(
                    "~/Scripts/app/guest/guestIndex.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminIndex").Include(
                    "~/Scripts/app/admin/adminIndex.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminAddCategory").Include(
                    "~/Scripts/app/admin/adminAddCategory.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/customerIndex").Include(
                    "~/Scripts/app/customer/customerIndex.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/customerCart").Include(
                    "~/Scripts/app/customer/customerCart.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/customerMovieDetail").Include(
                    "~/Scripts/app/customer/customerMovieDetail.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/customerOrderHistory").Include(
                    "~/Scripts/app/customer/customerOrderHistory.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/sellerIndex").Include(
                    "~/Scripts/app/seller/sellerIndex.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/sellerMovieDetail").Include(
                    "~/Scripts/app/seller/sellerMovieDetail.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/sellerAddMovie").Include(
                    "~/Scripts/app/seller/sellerAddMovie.controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/sellerOrder").Include(
                    "~/Scripts/app/seller/sellerOrder.controller.js"));
        }
    }
}
