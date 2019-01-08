using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Serviço para acesso ao WebGuardian.
    /// </summary>
    public class WebGuardianSsoService : ISsoService
    {
        #region Fields
        private static readonly Regex s_isValidEmailRegex = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Compiled);
        private string m_emailDomain;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="WebGuardianSsoService"/>.
        /// </summary>
        /// <param name="emailDomain">O domínio do e-mail utilizado ao traduzir o usuário.</param>
        public WebGuardianSsoService(string emailDomain)
        {
            m_emailDomain = emailDomain;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o usuário.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <returns>O usuário.</returns>
        public Usuario ObterUsuario(string userName, string password)
        {
            var user = GetFromWebGuardian(Texts.UserData, (client) => client.ObterUsuario(userName, password));
            return Translate(user);
        }

        /// <summary>
        /// Obtém os usuários.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoAplicacao">O id da aplicação no WebGuardian.</param>
        /// <returns>Os usuários.</returns>
        public IEnumerable<Usuario> ObterUsuarios(string userName, string password, int idExternoAplicacao)
        {
            var user = GetFromWebGuardian(Texts.UserData, (client) => client.ObterUsuariosAplicacao(userName, password, idExternoAplicacao));

            // Utiliza o distinct para remover os usuários que o WebGuardian retornar repetidos, pelo menos em ambiente de DEV, parecia que
            // ele duplicava para cada papel que o usuário estava relacionado.
            return user.Distinct(new UsuarioTOEqualityComparer()).Select(u => Translate(u));
        }

        /// <summary>
        /// Obtém o papel por seu id no WebGuardian.
        /// </summary>
        /// <param name="idExternoPapel">O id do papel no WebGuardian.</param>
        /// <returns>O papel.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Papel ObterPapel(int idExternoPapel)
        {
            var profile = GetFromWebGuardian(Texts.ProfileData, (client) => client.ObterPerfil(idExternoPapel));
            return Translate(profile);
        }

        /// <summary>
        /// Obtém os papéis do usuário na aplicação.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoAplicacao">O id da aplicação no WebGuardian.</param>
        /// <returns>Os papéis.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IEnumerable<Papel> ObterPapeis(string userName, string password, int idExternoAplicacao)
        {
            var profiles = GetFromWebGuardian(Texts.ProfileData, (client) => client.ObterPerfisUsuario(userName, password, idExternoAplicacao));
            return profiles.Select(p => Translate(p));
        }

        /// <summary>
        /// Obtém as informações do papel, como menus e ações concedidas.
        /// </summary>
        /// <param name="userName">O username do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="idExternoPapel">O id do papel no WebGuardian.</param>
        /// <returns>As informações do papel.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public UserRoleInfo ObterInformacoesDoPapel(string userName, string password, int idExternoPapel)
        {
            var permissoes = GetFromWebGuardian<PermissoesTO>(
                Texts.Routes,
                (client) => client.ObterPermissoesPerfil(userName, password, idExternoPapel));

            var result = RemoverDuplicidadesPerfil(userName, permissoes);

            IEnumerable<string> menusDistintos = result.Item1;
            IEnumerable<string> eventosDistintos = result.Item2;

            return new UserRoleInfo
            {
                GrantedMenus = menusDistintos.Select(url => new UserMenuInfo(url)),
                GrantedActions = eventosDistintos.Select(descricao => new UserActionInfo(descricao))
            };
        }

        /// <summary>
        /// Obtém a lista de ações seguras da aplicação.
        /// </summary>
        /// <param name="userName">O nome do usuário.</param>
        /// <param name="password">A senha.</param>
        /// <param name="idExternoAplicacao">O id externo da aplicação.</param>
        /// <returns>As ações.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IEnumerable<UserActionInfo> ObterAcoesSegurasDaAplicacao(string userName, string password, int idExternoAplicacao)
        {
            var eventos = GetFromWebGuardian<EventoTO[]>(
                Texts.Events,
                (client) => client.ObterEventosAplicacao(userName, password, idExternoAplicacao));

            return eventos
                .Where(e => e.Status == enStatus.stAtivo)
                .Select(e => Translate(e));
        }

        /// <summary>
        /// Altera a senha do usuário.
        /// </summary>
        /// <param name="userName">O nome do usuário.</param>
        /// <param name="currentPassword">A senha atual.</param>
        /// <param name="newPassword">A nova senha.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void AlterarSenha(string userName, string currentPassword, string newPassword)
        {
            GetResultFromWebGuardian(
                Texts.ChangePassword,
                (client) => client.AlterarSenha(userName, currentPassword, newPassword));
        }

        private static TResult GetFromWebGuardian<TResult>(string name, Func<ServiceWebGuardianSoapClient, IWebGuardianResultado<TResult>> call)
        {
            var result = GetResultFromWebGuardian(name, call);
            return ((IWebGuardianResultado<TResult>)result).Dado;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private static IWebGuardianResultado GetResultFromWebGuardian(string name, Func<ServiceWebGuardianSoapClient, IWebGuardianResultado> call)
        {
            try
            {
                using (var client = new ServiceWebGuardianSoapClient())
                {
                    var result = call(client);

                    if (result.Status.CodigoRetorno == 0)
                    {
                        return result;
                    }

                    throw new WebGuardianException(result.Status);
                }
            }
            catch (WebGuardianException ex)
            {
                if (ex.Message == null)
                {
                    LogService.Error(Texts.WebGuardianErrorLog, name, ex.ErrorCode, ex.OriginalMessage);
                    throw new InvalidOperationException(Texts.ErrorGettingDataFromWebGuardian.With(name.ToLowerInvariant(), ex.OriginalMessage), ex);
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Texts.ErrorGettingDataFromWebGuardian.With(name.ToLowerInvariant(), ex.Message), ex);
            }
        }

        private static Tuple<IEnumerable<string>, IEnumerable<string>> RemoverDuplicidadesPerfil(string userName, PermissoesTO permissoes)
        {
            var menus = permissoes.Menus.Where(menuTo => menuTo.Status == enStatus.stAtivo).GroupBy(menuTo => menuTo.Url).ToArray();
            var menusDistintos = menus.Select(grupo => grupo.Key).ToArray();
            var menusDuplicados = menus.Where(grupo => grupo.Count() > 1).ToArray();

            var eventos = permissoes.Eventos.Where(eventoTo => eventoTo.Status == enStatus.stAtivo).GroupBy(eventoTo => eventoTo.Descricao).ToArray();
            var eventosDistintos = eventos.Select(grupo => grupo.Key).ToArray();
            var eventosDuplicados = eventos.Where(grupo => grupo.Count() > 1).ToArray();

            LogarDuplicidadesPerfil(userName, permissoes, menusDuplicados, eventosDuplicados);

            return new Tuple<IEnumerable<string>, IEnumerable<string>>(menusDistintos, eventosDistintos);
        }

        private static void LogarDuplicidadesPerfil(string userName, PermissoesTO permissoes, IGrouping<string, MenuTO>[] menusDuplicados, IGrouping<string, EventoTO>[] eventosDuplicados)
        {
            if (menusDuplicados.Count() > 0)
            {
                LogService.Warning(Texts.WebGuardianMenuDuplicates.With(permissoes.Perfil.Nome, userName, menusDuplicados.Select(group => group.Key).JoinWords()));
            }

            if (eventosDuplicados.Count() > 0)
            {
                LogService.Warning(Texts.WebGuardianEventDuplicates.With(permissoes.Perfil.Nome, userName, eventosDuplicados.Select(group => group.Key).JoinWords()));
            }
        }

        private static Papel Translate(PerfilTO profile)
        {
            var papel = new Papel
            {
                IdExterno = profile.Codigo,
                Name = profile.Nome,
                Description = profile.Descricao
            };

            // Em razão de um erro no portal IDM que interpreta os perfis do WebGuardian sem considerar em qual aplicação ele foi criado, ele
            // entende, por exemplo, que o perfil SGP-Administrador do SGP legado é o mesmo do do perfil SGP-Administrador do SGP Reescrita.
            // Para resolver, no WebGuardian do SGP Reescrita foi removido prefixo "SGP-" por "REE-" e aqui ao trazermos do WebGuardian, fazemos a substituição
            // para "SGP-" novamente, assim evitando que seja criado outro perfil já que é necessário que sejam mantidos os mesmo em razão de usar o mesmo banco do
            // legado e questões de permissão por perfil (alçadas).
            papel.Name = papel.Name.Replace("REE-", "SGP-");

            return papel;
        }

        private static UserActionInfo Translate(EventoTO evento)
        {
            return new UserActionInfo(evento.Descricao);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private Usuario Translate(UsuarioTO user)
        {
            return new Usuario
            {
                UserName = user.Login.ToLowerInvariant().Trim(),
                FullName = user.Nome,
                Passwd = user.Senha,
                PasswdFormat = 0,
                Email = ParseEmail(user),
                CreationDate = user.Dt_Inclusao,
                IsApproved = true,
                IsLockedOut = false
            };
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private string ParseEmail(UsuarioTO user)
        {
            var email = user.Email;

            if (!s_isValidEmailRegex.IsMatch(email) && !string.IsNullOrEmpty(m_emailDomain))
            {
                email = "{0}@{1}".With(user.Login.ToLowerInvariant().Trim(), m_emailDomain);
            }

            return email;
        }
        #endregion
    }
}
