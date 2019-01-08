using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.CompraCasada;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain"), Category("CompraCasada")]
    public class CompraCasadaServiceTest
    {
        [Test]
        public void RemoveVinculoItemPai_ItemPaiVinculado_VinculoRemovido()
        {
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var relacaoItemLojaCDService = MockRepository.GenerateMock<IRelacaoItemLojaCDService>();
            var auditGateway = MockRepository.GenerateMock<IAuditGateway>();
            var auditService = new AuditService(auditGateway);

            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            compraCasadaGateway.Expect(g => g.Find(null, null)).IgnoreArguments().Return(new[] { new CompraCasada() { blItemPai = true, blAtivo = true } });

            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, auditService);
            compraCasadaService.RemoverVinculoItemPai(new ItemDetalhe()
            {
                PaiCompraCasada = false,
                IDItemDetalhe = 1,
                FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() { IDFornecedorParametro = 1 },
                ItemSaida = new ItemDetalhe() { IDItemDetalhe = 2 }
            });

            relacaoItemLojaCDService.AssertWasCalled(g => g.RemoverRelacionamentoPorItemEntrada(1, true));
        }

        [Test]
        public void SalvarItensCompraCasada_ItensCompraCasadaSemPai_MensagemErro()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);

            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();

            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe()
                { 
                    PaiCompraCasada = false, 
                    IDItemDetalhe = 1,
                    QtVendorPackage = 2,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            var result = compraCasadaService.SalvarItensCompraCasada(filtro);

            Assert.IsNotNull(result);
        }

        [Test]
        public void SalvarItensCompraCasada_ItensCompraCasada_SomaPackVendorNaoFecha()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(5, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });

            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itemExistente = new ItemDetalhe()
            {
                FilhoCompraCasada = null,
                PaiCompraCasada = null,
                CdSistema = 1,
                IDItemDetalhe = 5,
                ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 },
                IDCompraCasada = 1,
                FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() { IDFornecedorParametro = 1 },
                VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97,
                VlCustoUnitario = 1,
                QtVendorPackage = 1,
                Traits = 1
            };

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe()
                { 
                    PaiCompraCasada = true, 
                    FilhoCompraCasada = false,
                    QtVendorPackage = 6,
                    IDItemDetalhe = 1,
                    CdSistema = 1,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                { 
                    FilhoCompraCasada = true, 
                    PaiCompraCasada = false,
                    QtVendorPackage =1,
                    CdSistema = 1,
                    IDItemDetalhe = 3,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                {
                    FilhoCompraCasada = true,
                    PaiCompraCasada = false,
                    CdSistema = 1,
                    IDItemDetalhe = 5,
                    ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 },
                    IDCompraCasada = 1,
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() { IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97,
                    VlCustoUnitario = 1,
                    QtVendorPackage = 1,
                    Traits = 1
                },
                itemExistente
            };

            var compraCasadaExistente = new CompraCasada()
            {
                blAtivo = true,
                blItemPai = false,
                IDCompraCasada = 1,
                IDFornecedorParametro = 1,
                IDItemDetalheEntrada = 5,
                IDItemDetalheSaida = 1
            };

            compraCasadaGateway.Expect(g => g.FindById(1)).Return(compraCasadaExistente);

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens, blPossuiCadastro = true };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            var result = compraCasadaService.SalvarItensCompraCasada(filtro);

            Assert.IsNotNull(result);
        }

        [Test]
        public void SalvarItensCompraCasada_ItensCompraCasada_RemoveuLogicamenteItemExistente()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(0)).IgnoreArguments().Return(new LojaCdParametro() { IDLojaCDParametro = 0, IDLoja = 1, IDCD = 1 });
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(5, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });

            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itemExistente = new ItemDetalhe()
            {
                FilhoCompraCasada = null,
                PaiCompraCasada = null,
                CdSistema = 1,
                IDItemDetalhe = 5,
                ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 },
                IDCompraCasada = 1,
                FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() { IDFornecedorParametro = 1 },
                VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97,
                VlCustoUnitario = 1,
                QtVendorPackage = 1,
                Traits = 1
            };

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe()
                { 
                    PaiCompraCasada = true, 
                    FilhoCompraCasada = false,
                    QtVendorPackage = 2,
                    IDItemDetalhe = 1,
                    CdSistema = 1,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                { 
                    FilhoCompraCasada = true, 
                    PaiCompraCasada = false,
                    QtVendorPackage =1,
                    CdSistema = 1,
                    IDItemDetalhe = 3,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                {
                    FilhoCompraCasada = true,
                    PaiCompraCasada = false,
                    CdSistema = 1,
                    IDItemDetalhe = 5,
                    ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 },
                    IDCompraCasada = 1,
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() { IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97,
                    VlCustoUnitario = 1,
                    QtVendorPackage = 1,
                    Traits = 1
                },
                itemExistente
            };

            var compraCasadaExistente = new CompraCasada()
            {
                blAtivo = true,
                blItemPai = false,
                IDCompraCasada = 1,
                IDFornecedorParametro = 1,
                IDItemDetalheEntrada = 5,
                IDItemDetalheSaida = 1
            };

            compraCasadaGateway.Expect(g => g.FindById(1)).Return(compraCasadaExistente);

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens, blPossuiCadastro = true };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            relacaoItemLojaCDGateway.Expect(g => g.Find("idItemEntrada = @idItemEntrada", new { idItemEntrada = 1 })).IgnoreArguments()
               .Return(new[] { new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 } });

            relacaoItemLojaCDGateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            compraCasadaService.SalvarItensCompraCasada(filtro);

            compraCasadaGateway.AssertWasCalled(g => g.Update(compraCasadaExistente));
        }

        [Test]
        public void SalvarItensCompraCasada_ItensCompraCasada_AtualizouItens()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(0)).IgnoreArguments().Return(new LojaCdParametro() { IDLojaCDParametro = 0, IDLoja = 1, IDCD = 1 });
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(5, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });

            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itemExistente = new ItemDetalhe()
            {
                FilhoCompraCasada = true,
                PaiCompraCasada = false,
                CdSistema = 1,
                IDItemDetalhe = 5,
                ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 },
                IDCompraCasada = 1,
                FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() { IDFornecedorParametro = 1 },
                VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97,
                VlCustoUnitario = 1,
                QtVendorPackage = 1,
                Traits = 1
            };

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe()
                { 
                    PaiCompraCasada = true, 
                    FilhoCompraCasada = false,
                    QtVendorPackage = 2,
                    IDItemDetalhe = 1,
                    CdSistema = 1,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                { 
                    FilhoCompraCasada = true, 
                    PaiCompraCasada = false,
                    QtVendorPackage =1,
                    CdSistema = 1,
                    IDItemDetalhe = 3,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                itemExistente
            };

            var compraCasadaExistente = new CompraCasada()
            {
                blAtivo = true,
                blItemPai = false,
                IDCompraCasada = 1,
                IDFornecedorParametro = 1,
                IDItemDetalheEntrada = 5,
                IDItemDetalheSaida = 1
            };

            compraCasadaGateway.Expect(g => g.Find(null, null)).Return(new[] { compraCasadaExistente }).IgnoreArguments();

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            relacaoItemLojaCDGateway.Expect(g => g.Find("idItemEntrada = @idItemEntrada", new { idItemEntrada = 1 })).IgnoreArguments()
               .Return(new[] { new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 } });

            relacaoItemLojaCDGateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            compraCasadaService.SalvarItensCompraCasada(filtro);

            compraCasadaGateway.AssertWasCalled(g => g.Update(compraCasadaExistente));
        }

        [Test]
        public void SalvarItensCompraCasada_ItensCompraCasada_InseriuItens()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(0)).IgnoreArguments().Return(new LojaCdParametro() { IDLojaCDParametro = 0, IDLoja = 1, IDCD = 1 });
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(5, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe()
                { 
                    PaiCompraCasada = true, 
                    IDItemDetalhe = 1,
                    QtVendorPackage = 2,
                    CdSistema = 1,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                { 
                    FilhoCompraCasada = true, 
                    IDItemDetalhe = 3,
                    CdSistema = 1,
                    QtVendorPackage = 1,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                },
                new ItemDetalhe()
                { 
                    FilhoCompraCasada = true, 
                    QtVendorPackage = 1,
                    CdSistema = 1,
                    IDItemDetalhe = 5,
                    ItemSaida = new ItemDetalhe(){ IDItemDetalhe = 1 }, 
                    FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro(){ IDFornecedorParametro = 1 },
                    VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1
                }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            relacaoItemLojaCDGateway.Expect(g => g.Find("idItemEntrada = @idItemEntrada", new { idItemEntrada = 1 })).IgnoreArguments()
               .Return(new[] { new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 } });

            relacaoItemLojaCDGateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            compraCasadaService.SalvarItensCompraCasada(filtro);

            var pars = compraCasadaGateway.GetArgumentsForCallsMadeOn(g => g.Insert(new CompraCasada()));
            foreach (var p in pars)
            {
                var compra = p.FirstOrDefault() as CompraCasada;
                var existe = itens.Any(i => i.IDItemDetalhe == compra.IDItemDetalheEntrada);

                Assert.IsTrue(existe);
            }
        }

        [Test]
        public void VerificaPossuiCadastro_FiltrosItemExistente_True()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe(){ IDCompraCasada = 1, ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 } }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1 };

            compraCasadaGateway.Expect(g => g.PossuiCadastro(filtro)).Return(true);

            var result = compraCasadaService.VerificaPossuiCadastro(filtro);

            Assert.IsTrue(result);
        }

        [Test]
        public void ExcluirItensCompraCasada_ItensCompraCasada_RealizaExclusaoLogica()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            sugestaoPedidoGateway.Expect(g => g.VerificaItemSaidaGradeAberta(1, 1, 1, DateTime.Now)).Return(false);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(0)).IgnoreArguments().Return(new LojaCdParametro() { IDLojaCDParametro = 0, IDLoja = 1, IDCD = 1 });

            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe(){ IDItemDetalhe = 1, IDCompraCasada = 1, ItemSaida = new ItemDetalhe() { IDItemDetalhe = 1 } },
                new ItemDetalhe(){ IDItemDetalhe = 2 }
            };

            relacaoItemLojaCDGateway.Expect(g => g.Find("idItemEntrada = @idItemEntrada", new { idItemEntrada = 1 })).IgnoreArguments()
                .Return(new[] { new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 } });

            relacaoItemLojaCDGateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            var cc = new CompraCasada() { IDCompraCasada = 1, blAtivo = true };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1 };

            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(itens);
            compraCasadaGateway.Expect(g => g.FindById(1)).Return(cc);
            compraCasadaService.ExcluirItensCompraCasada(filtro);

            Assert.IsFalse(cc.blAtivo);
        }

        [Test]
        public void ValidarItemFilhoMarcado_Itens_SatisfiedTrue()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(2, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe(){ IDItemDetalhe = 1, CdSistema = 1, PaiCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 2, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 3, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            var result = compraCasadaService.ValidarItemFilhoMarcado(filtro);

            //Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ValidarItemFilhoMarcado_ItensTipoRADiferente_SatisfiedFalse()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(2, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe(){ IDItemDetalhe = 1, CdSistema = 1, PaiCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 2, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd7, VlCustoUnitario = 1, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 3, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            var result = compraCasadaService.ValidarItemFilhoMarcado(filtro);

            //Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ValidarItemFilhoMarcado_ItensCustoUnitarioDiferente_SatisfiedFalse()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(2, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe(){ IDItemDetalhe = 1, CdSistema = 1, PaiCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 2, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 2, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 3, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            var result = compraCasadaService.ValidarItemFilhoMarcado(filtro);

            //Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ValidarItemFilhoMarcado_ItensTraitDiferente_SatisfiedFalse()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(1, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(2, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 2}
            });
            itemDetalheGateway.Expect(g => g.ObterTraitsPorItem(3, 1, null)).Return(new List<Loja>()
            {
                new Loja() { cdLoja = 1}
            });
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var itens = new List<ItemDetalhe>()
            {
                new ItemDetalhe(){ IDItemDetalhe = 1, CdSistema = 1, PaiCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 },
                new ItemDetalhe(){ IDItemDetalhe = 2, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 2 },
                new ItemDetalhe(){ IDItemDetalhe = 3, CdSistema = 1, FilhoCompraCasada = true, VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97, VlCustoUnitario = 1, Traits = 1 }
            };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 1, Itens = itens };
            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>());

            var result = compraCasadaService.ValidarItemFilhoMarcado(filtro);

            //Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void PesquisarItensCompraCasada_Filtros_Itens()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1 };

            compraCasadaGateway.Expect(g => g.PesquisarItensCompraCasada(filtro, paging))
                .Return(new List<ItemDetalhe>() 
                { 
                    new ItemDetalhe()
                    {
                        Id = 1,
                        CdSistema = 1,
                        IDDepartamento = 1
                    }
                });

            var result = compraCasadaService.PesquisarItensCompraCasada(filtro, paging);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void PesquisarItensEntrada_Filtros_Itens()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var filtro = new PesquisaCompraCasadaFiltro() { cdSistema = 1, idDepartamento = 1, idFornecedorParametro = 1, idItemDetalheSaida = 2 };

            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, paging))
                .Return(new List<ItemDetalhe>() 
                { 
                    new ItemDetalhe()
                    {
                        Id = 1,
                        CdSistema = 1,
                        IDDepartamento = 1,
                        FornecedorParametro = new Domain.Gerenciamento.FornecedorParametro() 
                        {
                            IDFornecedorParametro  = 1
                        },
                        ItemSaida = new ItemDetalhe()
                        {
                            IDItemDetalhe = 2
                        }
                    }
                });

            var result = compraCasadaService.PesquisarItensEntrada(filtro, paging);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void VerificaPossuiPai_FiltroSemPai_Null()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var filtro = new PesquisaCompraCasadaFiltro()
            {
                cdSistema = 1,
                idDepartamento = 1,
                idFornecedorParametro = 1,
                idItemDetalheSaida = 2,
                Itens = new List<ItemDetalhe>() { },
                ItemPaiSelecionado = new ItemDetalhe() { IDItemDetalhe = 456 }
            };

            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>() { });

            var retorno = compraCasadaService.VerificaPossuiPai(filtro);

            Assert.IsNull(retorno);
        }

        [Test]
        public void VerificaPossuiPai_FiltrosSemPai_ItemPaiBanco()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var filtro = new PesquisaCompraCasadaFiltro()
            {
                cdSistema = 1,
                idDepartamento = 1,
                idFornecedorParametro = 1,
                idItemDetalheSaida = 2,
                Itens = new List<ItemDetalhe>() { },
                ItemPaiSelecionado = new ItemDetalhe() { IDItemDetalhe = 456 }
            };

            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>() 
            { 
                new ItemDetalhe(){ PaiCompraCasada = true, IDItemDetalhe = 222 }
            });

            var retorno = compraCasadaService.VerificaPossuiPai(filtro);

            Assert.IsNotNull(retorno);

            Assert.IsFalse(filtro.Itens.Select(i => i.IDItemDetalhe).Contains(retorno.IDItemDetalhe));
        }

        [Test]
        public void VerificaPossuiPai_FiltroComItemPai_ItemPaiFiltro()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var filtro = new PesquisaCompraCasadaFiltro()
            {
                cdSistema = 1,
                idDepartamento = 1,
                idFornecedorParametro = 1,
                idItemDetalheSaida = 2,
                Itens = new List<ItemDetalhe>() { new ItemDetalhe() { IDItemDetalhe = 356, PaiCompraCasada = true } },
                ItemPaiSelecionado = new ItemDetalhe() { IDItemDetalhe = 456 }
            };

            var retorno = compraCasadaService.VerificaPossuiPai(filtro);

            Assert.IsNotNull(retorno);

            Assert.IsTrue(filtro.Itens.Select(i => i.IDItemDetalhe).Contains(retorno.IDItemDetalhe));
        }

        [Test]
        public void MergeItens_FiltroComItens_ItensFiltroComItensBanco()
        {
            var compraCasadaGateway = MockRepository.GenerateMock<ICompraCasadaGateway>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            var logGateway = MockRepository.GenerateMock<IAuditGateway>();
            var logService = new AuditService(logGateway);

            var relacaoItemLojaCDGateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var relacaoItemLojaCDService = new RelacaoItemLojaCDService(relacaoItemLojaCDGateway, gwLCP);
            var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            sugestaoPedidoCDGateway.Expect(g => g.VerificaItemSaidaGradeAbertaSugestaoCD(1, 1, 1, DateTime.Now)).Return(false);
            var compraCasadaService = new CompraCasadaService(compraCasadaGateway, itemDetalheGateway, sugestaoPedidoGateway, sugestaoPedidoCDGateway, relacaoItemLojaCDService, logService);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var filtro = new PesquisaCompraCasadaFiltro()
            {
                cdSistema = 1,
                idDepartamento = 1,
                idFornecedorParametro = 1,
                idItemDetalheSaida = 2,
                Itens = new List<ItemDetalhe>() { new ItemDetalhe() { IDItemDetalhe = 2 }, new ItemDetalhe() { IDItemDetalhe = 3, PaiCompraCasada = false, FilhoCompraCasada = false } },
            };

            compraCasadaGateway.Expect(g => g.PesquisarItensEntrada(filtro, null)).Return(new List<ItemDetalhe>()
            {
                new ItemDetalhe() { IDItemDetalhe = 1 }
            });

            var result = compraCasadaService.MergeItens(filtro);

            Assert.IsTrue(result.Select(i => i.IDItemDetalhe).Contains(1));
            Assert.IsTrue(result.Select(i => i.IDItemDetalhe).Contains(2));
            Assert.IsTrue(result.Select(i => i.IDItemDetalhe).Contains(3));

            Assert.AreEqual(3, result.GroupBy(g => g.IDItemDetalhe).Count());
        }
    }
}
