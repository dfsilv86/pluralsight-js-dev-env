using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para parâmetro de fornecedor utilizando o Dapper.
    /// </summary>
    public class DapperFornecedorParametroGateway : EntityDapperDataGatewayBase<FornecedorParametro>, IFornecedorParametroGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperFornecedorParametroGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperFornecedorParametroGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "FornecedorParametro", "IDFornecedorParametro")
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
                // TODO: se forem criadas as outras propriedades em FornecedorParametro, adicionar seus nomes aqui.
                return new string[] { "IDFornecedor", "cdV9D", "cdTipo", "tpPedidoMinimo", "vlValorMinimo" };
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Pesquisa parametros de fornecedores baseado nos filtros informados.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="stFornecedor">O status do fornecedor.</param>
        /// <param name="cdV9D">O código do vendor (9 dígitos).</param>
        /// <param name="nmFornecedor">A razão social.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parâmetros de fornecedores que se encaixam no filtro informado.</returns>
        public IEnumerable<FornecedorParametro> PesquisarPorFiltro(int cdSistema, string stFornecedor, long? cdV9D, string nmFornecedor, Paging paging)
        {
            return this.Resource.Query<FornecedorParametro, Fornecedor, FornecedorParametro>(
                Sql.FornecedorParametro.PesquisarPorFiltros,
                new
                {
                    cdSistema,
                    stFornecedor,
                    cdV9D,
                    nmFornecedor
                }, 
                MapFornecedorParametro, 
                "SplitOn1").AsPaging(paging);
        }

        /// <summary>
        /// Busca o parametro do fornecedor juntamente com o fornecedor.
        /// </summary>
        /// <param name="id">O identificador do parametro do fornecedor.</param>
        /// <returns>O parametro do fornecedor juntamente com o fornecedor.</returns>
        public FornecedorParametro ObterEstruturadoPorId(int id)
        {
            return Resource.Query<FornecedorParametro, Fornecedor, FornecedorParametro>(
                Sql.FornecedorParametro.ObterEstruturadoPorId,
                new { idFornecedorParametro = id },
                MapFornecedorParametro,
                "SplitOn1").SingleOrDefault();
        }

        /// <summary>
        /// Verifica se existe itens DSD para um Vendor
        /// </summary>
        /// <param name="cdV9D">O codigo 9 digitos do vendor.</param>
        /// <returns>Um valor que indica se o Vendor possui ou não itens DSD.</returns>
        public bool PossuiItensDSD(long cdV9D)
        {
            var qtdItensDSD = Resource.QueryOne<int>(
                Sql.FornecedorParametro.PesquisarItensDSD_Count,
                new { cdV9D = cdV9D });

            return qtdItensDSD > 0;
        }

        /// <summary>
        /// Busca review dates por detalhe.
        /// </summary>
        /// <param name="idFornecedorParametro">O código do fornecedor de 9 dígitos.</param>
        /// <param name="detalhe">O tipo de detalhamento.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os review dates filtrados por detalhe.</returns>
        public IEnumerable<FornecedorParametroReviewDate> ObterReviewDatesPorDetalhe(int idFornecedorParametro, TipoDetalhamentoReviewDate detalhe, Paging paging)
        {            
            var args = new
            {
                OrderField = "LojaCD",
                OrderDirection = "Asc",
                StartIndex = paging.Offset,
                PageSize = paging.Limit,
                idFornecedorParametro = idFornecedorParametro,
                tpDetalhe = detalhe.Value
            };

            var pagingStrategy = new DapperSoftPagingStrategy(paging);

            var results = StoredProcedure.Query<FornecedorParametroReviewDate>("PR_SelecionarFornecedorLojaCDParam", args).AsPaging(pagingStrategy);
            var firstRow = results.FirstOrDefault();
            pagingStrategy.TotalCount = firstRow == null ? 0 : firstRow.QtdRows;
            return results;
        }

        /// <summary>
        /// Pesquisa parametros de fornecedor.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdTipo">O canal do vendor, se aplicável.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parametros de fornecedor encontrados.</returns>
        public IEnumerable<FornecedorParametro> PesquisarPorSistemaCodigo9DigitosENomeFornecedor(int cdSistema, string cdTipo, long? cdV9D, string nmFornecedor, Paging paging)
        {
            return this.Resource.Query<FornecedorParametro, Fornecedor, FornecedorParametro>(
                Sql.FornecedorParametro.PesquisarPorSistemaCodigo9DigitosENomeFornecedor_Paging,
                new { cdSistema, cdTipo, cdV9D, nmFornecedor },
                MapFornecedorParametro,
                "SplitOn1")
                .AsPaging(
                    paging, 
                    Sql.FornecedorParametro.PesquisarPorSistemaCodigo9DigitosENomeFornecedor_Paging, 
                    Sql.FornecedorParametro.PesquisarPorSistemaCodigo9DigitosENomeFornecedor_Count);
        }

        /// <summary>
        /// Localiza um parâmetro de fornecedor pelo código de 9 dígitos e nome do fornecedor.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdTipo">O canal do vendor, se aplicável.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <returns>O parâmetro de fornecedor.</returns>
        public FornecedorParametro ObterPorSistemaECodigo9Digitos(int cdSistema, string cdTipo, long? cdV9D)
        {
            var parametros = this.Resource.Query<FornecedorParametro, Fornecedor, FornecedorParametro>(
                Sql.FornecedorParametro.PesquisarPorSistemaCodigo9DigitosENomeFornecedor_Paging,
                new { cdSistema, cdV9D, cdTipo, nmFornecedor = (string)null },
                MapFornecedorParametro,
                "SplitOn1")
                .AsPaging(
                    new Paging(0, 2),
                    Sql.FornecedorParametro.PesquisarPorSistemaCodigo9DigitosENomeFornecedor_Paging,
                    Sql.FornecedorParametro.PesquisarPorSistemaCodigo9DigitosENomeFornecedor_Count).ToList();

            if (parametros.Count > 1)
            {
                return null;
            }

            return parametros.FirstOrDefault();
        }

        private static FornecedorParametro MapFornecedorParametro(FornecedorParametro fornecedorParametro, Fornecedor fornecedor)
        {
            fornecedorParametro.Fornecedor = fornecedor;
            if (null != fornecedor)
            {
                fornecedor.IDFornecedor = fornecedorParametro.IDFornecedor;
            }

            return fornecedorParametro;
        }

        #endregion
    }
}
