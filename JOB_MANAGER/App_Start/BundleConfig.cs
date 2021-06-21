using System.Web;
using System.Web.Optimization;

namespace JOB_MANAGER
{
    public class BundleConfig
    {
        // Paketleme hakkında daha fazla bilgi için lütfen https://go.microsoft.com/fwlink/?LinkId=301862 adresini ziyaret edin
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Geliştirme yapmak ve öğrenmek için Modernizr'ın geliştirme sürümünü kullanın. Daha sonra,
            // üretim için hazır. https://modernizr.com adresinde derleme aracını kullanarak yalnızca ihtiyacınız olan testleri seçin.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            ));

                //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                //          "~/Scripts/bootstrap.js"));

                bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.min.css",                      
                      "~/Content/fontawesome-all.css",
                      "~/Content/adminlte/adminlte.css",
                      "~/Scripts/JsGrid/jsgrid.min.css",
                      "~/Scripts/JsGrid/jsgrid-theme.min.css",                      
                      "~/Plugins/summernote/summernote-bs4.css",
                      "~/Plugins/bs-stepper/css/bs-stepper.min.css",
                      //"~/Content/adminlte/plugins/selectpicker/bootstrap-select.min.css",
                      "~/Plugins/jqueryui/jquery-ui.css",
                      "~/Plugins/toastr/toastr.min.css",
                      "~/Content/site.css",
                      "~/Content/Spinner.css"
                      ));
        

            bundles.Add(new ScriptBundle("~/bundles/nexenus").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/jquery-3.4.1.js",
                        //"~/Scripts/bootstrap.min.js",
                        "~/Scripts/adminlte/adminlte.js",
                        //"~/Scripts/adminlte/selectpicker/bootstrap-select.js",
                        "~/Plugins/inputmask/jquery.inputmask.min.js",
                        "~/Plugins/pwstrength/pwstrength-bootstrap.min.js",
                        "~/Plugins/pwstrength/zxcvbn.js",
                        "~/Plugins/summernote/summernote-bs4.js",
                        "~/Plugins/bs-stepper/js/bs-stepper.min.js",
                        "~/Plugins/toastr/toastr.min.js",
                        "~/Scripts/JsGrid/jsgrid.min.js",
                        "~/Plugins/jqueryui/jquery-ui.js",
                        "~/Scripts/GlobalWf.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/dialog").Include(
                     "~/Scripts/modernizr-*",
                     "~/Scripts/jquery-3.4.1.js",
                     "~/Scripts/bootstrap.min.js",
                     "~/Scripts/adminlte/selectpicker/bootstrap-select.js",                     
                     "~/Plugins/toastr/toastr.min.js",
                     "~/Scripts/JsGrid/jsgrid.min.js",
                     "~/Scripts/GlobalWf.js"
         ));

        }
    }
}
