using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a RoteiroLoja.
    /// </summary>
    public class RoteiroLojaService : EntityDomainServiceBase<RoteiroLoja, IRoteiroLojaGateway>, IRoteiroLojaService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RoteiroLojaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para RoteiroLoja.</param>
        public RoteiroLojaService(IRoteiroLojaGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém as lojas válidas para vínculo com o roteiro.
        /// </summary>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="dsEstado">O estado da loja.</param>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A lista contendo as lojas válidas para vínculo com o roteiro.</returns>
        public IEnumerable<RoteiroLoja> ObterLojasValidas(long cdV9D, string dsEstado, int? idRoteiro, Paging paging)
        {
            Assert(new { Vendor9Digits = cdV9D }, new AllMustBeInformedSpec());
            
            return this.MainGateway.ObterLojasValidas(cdV9D, dsEstado, idRoteiro, paging);
        }

        /// <summary>
        /// Obtém as lojas pelo roteiro.
        /// </summary>
        /// <param name="idRoteiro">O id.</param>
        /// <returns>A lista de lojas.</returns>
        public IEnumerable<RoteiroLoja> ObterPorIdRoteiro(long idRoteiro)
        {
            Assert(new { IdRoteiro = idRoteiro }, new AllMustBeInformedSpec());

            return MainGateway.ObterPorIdRoteiro(idRoteiro);
        }
        #endregion
    }
}