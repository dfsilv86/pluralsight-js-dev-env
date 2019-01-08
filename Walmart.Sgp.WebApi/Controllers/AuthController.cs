using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Importing;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.App_Start;
using Walmart.Sgp.WebApi.Models;
using Pstore.WebApi.Models;
using Pstore.WebApi.Properties;

namespace Walmart.Sgp.WebApi.Controllers
{
    [AllowAnonymous]
    public class AuthController : ApiControllerBase<IUsuarioService>
    {
        #region Fields
        private readonly IPapelService m_papelService;
        private readonly IPermissaoService m_permissaoService;
        private readonly ISsoService m_ssoService;
        private readonly ILojaService m_lojaService;
        #endregion

        #region Constructors
        public AuthController(IUsuarioService service, IPapelService papelService, IPermissaoService permissaoService, ISsoService ssoService, ILojaService lojaService)
            : base(service)
        {
            m_papelService = papelService;
            m_permissaoService = permissaoService;
            m_ssoService = ssoService;
            m_lojaService = lojaService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("Auth/WebApiVersion")]
        public string GetWebApiVersion()
        {
            return WebApiConfig.ApiVersion;
        }

        [HttpGet]
        [Route("Auth/ServerName")]
        public string GetServerName()
        {
            return Environment.MachineName;
        }

        [HttpPost]
        public LoginResponse Login([FromBody]LoginRequest authInfo)
        {
          //  var user = MainService.ObterPorUserName(authInfo.UserName);

            var userModel = new UserModel
            {
                Nome = authInfo.UserName,
                Dispensa = null
               //Papeis = papeis,
               //Lojas = usuarioPermissoes.TipoPermissao == TipoPermissao.PorLoja ? usuarioPermissoes.Lojas : null,
               //HasPermissions = false,
               //TipoPermissao = usuarioPermissoes.TipoPermissao,
               //Culture = RuntimeContext.Current.Culture
            };

            Commit();

            return new LoginResponse
            {
                Token = CreateToken(userModel),
                User = userModel
            };

            //return !authInfo.IdExternoPapel.HasValue || authInfo.IdExternoPapel.Value == 0
            //    ? LogarSemPapel(authInfo)
            //    : LogarComPapel(authInfo);
        }

        [HttpPut]
        public void AlterarSenha([FromBody]ChangePasswordRequest model)
        {
            SpecService.Assert(model, new AllMustBeInformedSpec());
            SpecService.Assert(new { model.NewPassword, model.ConfirmPassword }, new AllMustBeEqualSpec());
            m_ssoService.AlterarSenha(model.UserName, model.CurrentPassword, model.NewPassword);
        }

        /// <summary>
        /// Obtém as lojas que o usuário corrente tem acesso.
        /// </summary>
        /// <returns>As lojas.</returns>
        [HttpPut]
        [Route("Auth/Users/Current/Stores")]
        public IEnumerable<Loja> ObterLojas()
        {
            var currentUser = RuntimeContext.Current.User;
            var usuarioPermissoes = m_permissaoService.ObterPermissoesDoUsuario(currentUser.Id);

            return usuarioPermissoes.Lojas;
        }

        /// <summary>
        /// Realiza a alteração da loja selecionada pelo usuário corrente.
        /// </summary>
        /// <param name="idLoja">O id da nova loja selecionada.</param>
        /// <returns>O novo token com a loja selecionada.</returns>
        [HttpPut]
        [Route("Auth/Users/Current/Stores/{idLoja}/selected")]
        public string AlterarLoja(int idLoja)
        {
            var currentUser = RuntimeContext.Current.User;
            var usuarioPermissoes = m_permissaoService.ObterPermissoesDoUsuario(currentUser.Id);                
            var novaLojaSelecionada = usuarioPermissoes.Lojas.FirstOrDefault(l => l.IDLoja == idLoja);
            
            if (novaLojaSelecionada == null)
            {
                throw new UserInvalidOperationException(Texts.YouHaveNoAccessToStore);
            }

            currentUser.StoreId = idLoja;

            return TokenService.CreateToken(currentUser);            
        }

