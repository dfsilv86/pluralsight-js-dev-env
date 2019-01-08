using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para RelacaoItemLojaCD utilizando o Dapper.
    /// </summary>
    public class DapperRelacaoItemLojaCDGateway : EntityDapperDataGatewayBase<RelacaoItemLojaCD>, IRelacaoItemLojaCDGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRelacaoItemLojaCDGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRelacaoItemLojaCDGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RelacaoItemLojaCD", "IDRelacaoItemLojaCD")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDItem", "IDLojaCDParametro", "blAtivo", "cdSistema", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "IDItemEntrada", "CdCrossRef", "VlTipoReabastecimento" };
            }
        }

        /// <summary>
        /// Verifica se uma loja é atendida por um CD.
        /// </summary>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se uma loja é atendida por um CD.</returns>
        public bool VerificaLojaAtendeCD(long cdLoja, long cdCD, long cdSistema)
        {
            var result = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.VerificaLojaAtendidaCD, new { cdLoja, cdCD, cdSistema });

            return result > 0;
        }

        /// <summary>
        /// Verifica se um CD existe e é ativo.
        /// </summary>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se o CD existe e está ativo.</returns>
        public bool VerificaCDExistente(long cdCD, long cdSistema)
        {
            var result = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.ObterQtdCDAtivos, new { cdCD, cdSistema });

            return result > 0;
        }

        /// <summary>
        /// Verifica se uma loja existe e é ativa.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se a loja existe e está ativa.</returns>
        public bool VerificaLojaExistente(long cdLoja, long cdSistema)
        {
            var result = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.ObterQtdLojasAtivas, new { cdLoja, cdSistema });

            return result > 0;
        }

        /// <summary>
        /// Verifica se item que faz parte de uma XREF, não é um item prime
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna entidade contendo as informações de item prime.</returns>
        public RelacaoItemLojaCDXrefItemPrime ItemXrefPrime(long cdItem, long cdCD, long cdLoja, long cdSistema)
        {
            return this.Resource.QueryOne<RelacaoItemLojaCDXrefItemPrime>(Sql.RelacaoItemLojaCD.ItemXrefPrime, new { cdItem, cdCD, cdLoja, cdSistema });
        }

        /// <summary>
        /// Verifica se item faz parte de uma XREF, é um item prime e existe item staple secundário na mesma XREF
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna true se o item faz parte de uma XREF, é um item prime e existe item staple secundário na mesma XREF </returns>
        public bool ItemPossuiItensXrefSecundarios(long cdItem, long cdCD, long cdLoja, long cdSistema)
        {
            var result = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.VerificarItemXrefPossuiSecundarios, new { cdItem, cdCD, cdLoja, cdSistema });

            return result > 0;
        }

        /// <summary>
        /// Verifica se dois itens possuem relacionamento.
        /// </summary>
        /// <param name="cdItemEntrada">O codigo do item de entrada.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se os itens possuem relacionamento.</returns>
        public bool PossuiRelacionamentoSGP(long cdItemEntrada, long cdItemSaida, long cdSistema)
        {
            var count = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.PossuiRelacionamento, new { cdItemEntrada, cdItemSaida, cdSistema });

            return count > 0;
        }

        /// <summary>
        /// Verifica se um item de saida possui loja cadastrada.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se já possui cadastro.</returns>
        public bool ItemSaidaPossuiCadastro(long cdItemSaida, long cdCD, long cdLoja, long cdSistema)
        {
            var count = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.Cadastro_Count, new { cdItemSaida, cdCD, cdLoja, cdSistema });

            return count > 0;
        }

        /// <summary>
        /// Verifica se a loja e o CD possuem cadastro para o item de saída.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se já possuem cadastro.</returns>
        public bool LojaCDPossuiCadastroItemControleEstoque(long cdItemSaida, long cdCD, long cdLoja, long cdSistema)
        {
            var count = Resource.ExecuteScalar<int>(Sql.RelacaoItemLojaCD.LojaCDPossuiCadastroItemControleEstoque, new { cdItemSaida, cdCD, cdLoja, cdSistema });

            return count > 0;
        }

        /// <summary>
        /// Obtém um RelacaoItemLojaCD pelo seu id.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">Paginação do resultado</param>
        /// <returns>A entidade RelacaoItemLojaCD.</returns>
        public IEnumerable<RelacaoItemLojaCDConsolidado> ObterPorFiltro(RelacaoItemLojaCDFiltro filtro, Paging paging)
        {
            var usuario = RuntimeContext.Current.User;

            var args = new
            {
                filtro.cdItemSaida,
                filtro.dsEstado,
                filtro.idRegiaoCompra,
                filtro.idBandeira,
                filtro.idFornecedorParametro,
                blVinculado = filtro.blVinculado ? 1 : 0,
                idUsuario = usuario.Id,
                tipoPermissao = usuario.TipoPermissao,
                filtro.cdSistema
            };

            var result = this.Resource.Query<RelacaoItemLojaCD, RelacaoItemLojaCDConsolidado, ItemDetalhe, Fornecedor, RelacaoItemLojaCDConsolidado>(
                Sql.RelacaoItemLojaCD.ObterPorFiltro_Paging,
                args,
                MapRelacaoItemLojaCD,
                "SplitOn1,SplitOn2,SplitOn3")
                .AsPaging(paging, Sql.RelacaoItemLojaCD.ObterPorFiltro_Paging, Sql.RelacaoItemLojaCD.ObterPorFiltro_Count);

            foreach (var item in result)
            {
                var args2 = new { filtro.cdItemSaida, item.cdLoja, filtro.idFornecedorParametro, item.idCD };

                item.ItensDisponiveis = this.Resource.Query<ItemDetalhe, Fornecedor, FornecedorParametro, ItemDetalhe>(
                Sql.RelacaoItemLojaCD.ObterItemEntradaDisponivel,
                args2,
                MapItemDetalhe,
                "SplitOn1,SplitOn2");
            }

            return result;
        }

        /// <summary>
        /// Metodo para obter os dados para a exportação do cadastro de reabastecimentio item/loja.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Lista de RelacaoItemLojaCD</returns>
        public IEnumerable<RelacaoItemLojaCDConsolidado> ObterDadosExportacaoCadastro(RelacaoItemLojaCDFiltro filtro)
        {
            var usuario = RuntimeContext.Current.User;

            var args = new
            {
                filtro.cdItemSaida,
                filtro.dsEstado,
                filtro.idRegiaoCompra,
                filtro.idBandeira,
                filtro.idFornecedorParametro,
                blVinculado = filtro.blVinculado ? 1 : 0,
                idUsuario = usuario.Id,
                tipoPermissao = usuario.TipoPermissao,
                filtro.cdSistema,
                filtro.cdV9D
            };

            var result = this.Resource.Query<RelacaoItemLojaCDConsolidado, dynamic, Departamento, ItemDetalhe, Fornecedor, FornecedorParametro, RelacaoItemLojaCDConsolidado>(
                Sql.RelacaoItemLojaCD.ExportacaoCadastroReabastecimento,
                args,
                MapRelacaoItemLojaCD,
                "CdItemEntrada,SplitOn2,SplitOn3,SplitOn4,SplitOn5");

            return result;
        }

        /// <summary>
        /// Obtém uma RelacaoItemLojaCD;
        /// </summary>
        /// <param name="cdCD"> O código do CD.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdItemDetalheEntrada">O código do item de entrada.</param>
        /// <param name="cdItemDetalheSaida">O código do item de saída.</param>
        /// <returns>Retorna uma RelacaoItemLojaCD</returns>
        public RelacaoItemLojaCD ObterPorFiltroConsiderandoXRef(long cdCD, long cdLoja, long cdItemDetalheEntrada, long cdItemDetalheSaida)
        {
            var args = new
            {
                cdCD = cdCD,
                cdLoja = cdLoja,
                cdItemEntrada = cdItemDetalheEntrada,
                cdItemSaida = cdItemDetalheSaida
            };

            return this.Resource.Query<RelacaoItemLojaCD>(
                Sql.RelacaoItemLojaCD.ObterDadosImportacao,
                args).SingleOrDefault();
        }

        /// <summary>
        /// Obtém os RelacaoItemLojaCD por dados de vinculo.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Os RelacaoItemLojaCD.</returns>
        public IEnumerable<RelacaoItemLojaCD> ObterPorVinculo(long cdLoja, long cdItemSaida, long cdSistema)
        {
            var args = new
            {
                cdLoja,
                cdItemSaida,
                cdSistema
            };

            return this.Resource.Query<RelacaoItemLojaCD>(
                Sql.RelacaoItemLojaCD.ObterPorVinculo,
                args);
        }

        /// <summary>
        /// Obtém todos os processamentos de importacao de vinculo/desvinculo.
        /// </summary>
        /// <param name="currentUserId">O id do usuário efetuando a consulta.</param>
        /// <param name="isAdministrator">Se o usuário efetuando a consulta é administrador.</param>
        /// <param name="createdUserId">O id do usuário.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        public IEnumerable<ProcessOrderModel> ObterProcessamentosImportacao(int currentUserId, bool isAdministrator, int? createdUserId, string processName, ProcessOrderState? state, Framework.Domain.Paging paging)
        {
            string stateNameCreated = Texts.ProcessOrderStateFixedValueCreated,
               stateNameQueued = Texts.ProcessOrderStateFixedValueQueued,
               stateNameError = Texts.ProcessOrderStateFixedValueError,
               stateNameIsExecuting = Texts.ProcessOrderStateFixedValueIsExecuting,
               stateNameFailed = Texts.ProcessOrderStateFixedValueFailed,
               stateNameFinished = Texts.ProcessOrderStateFixedValueFinished,
               stateNameResultsAvailable = Texts.ProcessOrderStateFixedValueResultsAvailable,
               stateNameResultsExpunged = Texts.ProcessOrderStateFixedValueResultsExpunged;

            Dictionary<int, ProcessOrderModel> result = new Dictionary<int, ProcessOrderModel>();

            var paginated = Resource.Query<ProcessOrder, ProcessOrderService, MemoryRuntimeUser, ProcessOrderArgument, ProcessOrderModel>(
                Sql.RelacaoItemLojaCD.ObterProcessamentosImportacao_Paging,
                new
                {
                    currentUserId,
                    isAdministrator,
                    createdUserId,
                    processName,
                    state,
                    stateNameCreated,
                    stateNameQueued,
                    stateNameError,
                    stateNameIsExecuting,
                    stateNameFailed,
                    stateNameFinished,
                    stateNameResultsAvailable,
                    stateNameResultsExpunged,
                },
                (po, pos, user, poa) => DapperProcessOrderGateway.MapProcessOrderArgument(result, po, pos, user, poa),
                "SplitOn1,SplitOn2,SplitOn3").AsPaging(paging, Sql.RelacaoItemLojaCD.ObterProcessamentosImportacao_Paging, Sql.RelacaoItemLojaCD.ObterProcessamentosImportacao_Count);

            paginated.Perform();

            var orders = result.Values;

            var summarized = orders; ////.Select(order => order.Summarize(true, argument => argument.IsExposed)).ToList();

            return new MemoryResult<ProcessOrderModel>(paginated, summarized);
        }

        /// <summary>
        /// Registra LOG para RelacaoItemLojaCD de acordo com os parametros informados.
        /// </summary>
        /// <param name="log">Informações para salvar no log.</param>
        public void RegistraLogRelacaoItemLojaCD(LogRelacaoItemLojaCD log)
        {
            StoredProcedure.Execute("PR_RegistraLogRelacaoItemLojaCD", log);
        }

        /// <summary>
        /// Obtém o tipo reabastecimento de um item vinculado a uma Xref, prime, no caso de cd convertido.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdcd">O código do CD.</param>
        /// <returns>O tipo de reabastecimento caso o item atenda ao cenário, ou null caso contrário.</returns>
        public ValorTipoReabastecimento ObterTipoReabastecimentoItemVinculadoXrefPrime(long cdItem, int cdLoja, int cdSistema, int cdcd)
        {
            return Resource.ExecuteScalar<ValorTipoReabastecimento>(Sql.RelacaoItemLojaCD.ObterTipoReabastecimentoItemVinculadoXrefPrime, new { cdItem, cdLoja, cdSistema, cdcd });
        }

        /// <summary>
        /// Verifica se o item é de saída (tpVinculado=S) e se pode ser vinculado (deve ser Staple, Prime primario, e possuir itens secundarios na mesma xref).
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdcd">O código do CD.</param>
        /// <returns>Se o item é de saída e se pode ser vinculado, ou null caso não seja de saída.</returns>
        public bool? ObterItemSaidaAtendeRequisitos(long cdItem, int cdLoja, int cdSistema, int cdcd)
        {
            return Resource.ExecuteScalar<bool?>(Sql.RelacaoItemLojaCD.ObterItemSaidaAtendeRequisitos, new { cdItem, cdLoja, cdSistema, cdcd });
        }

        private static ItemDetalhe PopulaItem(dynamic itemEntrada)
        {
            var item = new ItemDetalhe()
            {
                DsItem = itemEntrada.DsItemEntrada,
                CdItem = itemEntrada.CdItemEntrada
            };

            return item;
        }

        private static ItemDetalhe MapItemDetalhe(ItemDetalhe itemDetalhe, Fornecedor fornecedor, FornecedorParametro fornecedorParametro)
        {
            itemDetalhe.Fornecedor = fornecedor;
            itemDetalhe.FornecedorParametro = fornecedorParametro;

            return itemDetalhe;
        }

        private RelacaoItemLojaCDConsolidado MapRelacaoItemLojaCD(RelacaoItemLojaCD relacaoItemLojaCD, RelacaoItemLojaCDConsolidado relacaoItemLojaCDConsolidado, ItemDetalhe itemEntrada, Fornecedor fornecedor)
        {
            if (itemEntrada.IDFornecedor.HasValue)
            {
                fornecedor.IDFornecedor = itemEntrada.IDFornecedor.Value;
                itemEntrada.Fornecedor = fornecedor;
            }

            relacaoItemLojaCDConsolidado.ItemEntrada = itemEntrada;
            relacaoItemLojaCDConsolidado.RelacaoItemLojaCD = relacaoItemLojaCD;
            return relacaoItemLojaCDConsolidado;
        }

        private RelacaoItemLojaCDConsolidado MapRelacaoItemLojaCD(RelacaoItemLojaCDConsolidado relacaoItemLojaCDConsolidado, dynamic itemEntrada, Departamento departamentoItemSaida, ItemDetalhe itemSaida, Fornecedor fornecedorEntrada, FornecedorParametro fornecedorParametroEntrada)
        {
            relacaoItemLojaCDConsolidado.ItemEntrada = itemEntrada != null ? PopulaItem(itemEntrada) : new ItemDetalhe();
            relacaoItemLojaCDConsolidado.ItemEntrada.Fornecedor = fornecedorEntrada;
            relacaoItemLojaCDConsolidado.ItemEntrada.FornecedorParametro = fornecedorParametroEntrada;

            itemSaida.Departamento = departamentoItemSaida;

            relacaoItemLojaCDConsolidado.RelacaoItemLojaCD = new RelacaoItemLojaCD();
            relacaoItemLojaCDConsolidado.RelacaoItemLojaCD.Item = itemSaida;

            return relacaoItemLojaCDConsolidado;
        }
    }
}
