using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de Roteiro.
    /// </summary>
    public interface IRoteiroService : IDomainService<Roteiro>
    {
        /// <summary>
        /// Obtém os roteiros dos fornecedores.
        /// </summary>
        /// <param name="cdV9D">O código 9 dígitos do fornecedor.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="roteiro">O nome do roteiro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A lista contendo os roteiros dos fornecedores.</returns>
        IEnumerable<Roteiro> ObterRoteirosPorFornecedor(long? cdV9D, int? cdDepartamento, int? cdLoja, string roteiro, Paging paging);

        /// <summary>
        /// Obtém uma lista de SugestaoPedido com Loja populada
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="paging">A paginação. (OPCIONAL)</param>
        /// <returns>Uma lista de sugestaoPedido.</returns>
        IEnumerable<SugestaoPedido> ObterSugestaoPedidoLoja(int idRoteiro, DateTime dtPedido, int idItemDetalhe, Paging paging);

        /// <summary>
        /// Salva uma lista de SugestaoPedido convertida para exibição em Caixa.
        /// </summary>
        /// <param name="sugestoesConvertidas">As sugestoes convertidas.</param>
        /// <param name="idRoteiro">O id do roteiro.</param>
        void SalvarSugestaoPedidoConvertidoCaixa(IEnumerable<SugestaoPedido> sugestoesConvertidas, int idRoteiro);

        /// <summary>
        /// Obtém um Roteiro estruturado pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade Roteiro.</returns>
        Roteiro ObterEstruturadoPorId(int id);
    }
}
