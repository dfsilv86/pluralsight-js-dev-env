using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ItemDetalheController : ApiControllerBase<IItemDetalheService>
    {
        #region Fields
        private const string NomeArquivoRelatorioDataHora = "{0}_{1:yyyyMMdd}_{1:HHmmss}";
        private readonly IFileVaultService m_fileVault;
        private readonly IEstoqueService m_estoqueService;
        private readonly IParametroSistemaService m_parametroSistemaService;
        #endregion

        #region Constructor
        public ItemDetalheController(IItemDetalheService mainService, IEstoqueService estoqueService, IFileVaultService fileVault, IParametroSistemaService parametroSistemaService)
            : base(mainService)
        {
            m_estoqueService = estoqueService;
            m_fileVault = fileVault;
            m_parametroSistemaService = parametroSistemaService;
        }
        #endregion

        #region Actions

        [HttpGet]
        public ItemDetalhe ObterPorId(int id)
        {
            return this.MainService.ObterPorId(id);
        }

        [HttpGet]
        [Route("ItemDetalhe/{id}/Estruturado")]
        public ItemDetalhe ObterEstruturadoPorId(int id)
        {
            return this.MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        public ItemDetalhe ObterPorOldNumberESistema(long cdItem, byte cdSistema)
        {
            return this.MainService.ObterPorItemESistema(cdItem, cdSistema);
        }

        [HttpGet]
        public ItemDetalhe ObterPorOldNumberESistemaDepartamento(long cdItem, byte cdSistema, int cdDepartamento)
        {
            return this.MainService.ObterPorOldNumberESistemaEDepartamento(cdItem, cdSistema, cdDepartamento);
        }

        [HttpGet]
        public ItemDetalhe ObterPorPluESistema(long cdPLU, byte cdSistema)
        {
            return this.MainService.ObterPorPluESistema(cdPLU, cdSistema);
        }

        [HttpGet]
        public IEnumerable<ItemDetalheCD> PesquisarItensSaidaComCDConvertido(long? cdItem, int? cdDepartamento, int? cdSistema, int? idCD, int filtroMS, int filtroCadastro, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarItensSaidaComCDConvertido(cdItem, cdDepartamento, cdSistema, idCD, filtroMS, filtroCadastro, paging);
        }

        [HttpGet]
        public IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarItensEntradaPorSaidaCD(cdItem, cdCD, cdSistema, paging);
        }

        [HttpGet]
        [Route("ItemDetalhe/ItemEntradaPorItemSaida")]
        public IEnumerable<ItemDetalhe> ObterItemEntradaPorItemSaida(byte cdSistema, long cdItemSaida, int? idFornecedorParametro)
        {
            return this.MainService.ObterItemEntradaPorItemSaida(cdSistema, cdItemSaida, idFornecedorParametro);
        }

        [HttpGet]
        [Route("ItemDetalhe/ItemSaidaPorFornecedorItemEntrada")]
        public ItemDetalhe ObterItemSaidaPorFornecedorItemEntrada(long cdItem, byte cdSistema, int? idFornecedorParametro, int? idRegiaoCompra, string tpStatus, string blPerecivel)
        {
            return this.MainService.ObterItemSaidaPorFornecedorItemEntrada(cdItem, cdSistema, idFornecedorParametro, idRegiaoCompra, tpStatus, blPerecivel);
        }

        [HttpGet]
        [Route("ItemDetalhe/ListaItemSaidaPorFornecedorItemEntrada")]
        public IEnumerable<ItemDetalhe> ObterListaItemSaidaPorFornecedorItemEntrada([FromUri]ItemDetalheFiltro filtro, [FromUri]Paging paging)
        {
            return this.MainService.ObterListaItemSaidaPorFornecedorItemEntrada(filtro, paging);
        }

        [HttpGet]
        public IEnumerable<ItemDetalhe> PesquisarPorFiltro(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int? idUsuario, string tpVinculado, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarPorFiltro(cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario ?? 0, tpVinculado, paging);
        }

        [HttpGet]
        [Route("ItemDetalhe/TipoReabastecimento")]
        public IEnumerable<ItemDetalhe> PesquisarPorFiltroTipoReabastecimento(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int? idUsuario, int? idRegiaoCompra, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarPorFiltroTipoReabastecimento(cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario ?? 0, idRegiaoCompra, paging);
        }

        [HttpPut]
        [Route("ItemDetalhe/{idItemDetalhe}/Custos")]
        public void AtualizarDadosCustos(int idItemDetalhe, [FromBody] ItemDetalhe itemDetalhe)
        {
            itemDetalhe.IDItemDetalhe = idItemDetalhe;

            this.MainService.AlterarDadosCustos(itemDetalhe);

            Commit();
        }

        [HttpPut]
        [Route("ItemDetalhe/{id}/InformacoesCadastrais")]
        public void AlterarInformacoesCadastrais(int id, [FromBody]ItemDetalhe itemDetalhe)
        {
            itemDetalhe.IDItemDetalhe = id;

            this.MainService.AlterarInformacoesCadastrais(itemDetalhe);

            Commit();
        }

        [HttpGet]
        [Route("ItemDetalhe/{idItemDetalhe}/Custos/MaisRecentesPorLoja")]
        public IEnumerable<Estoque> PesquisarUltimoCustoDoItemPorLoja(int idItemDetalhe, int? idLoja, [FromUri] Paging paging)
        {
            return this.m_estoqueService.PesquisarUltimoCustoDoItemPorLoja(idItemDetalhe, idLoja, paging);
        }

        /// <summary>
        /// Obtém os últimos 5 recebimentos do item ou de suas entradas.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 recebimentos.</returns>
        /// <remarks>Como a consulta utiliza também os itens de entrada do item informado, é possível que uma NotaFiscal possua mais de um NotaFiscalItem. O conjunto de todos NotaFiscalItem deve ter os 5 itens.</remarks>
        [HttpGet]
        [Route("ItemDetalhe/{idItemDetalhe}/Custos/Loja/{idLoja}/UltimosCincoRecebimentos")]
        public IEnumerable<NotaFiscal> ObterOsCincoUltimosRecebimentosDoItemPorLoja(int idItemDetalhe, int idLoja)
        {
            return this.m_estoqueService.ObterOsCincoUltimosRecebimentosDoItemPorLoja(idItemDetalhe, idLoja);
        }

        /// <summary>
        /// Obtém os custos mais recentes de itens relacionados a um item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>Os custos.</returns>
        [HttpGet]
        [Route("ItemDetalhe/{idItemDetalhe}/Custos/Loja/{idLoja}/CustosRelacionados")]
        public IEnumerable<CustoItemRelacionadoResponse> ObtemUltimoCustoDeItensRelacionadosNaLoja(int idItemDetalhe, int idLoja)
        {
            return this.m_estoqueService.ObterUltimoCustoDeItensRelacionadosNaLoja(idItemDetalhe, idLoja);
        }

        [HttpGet]
        [Route("ItemDetalhe/Estruturado")]
        public IEnumerable<ResultadoConsultaItem> Consultar([FromUri] ItemFiltro filtro, [FromUri] Paging paging)
        {
            return MainService.PesquisarAbertoPorBandeira(filtro, paging);
        }

        [HttpGet]
        [Route("ItemDetalhe/Estoque")]
        public IEnumerable<ResultadoConsultaItemPorLoja> ObterInformacoesEstoquePorLoja(int cdItem, int idBandeira, int? idLoja, [FromUri]Paging paging)
        {
            return MainService.ObterInformacoesEstoquePorLoja(cdItem, idBandeira, idLoja, paging);
        }

        [HttpGet]
        [Route("ItemDetalhe/InformacoesCadastrais")]
        public ObterInformacoesCadastraisResponse ObterInformacoesCadastrais(int cdItem, int idBandeira)
        {
            var itemDetalheBandeira = MainService.ObterInformacoesCadastrais(cdItem, idBandeira);

            if (itemDetalheBandeira == null)
            {
                return null;
            }

            return new ObterInformacoesCadastraisResponse
            {
                ItemDetalhe = itemDetalheBandeira.Item1,
                Bandeira = itemDetalheBandeira.Item2
            };            
        }

        [HttpGet]
        public IEnumerable<ItemDetalhe> ObterItensDetalheReturnSheet(int relacionamentoSGP, int cdDepartamento, int? cdItemDetalhe, long? cdV9D, int? idRegiaoCompra, int cdSistema, int idReturnSheet, [FromUri]Paging paging)
        {
            return this.MainService.ObterItensDetalheReturnSheet(relacionamentoSGP, cdDepartamento, cdItemDetalhe, cdV9D, idRegiaoCompra, cdSistema, idReturnSheet, paging);
        }

        [HttpGet]
        [Route("ItemDetalhe/Custos")]
        public IEnumerable<ItemCusto> ObterItemCustos(int cdItem, int idBandeira, int? idLoja, [FromUri]Paging paging)
        {
            return MainService.ObterItemCustos(cdItem, idBandeira, idLoja, paging);
        }

        [HttpGet]
        [Route("ItemDetalhe/ValidarIntervaloNaoDeveExcederSessentaDias")]
        public void ValidarIntervaloNaoDeveExcederSessentaDias([FromUri]RangeValue<DateTime> periodo)
        {
            SpecService.Assert(periodo, new IntervaloNaoDeveExcederSessentaDiasSpec());
        }

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("ItemDetalhe/ExportarRelatorioItensAbcVendas")]
        public HttpResponseMessage ExportarRelatorioItensAbcVendas(ItensAbcVendasModel model)
        {
            SpecService.Assert(model.Periodo, new DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec(m_parametroSistemaService));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("DeptoCat", model.DeptoCat);
            parameters.Add("lblDeptoCat", model.LblDeptoCat);
            parameters.Add("Loja", model.Loja);
            parameters.Add("Periodo", string.Format("{0:dd/MM/yyyy} à {1:dd/MM/yyyy}", model.Periodo.StartValue, model.Periodo.EndValue));
            parameters.Add("emissao", string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            parameters.Add("idLoja", model.IDLoja);
            parameters.Add("idDepartamento", model.IDDepartamento);
            parameters.Add("idCategoria", model.IDCategoria);
            parameters.Add("dtIni", model.Periodo.StartValue.Value.ToString("yyyy/MM/dd"));
            parameters.Add("dtFim", model.Periodo.EndValue.Value.ToString("yyyy/MM/dd"));

            var reportName = string.Format(NomeArquivoRelatorioDataHora, "RelABCVendas", DateTime.Now);

            return Download(ReportFile.ItemDetalheItensAbcVendas, reportName, parameters);
        }

        [HttpGet]
        [Route("ItemDetalhe/ObterTraitsPorItem")]
        public IEnumerable<Loja> ObterTraitsPorItem(int idItemDetalhe, int cdSistema, [FromUri] Paging paging)
        {
            return this.MainService.ObterTraitsPorItem(idItemDetalhe, cdSistema, paging);
        }

        private HttpResponseMessage Download(string reportName, string fileName, Dictionary<string, object> parameters)
        {
            return DownloadReportHelper.DownloadExcel(Request, reportName, fileName, parameters, m_fileVault);
        }
        #endregion
    }
}