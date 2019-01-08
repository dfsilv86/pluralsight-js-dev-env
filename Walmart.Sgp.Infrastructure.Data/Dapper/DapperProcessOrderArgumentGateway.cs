using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para process order argument utilizando o Dapper.
    /// </summary>
    public class DapperProcessOrderArgumentGateway : EntityDapperDataGatewayBase<ProcessOrderArgument>, IProcessOrderArgumentGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperProcessOrderArgumentGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperProcessOrderArgumentGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ProcessOrderArgument", "ProcessOrderArgumentId")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get { return new string[] { "ProcessOrderId", "Name", "Value", "IsExposed" }; }
        }

        /// <summary>
        /// Determina se existe um argumento cujo valor é o FileVaultTicket especificado.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <returns>True se existe um argumento com este arquivo, false caso contrário.</returns>
        public bool HasFileVaultTicket(FileVaultTicket ticket)
        {
            return Resource.ExecuteScalar<bool>(Sql.ProcessOrderArgument.HasFileVaultTicket, new { ticket = FileVaultTicket.Serialize(ticket) });
        }
    }
}
