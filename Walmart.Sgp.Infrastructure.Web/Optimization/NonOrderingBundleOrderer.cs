namespace Walmart.Sgp.Infrastructure.Web.Optimization
{
    using System.Collections.Generic;
    using System.Web.Optimization;

    /// <summary>
    /// Implementação de IBundleOrderer que não faz ordenação.
    /// </summary>
    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        /// <summary>
        /// Orders the files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="files">The files.</param>
        /// <returns>Ordered files.</returns>
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
