using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using Walmart.Sgp.Application.Exporting;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento.CompraCasada;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;
using Walmart.Sgp.Infrastructure.Web.Security;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class CompraCasadaController : ApiControllerBase<ICompraCasadaService>
    {
        private readonly IFileVaultService m_fileVaultService;
        private CompraCasadaExcelExporter m_compraCasadaExcelExporter;

        public CompraCasadaController(ICompraCasadaService mainService, CompraCasadaExcelExporter compraCasadaExcelExporter, IFileVaultService fileVaultService)
            : base(mainService)
        {
            m_compraCasadaExcelExporter = compraCasadaExcelExporter;
            m_fileVaultService = fileVaultService;
        }

        /// <summary>
        /// Verifica se um cadastro de compra casada possui pai.
        /// </summary>
        /// <param name="filtros">Os filtros e os itens em memoria.</param>
        /// <returns>Se tem ou nao pai.</returns>
        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("CompraCasada/VerificaPossuiPai")]
        public ItemDetalhe VerificaPossuiPai(PesquisaCompraCasadaFiltro filtros)
        {
            return this.MainService.VerificaPossuiPai(filtros);
        }

        /// <summary>
        /// Excluir a compra casada.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        [HttpDelete]
        [SecurityWebApiAction("CompraCasada.Excluir")]
        [Route("CompraCasada/Excluir")]
        public void Excluir([FromUri] PesquisaCompraCasadaFiltro filtros)
        {
            this.MainService.ExcluirItensCompraCasada(filtros);
            Commit();
        }

        /// <summary>
        /// Verifica se a compra casada possui cadastro.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        /// <returns>Se existe ou nao cadastro.</returns>
        [HttpGet]
        [Route("CompraCasada/VerificaPossuiCadastro")]
        public bool VerificaPossuiCadastro([FromUri] PesquisaCompraCasadaFiltro filtros)
        {
            return this.MainService.VerificaPossuiCadastro(filtros);
        }

        /// <summary>
        /// Salvar compra casada.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada e os itens alterados.</param>
        /// <returns>Uma lista de SpecResult se houver problemas, caso contrário NULL.</returns>
        [HttpPost]
        [SecurityWebApiAction("CompraCasada.Salvar")]
        [Route("CompraCasada/Salvar")]
        public IEnumerable<SpecResult> Salvar(PesquisaCompraCasadaFiltro filtros)
        {
            var result = this.MainService.SalvarItensCompraCasada(filtros);
            Commit();

            return result;
        }

        /// <summary>
        /// Validações utilizadas nos itens filhos no front-end.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada e os itens alterados.</param>
        /// <returns>Uma lista de SpecResult se houver problemas, caso contrário NULL.</returns>
        [HttpPost]
        [SecurityWebApiAction(AllowWriteActionWithoutPermission = true)]
        [Route("CompraCasada/ValidarItemFilhoMarcado")]
        public IEnumerable<SpecResult> ValidarItemFilhoMarcado(PesquisaCompraCasadaFiltro filtros)
        {
            return MainService.ValidarItemFilhoMarcado(filtros);
        }

        /// <summary>
        /// Pesquisar Itens de entrada para Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        [HttpGet, Route("CompraCasada/PesquisarItensEntrada")]
        public IEnumerable<ItemDetalhe> PesquisarItensEntrada([FromUri] PesquisaCompraCasadaFiltro filtro, [FromUri] Paging paging)
        {
            return this.MainService.PesquisarItensEntrada(filtro, paging);
        }

        /// <summary>
        /// Pesquisar Itens para Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        [HttpGet, Route("CompraCasada/Pesquisar")]
        public IEnumerable<ItemDetalhe> PesquisarItensCompraCasada([FromUri] PesquisaCompraCasadaFiltro filtro, [FromUri] Paging paging)
        {
            return this.MainService.PesquisarItensCompraCasada(filtro, paging);
        }

        /// <summary>
        /// Realiza a exportação dos registros de cadastro de compra casada.
        /// </summary>
        /// <param name="filtro">Os filtros da exportação.</param>
        /// <returns>Planilha com os dados exportados.</returns>
        [HttpGet, Route("CompraCasada/Exportar")]
        public HttpResponseMessage Exportar([FromUri] PesquisaCompraCasadaFiltro filtro)
        {
            var stream = m_compraCasadaExcelExporter.Exportar(filtro);

            return stream.WithFileVault("Exportacao.xlsx", m_fileVaultService);
        }
    }
}