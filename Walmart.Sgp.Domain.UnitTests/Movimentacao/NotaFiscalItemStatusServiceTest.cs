using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Memory;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao
{
    [TestFixture]
    [Category("Domain")]
    public class NotaFiscalItemStatusServiceTest
    {
        [Test]
        public void ObterTodos_NoArgs_NotaFiscalItemStatus()
        {
            var gateway = MockRepository.GenerateMock<MemoryNotaFiscalItemStatusGateway>(); ;
            gateway.Insert(new NotaFiscalItemStatus {  IDNotaFiscalItemStatus = 1, DsNotaFiscalItemStatus = "Conforme", SiglaNotaFiscalItemStatus = "C" });

            var target = new NotaFiscalItemStatusService(gateway);
            var actual = target.ObterTodos();

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(1, actual.First().IDNotaFiscalItemStatus);
            Assert.AreEqual("Conforme", actual.First().DsNotaFiscalItemStatus);
            Assert.AreEqual("C", actual.First().SiglaNotaFiscalItemStatus);

            gateway.VerifyAllExpectations();
        }
    }
}
