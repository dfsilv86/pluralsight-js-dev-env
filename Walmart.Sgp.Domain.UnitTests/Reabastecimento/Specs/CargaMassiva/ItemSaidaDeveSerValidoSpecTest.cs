using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class ItemSaidaDeveSerValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemEntrada_Satisfied()
        {
            var service = MockRepository.GenerateMock<IRelacaoItemLojaCDService>();

            service.Expect(s => s.ObterItemSaidaAtendeRequisitos(10, 1, 1, 1)).Return(null);

            var target = new ItemSaidaDeveSerValidoSpec(service.ObterItemSaidaAtendeRequisitos, 1);

            var actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 10 } };

            var result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            service.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaComRequisitos_Satisfied()
        {
            var service = MockRepository.GenerateMock<IRelacaoItemLojaCDService>();

            service.Expect(s => s.ObterItemSaidaAtendeRequisitos(10, 1, 1, 1)).Return(true);

            var target = new ItemSaidaDeveSerValidoSpec(service.ObterItemSaidaAtendeRequisitos, 1);

            var actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 10 } };

            var result = target.IsSatisfiedBy(actual);

            Assert.IsTrue(result.Satisfied);

            service.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaSemrequisitos_NotSatisfied()
        {
            var service = MockRepository.GenerateMock<IRelacaoItemLojaCDService>();

            service.Expect(s => s.ObterItemSaidaAtendeRequisitos(10, 1, 1, 1)).Return(false);

            var target = new ItemSaidaDeveSerValidoSpec(service.ObterItemSaidaAtendeRequisitos, 1);

            var actual = new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdCD = 1, CdLoja = 1, CdItemDetalheEntrada = 10 } };

            var result = target.IsSatisfiedBy(actual);

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual(Texts.ItemOutputMustBeValid, result.Reason);

            service.VerifyAllExpectations();
        }
    }
}
