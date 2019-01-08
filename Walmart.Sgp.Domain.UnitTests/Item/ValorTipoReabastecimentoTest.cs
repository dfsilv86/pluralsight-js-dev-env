using NUnit.Framework;
using System;
using Walmart.Sgp.Domain.Item;
namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    [Category("Item")]
    public class ValorTipoReabastecimentoTest
    {
        [Test]
        public void OperadorImplicito_NullableInt16_TipoReabastecimento()
        {
            var expected = ValorTipoReabastecimento.CrossDocking3;
            short? target = 3;
            var actual = (ValorTipoReabastecimento)target;
            Assert.AreEqual(expected, actual);            
        }

        [Test]
        public void OperadorImplicito_Int16_TipoReabastecimento()
        {
            var expected = ValorTipoReabastecimento.CrossDocking3;
            short target = 3;
            var actual = (ValorTipoReabastecimento)target;
            Assert.AreEqual(expected, actual);
        }

        [Test]        
        public void OperadorImplicito_InvalidNullableInt16_InvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() =>
            {
                short? target = -3;
                var actual = (ValorTipoReabastecimento)target;
            });            
        }

        [Test]
        public void OperadorExplicito_TipoReabastecimento_Int16()
        {
            short? expected = 3;
            var target = ValorTipoReabastecimento.CrossDocking3;
            var actual = (short?)target;
            Assert.AreEqual(expected, actual);
        }
    }
}
