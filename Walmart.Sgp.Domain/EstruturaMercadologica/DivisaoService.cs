using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado à divisão.
    /// </summary>
    public class DivisaoService : DomainServiceBase<IDivisaoGateway>, IDivisaoService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DivisaoService" />.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para divisao.</param>
        public DivisaoService(IDivisaoGateway mainGateway)
                    : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém a divisão pelo código da divisão e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A divisão.</returns>
        public Divisao ObterPorDivisaoESistema(int cdDivisao, byte cdSistema)
        {
            return this.MainGateway.ObterPorDivisaoESistema(cdDivisao, cdSistema);
        }

        /// <summary>
        /// Pesquisa divisões filtrando pelo código de divisão, descrição da divisão e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="dsDivisao">A descrição da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A divisão.</returns>
        /// <remarks>Não valida o usuário que está efetuando a pesquisa. O filtro da descrição é Contains.</remarks>
        public IEnumerable<Divisao> PesquisarPorSistema(int? cdDivisao, string dsDivisao, byte cdSistema, Paging paging)
        {
            // Só é necessário validar se o sistema foi informado.
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorSistema(cdDivisao, dsDivisao, cdSistema, paging);
        }

        #endregion
    }
}
