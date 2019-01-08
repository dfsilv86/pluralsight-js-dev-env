using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento.Roteirizacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a RoteiroPedido.
    /// </summary>
    public class RoteiroPedidoService : EntityDomainServiceBase<RoteiroPedido, IRoteiroPedidoGateway>, IRoteiroPedidoService
    {
        private static String[] s_auditProperties = new String[] { "blAutorizado" };
        private readonly IRoteiroGateway m_roteiroGateway;
        private readonly IAuditService m_auditService;

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RoteiroPedidoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para RoteiroPedido.</param>
        /// <param name="roteiroGateway">O table data gateway para Roteiro.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public RoteiroPedidoService(IRoteiroPedidoGateway mainGateway, IRoteiroGateway roteiroGateway, IAuditService auditService)
            : base(mainGateway)
        {
            this.m_roteiroGateway = roteiroGateway;
            this.m_auditService = auditService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém os pedidos roteirizados pelos filtros
        /// </summary>
        /// <param name="dtPedido">Data do pedido</param>
        /// <param name="idDepartamento">ID do departamento</param>
        /// <param name="cdV9D">Código Vendor 9 Dig.</param>
        /// <param name="stPedido">Status do pedido (Autorizado(true) ou Não autorizado (false))</param>
        /// <param name="roteiro">Descrição do roteiro</param>
        /// <param name="paging">Parametro de paginação</param>
        /// <returns>Lista de PedidoRoteirizadoConsolidado</returns>
        public IEnumerable<PedidoRoteirizadoConsolidado> ObterPedidosRoteirizados(DateTime dtPedido, int idDepartamento, long? cdV9D, bool? stPedido, string roteiro, Paging paging)
        {
            Assert(new { orderDate = dtPedido, IdDepartamento = idDepartamento }, new AllMustBeInformedSpec());

            var result = this.MainGateway.ObterPedidosRoteirizados(dtPedido, idDepartamento, cdV9D, stPedido, roteiro, paging);

            if (result != null)
            {
                foreach (var r in result)
                {
                    // Code-review: existem duas consultas dentro de CalcularTotalRoteiro que não se diferem pelo usarQtdRoteiroRA
                    // Fazer um refactoring no código onde essas  consultas não sejam repetidas para as linhas abaixo.
                    r.TotalPedido = this.CalcularTotalRoteiro(r.idRoteiro, dtPedido, false);
                    r.TotalPedidoRA = this.CalcularTotalRoteiro(r.idRoteiro, dtPedido, true);
                }
            }

            return result;
        }

        /// <summary>
        /// Autoriza um roteiro.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">Data do pedido.</param>
        public void AutorizarPedidos(int idRoteiro, DateTime dtPedido)
        {
            var roteirosPedidos = this.MainGateway.ObterRoteirosPedidosParaAutorizar(idRoteiro, dtPedido);
            var dt = DateTime.Now;
            if (roteirosPedidos == null)
            {
                return;
            }

            foreach (var roteiroPedido in roteirosPedidos)
            {
                var entidade = this.MainGateway.FindById(roteiroPedido.IDRoteiroPedido);

                entidade.blAutorizado = true;
                entidade.dhAutorizacao = dt;
                entidade.idUsuarioAutorizacao = RuntimeContext.Current.User.Id;

                this.MainGateway.Update(entidade);
                this.m_auditService.LogUpdate(entidade, s_auditProperties);
            }
        }

        /// <summary>
        /// Busca uma lista de RoteiroPedido
        /// </summary>
        /// <param name="idRoteiro">Id do Roteiro</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <param name="paging">Parametro de Paginação.</param>
        /// <returns>uma lista de RoteiroPedido</returns>
        public IEnumerable<RoteiroPedido> ObterRoteirosPedidosPorRoteiroEdtPedido(int idRoteiro, DateTime dtPedido, Paging paging)
        {
            Assert(new { IdRoteiro = idRoteiro, DtPedido = dtPedido }, new AllMustBeInformedSpec());

            var result = this.MainGateway.ObterRoteirosPedidosPorRoteiroEdtPedido(idRoteiro, dtPedido, paging);

            if (result != null)
            {
                foreach (var r in result)
                {
                    r.QtdSolicitada = this.CalcularTotalRoteiroItem(r.idRoteiro, dtPedido, false, r.ItemDetalhe.IDItemDetalhe);
                    r.QtdRoteiroRA = this.CalcularTotalRoteiroItem(r.idRoteiro, dtPedido, true, r.ItemDetalhe.IDItemDetalhe);
                }
            }

            return result;
        }

        /// <summary>
        /// Calcula o Pedido com base no campo informado
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="usarQtdRoteiroRA">Se TRUE: QtdRoteiroRA, se FALSE: qtdPackCompra)</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <returns>O total.</returns>
        public int CalcularTotalRoteiroItem(int idRoteiro, DateTime dtPedido, bool usarQtdRoteiroRA, int idItemDetalhe)
        {
            Assert(new { IdRoteiro = idRoteiro, UsarQtdRoteiroRA = usarQtdRoteiroRA, DtPedido = dtPedido }, new AllMustBeInformedSpec());

            var sugestoes = this.MainGateway.BuscarSugestaoPedidoPorRoteiroItem(idRoteiro, dtPedido, idItemDetalhe);

            var roteiro = this.m_roteiroGateway.FindById(idRoteiro);

            return SomarEConverterPesoQuilo(roteiro, sugestoes, usarQtdRoteiroRA);
        }

        /// <summary>
        /// Calcula o TotalPedido com base no campo informado
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="usarQtdRoteiroRA">Se TRUE: QtdRoteiroRA, se FALSE: qtdPackCompra)</param>
        /// <returns>O total.</returns>
        public int CalcularTotalRoteiro(int idRoteiro, DateTime dtPedido, bool usarQtdRoteiroRA)
        {
            Assert(new { IdRoteiro = idRoteiro, UsarQtdRoteiroRA = usarQtdRoteiroRA, DtPedido = dtPedido }, new AllMustBeInformedSpec());

            var sugestoes = this.MainGateway.BuscarSugestaoPedidoPorRoteiro(idRoteiro, dtPedido);

            var roteiro = this.m_roteiroGateway.FindById(idRoteiro);

            return SomarEConverterPesoQuilo(roteiro, sugestoes, usarQtdRoteiroRA);
        }

        /// <summary>
        /// Obtém a quantidade de pedidos não autorizados para a data corrente.
        /// </summary>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <returns>Retorna a quantidade de pedidos não autorizados para a data corrente.</returns>
        public int QtdPedidosNaoAutorizadosParaDataCorrente(int idRoteiro)
        {
            Assert(new { idRoteiro }, new AllMustBeInformedSpec());

            return MainGateway.QtdPedidosNaoAutorizadosParaDataCorrente(idRoteiro);
        }

        /// <summary>
        /// Retorna um RoteiroPedido com informacoes de Autorizacao DO ROTEIRO.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Um RoteiroPedido.</returns>
        public RoteiroPedido ObterDadosAutorizacaoRoteiro(int idRoteiro, DateTime dtPedido)
        {
            Assert(new { IdRoteiro = idRoteiro, DtPedido = dtPedido }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterDadosAutorizacao(idRoteiro, dtPedido);
        }

        private static int SomarEConverterPesoQuilo(Roteiro roteiro, IEnumerable<SugestaoPedido> sugestoes, bool usarQtdRoteiroRA)
        {
            if (sugestoes == null)
            {
                return 0;
            }

            if (roteiro.blKgCx)
            {
                return sugestoes.ToList().Sum(s => usarQtdRoteiroRA ? s.QtdSugestaoRoteiroRAToCaixa() : s.QtdPackCompraToCaixa());
            }

            return sugestoes.ToList().Sum(s => usarQtdRoteiroRA ? s.QtdSugestaoRoteiroRAToQuilo() : s.QtdPackCompraToQuilo());
        }
        #endregion
    }
}