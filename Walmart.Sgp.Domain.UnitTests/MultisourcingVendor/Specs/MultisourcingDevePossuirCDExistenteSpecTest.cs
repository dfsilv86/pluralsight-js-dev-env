using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingDevePossuirCDExistenteSpecTest
    {
        [Test]
        public void IsSatisfiedBy_CDExiste_True()
        {
            var multisourcing = new Multisourcing
            {
                CD = 100
            };

            var target = new MultisourcingDevePossuirCDExistenteSpec(cd => 1);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_CDNaoExiste_False()
        {
            var multisourcing = new Multisourcing
            {
                CD = 100
            };

            var target = new MultisourcingDevePossuirCDExistenteSpec(cd => 0);
            var actual = target.IsSatisfiedBy(new [] { multisourcing });

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
