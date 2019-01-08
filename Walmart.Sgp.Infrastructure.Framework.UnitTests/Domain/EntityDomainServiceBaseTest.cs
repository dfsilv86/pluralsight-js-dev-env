using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Framework")]
    public class EntityDomainServiceBaseTest
    {       
        [Test]
        public void Salvar_EhStampContainer_ComCarimbo()
        {
            var gateway = MockRepository.GenerateMock<IDataGateway<EntityStub>>();
            var target = new EntityDomainServiceStub(gateway);
            var actual = new EntityStub();
            
            // Insert.
            target.Salvar(actual);
            Assert.IsTrue(actual.CdUsuarioCriacao.HasValue);
            Assert.AreNotEqual(DateTime.MinValue, actual.DhCriacao);
            Assert.IsFalse(actual.CdUsuarioAtualizacao.HasValue);
            Assert.IsFalse(actual.DhAtualizacao.HasValue);

            // Update.
            actual.Id = 1;
            target.Salvar(actual);
            Assert.IsTrue(actual.CdUsuarioCriacao.HasValue);
            Assert.AreNotEqual(DateTime.MinValue, actual.DhCriacao);
            Assert.IsTrue(actual.CdUsuarioAtualizacao.HasValue);
            Assert.IsTrue(actual.DhAtualizacao.HasValue);
        }  

        [Test]
        public void ObterTodos_Paging_Paginado()
        {
            var paging = new Paging();
            var gateway = MockRepository.GenerateMock<IDataGateway<EntityStub>>();
            gateway.Expect(g => g.FindAll(paging)).Return(new EntityStub[0]);
            var target = new EntityDomainServiceStub(gateway);
            var actual = target.ObterTodos(paging);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ObterPorIds_Ids_Entidades()
        {
            var ids = new int[] { 1, 2 };

            var gateway = MockRepository.GenerateMock<IDataGateway<EntityStub>>();
            gateway.Expect(g => g.FindByIds(ids)).Return(new EntityStub[0]);
            var target = new EntityDomainServiceStub(gateway);
            var actual = target.ObterPorIds(ids);
            Assert.IsNotNull(actual);
        }
    }
}
