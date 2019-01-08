namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    using System.Globalization;

    /// <summary>
    /// Contexto de execução em memória.
    /// <remarks>Utilizado principalmente para questões de teste.</remarks>
    /// </summary>
    public class MemoryRuntimeContext : IRuntimeContext
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryRuntimeContext"/>.
        /// </summary>
        public MemoryRuntimeContext()
        {
            User = new MemoryRuntimeUser()
            {
                UserName = "memory",
                FullName = "The memory",
                RoleName = "Guest",
                RoleId = 1,
                Id = 1,
            };

            Culture = new CultureInfo("pt-BR");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o usuário corrente.
        /// </summary>
        public IRuntimeUser User { get; set; }

        /// <summary>
        /// Obtém ou define a cultura.
        /// </summary>
        public CultureInfo Culture { get; set; }
        #endregion
    }
}
