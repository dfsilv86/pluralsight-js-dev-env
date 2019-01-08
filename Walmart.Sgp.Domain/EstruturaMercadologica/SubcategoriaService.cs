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
    /// Serviço de domínio relacionado a subcategoria.
    /// </summary>
    public class SubcategoriaService : DomainServiceBase<ISubcategoriaGateway>, ISubcategoriaService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SubcategoriaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para subcategoria.</param>
        public SubcategoriaService(ISubcategoriaGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém a subcategoria pelo código de subcategoria e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A subcategoria.</returns>
        public Subcategoria ObterPorSubcategoriaESistema(int cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            return this.MainGateway.ObterPorSubcategoriaESistema(cdSubcategoria, cdCategoria, cdDepartamento, cdSistema);
        }

        /// <summary>
        /// Pesquisa subcategorias filtrando pelo código da subcategoria, descrição da subcategoria, código da categoria, código do departamento, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="dsSubcategoria">Descrição da subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As subcategorias.</returns>
        public IEnumerable<Subcategoria> PesquisarPorSubcategoriaCategoriaDepartamentoESistema(int? cdSubcategoria, string dsSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorSubcategoriaCategoriaDepartamentoESistema(cdSubcategoria, dsSubcategoria, cdCategoria, cdDepartamento, cdSistema, paging);
        }
        #endregion
    }
}
