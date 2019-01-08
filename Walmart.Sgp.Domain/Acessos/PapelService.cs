using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Serviço de domínio relacionado a papel.
    /// </summary>
    public class PapelService : EntityDomainServiceBase<Papel, IPapelGateway>, IPapelService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PapelService"/>.
        /// </summary>
        /// <param name="papelGateway">O table data gateway para papel.</param>
        public PapelService(IPapelGateway papelGateway)
            : base(papelGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o papel pelo nome.
        /// </summary>
        /// <param name="nome">O nome.</param>
        /// <returns>O papel, se existir.</returns>
        public Papel ObterPorNome(string nome)
        {
            return MainGateway.Find("Name = @nome", new { nome }).SingleOrDefault();
        }
        #endregion
    }
}
