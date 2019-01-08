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
    public class LojaDeveSerAtendidaPeloCDSpecTest
    {
        [Test]
        public void IsSatisfiedBy_LojaAtendidaPeloCD_True()
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

            var target = new LojaDeveSerAtendidaPeloCDSpec((q, w, e) => { return true; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_LojaNaoAtendidaPeloCD_False()
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

            var target = new LojaDeveSerAtendidaPeloCDSpec((q, w, e) => { return false; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsFalse(actual.Satisfied);
        }

    }
}
