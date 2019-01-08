using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class DepartamentoDeveSerPerecivelSpecTest
    {
        [Test]
        public void DepartamentoDeveSerPerecivelSpec_IsSatisfiedBy_Satisfied()
        {
            var depto = new Departamento { blPerecivel = "S" };

            var target = new DepartamentoDeveSerPerecivelSpec("teste.txt");

            Assert.IsTrue(target.IsSatisfiedBy(depto));
        }

        [Test]
        public void DepartamentoDeveSerPerecivelSpec_IsSatisfiedBy_NotSatisfied()
        {
            var depto = new Departamento();

            var target = new DepartamentoDeveSerPerecivelSpec("teste.txt");

            var result = target.IsSatisfiedBy(depto);

            Assert.IsFalse(result.Satisfied);

            // Legado - InventarioImportacao.cs linha 1166
            Assert.AreEqual("O departamento {0} do arquivo {1} não é um departamento perecível.".With(depto.cdDepartamento, "teste.txt"), result.Reason);
        }
    }
}
