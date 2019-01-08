using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Interface de Implementação do Gateway de Fornecedor
    /// </summary>
    public interface IFornecedorGateway : IDataGateway<Fornecedor>
    {
        /// <summary>
        /// Obtém um fornecedor pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O fornecedor.</returns>
        Fornecedor Obter(long id);

        /// <summary>
        /// Obtém um Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>A entidade Fornecedor.</returns>
        Fornecedor ObterPorSistemaCodigo(short cdSistema, int cdFornecedor);

        /// <summary>
        /// Obtém uma lista de Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">Paginação do resultado.</param>
        /// <returns>A lista de entidade Fornecedor.</returns>
        IEnumerable<Fornecedor> ObterListaPorSistemaCodigoNome(short cdSistema, int? cdFornecedor, string nmFornecedor, Paging paging);

        /// <summary>
        /// Obtém o fornecedor ativo com base no código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna o fornecedor juntamente com os parâmetros ativos.</returns>
        Fornecedor ObterAtivoPorCodigoESistemaComProjecao(long cdV9D, int cdSistema);

        /// <summary>
        /// Retorna true se o cdFornecedor informado é do tipo Walmart.
        /// </summary>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>Se o fornecedor é Walmart ou não.</returns>
        bool VerificaVendorWalmart(long cdFornecedor);
    }
}
