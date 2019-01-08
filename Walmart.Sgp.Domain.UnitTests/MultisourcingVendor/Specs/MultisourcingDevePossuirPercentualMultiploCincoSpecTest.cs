using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingDevePossuirPercentualMultiploCincoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_PercentualMultiploCinco_True()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    vlPercentual = 100
                }
            };

            var target = new MultisourcingDevePossuirPercentualMultiploCincoSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_PercentualNaoMultiploCinco_False()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    vlPercentual = 11
                }
            };

            var target = new MultisourcingDevePossuirPercentualMultiploCincoSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}