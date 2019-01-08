using System;
using NUnit.Framework;
using Walmart.Sgp.Domain.Movimentacao;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Ado")]
    [Category("Dapper")]
    public class MovimentacaoGatewayTest : DataGatewayTestBase<IMovimentacaoGateway>
    {
        [Test]
        public void SelExtratoProdutoMovimentacao_Args_Extrato()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var actual = target.SelExtratoProdutoMovimentacao(1, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1), 1, "", 1);
                Assert.IsNotNull(actual);
            });
        }
    }
}
