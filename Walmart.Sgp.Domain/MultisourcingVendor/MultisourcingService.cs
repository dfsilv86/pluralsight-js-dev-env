using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Serviço de cadastro de Multisourcing.
    /// </summary>
    public class MultisourcingService : EntityDomainServiceBase<Multisourcing, IMultisourcingGateway>, IMultisourcingService
    {
        private readonly ILogMultiSourcingService m_logMultiSourcingService;
        private readonly IItemDetalheService m_itemDetalheService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingService"/>.
        /// </summary>
        /// <param name="mainGateway">O gateway.</param>
        /// <param name="logMultiSourcingService">O service de LOG.</param>
        /// <param name="itemDetalheService">O service de ItemDetalhe.</param>
        public MultisourcingService(IMultisourcingGateway mainGateway, ILogMultiSourcingService logMultiSourcingService, IItemDetalheService itemDetalheService)
            : base(mainGateway)
        {
            m_logMultiSourcingService = logMultiSourcingService;
            m_itemDetalheService = itemDetalheService;
        }

        /// <summary>
        /// Obtém um Multisourcing pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O Multisourcing.</returns>
        public Multisourcing Obter(long id)
        {
            return this.MainGateway.Obter(id);
        }

        /// <summary>
        /// Insere ou Atualiza um Multisourcing com base nos itens selecionados.
        /// </summary>
        /// <param name="itens">Os itens de entrada.</param>
        /// <param name="cdUsuario">O ID do usuário logado.</param>
        /// <param name="cdSistema">O Codigo do Sistema.</param>
        public void SalvarMultisourcing(IEnumerable<ItemDetalheCD> itens, int cdUsuario, int cdSistema)
        {
            itens = itens.Where(item => item.idCompraCasada == null);

            ValidarMultisourcing(itens, cdUsuario, cdSistema);

            var dt = DateTime.Now;

            foreach (var i in itens)
            {
                if (i.vlPercentual == null)
                {
                    i.vlPercentual = 0;
                }

                var oldMultisourcing = this.MainGateway.ObterPorRelacionamentoItemSecundarioECD(i.IDRelacionamentoItemSecundario, i.IDCD);

                if (oldMultisourcing == null)
                {
                    InserirMultisourcing(cdUsuario, dt, i);
                }
                else
                {
                    AtualizarMultisourcing(cdUsuario, dt, i, oldMultisourcing);
                }
            }
        }

        /// <summary>
        /// Excluir um cadastro de multisourcing.
        /// </summary>
        /// <param name="cdItemSaida">O código do item de saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        public void Excluir(long cdItemSaida, long cdCD)
        {
            Assert(new { OutputItemCode = cdItemSaida, cd = cdCD }, new AllMustBeInformedSpec());

            var itens = this.MainGateway.ObterPorCdItemSaidaEcdCD(cdItemSaida, cdCD);
            foreach (var item in itens)
            {
                this.m_logMultiSourcingService.Logar(TpOperacao.Exclusao, item, null, "Exclusão de cadastro de multisourcing Id=" + item.IDMultisourcing);
                this.MainGateway.Delete(item.IDMultisourcing);
            }
        }

        /// <summary>
        /// Excluir um cadastro de multisourcing.
        /// </summary>
        /// <param name="itens"> Lista de itens que compoem o cad.</param>
        public void Excluir(IEnumerable<ItemDetalheCD> itens)
        {
            Assert(new { Itens = itens }, new AllMustBeInformedSpec());

            foreach (var item in itens)
            {
                var ms = this.MainGateway.ObterPorRelacionamentoItemSecundarioECD(item.IDRelacionamentoItemSecundario, item.IDCD);
                if (ms != null)
                {
                    this.m_logMultiSourcingService.Logar(TpOperacao.Exclusao, ms, null, "Exclusão de cadastro de multisourcing Id=" + ms.IDMultisourcing);
                    this.MainGateway.Delete(ms.IDMultisourcing);
                }
            }
        }

        /// <summary>
        /// Salva os dados de multisourcing.
        /// </summary>
        /// <param name="multisourcings">Os multisourcings.</param>
        /// <param name="cdUsuario">O código de usuario.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        public void SalvarMultisourcings(IEnumerable<Multisourcing> multisourcings, int cdUsuario, int cdSistema)
        {
            ExcluirMultisourcings(multisourcings, cdUsuario, cdSistema);

            foreach (var multisourcing in multisourcings)
            {
                var oldMultisourcing = this.MainGateway.ObterPorRelacionamentoItemSecundarioECD(multisourcing.IDRelacionamentoItemSecundario, multisourcing.IDCD);

                if (oldMultisourcing == null)
                {
                    InserirMultisourcings(cdUsuario, multisourcing);
                }
                else
                {
                    AtualizarMultisourcings(cdUsuario, multisourcing, oldMultisourcing);
                }
            }
        }

        private void ValidarMultisourcing(IEnumerable<ItemDetalheCD> itens, int cdUsuario, int cdSistema)
        {
            Assert(new { Itens = itens, IDUsuario = cdUsuario }, new AllMustBeInformedSpec());
            Assert(itens, new ListaDeItensNaoPodeTerItemCemPorcentoSpec());
            Assert(itens.Select(q => q.vlPercentual), new PercentuaisDevemSerMultiploDeCincoSpec());
            Assert(itens, new ListaDeItensDevePossuirPercentualCompletoSpec());
            Assert(itens, new ListaDeItensNaoPodeRepetirVendorSpec(m_itemDetalheService.ObterPorItemESistema, cdSistema));
        }

        private void InserirMultisourcings(int cdUsuario, Multisourcing multisourcing)
        {
            multisourcing.cdUsuarioInclusao = multisourcing.cdUsuarioAlteracao = cdUsuario;
            multisourcing.dtInclusao = multisourcing.dtAlteracao = DateTime.Now;

            this.MainGateway.Insert(multisourcing);
            m_logMultiSourcingService.Logar(TpOperacao.Inclusao, null, multisourcing, "Inclusão de cadastro de multisourcing Id=" + multisourcing.IDMultisourcing);
        }

        private void AtualizarMultisourcings(int cdUsuario, Multisourcing multisourcing, Multisourcing oldMultisourcing)
        {
            multisourcing.IDMultisourcing = oldMultisourcing.IDMultisourcing;
            multisourcing.cdUsuarioInclusao = oldMultisourcing.cdUsuarioInclusao;
            multisourcing.dtInclusao = oldMultisourcing.dtInclusao;
            multisourcing.cdUsuarioAlteracao = cdUsuario;
            multisourcing.dtAlteracao = DateTime.Now;

            this.MainGateway.Update(multisourcing);
            m_logMultiSourcingService.Logar(TpOperacao.Alteracao, oldMultisourcing, multisourcing, "Alteração de cadastro de multisourcing Id=" + multisourcing.IDMultisourcing);
        }

        private void ExcluirMultisourcings(IEnumerable<Multisourcing> multisourcings, int cdUsuario, int cdSistema)
        {
            var grouped = multisourcings.GroupBy(m => new { m.CdItemDetalheSaida, m.CD });

            foreach (var group in grouped)
            {
                var itensToExclude = IdentificarMultisourcingsParaExclusao(group.ToList(), group.Key.CdItemDetalheSaida, group.Key.CD, cdSistema);

                foreach (var itemToExclude in itensToExclude)
                {
                    var oldMultisourcing = this.MainGateway.ObterPorRelacionamentoItemSecundarioECD(itemToExclude.IDRelacionamentoItemSecundario, itemToExclude.IDCD);

                    if (oldMultisourcing != null)
                    {
                        oldMultisourcing.cdUsuarioAlteracao = cdUsuario;

                        m_logMultiSourcingService.Logar(TpOperacao.Exclusao, oldMultisourcing, null, "Exclusão de cadastro de multisourcing Id=" + oldMultisourcing.IDMultisourcing);
                        this.MainGateway.Delete(oldMultisourcing.IDMultisourcing);
                    }
                }
            }
        }

        private IEnumerable<Multisourcing> IdentificarMultisourcingsParaExclusao(IEnumerable<Multisourcing> multisourcings, long cdItemDetalheSaida, int cdCD, int cdSistema)
        {
            var multisourcingsBase = m_itemDetalheService
                .PesquisarItensEntradaPorSaidaCD(cdItemDetalheSaida, cdCD, cdSistema)
                .Select(m => new Multisourcing
                {
                    IDRelacionamentoItemSecundario = m.IDRelacionamentoItemSecundario,
                    IDCD = m.IDCD
                });

            return multisourcingsBase.Except(multisourcings, new MultisourcingEqualityComparer());
        }

        private void AtualizarMultisourcing(int cdUsuario, DateTime dt, ItemDetalheCD i, Multisourcing ms)
        {
            if (0 == i.vlPercentual)
            {
                // Se zerou um multisourcing que já existe, deleta ele
                this.m_logMultiSourcingService.Logar(TpOperacao.Exclusao, ms, null, null);
                this.MainGateway.Delete(ms.IDMultisourcing);
            }
            else
            {
                var msAnterior = new Multisourcing() { vlPercentual = ms.vlPercentual, cdUsuarioAlteracao = ms.cdUsuarioAlteracao, dtAlteracao = ms.dtAlteracao };

                ms.vlPercentual = i.vlPercentual.Value;
                ms.cdUsuarioAlteracao = cdUsuario;
                ms.dtAlteracao = dt;

                this.MainGateway.Update(ms);
                this.m_logMultiSourcingService.Logar(TpOperacao.Exclusao, msAnterior, ms, null);
            }
        }

        private void InserirMultisourcing(int cdUsuario, DateTime dt, ItemDetalheCD i)
        {
            if (i.vlPercentual == 0)
            {
                return;
            }

            var ms = new Multisourcing();
            ms.IDCD = i.IDCD;
            ms.vlPercentual = i.vlPercentual.Value;
            ms.cdUsuarioInclusao = cdUsuario;
            ms.cdUsuarioAlteracao = cdUsuario;
            ms.dtAlteracao = dt;
            ms.dtInclusao = dt;
            ms.IDRelacionamentoItemSecundario = i.IDRelacionamentoItemSecundario;

            this.MainGateway.Insert(ms);
            this.m_logMultiSourcingService.Logar(TpOperacao.Inclusao, null, ms, null);
        }
    }
}