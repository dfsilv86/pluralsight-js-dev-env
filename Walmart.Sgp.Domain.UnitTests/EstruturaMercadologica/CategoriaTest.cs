using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class CategoriaTest
    {
        [Test]
        public void Descricao_ILojaSecao_dsCategoria()
        {
            var target = new Categoria { cdCategoria = 1, dsCategoria = "TESTE" };
            Assert.AreEqual(1, target.Codigo);
            Assert.AreEqual("TESTE", target.Descricao);
        }
    }
}
