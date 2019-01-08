using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingPercentualDevePossuirValorSpecTest
    {
        [Test]
        public void IsSatisfiedBy_PercentualPreenchido_True()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    vlPercentual = 100
                }
            };

            var target = new MultisourcingPercentualDevePossuirValorSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_PercentualVazio_False()
        {
            var multisoucings = new[]
            { 
                new Multisourcing()
            };

            var target = new MultisourcingPercentualDevePossuirValorSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}