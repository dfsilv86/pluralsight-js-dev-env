using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para categoria de inventário em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryCategoriaGateway : MemoryDataGateway<Categoria>, ICategoriaGateway
    {
        /// <summary>
        /// Obtém uma categoria a partir do código da categoria e do código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>A categoria.</returns>
        public Categoria ObterPorCategoriaESistema(int cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa categorias filtrando pelo código da categoria, descrição da categoria, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="dsCategoria">The ds categoria.</param>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As categorias.</returns>
        public IEnumerable<Categoria> PesquisarPorCategoriaDepartamentoESistema(int? cdCategoria, string dsCategoria, byte cdSistema, int? cdDepartamento, Paging paging)
        {
            throw new NotImplementedException();
        }
    }
}