using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    [Category("Acessos")]
    public class UsuarioPermissoesTest
    {
        [Test]
        public void Constructor_SemBandeiraESemLoja_Exception()
        {
            Assert.Catch(() =>
            {
                new UsuarioPermissoes(
                    1,
                    new Bandeira[0],
                    new Loja[0]);
            }, Texts.PermissionsDoNotDefinedForUser);
        }

        [Test]
        public void Constructor_ComBandeiraESemLoja_TipoPermissaoPorBandeira()
        {
            var target = new UsuarioPermissoes(
                    11,
                    new Bandeira[] { new Bandeira() },
                    new Loja[0]);

            Assert.AreEqual(1, target.Bandeiras.Count());
            Assert.AreEqual(0, target.Lojas.Count());
            Assert.AreEqual(11, target.IDUsuario);
            Assert.AreEqual(TipoPermissao.PorBandeira, target.TipoPermissao);
        }

        [Test]
        public void Constructor_ComBandeiraEComLoja_TipoPermissaoPorLoja()
        {
            var target = new UsuarioPermissoes(
                    11,
                    new Bandeira[] { new Bandeira() },
                    new Loja[] { new Loja() });

            Assert.AreEqual(1, target.Bandeiras.Count());
            Assert.AreEqual(1, target.Lojas.Count());
            Assert.AreEqual(11, target.IDUsuario);
            Assert.AreEqual(TipoPermissao.PorLoja, target.TipoPermissao);
        }
    }
}
