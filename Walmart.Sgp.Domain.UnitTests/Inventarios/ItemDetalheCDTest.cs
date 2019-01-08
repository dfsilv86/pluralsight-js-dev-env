using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios
{
    [TestFixture]
    [Category("Domain"), Category("Inventarios")]
    public class ItemDetalheCDTest
    {
        [Test]
        public void Multivendor_QuantidadeItens_PossuiMultivendor()
        {
            var target = new ItemDetalheCD { QtdItensEntrada = 3 };
            Assert.IsTrue(target.Multivendor);
        }


        [Test]
        public void PossuiCadastro_QuantidadeMultisourcing_PossuiCadastro()
        {
            var target = new ItemDetalheCD { QtdMultisourcing = 2 };
            Assert.IsTrue(target.PossuiCadastro);
        }
    }
}
