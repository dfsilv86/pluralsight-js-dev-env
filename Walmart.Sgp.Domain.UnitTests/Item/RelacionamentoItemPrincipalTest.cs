using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    public class RelacionamentoItemPrincipalTest
    {
        [Test]
        public void EhEntrada_Manipulado_True()
        {
            var target = new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado };
            Assert.IsTrue(target.EhEntrada());
            Assert.IsFalse(target.EhSaida());
        }

        [Test]
        public void EhEntrada_Receituario_False()
        {
            var target = new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario };
            Assert.IsFalse(target.EhEntrada());
        }

        [Test]
        public void EhEntrada_Vinculado_False()
        {
            var target = new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado };
            Assert.IsFalse(target.EhEntrada());
        }

    }
}
