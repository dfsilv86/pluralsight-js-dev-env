using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.MultisourcingVendor;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingTest
    {
        [Test]
        public void IsValid_NenhumMotivoParaNaoSerValido_EValido()
        {
            var target = new Multisourcing { NotSatisfiedSpecReasons = new List<string>() };
            Assert.IsTrue(target.IsValid);
        }

        [Test]
        public void IsValid_MotivosParaNaoSerValido_NaoEValido()
        {
            var motivos = new List<string>() { "Teste" };

            var target = new Multisourcing { NotSatisfiedSpecReasons = motivos };

            Assert.IsFalse(target.IsValid);
        }
    }
}
