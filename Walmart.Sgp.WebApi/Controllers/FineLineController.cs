using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class FineLineController : ApiControllerBase<IFineLineService>
    {
        public FineLineController(IFineLineService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Obtém o fineline pelo código de fineline e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código de fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O fineline.</returns>
        [HttpGet]
        public FineLine ObterPorFineLineESistema(int cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            return this.MainService.ObterPorFineLineESistema(cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema);
        }

        /// <summary>
        /// Pesquisa finelines filtrando pelo código do fineline, descrição do fineline, código da subcategoria, código da categoria, código do departamento, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="dsFineLine">A descrição do fineline.</param>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Os finelines.</returns>
        [HttpGet]
        public IEnumerable<FineLine> PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(int? cdFineLine, string dsFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte? cdSistema, [FromUri]Paging paging)
        {
            if (string.IsNullOrWhiteSpace(dsFineLine))
            {
                dsFineLine = null;
            }

            return this.MainService.PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(
                cdFineLine, 
                dsFineLine, 
                cdSubcategoria, 
                cdCategoria, 
                cdDepartamento, 
                cdSistema ?? 0,
                paging);
        }
    }
}
