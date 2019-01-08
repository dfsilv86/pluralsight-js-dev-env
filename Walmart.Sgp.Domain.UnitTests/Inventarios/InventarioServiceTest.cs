using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios
{
    [TestFixture]
    [Category("Domain"), Category("Inventarios")]
    public class InventarioServiceTest
    {

        [Test]
        public void GetNextInventoryDate_StoreId_DateTime()
        {
            var gateway = MockRepository.GenerateMock<IInventarioGateway>();
            var date = DateTime.Now;
            gateway.Expect(g => g.ObterDataInventarioDaLoja(1)).Return(date);

            var target = new InventarioService(gateway, MockRepository.GenerateMock<IInventarioAgendamentoGateway>(), null, null, null, null, null);
            Assert.AreEqual(date, target.ObterDataInventarioDaLoja(1));
        }

        [Test]
        public void ObterQuantidadeLojasSemAgendamento_Usuario_Quantidade()
        {
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterQuantidadeLojasSemAgendamento(1)).Return(11);
            agendamentoGateway.Expect(e => e.ObterQuantidadeLojasSemAgendamento(2)).Return(200);
            var target = new InventarioService(null, agendamentoGateway, null, null, null, null, null);

            Assert.AreEqual(11, target.ObterQuantidadeLojasSemAgendamento(1));
            Assert.AreEqual(200, target.ObterQuantidadeLojasSemAgendamento(2));

            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterAgendamentos_Filtro_Agendamentos()
        {
            var filtro = new InventarioAgendamentoFiltro { IDBandeira = 1, CdLoja = 2 };
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterAgendamentos(filtro)).Return(new InventarioAgendamento[] { new InventarioAgendamento() });
            var target = new InventarioService(null, agendamentoGateway, null, null, null, null, null);

            Assert.AreEqual(1, target.ObterAgendamentos(filtro).Count());

            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterNaoAgendados_Filtro_NaoAgendados()
        {
            var filtro = new InventarioAgendamentoFiltro { IDBandeira = 1, CdLoja = 2 };
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterNaoAgendados(filtro)).Return(new InventarioNaoAgendado[] { new InventarioNaoAgendado() });
            var target = new InventarioService(null, agendamentoGateway, null, null, null, null, null);

            Assert.AreEqual(1, target.ObterNaoAgendados(filtro).Count());

            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void CancelarInventario_Id_InventarioCanceladoECriticasInativadas()
        {
            var inventario = new Inventario { Id = 1, stInventario = InventarioStatus.Aberto, dhInventario = DateTime.Today };
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(t => t.FindById(1))
                .Return(inventario);
            inventarioGateway.Expect(e => e.CancelarInventario(inventario));

            var criticaGateway = MockRepository.GenerateMock<IInventarioCriticaGateway>();
            criticaGateway.Expect(e => e.InativarCriticas(inventario));

            var target = new InventarioService(inventarioGateway, null, criticaGateway, null, null, null, null);
            target.Cancelar(1);

            inventarioGateway.VerifyAllExpectations();
            criticaGateway.VerifyAllExpectations();
        }

        [Test]
        public void RemoverAgendamentos_Ids_AgendamentosEInventariosCancelados()
        {
            var agendamentoIds = new int[] { 1, 2, 3 };
            var inventario1 = new Inventario { Id = 11, stInventario = InventarioStatus.Aberto, dhInventario = DateTime.Today.AddDays(1), IDDepartamento = 1 };
            var inventario2 = new Inventario { Id = 22, stInventario = InventarioStatus.Aprovado, dhInventario = DateTime.Today.AddDays(1), IDDepartamento = 1 };
            var inventario3 = new Inventario { Id = 33, stInventario = InventarioStatus.Contabilizado, dhInventario = DateTime.Today.AddDays(1), IDDepartamento = 1 };


            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterEstruturadosPorIds(agendamentoIds)).Return(new InventarioAgendamento[] 
            {
                new InventarioAgendamento { Id = 1, IDInventario = 11, stAgendamento = InventarioAgendamentoStatus.Agendado, Inventario = inventario1 },
                new InventarioAgendamento { Id = 2, IDInventario = 22, stAgendamento = InventarioAgendamentoStatus.Executado, Inventario = inventario2 },
                new InventarioAgendamento { Id = 3, IDInventario = 33, stAgendamento = InventarioAgendamentoStatus.Agendado, Inventario = inventario3 },            
            });

            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            inventarioGateway.Expect(e => e.CancelarInventario(inventario1));
            inventarioGateway.Expect(e => e.CancelarInventario(inventario2));
            inventarioGateway.Expect(e => e.CancelarInventario(inventario3));

            var criticaGateway = MockRepository.GenerateMock<IInventarioCriticaGateway>();
            criticaGateway.Expect(e => e.InativarCriticas(inventario1));
            criticaGateway.Expect(e => e.InativarCriticas(inventario2));
            criticaGateway.Expect(e => e.InativarCriticas(inventario3));

            var target = new InventarioService(inventarioGateway, agendamentoGateway, criticaGateway, null, null, null, null);
            target.RemoverAgendamentos(agendamentoIds);

            agendamentoGateway.VerifyAllExpectations();
            inventarioGateway.VerifyAllExpectations();
            criticaGateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterAgendamentoEstruturadoPorId_Id_Estruturado()
        {

            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();
            agendamentoGateway.Expect(e => e.ObterEstruturadosPorIds(null)).IgnoreArguments().Return(new InventarioAgendamento[] 
            {
                new InventarioAgendamento { Id = 1, IDInventario = 11, stAgendamento = InventarioAgendamentoStatus.Agendado,},            
            });

            var target = new InventarioService(null, agendamentoGateway, null, null, null, null, null);
            Assert.IsNotNull(target.ObterAgendamentoEstruturadoPorId(1));
        }

        [Test]
        public void InserirAgendamentos_UmDepartamentoDeUmaLoja_AgendamentoInserido()
        {
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();

            agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            agendamentoGateway.Expect(e => e.Insert(new InventarioAgendamento())).IgnoreArguments().Repeat.Once();

            inventarioGateway.Expect(e => e.ContarInventarios(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            inventarioGateway.Expect(e => e.Insert(new Inventario())).IgnoreArguments().Repeat.Once();

            var categoriaService = MockRepository.GenerateMock<ICategoriaService>();
            var target = new InventarioService(inventarioGateway, agendamentoGateway, null, categoriaService, null, null, null);
            target.InserirAgendamentos(
                DateTime.Today,
                new Loja[] { new Loja { Id = 1, IDBandeira = 11 } },
                new Departamento[] { new Departamento { Id = 2 } });

            inventarioGateway.VerifyAllExpectations();
            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void InserirAgendamentos_TodosOsDepartamentosDeUmaLoja_AgendamentosInseridos()
        {
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();

            agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            agendamentoGateway.Expect(e => e.Insert(new InventarioAgendamento())).IgnoreArguments().Repeat.Times(2);

            inventarioGateway.Expect(e => e.ContarInventarios(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            inventarioGateway.Expect(e => e.Insert(new Inventario())).IgnoreArguments().Repeat.Times(2);

            var categoriaService = MockRepository.GenerateMock<ICategoriaService>();
            var target = new InventarioService(inventarioGateway, agendamentoGateway, null, categoriaService, null, null, null);

            var result = target.InserirAgendamentos(
                DateTime.Today,
                new Loja[] { new Loja { Id = 1, IDBandeira = 11 } },
                new Departamento[] { new Departamento { Id = 2 }, new Departamento { Id = 3 } });

            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(2, result.Validos);

            inventarioGateway.VerifyAllExpectations();
            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void InserirAgendamentos_UmDepartamentoDeTodasAsLojas_AgendamentosInseridos()
        {
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();

            agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            agendamentoGateway.Expect(e => e.Insert(new InventarioAgendamento())).IgnoreArguments().Repeat.Times(2);

            inventarioGateway.Expect(e => e.ContarInventarios(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            inventarioGateway.Expect(e => e.Insert(new Inventario())).IgnoreArguments().Repeat.Times(2);

            var categoriaService = MockRepository.GenerateMock<ICategoriaService>();
            var target = new InventarioService(inventarioGateway, agendamentoGateway, null, categoriaService, null, null, null);

            var result = target.InserirAgendamentos(
                DateTime.Today,
                new Loja[] { new Loja { Id = 1, IDBandeira = 2000 }, new Loja { Id = 2, IDBandeira = 2000 } },
                new Departamento[] { new Departamento { Id = 2 } });

            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(2, result.Validos);

            inventarioGateway.VerifyAllExpectations();
            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void InserirAgendamentos_TodosOsDepartamentosDeTodasAsLojas_AgendamentosInseridos()
        {
            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();

            agendamentoGateway.Expect(e => e.ContarAgendamentos(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aberto, InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            agendamentoGateway.Expect(e => e.Insert(new InventarioAgendamento())).IgnoreArguments().Repeat.Times(4);

            inventarioGateway.Expect(e => e.ContarInventarios(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            inventarioGateway.Expect(e => e.Insert(new Inventario())).IgnoreArguments().Repeat.Times(4);

            var categoriaService = MockRepository.GenerateMock<ICategoriaService>();
            var target = new InventarioService(inventarioGateway, agendamentoGateway, null, categoriaService, null, null, null);

            var result = target.InserirAgendamentos(
                DateTime.Today,
                new Loja[] { new Loja { Id = 1, IDBandeira = 2000 }, new Loja { Id = 2, IDBandeira = 2000 } },
                new Departamento[] { new Departamento { Id = 2 }, new Departamento { Id = 3 } });

            Assert.AreEqual(4, result.Total);
            Assert.AreEqual(4, result.Validos);

            inventarioGateway.VerifyAllExpectations();
            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void AtualizarAgendamentos_DtAgendamentoeIDs_AgendamentosAtualizados()
        {
            var ids = new int[] { 1, 2 };

            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();

            agendamentoGateway.Expect(e => e.ObterEstruturadosPorIds(ids)).Return(new InventarioAgendamento[] {
                new InventarioAgendamento { Inventario = new Inventario { IDLoja = 1, IDDepartamento = 2 } },
                new InventarioAgendamento { Inventario = new Inventario { IDLoja = 3, IDDepartamento = 4 } }
            });
            agendamentoGateway.Expect(e => e.Update("", "", new object())).IgnoreArguments().Repeat.Once();

            inventarioGateway.Expect(e => e.ContarInventarios(1, 2, new RangeValue<DateTime>(), InventarioStatus.Aprovado)).IgnoreArguments().Return(0);
            inventarioGateway.Expect(e => e.Update("", "", new object())).IgnoreArguments().Repeat.Once();

            var target = new InventarioService(inventarioGateway, agendamentoGateway, null, null, null, null, null);
            var result = target.AtualizarAgendamentos(
                DateTime.Today,
                ids);

            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(2, result.Validos);

            inventarioGateway.VerifyAllExpectations();
            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void AtualizarAgendamentos_DataAnterior_AgendamentosNaoAtualizados()
        {
            var ids = new int[] { 1, 2 };

            var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
            var agendamentoGateway = MockRepository.GenerateMock<IInventarioAgendamentoGateway>();

            agendamentoGateway.Expect(e => e.ObterEstruturadosPorIds(ids)).Return(new InventarioAgendamento[] {
                new InventarioAgendamento { Inventario = new Inventario { IDLoja = 1, IDDepartamento = 2 } },
                new InventarioAgendamento { Inventario = new Inventario { IDLoja = 3, IDDepartamento = 4 } }
            });

            var target = new InventarioService(inventarioGateway, agendamentoGateway, null, null, null, null, null);
            var result = target.AtualizarAgendamentos(
                DateTime.Today.AddDays(-2),
                ids);

            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(0, result.Validos);
            Assert.AreEqual(2, result.Invalidos);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Mensagem));

            inventarioGateway.VerifyAllExpectations();
            agendamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterOperacoesPermitidas_InventarioAberto_Operacoes()
        {
            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Aberto
            };

            var target = new InventarioService(null, null, null, null, null, null, null);
            var actual = target.ObterOperacoesPermitidas(inventario);
            Assert.IsFalse(actual.VoltarStatus);
            Assert.IsFalse(actual.Aprovar);
            Assert.IsFalse(actual.Finalizar);
            Assert.IsTrue(actual.Cancelar);
            Assert.IsFalse(actual.ExportarComparacaoEstoque);
            Assert.IsFalse(actual.AlterarItem);
        }

        [Test]
        public void ObterOperacoesPermitidas_InventarioImportado_Operacoes()
        {
            var inventario = new Inventario
            {
                IDInventario = 1,
                stInventario = InventarioStatus.Importado
            };

            var target = new InventarioService(null, null, null, null, null, null, null);
            var actual = target.ObterOperacoesPermitidas(inventario);
            Assert.IsFalse(actual.VoltarStatus);
            Assert.IsFalse(actual.Aprovar);
            Assert.IsFalse(actual.Finalizar);
            Assert.IsTrue(actual.Cancelar);
            Assert.IsFalse(actual.ExportarComparacaoEstoque);
            Assert.IsTrue(actual.AlterarItem);
        }

        /// <summary>
        /// Este teste método é bypass mas não é pelo pelo auto teste
        /// pois o nome do método do gateway não é o mesmo do metodo
        /// do serviço porém a nomenclatura está correta.
        /// </summary>
        [Test]
        public void ObterItensEstruturadoPorFiltro_Filtro_Itens()
        {
            var filtro = new InventarioItemFiltro();
            var inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            inventarioItemGateway.Expect(t => t.ObterEstruturadoPorFiltro(filtro, null))
                .IgnoreArguments()
                .Return(new[]
                {
                    new InventarioItemSumario
                    {
                        IDInventarioItem = 1,
                        IDInventario = 2
                    }
                });

            var target = new InventarioService(null, null, null, null, null, inventarioItemGateway, null);
            var actual = target.ObterItensEstruturadoPorFiltro(filtro, new Paging()).ToArray();

            inventarioItemGateway.VerifyAllExpectations();
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(1, actual[0].IDInventarioItem);
            Assert.AreEqual(2, actual[0].IDInventario);
        }

        /// <summary>
        /// Este teste método é bypass mas não é pelo pelo auto teste
        /// pois o nome do método do gateway não é o mesmo do metodo
        /// do serviço porém a nomenclatura está correta.
        /// </summary>
        [Test]
        public void ObterItemPorId_Id_Item()
        {
            var inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            inventarioItemGateway.Expect(t => t.ObterEstruturadoPorId(1))
                .IgnoreArguments()
                .Return(
                    new InventarioItem
                    {
                        IDInventarioItem = 1,
                        IDInventario = 2
                    }
                );

            var target = new InventarioService(null, null, null, null, null, inventarioItemGateway, null);
            var actual = target.ObterItemEstruturadoPorId(1);

            inventarioItemGateway.VerifyAllExpectations();
            Assert.AreEqual(1, actual.IDInventarioItem);
            Assert.AreEqual(2, actual.IDInventario);
        }

        [Test]
        public void SalvarItem_ItemExistente_Atualizar()
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

                var item = new InventarioItem
                {
                    IDItemDetalhe = 1,
                    IDInventarioItem = 1,
                    qtItem = 1
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Aprovado
                };

                var inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
                inventarioItemGateway.Expect(t => t.Atualizar(item, inventario, true));

                var target = new InventarioService(null, null, null, null, null, inventarioItemGateway, null);
                target.SalvarItem(item, inventario);
                Assert.AreEqual(1, item.IDInventario);
                inventarioItemGateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void SalvarItem_ItemNovo_Inserir()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    IsAdministrator = true,
                    IsGa = true,
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.AdicionarItem) }
                };

                var item = new InventarioItem
                {
                    IDItemDetalhe = 1,
                    qtItem = 1,
                    ItemDetalhe = new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        TpVinculado = TipoVinculado.Saida,
                        IDDepartamento = 2
                    }
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    IDDepartamento = 2,
                    stInventario = InventarioStatus.Aprovado
                };

                var inventarioItemGateway = MockRepository.GenerateMock<IInventarioItemGateway>();
                inventarioItemGateway.Expect(t => t.Inserir(item, true));

                var itemDetalheGateway = MockRepository.GenerateMock<IItemDetalheGateway>();
                itemDetalheGateway.Expect(t => t.FindById(1))
                    .Return(item.ItemDetalhe);

                var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
                inventarioGateway
                    .Expect(t => t.Find(string.Empty, string.Empty, new object()))
                    .IgnoreArguments()
                    .Return(new[] { inventario });

                var target = new InventarioService(inventarioGateway, null, null, null, null, inventarioItemGateway, itemDetalheGateway);
                target.SalvarItem(item, inventario);
                Assert.AreEqual(1, item.IDInventario);
                inventarioItemGateway.VerifyAllExpectations();
                inventarioGateway.VerifyAllExpectations();
                itemDetalheGateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void RemoverItem_IdItemInventario_ChamaMetodoRemoverDoGatewayDeItem()
        {
            var gateway = MockRepository.GenerateMock<IInventarioItemGateway>();
            var item = new InventarioItem
            {
                IDInventarioItem = 1
            };

            gateway.Expect(t => t.FindById(1)).Return(item);
            gateway.Expect(t => t.Remover(item));

            var target = new InventarioService(null, null, null, null, null, gateway, null);
            target.RemoverItem(1);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterIrregularidadesFinalizacao_TodosInvalidos_TresTextos()
        {
            var gateway = MockRepository.GenerateMock<IInventarioGateway>();
            gateway.Expect(t => t.PossuiItemInativoDeletado(1))
                .Return(true);
            gateway.Expect(t => t.PossuiSortimentoInvalido(1))
                .Return(true);
            gateway.Expect(t => t.PossuiItemComCustoDeCadastro(1))
                .Return(true);

            var target = new InventarioService(gateway, null, null, null, null, null, null);

            var actual = target.ObterIrregularidadesFinalizacao(1);

            Assert.AreEqual(3, actual.Count());
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterIrregularidadesAprovacao_TodosInvalidos_TresTextos()
        {
            var gateway = MockRepository.GenerateMock<IInventarioGateway>();
            gateway.Expect(t => t.PossuiItemInativoDeletado(1))
                .Return(true);
            gateway.Expect(t => t.PossuiSortimentoInvalido(1))
                .Return(true);
            gateway.Expect(t => t.PossuiItemComCustoDeCadastro(1))
                .Return(true);

            var target = new InventarioService(gateway, null, null, null, null, null, null);

            var actual = target.ObterIrregularidadesAprovacao(1);

            Assert.AreEqual(3, actual.Count());
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterIrregularidadesAprovacao_ItemVinculadoEntrada_Erro()
        {
            var gateway = MockRepository.GenerateMock<IInventarioGateway>();
            gateway.Expect(t => t.PossuiItemVinculadoEntrada(1))
                .Return(true);

            var target = new InventarioService(gateway, null, null, null, null, null, null);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.ObterIrregularidadesAprovacao(1);
            });

            gateway.VerifyAllExpectations();
        }



        [Test]
        public void Finalizar_InventarioAprovado_Finalizado()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.Finalizar) }
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Aprovado
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(inventario);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                target.Finalizar(1);
                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void Finalizar_InventarioNaoAprovado_NaoFinaliza()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.Finalizar) }
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Cancelado
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(inventario);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                Assert.Throws<NotSatisfiedSpecException>(() =>
                {
                    target.Finalizar(1);
                });

                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void Finalizar_InventarioInexistente_InvalidOperation()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.Finalizar) }
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(null);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                Assert.Throws<UserInvalidOperationException>(() =>
                {
                    target.Finalizar(1);
                });

                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void VoltarStatus_InventarioInexistente_InvalidOperation()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1                    
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(null);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                Assert.Throws<UserInvalidOperationException>(() =>
                {
                    target.VoltarStatus(1);
                });

                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void VoltarStatus_InventarioNaoAprovadoNemFinalizado_NaoVolta()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1                    
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Cancelado
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(inventario);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                Assert.Throws<NotSatisfiedSpecException>(() =>
                {
                    target.VoltarStatus(1);
                });

                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void VoltarStatus_InventarioAprovado_Importado()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    IsAdministrator = true                    
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Aprovado,
                    dhInventario = new DateTime(2000, 4, 1)
                };

                var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
                inventarioGateway.Expect(t => t.FindById(1)).Return(inventario);
                inventarioGateway.Expect(t => t.ReverterParaStatus(inventario, InventarioStatus.Importado));

                var fechamentoFiscalGateway = MockRepository.GenerateMock<IFechamentoFiscalGateway>();
                fechamentoFiscalGateway.Stub(t => t.ObterUltimo(0)).IgnoreArguments()
                    .Return(new FechamentoFiscal { nrMes = 3, nrAno = 2000 });
                
                inventarioGateway.Expect(t => t.ObterUltimo(0, null, DateTime.MinValue)).IgnoreArguments()
                    .Return(new Inventario
                    {
                        IDInventario = 1
                    });


                var target = new InventarioService(inventarioGateway, null, null, null, fechamentoFiscalGateway, null, null);
                target.VoltarStatus(1);
                inventarioGateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void VoltarStatus_InventarioFinalizado_Aprovado()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    IsAdministrator = true                    
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Aprovado,
                    dhInventario = new DateTime(2000, 4, 1)
                };

                var inventarioGateway = MockRepository.GenerateMock<IInventarioGateway>();
                inventarioGateway.Expect(t => t.FindById(1)).Return(inventario);
                inventarioGateway.Expect(t => t.ReverterParaStatus(inventario, InventarioStatus.Importado));

                var fechamentoFiscalGateway = MockRepository.GenerateMock<IFechamentoFiscalGateway>();
                fechamentoFiscalGateway.Stub(t => t.ObterUltimo(0)).IgnoreArguments()
                    .Return(new FechamentoFiscal { nrMes = 3, nrAno = 2000 });

                inventarioGateway.Expect(t => t.ObterUltimo(0, null, DateTime.MinValue)).IgnoreArguments()
                    .Return(new Inventario
                    {
                        IDInventario = 1
                    });


                var target = new InventarioService(inventarioGateway, null, null, null, fechamentoFiscalGateway, null, null);
                target.VoltarStatus(1);
                inventarioGateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void Aprovar_InventarioImportado_Aprovado()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.Aprovar) }
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Importado
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(inventario);
                gateway.Expect(t => t.PossuiItemVinculadoEntrada(1)).Return(false);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                target.Aprovar(1);
                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void Aprovar_InventarioNaoImportado_NaoAprova()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.Aprovar) }
                };

                var inventario = new Inventario
                {
                    IDInventario = 1,
                    stInventario = InventarioStatus.Cancelado
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(inventario);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                Assert.Throws<NotSatisfiedSpecException>(() =>
                {
                    target.Aprovar(1);
                });

                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void Aprovar_InventarioInexistente_Exception()
        {
            var prevRuntimeUser = RuntimeContext.Current.User;
            var runtimeContext = (MemoryRuntimeContext)RuntimeContext.Current;
            try
            {
                runtimeContext.User = new MemoryRuntimeUser
                {
                    Id = 1,
                    Actions = new[] { new UserActionInfo(InventarioPermissoes.Finalizar) }
                };

                var gateway = MockRepository.GenerateMock<IInventarioGateway>();
                gateway.Expect(t => t.FindById(1)).Return(null);

                var target = new InventarioService(gateway, null, null, null, null, null, null);
                Assert.Throws<UserInvalidOperationException>(() =>
                {
                    target.Aprovar(1);
                });

                gateway.VerifyAllExpectations();
            }
            finally
            {
                runtimeContext.User = prevRuntimeUser;
            }
        }

        [Test]
        public void PesquisarCriticas_Filtro_Criticas()
        {
            var filtro = new InventarioCriticaFiltro();
            var paging = new Paging();
            var criticaGateway = MockRepository.GenerateMock<IInventarioCriticaGateway>();
            criticaGateway.Expect(e => e.Pesquisar(filtro, paging));

            var target = new InventarioService(null, null, criticaGateway, null, null, null, null);
            target.PesquisarCriticas(filtro, paging);
            
            criticaGateway.VerifyAllExpectations();
        }
    }
}
