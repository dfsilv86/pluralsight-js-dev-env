using System.Collections.Generic;
using System.Data.SqlClient;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para de relacionamento de item secundario utilizando o Dapper.
    /// </summary>
    public class DapperRelacionamentoItemSecundarioGateway : EntityDapperDataGatewayBase<RelacionamentoItemSecundario>, IRelacionamentoItemSecundarioGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRelacionamentoItemSecundarioGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRelacionamentoItemSecundarioGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RelacionamentoItemSecundario", "IDRelacionamentoItemSecundario")
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
                return new string[]
                {                    
                    "IDRelacionamentoItemPrincipal",
                    "psItem",
                    "tpItem",
                    "pcRendimentoDerivado",
                    "IDItemDetalhe",
                    "qtItemUn"
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determina o percentual de rendimento derivado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemSecundario.</remarks>
        public decimal? ObterPercentualRendimentoDerivado(int idItemDetalhe, byte cdSistema)
        {
            return this.Resource.ExecuteScalar<decimal?>(Sql.RelacionamentoItemSecundario.ObterPercentualRendimentoDerivado, new { idItemDetalhe, cdSistema });
        }

        /// <summary>
        /// Obtém os relacionamentos onde o item participa como secundário.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>Os relacionamentos principais, onde o item participa como secundário, sendo que a lista de secundários retornada contém apenas o relacionamento onde o item está presente.</returns>
        public IEnumerable<RelacionamentoItemPrincipal> ObterSecundariosPorItem(int idItemDetalhe)
        {
            return this.Resource.Query<RelacionamentoItemPrincipal, ItemDetalhe, RelacionamentoItemSecundario, ItemDetalhe, RelacionamentoItemPrincipal>(
                Sql.RelacionamentoItemSecundario.ObterSecundariosPorItem,
                new { idItemDetalhe },
                (rip, id1, ris, id2) =>
                {
                    rip.RelacionamentoSecundario.Add(ris);
                    rip.ItemDetalhe = id1;
                    ris.ItemDetalhe = id2;
                    return rip;
                },
                "SplitOn1,SplitOn2,SplitOn3");
        }
        #endregion
    }
}
