using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaDetalheServiceTest
    {
        [Test]
        public void ObterPorIdAlcada_IdAlcada_AlcadaDetalhes()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaDetalheGateway>();
            gateway.Expect(g => g.ObterPorIdAlcada(1, null)).Return(new [] {
                new AlcadaDetalhe() 
                { 
                    IDAlcada = 1,
                    IDAlcadaDetalhe = 1
                }});

            var target = new AlcadaDetalheService(gateway);

            var result = target.ObterPorIdAlcada(1, null);

            Assert.AreEqual(1, result.Count());
        }
    }
}
