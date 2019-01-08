namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Define a interface de permissões durante a execução da aplicação.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface IRuntimePermission
    {
        #region Properties
        /// <summary>
        /// Obtém o nome da controller relacionada a permissão.
        /// </summary>
        string ControllerName { get; }

        /// <summary>
        /// Obtém o nome da action relacionada a permissão.
        /// </summary>
        string ActionName { get; }
        #endregion
    }
}
