using System.Web.Http;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ParametroController : ApiControllerBase<IParametroService>
    {
        #region Constructor
        public ParametroController(IParametroService mainService)
            : base(mainService)
        {
        }
        #endregion

        #region Actions
        [HttpGet]
        [Route("Parametro/Estruturado")]
        public Parametro ObterEstruturado()
        {
            return this.MainService.ObterEstruturado();
        }

        [HttpPost]
        public Parametro Salvar(Parametro entidade)
        {
            entidade.cdUsuarioAlteracao = RuntimeContext.Current.User.Id;

            MainService.Salvar(entidade);
            Commit();

            return entidade;
        }
        #endregion
    }
}