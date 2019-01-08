using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para nota fiscal utilizando o Dapper.
    /// </summary>
    public class DapperNotaFiscalGateway : EntityDapperDataGatewayBase<NotaFiscal>, INotaFiscalGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperNotaFiscalGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperNotaFiscalGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "NotaFiscal", "IDNotaFiscal")
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDConcentrador", "IDLoja", "IDBandeira", "IDFornecedor", "nrNotaFiscal", "srNotaFiscal", "dtEmissao", "dtRecebimento", "dtCadastroLivro", "dtCadastroConcentrador", "dtAtualizacaoConcentrador", "dtInclusaoHistorico", "dtAlteracaoHistorico", "blDivergente", "dtLiberacao", "IDTipoNota", "Visivel", "tpOperacao", "cdCfop", "DhCriacao" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém uma nota fiscal pelo seu id e retorna a nota fiscal com informações das entidades associadas.
        /// </summary>
        /// <param name="idNotaFiscal">O id de nota fiscal.</param>
        /// <returns>A NotaFiscal com informações de Loja, Fornecedor e Bandeira.</returns>
        public NotaFiscal ObterEstruturadoPorId(int idNotaFiscal)
        {
            NotaFiscal result = null;

            this.Resource.Query<NotaFiscal, Loja, Fornecedor, Bandeira, NotaFiscal>(
                Sql.NotaFiscal.ObterEstruturadoPorId,
                new { idNotaFiscal },
                (notaFiscal, loja, fornecedor, bandeira) =>
                {
                    if (result == null)
                    {
                        result = notaFiscal;
                        result.Loja = loja;
                        result.Fornecedor = fornecedor;
                        result.Bandeira = bandeira;
                    }

                    return result;
                },
                "SplitOn1,SplitOn2,SplitOn3").Perform();

            return result;
        }

        /// <summary>
        /// Pesquisa detalhe de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<NotaFiscal> PesquisarPorFiltros(NotaFiscalFiltro filtro, Paging paging)
        {
            var args = new
            {
                cdSistema = filtro.CdSistema,
                idBandeira = filtro.IdBandeira,
                cdLoja = filtro.CdLoja,
                cdFornecedor = filtro.CdFornecedor,
                nrNotaFiscal = filtro.NrNotaFiscal,
                cdItem = filtro.CdItem,
                dtRecebimentoInicio = filtro.DtRecebimento.StartValue,
                dtRecebimentoFim = filtro.DtRecebimento.EndValue,
                dtCadastroConcentradorInicio = filtro.DtCadastroConcentrador.StartValue,
                dtCadastroConcentradorFim = filtro.DtCadastroConcentrador.EndValue,
                dtAtualizacaoConcentradorInicio = filtro.DtAtualizacaoConcentrador.StartValue,
                dtAtualizacaoConcentradorFim = filtro.DtAtualizacaoConcentrador.EndValue
            };

            return this.Resource.Query<NotaFiscal, Loja, Fornecedor, NotaFiscal>(
                Sql.NotaFiscal.PesquisarPorFiltros,
                args,
                MapNotaFiscal,
                "SplitOn1,SplitOn2")
            .AsPaging(paging);
        }

        /// <summary>
        /// Obtém o último item de nota fiscal recebido na loja para o item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>O item na última nota fiscal da loja.</returns>
        public NotaFiscalItem ObterItemNaUltimaNotaRecebidaDaLoja(int idItemDetalhe, int idLoja)
        {
            return Resource.QueryOne<NotaFiscalItem>(Sql.NotaFiscal.ObterItemNaUltimaNotaRecebidaDaLoja, new { idItemDetalhe, idLoja });
        }

        /// <summary>
        /// Obtém os itens da nota fiscal com o id informado.
        /// </summary>
        /// <param name="idNotaFiscal">O id da nota fiscal.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens da nota fiscal.</returns>
        public IEnumerable<NotaFiscalItem> ObterItensDaNotaFiscal(int idNotaFiscal, Paging paging)
        {
            return Resource.Query<NotaFiscalItem, NotaFiscalItemStatus, ItemDetalhe, NotaFiscalItem>(
                Sql.NotaFiscal.ObterItensDaNotaFiscal,
                new { idNotaFiscal },
                (nfi, nfis, id) =>
                {
                    nfi.Status = nfis;
                    nfi.ItemDetalhe = id;

                    return nfi;
                },
                "SplitOn1,SplitOn2")
                .AsPaging(paging);
        }

        /// <summary>
        /// Pesquisa de custos de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens</returns>
        public IEnumerable<CustoNotaFiscal> PesquisarCustosPorFiltros(NotaFiscalFiltro filtro, Paging paging)
        {
            var args = new
            {
                idBandeira = filtro.IdBandeira,
                idLoja = filtro.IdLoja,
                idFornecedor = filtro.IdFornecedor,
                nrNotaFiscal = filtro.NrNotaFiscal,
                idItemDetalhe = filtro.IdItemDetalhe,
                idDepartamento = filtro.IdDepartamento,
                idNotaFiscalItemStatus = filtro.IdNotaFiscalItemStatus,
                dtRecebimentoInicio = filtro.DtRecebimento.StartValue,
                dtRecebimentoFim = filtro.DtRecebimento.EndValue,
                dtCadastroConcentradorInicio = filtro.DtCadastroConcentrador.StartValue,
                dtCadastroConcentradorFim = filtro.DtCadastroConcentrador.EndValue,
                dtAtualizacaoConcentradorInicio = filtro.DtAtualizacaoConcentrador.StartValue,
                dtAtualizacaoConcentradorFim = filtro.DtAtualizacaoConcentrador.EndValue
            };

            return Resource.Query<CustoNotaFiscal>(Sql.NotaFiscal.PesquisarCustosPorFiltros, args).AsPaging(paging);
        }

        /// <summary>
        /// Pesquisa ultimas entradas de notas fiscais pelos filtros informados.
        /// </summary>
        /// <param name="idItemDetalhe">O ID do item</param>
        /// <param name="idLoja">O ID da loja</param>        
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As entradas</returns>
        public IEnumerable<NotaFiscalConsolidado> PesquisarUltimasEntradasPorFiltro(long idItemDetalhe, int idLoja, DateTime dtSolicitacao, Paging paging)
        {
            var args = new
            {
                idItemDetalhe = idItemDetalhe,
                idLoja = idLoja,
                dtSolicitacao = dtSolicitacao
            };

            return this.Resource.Query<NotaFiscalConsolidado>(Sql.NotaFiscal.PesquisarUltimasEntradasPorFiltros, args);
        }

        /// <summary>
        /// Pesquisa os custos do item.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja</param>
        /// <param name="cdItem">O codigo do item</param>
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <returns>Os custos do item.</returns>
        public NotaFiscalItemCustosConsolidado ObterCustosPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao)
        {
            return this.Resource.QueryOne<NotaFiscalItemCustosConsolidado>(Sql.NotaFiscal.ObterCustosPorItem, new { cdLoja = cdLoja, cdItem = cdItem, dtSolicitacao = dtSolicitacao });
        }

        /// <summary>
        /// Verifica se existe notas pendentes para o item informado
        /// </summary>
        /// <param name="cdLoja">O codigo da loja</param>
        /// <param name="cdItem">O codigo do item</param>
        /// <param name="dtSolicitacao">A data da solicitacao</param>
        /// <returns>Verdadeiro ou falso</returns>        
        public bool ExisteNotasPendentesPorItem(int cdLoja, long cdItem, DateTime dtSolicitacao)
        {
            return this.Resource.ExecuteScalar<bool>(Sql.NotaFiscal.ExisteNotasPendentesPorItem, new { cdLoja = cdLoja, cdItem = cdItem, dtSolicitacao = dtSolicitacao });
        }

        private NotaFiscal MapNotaFiscal(NotaFiscal notaFiscal, Loja loja, Fornecedor fornecedor)
        {
            notaFiscal.Loja = loja;
            notaFiscal.Fornecedor = fornecedor;
            return notaFiscal;
        }
        #endregion
    }
}
