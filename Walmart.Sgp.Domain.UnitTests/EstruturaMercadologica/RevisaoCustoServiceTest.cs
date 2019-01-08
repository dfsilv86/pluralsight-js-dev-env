using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class RevisaoCustoServiceTest
    {
        [Test]
        public void Salvar_Novo_Inserido()
        {
            var currentContext = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext() { User = new MemoryRuntimeUser { Id = 123 } };

                RevisaoCusto revisaoCusto = new RevisaoCusto
                {
                    IDLoja = 1005,
                    IDItemDetalhe = 8022604,
                    IDStatusRevisaoCusto = 1,
                    IDMotivoRevisaoCusto = 1,
                    dtSolicitacao = new DateTime(2015, 12, 30),
                    vlCustoSolicitado = 0,
                    dsMotivo = null,
                    dtCriacao = new DateTime(2012, 09, 22)
                };

                var gateway = MockRepository.GenerateMock<IRevisaoCustoGateway>();
                gateway.Expect(rcg => rcg.Insert(revisaoCusto));

                var target = new RevisaoCustoService(gateway);

                target.Salvar(revisaoCusto);

                Assert.AreEqual(123, revisaoCusto.IDUsuarioSolicitante);
                Assert.AreNotEqual(new DateTime(2012, 09, 22), revisaoCusto.dtCriacao);

                gateway.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void Salvar_Existente_Alterado()
        {
            var currentContext = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext() { User = new MemoryRuntimeUser { Id = 12 } };

                RevisaoCusto revisaoCusto = new RevisaoCusto
                {
                    IDRevisaoCusto = 1,
                    IDLoja = 1005,
                    IDItemDetalhe = 8022604,
                    IDStatusRevisaoCusto = 1,
                    IDMotivoRevisaoCusto = 1,
                    IDUsuarioSolicitante = 123,
                    dtSolicitacao = new DateTime(2015, 12, 30),
                    vlCustoSolicitado = 0,
                    dsMotivo = null,
                    dtCriacao = new DateTime(2012, 09, 22)
                };

                var gateway = MockRepository.GenerateMock<IRevisaoCustoGateway>();

                gateway.Expect(rcg => rcg.Update(revisaoCusto));

                var target = new RevisaoCustoService(gateway);

                target.Salvar(revisaoCusto);

                Assert.AreEqual(123, revisaoCusto.IDUsuarioSolicitante);
                Assert.AreEqual(new DateTime(2012, 09, 22), revisaoCusto.dtCriacao);

                gateway.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

    }
}
