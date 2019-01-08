using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para formato utilizando o Dapper.
    /// </summary>
    public class DapperFormatoGateway : EntityDapperDataGatewayBase<Formato>, IFormatoGateway
    {
        #region Constructors                
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperFormatoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperFormatoGateway(ApplicationDatabases databases) 
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
