using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para estoque utilizando o Dapper.
    /// </summary>
    public class DapperEstoqueGateway : DapperDataGatewayBase<Estoque>, IEstoqueGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperEstoqueGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        /// <remarks>
        /// Configura o Dapper para considerar a conversão de cada uma das classes que herdam de FixedValuesBase.
        /// FixedValuesBase&lt;string&gt; serão string de tamanho fixo no banco de dados.
        /// FixedValuesBase&lt;int&gt; serão Int32 no banco de dados.
        /// </remarks>
        public DapperEstoqueGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtém a informação mais recente sobre custo de item do estoque das lojas.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="tipoPermissao">O tipo de permissão do usuário.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A informação mais recente de custos. (conforme Estoque.dtRecebimento)</returns>
        public IEnumerable<Estoque> PesquisarUltimoCustoDoItemPorLoja(int idItemDetalhe, int? idLoja, int idUsuario, TipoPermissao tipoPermissao, Paging paging)
        {
            return this.Resource.Query<Estoque, ItemDetalhe, Loja, Estoque>(
                Sql.Estoque.ObterUltimoCustoDoItemPorLoja_Paging,
                new { idItemDetalhe, idLoja, idUsuario, tipoPermissao },
                (estoque, item, loja) =>
                {
                    estoque.ItemDetalhe = item;
                    item.IDItemDetalhe = (int)estoque.IDItemDetalhe;
                    estoque.Loja = loja;
                    loja.IDLoja = estoque.IDLoja;
                    return estoque;
                },
                "SplitOn1,SplitOn2")
                .AsPaging(paging, Sql.Estoque.ObterUltimoCustoDoItemPorLoja_Paging, Sql.Estoque.ObterUltimoCustoDoItemPorLoja_Count);
        }

        /// <summary>
        /// Obtém a lista de ids de item detalhe que são itens de entrada de um determinado item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>A lista de ids de itens de entrada.</returns>
        /// <remarks>Consulta recursiva no banco, está com OPTION MAXRECURSION 10 no momento.</remarks>
        public IEnumerable<int> ObterOrigemItem(int idItemDetalhe)
        {
            return this.Resource.Query<int>(Sql.Estoque.ObterOrigensItem, new { idItemDetalhe });
        }

        /// <summary>
        /// Obtém os últimos 5 recebimentos do item ou de suas entradas.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 recebimentos.</returns>
        /// <remarks>Como a consulta utiliza também os itens de entrada do item informado, é possível que uma NotaFiscal possua mais de um NotaFiscalItem. O conjunto de todos NotaFiscalItem deve ter os 5 itens.</remarks>
        public IEnumerable<NotaFiscal> ObterOsCincoUltimosRecebimentosDoItemPorLoja(int idItemDetalhe, int idLoja)
        {
            Dictionary<long, NotaFiscal> result = new Dictionary<long, NotaFiscal>();
            Dictionary<int, Loja> lojas = new Dictionary<int, Loja>();
            Dictionary<int, ItemDetalhe> itens = new Dictionary<int, ItemDetalhe>();

            this.Resource.Query<NotaFiscal, Loja, NotaFiscalItem, ItemDetalhe, NotaFiscal>(
                Sql.Estoque.ObterOsCincoUltimosRecebimentosDoItemPorLoja,
                new { idItemDetalhe, idLoja },
                (nf, lj, nfi, id) =>
                {
                    if (!result.ContainsKey(nf.IDNotaFiscal))
                    {
                        result[nf.IDNotaFiscal] = nf;
                    }

                    NotaFiscal theNf = result[nf.IDNotaFiscal];
                    MapLoja(lojas, nf, lj, theNf);
                    theNf.Itens.Add(nfi);
                    MapItem(itens, nfi, id);
                    nfi.ItemDetalhe = itens[(int)nfi.IDItemDetalhe];

                    return nf;
                },
                "SplitOn1,SplitOn2,SplitOn3").Perform();

            return result.Values;
        }

        /// <summary>
        /// Obtém os últimos 5 custo do item na loja
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A lista com os últimos 5 custos.</returns>
        public IEnumerable<CustoMaisRecente> ObterOsCincoUltimosCustosDoItemPorLoja(int cdItem, int idLoja)
        {
            return StoredProcedure.Query<dynamic, Estoque, dynamic, CustoMaisRecente>(
                "PR_SelecionarItemNotas",
                new { cdItem, idLoja },
                (d1, estoque, d2) =>
                {
                    estoque.ItemDetalhe = new ItemDetalhe
                    {
                        IDItemDetalhe = estoque.IDItemDetalhe,
                        DsItem = d2.dsItem,
                        CdItem = cdItem
                    };

                    estoque.vlCustoCompraAtual = d2.vlCusto;

                    var notaFiscal = new NotaFiscal
                    {
                       IDLoja = d2.IDLoja,
                       nrNotaFiscal = d2.NrNota == null ? (long?)null : Convert.ToInt64(d2.NrNota),
                       dtRecebimento = d2.dtRecebimento,
                       dtEmissao = DateTime.ParseExact(d2.dtEmissao, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       DhCriacao = d2.dhCriacao                       
                    };                  

                    var custo = new CustoMaisRecente(notaFiscal, estoque);
                    custo.NrLinha = d1.nrLinha;

                    return custo;
                },
                "IDItemDetalhe,vlCusto");
        }

        /// <summary>
        /// Obtém a informação mais recente sobre custo de item da nota fiscal e do estoque em uma loja.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>A informação mais recente de custos. (conforme NotaFiscal.dtRecebimento e Estoque.dtRecebimento)</returns>
        public CustoMaisRecente ObterUltimoCustoDoItemNaLoja(int idItemDetalhe, int idLoja)
        {
            return this.Resource.Query<NotaFiscal, Loja, NotaFiscalItem, ItemDetalhe, Estoque, CustoMaisRecente>(
                Sql.Estoque.ObterUltimoCustoDoItemNaLoja,
                new { idItemDetalhe, idLoja },
                (nf, lj, nfi, id, ee) =>
                {
                    if (null != nf && nf.IDNotaFiscal != 0)
                    {
                        nf.Loja = lj;
                        nf.Itens.Add(nfi);
                        if (null != nfi)
                        {
                            nfi.ItemDetalhe = id;
                        }
                    }

                    return new CustoMaisRecente(nf, null != ee && ee.IDEstoque != 0 ? ee : null);
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4").FirstOrDefault() ?? new CustoMaisRecente(null, null);
        }

        /// <summary>
        /// Obtém o custo contábil mais recente de um item em uma loja.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>O custo contábil mais recente.</returns>
        public decimal ObterUltimoCustoContabilItem(int idItemDetalhe, int idLoja)
        {
            return this.Resource.ExecuteScalar<decimal?>(
                Sql.Estoque.ObterUltimoCustoContabilItem,
                new { idItemDetalhe, idLoja }) ?? 0m;
        }

        /// <summary>
        /// Realiza o ajuste do estoque manipulado.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajustado.</param>
        public void AjusteEstoqueManipulado(Estoque estoque)
        {            
            StoredProcedure.Execute(
                "PR_AjustarEstoqueManipulado",
                new
            {
                idItemDetalhe = estoque.IDItemDetalhe,
                idLoja = estoque.IDLoja,
                qtd = estoque.qtEstoqueFisico,
                idTipoMovimentacao = estoque.TipoAjuste.Id,
                dtAjusteEstoque = estoque.dhAtualizacao,
                idUser = RuntimeContext.Current.User.Id,
                tpMovimentacao = estoque.TipoMovimentacao,
                idTipoMovimentacaoPerda = (int?)null,
                idMotivoMovimentacao = estoque.MotivoAjuste.Id
            });
        }

        /// <summary>
        /// Realiza o ajuste do estoque receituário.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajustado.</param>
        public void AjustarEstoqueReceituario(Estoque estoque)
        {
            StoredProcedure.Execute("PR_AjustarEstoqueReceituario", BuildArgs(estoque));
        }

        /// <summary>
        /// Realiza o ajuste do estoque vinculado.
        /// </summary>
        /// <param name="estoque">O estoque a ser ajustado.</param>
        public void AjustarEstoqueDireto(Estoque estoque)
        {            
            StoredProcedure.Execute("PR_AjustarEstoqueDireto", BuildArgs(estoque));
        }

        /// <summary>
        /// Realiza o ajuste do estoque manipulado para MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">O estoque a ser ajustado.</param>
        public void AjusteEstoqueManipulado(MovimentacaoMtr movimentacaoMtr)
        {
            StoredProcedure.Execute("PR_AjustarEstoqueManipuladoMTR", BuildArgs(movimentacaoMtr));
        }

        /// <summary>
        /// Realiza o ajuste do estoque receituário para MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">O estoque a ser ajustado.</param>
        public void AjustarEstoqueReceituario(MovimentacaoMtr movimentacaoMtr)
        {
            StoredProcedure.Execute("PR_AjustarEstoqueReceituarioMTR", BuildArgs(movimentacaoMtr));
        }

        /// <summary>
        /// Realiza o ajuste do estoque direto para MTR.
        /// </summary>
        /// <param name="movimentacaoMtr">O estoque a ser ajustado.</param>
        public void AjustarEstoqueDireto(MovimentacaoMtr movimentacaoMtr)
        {
            StoredProcedure.Execute("PR_AjustarEstoqueDiretoMTR", BuildArgs(movimentacaoMtr));
        }

        private static object BuildArgs(Estoque estoque)
        {
            return new
            {
                idItemDetalhe = estoque.IDItemDetalhe,
                idLoja = estoque.IDLoja,
                qtd = estoque.qtEstoqueFisico,

                // No legado é passado o TipoAjuste para idTipoMovimentacao: AjusteItem.cs, linha 66.
                idTipoMovimentacao = estoque.TipoAjuste.Id,
                dtAjusteEstoque = estoque.dhAtualizacao,
                idUser = RuntimeContext.Current.User.Id,
                tpMovimentacao = estoque.TipoMovimentacao,
                idTipoMovimentacaoPerca = (int?)null,
                idMotivoMovimentacao = estoque.MotivoAjuste.Id
            };
        }

        private static object BuildArgs(MovimentacaoMtr movimentacaoMtr)
        {
            // rotinas MTR recebem apenas a data; se passar data e hora ocorre bug
            return new
            {
                idItemOrigem = movimentacaoMtr.IdItemOrigem,
                idItemDestino = movimentacaoMtr.IdItemDestino,
                idLoja = movimentacaoMtr.IdLoja,
                qtd = movimentacaoMtr.Quantidade,
                dtAjusteEstoque = DateTime.Today,
                idUser = RuntimeContext.Current.User.Id
            };
        }

        private static void MapItem(Dictionary<int, ItemDetalhe> itens, NotaFiscalItem nfi, ItemDetalhe id)
        {
            if (!itens.ContainsKey((int)nfi.IDItemDetalhe))
            {
                itens[(int)nfi.IDItemDetalhe] = id;
            }
        }

        private static void MapLoja(Dictionary<int, Loja> lojas, NotaFiscal nf, Loja lj, NotaFiscal theNf)
        {
            if (nf.IDLoja.HasValue)
            {
                if (!lojas.ContainsKey(nf.IDLoja.Value))
                {
                    lojas[nf.IDLoja.Value] = lj;
                }

                theNf.Loja = lojas[theNf.IDLoja.Value];
            }
        }

        #endregion
    }
}
