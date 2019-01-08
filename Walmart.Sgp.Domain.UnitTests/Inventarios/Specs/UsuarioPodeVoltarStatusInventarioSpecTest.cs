using System;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioPodeVoltarStatusInventarioSpecTest
    {
        [Test]
        public void SatisfiedBy_UsuarioNaoAdministrador_NotSatisfied()
        {
            var usuario = new MemoryRuntimeUser
            {
                RoleId = 0,
                Id = 1,
                UserName = "Teste"
            };

            Assert.IsFalse(usuario.IsAdministrator);

            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Aprovado
            };

            var target = new UsuarioPodeVoltarStatusInventarioSpec(inventario, null, null);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }

        [Test]
        public void SatisfiedBy_UsuarioSemPermissao_NotSatisfied()
        {
            var usuario = new MemoryRuntimeUser
            {
                RoleId = 8,
                Id = 1,
                RoleName = "Administrador",
                UserName = "Teste",
                IsAdministrator = false                
            };

            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Aprovado
            };

            var target = new UsuarioPodeVoltarStatusInventarioSpec(inventario, null, null);
            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }

        [Test]
        public void SatisfiedBy_FechamentoFiscalComDataMenorQueInventario_NotSatisfied()
        {
            var usuario = new MemoryRuntimeUser
            {
                RoleId = 8,
                Id = 1,
                RoleName = "Administrador",
                UserName = "Teste",
                IsAdministrator = true                
            };

            Assert.IsTrue(usuario.IsAdministrator);

            var inventario = new Inventario
            {
                stInventario = InventarioStatus.Aprovado,
                dhInventario = new DateTime(2000, 2, 1)
            };
            
            var fechamentoFiscalGateway = MockRepository.GenerateMock<IFechamentoFiscalGateway>();
            fechamentoFiscalGateway.Expect(t => t.ObterUltimo(0)).IgnoreArguments()
                .Return(new FechamentoFiscal { nrMes = 3, nrAno = 2000 });

            var target = new UsuarioPodeVoltarStatusInventarioSpec(inventario, null, fechamentoFiscalGateway);

            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
        }

        [Test]
        public void SatisfiedBy_UltimoInventarioDiferente_NotSatisfied()
        {
            var usuario = new MemoryRuntimeUser
            {
                RoleId = 8,
                Id = 1,
                RoleName = "Administrador",
                IsAdministrator = true,
                UserName = "Teste"                
            };

            Assert.IsTrue(usuario.IsAdministrator);

            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Aprovado,
                dhInventario = new System.DateTime(2000, 4, 1)
            };
            
            var fechamentoFiscalGateway = MockRepository.GenerateMock<IFechamentoFiscalGateway>();
            fechamentoFiscalGateway.Stub(t => t.ObterUltimo(0)).IgnoreArguments()
                .Return(new FechamentoFiscal { nrMes = 3, nrAno = 2000 });

            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(t => t.ObterUltimo(0, null, DateTime.MinValue)).IgnoreArguments()
                .Return(new Inventario
                {
                    IDInventario = 2
                });

            var target = new UsuarioPodeVoltarStatusInventarioSpec(
                inventario,
                inventarioGateway,
                fechamentoFiscalGateway);

            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsFalse(actual);
            inventarioGateway.VerifyAllExpectations();
        }

        [Test]
        public void SatisfiedBy_UltimoInventarioIgual_Satisfied()
        {
            var usuario = new MemoryRuntimeUser
            {
                RoleId = 8,
                Id = 1,
                RoleName = "Administrador",
                IsAdministrator = true,
                UserName = "Teste"                
            };

            Assert.IsTrue(usuario.IsAdministrator);

            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Aprovado,
                dhInventario = new System.DateTime(2000, 4, 1)
            };

            var fechamentoFiscalGateway = MockRepository.GenerateMock<IFechamentoFiscalGateway>();
            fechamentoFiscalGateway.Stub(t => t.ObterUltimo(0)).IgnoreArguments()
                .Return(new FechamentoFiscal { nrMes = 3, nrAno = 2000 });

            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(t => t.ObterUltimo(0, null, DateTime.MinValue)).IgnoreArguments()
                .Return(new Inventario
                {
                    IDInventario = 1
                });

            var target = new UsuarioPodeVoltarStatusInventarioSpec(
                inventario,
                inventarioGateway,
                fechamentoFiscalGateway);

            var actual = target.IsSatisfiedBy(usuario);
            Assert.IsTrue(actual);
            inventarioGateway.VerifyAllExpectations();
        }
    }
}