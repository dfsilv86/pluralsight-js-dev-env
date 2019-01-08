using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para log relacionamento transferencia utilizando o Dapper.
    /// </summary>
    public class DapperLogRelacionamentoTransferenciaGateway : EntityDapperDataGatewayBase<LogRelacionamentoTransferencia>, ILogRelacionamentoTransferenciaGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperLogRelacionamentoTransferenciaGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperLogRelacionamentoTransferenciaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "LogRelacionamentoTransferencia", "IDLogRelacionamentoTransferencia")
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
                return new string[] { "IDItemDetalheOrigem", "IDItemDetalheDestino", "IDLoja", "dtCriacao", "IDUsuario", "tpOperacao" };
            }
        }
        #endregion
    }
}
