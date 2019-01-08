using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class VendorRoteiroDeveConterItensDSDSpecTest
    {
        [Test]
        public void IsSatisfiedBy_VendorNaoContemItens_NotSatisfied()
        {
            Func<long, bool> func = (l) => false;

            var target = new VendorRoteiroDeveConterItensDSDSpec(func);

            var roteiro = new Roteiro();

            var result = target.IsSatisfiedBy(roteiro);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsSatisfiedBy_VendorContemItens_Satisfied()
        {
            Func<long, bool> func = (l) => true;

            var target = new VendorRoteiroDeveConterItensDSDSpec(func);

            var roteiro = new Roteiro();

            var result = target.IsSatisfiedBy(roteiro);

            Assert.IsTrue(result);
        }
    }
}