using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Acessos.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaNaoPodeTerDetalheRepetidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_AlcadaSemDetalheDuplicado_True()
        {
            var alcada = new Alcada
            {
                IDAlcada = 1,
                Detalhe = new List<AlcadaDetalhe>()
            };

            var gw = MockRepository.GenerateMock<IAlcadaGateway>();
            gw.Expect(g => g.ObterEstruturado(1, null)).Return(alcada);

            var target = new AlcadaNaoPodeTerDetalheRepetidoSpec(gw.ObterEstruturado);
            var actual = target.IsSatisfiedBy(alcada);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaComDetalheDuplicadoEmMemoria_False()
        {
            var alcada = new Alcada
            {
                IDAlcada = 1,
                Detalhe = new List<AlcadaDetalhe>()
                {
                    new AlcadaDetalhe()
                    {
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira() { IDBandeira = 1 },
                        Departamento = new Domain.EstruturaMercadologica.Departamento() { IDDepartamento = 1 },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa() { IdRegiaoAdministrativa = 1 }
                    },
                    new AlcadaDetalhe()
                    {
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira() { IDBandeira = 1 },
                        Departamento = new Domain.EstruturaMercadologica.Departamento() { IDDepartamento = 1 },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa() { IdRegiaoAdministrativa = 1 }
                    }
                }
            };

            var gw = MockRepository.GenerateMock<IAlcadaGateway>();
            gw.Expect(g => g.ObterEstruturado(1, null)).Return(alcada);

            var target = new AlcadaNaoPodeTerDetalheRepetidoSpec(gw.ObterEstruturado);
            var actual = target.IsSatisfiedBy(alcada);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AlcadaComDetalheDuplicadoEmBanco_False()
        {
            var alcada = new Alcada
            {
                IDAlcada = 1,
                Detalhe = new List<AlcadaDetalhe>()
                {
                    new AlcadaDetalhe()
                    {
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira() { IDBandeira = 1 },
                        Departamento = new Domain.EstruturaMercadologica.Departamento() { IDDepartamento = 1 },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa() { IdRegiaoAdministrativa = 1 }
                    }
                }
            };

            var alcadaEmBanco = new Alcada()
            {
                IDAlcada = 1,
                Detalhe = new List<AlcadaDetalhe>()
                {
                    new AlcadaDetalhe()
                    {
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira() { IDBandeira = 1 },
                        Departamento = new Domain.EstruturaMercadologica.Departamento() { IDDepartamento = 1 },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa() { IdRegiaoAdministrativa = 1 }
                    }
                }
            };

            var gw = MockRepository.GenerateMock<IAlcadaGateway>();
            gw.Expect(g => g.ObterEstruturado(1, null)).Return(alcadaEmBanco);

            var target = new AlcadaNaoPodeTerDetalheRepetidoSpec(gw.ObterEstruturado);
            var actual = target.IsSatisfiedBy(alcada);

            Assert.IsFalse(actual.Satisfied);
        }

    }
}
