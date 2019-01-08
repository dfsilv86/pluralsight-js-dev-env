using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um serviço de categoria.
    /// </summary>
    public interface ICategoriaService
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
        /// <param name="dsCategoria">Descrição da categoria.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As categorias.</returns>
        IEnumerable<Categoria> PesquisarPorCategoriaDepartamentoESistema(int? cdCategoria, string dsCategoria, byte cdSistema, int? cdDepartamento, Paging paging);

        /// <summary>
        /// Obtém o ID da categoria por código do sistema e código da categoria.
        /// </summary>
        /// <remarks>
        /// Apenas categorias ativa são consideradas.
        /// </remarks>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <returns>O id da categoria.</returns>
        int ObterIDCategoria(int cdSistema, int cdCategoria);

        /// <summary>
        /// Obtém as categorias do sistema.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <returns>As categorias.</returns>
        IEnumerable<Categoria> ObterPorSistema(int cdSistema);
    }
}
