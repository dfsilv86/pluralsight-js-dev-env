using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]    
    public class TipoIntervaloTest
    {
        [Test]
        public void OperadorImplicito_NullableInt16_TipoIntervalo()
        {
            var expected = TipoIntervalo.Semanal;
            short? target = 1;
            var actual = (TipoIntervalo)target;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OperadorImplicito_Int16_TipoIntervalo()
        {
            var expected = TipoIntervalo.Semanal;
            short target = 1;
            var actual = (TipoIntervalo)target;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OperadorImplicito_InvalidNullableInt16_InvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() =>
            {
                short? target = -1;
                var actual = (TipoIntervalo)target;
            });
        }

        [Test]
        public void OperadorExplicito_TipoIntervalo_Int16()
        {
            short? expected = 1;
            var target = TipoIntervalo.Semanal;
            var actual = (short?)target;
            Assert.AreEqual(expected, actual);
        }
    }
}
