using System.Collections.Generic;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para status item da nota fiscal utilizando o Dapper.
    /// </summary>
    public class DapperNotaFiscalItemStatusGateway : EntityDapperDataGatewayBase<NotaFiscalItemStatus>, INotaFiscalItemStatusGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperNotaFiscalItemStatusGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperNotaFiscalItemStatusGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "NotaFiscalItemStatus", "IDNotaFiscalItemStatus")
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
                    "DsNotaFiscalItemStatus",
                    "SiglaNotaFiscalItemStatus"
                };
            }
        }
        #endregion
    }
}
