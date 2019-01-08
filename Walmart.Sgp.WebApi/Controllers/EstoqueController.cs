using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class EstoqueController : ApiControllerBase<IEstoqueService>
    {
        #region Fields
        private const string NomeArquivoRelatorioDataHora = "{0}_{1:yyyyMMdd}_{1:HHmmss}";
        private readonly IFileVaultService m_fileVaultService;
        #endregion

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EstoqueController"/>.
        /// </summary>
        /// <param name="estoqueService">O serviço de estoque.</param>
        /// <param name="fileVaultService">O serviço de file vault.</param>
        public EstoqueController(IEstoqueService estoqueService, IFileVaultService fileVaultService)
            : base(estoqueService)
        {
            m_fileVaultService = fileVaultService;
        }
             
        [HttpPut]
        [Route("Estoque/Ajustar")]
        public void Ajustar(Estoque entidade)
        {
            MainService.Ajustar(entidade);
            Commit();
        }

        [HttpPut]
        [Route("Estoque/RealizarMtr")]
        public void RealizarMtr(MovimentacaoMtr movimentacaoMtr)
        {
            MainService.RealizarMtr(movimentacaoMtr);
            Commit();
        }

        [HttpGet]
        [Route("Estoque/UltimoCustoContabilItem")]
        public decimal ObterUltimoCustoContabilItem(int idItemDetalhe, int idLoja)
        {
            return MainService.ObterUltimoCustoContabilItem(idItemDetalhe, idLoja);
        }

        [HttpGet]
        [Route("Estoque/UltimosCustosDoItemPorLoja")]
        public IEnumerable<CustoMaisRecente> ObterOsCincoUltimosCustosDoItemPorLoja(int cdItem, int idLoja)
        {
            return MainService.ObterOsCincoUltimosCustosDoItemPorLoja(cdItem, idLoja);
        }

        [HttpPost]
        [Route("Estoque/ExportarRelatorioExtratoProduto")]
        public HttpResponseMessage ExportarRelatorio(ExtratoProdutoModel model)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Loja", string.Format("{0} - {1}", model.CdLoja, model.NmLoja));
            parameters.Add("CdItem", string.Format("{0} - {1}", model.CdItem, model.DsItem));
            parameters.Add("Periodo", string.Format("{0:dd/MM/yyyy} à {1:dd/MM/yyyy}", model.DtIni, model.DtFim));
            parameters.Add("EstoqueInicial", model.EstoqueInicial - model.QtdMovimentacao);
            parameters.Add("IDLOJA", model.IDLoja);
            parameters.Add("DTINI", model.DtIni.ToString("yyyy/MM/dd"));
            parameters.Add("DTFIM", model.DtFim.ToString("yyyy/MM/dd"));
            parameters.Add("IDITEM", model.IDItem);

            var reportName = string.Format(NomeArquivoRelatorioDataHora, "relExtratoProdutoMovimentacao", DateTime.Now);

            return DownloadReportHelper.DownloadExcel(Request, ReportFile.EstoqueExtratoProduto, reportName, parameters, m_fileVaultService);
        }
    }
}
