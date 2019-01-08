using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um table data gateway para parâmetro de fornecedor.
    /// </summary>
    public interface IFornecedorParametroGateway : IDataGateway<FornecedorParametro>
    {
        /// <summary>
        /// Verifica se existe itens DSD para um Vendor
        /// </summary>
        /// <param name="cdV9D">O codigo 9 digitos do vendor.</param>
        /// <returns>Um valor que indica se o Vendor possui ou não itens DSD.</returns>
        bool PossuiItensDSD(long cdV9D);

        /// <summary>
        /// Pesquisa parametros de fornecedores baseado nos filtros informados.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="stFornecedor">O status do fornecedor.</param>
        /// <param name="cdV9D">O código do vendor (9 dígitos).</param>
        /// <param name="nmFornecedor">A razão social.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parâmetros de fornecedores que se encaixam no filtro informado.</returns>
        IEnumerable<FornecedorParametro> PesquisarPorFiltro(int cdSistema, string stFornecedor, long? cdV9D, string nmFornecedor, Paging paging);

        /// <summary>
        /// Busca o parametro do fornecedor juntamente com o fornecedor.
        /// </summary>
        /// <param name="id">O identificador do parametro do fornecedor.</param>
        /// <returns>O parametro do fornecedor juntamente com o fornecedor.</returns>
        FornecedorParametro ObterEstruturadoPorId(int id);

        /// <summary>
        /// Busca review dates por detalhe.
        /// </summary>
        /// <param name="idFornecedorParametro">O id do fornecedor parâmetro.</param>
        /// <param name="detalhe">O tipo de detalhamento.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os review dates filtrados por detalhe.</returns>
        IEnumerable<FornecedorParametroReviewDate> ObterReviewDatesPorDetalhe(int idFornecedorParametro, TipoDetalhamentoReviewDate detalhe, Paging paging);

        /// <summary>
        /// Pesquisa parametros de fornecedor.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdTipo">O canal do vendor, se aplicável.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parametros de fornecedor encontrados.</returns>
        IEnumerable<FornecedorParametro> PesquisarPorSistemaCodigo9DigitosENomeFornecedor(int cdSistema, string cdTipo, long? cdV9D, string nmFornecedor, Paging paging);

        /// <summary>
        /// Localiza um parâmetro de fornecedor pelo código de 9 dígitos e nome do fornecedor.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdTipo">O canal do vendor, se aplicável.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <returns>O parâmetro de fornecedor.</returns>
        FornecedorParametro ObterPorSistemaECodigo9Digitos(int cdSistema, string cdTipo, long? cdV9D);
    }
}
