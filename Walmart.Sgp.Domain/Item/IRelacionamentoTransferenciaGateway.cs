using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface de um table data gateway para relacionamento transferencia item.
    /// </summary>
    public interface IRelacionamentoTransferenciaGateway : IDataGateway<RelacionamentoTransferencia>
    {
        /// <summary>
        /// Obtém um RelacionamentoTransferencia pelo seu id, com informações básicas da estrutura mercadológica.
        /// </summary>
        /// <param name="id">O id do relacionamento.</param>
        /// <returns>O RelacionamentoTransferencia com informações básicas da estrutura mercadológica.</returns>
        RelacionamentoTransferencia ObterEstruturadoPorId(int id);

        /// <summary>
        /// Pesquisa detalhe de relacionamentoTransferencia pelos filtros informados.
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os relacionamentos</returns>
        IEnumerable<RelacionamentoTransferenciaConsolidado> PesquisarPorFiltro(RelacionamentoTransferenciaFiltro filtro, Paging paging);

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamento</returns>
        IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionados(long idItemDetalheDestino, Paging paging);

        /// <summary>
        /// Obtem a quantidade de registros pelo item destino
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <returns>A quantidade de registros</returns>
        int ObterQuantidadePorItemDestino(long idItemDetalheDestino);

        /// <summary>
        /// Pesquina os relacionamento conforme os filtros
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item detalhe</param>
        /// <param name="idItemDetalheOrigem">O id do item origem</param>
        /// <param name="idLoja">O id da loja</param>
        /// <param name="blAtivo">O status</param>
        /// <returns>Os relacionamentos</returns>
        IEnumerable<RelacionamentoTransferencia> PesquisarPorItemDestinoOrigemLojaAtivo(long idItemDetalheDestino, long idItemDetalheOrigem, int idLoja, bool blAtivo = true);

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="cdItemDestino">O codigo do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamentos</returns>
        IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionadosPorCdItemDestino(long cdItemDestino, Paging paging);
    }
}
