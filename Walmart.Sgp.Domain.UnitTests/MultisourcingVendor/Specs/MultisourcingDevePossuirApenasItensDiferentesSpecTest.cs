using NUnit.Framework;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingDevePossuirApenasItensDiferentesSpecTest
    {
        [Test]
        public void IsSatisfiedBy_NaoDuplicado_True()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    CdItemDetalheSaida = 9568378,
                    CdItemDetalheEntrada = 9549996,
                    vlPercentual = 90
                },

                new Multisourcing
                {
                    CdItemDetalheSaida = 8050800,
                    CdItemDetalheEntrada = 8050667,
                    vlPercentual = 80
                },
            };

            var target = new MultisourcingDevePossuirApenasItensDiferentesSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_Duplicado_False()
        {
            var multisoucings = new[]
            { 
                new Multisourcing
                {
                    CdItemDetalheSaida = 9568378,
                    CdItemDetalheEntrada = 9549996,
                    vlPercentual = 90
                },

                new Multisourcing
                {
                    CdItemDetalheSaida = 9568378,
                    CdItemDetalheEntrada = 9549996,
                    vlPercentual = 80
                },
            };

            var target = new MultisourcingDevePossuirApenasItensDiferentesSpec();
            var actual = target.IsSatisfiedBy(multisoucings);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}