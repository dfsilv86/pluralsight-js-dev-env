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
    public class DepartamentoTest
    {
        [Test]
        public void Descricao_ILojaSecao_dsDepartamento()
        {
            var target = new Departamento { cdDepartamento = 1, dsDepartamento = "TESTE" };
            Assert.AreEqual(1, target.Codigo);
            Assert.AreEqual("TESTE", target.Descricao);
        }
    }
}
