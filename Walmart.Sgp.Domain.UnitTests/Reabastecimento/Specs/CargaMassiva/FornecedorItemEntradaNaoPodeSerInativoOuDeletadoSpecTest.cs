using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs.CargaMassiva
{
    [TestFixture]
    [Category("Domain")]
    public class FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_Ativo_Satisfied()
        {
            IItemDetalheService service = MockRepository.GenerateMock<IItemDetalheService>();

            service.Expect(s => s.ObterPorItemESistema(10, 1)).Return(new ItemDetalhe { Id = 1 });
            service.Expect(s => s.ObterEstruturadoPorId(1)).Return(new ItemDetalhe { Id = 1, Fornecedor = new Fornecedor { blAtivo = true, stFornecedor = "A" } });

            var target = new FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec(service.ObterPorItemESistema, service.ObterEstruturadoPorId, 1);

            var result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 10 } });

            Assert.IsTrue(result.Satisfied);

            service.VerifyAllExpectations();
        }

        [Test]
        public void IsSatisfiedBy_ItemNulo_Satisfied()
        {
            IItemDetalheService service = MockRepository.GenerateMock<IItemDetalheService>();

            service.Expect(s => s.ObterPorItemESistema(10, 1)).Return(null);

            var target = new FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec(service.ObterPorItemESistema, service.ObterEstruturadoPorId, 1);

            var result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 10 } });

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_Inativo_NotSatisfied()
        {
            IItemDetalheService service = MockRepository.GenerateMock<IItemDetalheService>();

            service.Expect(s => s.ObterPorItemESistema(10, 1)).Return(new ItemDetalhe { Id = 1 });
            service.Expect(s => s.ObterPorItemESistema(20, 1)).Return(new ItemDetalhe { Id = 2 });
            service.Expect(s => s.ObterPorItemESistema(30, 1)).Return(new ItemDetalhe { Id = 3 });
            service.Expect(s => s.ObterEstruturadoPorId(1)).Return(new ItemDetalhe { Id = 1, Fornecedor = new Fornecedor { blAtivo = true, stFornecedor = "I" } });
            service.Expect(s => s.ObterEstruturadoPorId(2)).Return(new ItemDetalhe { Id = 2, Fornecedor = new Fornecedor { blAtivo = true, stFornecedor = "D" } });
            service.Expect(s => s.ObterEstruturadoPorId(3)).Return(new ItemDetalhe { Id = 3, Fornecedor = new Fornecedor { blAtivo = false, stFornecedor = "A" } });

            var target = new FornecedorItemEntradaNaoPodeSerInativoOuDeletadoSpec(service.ObterPorItemESistema, service.ObterEstruturadoPorId, 1);

            var result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 10 } });

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual("Fornecedor do Item de Entrada Inativo ou Deletado", result.Reason);

            result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 20 } });

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual("Fornecedor do Item de Entrada Inativo ou Deletado", result.Reason);

            result = target.IsSatisfiedBy(new RelacaoItemLojaCDVinculo[] { new RelacaoItemLojaCDVinculo { CdItemDetalheEntrada = 30 } });

            Assert.IsFalse(result.Satisfied);
            Assert.AreEqual("Fornecedor do Item de Entrada Inativo ou Deletado", result.Reason);

            service.VerifyAllExpectations();
        }
    }
}
