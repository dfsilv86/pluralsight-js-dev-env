using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Movimentacao.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Domain.Gerenciamento.Specs;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class RegiaoDeveTerDistritosSpecTest 
    {
        [Test]
        public void IsSatisfiedBy_SemDistritos_False()
        {
            var target = new RegiaoDeveTerDistritosSpec();
            var actual = target.IsSatisfiedBy(new Regiao());
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.RegionMustHaveDistricts, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ComDistritos_True()
        {
            var target = new RegiaoDeveTerDistritosSpec();
            var actual = target.IsSatisfiedBy(new Regiao { Distritos = new Distrito[] { new Distrito() } });
            Assert.IsTrue(actual.Satisfied);            
        }       
    }
}
