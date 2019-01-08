using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class DepartamentoServiceTest
    {
        #region Fields
        private IDepartamentoGateway m_departamentoGateway;
        private DepartamentoService m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_departamentoGateway = new MemoryDepartamentoGateway();
            m_departamentoGateway.Insert(new Departamento { Id = 1, cdSistema = 1, cdDepartamento = 11, dsDepartamento = "11 perecivel", blPerecivel = "S" });
            m_departamentoGateway.Insert(new Departamento { Id = 2, cdSistema = 1, cdDepartamento = 12, dsDepartamento = "12 perecivel", blPerecivel = "S" });
            m_departamentoGateway.Insert(new Departamento { Id = 3, cdSistema = 1, cdDepartamento = 13, dsDepartamento = "13 nao perecivel", blPerecivel = "N" });
            m_departamentoGateway.Insert(new Departamento { Id = 4, cdSistema = 2, cdDepartamento = 14, dsDepartamento = "14 perecivel", blPerecivel = "S" });

            m_target = new DepartamentoService(m_departamentoGateway);
        }
        #endregion

        #region Tests
        [Test]
        public void ObterDepartamentosPorSistema_CdSistemaEBlPerecivel_Departamentos()
        {            
            var actual = m_target.ObterPorSistema(1, true);
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(1, actual.First().Id);
            Assert.AreEqual(2, actual.Last().Id);

            actual = m_target.ObterPorSistema(1, false);
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(3, actual.First().Id);

            actual = m_target.ObterPorSistema(2, true);
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(4, actual.First().Id);

            actual = m_target.ObterPorSistema(2, false);
            Assert.AreEqual(0, actual.Count());            
        }

        [Test]
        public void ObterPorCdDepartamento_CdDepartamento_Departamento()
        {
            m_departamentoGateway.Insert(new Departamento { Id = 1, cdSistema = 1, cdDepartamento = 11, dsDepartamento = "11 perecivel", blPerecivel = "S" });
            m_departamentoGateway.Insert(new Departamento { Id = 2, cdSistema = 1, cdDepartamento = 12, dsDepartamento = "12 perecivel", blPerecivel = "S" });
            m_departamentoGateway.Insert(new Departamento { Id = 3, cdSistema = 1, cdDepartamento = 13, dsDepartamento = "13 nao perecivel", blPerecivel = "N" });
            m_departamentoGateway.Insert(new Departamento { Id = 4, cdSistema = 2, cdDepartamento = 14, dsDepartamento = "14 perecivel", blPerecivel = "S" });

            var actual = m_target.ObterPorCdDepartamento(1, 11);
            Assert.AreEqual(1, actual.Id);

            actual = m_target.ObterPorCdDepartamento(2, 14);
            Assert.AreEqual(4, actual.Id);

            actual = m_target.ObterPorCdDepartamento(1, 14);
            Assert.IsNull(actual);
        }

        [Test]
        public void AtualizarPerecivel_True_PerecivelAtualizado()
        {
            var departamento = new Departamento
            {
                Id = 50,
                cdSistema = 1,
                cdDepartamento = 50,
                dsDepartamento = "Departamento 50",
                blPerecivel = "N"
            };

            var gateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            gateway.Expect(t => t.Update("", departamento))
                .IgnoreArguments()
                .Callback((string s, Departamento d) =>
                {
                    departamento.blPerecivel = d.blPerecivel;
                    departamento.DhAtualizacao = d.DhAtualizacao;
                    departamento.CdUsuarioAtualizacao = d.CdUsuarioAtualizacao;
                    return true;
                });
            
            var target = new DepartamentoService(gateway);
            
            target.AtualizarPerecivel(departamento.Id, true);
            
            Assert.AreEqual("S", departamento.blPerecivel);
            Assert.IsNotNull(departamento.DhAtualizacao);
            Assert.AreEqual(RuntimeContext.Current.User.Id, departamento.CdUsuarioAtualizacao);
        }

        [Test]
        public void AtualizarPerecivel_False_PerecivelAtualizado()
        {
            var departamento = new Departamento
            {
                Id = 50,
                cdSistema = 1,
                cdDepartamento = 50,
                dsDepartamento = "Departamento 50",
                blPerecivel = "S"
            };

            var gateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            gateway.Expect(t => t.Update("", departamento))
                .IgnoreArguments()
                .Callback((string s, Departamento d) =>
                {
                    departamento.blPerecivel = d.blPerecivel;
                    departamento.DhAtualizacao = d.DhAtualizacao;
                    departamento.CdUsuarioAtualizacao = d.CdUsuarioAtualizacao;
                    return true;
                });

            var target = new DepartamentoService(gateway);

            target.AtualizarPerecivel(departamento.Id, false);

            Assert.AreEqual("N", departamento.blPerecivel);
            Assert.IsNotNull(departamento.DhAtualizacao);
            Assert.AreEqual(RuntimeContext.Current.User.Id, departamento.CdUsuarioAtualizacao);
        }
        #endregion
    }
}
