using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um serviço de subcategoria.
    /// </summary>
    public interface ISubcategoriaService
    {
        /// <summary>
        /// Obtém a subcategoria pelo código de subcategoria e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A subcategoria.</returns>
        Subcategoria ObterPorSubcategoriaESistema(int cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema);

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
        IEnumerable<Subcategoria> PesquisarPorSubcategoriaCategoriaDepartamentoESistema(int? cdSubcategoria, string dsSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema, Paging paging);
    }
}
