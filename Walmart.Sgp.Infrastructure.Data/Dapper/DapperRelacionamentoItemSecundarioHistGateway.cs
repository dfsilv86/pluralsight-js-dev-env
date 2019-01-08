using System.Collections.Generic;
using System.Data.SqlClient;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para histórico de relacionamento de item secundario utilizando o Dapper.
    /// </summary>
    public class DapperRelacionamentoItemSecundarioHistGateway : EntityDapperDataGatewayBase<RelacionamentoItemSecundarioHist>, IRelacionamentoItemSecundarioHistGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRelacionamentoItemSecundarioHistGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRelacionamentoItemSecundarioHistGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RelacionamentoItemSecundarioHist", "IDRelacionamentoItemSecundarioHist")
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
                    "IDRelacionamentoItemPrincipalHist",
                    "IDRelacionamentoItemSecundario",
                    "IDRelacionamentoItemPrincipal",
                    "psItem",
                    "tpItem",
                    "pcRendimentoDerivado",
                    "IDItemDetalhe",
                    "qtItemUn",
                    "tpAcao",
                    "dhCriacao",
                    "cdUsuarioCriacao"
                };
            }
        }
        #endregion
    }
}
