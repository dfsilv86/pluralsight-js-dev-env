using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using NUnit.Framework;
using Walmart.Sgp.CodeQuality.Validators;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;
using Walmart.Sgp.Infrastructure.Web.Security;
using Walmart.Sgp.WebApi.Controllers;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests.Importing.WebGuardian
{
    [TestFixture]
    public class WebGuardianServiceTest
    {
        #region Fields
        private const string Username = "wmedeir";
        private const string Password = "Walmart2";        
        private int m_codigoApp;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_codigoApp = Walmart.Sgp.WebApi.Properties.Settings.Default.SsoApplicationCode;
        }
        #endregion

        [Test]
        [Category("WebGuardian")]
        public void ObterAcoesSegurasDaAplicacao_UsuarioValido_Acoes()
        {
            var target = new WebGuardianSsoService(null);
            var actual = target.ObterAcoesSegurasDaAplicacao(Username, Password, m_codigoApp);
            Assert.AreNotEqual(0, actual.Count());
            Assert.IsTrue(actual.Any(a => a.Id.Equals("Inventario.ImportarAutomaticoLoja")));
            Assert.IsTrue(actual.Any(a => a.Id.Equals("Inventario.RemoverAgendamentos")));            
        }

        [Test]
        [Category("WebGuardian")]
        public void ObterInformacoesDoPapel_UsuarioValido_MenusEAcoes()
        {            
            var target = new WebGuardianSsoService(null);
            var papel = target.ObterPapeis(Username, Password, m_codigoApp).FirstOrDefault(f => f.Name.Equals("SGP-Administrador"));
            var actual = target.ObterInformacoesDoPapel(Username, Password, papel.IdExterno);
            Assert.AreNotEqual(0, actual.GrantedMenus.Count());
            Assert.IsTrue(actual.GrantedMenus.Any(a => a.Route.Equals("/item/relacionamento/receituario")));
            Assert.IsTrue(actual.GrantedMenus.Any(a => a.Route.Equals("/estoque/extrato")));

            Assert.AreNotEqual(0, actual.GrantedActions.Count());            
            Assert.IsTrue(actual.GrantedActions.Any(a => a.Id.Equals("Inventario.ImportarAutomaticoLoja")));
        }

        [Test]
        [Category("WebGuardian")]
        public void ObterUsuarios_UsuarioValido_Usuarios()
        {
            var target = new WebGuardianSsoService(null);
            var actual = target.ObterUsuarios(Username, Password, m_codigoApp).ToArray();
            Assert.AreNotEqual(0, actual.Length);            
        }

        [Test]
        [Category("WebGuardian")]
        public void ObterAcoesSegurasDaAplicacao_ActionsDeEscrita_PossuemEventosNoWebGuardian()
        {            
            var target = new WebGuardianSsoService(null);
            var actual = target.ObterAcoesSegurasDaAplicacao(Username, Password, m_codigoApp).Select(a => a.Id);

            // Obtém todas as actions de escrita da web api
            var controllers = typeof(AuthController).Assembly.GetTypes().Where(t => t.Name.EndsWith("Controller") && !t.IsDefined(typeof(AllowAnonymousAttribute), true));

            var actions = controllers
                .SelectMany(t => t.GetMethods().Where(m =>
                    (m.IsDefined(typeof(HttpPostAttribute), true)
                    || m.IsDefined(typeof(HttpPutAttribute), true)
                    || m.IsDefined(typeof(HttpDeleteAttribute), true)
                    || m.IsDefined(typeof(SecurityWebApiActionAttribute), true))
                    && !m.IsDefined(typeof(AllowAnonymousAttribute), true)))
                    .Select(m => new { Action = m, Attribute = m.GetCustomAttributes(typeof(SecurityWebApiActionAttribute), true).FirstOrDefault() as SecurityWebApiActionAttribute })
                    .Where(m => m.Attribute == null || !m.Attribute.AllowWriteActionWithoutPermission)
                    .Select(m => m.Attribute == null
                        ? "{0}.{1}".With(m.Action.DeclaringType.Name.Replace("Controller", ""), m.Action.Name)
                        : m.Attribute.ActionId)
                .OrderBy(a => a)
                .ToArray();

            var eventosFaltantesNoWebGuardian = actions.Except(actual).ToList();

            // Verifica no html pelo uso da security-action.
            Validator.ForEachHtmlFile((fileName, dom) =>
            {
                var securityActions = dom["[security-action]"];

                foreach (var sa in securityActions)
                {
                    var actionId = sa.Attributes["security-action"];

                    if (!actual.Contains(actionId) && !eventosFaltantesNoWebGuardian.Contains(actionId))
                    {
                        eventosFaltantesNoWebGuardian.Add(actionId);
                    }
                }
            });

            if(eventosFaltantesNoWebGuardian.Count > 0)
            {
                var msg = new StringBuilder();
                msg.Append("Existem ações de escrita na web api que não possuem um evento criado no WebGuardian. Execute o insert abaixo no banco do WebGuardian para inserí-los. Após associe ao perfil desejado (diretamente pelo interface do WebGuardian):");
                msg.AppendLine().AppendLine();
                msg.AppendFormat("DECLARE @CODAPP INT = {0} -- Altere o Codigo_Aplicacao caso esteja em outro ambiente.", m_codigoApp);
                msg.AppendLine();
                msg.Append("DECLARE @CODFORM INT = (SELECT Codigo_Formulario FROM  [usr_adminWGN].[WGN_Formulario] WHERE Codigo_Aplicacao = @CODAPP AND Nome = 'Geral') -- Codigo_Formulario do formulário 'Geral'.");

                msg.AppendLine();
                msg.Append("DECLARE @CODUSER INT = 1 -- Codigo_usuario que está criando os eventos.");                                
                msg.AppendLine().AppendLine();
                msg.Append("BEGIN TRANSACTION");

                foreach (var e in eventosFaltantesNoWebGuardian)
                {
                    msg.AppendLine();
                    msg.AppendFormat("INSERT [usr_adminWGN].[WGN_Evento] ([Codigo_Formulario], [Codigo_Aplicacao], [Nome], [Descricao], [Icone], [Url], [Image], [Target], [Status], [Dt_Alteracao], [Usuario_Alteracao], [Dt_Inclusao], [Usuario_Inclusao]) VALUES (@CODFORM, @CODAPP, '{0}', '{1}', NULL, NULL, NULL, NULL, 'A', NULL, NULL, GETDATE(), @CODUSER)", e, GetWebGuardianEventName(e));
                }

                msg.AppendLine();
                msg.Append("COMMIT");

                Assert.Fail(msg.ToString());
            }
            
        }

        private string GetWebGuardianEventName(string e)
        {
            return e.Length > 30 ? e.Substring(0, 30) : e;
        }
    }
}
