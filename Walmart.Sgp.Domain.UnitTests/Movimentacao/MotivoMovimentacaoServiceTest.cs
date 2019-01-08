using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Memory;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao
{
    [TestFixture]
    [Category("Domain")]
    public class MotivoMovimentacaoServiceTest
    {
        [Test]
        public void ObterVisiveis_NoArgs_ApenasVisiveis()
        {
            var gateway = new MemoryMotivoMovimentacaoGateway();
            gateway.Insert(new MotivoMovimentacao { Id = 1, blAtivo = true, blExibir = false });
            gateway.Insert(new MotivoMovimentacao { Id = 2, blAtivo = true, blExibir = true });
            gateway.Insert(new MotivoMovimentacao { Id = 3, blAtivo = false, blExibir = true });
            gateway.Insert(new MotivoMovimentacao { Id = 4, blAtivo = true, blExibir = true });

            var target = new MotivoMovimentacaoService(gateway);
            var actual = target.ObterVisiveis();
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(1, actual.Count(a => a.Id == 2));
            Assert.AreEqual(1, actual.Count(a => a.Id == 4));
        }
    }
}
