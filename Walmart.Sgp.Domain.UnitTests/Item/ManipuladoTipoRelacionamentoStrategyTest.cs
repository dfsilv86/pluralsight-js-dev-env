using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    public class ManipuladoTipoRelacionamentoStrategyTest
    {
        [Test]
        public void DesmarcarItemDetalheComoPrincipal_ItemDetalhe_TipoDesmarcado()
        {
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.AlterarManipulado(1, TipoManipulado.NaoDefinido);
            var target = new ManipuladoTipoRelacionamentoStrategy(itemDetalheService);
            target.DesmarcarItemDetalheComoPrincipal(1);
        }

        [Test]
        public void DesmarcarItemDetalheComoSecundario_ItemDetalhe_TipoDesmarcado()
        {
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.AlterarManipulado(1, TipoManipulado.NaoDefinido);
            var target = new ManipuladoTipoRelacionamentoStrategy(itemDetalheService);
            target.DesmarcarItemDetalheComoSecundario(1);
        }

        [Test]
        public void MarcarItemDetalheComoPrincipal_ItemDetalhe_TipoMarcado()
        {
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.AlterarManipulado(1, TipoManipulado.Pai);
            var target = new ManipuladoTipoRelacionamentoStrategy(itemDetalheService);
            target.MarcarItemDetalheComoPrincipal(1);
        }

        [Test]
        public void MarcarItemDetalheComSecundario_ItemDetalhe_TipoMarcado()
        {
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.AlterarManipulado(1, TipoManipulado.Derivado);
            var target = new ManipuladoTipoRelacionamentoStrategy(itemDetalheService);
            target.MarcarItemDetalheComoSecundario(1);
        }
    }
}
