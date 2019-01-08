using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class VinculoItemXrefDeveSerPrimeSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemXrefPrime_True()
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

            var result = new RelacaoItemLojaCDXrefItemPrime()
            {
                CdItem = 1,
                CdItemPrime = 1,
                Sequencial = 1,
            };

            var target = new VinculoItemXrefDeveSerPrimeSpec((q, w, e, r) => { return result; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemXrefNaoPrime_False()
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

            var result = new RelacaoItemLojaCDXrefItemPrime()
            {
                Sequencial = 2,
                CdItemPrime = 1
            };

            var target = new VinculoItemXrefDeveSerPrimeSpec((q, w, e, r) => { return result; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsFalse(actual.Satisfied);
        }

    }
}
