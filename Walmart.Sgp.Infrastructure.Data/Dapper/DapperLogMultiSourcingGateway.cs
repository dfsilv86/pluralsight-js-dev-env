using System.Collections.Generic;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para LogMultiSourcing utilizando o Dapper.
    /// </summary>
    public class DapperLogMultiSourcingGateway : EntityDapperDataGatewayBase<LogMultiSourcing>, ILogMultiSourcingGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperLogMultiSourcingGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperLogMultiSourcingGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "LogMultiSourcing", "IdLogMultiSourcing")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IdCd", "IdItemDetalheSaida", "IdItemDetalheEntrada", "IdFornecedorParametro", "Data", "IdUsuario", "PercAnterior", "PercPosterior", "TpOperacao", "Observacao" };
            }
        }
    }
}
