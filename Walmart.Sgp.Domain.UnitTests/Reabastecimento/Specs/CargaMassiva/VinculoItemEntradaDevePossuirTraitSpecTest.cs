using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class VinculoItemEntradaDevePossuirTraitSpecTest
    {
        [Test]
        public void IsSatisfiedBy_VinculoItemComTrait_True()
        {
            var vinculos = new List<RelacaoItemLojaCDVinculo>()
            {
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 1,
                    CdItemDetalheEntrada = 1,
                    CdItemDetalheSaida = 1,
                    CdLoja = 1,
                    RowIndex = 1
                }
            };

            var target = new VinculoItemEntradaDevePossuirTraitSpec((q, w, e) => { return true; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_VinculoItemSemTrait_False()
        {
            var vinculos = new List<RelacaoItemLojaCDVinculo>()
            {
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 1,
                    CdItemDetalheEntrada = 1,
                    CdItemDetalheSaida = 1,
                    CdLoja = 1,
                    RowIndex = 1
                }
            };

            var target = new VinculoItemEntradaDevePossuirTraitSpec((q, w, e) => { return false; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsFalse(actual.Satisfied);
        }

    }
}
