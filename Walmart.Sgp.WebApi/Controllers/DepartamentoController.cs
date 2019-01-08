using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.WebApi.Models;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class DepartamentoController : ApiControllerBase<IDepartamentoService>
    {
        public DepartamentoController(IDepartamentoService mainService)
            : base(mainService)
        {
        }

        /// <summary>
        /// Obtém um departamento pelo seu código de departamento e estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="modoPereciveis">Indica como o serviço deve tratar retorno de departamentos em relação a estes serem perecíveis ou outro.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Conforme modoPereciveis, retorna todos departamentos ou apenas se blPerecivel='S' (comportamento padrão das lookups de departamento)</remarks>
        [HttpGet]
        public Departamento ObterPorDepartamentoESistema(int cdDepartamento, byte cdSistema, string modoPereciveis, bool? excluirPadaria)
        {
            return this.MainService.ObterPorDepartamentoESistema(cdDepartamento, cdSistema, modoPereciveis);
        }

        /// <summary>
        /// Pesquisa departamentos filtrando pelo código de departamento, descrição do departamento, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="dsDepartamento">A descrição do departamento.</param>
        /// <param name="blPerecivel">A flag de perecível.</param>
        /// <param name="cdDivisao">O código da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Traz apenas a descrição da divisão.</remarks>
        [HttpGet]
        public IEnumerable<Departamento> PesquisarPorDivisaoESistema(int? cdDepartamento, string dsDepartamento, bool? blPerecivel, int? cdDivisao, byte? cdSistema, bool? excluirPadaria, [FromUri]Paging paging)
        {
            if (string.IsNullOrWhiteSpace(dsDepartamento))
            {
                dsDepartamento = null;
            }

            return this.MainService.PesquisarPorDivisaoESistema(cdDepartamento, dsDepartamento, blPerecivel, cdDivisao, cdSistema ?? 0, paging);
        }

        [HttpGet]
        [Route("Departamento/{id}/Estruturado")]
        public Departamento ObterEstruturadoPorId(int id)
        {
            return MainService.ObterEstruturadoPorId(id);
        }

        [HttpGet]
        [Route("Departamento/PorSistema")]
        public IEnumerable<Departamento> ObterPorSistema(int cdSistema, bool blPerecivel)
        {
            return MainService.ObterPorSistema(cdSistema, blPerecivel);
        }

        [HttpPut]
        public void AtualizarPerecivel(DepartamentoUpdateRequest departamento)
        {
            MainService.AtualizarPerecivel(departamento.IDDepartamento, departamento.BlPerecivel);
            Commit();
        }
    }
}
