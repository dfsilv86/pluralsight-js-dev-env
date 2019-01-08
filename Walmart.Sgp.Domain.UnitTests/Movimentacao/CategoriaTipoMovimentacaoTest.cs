using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao
{
    [TestFixture]
    [Category("Domain")]
    public class CategoriaTipoMovimentacaoTest
    {
        [Test]
        public void ObterIdsTipoMovimentacao_CategoriaNaoMapeada_Exception()
        {
            Assert.Catch(() =>
            {
                CategoriaTipoMovimentacao.ObterIdsTipoMovimentacao(CategoriaTipoMovimentacao.NaoDefinida);
            }, Texts.NoIdsMapForMovementTypeCategory, CategoriaTipoMovimentacao.NaoDefinida);

        }

        [Test]
        public void ObterIdsTipoMovimentacao_AjusteDeEstoque_IDs()
        {
            var actual = CategoriaTipoMovimentacao.ObterIdsTipoMovimentacao(CategoriaTipoMovimentacao.AjusteDeEstoque);
            Assert.AreEqual(new int[] { 15, 16 }, actual);
        }

        [Test]
        public void ObterIdsTipoMovimentacao_Quebra_IDs()
        {
            var actual = CategoriaTipoMovimentacao.ObterIdsTipoMovimentacao(CategoriaTipoMovimentacao.Quebra);
            Assert.AreEqual(new int[] { 11, 12, 13 }, actual);
        }
    }
}
