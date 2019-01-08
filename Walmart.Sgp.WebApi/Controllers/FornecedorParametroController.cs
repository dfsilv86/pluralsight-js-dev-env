using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class FornecedorParametroController : ApiControllerBase<IFornecedorParametroService>
    {
        public FornecedorParametroController(IFornecedorParametroService mainService) 
            : base(mainService)
        {
        }

        /// <summary>
        /// Pesquisa parametros de fornecedores baseado nos filtros informados.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="stFornecedor">O status do fornecedor.</param>
        /// <param name="cdV9D">O código do vendor (9 dígitos).</param>
        /// <param name="nmFornecedor">A razão social.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parâmetros de fornecedores que se encaixam no filtro informado.</returns>
        [HttpGet]
        [Route("Fornecedor/Parametro")]
        public IEnumerable<FornecedorParametro> PesquisarPorFiltro(int cdSistema, string stFornecedor, long? cdV9D, string nmFornecedor, [FromUri] Paging paging)
        {
            return MainService.PesquisarPorFiltro(cdSistema, stFornecedor, cdV9D, nmFornecedor, paging);
        }

        [HttpGet]
        [Route("Fornecedor/Parametro/{id}")]
        public FornecedorParametro ObterEstruturadoPorId(int id)
        {
            return MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        [Route("Fornecedor/Parametro/{idFornecedorParametro}/ReviewDate/{detalhamento}")]
        public IEnumerable<FornecedorParametroReviewDate> ObterReviewDates(int idFornecedorParametro, string detalhamento, [FromUri]Paging paging)
        {
            return MainService.ObterReviewDatesPorDetalhe(idFornecedorParametro, (TipoDetalhamentoReviewDate)detalhamento, paging);
        }

        [HttpGet]
        [Route("Fornecedor/Parametro/Vendor")]
        public IEnumerable<FornecedorParametro> PesquisarPorSistemaCodigo9DigitosENomeFornecedor(int cdSistema, string cdTipo, long? cdV9D, string nmFornecedor, [FromUri]Paging paging)
        {
            nmFornecedor = string.IsNullOrWhiteSpace(nmFornecedor) ? null : nmFornecedor;

            return this.MainService.PesquisarPorSistemaCodigo9DigitosENomeFornecedor(cdSistema, cdTipo, cdV9D, nmFornecedor, paging);
        }

        [HttpGet]
        [Route("Fornecedor/Parametro/Vendor/{cdV9D}")]
        public FornecedorParametro ObterPorSistemaCodigo9DigitosENomeFornecedor(int cdSistema, string cdTipo, long? cdV9D)
        {
            return this.MainService.ObterPorSistemaECodigo9Digitos(cdSistema, cdTipo, cdV9D);
        }
    }
}