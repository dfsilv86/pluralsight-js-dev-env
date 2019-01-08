using NUnit.Framework;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class PercentualItemSaidaCDSpecTest
    {
        [Test]
        public void IsSatisfiedBy_PercentualMenorQue100_False()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    CdItemDetalheSaida = 9845123,
                    CD = 7400,
                    vlPercentual = 25
                },

                new Multisourcing 
                {
                    CdItemDetalheSaida = 9845123,
                    CD = 7400,
                    vlPercentual = 25
                }
            };

            var target = new PercentualItemSaidaCDSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_PercentualMaiorQue100_False()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    CdItemDetalheSaida = 9845123,
                    CD = 7400,
                    vlPercentual = 25
                },

                new Multisourcing 
                {
                    CdItemDetalheSaida = 9845123,
                    CD = 7400,
                    vlPercentual = 100
                }
            };

            var target = new PercentualItemSaidaCDSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_PercentualIgualA100_True()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    CdItemDetalheSaida = 9845123,
                    CD = 7400,
                    vlPercentual = 50
                },

                new Multisourcing 
                {
                    CdItemDetalheSaida = 9845123,
                    CD = 7400,
                    vlPercentual = 50
                }
            };

            var target = new PercentualItemSaidaCDSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsTrue(actual.Satisfied);
        }
    }
}