namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    using System.Globalization;

    /// <summary>
    /// Define a interface de um contexto de execução.
    /// </summary>
    public interface IRuntimeContext
    {
        #region Properties
        /// <summary>
        /// Obtém o usuário corrente.
        /// </summary>
        IRuntimeUser User { get; }

        /// <summary>
        /// Obtém cultura.
        /// </summary>
        CultureInfo Culture { get; }
        #endregion
    }
}
