using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class CategoriaController : ApiControllerBase<ICategoriaService>
    {
        public CategoriaController(ICategoriaService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Obtém uma categoria a partir do código da categoria e do código de estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>A categoria.</returns>
        [HttpGet]
        public Categoria ObterPorCategoriaESistema(int cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            return this.MainService.ObterPorCategoriaESistema(cdCategoria, cdDepartamento, cdSistema);
        }

        /// <summary>
        /// Pesquisa categorias filtrando pelo código da categoria, descrição da categoria, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="dsCategoria">The ds categoria.</param>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <returns>As categorias.</returns>
        [HttpGet]
        public IEnumerable<Categoria> PesquisarPorCategoriaDepartamentoESistema(int? cdCategoria, string dsCategoria, byte? cdSistema, int? cdDepartamento, [FromUri]Paging paging)
        {
            if (string.IsNullOrWhiteSpace(dsCategoria))
            {
                dsCategoria = null;
            }

            return this.MainService.PesquisarPorCategoriaDepartamentoESistema(cdCategoria, dsCategoria, cdSistema ?? 0, cdDepartamento, paging);
        }

        [HttpGet]
        [Route("Categoria/PorSistema")]
        public IEnumerable<Categoria> ObterPorSistema(int cdSistema)
        {
            return MainService.ObterPorSistema(cdSistema);
        }
    }
}