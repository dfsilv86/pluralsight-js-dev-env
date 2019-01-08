using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class RelacaoItemLojaCDTest
    {
        [Test]
        public void UmaOuMaisNotSatisfiedSpecReasons_IsValid_False()
        {
            var model = new Walmart.Sgp.Domain.Reabastecimento.RelacaoItemLojaCDVinculo();
            model.NotSatisfiedSpecReasons = new List<string>()
            {
                "Error X"
            };

            Assert.IsFalse(model.IsValid);
        }

        [Test]
        public void UmaOuMaisNotSatisfiedSpecReasons_IsValid_True()
        {
            var model = new Walmart.Sgp.Domain.Reabastecimento.RelacaoItemLojaCDVinculo();
            model.NotSatisfiedSpecReasons = new List<string>();

            Assert.IsTrue(model.IsValid);
        }
    }
}
