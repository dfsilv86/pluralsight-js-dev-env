#if ADO_BENCHMARK
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para formato utilizando o ADO .NET.
    /// </summary>
    public class AdoFormatoGateway : EntityAdoDataGatewayBase<Formato>, IFormatoGateway
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoFormatoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        protected AdoFormatoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Formato", "IDFormato")
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
                    "cdSistema",
                    "dsFormato"
                };
            }
        }
        #endregion
    }
}
#endif