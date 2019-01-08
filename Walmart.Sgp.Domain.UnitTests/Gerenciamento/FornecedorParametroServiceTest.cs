using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class FornecedorParametroServiceTest
    {
        [Test]
        public void EstaInativoOuExcluido_FornecedorAtivo_False()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var item = new ItemDetalhe() { IDItemDetalhe = 1, CdItem = 1, CdSistema = 1, IdFornecedorParametro = 1 };

            itemDetalheGw.Expect(g => g.ObterPorItemESistema(1, 1)).Return(item);

            var fp = new FornecedorParametro() { IDFornecedorParametro = 1, blAtivo = true, cdStatusVendor = "A" };

            gw.Expect(g => g.ObterEstruturadoPorId(1)).Return(fp);

            var result = svc.EstaInativoOuExcluido(1, 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void EstaInativoOuExcluido_FornecedorInativo_True()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var item = new ItemDetalhe() { IDItemDetalhe = 1, CdItem = 1, CdSistema = 1, IdFornecedorParametro = 1 };

            itemDetalheGw.Expect(g => g.ObterPorItemESistema(1, 1)).Return(item);

            var fp = new FornecedorParametro() { IDFornecedorParametro = 1, blAtivo = true, cdStatusVendor = "I" };

            gw.Expect(g => g.ObterEstruturadoPorId(1)).Return(fp);

            var result = svc.EstaInativoOuExcluido(1, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void EstaInativoOuExcluido_FornecedorExcluido_True()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var item = new ItemDetalhe() { IDItemDetalhe = 1, CdItem = 1, CdSistema = 1, IdFornecedorParametro = 1 };

            itemDetalheGw.Expect(g => g.ObterPorItemESistema(1, 1)).Return(item);

            var fp = new FornecedorParametro() { IDFornecedorParametro = 1, blAtivo = false, cdStatusVendor = "A" };

            gw.Expect(g => g.ObterEstruturadoPorId(1)).Return(fp);

            var result = svc.EstaInativoOuExcluido(1, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void EstaInativoOuExcluido_FornecedorInexistente_False()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var item = new ItemDetalhe() { IDItemDetalhe = 1, CdItem = 1, CdSistema = 1, IdFornecedorParametro = 1 };

            itemDetalheGw.Expect(g => g.ObterPorItemESistema(1, 1)).Return(item);

            var result = svc.EstaInativoOuExcluido(1, 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void EstaInativoOuExcluido_ItemInexistente_False()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var result = svc.EstaInativoOuExcluido(1, 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void PossuiVendorVinculado_ItemInexistente_True()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var result = svc.PossuiVendorVinculado(1, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void PossuiVendorVinculado_ItemComVendor_True()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var item = new ItemDetalhe() { IDItemDetalhe = 1, CdItem = 1, CdSistema = 1, IdFornecedorParametro = 1 };

            var fp = new FornecedorParametro() { IDFornecedorParametro = 1 };

            itemDetalheGw.Expect(g => g.ObterPorItemESistema(1, 1)).Return(item);

            gw.Expect(g => g.FindById(1)).Return(fp);

            var result = svc.PossuiVendorVinculado(1, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void PossuiVendorVinculado_ItemSemVendor_False()
        {
            var gw = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var itemDetalheGw = MockRepository.GenerateMock<IItemDetalheGateway>();

            var svc = new FornecedorParametroService(gw, itemDetalheGw);

            var item = new ItemDetalhe() { IDItemDetalhe = 1, CdItem = 1, CdSistema = 1, };

            itemDetalheGw.Expect(g => g.ObterPorItemESistema(1, 1)).Return(item);

            var result = svc.PossuiVendorVinculado(1, 1);

            Assert.IsFalse(result);
        }

    }
}
