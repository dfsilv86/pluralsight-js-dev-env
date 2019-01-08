using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Informações sobre uma menu do usuário.
    /// </summary>
    [DebuggerDisplay("{Route}")]
    public class UserMenuInfo : IEquatable<UserMenuInfo>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserMenuInfo"/>.
        /// </summary>
        /// <param name="route">A rota.</param>
        public UserMenuInfo(string route)
        {
            Route = route;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define a rota da ação.
        /// </summary>
        public string Route { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Compara com obj.
        /// </summary>
        /// <param name="obj">O outro objeto a ser comparado.</param>
        /// <returns>True se é igual.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as UserMenuInfo);
        }

        /// <summary>
        /// Compara com obj.
        /// </summary>
        /// <param name="other">O outro objeto a ser comparado.</param>
        /// <returns>True se é igual.</returns>
        public bool Equals(UserMenuInfo other)
        {
            return other != null && other.Route.Equals(Route);
        }

        /// <summary>
        /// Obtém o código hash.
        /// </summary>
        /// <returns>O código.</returns>
        public override int GetHashCode()
        {
            return Route.GetHashCode();
        }
        #endregion
    }
}
