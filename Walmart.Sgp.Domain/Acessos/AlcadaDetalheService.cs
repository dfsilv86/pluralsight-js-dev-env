using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Serviço de domínio relacionado a AlcadaDetalhe.
    /// </summary>
    public class AlcadaDetalheService : EntityDomainServiceBase<AlcadaDetalhe, IAlcadaDetalheGateway>, IAlcadaDetalheService
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AlcadaDetalheService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para main data.</param>
        public AlcadaDetalheService(IAlcadaDetalheGateway mainGateway)
            : base(mainGateway)
        {
        }

        /// <summary>
        /// Obter uma lista de AlcadaDetalhe por IdAlcada.
        /// </summary>
        /// <param name="idAlcada">O id da Alcada.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>A lista de AlcadaDetalhe.</returns>
        public IEnumerable<AlcadaDetalhe> ObterPorIdAlcada(int idAlcada, Paging paging)
        {
            Assert(new { Competence = idAlcada }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorIdAlcada(idAlcada, paging);
        }
    }
}
