using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    public class ReturnSheetItemLojaEqualityComparerTest
    {
        [Test]
        public void Equals_ObjetosIguais_True() 
        {
            var obj1 = new ReturnSheetItemLoja
            {
                IdItemDetalhe = 1,
                IdLoja = 2
            };

            var obj2 = new ReturnSheetItemLoja
            {
                IdItemDetalheEntrada = 1,
                IdLoja = 2
            };

            var target = new ReturnSheetItemLojaEqualityComparer();
            var actual = target.Equals(obj1, obj2);

            Assert.IsTrue(actual);
        }

        [Test]
        public void Equals_ObjetosDiferentes_False()
        {
            var obj1 = new ReturnSheetItemLoja
            {
                IdItemDetalhe = 1,
                IdLoja = 2
            };

            var obj2 = new ReturnSheetItemLoja
            {
                IdItemDetalheEntrada = 2,
                IdLoja = 2
            };

            var target = new ReturnSheetItemLojaEqualityComparer();
            var actual = target.Equals(obj1, obj2);

            Assert.IsFalse(actual);
        }
    }
}
