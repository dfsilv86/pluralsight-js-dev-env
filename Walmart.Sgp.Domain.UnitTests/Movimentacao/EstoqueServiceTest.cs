using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao
{
    [TestFixture]
    [Category("Domain")]
    public class EstoqueServiceTest
    {
        [Test]
        public void ObterUltimoCustoDeItensRelacionadosNaLoja_Principal_Response()
        {
            DateTime ontem = DateTime.Now.AddDays(-1);

            var estoqueGateway = MockRepository.GenerateMock<IEstoqueGateway>();
            var itemRelacionadoGateway = MockRepository.GenerateMock<IItemRelacionamentoGateway>();
            var relacionamentoItemSecundarioGateway = MockRepository.GenerateMock<IRelacionamentoItemSecundarioGateway>();

            itemRelacionadoGateway
                .Expect(g => g.ObterPrincipaisPorItem(1))
                .Return(new RelacionamentoItemPrincipal[] 
                {
                    new RelacionamentoItemPrincipal 
                    { 
                        IDRelacionamentoItemPrincipal = 1,
                        IDItemDetalhe = 1,
                        BlReprocessamentoManual = true, 
                        PcRendimentoReceita = 90, 
                        RelacionamentoSecundario = new RelacionamentoItemSecundario[] 
                        { 
                            new RelacionamentoItemSecundario { IDItemDetalhe = 2 } 
                        } 
                    }
                });

            relacionamentoItemSecundarioGateway
                .Expect(g => g.ObterSecundariosPorItem(1))
                .Return(new RelacionamentoItemPrincipal[0]);

            estoqueGateway.Expect(g => g.ObterUltimoCustoDoItemNaLoja(2, 1)).Return(new CustoMaisRecente(new NotaFiscal { dtRecebimento = ontem, Itens = new NotaFiscalItem[] { new NotaFiscalItem { IDItemDetalhe = 2, vlCusto = 5 } } }, null));

            var target = new EstoqueService(estoqueGateway, itemRelacionadoGateway, relacionamentoItemSecundarioGateway, null, null);

            var result = target.ObterUltimoCustoDeItensRelacionadosNaLoja(1, 1).ToArray();

            estoqueGateway.VerifyAllExpectations();
            itemRelacionadoGateway.VerifyAllExpectations();
            relacionamentoItemSecundarioGateway.VerifyAllExpectations();

            Assert.AreEqual(1, result.Length);

            var item = result.First();

            Assert.IsTrue(item.IsPrincipal);
            Assert.AreEqual(1, item.RelacionamentoPrincipal.IDItemDetalhe);
            Assert.AreEqual(90, item.RelacionamentoPrincipal.PcRendimentoReceita);
            Assert.AreEqual(2, item.RelacionamentoSecundario.IDItemDetalhe);
            Assert.IsNotNull(item.CustoMaisRecente);
            Assert.AreEqual(ontem, item.CustoMaisRecente.NotaFiscal.dtRecebimento);
            Assert.AreEqual(1, item.CustoMaisRecente.NotaFiscal.Itens.Count);
            Assert.AreEqual(2, item.CustoMaisRecente.NotaFiscal.Itens.Single().IDItemDetalhe);
        }

        [Test]
        public void ObterUltimoCustoDeItensRelacionadosNaLoja_Secundario_Response()
        {
            DateTime ontem = DateTime.Now.AddDays(-1);

            var estoqueGateway = MockRepository.GenerateMock<IEstoqueGateway>();
            var itemRelacionadoGateway = MockRepository.GenerateMock<IItemRelacionamentoGateway>();
            var relacionamentoItemSecundarioGateway = MockRepository.GenerateMock<IRelacionamentoItemSecundarioGateway>();

            itemRelacionadoGateway
                .Expect(g => g.ObterPrincipaisPorItem(1))
                .Return(new RelacionamentoItemPrincipal[0]);

            relacionamentoItemSecundarioGateway
                .Expect(g => g.ObterSecundariosPorItem(1))
                .Return(new RelacionamentoItemPrincipal[]
                {
                    new RelacionamentoItemPrincipal 
                    { 
                        IDRelacionamentoItemPrincipal = 1,
                        IDItemDetalhe = 2,
                        BlReprocessamentoManual = true, 
                        PcRendimentoReceita = 90, 
                        RelacionamentoSecundario = new RelacionamentoItemSecundario[] 
                        { 
                            new RelacionamentoItemSecundario { IDItemDetalhe = 1 } 
                        }
                    }
                });

            estoqueGateway.Expect(g => g.ObterUltimoCustoDoItemNaLoja(2, 1)).Return(new CustoMaisRecente(new NotaFiscal { dtRecebimento = ontem, Itens = new NotaFiscalItem[] { new NotaFiscalItem { IDItemDetalhe = 2, vlCusto = 5 } } }, null));
            estoqueGateway.Stub(g => g.ObterUltimoCustoDoItemNaLoja(1, 1)).Throw(new InvalidOperationException("Não deve consultar ultimo custo do item 1."));

            var target = new EstoqueService(estoqueGateway, itemRelacionadoGateway, relacionamentoItemSecundarioGateway, null, null);

            var result = target.ObterUltimoCustoDeItensRelacionadosNaLoja(1, 1).ToArray();

            estoqueGateway.VerifyAllExpectations();
            itemRelacionadoGateway.VerifyAllExpectations();
            relacionamentoItemSecundarioGateway.VerifyAllExpectations();

            Assert.AreEqual(1, result.Length);

            var item = result.First();

            Assert.IsFalse(item.IsPrincipal);
            Assert.AreEqual(2, item.RelacionamentoPrincipal.IDItemDetalhe);
            Assert.AreEqual(1, item.RelacionamentoSecundario.IDItemDetalhe);
            Assert.IsNotNull(item.CustoMaisRecente);
            Assert.AreEqual(ontem, item.CustoMaisRecente.NotaFiscal.dtRecebimento);
            Assert.AreEqual(1, item.CustoMaisRecente.NotaFiscal.Itens.Count);
            Assert.AreEqual(2, item.CustoMaisRecente.NotaFiscal.Itens.Single().IDItemDetalhe);
        }

        [Test]
        public void ObterUltimoCustoDeItensRelacionadosNaLoja_CustosParalelos_Response()
        {
            DateTime ontem = DateTime.Now.AddDays(-1);

            var estoqueGateway = MockRepository.GenerateMock<IEstoqueGateway>();
            var itemRelacionadoGateway = MockRepository.GenerateMock<IItemRelacionamentoGateway>();
            var relacionamentoItemSecundarioGateway = MockRepository.GenerateMock<IRelacionamentoItemSecundarioGateway>();

            itemRelacionadoGateway
                .Expect(g => g.ObterPrincipaisPorItem(1))
                .Return(new RelacionamentoItemPrincipal[] 
                {
                    new RelacionamentoItemPrincipal 
                    { 
                        IDRelacionamentoItemPrincipal = 1,
                        IDItemDetalhe = 1,
                        BlReprocessamentoManual = true, 
                        PcRendimentoReceita = 90, 
                        RelacionamentoSecundario = new RelacionamentoItemSecundario[] 
                        { 
                            new RelacionamentoItemSecundario { IDItemDetalhe = 2 } 
                        } 
                    },
                });

            relacionamentoItemSecundarioGateway
                .Expect(g => g.ObterSecundariosPorItem(1))
                .Return(new RelacionamentoItemPrincipal[]
                {
                    new RelacionamentoItemPrincipal 
                    { 
                        IDRelacionamentoItemPrincipal = 2,
                        IDItemDetalhe = 2,
                        BlReprocessamentoManual = true, 
                        PcRendimentoReceita = 80, 
                        RelacionamentoSecundario = new RelacionamentoItemSecundario[] 
                        { 
                            new RelacionamentoItemSecundario { IDItemDetalhe = 1 } 
                        }
                    },
                    new RelacionamentoItemPrincipal 
                    { 
                        IDRelacionamentoItemPrincipal = 3,
                        IDItemDetalhe = 3,
                        BlReprocessamentoManual = true, 
                        PcRendimentoReceita = 70, 
                        RelacionamentoSecundario = new RelacionamentoItemSecundario[] 
                        { 
                            new RelacionamentoItemSecundario { IDItemDetalhe = 1 } 
                        }
                    },
                });

            estoqueGateway.Stub(g => g.ObterUltimoCustoDoItemNaLoja(1, 1)).Throw(new InvalidOperationException("Não deve consultar ultimo custo do item 1."));

            estoqueGateway.Expect(g => g.ObterUltimoCustoDoItemNaLoja(3, 1)).Return(new CustoMaisRecente(new NotaFiscal { dtRecebimento = ontem, Itens = new NotaFiscalItem[] { new NotaFiscalItem { IDItemDetalhe = 3, vlCusto = 5 } } }, null)).Repeat.Once();
            estoqueGateway.Stub(g => g.ObterUltimoCustoDoItemNaLoja(3, 1)).Throw(new InvalidOperationException("Deve buscar custos do item 1 apenas uma vez"));

            estoqueGateway.Expect(g => g.ObterUltimoCustoDoItemNaLoja(2, 1)).Return(new CustoMaisRecente(new NotaFiscal { dtRecebimento = ontem, Itens = new NotaFiscalItem[] { new NotaFiscalItem { IDItemDetalhe = 2, vlCusto = 6 } } }, null)).Repeat.Once();
            estoqueGateway.Stub(g => g.ObterUltimoCustoDoItemNaLoja(2, 1)).Throw(new InvalidOperationException("Deve buscar custos do item 2 apenas uma vez"));

            var target = new EstoqueService(estoqueGateway, itemRelacionadoGateway, relacionamentoItemSecundarioGateway, null, null);

            var result = target.ObterUltimoCustoDeItensRelacionadosNaLoja(1, 1).ToArray();

            estoqueGateway.VerifyAllExpectations();
            itemRelacionadoGateway.VerifyAllExpectations();
            relacionamentoItemSecundarioGateway.VerifyAllExpectations();

            Assert.AreEqual(3, result.Length);

            var item = result[0];

            Assert.IsTrue(item.IsPrincipal);
            Assert.AreEqual(1, item.RelacionamentoPrincipal.IDRelacionamentoItemPrincipal);
            Assert.AreEqual(1, item.RelacionamentoPrincipal.IDItemDetalhe);
            Assert.AreEqual(2, item.RelacionamentoSecundario.IDItemDetalhe);
            Assert.IsNotNull(item.CustoMaisRecente);
            Assert.AreEqual(ontem, item.CustoMaisRecente.NotaFiscal.dtRecebimento);
            Assert.AreEqual(1, item.CustoMaisRecente.NotaFiscal.Itens.Count);
            Assert.AreEqual(2, item.CustoMaisRecente.NotaFiscal.Itens.Single().IDItemDetalhe);

            item = result[1];

            Assert.IsFalse(item.IsPrincipal);
            Assert.AreEqual(2, item.RelacionamentoPrincipal.IDRelacionamentoItemPrincipal);
            Assert.AreEqual(2, item.RelacionamentoPrincipal.IDItemDetalhe);
            Assert.AreEqual(1, item.RelacionamentoSecundario.IDItemDetalhe);
            Assert.IsNotNull(item.CustoMaisRecente);
            Assert.AreEqual(ontem, item.CustoMaisRecente.NotaFiscal.dtRecebimento);
            Assert.AreEqual(1, item.CustoMaisRecente.NotaFiscal.Itens.Count);
            Assert.AreEqual(2, item.CustoMaisRecente.NotaFiscal.Itens.Single().IDItemDetalhe);

            item = result[2];

            Assert.IsFalse(item.IsPrincipal);
            Assert.AreEqual(3, item.RelacionamentoPrincipal.IDRelacionamentoItemPrincipal);
            Assert.AreEqual(3, item.RelacionamentoPrincipal.IDItemDetalhe);
            Assert.AreEqual(1, item.RelacionamentoSecundario.IDItemDetalhe);
            Assert.IsNotNull(item.CustoMaisRecente);
            Assert.AreEqual(ontem, item.CustoMaisRecente.NotaFiscal.dtRecebimento);
            Assert.AreEqual(1, item.CustoMaisRecente.NotaFiscal.Itens.Count);
            Assert.AreEqual(3, item.CustoMaisRecente.NotaFiscal.Itens.Single().IDItemDetalhe);

            Assert.AreEqual(result[0].CustoMaisRecente, result[1].CustoMaisRecente);
        }

        [Test]
        public void Ajustar_EhErroDeQuebraPDV_DhAtualizacao()
        {
            var target = new EstoqueService(null, null, null, null, null);

            Assert.Catch(() =>
            {
                target.Ajustar(new Estoque
                {
                    MotivoAjuste = new MotivoMovimentacao
                    {
                        Id = MotivoMovimentacao.IDErroDeQuebraPDV
                    },
                    Loja = new Loja(),
                    dhAtualizacao = null
                });
            });
        }

        [Test]
        public void Ajustar_Manipulado_AjusteEstoqueManipulado()
        {
            var estoque = new Estoque
            {
                MotivoAjuste = new MotivoMovimentacao
                {
                    Id = MotivoMovimentacao.IDErroDeQuebraPDV
                },
                dhAtualizacao = DateTime.Now,
                IDItemDetalhe = 11
            };

            var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
            gateway.Expect(e => e.AjusteEstoqueManipulado(estoque));

            var notaFiscalService = MockRepository.GenerateMock<INotaFiscalService>();
            notaFiscalService.Expect(e => e.ObterItemNaUltimaNotaRecebidaDaLoja(2, 1)).Return(null);

            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorId(11)).Return(new ItemDetalhe { TpManipulado = TipoManipulado.Derivado });

            var target = new EstoqueService(gateway, null, null, notaFiscalService, itemDetalheService);

            target.Ajustar(estoque);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void Ajustar_Receituario_AjusteEstoqueReceituario()
        {
            var estoque = new Estoque
            {
                MotivoAjuste = new MotivoMovimentacao
                {
                    Id = 1
                },
                dhAtualizacao = null,
                IDItemDetalhe = 11
            };

            var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
            gateway.Expect(e => e.AjustarEstoqueReceituario(estoque));

            var notaFiscalService = MockRepository.GenerateMock<INotaFiscalService>();
            notaFiscalService.Expect(e => e.ObterItemNaUltimaNotaRecebidaDaLoja(2, 1)).Return(null);

            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorId(11)).Return(new ItemDetalhe { TpReceituario = TipoReceituario.Insumo });

            var target = new EstoqueService(gateway, null, null, notaFiscalService, itemDetalheService);

            target.Ajustar(estoque);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void Ajustar_Receituario_AjusteEstoqueVinculado()
        {
            var estoque = new Estoque
            {
                MotivoAjuste = new MotivoMovimentacao
                {
                    Id = 1
                },
                dhAtualizacao = null,
                IDItemDetalhe = 11
            };

            var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
            gateway.Expect(e => e.AjustarEstoqueDireto(estoque));

            var notaFiscalService = MockRepository.GenerateMock<INotaFiscalService>();
            notaFiscalService.Expect(e => e.ObterItemNaUltimaNotaRecebidaDaLoja(2, 1)).Return(null);

            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorId(11)).Return(new ItemDetalhe { TpManipulado = null, TpVinculado = TipoVinculado.Entrada });

            var target = new EstoqueService(gateway, null, null, notaFiscalService, itemDetalheService);

            target.Ajustar(estoque);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void RealizarMtr_MovimentacaoMtrInvalida_Exception()
        {
            var target = new EstoqueService(null, null, null, null, null);

            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                target.RealizarMtr(new MovimentacaoMtr
                {
                    IdItemDestino = 1,
                    IdLoja = 2,
                    Quantidade = 3
                });
            });

            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                target.RealizarMtr(new MovimentacaoMtr
                {
                    IdItemOrigem = 1,
                    IdLoja = 2,
                    Quantidade = 3
                });
            });

            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                target.RealizarMtr(new MovimentacaoMtr
                {
                    IdItemOrigem = 1,
                    IdItemDestino = 1,
                    Quantidade = 3
                });
            });

            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                target.RealizarMtr(new MovimentacaoMtr
                {
                    IdItemOrigem = 1,
                    IdItemDestino = 1,
                    IdLoja = 2
                });
            });
        }

        [Test]
        public void RealizarMtr_ItemOrigemTipoReceituarioTransformado_AjustarEstoqueReceituario()
        {
            var movimentacaoMtr = new MovimentacaoMtr 
            {
                IdItemOrigem = 1,
                IdItemDestino = 2,
                IdLoja = 3,
                Quantidade = 4                 
            };
            var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
            gateway.Expect(e => e.AjustarEstoqueReceituario(movimentacaoMtr));

            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorId(1)).Return(new ItemDetalhe { TpReceituario = TipoReceituario.Transformado });
            itemDetalheService.Expect(e => e.ObterPorIds(1, 2)).IgnoreArguments().Return(new ItemDetalhe[] { new ItemDetalhe { IDDepartamento = 11 }, new ItemDetalhe { IDDepartamento = 22 } });
            
            var target = new EstoqueService(gateway, null, null, null, itemDetalheService);
            target.RealizarMtr(movimentacaoMtr);

            gateway.VerifyAllExpectations();
            itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void RealizarMtr_ItemOrigemTipoReceituarioTransformado_AjusteEstoqueManipulado()
        {
            var movimentacaoMtr = new MovimentacaoMtr
            {
                IdItemOrigem = 1,
                IdItemDestino = 2,
                IdLoja = 3,
                Quantidade = 4
            };
            var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
            gateway.Expect(e => e.AjusteEstoqueManipulado(movimentacaoMtr));

            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorId(1)).Return(new ItemDetalhe { TpManipulado = TipoManipulado.Derivado });
            itemDetalheService.Expect(e => e.ObterPorIds(1, 2)).IgnoreArguments().Return(new ItemDetalhe[] { new ItemDetalhe { IDDepartamento = 11 }, new ItemDetalhe { IDDepartamento = 22 } });

            var target = new EstoqueService(gateway, null, null, null, itemDetalheService);
            target.RealizarMtr(movimentacaoMtr);

            gateway.VerifyAllExpectations();
            itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void RealizarMtr_ItemOrigemTipoReceituarioTransformado_AjustarEstoqueDireto()
        {
            var movimentacaoMtr = new MovimentacaoMtr
            {
                IdItemOrigem = 1,
                IdItemDestino = 2,
                IdLoja = 3,
                Quantidade = 4
            };
            var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
            gateway.Expect(e => e.AjustarEstoqueDireto(movimentacaoMtr));

            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            itemDetalheService.Expect(e => e.ObterPorId(1)).Return(new ItemDetalhe { TpManipulado = TipoManipulado.Pai });
            itemDetalheService.Expect(e => e.ObterPorIds(1, 2)).IgnoreArguments().Return(new ItemDetalhe[] { new ItemDetalhe { IDDepartamento = 11 }, new ItemDetalhe { IDDepartamento = 22 } });

            var target = new EstoqueService(gateway, null, null, null, itemDetalheService);
            target.RealizarMtr(movimentacaoMtr);

            gateway.VerifyAllExpectations();
            itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void PesquisarUltimoCustoDoItemPorLoja_Args_Resultado()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 3, TipoPermissao = Infrastructure.Framework.Domain.Acessos.TipoPermissao.PorBandeira, RoleName = "Guest", RoleId = 1 } };

            try
            {
                Paging paging = new Paging();

                var gateway = MockRepository.GenerateMock<IEstoqueGateway>();
                gateway.Expect(e => e.PesquisarUltimoCustoDoItemPorLoja(1, 2, 3, Infrastructure.Framework.Domain.Acessos.TipoPermissao.PorBandeira, paging)).Return(new Estoque[] { new Estoque { IDEstoque = 1 } });

                var target = new EstoqueService(gateway, null, null, null, null);

                var actual = target.PesquisarUltimoCustoDoItemPorLoja(1, 2, paging);

                Assert.AreEqual(1, actual.Count());

                gateway.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }

        }
    }
}
