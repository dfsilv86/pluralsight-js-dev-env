using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para regiao utilizando o Dapper.
    /// </summary>
    public class DapperRegiaoGateway : EntityDapperDataGatewayBase<Regiao>, IRegiaoGateway
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRegiaoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperRegiaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Regiao", "IDRegiao")
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
                return new string[] { "nmRegiao", "IDBandeira" }; 
            }
        }
        #endregion
    }
}
