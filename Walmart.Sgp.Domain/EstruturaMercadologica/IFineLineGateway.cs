using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para fineline.
    /// </summary>
    public interface IFineLineGateway
    {
        /// <summary>
        /// Obtém o fineline pelo código de fineline e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código de fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O fineline.</returns>
        FineLine ObterPorFineLineESistema(int cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema);

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
        IEnumerable<FineLine> PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(int? cdFineLine, string dsFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema, Paging paging);
    }
}
