using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class ItemEntradaVinculadoXrefPrimeDeveSerStapleSpecTest
    {
        [Test]
        public void IsSatisfiedBy_Null_Satisfied()
        {
            IRelacaoItemLojaCDGateway gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();

            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(10, 1, 1, 1)).Return(null);

            var target = new ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec(gateway.ObterTipoReabastecimentoItemVinculadoXrefPrime, 1);

            var actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 10 } };

            var result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_Staple_Satisfied()
        {
            IRelacaoItemLojaCDGateway gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();

            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(10, 1, 1, 1)).Return(ValorTipoReabastecimento.StapleStock20);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(20, 1, 1, 1)).Return(ValorTipoReabastecimento.StapleStock22);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(30, 1, 1, 1)).Return(ValorTipoReabastecimento.StapleStock40);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(40, 1, 1, 1)).Return(ValorTipoReabastecimento.StapleStock42);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(50, 1, 1, 1)).Return(ValorTipoReabastecimento.StapleStock43);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(60, 1, 1, 1)).Return(ValorTipoReabastecimento.StapleStock81);

            var target = new ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec(gateway.ObterTipoReabastecimentoItemVinculadoXrefPrime, 1);

            var actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 10 } };

            var result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 20 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 30 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 40 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 50 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 60 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_NonStaple_NotSatisfied()
        {
            IRelacaoItemLojaCDGateway gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();

            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(10, 1, 1, 1)).Return(ValorTipoReabastecimento.CrossDocking3);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(20, 1, 1, 1)).Return(ValorTipoReabastecimento.CrossDocking33);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(30, 1, 1, 1)).Return(ValorTipoReabastecimento.CrossDocking94);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(40, 1, 1, 1)).Return(ValorTipoReabastecimento.Dsd37);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(50, 1, 1, 1)).Return(ValorTipoReabastecimento.Dsd7);
            gateway.Expect(g => g.ObterTipoReabastecimentoItemVinculadoXrefPrime(60, 1, 1, 1)).Return(ValorTipoReabastecimento.Dsd97);
            
            var target = new ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec(gateway.ObterTipoReabastecimentoItemVinculadoXrefPrime, 1);

            var actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 10 } };

            var result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemXrefPrimeNotStaple, result.Reason);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 20 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemXrefPrimeNotStaple, result.Reason);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 30 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemXrefPrimeNotStaple, result.Reason);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 40 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemXrefPrimeNotStaple, result.Reason);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 50 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemXrefPrimeNotStaple, result.Reason);

            actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 60 } };

            result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemXrefPrimeNotStaple, result.Reason);

            gateway.VerifyAllExpectations();
        }
    }
}
