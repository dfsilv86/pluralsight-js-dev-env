using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Inventarios")]
    public class ListaDeItensDevePossuirPercentualCompletoSpecTest
    {
        [Test]
        public void ListaDeItensDevePossuirPercentualCompletoSpec_ListaCompleta_True()
        {
            var lista = new ItemDetalheCD[] { new ItemDetalheCD { vlPercentual = 49 }, new ItemDetalheCD { vlPercentual = 51 } };

            var target = new ListaDeItensDevePossuirPercentualCompletoSpec();

            Assert.IsTrue(target.IsSatisfiedBy(lista).Satisfied);
        }

        [Test]
        public void ListaDeItensDevePossuirPercentualCompletoSpec_ListaIncompleta_False()
        {
            var lista = new ItemDetalheCD[] { new ItemDetalheCD { vlPercentual = 49 }, new ItemDetalheCD { vlPercentual = 49 } };

            var target = new ListaDeItensDevePossuirPercentualCompletoSpec();

            Assert.IsFalse(target.IsSatisfiedBy(lista).Satisfied);
        }
    }
}
