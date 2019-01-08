using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemSaida;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_VinculoItemSaidaItemVinculado_True()
        {
            var vinculos = new List<RelacaoItemLojaCDVinculo>()
            {
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 1,
                    CdItemDetalheEntrada = 2,
                    CdItemDetalheSaida = 1,
                    CdLoja = 1,
                    RowIndex = 1
                }
            };

            var relacao = new RelacaoItemLojaCD() { IDItem = 1, IdItemEntrada = 2 };

            var target = new VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpec((q, w, e) => { return new RelacaoItemLojaCD[] { relacao }; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_VinculoItemSaidaSemItemVinculado_False()
        {
            var vinculos = new List<RelacaoItemLojaCDVinculo>()
            {
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 1,
                    CdItemDetalheEntrada = 2,
                    CdItemDetalheSaida = 1,
                    CdLoja = 1,
                    RowIndex = 1
                }
            };

            var relacao = new RelacaoItemLojaCD() { IDItem = 1, IdItemEntrada = null };

            var target = new VinculoItemSaidaNaoPodeTerItemEntradaVinculadoSpec((q, w, e) => { return new RelacaoItemLojaCD[] { relacao }; }, 1);
            var actual = target.IsSatisfiedBy(vinculos);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
