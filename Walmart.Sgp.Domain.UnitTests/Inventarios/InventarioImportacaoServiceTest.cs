using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Is = Rhino.Mocks.Constraints.Is;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios
{
    [TestFixture]
    [Category("Domain")]
    public class InventarioImportacaoServiceTest
    {
        [Test]
        public void ObterPrefixosArquivos_TiposFormato_Prefixos()
        {
            IParametroService parametroServiceRtl = MockRepository.GenerateMock<IParametroService>();
            parametroServiceRtl.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Rtl
            });

            IParametroService parametroServicePipe = MockRepository.GenerateMock<IParametroService>();
            parametroServicePipe.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Pipe
            });

            ConfiguracaoArquivosInventario configuracao = new ConfiguracaoArquivosInventario 
            {
                PrefixoArquivoPipe = "Pipe",
                PrefixoArquivoRtl = "Rtl"
            };

            var target = new InventarioImportacaoService(null, parametroServiceRtl, null, null, null, null, null, null, null, null, null, null, configuracao);

            var prefixos = target.ObterPrefixosArquivos().ToArray();

            Assert.IsNotNull(prefixos);
            Assert.AreEqual(1, prefixos.Length);
            Assert.AreEqual("Rtl", prefixos[0]);

            target = new InventarioImportacaoService(null, parametroServicePipe, null, null, null, null, null, null, null, null, null, null, configuracao);

            prefixos = target.ObterPrefixosArquivos().ToArray();

            Assert.IsNotNull(prefixos);
            Assert.AreEqual(1, prefixos.Length);
            Assert.AreEqual("Pipe", prefixos[0]);
        }

        [Test]
        public void ImportarAutomatico_ArquivosRtlSupercenter_Sucesso()
        {
            string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdDepartamento = 1,
                CdSistema = 1,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdDepartamento = 1,
                IdLoja = 1
            };

            IParametroService parametroService = MockRepository.GenerateMock<IParametroService>();
            parametroService.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Rtl,
                qtdDiasArquivoInventarioAtacado = 5,
                qtdDiasArquivoInventarioVarejo = 5,
                dsServidorSmartDiretorio = "/bar",
                dsServidorSmartEndereco = "foo",
                dsServidorSmartNomeUsuario = "hikaru",
                dsServidorSmartSenha = "ichijio"
            });

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            Loja loja = new Loja
            {
                IDLoja = 1,
                cdLoja = 1,
                TipoArquivoInventario = TipoArquivoInventario.Final
            };

            Departamento departamento = new Departamento
            {
                cdDepartamento = 1,
                IDDepartamento = 1,
                blPerecivel = "S"
            };

            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

            ITransferidorArquivosInventario transferidorArquivosInventario = MockRepository.GenerateMock<ITransferidorArquivosInventario>();
            transferidorArquivosInventario.Expect(tai => tai.ObterArquivosViaFtp(request.CdSistema, request.CdDepartamento, inventario.dhInventario, TipoOrigemImportacao.Loja, TipoProcessoImportacao.Automatico, loja)).Return(arquivosLocais);
            transferidorArquivosInventario.Expect(tai => tai.RemoverBackupsAntigos(1, inventario.dhInventario));
            transferidorArquivosInventario.Expect(tai => tai.ExcluirArquivosNaoProcessados(loja.cdLoja, new ArquivoInventario[0])).IgnoreArguments();

            ILeitorArquivoInventario leitorArquivoInventario = MockRepository.GenerateMock<ILeitorArquivoInventario>();
            leitorArquivoInventario.Expect(lai => lai.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, request.CdSistema, arquivosLocais, inventario.dhInventario)).Return(new ArquivoInventario[] 
            {
                new ArquivoInventario(1, arquivosLocais[0], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = true,
                    UltimoCdDepartamentoLido = 1,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 1,
                            DescricaoItem = "Foo",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 456,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                   }
                },
                new ArquivoInventario(1, arquivosLocais[1], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = false,
                    UltimoCdDepartamentoLido = 2,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                    }
                }
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(null);
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.IdDepartamento, null)).Return(new Inventario[] 
            {
                inventario
            });

            InventarioSumario inventarioEstruturado = new InventarioSumario
                {
                    IDInventario = inventario.IDInventario,
                    dhInventario = inventario.dhInventario,
                    Loja = loja,
                    Departamento = departamento,
                    IDLoja = 1,
                };

            inventarioGateway.Expect(ig => ig.ObterEstruturadoPorId(2)).Return(inventarioEstruturado);

            inventarioGateway.Expect(ig => ig.Update(inventarioEstruturado));

            IInventarioItemGateway inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            inventarioItemGateway.Expect(iig => iig.Find(null, null)).IgnoreArguments().Return(new InventarioItemSumario[]
                {
                    new InventarioItemSumario
                    {
                        IDInventario = 2,
                        IDInventarioItem = 1,
                        qtItemInicial = 1,
                        IDItemDetalhe = 10
                    }
                });

            inventarioItemGateway.Expect(ig => ig.Insert((IEnumerable<InventarioItem>)null)).IgnoreArguments();
            inventarioItemGateway.Expect(ig => ig.Update(null)).IgnoreArguments();

            IItemDetalheGateway itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe { IDItemDetalhe = 10, CdItem = 1, DsItem = "Foo" });
            itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(3, 1)).Return(new ItemDetalhe { IDItemDetalhe = 30, CdItem = 3, DsItem = "Bar" });

            ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
            logger.Expect(lg => lg.InserirInventarioCritica(request.IdLoja, Texts.InventoryImportCannotFindItem.With("Nao cadastrado", 2), 9, 2, null, null, inventario.dhInventario));

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, parametroService, bandeiraService, lojaService, leitorArquivoInventario, transferidorArquivosInventario, null, inventarioItemGateway, itemDetalheGateway, logger, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
            Assert.AreEqual(1, result.QtdArquivosTransferidos);
            Assert.AreEqual(1, result.QtdCriticas);

            inventarioGateway.VerifyAllExpectations();
            inventarioItemGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            transferidorArquivosInventario.VerifyAllExpectations();
            leitorArquivoInventario.VerifyAllExpectations();
            parametroService.VerifyAllExpectations();
            itemDetalheGateway.VerifyAllExpectations();
            logger.VerifyAllExpectations();
        }

        [Test]
        public void ImportarAutomatico_ArquivosPipeAtacado_Sucesso()
        {
            string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdDepartamento = 1,
                CdSistema = 2,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdDepartamento = 1,
                IdLoja = 1
            };

            IParametroService parametroService = MockRepository.GenerateMock<IParametroService>();
            parametroService.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Pipe,
                qtdDiasArquivoInventarioAtacado = 5,
                qtdDiasArquivoInventarioVarejo = 5,
                dsServidorSmartDiretorio = "/bar",
                dsServidorSmartEndereco = "foo",
                dsServidorSmartNomeUsuario = "hikaru",
                dsServidorSmartSenha = "ichijio"
            });

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            Loja loja = new Loja
            {
                IDLoja = 1,
                cdLoja = 1,
                TipoArquivoInventario = TipoArquivoInventario.Final,
                cdSistema = 2,
            };

            Departamento departamento = new Departamento
            {
                cdDepartamento = 1,
                IDDepartamento = 1,
                blPerecivel = "S"
            };

            Categoria categoria = new Categoria
            {
                cdCategoria = 1,
                IDCategoria = 2,
                IDDepartamento = 1,
            };

            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

            ITransferidorArquivosInventario transferidorArquivosInventario = MockRepository.GenerateMock<ITransferidorArquivosInventario>();
            transferidorArquivosInventario.Expect(tai => tai.ObterArquivosViaFtp(request.CdSistema, request.CdDepartamento, inventario.dhInventario, TipoOrigemImportacao.Loja, TipoProcessoImportacao.Automatico, loja)).Return(arquivosLocais);
            transferidorArquivosInventario.Expect(tai => tai.RemoverBackupsAntigos(1, inventario.dhInventario));

            ILeitorArquivoInventario leitorArquivoInventario = MockRepository.GenerateMock<ILeitorArquivoInventario>();
            leitorArquivoInventario.Expect(lai => lai.LerArquivosPipe(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, request.CdSistema, request.IdLoja, arquivosLocais, inventario.dhInventario)).Return(new ArquivoInventario[] 
            {
                new ArquivoInventario(1, arquivosLocais[0], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = true,
                    UltimoCdDepartamentoLido = 1,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 1,
                            DescricaoItem = "Foo",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 456,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                   }
                },
                new ArquivoInventario(1, arquivosLocais[1], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = false,
                    UltimoCdDepartamentoLido = 2,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                    }
                }
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(null);
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, null, 2)).Return(new Inventario[] 
            {
                inventario
            });

            InventarioSumario inventarioEstruturado = new InventarioSumario
            {
                IDInventario = inventario.IDInventario,
                dhInventario = inventario.dhInventario,
                Loja = loja,
                Departamento = departamento,
                IDLoja = 1,
            };

            inventarioGateway.Expect(ig => ig.ObterEstruturadoPorId(2)).Return(inventarioEstruturado);

            inventarioGateway.Expect(ig => ig.Update(inventarioEstruturado));

            ICategoriaService categoriaService = MockRepository.GenerateMock<ICategoriaService>();
            categoriaService.Expect(cs => cs.ObterPorCategoriaESistema(1, 1, 2)).Return(categoria);

            IInventarioItemGateway inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            inventarioItemGateway.Expect(iig => iig.Find(null, null)).IgnoreArguments().Return(new InventarioItemSumario[]
                {
                    new InventarioItemSumario
                    {
                        IDInventario = 2,
                        IDInventarioItem = 1,
                        qtItemInicial = 1,
                        IDItemDetalhe = 10
                    }
                });

            inventarioItemGateway.Expect(ig => ig.Insert((IEnumerable<InventarioItem>)null)).IgnoreArguments();
            inventarioItemGateway.Expect(ig => ig.Update(null)).IgnoreArguments();

            IItemDetalheGateway itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(1, 2)).Return(new ItemDetalhe { IDItemDetalhe = 10, CdItem = 1, DsItem = "Foo" });
            itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(3, 2)).Return(new ItemDetalhe { IDItemDetalhe = 30, CdItem = 3, DsItem = "Bar" });

            ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
            logger.Expect(lg => lg.InserirInventarioCritica(request.IdLoja, Texts.InventoryImportCannotFindItem.With("Nao cadastrado", 2), 9, 2, null, null, inventario.dhInventario));

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, parametroService, bandeiraService, lojaService, leitorArquivoInventario, transferidorArquivosInventario, categoriaService, inventarioItemGateway, itemDetalheGateway, logger, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
            Assert.AreEqual(1, result.QtdArquivosTransferidos);
            Assert.AreEqual(1, result.QtdCriticas);

            inventarioGateway.VerifyAllExpectations();
            inventarioItemGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            transferidorArquivosInventario.VerifyAllExpectations();
            leitorArquivoInventario.VerifyAllExpectations();
            parametroService.VerifyAllExpectations();
            itemDetalheGateway.VerifyAllExpectations();
            logger.VerifyAllExpectations();
        }

        [Test]
        public void ImportarAutomatico_ArquivosRtlAtacado_Sucesso()
        {
            string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdDepartamento = 1,
                CdSistema = 2,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdDepartamento = 1,
                IdLoja = 1
            };

            IParametroService parametroService = MockRepository.GenerateMock<IParametroService>();
            parametroService.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Rtl,
                qtdDiasArquivoInventarioAtacado = 5,
                qtdDiasArquivoInventarioVarejo = 5,
                dsServidorSmartDiretorio = "/bar",
                dsServidorSmartEndereco = "foo",
                dsServidorSmartNomeUsuario = "hikaru",
                dsServidorSmartSenha = "ichijio"
            });

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            Loja loja = new Loja
            {
                IDLoja = 1,
                cdLoja = 1,
                TipoArquivoInventario = TipoArquivoInventario.Parcial,
                cdSistema = 2,
            };

            Departamento departamento = new Departamento
            {
                cdDepartamento = 1,
                IDDepartamento = 1,
                blPerecivel = "S"
            };

            Categoria categoria = new Categoria
            {
                cdCategoria = 1,
                IDCategoria = 2,
                IDDepartamento = 1,
            };

            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

            ITransferidorArquivosInventario transferidorArquivosInventario = MockRepository.GenerateMock<ITransferidorArquivosInventario>();
            transferidorArquivosInventario.Expect(tai => tai.ObterArquivosViaFtp(request.CdSistema, request.CdDepartamento, inventario.dhInventario, TipoOrigemImportacao.Loja, TipoProcessoImportacao.Automatico, loja)).Return(arquivosLocais);
            transferidorArquivosInventario.Expect(tai => tai.RemoverBackupsAntigos(1, inventario.dhInventario));

            ILeitorArquivoInventario leitorArquivoInventario = MockRepository.GenerateMock<ILeitorArquivoInventario>();
            leitorArquivoInventario.Expect(lai => lai.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, request.IdLoja, arquivosLocais, TipoArquivoInventario.Parcial, inventario.dhInventario)).Return(new ArquivoInventario[] 
            {
                new ArquivoInventario(1, arquivosLocais[0], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = true,
                    UltimoCdDepartamentoLido = 1,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 1,
                            DescricaoItem = "Foo",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 456,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                   }
                },
                new ArquivoInventario(1, arquivosLocais[1], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = false,
                    UltimoCdDepartamentoLido = 2,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                    }
                }
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(null);
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, null, 2)).Return(new Inventario[] 
            {
                inventario
            });

            InventarioSumario inventarioEstruturado = new InventarioSumario
            {
                IDInventario = inventario.IDInventario,
                dhInventario = inventario.dhInventario,
                Loja = loja,
                Departamento = departamento,
                IDLoja = 1,
            };

            inventarioGateway.Expect(ig => ig.ObterEstruturadoPorId(2)).Return(inventarioEstruturado);

            inventarioGateway.Expect(ig => ig.Update(inventarioEstruturado));

            ICategoriaService categoriaService = MockRepository.GenerateMock<ICategoriaService>();
            categoriaService.Expect(cs => cs.ObterPorCategoriaESistema(1, 1, 2)).Return(categoria);

            IInventarioItemGateway inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            inventarioItemGateway.Expect(iig => iig.Find(null, null)).IgnoreArguments().Return(new InventarioItemSumario[]
                {
                    new InventarioItemSumario
                    {
                        IDInventario = 2,
                        IDInventarioItem = 1,
                        qtItemInicial = 1,
                        IDItemDetalhe = 10
                    }
                });

            inventarioItemGateway.Expect(ig => ig.Insert((IEnumerable<InventarioItem>)null)).IgnoreArguments();
            inventarioItemGateway.Expect(ig => ig.Update(null)).IgnoreArguments();

            IItemDetalheGateway itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(1, 2)).Return(new ItemDetalhe { IDItemDetalhe = 10, CdItem = 1, DsItem = "Foo" });
            itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(3, 2)).Return(new ItemDetalhe { IDItemDetalhe = 30, CdItem = 3, DsItem = "Bar" });

            ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
            logger.Expect(lg => lg.InserirInventarioCritica(request.IdLoja, Texts.InventoryImportCannotFindItem.With("Nao cadastrado", 2), 9, 2, null, null, inventario.dhInventario));

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, parametroService, bandeiraService, lojaService, leitorArquivoInventario, transferidorArquivosInventario, categoriaService, inventarioItemGateway, itemDetalheGateway, logger, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
            Assert.AreEqual(1, result.QtdArquivosTransferidos);
            Assert.AreEqual(1, result.QtdCriticas);

            inventarioGateway.VerifyAllExpectations();
            inventarioItemGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            transferidorArquivosInventario.VerifyAllExpectations();
            leitorArquivoInventario.VerifyAllExpectations();
            parametroService.VerifyAllExpectations();
            itemDetalheGateway.VerifyAllExpectations();
            logger.VerifyAllExpectations();
        }

        [Test]
        public void ImportarAutomatico_ArquivosDepartamentoNaoPerecivel_Critica()
        {
            string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdDepartamento = 1,
                CdSistema = 1,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdDepartamento = 1,
                IdLoja = 1
            };

            IParametroService parametroService = MockRepository.GenerateMock<IParametroService>();
            parametroService.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Rtl,
                qtdDiasArquivoInventarioAtacado = 5,
                qtdDiasArquivoInventarioVarejo = 5,
                dsServidorSmartDiretorio = "/bar",
                dsServidorSmartEndereco = "foo",
                dsServidorSmartNomeUsuario = "hikaru",
                dsServidorSmartSenha = "ichijio"
            });

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            Loja loja = new Loja
            {
                IDLoja = 1,
                cdLoja = 1,
                TipoArquivoInventario = TipoArquivoInventario.Final
            };

            Departamento departamento = new Departamento
            {
                cdDepartamento = 1,
                IDDepartamento = 1,
            };

            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

            ITransferidorArquivosInventario transferidorArquivosInventario = MockRepository.GenerateMock<ITransferidorArquivosInventario>();
            transferidorArquivosInventario.Expect(tai => tai.ObterArquivosViaFtp(request.CdSistema, request.CdDepartamento, inventario.dhInventario, TipoOrigemImportacao.Loja, TipoProcessoImportacao.Automatico, loja)).Return(arquivosLocais);

            ILeitorArquivoInventario leitorArquivoInventario = MockRepository.GenerateMock<ILeitorArquivoInventario>();
            leitorArquivoInventario.Expect(lai => lai.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, request.CdSistema, arquivosLocais, inventario.dhInventario)).Return(new ArquivoInventario[] 
            {
                new ArquivoInventario(1, arquivosLocais[0], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = true,
                    UltimoCdDepartamentoLido = 1,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 1,
                            DescricaoItem = "Foo",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 456,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                   }
                },
                new ArquivoInventario(1, arquivosLocais[1], DateTime.Today)
                {
                    DataArquivo = DateTime.Today,
                    IsArquivoValido = false,
                    UltimoCdDepartamentoLido = 2,
                    CdLoja = 1,
                    Itens = new ArquivoInventarioItem[]
                    {
                        new ArquivoInventarioItem
                        {
                            CdItem = 3,
                            DescricaoItem = "Bar",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                        new ArquivoInventarioItem
                        {
                            CdItem = 2,
                            DescricaoItem = "Nao cadastrado",
                            QtItem = 123,
                            CustoUnitario = 12m,
                            UltimaContagem = DateTime.Today.AddHours(2)
                        },
                    }
                }
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(null);
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.IdDepartamento, null)).Return(new Inventario[] 
            {
                inventario
            });

            InventarioSumario inventarioEstruturado = new InventarioSumario
            {
                IDInventario = inventario.IDInventario,
                dhInventario = inventario.dhInventario,
                Loja = loja,
                Departamento = departamento,
                IDLoja = 1,
            };

            inventarioGateway.Expect(ig => ig.ObterEstruturadoPorId(2)).Return(inventarioEstruturado);

            ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
            logger.Expect(lg => lg.InserirInventarioCritica(request.IdLoja, Texts.FileDepartmentIsNotPerishable.With(1, arquivosLocais[0]), 5, 2, null, null, inventario.dhInventario));

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, parametroService, bandeiraService, lojaService, leitorArquivoInventario, transferidorArquivosInventario, null, null, null, logger, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.AreEqual(1, result.QtdCriticas);
            Assert.AreEqual(1, result.QtdArquivosUsados);
            Assert.AreEqual(1, result.QtdArquivosTransferidos);
            Assert.IsTrue(result.Sucesso);

            inventarioGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            transferidorArquivosInventario.VerifyAllExpectations();
            leitorArquivoInventario.VerifyAllExpectations();
            parametroService.VerifyAllExpectations();
            logger.VerifyAllExpectations();
        }

        [Test]
        public void ImportarAutomatico_InventarioExisteCdDepartamentoInformado_SemSucesso()
        {
            string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdDepartamento = 1,
                CdSistema = 1,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdDepartamento = 1,
                IdLoja = 1
            };

            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = false
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.IdDepartamento, null)).Return(new Inventario[]
            {
                inventario
            });
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(new Inventario
            {
                stInventario = InventarioStatus.Finalizado
            });

            ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
            logger.Expect(il => il.InserirInventarioCritica(request.IdLoja, Texts.ImportLogErrorAlreadyExists, 2, null, null, null, inventario.dhInventario));

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, null, bandeiraService, null, null, null, null, null, null, logger, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.AreEqual(Texts.ImportLogErrorAlreadyExistsStatusDept.With(request.CdDepartamento, InventarioStatus.Finalizado.Description), result.Mensagem);

            logger.VerifyAllExpectations();
            inventarioGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            Assert.IsFalse(result.Sucesso);
        }

        [Test]
        public void ImportarAutomatico_InventarioExiste_SemSucesso()
        {
            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdSistema = 1,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdLoja = 1
            };

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.IdDepartamento, null)).Return(new Inventario[] 
            {
                inventario
            });
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(new Inventario
            {
                stInventario = InventarioStatus.Finalizado
            });

            ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
            logger.Expect(il => il.InserirInventarioCritica(request.IdLoja, Texts.ImportLogErrorAlreadyExists, 2, null, null, null, inventario.dhInventario));

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, null, bandeiraService, null, null, null, null, null, null, logger, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.AreEqual(Texts.ImportLogErrorAlreadyExistsStatus.With(InventarioStatus.Finalizado.Description), result.Mensagem);

            logger.VerifyAllExpectations();
            inventarioGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            Assert.IsFalse(result.Sucesso);
        }

        [Test]
        public void ImportarAutomatico_NenhumArquivoValido_SucessoZeroArquivosEncontrados()
        {
            string[] arquivosLocais = new string[] { "arquivo invalido" };

            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdSistema = 1,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdLoja = 1
            };

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            Loja loja = new Loja
            {
                IDLoja = 1,
                cdLoja = 1,
                TipoArquivoInventario = TipoArquivoInventario.Final
            };

            Inventario inventario = new Inventario
            {
                IDInventario = 2,
                dhInventario = DateTime.Today.AddDays(1)
            };

            ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
            lojaService.Expect(ls => ls.ObterPorId(request.IdLoja)).Return(loja);

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.IdDepartamento, null)).Return(new Inventario[]
            {
                inventario
            });
            inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(request)).Return(null);

            ITransferidorArquivosInventario transferidorArquivosInventario = MockRepository.GenerateMock<ITransferidorArquivosInventario>();
            transferidorArquivosInventario.Expect(tai => tai.ObterArquivosViaFtp(request.CdSistema, request.CdDepartamento, inventario.dhInventario, TipoOrigemImportacao.Loja, TipoProcessoImportacao.Automatico, loja)).Return(arquivosLocais);

            transferidorArquivosInventario.Expect(tai => tai.ExcluirArquivosNaoProcessados(loja.cdLoja, new ArquivoInventario[0])).IgnoreArguments();

            IParametroService parametroService = MockRepository.GenerateMock<IParametroService>();
            parametroService.Expect(ps => ps.Obter()).Return(new Parametro
            {
                TpArquivoInventario = TipoFormatoArquivoInventario.Rtl,
                qtdDiasArquivoInventarioAtacado = 5,
                qtdDiasArquivoInventarioVarejo = 5,
                dsServidorSmartDiretorio = "/bar",
                dsServidorSmartEndereco = "foo",
                dsServidorSmartNomeUsuario = "hikaru",
                dsServidorSmartSenha = "ichijio"
            });

            ILeitorArquivoInventario leitorArquivoInventario = MockRepository.GenerateMock<ILeitorArquivoInventario>();
            leitorArquivoInventario.Expect(lai => lai.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 1, arquivosLocais, inventario.dhInventario)).Return(new ArquivoInventario[0]);

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, parametroService, bandeiraService, lojaService, leitorArquivoInventario, transferidorArquivosInventario, null, null, null, null, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.AreEqual(Texts.ImportLogSuccessNoFiles, result.Mensagem);
            Assert.IsTrue(result.Sucesso);

            inventarioGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
            transferidorArquivosInventario.VerifyAllExpectations();
            leitorArquivoInventario.VerifyAllExpectations();
            parametroService.VerifyAllExpectations();
            lojaService.VerifyAllExpectations();
        }

        [Test]
        public void ImportarAutomatico_NenhumInventarioAberto_SemSucesso()
        {
            ImportarInventarioAutomaticoRequest request = new ImportarInventarioAutomaticoRequest
            {
                CdSistema = 1,
                DataInventario = DateTime.Today,
                IdBandeira = 1,
                IdLoja = 1
            };

            IBandeiraService bandeiraService = MockRepository.GenerateMock<IBandeiraService>();
            bandeiraService.Expect(bs => bs.ObterPorId(1)).Return(new Bandeira
            {
                BlImportarTodos = true
            });

            IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.IdDepartamento, null)).Return(new Inventario[0]);

            InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, null, bandeiraService, null, null, null, null, null, null, null, null, null, null);

            var result = target.ImportarAutomatico(request, TipoOrigemImportacao.Loja);

            Assert.IsNotNull(result);
            Assert.AreEqual(Texts.ImportLogNoSchedule, result.Mensagem);
            Assert.IsFalse(result.Sucesso);

            inventarioGateway.VerifyAllExpectations();
            bandeiraService.VerifyAllExpectations();
        }

        [Test]
        public void ImportarManual_Arquivos_Sucesso()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;

            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    IsAdministrator = true,
                    IsGa = true,
                    Id = 1
                };

                FileVaultTicket[] tickets = new FileVaultTicket[] { FileVaultTicket.Create("spi28.valido.Z"), FileVaultTicket.Create("spi28.invalido.Z") };
                string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

                ImportarInventarioManualRequest request = new ImportarInventarioManualRequest
                {
                    CdSistema = 1,
                    DataInventario = DateTime.Today,
                    IdBandeira = 1,
                    IdLoja = 1,
                    Arquivos = tickets
                };

                IParametroService parametroService = MockRepository.GenerateMock<IParametroService>();
                parametroService.Expect(ps => ps.Obter()).Return(new Parametro
                {
                    TpArquivoInventario = TipoFormatoArquivoInventario.Rtl,
                    qtdDiasArquivoInventarioAtacado = 5,
                    qtdDiasArquivoInventarioVarejo = 5,
                    dsServidorSmartDiretorio = "/bar",
                    dsServidorSmartEndereco = "foo",
                    dsServidorSmartNomeUsuario = "hikaru",
                    dsServidorSmartSenha = "ichijio"
                });

                Loja loja = new Loja
                {
                    IDLoja = 1,
                    cdLoja = 1,
                    TipoArquivoInventario = TipoArquivoInventario.Final
                };

                Departamento departamento = new Departamento
                {
                    cdDepartamento = 1,
                    IDDepartamento = 1,
                    blPerecivel = "S"
                };

                ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
                lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

                ITransferidorArquivosInventario transferidorArquivosInventario = MockRepository.GenerateMock<ITransferidorArquivosInventario>();
                transferidorArquivosInventario.Expect(tai => tai.RemoverBackupsAntigos(1, DateTime.Today));
                transferidorArquivosInventario.Expect(tai => tai.CopiarArquivosParaImportar(tickets, loja)).Return(arquivosLocais);

                ILeitorArquivoInventario leitorArquivoInventario = MockRepository.GenerateMock<ILeitorArquivoInventario>();
                leitorArquivoInventario.Expect(lai => lai.LerArquivosRtlSupercenter(TipoProcessoImportacao.Manual, TipoOrigemImportacao.HO, request.CdSistema, arquivosLocais, DateTime.Today)).Return(new ArquivoInventario[] 
                {
                    new ArquivoInventario(1, arquivosLocais[0], DateTime.Today)
                    {
                        DataArquivo = DateTime.Today,
                        IsArquivoValido = true,
                        UltimoCdDepartamentoLido = 1,
                        CdLoja = 1,
                        Itens = new ArquivoInventarioItem[]
                        {
                            new ArquivoInventarioItem
                            {
                                CdItem = 1,
                                DescricaoItem = "Foo",
                                QtItem = 123,
                                CustoUnitario = 12m,
                                UltimaContagem = DateTime.Today.AddHours(2)
                            },
                            new ArquivoInventarioItem
                            {
                                CdItem = 2,
                                DescricaoItem = "Nao cadastrado",
                                QtItem = 123,
                                CustoUnitario = 12m,
                                UltimaContagem = DateTime.Today.AddHours(2)
                            },
                            new ArquivoInventarioItem
                            {
                                CdItem = 3,
                                DescricaoItem = "Bar",
                                QtItem = 456,
                                CustoUnitario = 12m,
                                UltimaContagem = DateTime.Today.AddHours(2)
                            },
                       }
                    },
                    new ArquivoInventario(1, arquivosLocais[1], DateTime.Today)
                    {
                        DataArquivo = DateTime.Today,
                        IsArquivoValido = false,
                        UltimoCdDepartamentoLido = 2,
                        CdLoja = 1,
                        Itens = new ArquivoInventarioItem[]
                        {
                            new ArquivoInventarioItem
                            {
                                CdItem = 3,
                                DescricaoItem = "Bar",
                                QtItem = 123,
                                CustoUnitario = 12m,
                                UltimaContagem = DateTime.Today.AddHours(2)
                            },
                            new ArquivoInventarioItem
                            {
                                CdItem = 2,
                                DescricaoItem = "Nao cadastrado",
                                QtItem = 123,
                                CustoUnitario = 12m,
                                UltimaContagem = DateTime.Today.AddHours(2)
                            },
                        }
                    }
                });

                IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
                inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(null)).IgnoreArguments().Return(null);
                inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, request.DataInventario, null, null)).Return(new Inventario[] 
                {
                    new Inventario { IDInventario = 2, dhInventario = DateTime.Today.AddDays(1) }
                });
                inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, null, null, null)).Return(new Inventario[] 
                {
                    new Inventario { IDInventario = 2, dhInventario = DateTime.Today.AddDays(1) }
                });

                InventarioSumario inventario = new InventarioSumario
                {
                    IDInventario = 2,
                    dhInventario = DateTime.Today.AddDays(1),
                    Loja = loja,
                    Departamento = departamento,
                    IDLoja = 1,
                };

                inventarioGateway.Expect(ig => ig.ObterEstruturadoPorId(2)).Return(inventario);

                inventarioGateway.Expect(ig => ig.Update(inventario));

                IInventarioItemGateway inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
                inventarioItemGateway.Expect(iig => iig.Find(null, null)).IgnoreArguments().Return(new InventarioItemSumario[]
                {
                    new InventarioItemSumario
                    {
                        IDInventario = 2,
                        IDInventarioItem = 1,
                        qtItemInicial = 1,
                        IDItemDetalhe = 10
                    }
                });

                inventarioItemGateway.Expect(ig => ig.Insert((IEnumerable<InventarioItem>)null)).IgnoreArguments();
                inventarioItemGateway.Expect(ig => ig.Update(null)).IgnoreArguments();

                IItemDetalheGateway itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
                itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(1, 1)).Return(new ItemDetalhe { IDItemDetalhe = 10, CdItem = 1, DsItem = "Foo" });
                itemDetalheGateway.Expect(idg => idg.ObterPorItemESistema(3, 1)).Return(new ItemDetalhe { IDItemDetalhe = 30, CdItem = 3, DsItem = "Bar" });

                ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
                logger.Expect(lg => lg.InserirInventarioCritica(request.IdLoja, Texts.InventoryImportCannotFindItem.With("Nao cadastrado", 2), 9, 2, null, null, DateTime.Today));

                IPermissaoService permissaoService = MockRepository.GenerateMock<IPermissaoService>();
                permissaoService.Expect(ps => ps.PossuiPermissaoLoja(runtimeContext.User.Id, request.IdLoja)).Return(true);

                Departamento[] departamentos = new Departamento[] { departamento, new Departamento { cdDepartamento = 2 } };

                IDepartamentoService departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
                departamentoService.Expect(ds => ds.PesquisarPorDivisaoESistema(null, null, true, null, request.CdSistema, null)).Constraints(Is.Null(), Is.Null(), Is.Equal(true), Is.Null(), Is.Equal(request.CdSistema), Is.Anything()).Return(departamentos);

                InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, parametroService, null, lojaService, leitorArquivoInventario, transferidorArquivosInventario, null, inventarioItemGateway, itemDetalheGateway, logger, permissaoService, departamentoService, null);

                var result = target.ImportarManual(request, TipoOrigemImportacao.HO);

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Sucesso);
                Assert.AreEqual(1, result.QtdArquivosTransferidos);
                Assert.AreEqual(1, result.QtdCriticas);

                inventarioGateway.VerifyAllExpectations();
                inventarioItemGateway.VerifyAllExpectations();
                transferidorArquivosInventario.VerifyAllExpectations();
                leitorArquivoInventario.VerifyAllExpectations();
                parametroService.VerifyAllExpectations();
                itemDetalheGateway.VerifyAllExpectations();
                logger.VerifyAllExpectations();
                permissaoService.VerifyAllExpectations();
                lojaService.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void ImportarManual_SemPermissao_Exception()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;

            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    IsAdministrator = true,
                    IsGa = true,
                    Id = 1
                };

                FileVaultTicket[] tickets = new  FileVaultTicket[] { FileVaultTicket.Create("spi28.valido.Z"), FileVaultTicket.Create("spi28.invalido.Z") };
                string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

                ImportarInventarioManualRequest request = new ImportarInventarioManualRequest
                {
                    CdSistema = 1,
                    DataInventario = DateTime.Today,
                    IdBandeira = 1,
                    IdLoja = 1,
                    Arquivos = tickets
                };

                Loja loja = new Loja
                {
                    IDLoja = 1,
                    cdLoja = 1,
                    TipoArquivoInventario = TipoArquivoInventario.Final,
                    nmLoja = "Teste"
                };

                ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
                lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

                IPermissaoService permissaoService = MockRepository.GenerateMock<IPermissaoService>();
                permissaoService.Expect(ps => ps.PossuiPermissaoLoja(runtimeContext.User.Id, request.IdLoja)).Return(false);

                InventarioImportacaoService target = new InventarioImportacaoService(null, null, null, lojaService, null, null, null, null, null, null, permissaoService, null, null);

                Assert.Throws<UserInvalidOperationException>(() =>
                {
                    var result = target.ImportarManual(request, TipoOrigemImportacao.HO);
                });

                lojaService.VerifyAllExpectations();
                permissaoService.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void ImportarManual_NenhumInventarioAberto_NenhumDepartamentoParaImportar()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;

            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    IsAdministrator = true,
                    IsGa = true,
                    Id = 1
                };

                FileVaultTicket[] tickets = new  FileVaultTicket[] { FileVaultTicket.Create("spi28.valido.Z"), FileVaultTicket.Create("spi28.invalido.Z") };
                string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

                ImportarInventarioManualRequest request = new ImportarInventarioManualRequest
                {
                    CdSistema = 1,
                    DataInventario = DateTime.Today,
                    IdBandeira = 1,
                    IdLoja = 1,
                    Arquivos = tickets
                };

                Loja loja = new Loja
                {
                    IDLoja = 1,
                    cdLoja = 1,
                    TipoArquivoInventario = TipoArquivoInventario.Final,
                    nmLoja = "Teste"
                };

                Departamento departamento = new Departamento
                {
                    cdDepartamento = 1,
                    IDDepartamento = 1,
                    blPerecivel = "S"
                };

                ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
                lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

                IPermissaoService permissaoService = MockRepository.GenerateMock<IPermissaoService>();
                permissaoService.Expect(ps => ps.PossuiPermissaoLoja(runtimeContext.User.Id, request.IdLoja)).Return(true);

                Departamento[] departamentos = new Departamento[] { departamento, new Departamento { cdDepartamento = 2 } };

                IDepartamentoService departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
                departamentoService.Expect(ds => ds.PesquisarPorDivisaoESistema(null, null, true, null, request.CdSistema, null)).Constraints(Is.Null(), Is.Null(), Is.Equal(true), Is.Null(), Is.Equal(request.CdSistema), Is.Anything()).Return(departamentos);

                IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
                inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, request.DataInventario, null, null)).Return(new Inventario[0]);

                InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, null, null, lojaService, null, null, null, null, null, null, permissaoService, departamentoService, null);

                var result = target.ImportarManual(request, TipoOrigemImportacao.HO);

                Assert.IsFalse(result.Sucesso);
                Assert.AreEqual(Texts.CannotImportAnyDepartment, result.Mensagem);

                lojaService.VerifyAllExpectations();
                permissaoService.VerifyAllExpectations();
                departamentoService.VerifyAllExpectations();
                inventarioGateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void ImportarManual_InventarioFinalizadoMesmaData_NenhumDepartamentoParaImportar()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;

            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    IsAdministrator = true,
                    IsGa = true,
                    Id = 1
                };

                FileVaultTicket[] tickets = new  FileVaultTicket[] { FileVaultTicket.Create("spi28.valido.Z"), FileVaultTicket.Create("spi28.invalido.Z") };
                string[] arquivosLocais = new string[] { "spi28.valido.Z", "spi28.invalido.Z" };

                ImportarInventarioManualRequest request = new ImportarInventarioManualRequest
                {
                    CdSistema = 1,
                    DataInventario = DateTime.Today,
                    IdBandeira = 1,
                    IdLoja = 1,
                    Arquivos = tickets
                };

                Loja loja = new Loja
                {
                    IDLoja = 1,
                    cdLoja = 1,
                    TipoArquivoInventario = TipoArquivoInventario.Final,
                    nmLoja = "Teste"
                };

                Departamento departamento = new Departamento
                {
                    cdDepartamento = 1,
                    IDDepartamento = 1,
                    blPerecivel = "S"
                };

                ILojaService lojaService = MockRepository.GenerateMock<ILojaService>();
                lojaService.Expect(ls => ls.ObterPorId(1)).Return(loja);

                IPermissaoService permissaoService = MockRepository.GenerateMock<IPermissaoService>();
                permissaoService.Expect(ps => ps.PossuiPermissaoLoja(runtimeContext.User.Id, request.IdLoja)).Return(true);

                Departamento[] departamentos = new Departamento[] { departamento, new Departamento { cdDepartamento = 2 } };

                IDepartamentoService departamentoService = MockRepository.GenerateMock<IDepartamentoService>();
                departamentoService.Expect(ds => ds.PesquisarPorDivisaoESistema(null, null, true, null, request.CdSistema, null)).Constraints(Is.Null(), Is.Null(), Is.Equal(true), Is.Null(), Is.Equal(request.CdSistema), Is.Anything()).Return(departamentos);

                IInventarioGateway inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
                inventarioGateway.Expect(ig => ig.ObterInventariosAbertosParaImportacao(request.IdLoja, request.DataInventario, null, null)).Return(new Inventario[] 
                {
                    new Inventario { IDInventario = 2, dhInventario = DateTime.Today.AddDays(1) }
                });
                inventarioGateway.Expect(ig => ig.ObterInventarioAprovadoFinalizadoMesmaData(null)).IgnoreArguments().Return(new Inventario
                {
                    stInventario = InventarioStatus.Finalizado
                });

                ILeitorLogger logger = MockRepository.GenerateMock<ILeitorLogger>();
                logger.Expect(lg => lg.InserirInventarioCritica(request.IdLoja, Texts.ImportLogErrorAlreadyExists, 2, null, null, null, DateTime.Today));

                InventarioImportacaoService target = new InventarioImportacaoService(inventarioGateway, null, null, lojaService, null, null, null, null, null, logger, permissaoService, departamentoService, null);

                var result = target.ImportarManual(request, TipoOrigemImportacao.HO);

                Assert.IsFalse(result.Sucesso);
                Assert.AreEqual(Texts.CannotImportAnyDepartment, result.Mensagem);

                lojaService.VerifyAllExpectations();
                permissaoService.VerifyAllExpectations();
                departamentoService.VerifyAllExpectations();
                inventarioGateway.VerifyAllExpectations();
                logger.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }
    }
}
