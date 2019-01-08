using System;
using System.Collections.Generic;
using System.Web.Http;
using Walmart.Sgp.Domain.Movimentacao;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class MovimentacaoController : ApiControllerBase<IMovimentacaoService>
    {
        public MovimentacaoController(IMovimentacaoService service) : base(service)
        {
        }

        [HttpGet]
        public IEnumerable<ItemExtrato> RelExtratoProdutoMovimentacao(int idLoja, long idItemDetalhe, DateTime dtIni, DateTime dtFim)
        {
            return this.MainService.RelExtratoProdutoMovimentacao(idLoja, dtIni, dtFim, idItemDetalhe, "N", null);
        }

        [HttpGet]
        public IEnumerable<ItemExtrato> RelExtratoProdutoMovimentacao(int idLoja, long idItemDetalhe, DateTime dtIni, DateTime dtFim, int? idInventario)
        {
            return this.MainService.RelExtratoProdutoMovimentacao(idLoja, dtIni, dtFim, idItemDetalhe, "I", idInventario);
        }

        [HttpGet]
        [Route("Movimentacao/{id}/estruturado")]
        public Movimentacao ObterEstruturadoPorId(int id)
        {
            var result = MainService.ObterEstruturadoPorId(id);

            return result;
        }

        [HttpGet]
        [Route("Movimentacao/DatasDeQuebra")]
        public IEnumerable<DateTime> ObterDatasDeQuebra(int idItemDetalhe, int idLoja)
        {
            var result = MainService.ObterDatasDeQuebra(idItemDetalhe, idLoja);

            return result;
        }
    }
}