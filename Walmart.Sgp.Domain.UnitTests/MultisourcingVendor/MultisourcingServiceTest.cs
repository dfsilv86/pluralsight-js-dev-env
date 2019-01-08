using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.IO.Excel;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingServiceTest
    {
        [Test]
        public void Excluir_ItemSaidaECentroDistribuicao_ExecutaMetodoDeleteDoGateway()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            multisourcingGateway.Expect(g => g.ObterPorCdItemSaidaEcdCD(1, 1)).Return(new Multisourcing[] {new Multisourcing(){
                IDCD = 1,
                IDMultisourcing = 1,
                IDRelacionamentoItemSecundario = 1
            }});

            multisourcingService.Excluir(1, 1);

            multisourcingGateway.AssertWasCalled(x => x.Delete(1));
        }

        [Test]
        public void Excluir_ItemDetalheCDs_ExecutarMetodoDeleteDoGateway()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            var multisourcing = new ItemDetalheCD
            {
                IDRelacionamentoItemSecundario = 1,
                IDCD = 1
            };

            multisourcingGateway.Expect(q => q.ObterPorRelacionamentoItemSecundarioECD(1, 1)).Return(new Multisourcing()
            {
                IDMultisourcing = 1
            });

            multisourcingService.Excluir(new ItemDetalheCD[] { multisourcing });

            multisourcingGateway.AssertWasCalled(x => x.Delete(1));
        }

        [Test]
        public void SalvarMultisourcing_ItemDetalheCDs_ExecutaMetodoInsertDoGateway()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            itemDetalheService.Expect(g => g.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 1
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(2, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 2
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(3, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 3
            });

            var multisourcing = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 1,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = null,
                vlPercentual = 50,
                IDCD = 1
            };

            var multisourcing2 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 2,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = null,
                vlPercentual = 50,
                IDCD = 1
            };

            var multisourcing3 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 3,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = null,
                vlPercentual = 0,
                IDCD = 1
            };

            multisourcingService.SalvarMultisourcing(new ItemDetalheCD[] { multisourcing, multisourcing2, multisourcing3 }, 1, 1);

            multisourcingGateway.AssertWasCalled(x => x.Insert(new Multisourcing() { }));
        }

        [Test]
        public void SalvarMultisourcing_ItemDetalheCDsComUmVlPercentualNulo_ExcluiMultisourcingRelacionadoAoItemDetalheCDComVlPercentualNulo()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            itemDetalheService.Expect(g => g.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 1
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(2, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 2
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(44, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 44
            });

            var multisourcing2 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 2,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 2,
                IDMultisourcing = 2,
                vlPercentual = null,
                IDCD = 1
            };

            var multisourcing = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 1,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = 1,
                vlPercentual = 50,
                IDCD = 1
            };

            var multisourcing3 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 44,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = 123,
                vlPercentual = 50,
                IDCD = 1
            };

            var m = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 50,
                cdUsuarioAlteracao = 1,
                dtAlteracao = DateTime.Now,
                IDCD = 1,
                IDRelacionamentoItemSecundario = 1
            };

            var m2 = new Multisourcing()
            {
                IDMultisourcing = 2,
                vlPercentual = 50,
                cdUsuarioAlteracao = 1,
                dtAlteracao = DateTime.Now,
                IDCD = 1,
                IDRelacionamentoItemSecundario = 2
            };

            multisourcingGateway.Expect(q => q.ObterPorRelacionamentoItemSecundarioECD(1, 1)).Return(m);
            multisourcingGateway.Expect(q => q.ObterPorRelacionamentoItemSecundarioECD(2, 1)).Return(m2);

            multisourcingGateway.Expect(q => q.Obter(1)).Return(m);
            multisourcingGateway.Expect(q => q.Obter(2)).Return(m2);

            multisourcingService.SalvarMultisourcing(new ItemDetalheCD[] { multisourcing, multisourcing2, multisourcing3 }, 1, 1);

            multisourcingGateway.AssertWasCalled(g => g.Delete(m2.IDMultisourcing));
        }

        [Test]
        public void SalvarMultisourcing_ItemDetalheCDsComUmIDMultisourcingInvalido_NaoAtualizaENaoExcluiMultisourcing()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            itemDetalheService.Expect(g => g.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 1
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(2, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 2
            });

            var multisourcing = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 1,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = 200,
                vlPercentual = 50,
                IDCD = 1
            };

            var m = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 100,
                cdUsuarioAlteracao = 1,
                dtAlteracao = DateTime.Now
            };

            var multisourcing2 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 2,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 2,
                IDMultisourcing = 202,
                vlPercentual = 50,
                IDCD = 1
            };


            multisourcingGateway.Expect(q => q.Obter(1)).Return(m);

            multisourcingService.SalvarMultisourcing(new ItemDetalheCD[] { multisourcing, multisourcing2 }, 1, 1);

            multisourcingGateway.AssertWasNotCalled(g => g.Delete(m.IDMultisourcing));

            multisourcingGateway.AssertWasNotCalled(g => g.Update(m));
        }

        [Test]
        public void SalvarMultisourcing_ItemDetalheCDs_AtualizaOMultisourcingComIDUm()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            itemDetalheService.Expect(g => g.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 1
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(2, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 2
            });

            var multisourcing = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 1,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = 1,
                vlPercentual = 50,
                IDCD = 1
            };

            var multisourcing2 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 2,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 1234589,
                IDItemDetalhe = 2,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 2,
                IDMultisourcing = 31,
                vlPercentual = 50,
                IDCD = 1
            };

            var m1 = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 100,
                cdUsuarioAlteracao = 1,
                dtAlteracao = DateTime.Now
            };

            multisourcingGateway.Expect(q => q.ObterPorRelacionamentoItemSecundarioECD(1, 1)).Return(m1);

            multisourcingGateway.Expect(q => q.Obter(1)).Return(m1);

            multisourcingService.SalvarMultisourcing(new ItemDetalheCD[] { multisourcing, multisourcing2 }, 1, 1);

            multisourcingGateway.AssertWasCalled(x => x.Update(new Multisourcing() { IDMultisourcing = 1 }));
        }

        [Test]
        public void ObterMultisourcingPorId_IdDeUmMultisourcing_MultisourcingComId2016()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            multisourcingGateway
                .Expect(x => x.Obter(2016))
                .Return(new Multisourcing() { Id = 2016 });

            Assert.IsNotNull(multisourcingService.Obter(2016));
        }

        [Test]
        public void SalvarMultisourcings_Multisourcings_InserirMultisourcingsELogarAsInclusoes()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            multisourcingGateway
                .Expect(x => x.ObterPorRelacionamentoItemSecundarioECD(123, 10))
                .Return(null);

            itemDetalheService
                .Expect(x => x.PesquisarItensEntradaPorSaidaCD(500028463, 7471, 1))
                .Return(new ItemDetalheCD[0]);

            var multisourcing = new Multisourcing
            {
                IDMultisourcing = 0,
                IDRelacionamentoItemSecundario = 123,
                CdItemDetalheSaida = 500028463,
                IDCD = 10,
                CD = 7471
            };

            multisourcingService.SalvarMultisourcings(new[] { multisourcing }, 1, 1);

            multisourcingGateway.AssertWasCalled(x => x.Insert(multisourcing));
            logMultiSourcingService.AssertWasCalled(x => x.Logar(TpOperacao.Inclusao, null, multisourcing, "Inclusão de cadastro de multisourcing Id=" + multisourcing.IDMultisourcing));
        }

        [Test]
        public void SalvarMultisourcings_Multisourcings_AtualizarMultisourcingsELogarAsAtualizacoes()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            var oldMultisourcing = new Multisourcing
            {
                IDMultisourcing = 100,
                IDRelacionamentoItemSecundario = 123,
                CdItemDetalheSaida = 500028463,
                IDCD = 10,
                CD = 7471
            };

            multisourcingGateway
                .Expect(x => x.ObterPorRelacionamentoItemSecundarioECD(123, 10))
                .Return(oldMultisourcing);

            itemDetalheService
                .Expect(x => x.PesquisarItensEntradaPorSaidaCD(500028463, 7471, 1))
                .Return(new ItemDetalheCD[0]);

            var newMultisourcing = new Multisourcing
            {
                IDMultisourcing = 0,
                IDRelacionamentoItemSecundario = 123,
                CdItemDetalheSaida = 500028463,
                IDCD = 10,
                CD = 7471
            };

            multisourcingService.SalvarMultisourcings(new[] { newMultisourcing }, 1, 1);

            multisourcingGateway.AssertWasCalled(x => x.Update(newMultisourcing));
            logMultiSourcingService.AssertWasCalled(x => x.Logar(TpOperacao.Alteracao, oldMultisourcing, newMultisourcing, "Alteração de cadastro de multisourcing Id=" + oldMultisourcing.IDMultisourcing));
        }

        [Test]
        public void SalvarMultisourcings_Multisourcings_ExcluirMultisourcingsELogarAsExclusoes()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            var multisourcing = new Multisourcing
            {
                IDMultisourcing = 100,
                IDRelacionamentoItemSecundario = 123,
                CdItemDetalheSaida = 500028463,
                IDCD = 10,
                CD = 7471
            };

            var multisourcingToExclude = new Multisourcing
            {
                IDMultisourcing = 101,
                IDRelacionamentoItemSecundario = 111,
                IDCD = 10
            };

            itemDetalheService
                .Expect(x => x.PesquisarItensEntradaPorSaidaCD(500028463, 7471, 1))
                .Return(new[] 
                {
                    new ItemDetalheCD
                    {
                        IDRelacionamentoItemSecundario = 111,
                        IDCD = 10,
                    },

                    new ItemDetalheCD
                    {
                        IDRelacionamentoItemSecundario = 123,
                        IDCD = 10,
                    }
                });

            multisourcingGateway
                .Expect(x => x.ObterPorRelacionamentoItemSecundarioECD(111, 10))
                .Return(multisourcingToExclude);

            multisourcingService.SalvarMultisourcings(new[] { multisourcing }, 1, 1);

            logMultiSourcingService.AssertWasCalled(x => x.Logar(TpOperacao.Exclusao, multisourcingToExclude, null, "Exclusão de cadastro de multisourcing Id=" + multisourcingToExclude.IDMultisourcing));
            multisourcingGateway.AssertWasCalled(x => x.Delete(101));
        }

        [Test]
        public void SalvarMultisourcing_ItemDetalheCDsComCadastroCompraCasada_InsereMultisourcingsSemItemDetalheCDComCompraCasada()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            itemDetalheService.Expect(g => g.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 1
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(2, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 2
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(3, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 3
            });

            var multisourcing = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 1,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = null,
                vlPercentual = 60,
                IDCD = 1,
                idCompraCasada = null
            };

            var multisourcing2 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 2,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 2,
                IDMultisourcing = null,
                vlPercentual = 40,
                IDCD = 1,
                idCompraCasada = null
            };

            var multisourcing3 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 3,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 3,
                IDMultisourcing = null,
                vlPercentual = 0,
                IDCD = 1,
                idCompraCasada = 1
            };

            var itens = new ItemDetalheCD[] { multisourcing, multisourcing2 };

            multisourcingService.SalvarMultisourcing(itens, 1, 1);

            var parametros = multisourcingGateway.GetArgumentsForCallsMadeOn(x => x.Insert(new Multisourcing()));

            var p1 = (Multisourcing)parametros[0][0];
            var p2 = (Multisourcing)parametros[1][0];

            Assert.AreEqual(multisourcing.IDRelacionamentoItemSecundario, p1.IDRelacionamentoItemSecundario);
            Assert.AreEqual(multisourcing2.IDRelacionamentoItemSecundario, p2.IDRelacionamentoItemSecundario);

        }

        [Test]
        public void SalvarMultisourcing_ItemDetalheCDsComCadastroCompraCasada_AtualizaMultisourcingsSemItemDetalheCDComCompraCasada()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingService = MockRepository.GenerateMock<ILogMultiSourcingService>();
            var itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            var multisourcingService = new MultisourcingService(multisourcingGateway, logMultiSourcingService, itemDetalheService);

            itemDetalheService.Expect(g => g.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 1
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(2, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 2
            });

            itemDetalheService.Expect(g => g.ObterPorItemESistema(3, 1)).Return(new ItemDetalhe()
            {
                IdFornecedorParametro = 3
            });

            var multisourcing = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 1,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 1,
                IDMultisourcing = 1,
                vlPercentual = 60,
                IDCD = 1,
                idCompraCasada = null
            };

            var multisourcing2 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 2,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 2,
                IDMultisourcing = 2,
                vlPercentual = 40,
                IDCD = 1,
                idCompraCasada = null
            };

            var multisourcing3 = new ItemDetalheCD
            {
                cdCD = 1,
                cdItem = 3,
                dsItem = "desc. do item",
                nmFornecedor = "nome do fornecedor",
                cdV9D = 123456789,
                IDItemDetalhe = 1,
                cdTipo = 'D',
                cdItemSaida = 2,
                IDRelacionamentoItemSecundario = 3,
                IDMultisourcing = 3,
                vlPercentual = 0,
                IDCD = 1,
                idCompraCasada = 1
            };

            var itens = new ItemDetalheCD[] { multisourcing, multisourcing2 };

            multisourcingGateway
                .Expect(x => x.Obter(1))
                .Return(new Multisourcing() { Id = 1 });

            var m1 = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 100,
                cdUsuarioAlteracao = 1,
                dtAlteracao = DateTime.Now
            };

            var m2 = new Multisourcing()
            {
                IDMultisourcing = 2,
                vlPercentual = 100,
                cdUsuarioAlteracao = 1,
                dtAlteracao = DateTime.Now
            };

            multisourcingGateway.Expect(q => q.Obter(1)).Return(m1);
            multisourcingGateway.Expect(q => q.ObterPorRelacionamentoItemSecundarioECD(1, 1)).Return(m1);

            multisourcingGateway.Expect(q => q.Obter(2)).Return(m2);
            multisourcingGateway.Expect(q => q.ObterPorRelacionamentoItemSecundarioECD(2, 1)).Return(m2);

            multisourcingService.SalvarMultisourcing(itens, 1, 1);

            var parametros = multisourcingGateway.GetArgumentsForCallsMadeOn(x => x.Update(new Multisourcing()));

            var p1 = (Multisourcing)parametros[0][0];
            var p2 = (Multisourcing)parametros[1][0];

            Assert.AreEqual(multisourcing.IDRelacionamentoItemSecundario, p1.IDMultisourcing);
            Assert.AreEqual(multisourcing2.IDRelacionamentoItemSecundario, p2.IDMultisourcing);

        }
    }
}
