using System.Collections.Generic;
using System.Data.SqlClient;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para papel utilizando o Dapper.
    /// </summary>
    public class DapperPapelGateway : EntityDapperDataGatewayBase<Papel>, IPapelGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperPapelGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperPapelGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "CWIRole")
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
                return new[]
                {                    
                    "Name",
                    "Description",
                    "IdApplication",
                    "IsAdmin",
                    "IsGa",
                    "IsHo"
                };
            }
        }
        #endregion
    }
}
