using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class LojaDepartamentoSelectorTest
    {
        [Test]
        public void SelecionarUmDepartamentoDeUmaLoja_Args_1Loja1Departamento()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterPorId(1)).Return(new Loja { Id = 1, IDBandeira = 11 });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorId(2)).Return(new Departamento { Id = 2 });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.SelecionarUmDepartamentoDeUmaLoja(1, 1, 2);

            Assert.AreEqual(1, target.Lojas.Count());            
            Assert.AreEqual(1, target.Departamentos.Count());
        }

        [Test]
        public void SelecionarTodosOsDepartamentosDeUmaLoja_Args_1LojaTodosDepartamentos()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterPorId(1)).Return(new Loja { Id = 1, IDBandeira = 11 });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorSistema(1000, true)).Return(new Departamento[] { new Departamento { Id = 2 }, new Departamento { Id = 3 } });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.SelecionarTodosOsDepartamentosDeUmaLoja(1000, 2);

            Assert.AreEqual(1, target.Lojas.Count());
            Assert.AreEqual(2, target.Departamentos.Count());
        }

        [Test]
        public void SelecionarUmDepartamentoDeTodasAsLojas_Args_TodasLojas1Departamento()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterLojasPorBandeira(2000)).Return(new Loja[] { new Loja { Id = 1, IDBandeira = 2000 }, new Loja { Id = 2, IDBandeira = 2000 } });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorId(2)).Return(new Departamento { Id = 2 });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.SelecionarUmDepartamentoDeTodasAsLojas(1, 2000, 2);

            Assert.AreEqual(2, target.Lojas.Count());
            Assert.AreEqual(1, target.Departamentos.Count());
        }

        [Test]
        public void SelecionarTodosOsDepartamentosDeTodasAsLojas_Args_TodasLojasTodosDepartamentos()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterLojasPorBandeira(2000)).Return(new Loja[] { new Loja { Id = 1, IDBandeira = 2000 }, new Loja { Id = 2, IDBandeira = 2000 } });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorSistema(1000, true)).Return(new Departamento[] { new Departamento { Id = 2 }, new Departamento { Id = 3 } });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.SelecionarTodosOsDepartamentosDeTodasAsLojas(1000, 2000);

            Assert.AreEqual(2, target.Lojas.Count());
            Assert.AreEqual(2, target.Departamentos.Count());
        }     

          [Test]
        public void Selecionar_UmDepartamentoDeUmaLoja_1Loja1Departamento()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterPorId(1)).Return(new Loja { Id = 1, IDBandeira = 11 });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorId(2)).Return(new Departamento { Id = 2 });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.Selecionar(1000, 2000, 1, 2);

            Assert.AreEqual(1, target.Lojas.Count());            
            Assert.AreEqual(1, target.Departamentos.Count());
        }

        [Test]
        public void Selecionar_TodosOsDepartamentosDeUmaLoja_1LojaTodosDepartamentos()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterPorId(1)).Return(new Loja { Id = 1, IDBandeira = 11 });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorSistema(1000, true)).Return(new Departamento[] { new Departamento { Id = 2 }, new Departamento { Id = 3 } });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.Selecionar(1000, 200, 2, null);

            Assert.AreEqual(1, target.Lojas.Count());
            Assert.AreEqual(2, target.Departamentos.Count());
        }

        [Test]
        public void Selecionar_UmDepartamentoDeTodasAsLojas_TodasLojas1Departamento()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterLojasPorBandeira(2000)).Return(new Loja[] { new Loja { Id = 1, IDBandeira = 2000 }, new Loja { Id = 2, IDBandeira = 2000 } });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorId(2)).Return(new Departamento { Id = 2 });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.Selecionar(1000, 2000, null, 2);

            Assert.AreEqual(2, target.Lojas.Count());
            Assert.AreEqual(1, target.Departamentos.Count());
        }

        [Test]
        public void Selecionar_TodosOsDepartamentosDeTodasAsLojas_TodasLojasTodosDepartamentos()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(e => e.ObterLojasPorBandeira(2000)).Return(new Loja[] { new Loja { Id = 1, IDBandeira = 2000 }, new Loja { Id = 2, IDBandeira = 2000 } });

            var departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
            departamentoService.Expect(e => e.ObterPorSistema(1000, true)).Return(new Departamento[] { new Departamento { Id = 2 }, new Departamento { Id = 3 } });

            var target = new LojaDepartamentoSelector(lojaService, departamentoService);
            target.Selecionar(1000, 2000, null, null);

            Assert.AreEqual(2, target.Lojas.Count());
            Assert.AreEqual(2, target.Departamentos.Count());
        }     
    }
}
