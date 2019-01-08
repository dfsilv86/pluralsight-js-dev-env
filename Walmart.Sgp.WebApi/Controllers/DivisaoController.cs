using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class DivisaoController : ApiControllerBase<IDivisaoService>
    {
        public DivisaoController(IDivisaoService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Obtém a divisão pelo código da divisão e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A divisão.</returns>
        [HttpGet]
        public Divisao ObterPorDivisaoESistema(int cdDivisao, byte cdSistema)
        {
            return this.MainService.ObterPorDivisaoESistema(cdDivisao, cdSistema);
        }

        /// <summary>
        /// Pesquisa divisões filtrando pelo código de divisão, descrição da divisão e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="dsDivisao">A descrição da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A divisão.</returns>
        /// <remarks>Não valida o usuário que está efetuando a pesquisa. O filtro da descrição é Contains.</remarks>
        [HttpGet]
        public IEnumerable<Divisao> PesquisarPorSistema(int? cdDivisao, string dsDivisao, byte? cdSistema, [FromUri]Paging paging)
        {
            if (string.IsNullOrWhiteSpace(dsDivisao))
            {
                dsDivisao = null;
            }

            return this.MainService.PesquisarPorSistema(cdDivisao, dsDivisao, cdSistema ?? 0, paging);
        }
    }
}