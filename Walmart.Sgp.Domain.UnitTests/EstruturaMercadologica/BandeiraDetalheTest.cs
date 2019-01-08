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
    public class BandeiraDetalheTest
    {
        [Test]
        public void Secao_DepartamentoOuCategoria_Secao()
        {
            var target = new BandeiraDetalhe();
            Assert.IsNull(target.Secao);

            target.Categoria = new Categoria();
            Assert.AreEqual(target.Categoria, target.Secao);

            target.Departamento = new Departamento();
            Assert.AreEqual(target.Departamento, target.Secao);
        }
    }
}
