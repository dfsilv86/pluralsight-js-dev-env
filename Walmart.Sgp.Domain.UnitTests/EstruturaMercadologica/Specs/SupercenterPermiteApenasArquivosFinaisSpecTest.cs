using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.EstruturaMercadologica.Specs;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class SupercenterPermiteApenasArquivosFinaisSpecTest
    {
        [Test]
        public void SupercenterPermiteApenasArquivosFinaisSpec_SupercenterFinal_Satisfied()
        {
            var loja = new Loja { cdSistema = 1, TipoArquivoInventario = TipoArquivoInventario.Final };

            var target = new SupercenterPermiteApenasArquivosFinaisSpec();

            Assert.IsTrue(target.IsSatisfiedBy(loja));
        }

        [Test]
        public void SupercenterPermiteApenasArquivosFinaisSpec_SupercenterParcial_NotSatisfied()
        {
            var loja = new Loja { cdSistema = 1, TipoArquivoInventario = TipoArquivoInventario.Parcial };

            var target = new SupercenterPermiteApenasArquivosFinaisSpec();

            Assert.IsFalse(target.IsSatisfiedBy(loja));
        }

        [Test]
        public void SupercenterPermiteApenasArquivosFinaisSpec_AtacadoFinal_Satisfied()
        {
            var loja = new Loja { cdSistema = 2, TipoArquivoInventario = TipoArquivoInventario.Final };

            var target = new SupercenterPermiteApenasArquivosFinaisSpec();

            Assert.IsTrue(target.IsSatisfiedBy(loja));
        }

        [Test]
        public void SupercenterPermiteApenasArquivosFinaisSpec_AtacadoParcial_Satisfied()
        {
            var loja = new Loja { cdSistema = 2, TipoArquivoInventario = TipoArquivoInventario.Parcial };

            var target = new SupercenterPermiteApenasArquivosFinaisSpec();

            Assert.IsTrue(target.IsSatisfiedBy(loja));
        }
    }
}
