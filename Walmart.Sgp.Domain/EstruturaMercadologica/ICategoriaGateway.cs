using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para categoria.
    /// </summary>
    public interface ICategoriaGateway : IDataGateway<Categoria>
    {
        /// <summary>
        /// Obtém uma categoria a partir do código da categoria e do código de estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>A categoria.</returns>
        Categoria ObterPorCategoriaESistema(int cdCategoria, int? cdDepartamento, byte cdSistema);

        /// <summary>
        /// Pesquisa categorias filtrando pelo código da categoria, descrição da categoria, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="dsCategoria">The ds categoria.</param>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As categorias.</returns>
        IEnumerable<Categoria> PesquisarPorCategoriaDepartamentoESistema(int? cdCategoria, string dsCategoria, byte cdSistema, int? cdDepartamento, Paging paging);
    }
}
