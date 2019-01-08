using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class FornecedorController : ApiControllerBase<IFornecedorService>
    {
        public FornecedorController(IFornecedorService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Obtém um Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>A entidade Fornecedor.</returns>
        [HttpGet]
        public Fornecedor ObterPorSistemaCodigo(short cdSistema, int cdFornecedor)
        {
            return this.MainService.ObterPorSistemaCodigo(cdSistema, cdFornecedor);
        }

        /// <summary>
        /// Obtém uma lista de Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">Paginação do resultado.</param>
        /// <returns>A lista de entidade Fornecedor.</returns>
        [HttpGet]
        public IEnumerable<Fornecedor> ObterListaPorSistemaCodigoNome(short cdSistema, int? cdFornecedor, string nmFornecedor, [FromUri]Paging paging)
        {
            if (String.IsNullOrEmpty(nmFornecedor))
            {
                nmFornecedor = null;
            }

            return this.MainService.ObterListaPorSistemaCodigoNome(cdSistema, cdFornecedor, nmFornecedor, paging);
        }
    }
}