        private LoginResponse LogarSemPapel(LoginRequest authInfo)
        {/*
            var settings = Settings.Default;
            IEnumerable<Papel> papeis;

            try
            {
                papeis = m_ssoService.ObterPapeis(authInfo.UserName, authInfo.Password, settings.SsoApplicationCode);
            }
            catch (SsoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(Texts.UnableToLogin, ex);
            }

            // Importa o usuário ou atualiza os dados do usuário.
            ImportSsoData(authInfo, settings);

            var user = MainService.ObterPorUserName(authInfo.UserName);

            // TODO: podemos remover aqui? ja que TipoPermissao é relevante apenas no LoginComPapel, e Lojas não é preenchido lá
            var usuarioPermissoes = m_permissaoService.ObterPermissoesDoUsuario(user.Id);

            var userModel = new UserModel
            {
                Nome = authInfo.UserName,
                Dispensa = authInfo.
                //Papeis = papeis,
                //Lojas = usuarioPermissoes.TipoPermissao == TipoPermissao.PorLoja ? usuarioPermissoes.Lojas : null,
                //HasPermissions = false,
                //TipoPermissao = usuarioPermissoes.TipoPermissao,
                Culture = RuntimeContext.Current.Culture
            };

            Commit();

            return new LoginResponse
            {
                Token = CreateToken(userModel),
                User = userModel
            };
            */
            return null;
        }

        private LoginResponse LogarComPapel(LoginRequest authInfo)
        {/*
            var settings = Settings.Default;

            try
            {
                var user = MainService.ObterPorUserName(authInfo.UserName);
                var papelExterno = m_ssoService.ObterPapel(authInfo.IdExternoPapel.Value);
                var papel = m_papelService.ObterPorNome(papelExterno.Name);

                // Importa o papel, caso ainda não exista no SGP.
                if (papel == null)
                {
                    ImportSsoData(authInfo, settings);
                    papel = m_papelService.ObterPorNome(papelExterno.Name);
                }

                var papelInfo = m_ssoService.ObterInformacoesDoPapel(authInfo.UserName, authInfo.Password, authInfo.IdExternoPapel.Value);
                var menus = new List<UserMenuInfo>(papelInfo.GrantedMenus);
                menus.Add(new UserMenuInfo("/login"));

                var usuarioPermissoes = m_permissaoService.ObterPermissoesDoUsuario(user.Id);

                var vm = new UserModel
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Id = user.Id,
                    UserName = user.UserName,
                    //HasPermissions = true,
                    Menus = menus,
                    Actions = papelInfo.GrantedActions,
                   // Role = papel,
                  //  IdLoja = authInfo.IdLoja,
                   // BandeiraId = authInfo.IdLoja.HasValue ? m_lojaService.ObterPorId(authInfo.IdLoja.Value).IDBandeira : 0,
                  //  Lojas = usuarioPermissoes.TipoPermissao == TipoPermissao.PorLoja ? usuarioPermissoes.Lojas : null,
                    Culture = RuntimeContext.Current.Culture,
                  //  CdLoja = authInfo.IdLoja.HasValue ? new int?(m_lojaService.ObterPorId(authInfo.IdLoja.Value).cdLoja) : null,
                  //  TipoPermissao = usuarioPermissoes.TipoPermissao
                };

                Commit();

                return new LoginResponse
                {
                    Token = CreateToken(vm),
                    User = vm
                };
            }
            catch (SsoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(Texts.UnableToLogin, ex);
            }
            */
            return null;
        }

        private void ImportSsoData(LoginRequest authInfo, Settings settings)
        {
            var options = new SsoOptions
            {
                ApplicationCode = settings.SsoApplicationCode,
                EmailDomain = settings.EmailDomain,
                UserName = authInfo.UserName,
                UserPassword = authInfo.Password,
                ProfileCode = authInfo.IdExternoPapel
            };

            var translator = new SsoUsuarioDataTranslator(m_ssoService, options);
            UsuarioAcessoInfo acessoInfo;

            try
            {
                acessoInfo = translator.Translate();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(Texts.UnableToLogin, ex);
            }

            // Importa as informações de acesso do usuário (usuário e papel) para dentro da base do SGP.
            var usuarioImporter = new UsuarioImporter(MainService, m_papelService);
            usuarioImporter.Import(acessoInfo);
        }

        private string CreateToken(UserModel user)
        {
            var runtimeUser = new MemoryRuntimeUser
            {
                Id = user.Id,
                UserName = user.Nome,
                Email = user.Email,                        
                Actions = user.Actions
              //  StoreId = user.IdLoja,
              //  TipoPermissao = user.TipoPermissao,
              //  HasAccessToSingleStore = user.HasAccessToSingleStore
            };

            var papel = user.Role;

            if (papel != null)
            {
                runtimeUser.RoleId = papel.Id;
                runtimeUser.RoleName = papel.Name;
                runtimeUser.IsAdministrator = papel.IsAdmin.GetValueOrDefault();
                runtimeUser.IsGa = papel.IsGa.GetValueOrDefault();
                runtimeUser.IsHo = papel.IsHo.GetValueOrDefault();
            }

            return TokenService.CreateToken(runtimeUser);
        }
        #endregion
    }
}