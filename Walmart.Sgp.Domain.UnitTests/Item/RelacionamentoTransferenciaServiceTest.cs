using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    public class RelacionamentoTransferenciaServiceTest
    {
        [Test]
        public void CriarTransferencias_Args_RelacionamentoTransferencia()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            mainGateway.Expect(e => e.PesquisarPorItemDestinoOrigemLojaAtivo(305816, 51554, 63, false)).Return(new List<RelacionamentoTransferencia>());

            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(e => e.ObterEstruturadoPorId(305816)).Return(new ItemDetalhe());

            var logRelacionamentoTransferenciaGateway = MockRepository.GenerateMock<ILogRelacionamentoTransferenciaGateway>();

            var target = new RelacionamentoTransferenciaService(mainGateway, itemDetalheGateway, logRelacionamentoTransferenciaGateway);
            var lojas = new List<Loja>();
            lojas.Add(new Loja
            {
                Id = 63,
                IDLoja = 63,
                blAtivo = true
            });

            target.CriarTransferencia(305816, 51554, lojas.ToArray());
            mainGateway.VerifyAllExpectations();
        }

        [Test]
        public void CriarTransferencias_Args_WithRelacionamentoInativos()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            mainGateway.Expect(e => e.PesquisarPorItemDestinoOrigemLojaAtivo(305816, 51554, 63, false)).Return(new RelacionamentoTransferencia[] { 
                new RelacionamentoTransferencia(),
                new RelacionamentoTransferencia()
            });

            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(e => e.ObterEstruturadoPorId(305816)).Return(new ItemDetalhe());

            var logRelacionamentoTransferenciaGateway = MockRepository.GenerateMock<ILogRelacionamentoTransferenciaGateway>();

            var target = new RelacionamentoTransferenciaService(mainGateway, itemDetalheGateway, logRelacionamentoTransferenciaGateway);
            var lojas = new List<Loja>();
            lojas.Add(new Loja
            {
                Id = 63,
                IDLoja = 63,
                blAtivo = true
            });

            target.CriarTransferencia(305816, 51554, lojas.ToArray());
            mainGateway.VerifyAllExpectations();
        }

        [Test]
        public void RemoverTransferencias_Args_RelacionamentoTransferencia()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            mainGateway.Expect(e => e.ObterEstruturadoPorId(1)).Return(new RelacionamentoTransferencia());
            mainGateway.Expect(e => e.ObterQuantidadePorItemDestino(305816)).Return(1);

            var itemGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemGateway.Expect(e => e.ObterEstruturadoPorId(305816)).Return(new ItemDetalhe());

            var logRelacionamentoTransferenciaGateway = MockRepository.GenerateMock<ILogRelacionamentoTransferenciaGateway>();

            var target = new RelacionamentoTransferenciaService(mainGateway, itemGateway, logRelacionamentoTransferenciaGateway);
            var relacionamentos = new List<RelacionamentoTransferencia>();
            relacionamentos.Add(new RelacionamentoTransferencia
            {
                Id = 1,
                IDRelacionamentoTransferencia = 1,
                IDItemDetalheDestino = 305816,
                IDItemDetalheOrigem = 51554,
                IDLoja = 63,
                dtCriacao = DateTime.Now,
                IDUsuario = 2337,
                blAtivo = true,
                Loja = new Loja
                {
                    IDLoja = 63
                }
            });

            target.RemoverTransferencias(relacionamentos.ToArray());
            mainGateway.VerifyAllExpectations();
        }

        [Test]
        public void RemoverTransferencias_Args_NotRelacionamentos()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            mainGateway.Expect(e => e.ObterEstruturadoPorId(1)).Return(new RelacionamentoTransferencia());
            mainGateway.Expect(e => e.ObterQuantidadePorItemDestino(305816)).Return(0);

            var itemGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemGateway.Expect(e => e.ObterEstruturadoPorId(305816)).Return(new ItemDetalhe());

            var logRelacionamentoTransferenciaGateway = MockRepository.GenerateMock<ILogRelacionamentoTransferenciaGateway>();

            var target = new RelacionamentoTransferenciaService(mainGateway, itemGateway, logRelacionamentoTransferenciaGateway);
            var relacionamentos = new List<RelacionamentoTransferencia>();
            relacionamentos.Add(new RelacionamentoTransferencia
            {
                Id = 1,
                IDRelacionamentoTransferencia = 1,
                IDItemDetalheDestino = 305816,
                IDItemDetalheOrigem = 51554,
                IDLoja = 63,
                dtCriacao = DateTime.Now,
                IDUsuario = 2337,
                blAtivo = true,
                Loja = new Loja
                {
                    IDLoja = 63
                }
            });

            target.RemoverTransferencias(relacionamentos.ToArray());
            mainGateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterQuantidadePorItemDestino_Args_RelacionamentoTransferencia()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            mainGateway.Expect(e => e.ObterQuantidadePorItemDestino(305816)).Return(1);

            var target = new RelacionamentoTransferenciaService(mainGateway, null, null);
            var count = target.ObterQuantidadePorItemDestino(305816);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void PesquisarItensRelacionados_Args_RelacionamentoTransferencia()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            var paging = new Paging();

            mainGateway.Expect(e => e.PesquisarItensRelacionados(305816, paging)).Return(new RelacionamentoTransferencia[] { 
                new RelacionamentoTransferencia(),
                new RelacionamentoTransferencia()
            });

            var target = new RelacionamentoTransferenciaService(mainGateway, null, null);
            var atual = target.PesquisarItensRelacionados(305816, paging);

            Assert.AreEqual(2, atual.Count());
        }

        [Test]
        public void ObterEstruturadoPorId_Args_RelacionamentoTransferencia()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            mainGateway.Expect(e => e.ObterEstruturadoPorId(1)).Return(new RelacionamentoTransferencia());

            var target = new RelacionamentoTransferenciaService(mainGateway, null, null);
            var atual = target.ObterEstruturadoPorId(1);

            Assert.IsNotNull(atual);
        }

        [Test]
        public void PesquisarItensRelacionadosPorCdItemDestino_Args_RelacionamentoTransferencia()
        {
            var mainGateway = MockRepository.GenerateMock<IRelacionamentoTransferenciaGateway>();
            var paging = new Paging();

            mainGateway.Expect(e => e.PesquisarItensRelacionadosPorCdItemDestino(8139586, paging)).Return(new RelacionamentoTransferencia[] { 
                new RelacionamentoTransferencia()
            });

            var target = new RelacionamentoTransferenciaService(mainGateway, null, null);
            var atual = target.PesquisarItensRelacionadosPorCdItemDestino(8139586, paging);

            Assert.AreEqual(1, atual.Count());
        }
    }
}
