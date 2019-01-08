using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a sugestao pedido.
    /// </summary>
    public class SugestaoPedidoService : EntityDomainServiceBase<SugestaoPedido, ISugestaoPedidoGateway>, ISugestaoPedidoService
    {
        #region Fields
        private static String[] s_auditProperties = new String[] { "vlEstoque", "qtdPackCompra", "qtdSugestaoRoteiroRA" };
        private readonly IAlcadaService m_alcadaService;
        private readonly IGradeSugestaoService m_gradeSugestaoService;
        private readonly ILojaService m_lojaService;
        private readonly IAutorizaPedidoService m_autorizaPedidoService;
        private readonly IAuditService m_auditService;
        private readonly IAlcadaDetalheService m_alcadaDetalheService;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoService" />
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        /// <param name="alcadaService">O serviço de alçada.</param>
        /// <param name="gradeSugestaoService">O serviço de grade sugestao.</param>
        /// <param name="lojaService">O serviço de autorização de pedido.</param>
        /// <param name="autorizaPedidoService">O serviço de loja.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        /// <param name="alcadaDetalheService">O serviço de detalhes de alçada.</param>
        public SugestaoPedidoService(
            ISugestaoPedidoGateway mainGateway,
            IAlcadaService alcadaService,
            IGradeSugestaoService gradeSugestaoService,
            ILojaService lojaService,
            IAutorizaPedidoService autorizaPedidoService,
            IAuditService auditService,
            IAlcadaDetalheService alcadaDetalheService)
            : base(mainGateway)
        {
            m_alcadaService = alcadaService;
            m_gradeSugestaoService = gradeSugestaoService;
            m_lojaService = lojaService;
            m_autorizaPedidoService = autorizaPedidoService;
            m_auditService = auditService;
            m_alcadaDetalheService = alcadaDetalheService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Pesquisa sugestões de pedidos pelos filtros informados.
        /// </summary>
        /// <param name="request">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>
        /// As sugestões de pedido.
        /// </returns>
        public IEnumerable<SugestaoPedidoModel> PesquisarPorFiltros(SugestaoPedidoFiltro request, Paging paging)
        {
            Assert(new { User = request.IDUsuario, RequestDate = request.dtPedido, MarketingStructure = request.cdSistema, Store = request.cdLoja }, new AllMustBeInformedSpec());
            Assert(new { Department = request.cdDepartamento }, new AllMustBeInformedSpec(true));

            var result = this.MainGateway.PesquisarPorFiltros(request, paging);

            AplicarInformacoesAlcada(result);

            AplicarInformacaoGrade(result);

            return result;
        }

        /// <summary>
        /// Verifica se o usuário pode autorizar pedido.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O status (L - a loja não permite, D - desabilitado, A - autorizado, P - pendente).</returns>
        /// <remarks>TODO: fixed value</remarks>
        public string ObterStatusAutorizarPedido(DateTime dtPedido, int idLoja, int idDepartamento, int cdSistema)
        {
            Assert(new { dateTime = dtPedido, idLoja, idDepartamento, cdSistema }, new AllMustBeInformedSpec());

            Loja loja = m_lojaService.ObterPorId(idLoja);

            if (!loja.blAutorizaPedido ?? false)
            {
                return "L";
            }

            if (m_autorizaPedidoService.ExisteAutorizacaoPedido(dtPedido, idLoja, idDepartamento))
            {
                return "A";
            }

            if (dtPedido.Date != DateTime.Today || !m_gradeSugestaoService.ExisteGradeSugestaoAberta(cdSistema, loja.IDBandeira.Value, idDepartamento, idLoja, DateTime.Now.ToMilitaryTime()))
            {
                return "D";
            }

            return "P";
        }

        /// <summary>
        /// Autoriza o pedido.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O status resultante (L - a loja não permite, D - desabilitado, A - autorizado, P - pendente).</returns>
        /// <remarks>TODO: fixed value</remarks>
        public string AutorizarPedido(DateTime dtPedido, int idLoja, int idDepartamento, int cdSistema)
        {
            Assert(new { dateTime = dtPedido, idLoja, idDepartamento, cdSistema }, new AllMustBeInformedSpec());

            var status = ObterStatusAutorizarPedido(dtPedido, idLoja, idDepartamento, cdSistema);

            if (status != "P")
            {
                return status;
            }

            m_autorizaPedidoService.AutorizarPedido(dtPedido, idLoja, idDepartamento);

            return "A";
        }

        /// <summary>
        /// Obtém uma sugestão pedido com dados do item detalhe do pedido e da loja.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestao pedido.</param>
        /// <returns>A sugestão pedido.</returns>
        /// <remarks>Usado pelo processo de alterar sugestão pedido.</remarks>
        public SugestaoPedido ObterEstruturado(int idSugestaoPedido)
        {
            Assert(new { idSugestaoPedido }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterEstruturado(idSugestaoPedido);
        }

        /// <summary>
        /// Salva uma lista de sugestões de pedido.
        /// </summary>
        /// <param name="valores">As sugestões.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Resultado da operação.</returns>
        public AlterarSugestoesResponse AlterarSugestoes(IEnumerable<SugestaoPedidoModel> valores, int idLoja, DateTime dtPedido)
        {
            Assert(RuntimeContext.Current.User, new UsuarioPodeAlterarSugestaoSpec(m_alcadaService));

            List<SugestaoPedidoModel> sugestoes = new List<SugestaoPedidoModel>();
            List<SugestaoPedidoModel> inexistentes = new List<SugestaoPedidoModel>();

            foreach (var valoresPostados in valores)
            {
                var sugestao = PrepararSugestaoParaAlteracao(valoresPostados);
                var target = (null == sugestao) ? inexistentes : sugestoes;
                target.Add(sugestao ?? valoresPostados);
            }

            AplicarInformacoesAlcada(sugestoes);

            List<AlterarSugestaoResponse> resultados = new List<AlterarSugestaoResponse>();

            resultados.AddRange(from inexistente in inexistentes select new AlterarSugestaoResponse(inexistente.IDSugestaoPedido) { Inexistente = true });

            if (sugestoes.Count > 0)
            {
                sugestoes.GroupBy(s => s.IDFornecedorParametro).ToList().ForEach(grouped =>
                {
                    resultados.AddRange(AlterarSugestoesFornecedor(grouped.Key, idLoja, grouped, dtPedido));
                });
            }

            return resultados.Summarize();
        }

        /// <summary>
        /// Valida uma alteração de uma sugestão de pedido.
        /// </summary>
        /// <param name="alteracao">Os dados alterados da sugestão de pedido (vlEstoque e qtdPackCompra).</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>O resultado da validação.</returns>
        public SpecResult ValidarAlteracaoSugestao(SugestaoPedidoModel alteracao, int idLoja, int idDepartamento, DateTime dtPedido)
        {
            SugestaoPedidoModel sugestao = this.PrepararSugestaoParaAlteracao(alteracao);

            AplicarInformacoesAlcada(new SugestaoPedidoModel[] { sugestao });

            if (!m_gradeSugestaoService.ExisteGradeSugestaoAberta(sugestao.ItemDetalhePedido.CdSistema, sugestao.Loja.IDBandeira.Value, sugestao.ItemDetalhePedido.IDDepartamento, sugestao.Loja.IDLoja, DateTime.Now.ToMilitaryTime()))
            {
                return new SpecResult(false, Texts.SuggestionGridCannotBeClosed);
            }

            return new AlteracaoSugestaoDeveSerPermitidaSpec().IsSatisfiedBy(sugestao);
        }

        /// <summary>
        /// Obtém informações de sugestão pedido com informações de alçada relacionadas.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestão de pedido.</param>
        /// <returns>
        /// A sugestão de pedido.
        /// </returns>
        public SugestaoPedidoModel ObterEstruturadoComAlcada(int idSugestaoPedido)
        {
            SugestaoPedido sp = ObterEstruturado(idSugestaoPedido);

            SugestaoPedidoModel model = new SugestaoPedidoModel(sp);

            AplicarInformacoesAlcada(new SugestaoPedidoModel[] { model });

            return model;
        }

        /// <summary>
        /// Obtém a quantidade de sugestões de pedidos para a data informada.
        /// </summary>
        /// <param name="dia">A data das sugestões de pedidos.</param>
        /// <returns>A quantidade de sugestões pedidos.</returns>
        public QuantidadeSugestaoPedido ObterQuantidade(DateTime dia)
        {
            var counts = MainGateway.Count(
                new
                {
                    manual = TipoOrigemCalculo.Manual,
                    data = dia.Date
                },
                "dtPedido = @data",
                "cdOrigemCalculo = @manual AND dtPedido = @data").ToArray();

            return new QuantidadeSugestaoPedido
            {
                Dia = dia,
                Total = counts[0],
                TotalOrigemCalculoManual = counts[1]
            };
        }

        /// <summary>
        /// Obtém os logs de auditoria de sugestão pedido.
        /// </summary>
        /// <param name="filter">O filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os logs de auditoria.</returns>
        public IEnumerable<AuditRecord<SugestaoPedido>> ObterLogs(AuditFilter filter, Paging paging)
        {
            return m_auditService.ObterRelatorio<SugestaoPedido>(s_auditProperties, filter, paging);
        }

        private static SpecResult ValidarAlteracoesSugestao(SugestaoPedidoModel sugestao)
        {
            return new AlteracaoSugestaoDeveSerPermitidaSpec().IsSatisfiedBy(sugestao);
        }

        /// <summary>
        /// Altera sugestões de pedido de um fornecedor.
        /// </summary>
        /// <param name="idFornecedorParametro">O id de parâmetro de fornecedor.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="sugestoes">As sugestões de pedido do fornecedor.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>O resultado de cada operação de alteração individual.</returns>
        private IEnumerable<AlterarSugestaoResponse> AlterarSugestoesFornecedor(long idFornecedorParametro, int idLoja, IEnumerable<SugestaoPedidoModel> sugestoes, DateTime dtPedido)
        {
            List<AlterarSugestaoResponse> resultados = new List<AlterarSugestaoResponse>();

            resultados.AddRange(sugestoes.Select(sugestao => AlterarSugestao(sugestao)));

            if (resultados.Any(x => x.Sucesso))
            {
                ConsolidarPedidoMinimo(idLoja, idFornecedorParametro, dtPedido, sugestoes);
                ConsolidarPedidoMinimoXDoc(dtPedido, idFornecedorParametro, sugestoes);
            }

            return resultados;
        }

        /// <summary>
        /// Altera uma sugestão de pedido.
        /// </summary>
        /// <param name="sugestao">A sugestão.</param>
        /// <returns>
        /// O resultado da operação.
        /// </returns>
        private AlterarSugestaoResponse AlterarSugestao(SugestaoPedidoModel sugestao)
        {
            if (!m_gradeSugestaoService.ExisteGradeSugestaoAberta(sugestao.ItemDetalhePedido.CdSistema, sugestao.Loja.IDBandeira.Value, sugestao.ItemDetalhePedido.IDDepartamento, sugestao.IdLoja, DateTime.Now.ToMilitaryTime()))
            {
                // psqSugestaoPedido.aspx.cs linha 253
                return new AlterarSugestaoResponse(sugestao.IDSugestaoPedido) { NaoSalvaGradeSugestao = true };
            }

            if (sugestao.cdOrigemCalculo != TipoOrigemCalculo.Manual && !ValidarAlteracoesSugestao(sugestao))
            {
                return new AlterarSugestaoResponse(sugestao.IDSugestaoPedido) { NaoSalvaPercentualAlteracao = true };
            }

            return EfetuarAlteracaoSugestao(sugestao);
        }

        private AlterarSugestaoResponse EfetuarAlteracaoSugestao(SugestaoPedidoModel sugestao)
        {
            if (null != sugestao.cdOrigemCalculo && sugestao.cdOrigemCalculo != TipoOrigemCalculo.Sgp)
            {
                sugestao.vlEstoque = sugestao.Original_vlEstoque;
            }

            this.AlterarSugestao(sugestao.IDSugestaoPedido, sugestao.qtdPackCompra, sugestao.vlEstoque);

            return new AlterarSugestaoResponse(sugestao.IDSugestaoPedido) { Sucesso = true };
        }

        private void AlterarSugestao(int idSugestaoPedido, int qtdPackCompra, decimal vlEstoque)
        {
            Assert(new { idSugestaoPedido }, new AllMustBeInformedSpec());

            var entidade = new SugestaoPedido
            {
                IDSugestaoPedido = idSugestaoPedido,
                qtdPackCompra = qtdPackCompra,
                vlEstoque = vlEstoque
            };

            MainGateway.Update("qtdPackCompra=@qtdPackCompra,vlEstoque=@vlEstoque", "IDSugestaoPedido=@idSugestaoPedido", entidade);
            m_auditService.LogUpdate(entidade, s_auditProperties);
        }

        private SugestaoPedidoModel PrepararSugestaoParaAlteracao(SugestaoPedidoModel valoresPostados)
        {
            var sugestaoOriginal = this.MainGateway.ObterEstruturado(valoresPostados.IDSugestaoPedido);

            if (null == sugestaoOriginal)
            {
                return null;
            }

            var sugestao = new SugestaoPedidoModel(sugestaoOriginal);

            sugestao.Original_vlEstoque = sugestao.vlEstoque;
            sugestao.Original_qtdPackCompra = sugestao.qtdPackCompra;

            sugestao.vlEstoque = valoresPostados.vlEstoque;
            sugestao.qtdPackCompra = valoresPostados.qtdPackCompra;

            sugestao.qtdPackCompraAlterado = valoresPostados.qtdPackCompraAlterado;

            return sugestao;
        }

        private void AplicarInformacoesAlcada(IEnumerable<SugestaoPedidoModel> result)
        {
            if (null == result || result.Count() == 0)
            {
                return;
            }

            var alcada = this.m_alcadaService.ObterPorPerfil(RuntimeContext.Current.User.RoleId);

            if (null != alcada)
            {
                var alcadaDetalhe = this.m_alcadaDetalheService.ObterPorIdAlcada(alcada.IDAlcada, null);

                if (alcadaDetalhe != null && alcadaDetalhe.Count() > 0)
                {
                    alcada.Detalhe = alcadaDetalhe.ToList();
                }
               
                foreach (var item in result)
                {
                    AplicarCalculosPorItem(item, alcada);
                    AplicarInformacoesAlcadaPorItem(item, alcada);

                }
            }
        }

        private void AplicarInformacoesAlcadaPorItem(SugestaoPedidoModel item, Alcada alcada)
        {
            item.blZerarItem = alcada.blZerarItem;
            item.blAlterarPercentual = alcada.blAlterarPercentual;
            item.blAlterarInformacaoEstoque = alcada.blAlterarInformacaoEstoque;

            item.PossuiAlcada = true;
        }

        private void AplicarCalculosPorItem(SugestaoPedidoModel item, Alcada alcada)
        {
            decimal percentualAlteradoASerConsiderado = alcada.vlPercentualAlterado ?? 0;

            if (alcada.Detalhe != null && alcada.Detalhe.Count > 0)
            {
                foreach (var alcadaItem in alcada.Detalhe)
                {
                    if (alcadaItem.IDBandeira == item.Loja.IDBandeira
                            && alcadaItem.IDRegiaoAdministrativa == item.Loja.IdRegiaoAdministrativa
                            && alcadaItem.IDDepartamento == item.ItemDetalheSugestao.IDDepartamento)
                    {
                        percentualAlteradoASerConsiderado = alcadaItem.vlPercentualAlterado;
                        break;
                    }

                }
            }

            var valorOriginal = item.CalcularValorOriginal();

            decimal valorPermitidoAlterar = (valorOriginal * percentualAlteradoASerConsiderado) / 100;
            decimal limiteValorSuperior = valorOriginal + valorPermitidoAlterar;
            decimal limiteValorInferior = Math.Max(0, valorOriginal - valorPermitidoAlterar);

            item.vlLimiteSuperior = limiteValorSuperior;
            item.vlLimiteInferior = limiteValorInferior;
        }

        private void AplicarInformacaoGrade(IEnumerable<SugestaoPedidoModel> result)
        {
            int vlHoraLimite = DateTime.Now.ToMilitaryTime();

            var grades = result.Select(x => new
            {
                cdSistema = x.ItemDetalhePedido.CdSistema,
                IDBandeira = x.Loja.IDBandeira.Value,
                IDDepartamento = x.ItemDetalhePedido.IDDepartamento,
                IdLoja = x.IdLoja
            }).Distinct().Select(k => new
            {
                Key = k,
                GradeAberta = m_gradeSugestaoService.ExisteGradeSugestaoAberta(k.cdSistema, k.IDBandeira, k.IDDepartamento, k.IdLoja, vlHoraLimite)
            }).ToList();

            result.AsParallel().ForAll(r =>
                r.GradeSugestoesAberta = grades.Single(x =>
                    x.Key.cdSistema == r.ItemDetalhePedido.CdSistema &&
                    x.Key.IDBandeira == r.Loja.IDBandeira &&
                    x.Key.IDDepartamento == r.ItemDetalhePedido.IDDepartamento &&
                    x.Key.IdLoja == r.IdLoja).GradeAberta);
        }

        private void ConsolidarPedidoMinimo(int idLoja, long idFornecedorParametro, DateTime dtPedido, IEnumerable<SugestaoPedidoModel> sugestoes)
        {
            var possuiSugestoesDiferenteXDoc = sugestoes.Any(s => s.vlTipoReabastecimento != null && !ValorTipoReabastecimento.TodosXDock.Contains(s.vlTipoReabastecimento));

            if (possuiSugestoesDiferenteXDoc)
            {
                this.MainGateway.ConsolidarPedidoMinimo(idLoja, idFornecedorParametro, dtPedido, RuntimeContext.Current.User.IsAdministrator);
            }
        }

        private void ConsolidarPedidoMinimoXDoc(DateTime dtPedido, long idFornecedorParametro, IEnumerable<SugestaoPedidoModel> sugestoes)
        {
            var sugestoesXDocPorCD = sugestoes
                .Where(s =>
                    s.vlTipoReabastecimento != null &&
                    s.idCD.HasValue &&
                    ValorTipoReabastecimento.TodosXDock.Contains(s.vlTipoReabastecimento)).GroupBy(s => s.idCD.Value);

            foreach (var agrupadoPorCD in sugestoesXDocPorCD)
            {
                this.MainGateway.ConsolidarPedidoMinimoXDoc(dtPedido, agrupadoPorCD.Key, idFornecedorParametro);
            }
        }
        #endregion
    }
}
