using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.EstruturaMercadologica.Specs;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Serviço de domínio relacionado a inventário.
    /// </summary>
    public class InventarioService : EntityDomainServiceBase<Inventario, IInventarioGateway>, IInventarioService
    {
        #region Fields
        private readonly IInventarioAgendamentoGateway m_agendamentoGateway;
        private readonly IInventarioCriticaGateway m_criticaGateway;
        private readonly ICategoriaService m_categoriaService;
        private readonly IFechamentoFiscalGateway m_fechamentoFiscalGateway;
        private readonly IInventarioItemGateway m_inventarioItemGateway;
        private readonly IItemDetalheGateway m_itemDetalheGateway;
        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para inventário.</param>
        /// <param name="agendamentoGateway">O table data gateway para agendamento de inventário.</param>
        /// <param name="criticaGateway">O table data gateway para críticas de inventário.</param>
        /// <param name="categoriaService">O serviço de categoria.</param>
        /// <param name="fechamentoFiscalGateway">O table data gateway para fechamento fiscal.</param>
        /// <param name="inventarioItemGateway">O table data gateway para inventario item.</param>
        /// <param name="itemDetalheGateway">O table data gateway para item detalhe.</param>
        public InventarioService(IInventarioGateway mainGateway, IInventarioAgendamentoGateway agendamentoGateway, IInventarioCriticaGateway criticaGateway, ICategoriaService categoriaService, IFechamentoFiscalGateway fechamentoFiscalGateway, IInventarioItemGateway inventarioItemGateway, IItemDetalheGateway itemDetalheGateway)
            : base(mainGateway)
        {
            m_agendamentoGateway = agendamentoGateway;
            m_criticaGateway = criticaGateway;
            m_categoriaService = categoriaService;
            m_fechamentoFiscalGateway = fechamentoFiscalGateway;
            m_inventarioItemGateway = inventarioItemGateway;
            m_itemDetalheGateway = itemDetalheGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Localiza a data do inventário da loja informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A data do inventário, se existir.</returns>
        public DateTime? ObterDataInventarioDaLoja(int idLoja)
        {
            return MainGateway.ObterDataInventarioDaLoja(idLoja);
        }

        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        public int ObterQuantidadeLojasSemAgendamento(int idUsuario)
        {
            return m_agendamentoGateway.ObterQuantidadeLojasSemAgendamento(idUsuario);
        }

        /// <summary>
        /// Obtém os agendamentos de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os agendamentos.</returns>
        public IEnumerable<InventarioAgendamento> ObterAgendamentos(InventarioAgendamentoFiltro filtro)
        {
            Assert(new { filtro.IDBandeira }, new AllMustBeInformedSpec());
            Assert(new { filtro.CdLoja, filtro.CdDepartamento, filtro.DtAgendamento }, new AtLeastOneMustBeInformedSpec());

            return m_agendamentoGateway.ObterAgendamentos(filtro);
        }

        /// <summary>
        /// Obtém os inventários não agendados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os inventários não agendados.</returns>
        public IEnumerable<InventarioNaoAgendado> ObterNaoAgendados(InventarioAgendamentoFiltro filtro)
        {
            Assert(new { filtro.IDBandeira }, new AllMustBeInformedSpec());
            Assert(new { filtro.CdLoja, filtro.CdDepartamento, filtro.DtAgendamento }, new AtLeastOneMustBeInformedSpec());

            return m_agendamentoGateway.ObterNaoAgendados(filtro);
        }

        /// <summary>
        /// Obtém a lista de inventários agendados e abertos.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="dataInventario">A data do inventário.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="idCategoria">O id da categoria.</param>
        /// <returns>Os Inventario que estão abertos.</returns>
        public IEnumerable<Inventario> ObterInventariosAbertosParaImportacao(int idLoja, DateTime? dataInventario, int? idDepartamento, int? idCategoria)
        {
            return this.MainGateway.ObterInventariosAbertosParaImportacao(idLoja, dataInventario, idDepartamento, idCategoria);
        }

        /// <summary>
        /// Remove os agendamentos de inventários informados.
        /// </summary>
        /// <param name="ids">Os ids dos agendamentos de inventário a serem removidos.</param>
        public void RemoverAgendamentos(int[] ids)
        {
            var agendamentos = m_agendamentoGateway.ObterEstruturadosPorIds(ids);
            Assert(agendamentos, new InventarioAgendamentoPodeSerRemovidoSpec(m_agendamentoGateway));

            foreach (var agendamento in agendamentos)
            {
                m_agendamentoGateway.Update(
                    "stAgendamento = @status",
                    "IDInventarioAgendamento = @id",
                    new
                    {
                        status = InventarioAgendamentoStatus.Cancelado,
                        id = agendamento.Id
                    });

                RealizarCancelamentoInventario(agendamento.Inventario);
            }
        }

        /// <summary>
        /// Cancelars the specified identifier inventario.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        public void Cancelar(int idInventario)
        {
            var inventario = MainGateway.FindById(idInventario);
            Assert(inventario, new InventarioPodeSerCanceladoSpec());
            RealizarCancelamentoInventario(inventario);
        }

        /// <summary>
        /// Obtém o agendamento estruturado por id.
        /// </summary>
        /// <param name="id">O id do agendamento.</param>
        /// <returns>O agendamento.</returns>
        public InventarioAgendamento ObterAgendamentoEstruturadoPorId(int id)
        {
            // TODO: utililizar método específico no gateway.
            return m_agendamentoGateway.ObterEstruturadosPorIds(new int[] { id }).FirstOrDefault();
        }

        /// <summary>
        /// Obtém os inventários que correspondem ao filtro especificado.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os inventários.</returns>
        public IEnumerable<InventarioSumario> ObterSumarizadoPorFiltro(InventarioFiltro filtro, Paging paging)
        {
            return MainGateway.ObterSumarizadoPorFiltro(filtro, paging);
        }

        /// <summary>
        /// Obtém o custo total por filtro.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>O valor do custo total.</returns>
        public decimal ObterCustoTotalPorFiltro(InventarioFiltro filtro)
        {
            return MainGateway.ObterCustoTotalPorFiltro(filtro);
        }

        /// <summary>
        /// Insere agendamentos para lojas x departamentos.
        /// </summary>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <param name="lojas">As lojas que devem ser gerados os agendamentos.</param>
        /// <param name="departamentos">Os departamentos que devem ser gerados os agendamentos.</param>
        /// <returns>A quantidade de agendamentos inseridos.</returns>
        public AgendamentoResponse InserirAgendamentos(DateTime dtAgendamento, IEnumerable<Loja> lojas, IEnumerable<Departamento> departamentos)
        {
            var agendamentos = InventarioAgendamento.Create(dtAgendamento, lojas, departamentos);

            var especificacao = new InventarioAgendamentoPodeSerSalvoSpec(MainGateway, m_agendamentoGateway);

            var agendamentosVerificados = agendamentos.Select(agendamento => new Tuple<InventarioAgendamento, SpecResult>(agendamento, especificacao.IsSatisfiedBy(agendamento))).ToArray();

            foreach (var agendamento in agendamentosVerificados.Where(x => x.Item2.Satisfied).Select(x => x.Item1))
            {
                var idUsuario = RuntimeContext.Current.User.Id;
                var now = DateTime.Now;

                // Insere inventário.                    
                var inventario = agendamento.Inventario;

                inventario.MarcarComoAberto(idUsuario, now, m_categoriaService);
                MainGateway.Insert(inventario);

                // Insere agendamento.
                agendamento.MarcarComoAgendado(inventario, idUsuario, now);
                m_agendamentoGateway.Insert(agendamento);
            }

            var result = agendamentosVerificados.Summarize();

            return result;
        }

        /// <summary>
        /// Atualiza a data de agendamento dos agendamentos de inventário informados.
        /// </summary>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <param name="agendamentoInventarioIDs">Os ids dos agendamentos.</param>
        /// <returns>A quantidade de agendamentos atualizados.</returns>
        public AgendamentoResponse AtualizarAgendamentos(DateTime dtAgendamento, params int[] agendamentoInventarioIDs)
        {
            var agendamentos = m_agendamentoGateway.ObterEstruturadosPorIds(agendamentoInventarioIDs).ToList();
            var spec = new InventarioAgendamentoPodeSerSalvoSpec(MainGateway, m_agendamentoGateway);

            agendamentos.ForEach(agendamento => agendamento.dtAgendamento = dtAgendamento);

            var agendamentosVerificados = agendamentos.Select(agendamento => new Tuple<InventarioAgendamento, SpecResult>(agendamento, spec.IsSatisfiedBy(agendamento))).ToArray();
            var agendamentosValidos = agendamentosVerificados.Where(agendamento => agendamento.Item2.Satisfied).Select(kvp => kvp.Item1);

            if (agendamentosValidos.Count() > 0)
            {
                InternalAtualizarAgendamentos(agendamentosValidos);
            }

            return agendamentosVerificados.Summarize();
        }

        /// <summary>
        /// Obtém o inventário estruturado pelo id especificado.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        /// <returns>
        /// O inventário estruturado.
        /// </returns>
        public InventarioSumario ObterEstruturadoPorId(int id)
        {
            return MainGateway.ObterEstruturadoPorId(id);
        }

        /// <summary>
        /// Obtém os itens do inventário por filtro.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de inventário que satisfazem o filtro.</returns>
        public IEnumerable<InventarioItemSumario> ObterItensEstruturadoPorFiltro(InventarioItemFiltro filtro, Paging paging)
        {
            return m_inventarioItemGateway.ObterEstruturadoPorFiltro(filtro, paging);
        }

        /// <summary>
        /// Obtém um item de inventário estruturado pelo id.
        /// </summary>
        /// <param name="idInventarioItem">O id do item de inventário.</param>
        /// <returns>O item do inventário.</returns>
        public InventarioItem ObterItemEstruturadoPorId(int idInventarioItem)
        {
            return m_inventarioItemGateway.ObterEstruturadoPorId(idInventarioItem);
        }

        /// <summary>
        /// Salva um item de inventário.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="inventario">O inventario que o item pertence.</param>
        public void SalvarItem(InventarioItem item, Inventario inventario)
        {
            item.IDInventario = inventario.IDInventario;

            Assert(
                new
                {
                    Inventory = item.IDInventario,
                    Item = item.IDItemDetalhe,
                    Quantity = item.qtItem
                },
                new AllMustBeInformedSpec());

            var alteradoGa = inventario.stInventario == InventarioStatus.Aprovado &&
                             RuntimeContext.Current.User.IsGa;

            Assert(item, new ItemInventarioDeveSerUnicoSpec(m_inventarioItemGateway));
            Assert(
                    RuntimeContext.Current.User,
                    new UsuarioPodeEditarItemInventarioSpec(inventario));

            if (item.IsNew)
            {
                InserirItem(item, inventario, alteradoGa);
            }
            else
            {
                m_inventarioItemGateway.Atualizar(item, inventario, alteradoGa);
            }
        }

        /// <summary>
        /// Exclui o item de inventário.
        /// </summary>
        /// <param name="idInventarioItem">O id do inventario item.</param>
        public void RemoverItem(int idInventarioItem)
        {
            var item = m_inventarioItemGateway.FindById(idInventarioItem);
            m_inventarioItemGateway.Remover(item);
        }

        /// <summary>
        /// Finaliza o inventário.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        public void Finalizar(int id)
        {
            var inventario = MainGateway.FindById(id);
            if (inventario == null)
            {
                throw new UserInvalidOperationException(Texts.NonExistingInventory);
            }

            Assert(RuntimeContext.Current.User, new UsuarioPodeFinalizarInventarioSpec(inventario));
            MainGateway.Finalizar(inventario);
        }

        /// <summary>
        /// Obtém as irregularidades do inventário.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// Uma coleção de irregularidades que o inventário possui.
        /// </returns>
        public IEnumerable<string> ObterIrregularidadesFinalizacao(int idInventario)
        {
            var inventario = new Inventario { IDInventario = idInventario };

            return new ISpec<Inventario>[]
            {
                new InventarioNaoPossuiItensInativosDeletadosSpec(MainGateway),
                new SortimentoDeveSerValidoSpec(MainGateway),
                new InventarioNaoPossuiCustoCadastroSpec(MainGateway),
            }.Select(t => t.IsSatisfiedBy(inventario))
            .Where(t => !t.Satisfied).Select(t => t.Reason).ToArray();
        }

        /// <summary>
        /// Obtém as operações permitidas sobre o inventário.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        /// <returns>As operações permitidas.</returns>
        public InventarioOperacoesPermitidas ObterOperacoesPermitidas(Inventario inventario)
        {
            var operacoes = new InventarioOperacoesPermitidas();
            operacoes.VoltarStatus = new UsuarioPodeVoltarStatusInventarioSpec(inventario, MainGateway, m_fechamentoFiscalGateway).IsSatisfiedBy(RuntimeContext.Current.User);
            operacoes.Aprovar = new UsuarioPodeAprovarInventarioSpec(inventario).IsSatisfiedBy(RuntimeContext.Current.User);
            operacoes.Finalizar = new UsuarioPodeFinalizarInventarioSpec(inventario).IsSatisfiedBy(RuntimeContext.Current.User);
            operacoes.Cancelar = new InventarioPodeSerCanceladoSpec().IsSatisfiedBy(inventario);
            var exportacaoComparacaoSpecResult = new UsuarioPodeExportarComparacaoEstoqueSpec(inventario).IsSatisfiedBy(RuntimeContext.Current.User);
            operacoes.ExportarComparacaoEstoque = exportacaoComparacaoSpecResult.Satisfied;
            operacoes.MensagemComparacaoEstoque = exportacaoComparacaoSpecResult.Reason;
            operacoes.ExcluirItem = true;
            operacoes.SalvarItem = true;
            operacoes.AlterarItem = new UsuarioPodeEditarItemInventarioSpec(inventario).IsSatisfiedBy(RuntimeContext.Current.User);
            return operacoes;
        }

        /// <summary>
        /// Volta o status do inventário.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        public void VoltarStatus(int idInventario)
        {
            var inventario = MainGateway.FindById(idInventario);
            if (inventario == null)
            {
                throw new UserInvalidOperationException(Texts.NonExistingInventory);
            }

            Assert(
                RuntimeContext.Current.User,
                new UsuarioPodeVoltarStatusInventarioSpec(inventario, MainGateway, m_fechamentoFiscalGateway));

            var novoStatus = inventario.stInventario == InventarioStatus.Aprovado
                ? InventarioStatus.Importado
                : InventarioStatus.Aprovado;

            MainGateway.ReverterParaStatus(inventario, novoStatus);
        }

        /// <summary>
        /// Obtém as irregularidades para aprovação do inventário.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// Uma coleção de irregularidades que o inventário possui.
        /// </returns>
        public IEnumerable<string> ObterIrregularidadesAprovacao(int idInventario)
        {
            Assert(new Inventario { IDInventario = idInventario }, new InventarioNaoPossuiItensVinculadosDeEntradaSpec(MainGateway));

            return ObterIrregularidadesFinalizacao(idInventario);
        }

        /// <summary>
        /// Aprova o inventário informado.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        public void Aprovar(int idInventario)
        {
            var inventario = MainGateway.FindById(idInventario);
            if (inventario == null)
            {
                throw new UserInvalidOperationException(Texts.NonExistingInventory);
            }

            Assert(RuntimeContext.Current.User, new UsuarioPodeAprovarInventarioSpec(inventario));
            Assert(new Inventario { IDInventario = idInventario }, new InventarioNaoPossuiItensVinculadosDeEntradaSpec(MainGateway));
            MainGateway.AjustarEstoqueInventariado(idInventario, true);
        }

        /// <summary>
        /// Pesquisa as críticas de inventário pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro para críticas de inventário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As críticas de inventário.</returns>
        public IEnumerable<InventarioCritica> PesquisarCriticas(InventarioCriticaFiltro filtro, Paging paging)
        {
            return m_criticaGateway.Pesquisar(filtro, paging);
        }

        private void InternalAtualizarAgendamentos(IEnumerable<InventarioAgendamento> agendamentosValidos)
        {
            m_agendamentoGateway.Update(
                "dtAgendamento = @dtAgendamento, dhAlteracao = @dhAlteracao",
                "IDInventarioAgendamento IN @agendamentoInventarioIDs",
                new
                {
                    dtAgendamento = agendamentosValidos.First().dtAgendamento,
                    dhAlteracao = DateTime.Now,
                    agendamentoInventarioIDs = agendamentosValidos.Select(agendamento => agendamento.Id).ToArray()
                });

            MainGateway.Update(
                "dhInventario = @dtAgendamento",
                "IDInventario IN @inventarioIDs",
                new
                {
                    dtAgendamento = agendamentosValidos.First().dtAgendamento,
                    inventarioIDs = agendamentosValidos.Select(a => a.IDInventario).ToArray()
                });
        }

        /// <summary>
        /// Realiza o cancelamento do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        private void RealizarCancelamentoInventario(Inventario inventario)
        {
            MainGateway.CancelarInventario(inventario);
            m_criticaGateway.InativarCriticas(inventario);
        }

        private void InserirItem(InventarioItem item, Inventario inventario, bool alteradoGa)
        {
            var itemDetalhe = m_itemDetalheGateway.FindById(item.IDItemDetalhe.Value);
            Assert(itemDetalhe, new ItemNaoDeveSerVinculadoDeEntradaSpec());
            var inventarioDepartamento = MainGateway.Find(
                "IDDepartamento",
                "IDInventario = @IDInventario",
                new { inventario.IDInventario })
                .FirstOrDefault();

            Assert(
                itemDetalhe,
                new ItemDevePossuirMesmoDepartamentoDoInventarioSpec(inventarioDepartamento.IDDepartamento.GetValueOrDefault()));
            m_inventarioItemGateway.Inserir(item, alteradoGa);
        }
        #endregion
    }
}
