using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;
namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class FornecedorParametroReviewDateTest
    {
        [Test]
        public void TipoIntervalo_ValorDiferenteSemanal_Quinzenal()
        {
            var target = new FornecedorParametroReviewDate();
            target.tpInterval = TipoIntervalo.NaoDefinido;
            var expected = TipoIntervalo.Quinzenal;
            Assert.AreEqual(expected, target.tpInterval);
        }

        [Test]
        public void TipoIntervalo_ValorNulo_Quinzenal()
        {
            var target = new FornecedorParametroReviewDate();
            target.tpInterval = null;
            var expected = TipoIntervalo.Quinzenal;
            Assert.AreEqual(expected, target.tpInterval);
        }

        [Test]
        public void TipoIntervalo_ValorSemanal_Semanal()
        {
            var target = new FornecedorParametroReviewDate();
            target.tpInterval = TipoIntervalo.Semanal;
            var expected = TipoIntervalo.Semanal;
            Assert.AreEqual(expected, target.tpInterval);
        }
    }
}
