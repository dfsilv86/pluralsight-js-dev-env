using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para parâmetro de loja/CD utilizando o Dapper.
    /// </summary>
    public class DapperLojaCdParametroGateway : EntityDapperDataGatewayBase<LojaCdParametro>, ILojaCdParametroGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperLojaCdParametroGateway"/>
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperLojaCdParametroGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "LojaCdParametro", "IDLojaCDParametro")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] 
                { 
                    "IDLoja",
                    "IDCD",
                    "blAtivo",
                    "cdSistema",
                    "dhCriacao",
                    "dhAtualizacao",
                    "cdUsuarioCriacao",
                    "cdUsuarioAtualizacao",
                    "vlLeadTime",
                    "vlFillRate",
                    "tpPedidoMinimo",
                    "vlValorMinimo",
                    "tpWeek",
                    "tpInterval",
                    "tpProduto" 
                };
            }
        }

        /// <summary>
        /// Pesquisa parâmetros de loja/CD pelos filtros informados.
        /// </summary>
        /// <param name="filtro">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os parâmetros de loja/CD.</returns>
        public IEnumerable<LojaCdParametroPorDepartamento> PesquisarPorFiltros(LojaCdParametroFiltro filtro, Paging paging)
        {
            var usuario = RuntimeContext.Current.User;

            return this.Resource.Query<LojaCdParametroPorDepartamento, Loja, CD, Departamento, ReviewDateCD, LojaCdParametroPorDepartamento>(
                Sql.LojaCdParametro.PesquisarPorFiltros_Paging,
                new
                {
                    filtro.CdLoja,
                    filtro.CdSistema,
                    filtro.IdBandeira,
                    filtro.NmLoja,
                    filtro.TpReabastecimento,
                    idUsuario = usuario.Id,
                    tipoPermissao = usuario.TipoPermissao
                },
                (lojaCdParametro, loja, cd, departamento, reviewDateCD) =>
                {
                    lojaCdParametro.Loja = loja;
                    lojaCdParametro.CD = cd;
                    lojaCdParametro.Departamento = departamento;
                    lojaCdParametro.ReviewDateCd = reviewDateCD;

                    return lojaCdParametro;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4")
                .AsPaging(paging, Sql.LojaCdParametro.PesquisarPorFiltros_Paging, Sql.LojaCdParametro.PesquisarPorFiltros_Count);
        }

        /// <summary>
        /// Obtém o LojaCdParametro com o id informado.
        /// </summary>
        /// <param name="idLojaCdParametro">O id.</param>
        /// <param name="tpReabastecimento">O tipo de reabastecimento.</param>
        /// <returns>O LojaCdParametro.</returns>
        public LojaCdParametro ObterEstruturadoPorId(int idLojaCdParametro, TipoReabastecimento tpReabastecimento)
        {
            LojaCdParametro result = null;

            this.Resource.Query<LojaCdParametro, Loja, CD, Departamento, ReviewDateCD, LojaCdParametro>(
                Sql.LojaCdParametro.ObterEstruturadoPorId,
                new { idLojaCdParametro, tpReabastecimento },
                (lojaCdParametro, loja, cd, departamento, reviewDateCD) =>
                {
                    if (result == null)
                    {
                        result = lojaCdParametro;
                        result.Loja = loja;
                        result.CD = cd;
                        result.tpReabastecimento = reviewDateCD.tpReabastecimento;
                    }

                    reviewDateCD.Departamento = departamento;
                    result.ReviewDates.Add(reviewDateCD);

                    return result;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4").Perform();

            return result;
        }
    }
}
