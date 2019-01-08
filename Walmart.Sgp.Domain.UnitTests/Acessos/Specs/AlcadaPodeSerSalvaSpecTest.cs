using System;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaPodeSerSalvaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_AlcadaSemPerfil_False()
        {
            var alcada = new Alcada
            {
                IDPerfil = 0
            };

            var target = new AlcadaPodeSerSalvaSpec();
            var actual = target.IsSatisfiedBy(alcada);
            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaSemPercentualFlag_True()
        {
            var alcada = new Alcada
            {
                IDPerfil = 1,
                blAlterarPercentual = false
            };

            var target = new AlcadaPodeSerSalvaSpec();
            var actual = target.IsSatisfiedBy(alcada);
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaSemPercentuais_False()
        {
            var alcada = new Alcada
            {
                IDPerfil = 1,
                blAlterarPercentual = true,
                vlPercentualAlterado = null
            };

            var target = new AlcadaPodeSerSalvaSpec();
            var actual = target.IsSatisfiedBy(alcada);
            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaComPerfilEPercentuais_True()
        {
            var alcada = new Alcada
            {
                IDPerfil = 1,
                blAlterarPercentual = true,
                vlPercentualAlterado = 100                
            };

            var target = new AlcadaPodeSerSalvaSpec();
            var actual = target.IsSatisfiedBy(alcada);
            Assert.IsTrue(actual.Satisfied);
        }
    }
}