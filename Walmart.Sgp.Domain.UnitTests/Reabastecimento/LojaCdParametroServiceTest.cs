using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class LojaCdParametroServiceTest
    {
        [Test]
        public void Salvar_Novo_Criado()
        {
            var gateway = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var cdService = MockRepository.GenerateMock<ICDService>();
            var target = new LojaCdParametroService(gateway, cdService);
            var entity = new LojaCdParametro
            {
                Id = 0,
                CD = new CD
                {
                    Id = 11,
                    nmNome = "Novo nome"
                },
                tpWeek = TipoSemana.Par,
                tpInterval = TipoIntervalo.Semanal
            };

            target.Salvar(entity);
            Assert.IsFalse(entity.CdUsuarioAtualizacao.HasValue);
            Assert.IsFalse(entity.DhAtualizacao.HasValue);
            Assert.IsTrue(entity.CdUsuarioCriacao.HasValue);
            Assert.AreNotEqual(DateTime.MinValue, entity.DhCriacao);

            cdService.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_Existente_Atualizado()
        {
            var gateway = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var cdService = MockRepository.GenerateMock<ICDService>();
            cdService.Expect(e => e.AtualizarNomeCD(11, "Novo nome"));

            var target = new LojaCdParametroService(gateway, cdService);
            var entity = new LojaCdParametro
            {
                Id = 1,
                CD = new CD
                {
                    Id = 11,
                    nmNome = "Novo nome"
                },
                tpWeek = TipoSemana.Par,
                tpInterval = TipoIntervalo.Semanal
            };

            target.Salvar(entity);
            Assert.IsTrue(entity.CdUsuarioAtualizacao.HasValue);
            Assert.IsTrue(entity.DhAtualizacao.HasValue);
            Assert.IsFalse(entity.CdUsuarioCriacao.HasValue);
            Assert.AreEqual(DateTime.MinValue, entity.DhCriacao);

            cdService.VerifyAllExpectations();
        }
    }
}
