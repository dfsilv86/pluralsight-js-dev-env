using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios
{
    [TestFixture]
    [Category("Domain")]
    [Category("Inventarios")]
    public class InventarioStatusTest
    {
        [Test]
        public void ConverterInteiro_Instancia_Inteiro()
        {
            var target = InventarioStatus.Importado;
            var actual = (int?)target;
            var expected = new int?(1);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConverterInteiro_Nulo_Nulo()
        {
            InventarioStatus target = null;
            var actual = (int?)target;
            int? expected = null;
            Assert.AreEqual(expected, actual);
        }
    }
}
