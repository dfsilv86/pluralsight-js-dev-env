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
    /// Serviço de domínio relacionado a fineline.
    /// </summary>
    public class FineLineService : DomainServiceBase<IFineLineGateway>, IFineLineService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FineLineService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para fineline.</param>
        public FineLineService(IFineLineGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o fineline pelo código de fineline e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código de fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O fineline.</returns>
        public FineLine ObterPorFineLineESistema(int cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            return this.MainGateway.ObterPorFineLineESistema(cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema);
        }

        /// <summary>
        /// Pesquisa finelines filtrando pelo código do fineline, descrição do fineline, código da subcategoria, código da categoria, código do departamento, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="dsFineLine">A descrição do fineline.</param>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os finelines.</returns>
        public IEnumerable<FineLine> PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(int? cdFineLine, string dsFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());            

            return this.MainGateway.PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(cdFineLine, dsFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, paging);
        }
        #endregion
    }
}
