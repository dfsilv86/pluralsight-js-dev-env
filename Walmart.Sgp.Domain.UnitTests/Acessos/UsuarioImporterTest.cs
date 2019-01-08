using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioImporterTest
    {
        [Test]
        public void Import_InvalidInput_Exception()
        {
            var target = new UsuarioImporter(null, null);
            var input = new UsuarioAcessoInfo();

            Assert.Throws<ArgumentNullException>(() =>
            {
                target.Import(input);
            }, Texts.UserIsRequired);     
        }       

        [Test]
        public void Import_UsuarioEPapelNaoExistem_Criados()
        {
            var input = new UsuarioAcessoInfo()
            {
                Usuario = new Usuario() { UserName = "novoUser" },
                Papel = new Papel() { Name = "novoRole" },
            };

            var usuarioService = MockRepository.GenerateMock<IUsuarioService>();
            usuarioService.Expect(u => u.ObterPorUserName("novoUser")).Return(null);
            usuarioService.Expect(u => u.Salvar(input.Usuario));

            var papelService = MockRepository.GenerateMock<IPapelService>();
            papelService.Expect(u => u.ObterPorNome("novoRole")).Return(null);
            papelService.Expect(u => u.Salvar(input.Papel));
            
            var target = new UsuarioImporter(usuarioService, papelService);
            target.Import(input);

            usuarioService.VerifyAllExpectations();
            papelService.VerifyAllExpectations();
        }

        [Test]
        public void Import_UsuarioEPapelExistem_ApenasUsuarioAtualizado()
        {
            var input = new UsuarioAcessoInfo()
            {
                Usuario = new Usuario() { UserName = "usuarioExistente", FullName = "novo nome" },
                Papel = new Papel() { Name = "papelExistente", Description = "nova descrição" },
            };

            var usuarioService = MockRepository.GenerateMock<IUsuarioService>();

            var usuarioExistente = new Usuario
            {
                Id =  1,
                UserName = "usuarioExistente",
                FullName = "Nome antigo"
            };
            usuarioService.Expect(u => u.ObterPorUserName("usuarioExistente")).Return(usuarioExistente);
            usuarioService.Expect(u => u.Salvar(input.Usuario)).WhenCalled((m) =>
            {
                var usuario = m.Arguments[0] as Usuario;

                Assert.AreEqual("novo nome", usuario.FullName);
            });

            var papelExistente = new Papel
            {
                Id = 1,
                Name = "papelExistente",
                Description = "Descrição antiga"
            };
            var papelService = MockRepository.GenerateMock<IPapelService>();
            papelService.Expect(u => u.ObterPorNome("papelExistente")).Return(papelExistente);         

            var target = new UsuarioImporter(usuarioService, papelService);
            target.Import(input);

            usuarioService.VerifyAllExpectations();
            papelService.VerifyAllExpectations();
        }
    }
}
