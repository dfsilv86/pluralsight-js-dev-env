using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Specs
{
    [TestFixture]
    public class RegistroDeveSerSelecionadoSpecTest
    {
        [Test]
        public void RegistroDeveSerSelecionadoSpecTest_RegistroAlteradoSelecionado_Satisfied()
        {
            var alterados = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 1,
                    blativo = true
                }
            };

            var persistidos = new IRegistroSelecionavel[0];

            var spec = new RegistroDeveSerSelecionadoSpec(() => persistidos);
            var actual = spec.IsSatisfiedBy(alterados);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void RegistroDeveSerSelecionadoSpecTest_RegistroPersitidoFoiDesselecionado_IsNotSatisfied()
        {
            var alterados = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 1,
                    blativo = false
                }
            };

            var persistidos = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 1,
                    blativo = true
                }
            };

            var spec = new RegistroDeveSerSelecionadoSpec(() => persistidos);
            var actual = spec.IsSatisfiedBy(alterados);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void RegistroDeveSerSelecionadoSpecTest_SomentePersitidosSelecionados_Satisfied()
        {
            var alterados = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 1,
                    blativo = false
                }
            };

            var persistidos = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 2,
                    blativo = true
                }
            };

            var spec = new RegistroDeveSerSelecionadoSpec(() => persistidos);
            var actual = spec.IsSatisfiedBy(alterados);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void RegistroDeveSerSelecionadoSpecTest_PersistidosEAlteradosSelecionados_Satisfied()
        {
            var alterados = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 1,
                    blativo = true
                }
            };

            var persistidos = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 2,
                    blativo = true
                }
            };

            var spec = new RegistroDeveSerSelecionadoSpec(() => persistidos);
            var actual = spec.IsSatisfiedBy(alterados);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void RegistroDeveSerSelecionadoSpecTest_PersistidosEAlteradosNaoSelecionados_Satisfied()
        {
            var alterados = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 1,
                    blativo = false
                }
            };

            var persistidos = new[] 
            { 
                new RoteiroLoja
                {
                    IDRoteiroLoja = 2,
                    blativo = false
                }
            };

            var spec = new RegistroDeveSerSelecionadoSpec(() => persistidos);
            var actual = spec.IsSatisfiedBy(alterados);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
