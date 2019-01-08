using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.IO.Importing;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;
using Walmart.Sgp.WebApi.Models;
using Walmart.Sgp.WebApi.Properties;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class UsuarioController : ApiControllerBase<IUsuarioService>
    {
        private readonly IPapelService m_papelService;
        private readonly ISsoService m_ssoService;

        public UsuarioController(IUsuarioService usuarioService, IPapelService papelService, ISsoService ssoService)
            : base(usuarioService)
        {
            m_papelService = papelService;
            m_ssoService = ssoService;
        }

        [HttpGet]
        public IEnumerable<Usuario> ObterTodos([FromUri] Paging paging)
        {
            return MainService.ObterTodos(paging);
        }

        [HttpGet]
        public IEnumerable<Usuario> Pesquisar(string userName, [FromUri]Paging paging)
        {            
            return MainService.Pesquisar(userName, paging);
        }

        [HttpGet]
        [Route("Usuario/PorUsuario/{userName}/")]
        public UsuarioResumo ObterPorUsuario(string userName)
        {
            return MainService.ObterResumidoPorUserName(userName);
        }

        [HttpGet]
        [Route("Usuario/{id}")]
        public UsuarioResumo ObterPorId(int id)
        {
            return MainService.ObterResumidoPorId(id);
        }

        [HttpGet]
        public IEnumerable<UsuarioResumo> PesquisarPorUsuario(string userName, string fullName, string email, int? cdUsuario, [FromUri]Paging paging)
        {
            return MainService.PesquisarResumidoPorUsuario(userName, fullName, email, cdUsuario, paging);
        }

        [HttpPost]
        [Route("Usuario/Importacao")]
        public int ImportarUsuarios(LoginRequest request)
        {
            var settings = Settings.Default;
            
            var usuariosParaImportar = m_ssoService.ObterUsuarios(RuntimeContext.Current.User.UserName, request.Password, settings.SsoApplicationCode).ToArray();
            var importer = new UsuarioImporter(MainService, m_papelService);

            foreach (var usuarioParaImportar in usuariosParaImportar)
            {
                importer.Import(new UsuarioAcessoInfo() { Usuario = usuarioParaImportar });
            }

            Commit();
            return usuariosParaImportar.Length;
        }
    }
}