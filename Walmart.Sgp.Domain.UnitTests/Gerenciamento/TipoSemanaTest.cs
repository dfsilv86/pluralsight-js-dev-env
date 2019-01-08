using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]    
    public class TipoSemanaTest
    {
        [Test]
        public void OperadorImplicito_NullableInt16_TipoSemana()
        {
            var expected = TipoSemana.Impar;
            short? target = 1;
            var actual = (TipoSemana)target;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OperadorImplicito_Int16_TipoSemana()
        {
            var expected = TipoSemana.Impar;
            short target = 1;
            var actual = (TipoSemana)target;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OperadorImplicito_InvalidNullableInt16_InvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() =>
            {
                short? target = -1;
                var actual = (TipoSemana)target;
            });
        }

        [Test]
        public void OperadorExplicito_TipoSemana_Int16()
        {
            short? expected = 1;
            var target = TipoSemana.Impar;
            var actual = (short?)target;
            Assert.AreEqual(expected, actual);
        }
    }
}
