using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class AutorizaPedidoServiceTest
    {
        [Test]
        public void ObterAutorizacoesPorSugestaoPedido_IdSugestaoPedido_Autorizacoes()
        {
            var gw = MockRepository.GenerateMock<IAutorizaPedidoGateway>();
            var sv = new AutorizaPedidoService(gw);

            var paging = new Paging();
            var a = new AutorizaPedido() { IdAutorizaPedido = 1 };

            gw
                .Expect(x => x.ObterAutorizacoesPorSugestaoPedido(1, paging))
                .Return(new[] { a });

            var actual = sv.ObterAutorizacoesPorSugestaoPedido(1, paging);

            Assert.AreEqual(1, actual.Count());
        }

        [Test]
        public void ExisteAutorizacaoPedido_NaoExiste_False()
        {
            var gateway = MockRepository.GenerateMock<IAutorizaPedidoGateway>();

            gateway.Expect(g => g.Find(null, null)).IgnoreArguments().Return(new AutorizaPedido[0]);
            gateway.Stub(g => g.Find(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AutorizaPedidoService(gateway);

            Assert.IsFalse(target.ExisteAutorizacaoPedido(DateTime.Today, 1, 1));

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void ExisteAutorizacaoPedido_Existe_True()
        {
            var gateway = MockRepository.GenerateMock<IAutorizaPedidoGateway>();

            gateway.Expect(g => g.Find(null, null)).IgnoreArguments().Return(new AutorizaPedido[] { new AutorizaPedido() });
            gateway.Stub(g => g.Find(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AutorizaPedidoService(gateway);

            Assert.IsTrue(target.ExisteAutorizacaoPedido(DateTime.Today, 1, 1));

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void AutorizarPedido_Pedido_Ok()
        {
            var gateway = MockRepository.GenerateMock<IAutorizaPedidoGateway>();

            gateway.Expect(g => g.Find((string)null, (object)null)).IgnoreArguments().Return(new AutorizaPedido[0]);
            gateway.Stub(g => g.Find((string)null, (object)null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Expect(g => g.Insert((AutorizaPedido)null)).IgnoreArguments();
            gateway.Stub(g => g.Insert((AutorizaPedido)null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AutorizaPedidoService(gateway);

            target.AutorizarPedido(DateTime.Today, 1, 1);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void AutorizarPedido_PedidoJaAutorizado_Exception()
        {
            var gateway = MockRepository.GenerateMock<IAutorizaPedidoGateway>();

            gateway.Expect(g => g.Find((string)null, (object)null)).IgnoreArguments().Return(new AutorizaPedido[] { new AutorizaPedido() });
            gateway.Stub(g => g.Find((string)null, (object)null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Insert((AutorizaPedido)null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AutorizaPedidoService(gateway);

            Assert.Throws<UserInvalidOperationException>(() =>
            {
                target.AutorizarPedido(DateTime.Today, 1, 1);
            });

            gateway.VerifyAllExpectations();
        }
    }
}
