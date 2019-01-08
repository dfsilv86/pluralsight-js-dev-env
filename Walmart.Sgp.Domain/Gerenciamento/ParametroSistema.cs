using System;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma parametro sistema.
    /// </summary>
    public class ParametroSistema : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDParametroSistema;
            }

            set
            {
                IDParametroSistema = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDParametroSistema.
        /// </summary>
        public int IDParametroSistema { get; set; }

        /// <summary>
        /// Obtém ou define nmParametroSistema.
        /// </summary>
        public string nmParametroSistema { get; set; }

        /// <summary>
        /// Obtém ou define dsParametroSistema.
        /// </summary>
        public string dsParametroSistema { get; set; }

        /// <summary>
        /// Obtém ou define vlParametroSistema.
        /// </summary>
        public string vlParametroSistema { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? dhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? cdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? cdUsuarioAtualizacao { get; set; }
        #endregion
    }
}
