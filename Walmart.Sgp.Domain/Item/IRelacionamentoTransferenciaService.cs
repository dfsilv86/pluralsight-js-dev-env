using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface de um serviço de relacionamento transferencia de itens.
    /// </summary>
    public interface IRelacionamentoTransferenciaService
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
        /// Cria o relacionamento de transferencia entre item destino e item origem por loja
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <param name="idItemDetalheOrigem">O id do item origem</param>
        /// <param name="lojas">As lojas</param>
        void CriarTransferencia(long idItemDetalheDestino, long idItemDetalheOrigem, Loja[] lojas);

        /// <summary>
        /// Remove o relacionamento de transferencia
        /// </summary>
        /// <param name="items">Os itens</param>        
        void RemoverTransferencias(RelacionamentoTransferencia[] items);

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="cdItemDestino">O codigo do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamentos</returns>
        IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionadosPorCdItemDestino(long cdItemDestino, Paging paging);
    }
}
