using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Serviço de domínio relacionado a relacionamento transferencia entre itens.
    /// </summary>
    public class RelacionamentoTransferenciaService : EntityDomainServiceBase<RelacionamentoTransferencia, IRelacionamentoTransferenciaGateway>, IRelacionamentoTransferenciaService
    {
        #region Fields
        private readonly IItemDetalheGateway m_itemDetalheGateway;
        private readonly ILogRelacionamentoTransferenciaGateway m_logRelacionamentoTransferenciaGateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacionamentoTransferenciaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para relacionamento transferencia.</param>                
        /// <param name="itemDetalheGateway">O table data gateway para item detalhe.</param>
        /// <param name="logRelacionamentoTransferenciaGateway">O table data gateway para log.</param>
        public RelacionamentoTransferenciaService(
            IRelacionamentoTransferenciaGateway mainGateway,
            IItemDetalheGateway itemDetalheGateway,
            ILogRelacionamentoTransferenciaGateway logRelacionamentoTransferenciaGateway)
            : base(mainGateway)
        {
            m_itemDetalheGateway = itemDetalheGateway;
            m_logRelacionamentoTransferenciaGateway = logRelacionamentoTransferenciaGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém um RelacionamentoTransferencia pelo seu id, com informações básicas da estrutura mercadológica.
        /// </summary>
        /// <param name="id">O id do relacionamento.</param>
        /// <returns>O RelacionamentoTransferencia com informações básicas da estrutura mercadológica.</returns>
        public RelacionamentoTransferencia ObterEstruturadoPorId(int id)
        {
            return this.MainGateway.ObterEstruturadoPorId(id);
        }

        /// <summary>
        /// Pesquisa detalhe de relacionamentoTransferencia pelos filtros informados.
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os relacionamentos</returns>
        public IEnumerable<RelacionamentoTransferenciaConsolidado> PesquisarPorFiltro(RelacionamentoTransferenciaFiltro filtro, Paging paging)
        {
            return this.MainGateway.PesquisarPorFiltro(filtro, paging);
        }

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamento</returns>
        public IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionados(long idItemDetalheDestino, Paging paging)
        {
            return this.MainGateway.PesquisarItensRelacionados(idItemDetalheDestino, paging);
        }

        /// <summary>
        /// Cria o relacionamento de transferencia entre item destino e item origem por loja
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <param name="idItemDetalheOrigem">O id do item origem</param>
        /// <param name="lojas">As lojas</param>
        public void CriarTransferencia(long idItemDetalheDestino, long idItemDetalheOrigem, Loja[] lojas)
        {
            var idUsuario = RuntimeContext.Current.User.Id;

            // Atualiza o campo BlItemTransferencia para true
            var item = this.m_itemDetalheGateway.ObterEstruturadoPorId((int)idItemDetalheDestino);
            item.BlItemTransferencia = true;
            this.m_itemDetalheGateway.Update(item);

            // Cria relacionamentos
            foreach (var loja in lojas)
            {
                // Cria log
                m_logRelacionamentoTransferenciaGateway.Insert(new LogRelacionamentoTransferencia
                {
                    IDItemDetalheOrigem = idItemDetalheOrigem,
                    IDItemDetalheDestino = idItemDetalheDestino,
                    IDLoja = loja.Id,
                    dtCriacao = DateTime.Now,
                    IDUsuario = idUsuario,
                    tpOperacao = "I"
                });

                // Remove os relacionamentos inativos conforme os parametros
                var rts = this.MainGateway.PesquisarPorItemDestinoOrigemLojaAtivo(idItemDetalheDestino, idItemDetalheOrigem, loja.Id, false);
                rts.ToList().ForEach(_ =>
                {
                    this.MainGateway.Delete(_.Id);
                });

                // Add relacionamento
                this.MainGateway.Insert(new RelacionamentoTransferencia
                {
                    IDItemDetalheDestino = idItemDetalheDestino,
                    IDItemDetalheOrigem = idItemDetalheOrigem,
                    IDLoja = loja.Id,
                    dtCriacao = DateTime.Now,
                    IDUsuario = idUsuario,
                    blAtivo = true
                });
            }
        }

        /// <summary>
        /// Remove o relacionamento de transferencia
        /// </summary>
        /// <param name="items">Os itens</param>        
        public void RemoverTransferencias(RelacionamentoTransferencia[] items)
        {
            var idUsuario = RuntimeContext.Current.User.Id;

            // Remove relacionamentos
            items.ToList().ForEach(_ =>
            {
                // Cria log
                m_logRelacionamentoTransferenciaGateway.Insert(new LogRelacionamentoTransferencia
                {
                    IDItemDetalheOrigem = _.IDItemDetalheOrigem,
                    IDItemDetalheDestino = _.IDItemDetalheDestino,
                    IDLoja = _.Loja.IDLoja,
                    dtCriacao = DateTime.Now,
                    IDUsuario = idUsuario,
                    tpOperacao = "E"
                });

                // Inativa os relacionamentos
                var r = this.MainGateway.ObterEstruturadoPorId(_.Id);
                r.blAtivo = false;
                r.dtInativo = DateTime.Now;
                r.IDUsuario = idUsuario;
                this.MainGateway.Update(r);
            });

            // Verifica se não existe relacionamentos para aquele item destino, caso não houver, setar false para BlItemTransferencia do item
            var idItemDetalheDestino = items[0].IDItemDetalheDestino;
            var count = ObterQuantidadePorItemDestino(idItemDetalheDestino);
            if (count <= 0)
            {
                var item = this.m_itemDetalheGateway.ObterEstruturadoPorId((int)idItemDetalheDestino);
                item.BlItemTransferencia = false;
                this.m_itemDetalheGateway.Update(item);
            }
        }

        /// <summary>
        /// Obtem a quantidade de registros pelo item destino
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <returns>A quantidade de registros</returns>
        public int ObterQuantidadePorItemDestino(long idItemDetalheDestino)
        {
            return this.MainGateway.ObterQuantidadePorItemDestino(idItemDetalheDestino);
        }

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="cdItemDestino">O codigo do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamentos</returns>
        public IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionadosPorCdItemDestino(long cdItemDestino, Paging paging)
        {
            return this.MainGateway.PesquisarItensRelacionadosPorCdItemDestino(cdItemDestino, paging);
        }
        #endregion
    }
}
