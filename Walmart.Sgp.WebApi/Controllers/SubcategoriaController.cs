using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class SubcategoriaController : ApiControllerBase<ISubcategoriaService>
    {
        public SubcategoriaController(ISubcategoriaService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Obtém a subcategoria pelo código de subcategoria e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>A subcategoria.</returns>
        [HttpGet]
        public Subcategoria ObterPorSubcategoriaESistema(int cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            return this.MainService.ObterPorSubcategoriaESistema(cdSubcategoria, cdCategoria, cdDepartamento, cdSistema);
        }

        /// <summary>
        /// Pesquisa subcategorias filtrando pelo código da subcategoria, descrição da subcategoria, código da categoria, código do departamento, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="dsSubcategoria">A descrição da subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>As subcategorias.</returns>
        [HttpGet]
        public IEnumerable<Subcategoria> PesquisarPorCategoriaDepartamentoESistema(int? cdSubcategoria, string dsSubcategoria, int? cdCategoria, int? cdDepartamento, byte? cdSistema, [FromUri]Paging paging)
        {
            if (string.IsNullOrWhiteSpace(dsSubcategoria))
            {
                dsSubcategoria = null;
            }

            return this.MainService.PesquisarPorSubcategoriaCategoriaDepartamentoESistema(cdSubcategoria, dsSubcategoria, cdCategoria, cdDepartamento, cdSistema ?? 0, paging);
        }
    }
}