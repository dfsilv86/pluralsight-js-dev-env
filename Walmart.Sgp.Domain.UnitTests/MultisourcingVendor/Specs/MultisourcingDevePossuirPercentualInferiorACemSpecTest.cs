using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingDevePossuirPercentualInferiorACemSpecTest
    {
        [Test]
        public void IsSatisfiedBy_PercentualIgual100_False()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    vlPercentual = 100
                }
            };

            var target = new MultisourcingDevePossuirPercentualInferiorACemSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_PercentualDiferente100_True()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    vlPercentual = 25
                }
            };

            var target = new MultisourcingDevePossuirPercentualInferiorACemSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsTrue(actual.Satisfied);
        }
    }
}