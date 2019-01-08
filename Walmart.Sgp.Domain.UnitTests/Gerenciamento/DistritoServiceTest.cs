using NUnit.Framework;
using Rhino.Mocks;
using System;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class DistritoServiceTest
    {
        [Test]
        public void Salvar_DistritoJaExiste_AtualizaPropriedades()
        {
            var distritoGateway = MockRepository.GenerateMock<IDistritoGateway>();
            var distritoService = new DistritoService(distritoGateway);

            var distrito = new Distrito()
            {
                IDDistrito = 15,
                IDRegiao = 1,
                nmDistrito = "Teste",
                cdUsuarioResponsavelDistrito = 1,
                dhCriacao = DateTime.Now.AddDays(-1),
                cdUsuarioCriacao = 200
            };

            distritoService.Salvar(distrito);

            distritoGateway.AssertWasCalled(g => g.Update(
                    @"dhAtualizacao = @dhAtualizacao, 
                     cdUsuarioAtualizacao = @cdUsuarioAtualizacao, 
                     cdUsuarioResponsavelDistrito = @cdUsuarioResponsavelDistrito",
                      distrito));
        }

        [Test]
        public void Salvar_DistritoNaoExiste_Insere()
        {
            var distritoGateway = MockRepository.GenerateMock<IDistritoGateway>();
            var distritoService = new DistritoService(distritoGateway);

            var novoDistrito = new Distrito() { IDRegiao = 1, nmDistrito = "Teste", cdUsuarioResponsavelDistrito = 1 };

            distritoService.Salvar(novoDistrito);

            distritoGateway.AssertWasCalled(g => g.Insert(novoDistrito));
        }

        [Test]
        public void ObterPorRegiao_IDRegiao_Distrito()
        {
            var distritos = new Distrito[] { new Distrito { IDDistrito = 1 }, new Distrito { IDDistrito = 2 } };

            var gateway = MockRepository.GenerateMock<IDistritoGateway>();
            gateway.Expect(dg => dg.Find(null, null)).IgnoreArguments().Return(distritos);

            var target = new DistritoService(gateway);

            var result = target.ObterPorRegiao(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, distritos);
        }
    }
}
