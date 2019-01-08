using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ItemRelacionamentoController : ApiControllerBase<IItemRelacionamentoService>
    {
        #region Fields
        private readonly IItemDetalheService m_itemDetalheService;
        private readonly IFileVaultService m_fileVaultService;
        private ItemDetalhe m_itemDetalhe;
        #endregion

        #region Constructor
        public ItemRelacionamentoController(IItemRelacionamentoService mainService, IItemDetalheService itemDetalheService, IFileVaultService fileVaultService)
            : base(mainService)
        {
            m_itemDetalheService = itemDetalheService;
            m_fileVaultService = fileVaultService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public RelacionamentoItemPrincipal ObterPorId(int id)
        {
            var result = MainService.ObterPorId(id);

            result.Departamento = result.ItemDetalhe.Departamento;

            return result;
        }

        [HttpGet]
        public IEnumerable<RelacionamentoItemPrincipal> PesquisarPorTipoRelacionamento(int? tipoRelacionamento, string dsItem, int? cdItem, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int? idRegiaoCompra, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarPorTipoRelacionamento(tipoRelacionamento ?? 0, dsItem, cdItem, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema ?? 0, idRegiaoCompra, paging);
        }

        [HttpPost]
        public RelacionamentoItemPrincipal Salvar(RelacionamentoItemPrincipal entidade)
        {
            MainService.Salvar(entidade);
            Commit();

            return entidade;
        }

        [HttpDelete]
        public void Remover(int id)
        {
            MainService.Remover(id);
            Commit();
        }

        [HttpPut]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("ItemRelacionamento/ValidarPrincipal")]
        public void ValidarPrincipal([FromBody]ValidarPrincipalRequest request)
        {
            SpecService.Assert(
                request.ItemDetalhe, 
                new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(
                    MainService, 
                    request.ItemRelacionamento, 
                    request.ItemRelacionamento.EhSaida()));
        }

        [HttpPut]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("ItemRelacionamento/ValidarSecundario")]
        public void ValidarAdicaoItemSecundario([FromBody]ItemRelacionamentoRequest request)
        {
            var principalEhSaida = request.RelacionamentoItemPrincipal.EhSaida();

            SpecService.Assert(request.ItemDetalhe, new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MainService, request.RelacionamentoItemPrincipal, !principalEhSaida));
            SpecService.Assert(request.ItemDetalhe, new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(request.RelacionamentoItemPrincipal));            
        }

        [HttpGet]
        [Route("ItemRelacionamento/{idItemDetalhe}/PercentualRendimentoTransformado")]
        public decimal? ObterPercentualRendimentoTransformado(int? idItemDetalhe, byte? cdSistema)
        {
            // TODO: não deveria utilizar o cdSistema do ItemDetalhe informado?
            return this.MainService.ObterPercentualRendimentoTransformado(idItemDetalhe ?? 0, cdSistema ?? 0);
        }

        [HttpGet]
        [Route("ItemRelacionamento/{idItemDetalhe}/PercentualRendimentoDerivado")]
        public decimal? ObterPercentualRendimentoDerivado(int? idItemDetalhe, byte? cdSistema)
        {
            // TODO: não deveria utilizar o cdSistema do ItemDetalhe informado?
            return this.MainService.ObterPercentualRendimentoDerivado(idItemDetalhe ?? 0, cdSistema ?? 0);
        }

        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("ItemRelacionamento/ExportarRelatorio")]
        public HttpResponseMessage ExportarRelatorio(ItemRelacionamentoModel model)
        {
            if (model.CdItem != null)
            {
                m_itemDetalhe = m_itemDetalheService.ObterPorItemESistema((long)model.CdItem, model.CdSistema);
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ConfiguraExportarRelatorio(model, parameters);

            string reportFile = string.Empty;
            string reportName = string.Empty;

            DefinirExportarRelatorio(model, ref reportFile, ref reportName);

            reportName += string.Format(" - {0:yyyyMMdd}", DateTime.Now);

            return DownloadReportHelper.DownloadExcel(Request, reportFile, reportName, parameters, m_fileVaultService);
        }

        /// <summary>
        /// Pesquisa informações sobre itens relacionados ao item informado.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Informações sobre itens relacionados.</returns>
        [HttpGet]
        [Route("ItemRelacionamento/PorCodigo/{cdItem}/Relacionados")]
        public ItensRelacionadosResponse ObterItensRelacionados(int cdItem, int? idLoja)
        {
            return this.MainService.ObterItensRelacionados(cdItem, idLoja);
        }

        #endregion

        private static void DefinirExportarRelatorio(ItemRelacionamentoModel model, ref string reportFile, ref string reportName)
        {
            if (model.TipoRelacionamento == TipoRelacionamento.Manipulado.Value)
            {
                reportFile = ReportFile.RelacionamentoItensManipulado;
                reportName = Texts.ReportManipulatedItems;
            }
            else if (model.TipoRelacionamento == TipoRelacionamento.Receituario.Value)
            {
                reportFile = ReportFile.RelacionamentoItensReceituario;
                reportName = Texts.ReportPrescriptionItems;
            }
            else if (model.TipoRelacionamento == TipoRelacionamento.Vinculado.Value)
            {
                reportFile = ReportFile.RelacionamentoItensVinculado;
                reportName = Texts.ReportAttachedItems;
            }
        }

        private void ConfiguraExportarRelatorio(ItemRelacionamentoModel model, Dictionary<string, object> parameters)
        {
            parameters.Add("NivelMercadologico", model.CdSistema == TipoSistema.Supercenter.Value ? "Departamento" : "Categoria");
            parameters.Add("cdSistema", model.CdSistema);
            parameters.Add("IDTipoRelacionamento", model.TipoRelacionamento);
            parameters.Add("IDItemDetalhe", m_itemDetalhe == null ? null : (long?)m_itemDetalhe.IDItemDetalhe);
            parameters.Add("IDDepartamento", model.IDDepartamento);
            parameters.Add("IDCategoria", model.IDCategoria);
            parameters.Add("dsItem", model.DsItem);
            parameters.Add("IDSubcategoria", model.IDSubcategoria);
            parameters.Add("IDFineline", model.IDFineline);
            parameters.Add("IDRegiaoCompra", model.IDRegiaoCompra);
        }
    }
}