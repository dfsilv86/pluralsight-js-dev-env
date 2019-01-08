using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Implementação de ISsoService para ser utilizada para testes/CI quando o WebGuardian não está acessível.
    /// </summary>
    public class FakeSsoService : ISsoService
    {
        #region Fields
        private readonly IUsuarioService m_usuarioService;
        private readonly IPapelService m_papelService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FakeSsoService"/>.
        /// <param name="usuarioService">O serviço de usuário.</param>
        /// <param name="papelService">O serviço de papel.</param>
        /// </summary>
        public FakeSsoService(IUsuarioService usuarioService, IPapelService papelService)
        {
            m_usuarioService = usuarioService;
            m_papelService = papelService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Altera a senha do usuário.
        /// </summary>
        /// <param name="userName">O nome do usuário.</param>
        /// <param name="currentPassword">A senha atual.</param>
        /// <param name="newPassword">A nova senha.</param>
        public void AlterarSenha(string userName, string currentPassword, string newPassword)
        {
        }

        /// <summary>
        /// Obtém a lista de ações seguras da aplicação.
        /// </summary>
        /// <param name="userName">O nome do usuário.</param>
        /// <param name="password">A senha.</param>
        /// <param name="idExternoAplicacao">O id externo da aplicação.</param>
        /// <returns>As ações.</returns>
        public IEnumerable<UserActionInfo> ObterAcoesSegurasDaAplicacao(string userName, string password, int idExternoAplicacao)
        {
            return new UserActionInfo[0];
        }

        /// <summary>
        /// Obtém as informações do papel, como menus e ações concedidas.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoPapel">O id do papel no WebGuardian.</param>
        /// <returns>As informações do papel.</returns>
        public UserRoleInfo ObterInformacoesDoPapel(string userName, string password, int idExternoPapel)
        {
            return new UserRoleInfo
            {
                GrantedActions = BuildGrantedActions(),
                GrantedMenus = BuildGrantedMenus()
            };
        }

        /// <summary>
        /// Obtém os papéis do usuário na aplicação.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoAplicacao">O id da aplicação no WebGuardian.</param>
        /// <returns>Os papéis.</returns>
        public IEnumerable<Papel> ObterPapeis(string userName, string password, int idExternoAplicacao)
        {
            var papeis = m_papelService.ObterTodos();

            foreach (var papel in papeis)
            {
                papel.IdExterno = papel.Id;
            }

            return papeis;
        }

        /// <summary>
        /// Obtém o papel por seu id no WebGuardian.
        /// </summary>
        /// <param name="idExternoPapel">O id do papel no WebGuardian.</param>
        /// <returns>O papel.</returns>
        public Papel ObterPapel(int idExternoPapel)
        {
            return m_papelService.ObterPorId(idExternoPapel);
        }

        /// <summary>
        /// Obtém o usuário.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <returns>O usuário.</returns>
        public Usuario ObterUsuario(string userName, string password)
        {
            return m_usuarioService.ObterPorUserName(userName);
        }

        /// <summary>
        /// Obtém o usuário.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoAplicacao">O id da aplicação no WebGuardian.</param>
        /// <returns>O usuário.</returns>
        public IEnumerable<Usuario> ObterUsuarios(string userName, string password, int idExternoAplicacao)
        {
            return m_usuarioService.ObterTodos();
        }

        private static IEnumerable<UserMenuInfo> BuildGrantedMenus()
        {
            var menuJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Walmart.Sgp.WebApp\Scripts\app\sgp.menus.js");
            var fileContent = File.ReadAllText(menuJsonFileName);

            var routes = new Regex("'*route'*: '(.+)'", RegexOptions.Compiled | RegexOptions.IgnoreCase).Matches(fileContent).Cast<Match>().Select(m => m.Groups[1].Value);

            return routes.Select(r => new UserMenuInfo(r));
        }

        private static IEnumerable<UserActionInfo> BuildGrantedActions()
        {
            var webApi = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(typeof(FakeSsoService).Assembly.CodeBase.Replace("file:///", string.Empty)), "Walmart.Sgp.WebApi.dll"));

            var controllers = from t in webApi.GetTypes() where t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) select t;

            var actions = from c in controllers select c.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            return GetActionIds(actions).Select(x => new UserActionInfo(x)).ToArray();
        }

        private static IEnumerable<string> GetActionIds(IEnumerable<MethodInfo[]> actions)
        {
            var attrs = new string[] { "HttpGetAttribute", "HttpPostAttribute", "HttpPutAttribute", "HttpDeleteAttribute", "RouteAttribute", "SecurityWebApiActionAttribute" };

            return from a in actions.SelectMany(a => a)
                   where a.CustomAttributes.Any(x => attrs.Contains(x.AttributeType.Name))
                   let defaultActionId = "{0}.{1}".With(a.DeclaringType.Name.Substring(0, a.DeclaringType.Name.Length - "Controller".Length), a.Name.Substring(0, Math.Min(30, a.Name.Length)))
                   let security = a.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "SecurityWebApiActionAttribute" && x.Constructor.GetParameters().Length == 1 && x.Constructor.GetParameters().FirstOrDefault().Name == "actionId")
                   let overridenActionId = null != security && null != security.ConstructorArguments[0].Value ? security.ConstructorArguments[0].Value.ToString() : null
                   select overridenActionId ?? defaultActionId;
        }

        #endregion
    }
}
