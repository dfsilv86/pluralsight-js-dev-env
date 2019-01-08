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
    /// Serviço de domínio relacionado a categoria.
    /// </summary>
    public class CategoriaService : DomainServiceBase<ICategoriaGateway>, ICategoriaService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CategoriaService" />.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para categoria.</param>
        public CategoriaService(ICategoriaGateway mainGateway)
                    : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém uma categoria a partir do código da categoria e do código de estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>A categoria.</returns>
        public Categoria ObterPorCategoriaESistema(int cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorCategoriaESistema(cdCategoria, cdDepartamento, cdSistema);
        }

        /// <summary>
        /// Pesquisa categorias filtrando pelo código da categoria, descrição da categoria, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="dsCategoria">The ds categoria.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As categorias.</returns>
        public IEnumerable<Categoria> PesquisarPorCategoriaDepartamentoESistema(int? cdCategoria, string dsCategoria, byte cdSistema, int? cdDepartamento, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorCategoriaDepartamentoESistema(cdCategoria, dsCategoria, cdSistema, cdDepartamento, paging);
        }

        /// <summary>
        /// Obtém o ID da categoria por código do sistema e código da categoria.
        /// </summary>
        /// <remarks>
        /// Apenas categorias ativa são consideradas.
        /// </remarks>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <returns>O id da categoria.</returns>
        public int ObterIDCategoria(int cdSistema, int cdCategoria)
        {
            return MainGateway.Find<int>("IDCategoria", "cdSistema = @cdSistema AND cdCategoria = @cdCategoria AND blAtivo = @blAtivo", new { cdSistema, cdCategoria, blAtivo = true }).First();
        }

        /// <summary>
        /// Obtém as categorias do sistema.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <returns>As categorias.</returns>
        public IEnumerable<Categoria> ObterPorSistema(int cdSistema)
        {
            return MainGateway.Find("cdSistema = @cdSistema AND blAtivo = @blAtivo", new { cdSistema = cdSistema, blAtivo = true });
        }
        #endregion
    }
}
