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
    public class InventarioAgendamentoPodeSerSalvoSpecTest
    {
        private InventarioAgendamento m_agendamento;
        private RangeValue<DateTime> m_intervalo;
        private IInventarioGateway m_inventarioGateway;
        private IInventarioAgendamentoGateway m_agendamentoGateway;
        private InventarioAgendamentoPodeSerSalvoSpec m_target;

        [SetUp]
        public void InitializeTest()
        {
            m_agendamento = new InventarioAgendamento
            {
                Inventario = new Inventario
                {
                    IDLoja = 1,
                    Loja = new Loja { cdLoja = 1 },
                    IDDepartamento = 2,
                    Departamento = new Departamento { cdDepartamento = 2 },
                    dhInventario = DateTime.Today.AddDays(20)
                },
                dtAgendamento = DateTime.Today.AddDays(10)
            };

            m_inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            m_intervalo = new RangeValue<DateTime> { StartValue = m_agendamento.dtAgendamento.AddDays(-5), EndValue = m_agendamento.dtAgendamento.AddDays(5) };
            m_agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            m_target = new InventarioAgendamentoPodeSerSalvoSpec(m_inventarioGateway, m_agendamentoGateway);
        }

        [Test]
        public void IsSatisfiedBy_DtAgendamentoInferiorAHoje_False()
        {
            m_agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, m_intervalo, InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            m_inventarioGateway.Expect(e => e.ContarInventarios(1, 2, m_intervalo, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);

            m_agendamento.IDInventarioAgendamento = 0;
            m_agendamento.dtAgendamento = DateTime.Now.AddDays(-1);
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.DateCannotBeLowerThanToday, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_InsertExisteAgendamentoAbertoOuImportado_False()
        {
            m_agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, m_intervalo, InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(1);

            m_agendamento.IDInventarioAgendamento = 0;
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ThereAreOthersOpenedApprovedInventoryScheduling.With(5, 1,2), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_InsertExisteInventarioAprovadoNoMes_False()
        {
            m_agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, m_intervalo, InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            m_inventarioGateway.Expect(e => e.ContarInventarios(1, 2, m_intervalo, InventarioStatus.Aprovado)).IgnoreArguments().Return(1);

            m_agendamento.IDInventarioAgendamento = 0;
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.InventoryDateIsNearToAnotherScheduling.With(1, 2), actual.Reason);
        }       

        [Test]
        public void IsSatisfiedBy_InsertNaoExisteAgendamentoAbertoOuImportadENaoExisteInventarioAprovadoNoMes_True()
        {
            m_agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, m_intervalo, InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            m_inventarioGateway.Expect(e => e.ContarInventarios(1, 2, m_intervalo, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);

            m_agendamento.IDInventarioAgendamento = 0;
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_UpdateDtAgendamentoPosteriorAHoje_False()
        {
            m_inventarioGateway.Expect(e => e.ContarInventarios(1, 2, m_intervalo, InventarioStatus.Aprovado)).IgnoreArguments().Return(1);

            m_agendamento.IDInventarioAgendamento = 1;
            m_agendamento.dtAgendamento = DateTime.Today;
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.OnlyUpdateInventorySchedulingWithScheduledStatus, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_UpdateExisteInventarioAprovadoNoMes_False()
        {
            m_inventarioGateway.Expect(e => e.ContarInventarios(1, 2, m_intervalo, InventarioStatus.Aprovado)).IgnoreArguments().Return(1);

            m_agendamento.IDInventarioAgendamento = 1;
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.CannotUpdateInventorySchedulingThereIsNotFinishedOne, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_UpdateENaoExisteInventarioAprovadoNoMes_True()
        {
            m_inventarioGateway.Expect(e => e.ContarInventarios(1, 2, m_intervalo, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);

            m_agendamento.IDInventarioAgendamento = 1;
            var actual = m_target.IsSatisfiedBy(m_agendamento);
            Assert.IsTrue(actual.Satisfied);
        }
    }
}