namespace Walmart.Sgp.WebApp
{
    using System.Web.Optimization;
    using Walmart.Sgp.Infrastructure.Web.Optimization;

    /// <summary>
    /// Classe para as configurações de bundle.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Registra os arquivos para bundles na aplicação.
        /// </summary>
        /// <remarks>
        /// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725.
        /// Use the development version of Modernizr to develop with and learn from. Then, when you're
        /// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        /// </remarks>
        /// <param name="bundles">Coleção de bundles da aplicação onde serão adicinodas novas configurações.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles).AsVersioned().AsNonOrdering();
            RegisterStyles(bundles).AsVersioned().AsNonOrdering();
#if DEV
#else
            BundleTable.EnableOptimizations = true;
#endif
        }

        /// <summary>
        /// Registra os scripts para compactação.
        /// </summary>
        /// <param name="bundles">Os bundles.</param>
        /// <returns>A coleção de bundles.</returns>
        private static BundleCollection RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/angular-js").Include(
                "~/Scripts/bower/jquery/dist/jquery.js",
                "~/Scripts/bower/bootstrap/dist/js/bootstrap.js",
                "~/Scripts/bower/angular/angular.js"));

            bundles.Add(new ScriptBundle("~/Scripts/angular-plugins-js").Include(
                "~/Scripts/bower/jquery.inputmask/dist/jquery.inputmask.bundle.js",
                "~/Scripts/bower/angular-cache/dist/angular-cache.js",
                "~/Scripts/bower/angular-sanitize/angular-sanitize.js",
                "~/Scripts/bower/angular-ui/build/angular-ui.js",
                "~/Scripts/bower/angular-input-masks/angular-input-masks-dependencies.js",
                "~/Scripts/bower/angular-input-masks/angular-input-masks.br.js",
                "~/Scripts/bower/angular-ui-router/release/angular-ui-router.js",
                "~/Scripts/bower/angular-bootstrap/ui-bootstrap.js",
                "~/Scripts/bower/angular-bootstrap/ui-bootstrap-tpls.js",
                "~/Scripts/bower/angular-translate/angular-translate.js",
                "~/Scripts/bower/angular-file-upload/dist/angular-file-upload.js",
                "~/Scripts/bower/base64/base64.js",
                "~/Scripts/bower/angular-ui-select2/src/select2.js",
                "~/Scripts/bower/select2/select2.js",
                "~/Scripts/bower/select2/select2_locale_pt-BR.js",
                "~/Scripts/bower/moment/min/moment.js",
                "~/Scripts/bower/moment/locale/pt-br.js",
                "~/Scripts/bower/moment-timezone/builds/moment-timezone-with-data.js",
                "~/Scripts/bower/angular-moment/angular-moment.js",
                "~/Scripts/bower/angular-bootstrap-confirm/dist/angular-bootstrap-confirm.js",
                "~/Scripts/bower/AngularJS-Toaster/toaster.js",
                "~/Scripts/bower/ng-dialog/js/ngDialog.js",
                "~/Scripts/bower/ng-mask/ngMask.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/sgp-js").Include(
                 "~/Scripts/polyfills.js")
                     .IncludeDirectory("~/Scripts/common/", "*.js", true)
                     .IncludeDirectory("~/Scripts/app/", "*.js", true));

            return bundles;
        }

        /// <summary>
        /// Registra os estilos para compactação.
        /// </summary>
        /// <param name="bundles">Os bundles.</param>
        /// <returns>A coleção de bundles.</returns>
        private static BundleCollection RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css/bootstrap/bootstrap-css").Include(
                    "~/Scripts/bower/bootstrap/dist/css/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css/theme/plugins/plugins-css").Include(
                    "~/Content/css/theme/plugins/jquery-ui.css",
                    "~/Content/css/theme/plugins/select2.css",
                    "~/Content/css/theme/plugins/uniform.css"));

            bundles.Add(new StyleBundle("~/Content/css/theme/theme-css").Include(
                    "~/Content/css/theme/main.css",
                    "~/Content/css/theme/icons.css"));

            bundles.Add(new StyleBundle("~/Content/css/plugins-css").Include(
                    "~/Scripts/bower/angular-ui/build/angular-ui.css",
                    "~/Scripts/bower/ng-dialog/css/ngDialog.css",
                    "~/Scripts/bower/ng-dialog/css/ngDialog-theme-default.css",
                    "~/Scripts/bower/AngularJS-Toaster/toaster.css"));

            bundles.Add(new StyleBundle("~/Content/css/sgp-css").Include(
                    "~/Content/css/font-awesome.css",
                    "~/Content/css/site.css",
                    "~/Content/css/login.css",
                    "~/Content/css/sgp-grids.css",
                    "~/Content/css/sgp-app.css",
                    "~/Content/css/theme/responsive.css"));

            //////// Incluído diretamente no Index.cshtml para o layout do loader da aplicação
            ////bundles.Add(new StyleBundle("~/Content/css/loader-css").Include(
            ////        "~/Content/css/loader.css"));

            return bundles;
        }
    }
}