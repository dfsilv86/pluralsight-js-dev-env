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
    public class TipoMovimentacaoServiceTest
    {
        [Test]
        public void ObterPorCategoria_Categoria_TipoMovimentacao()
        {
            var gateway = MockRepository.GenerateMock<ITipoMovimentacaoGateway>();
            gateway.Expect(e => e.Find("IDTipoMovimentacao IN @ids", new { ids = new int[] { 15, 16 } })).IgnoreArguments();
            var target = new TipoMovimentacaoService(gateway);
            target.ObterPorCategoria(CategoriaTipoMovimentacao.AjusteDeEstoque);

            gateway.VerifyAllExpectations();
        }
    }
}
