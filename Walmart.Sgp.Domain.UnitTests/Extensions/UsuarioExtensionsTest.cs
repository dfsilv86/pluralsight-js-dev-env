using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;

namespace Walmart.Sgp.Domain.UnitTests.Extensions
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioExtensionsTest
    {
        [Test]
        public void UsuarioExtensionsRemoverSenha_Usuario_Usuario()
        {
            Usuario user = new Usuario() { Passwd = "Foo" };

            Usuario result = user.RemoverSenha();

            Assert.IsNull(result.Passwd);
        }

        [Test]
        public void UsuarioExtensionsRemoverSenha_Usuarios_Usuarios()
        {
            Usuario[] users = new Usuario[] { new Usuario() { Passwd = "Foo" }, new Usuario() { Passwd = "Bar" } };

            Usuario[] results = users.RemoverSenha().ToArray();

            Assert.IsNull(results[0].Passwd);
            Assert.IsNull(results[1].Passwd);
        }

        [Test]
        public void UsuarioExtensionsRemoverSenha_UsuarioNull_Null()
        {
            Usuario user = null;

            Usuario result = user.RemoverSenha();

            Assert.IsNull(result);
        }

        [Test]
        public void UsuarioExtensionsRemoverSenha_UsuariosNull_Null()
        {
            Usuario[] users = null;

            var results = users.RemoverSenha();

            Assert.IsNull(results);
        }

        [Test]
        public void UsuarioExtensionsResumir_Usuario_UsuarioResumo()
        {
            Usuario user = new Usuario() { FullName = "Foo", UserName = "Bar", Id = 3, Passwd = "FooBar" };

            var result = user.Resumir();

            Assert.AreEqual(3, result.Id);
            Assert.AreEqual("Foo", result.FullName);
            Assert.AreEqual("Bar", result.UserName);
        }

        [Test]
        public void UsuarioExtensionsResumir_Null_Null()
        {
            Usuario user = null;

            var result = user.Resumir();

            Assert.IsNull(result);
        }
    }
}
