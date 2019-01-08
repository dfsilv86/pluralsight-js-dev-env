using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class MovimentacaoServiceTest
    {
        [Test]
        public void SelExtratoProdutoMovimentacao_Args_ItemsExtrato()
        {
            var gateway = MockRepository.GenerateMock<IMovimentacaoGateway>();
            var beginDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            gateway.Expect(g => g.SelExtratoProdutoMovimentacao(1, beginDate, endDate, 2, "", 3)).Return(new ItemExtrato[]
            {
                new ItemExtrato(),
                new ItemExtrato()
            });

            var target = new MovimentacaoService(gateway);
            var actual = target.RelExtratoProdutoMovimentacao(1, beginDate, endDate, 2, "", 3);

            Assert.AreEqual(2, actual.Count());
            gateway.VerifyAllExpectations();
        }
    }
}
