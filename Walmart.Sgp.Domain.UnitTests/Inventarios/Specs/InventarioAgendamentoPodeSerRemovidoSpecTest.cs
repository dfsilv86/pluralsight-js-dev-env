﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class InventarioAgendamentoPodeSerRemovidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_InventarioCancelado_False()
        {
            var agendamento = new InventarioAgendamento
            {
                Inventario = new Inventario { stInventario = InventarioStatus.Cancelado }
            };

            var target = new InventarioAgendamentoPodeSerRemovidoSpec(null);
            var actual = target.IsSatisfiedBy(agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.CanceledInventaryCannotBeRemoved, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ComUnicoAgendamentoParaData_False()
        {
            var agendamento = new InventarioAgendamento
            {
                Inventario = new Inventario 
                { 
                    stInventario = InventarioStatus.Aberto,
                    IDLoja = 1,
                    IDDepartamento = 2,
                    dhInventario = DateTime.Today.AddMonths(1)
                },
                Bandeira = new Bandeira { CdSistema = 11 },
                Loja = new Loja { cdLoja = 111 },
                Departamento = new Departamento { cdDepartamento = 1111 },
                dtAgendamento = DateTime.Today.AddMonths(1)
            };

            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterQuantidadeAgendamentos(1, 2, DateTime.Today.AddMonths(1))).Return(1);

            var target = new InventarioAgendamentoPodeSerRemovidoSpec(agendamentoGateway);
            var actual = target.IsSatisfiedBy(agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.UniqueInventarySchedulingToStoreCannotBeRemoved.With(11, 111, 1111, DateTime.Today.AddMonths(1)), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ComMaisDeUmAgendamentoParaDataMasComDataNaoFutura_False()
        {
            var agendamento = new InventarioAgendamento
            {
                Inventario = new Inventario
                {
                    stInventario = InventarioStatus.Aberto,
                    IDLoja = 1,
                    IDDepartamento = 2,
                    dhInventario = DateTime.Today
                }
            };

            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterQuantidadeAgendamentos(1, 2, DateTime.Today)).Return(2);

            var target = new InventarioAgendamentoPodeSerRemovidoSpec(agendamentoGateway);
            var actual = target.IsSatisfiedBy(agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.InventaryWithDateBeforeTodayCannotBeRemoved, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_NaoEhAgendamentoUnicoEEhDataFutura_True()
        {
            var agendamento = new InventarioAgendamento
            {
                Inventario = new Inventario
                {
                    stInventario = InventarioStatus.Aberto,
                    IDLoja = 1,
                    IDDepartamento = 2,
                    dhInventario = DateTime.Today.AddDays(1)
                }
            };

            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterQuantidadeAgendamentos(1, 2, DateTime.Today)).Return(2);

            var target = new InventarioAgendamentoPodeSerRemovidoSpec(agendamentoGateway);
            var actual = target.IsSatisfiedBy(agendamento);
            Assert.IsTrue(actual.Satisfied);
        }
    }
}