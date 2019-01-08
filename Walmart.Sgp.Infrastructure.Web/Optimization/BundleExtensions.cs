using System.Web.Optimization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Web.Optimization
{
    /// <summary>
    /// Extensions methods para bundles.
    /// </summary>
    public static class BundleExtensions
    {
        /// <summary>
        /// Faz com que os arquivos entregues pelo bundle recebam uma query string de versão.
        /// </summary>
        /// <param name="bundle">O bundle.</param>
        /// <returns>Bundle versionado.</returns>
        public static Bundle AsVersioned(this Bundle bundle)
        {
            ExceptionHelper.ThrowIfNull("bundle", bundle);

            bundle.Transforms.Add(new FileHashVersionBundleTransform());

            return bundle;
        }

        /// <summary>
        /// Faz com que os arquivos entregues pelos bundles recebam uma query string de versão.
        /// </summary>
        /// <param name="bundles">Os bundles.</param>
        /// <returns>Bundles versionados.</returns>
        public static BundleCollection AsVersioned(this BundleCollection bundles)
        {
            ExceptionHelper.ThrowIfNull("bundles", bundles);

            foreach (var bundle in bundles)
            {
                bundle.AsVersioned();
            }

            return bundles;
        }

        /// <summary>
        /// Não tenta inferir qualquer tipo de ordenação ao renderizar os recursos e 
        /// assim garante que a ordenação dos recursos na página html será a mesma que a definida dentro dos bundles.
        /// </summary>
        /// <param name="bundle">O bundle.</param>
        /// <returns>O bundle não ordenado.</returns>
        public static Bundle AsNonOrdering(this Bundle bundle)
        {
            ExceptionHelper.ThrowIfNull("bundle", bundle);

            bundle.Orderer = new NonOrderingBundleOrderer();

            return bundle;
        }

        /// <summary>
        /// Não tenta inferir qualquer tipo de ordenação ao renderizar os recursos e 
        /// assim garante que a ordenação dos recursos na página html será a mesma que a definida dentro dos bundles.
        /// </summary>
        /// <param name="bundles">Os bundles.</param>
        /// <returns>Os bundles não ordenados.</returns>
        public static BundleCollection AsNonOrdering(this BundleCollection bundles)
        {
            ExceptionHelper.ThrowIfNull("bundles", bundles);

            foreach (var bundle in bundles)
            {
                bundle.AsNonOrdering();
            }

            return bundles;
        }
    }
}
