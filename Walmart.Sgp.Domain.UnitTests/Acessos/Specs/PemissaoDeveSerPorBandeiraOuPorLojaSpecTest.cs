using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class PemissaoDeveSerPorBandeiraOuPorLojaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SemPermissaoBandeiraOuLoja_False()
        {
            var target = new PemissaoDeveSerPorBandeiraOuPorLojaSpec();
            var permissao = new Permissao();
            var actual = target.IsSatisfiedBy(permissao);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(actual.Reason, Texts.InformChainOrStore);

        }

        [Test]
        public void IsSatisfiedBy_ComPermissaoBandeiraELoja_False()
        {
            var target = new PemissaoDeveSerPorBandeiraOuPorLojaSpec();
            var permissao = new Permissao
            {
                Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira() },
                Lojas = new PermissaoLoja[] { new PermissaoLoja() }
            };

            var actual = target.IsSatisfiedBy(permissao);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(actual.Reason, Texts.PermissionShouldHaveChainOrStore);
        }

        [Test]
        public void IsSatisfiedBy_ComPermissaoBandeiraESemPermissaoLoja_True()
        {
            var target = new PemissaoDeveSerPorBandeiraOuPorLojaSpec();
            var permissao = new Permissao
            {
                Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira() }                
            };

            var actual = target.IsSatisfiedBy(permissao);
            Assert.IsTrue(actual.Satisfied);            
        }

        [Test]
        public void IsSatisfiedBy_SemPermissaoBandeiraEComPermissaoLoja_True()
        {
            var target = new PemissaoDeveSerPorBandeiraOuPorLojaSpec();
            var permissao = new Permissao
            {                
                Lojas = new PermissaoLoja[] { new PermissaoLoja() }
            };

            var actual = target.IsSatisfiedBy(permissao);
            Assert.IsTrue(actual.Satisfied); 
        }
    }
}
