using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class ListaDeItensNaoPodeTerItemCemPorcentoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItensDetalhePercentualMenorQueCem_True()
        {
            var target = new ListaDeItensNaoPodeTerItemCemPorcentoSpec();
            var actual = target.IsSatisfiedBy(new ItemDetalheCD[] { new ItemDetalheCD() { vlPercentual = 50 }, new ItemDetalheCD() { vlPercentual = 50 } });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItensDetalhePercentualIgualACem_False()
        {
            var target = new ListaDeItensNaoPodeTerItemCemPorcentoSpec();
            var actual = target.IsSatisfiedBy(new ItemDetalheCD[] { new ItemDetalheCD() { vlPercentual = 100 }, new ItemDetalheCD() { vlPercentual = 0 } });

            Assert.AreEqual(Texts.NotAllowedToHaveOneItemHundredPercent, actual.Reason);
        }
    }
}
