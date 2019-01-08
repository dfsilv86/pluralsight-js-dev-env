using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.Controllers
{
    /// <summary>
    /// A controller de permissao.
    /// </summary>
    public class PermissaoController : ApiControllerBase<IPermissaoService>
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissaoController"/>.
        /// </summary>
        /// <param name="service">O serviço de permissão.</param>
        public PermissaoController(IPermissaoService service)
            : base(service)
        {
        }

        #endregion

        #region Actions
        [HttpGet]
        public IEnumerable<Permissao> PesquisarComFilhos(int? idUsuario, int? idBandeira, int? idLoja, [FromUri]Paging paging)
        {
            return this.MainService.PesquisarComFilhos(idUsuario, idBandeira, idLoja, paging);
        }

        [HttpGet]
        public Permissao ObterPorId(int idPermissao)
        {
            return this.MainService.ObterPorId(idPermissao);
        }

        [HttpGet]
        [Route("Permissao/{idBandeira}/ValidarInclusaoBandeira")]
        public void ValidarInclusaoBandeira(int idBandeira)
        {
            this.MainService.ValidarInclusaoBandeira(idBandeira);
        }

        [HttpGet]
        [Route("Permissao/{idLoja}/ValidarInclusaoLoja")]
        public void ValidarInclusaoLoja(int idLoja)
        {
            this.MainService.ValidarInclusaoLoja(RuntimeContext.Current.User, idLoja);
        }

        [HttpGet]
        [Route("Permissao/PermissaoManutencao")]
        public bool PossuiPermissaoManutencao()
        {
            return this.MainService.PossuiPermissaoManutencao(RuntimeContext.Current.User);
        }

        [HttpPost]
        public Permissao Salvar(Permissao entidade)
        {
            MainService.Salvar(entidade);
            Commit();

            return entidade;
        }

        [HttpDelete]
        public void Remover(int idPermissao)
        {
            MainService.Remover(idPermissao);
            Commit();
        }
        #endregion
    }
}