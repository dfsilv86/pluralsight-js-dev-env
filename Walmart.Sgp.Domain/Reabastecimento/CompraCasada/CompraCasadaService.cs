using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CompraCasada;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.CompraCasada
{
    /// <summary>
    /// Serviço de domínio relacionado a CompraCasada.
    /// </summary>
    public class CompraCasadaService : EntityDomainServiceBase<CompraCasada, ICompraCasadaGateway>, ICompraCasadaService
    {
        private readonly IItemDetalheGateway m_itemDetalheGateway;
        private readonly ISugestaoPedidoGateway m_sugestaoPedidoGateway;
        private readonly ISugestaoPedidoCDGateway m_sugestaoPedidoCDGateway;
        private readonly IRelacaoItemLojaCDService m_relacaoItemLojaCDService;
        private readonly IAuditService m_auditService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CompraCasadaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para CompraCasada.</param>
        /// <param name="itemDetalheGateway">O table data gateway para ItemDetalhe.</param>
        /// <param name="sugestaoPedidoGateway">O table data gateway para SugestaoPedido.</param>
        /// <param name="sugestaoPedidoCDGateway">O table data gateway para SugestaoPedidoCD.</param>
        /// <param name="relacaoItemLojaCDService">O Serviço para RelacaoItemLojaCD.</param>
        /// <param name="auditService">O serviço de log.</param>
        public CompraCasadaService(ICompraCasadaGateway mainGateway, IItemDetalheGateway itemDetalheGateway, ISugestaoPedidoGateway sugestaoPedidoGateway, ISugestaoPedidoCDGateway sugestaoPedidoCDGateway, IRelacaoItemLojaCDService relacaoItemLojaCDService, IAuditService auditService)
            : base(mainGateway)
        {
            m_itemDetalheGateway = itemDetalheGateway;
            m_sugestaoPedidoGateway = sugestaoPedidoGateway;
            m_sugestaoPedidoCDGateway = sugestaoPedidoCDGateway;
            m_relacaoItemLojaCDService = relacaoItemLojaCDService;
            m_auditService = auditService;
        }

        /// <summary>
        /// Obtem o codigo do item pai pelo codigo de um item filho.
        /// </summary>
        /// <param name="cdItemFilho">O codigo do item filho.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Retorna zero se nao encontrar pai ou o codigo do item pai se encontrar.</returns>
        public int ObterCodItemPaiPorCodItemFilho(long cdItemFilho, long cdSistema)
        {
            Assert(new { Item = cdItemFilho, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterCodItemPaiPorCodItemFilho(cdItemFilho, cdSistema);
        }

        /// <summary>
        /// Verifica se um cadastro de compra casada possui pai.
        /// </summary>
        /// <param name="filtros">Os filtros e os itens em memoria.</param>
        /// <returns>Se tem ou nao pai.</returns>
        public ItemDetalhe VerificaPossuiPai(PesquisaCompraCasadaFiltro filtros)
        {
            Assert(new { Department = filtros.idDepartamento, System = filtros.cdSistema, VendorNineDigitsCode = filtros.idFornecedorParametro, OutputItemCode = filtros.idItemDetalheSaida }, new AllMustBeInformedSpec());

            var paiMemoria = filtros.Itens.FirstOrDefault(i => i.IDItemDetalhe != filtros.ItemPaiSelecionado.IDItemDetalhe && i.PaiCompraCasada.HasValue && i.PaiCompraCasada.Value);
            if (paiMemoria != null)
            {
                return paiMemoria;
            }

            var paiBanco = this.PesquisarItensEntrada(filtros, null).FirstOrDefault(i => i.PaiCompraCasada.HasValue && i.PaiCompraCasada.Value);
            if (paiBanco != null && paiBanco.IDItemDetalhe != filtros.ItemPaiSelecionado.IDItemDetalhe)
            {
                return paiBanco;
            }

            return null;
        }

        /// <summary>
        /// Verifica se a compra casada possui cadastro.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        /// <returns>Se existe ou nao cadastro.</returns>
        public bool VerificaPossuiCadastro(PesquisaCompraCasadaFiltro filtros)
        {
            Assert(new { Department = filtros.idDepartamento, System = filtros.cdSistema, VendorNineDigitsCode = filtros.idFornecedorParametro, OutputItemCode = filtros.idItemDetalheSaida }, new AllMustBeInformedSpec());

            return this.MainGateway.PossuiCadastro(filtros);
        }

        /// <summary>
        /// Excluir a compra casada.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        public void ExcluirItensCompraCasada(PesquisaCompraCasadaFiltro filtros)
        {
            Assert(new { Department = filtros.idDepartamento, System = filtros.cdSistema, VendorNineDigitsCode = filtros.idFornecedorParametro, OutputItemCode = filtros.idItemDetalheSaida }, new AllMustBeInformedSpec());

            var itens = this.MainGateway.PesquisarItensEntrada(filtros, null);

            Assert(itens, new ItensNaoDevemTerSugestaoPedidoPendenteSpec(filtros.idFornecedorParametro.Value, this.m_sugestaoPedidoGateway.VerificaItemSaidaGradeAberta, this.m_sugestaoPedidoCDGateway.VerificaItemSaidaGradeAbertaSugestaoCD));

            foreach (var item in itens)
            {
                if (!item.IDCompraCasada.HasValue)
                {
                    continue;
                }

                var cc = ConsolidarExclusaoLogica(item);
                m_auditService.LogDelete(cc, "blItemPai", "blAtivo");
            }
        }

        /// <summary>
        /// Validações utilizadas nos itens filhos no front-end.
        /// </summary>
        /// <param name="filtros">Filtros compra casada e itens alterados.</param>
        /// <returns>Uma lista de SpecResult se houver problemas, caso contrário NULL.</returns>
        public IEnumerable<SpecResult> ValidarItemFilhoMarcado(PesquisaCompraCasadaFiltro filtros)
        {
            var itens = MergeItens(filtros);

            var validacoes = new[] 
            {
                new TipoRAItensDeveSerIgualAoPaiSpec().IsSatisfiedBy(itens),
                new CustoUnitarioItensFilhoDevemSerIguaisSpec().IsSatisfiedBy(itens),
                new TraitItensFilhoDevemSerIguaisAoPaiSpec(m_itemDetalheGateway.ObterTraitsPorItem).IsSatisfiedBy(itens)
            }.Where(v => !v.Satisfied).ToList();

            return validacoes.Any() ? validacoes : null;
        }

        /// <summary>
        /// Pesquisar Itens de Entrada para Cadastro de Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        public IEnumerable<ItemDetalhe> PesquisarItensEntrada(PesquisaCompraCasadaFiltro filtro, Paging paging)
        {
            Assert(new { Department = filtro.idDepartamento, System = filtro.cdSistema, VendorNineDigitsCode = filtro.idFornecedorParametro, OutputItemCode = filtro.idItemDetalheSaida }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarItensEntrada(filtro, paging);
        }

        /// <summary>
        /// Pesquisar Itens para Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        public IEnumerable<ItemDetalhe> PesquisarItensCompraCasada(PesquisaCompraCasadaFiltro filtro, Paging paging)
        {
            Assert(new { Department = filtro.idDepartamento, System = filtro.cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarItensCompraCasada(filtro, paging);
        }

        /// <summary>
        /// Salvar compra casada.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada e os itens alterados.</param>
        /// <returns>Uma lista de SpecResult se houver problemas, caso contrário NULL.</returns>
        public IEnumerable<SpecResult> SalvarItensCompraCasada(PesquisaCompraCasadaFiltro filtros)
        {
            var itens = MergeItens(filtros);

            var msgsValidacao = AplicarValidacoesSalvar(itens, filtros.blPossuiCadastro, filtros.idFornecedorParametro.Value);
            if (msgsValidacao != null)
            {
                return msgsValidacao;
            }

            ConsolidarItensCompraCasada(filtros);

            return null;
        }

        /// <summary>
        /// Faz o merge dos itens que estão no frontend com os itens que estão na base.
        /// </summary>
        /// <param name="filtros">Filtro para pesquisa e os itens do frontend.</param>
        /// <returns>Itens da base + os itens do front.</returns>
        public IEnumerable<ItemDetalhe> MergeItens(PesquisaCompraCasadaFiltro filtros)
        {
            var retorno = new List<ItemDetalhe>();
            var itensNoBanco = this.PesquisarItensEntrada(filtros, null);
            retorno.AddRange(itensNoBanco.Where(i => !filtros.Itens.Select(s => s.IDItemDetalhe).Contains(i.IDItemDetalhe)));

            retorno.AddRange(filtros.Itens);

            var itensAnular = retorno.Where(item => (item.FilhoCompraCasada.HasValue && !item.FilhoCompraCasada.Value) && (item.PaiCompraCasada.HasValue && !item.PaiCompraCasada.Value)).ToList();
            itensAnular.ForEach(item => item.PaiCompraCasada = item.FilhoCompraCasada = null);

            return retorno;
        }

        /// <summary>
        /// Remove os vinculos de um item pai.
        /// </summary>
        /// <param name="item">O itemDetalhe pai.</param>
        public void RemoverVinculoItemPai(ItemDetalhe item)
        {
            var result = this.MainGateway.Find(
               "idItemDetalheSaida = @idItemDetalheSaida AND idFornecedorParametro = @idFornecedorParametro AND idItemDetalheEntrada = @idItemDetalheEntrada",
               new { idItemDetalheSaida = item.ItemSaida.IDItemDetalhe, idFornecedorParametro = item.FornecedorParametro.IDFornecedorParametro, idItemDetalheEntrada = item.IDItemDetalhe }).SingleOrDefault();

            if (result != null && result.blAtivo)
            {
                if ((!item.PaiCompraCasada.HasValue || !item.PaiCompraCasada.Value) && (result.blItemPai.HasValue && result.blItemPai.Value))
                {
                    m_relacaoItemLojaCDService.RemoverRelacionamentoPorItemEntrada(item.IDItemDetalhe, true);
                }
            }
        }

        private static IEnumerable<SpecResult> AplicarPreValidacoesSalvar(IEnumerable<ItemDetalhe> itens)
        {
            var preValidacoes = new[]
            {
                new QtdPaisDeveSerUmSpec().IsSatisfiedBy(itens),
                new QtdFilhosDeveSerNoMinimoDoisSpec().IsSatisfiedBy(itens),
            }.Where(v => !v.Satisfied);

            return preValidacoes.Any() ? preValidacoes : null;
        }

        private CompraCasada ConsolidarExclusaoLogica(ItemDetalhe item)
        {
            var cc = this.MainGateway.FindById(item.IDCompraCasada.Value);
            cc.blAtivo = false;
            this.MainGateway.Update(cc);

            m_relacaoItemLojaCDService.RemoverRelacionamentoPorItemEntrada(item.IDItemDetalhe, true);

            return cc;
        }

        private IEnumerable<SpecResult> AplicarValidacoesSalvar(IEnumerable<ItemDetalhe> itens, bool? blPossuiCadastro, int idFornecedorParamatro)
        {
            var msgsPreValidacao = AplicarPreValidacoesSalvar(itens);
            if (msgsPreValidacao != null)
            {
                return msgsPreValidacao;
            }

            if (blPossuiCadastro.HasValue && blPossuiCadastro.Value)
            {
                Assert(itens, new ItensNaoDevemTerSugestaoPedidoPendenteSpec(idFornecedorParamatro, this.m_sugestaoPedidoGateway.VerificaItemSaidaGradeAberta, this.m_sugestaoPedidoCDGateway.VerificaItemSaidaGradeAbertaSugestaoCD));
            }

            var validacoes = new List<SpecResult>() 
            {
                new TipoRAItensDeveSerIgualAoPaiSpec().IsSatisfiedBy(itens),
                new CustoUnitarioItensFilhoDevemSerIguaisSpec().IsSatisfiedBy(itens),
                new TraitItensFilhoDevemSerIguaisAoPaiSpec(m_itemDetalheGateway.ObterTraitsPorItem).IsSatisfiedBy(itens),
                new SomaVendorPackFilhosIgualPaiSpec().IsSatisfiedBy(itens),
            }.Where(v => !v.Satisfied).ToList();

            return validacoes.Any() ? validacoes : null;
        }

        private CompraCasada PopulaCompraCasada(ItemDetalhe item)
        {
            var result = this.MainGateway.Find(
                "idItemDetalheSaida = @idItemDetalheSaida AND idFornecedorParametro = @idFornecedorParametro AND idItemDetalheEntrada = @idItemDetalheEntrada",
                new { idItemDetalheSaida = item.ItemSaida.IDItemDetalhe, idFornecedorParametro = item.FornecedorParametro.IDFornecedorParametro, idItemDetalheEntrada = item.IDItemDetalhe });

            var compraCasada = new CompraCasada();
            if (result != null && result.SingleOrDefault() != null)
            {
                compraCasada = result.SingleOrDefault();
            }

            compraCasada.blAtivo = (item.PaiCompraCasada.HasValue && item.PaiCompraCasada.Value) || (item.FilhoCompraCasada.HasValue && item.FilhoCompraCasada.Value);
            compraCasada.blItemPai = item.PaiCompraCasada;
            compraCasada.IDFornecedorParametro = item.FornecedorParametro.IDFornecedorParametro;
            compraCasada.IDItemDetalheEntrada = item.IDItemDetalhe;
            compraCasada.IDItemDetalheSaida = item.ItemSaida.IDItemDetalhe;

            return compraCasada;
        }

        private void ConsolidarItensCompraCasada(PesquisaCompraCasadaFiltro filtros)
        {
            foreach (var item in filtros.Itens)
            {
                if (item.PaiCompraCasada.HasValue || item.FilhoCompraCasada.HasValue)
                {
                    SalvarCompraCasadaItem(item);
                }
                else if (item.IDCompraCasada.HasValue)
                {
                    var cc = ConsolidarExclusaoLogica(item);
                    m_auditService.LogUpdate(cc, "blItemPai", "blAtivo");
                }
            }
        }

        private void SalvarCompraCasadaItem(ItemDetalhe item)
        {
            var compraCasada = PopulaCompraCasada(item);

            if (item.FilhoCompraCasada.HasValue && item.FilhoCompraCasada.Value)
            {
                m_relacaoItemLojaCDService.RemoverRelacionamentoPorItemEntrada(item.IDItemDetalhe, false);
            }

            if (compraCasada.IsNew)
            {
                this.MainGateway.Insert(compraCasada);
                m_auditService.LogInsert(compraCasada, "blItemPai", "blAtivo");
                return;
            }

            RemoverVinculoItemPai(item);

            this.MainGateway.Update(compraCasada);
            m_auditService.LogUpdate(compraCasada, "blItemPai", "blAtivo");
        }
    }
}