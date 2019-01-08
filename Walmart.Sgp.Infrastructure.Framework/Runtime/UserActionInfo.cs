using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Informações sobre uma ação do usuário.
    /// </summary>
    [DebuggerDisplay("{Id}")]
    public class UserActionInfo : IEquatable<UserActionInfo>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserActionInfo"/>.
        /// </summary>
        /// <param name="id">O id da ação.</param>
        public UserActionInfo(string id)
        {
            Id = id;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o id da ação.
        /// </summary>
        public string Id { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Compara com obj.
        /// </summary>
        /// <param name="obj">O outro objeto a ser comparado.</param>
        /// <returns>True se é igual.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as UserActionInfo);
            
        }

        /// <summary>
        /// Compara com obj.
        /// </summary>
        /// <param name="other">O outro objeto a ser comparado.</param>
        /// <returns>True se é igual.</returns>
        public bool Equals(UserActionInfo other)
        {
            return other != null && other.Id.Equals(Id);
        }

        /// <summary>
        /// Obtém o código hash.
        /// </summary>
        /// <returns>O código.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
