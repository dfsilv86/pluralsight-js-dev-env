using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para motivo movimentação utilizando o Dapper.
    /// </summary>
    public class DapperMotivoMovimentacaoGateway : EntityDapperDataGatewayBase<MotivoMovimentacao>, IMotivoMovimentacaoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperMotivoMovimentacaoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperMotivoMovimentacaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "MotivoMovimentacao", "IDMotivo")
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
                    "dsMotivo",
                    "blAtivo",
                    "blExibir"
                };
            }
        }
        #endregion
    }
}
