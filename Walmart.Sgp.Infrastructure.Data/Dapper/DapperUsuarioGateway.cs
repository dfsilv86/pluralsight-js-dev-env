using System.Collections.Generic;
using System.Data.SqlClient;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para usuário utilizando o Dapper.
    /// </summary>
    public class DapperUsuarioGateway : EntityDapperDataGatewayBase<Usuario>, IUsuarioGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperUsuarioGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperUsuarioGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "CWIUser")
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
                    "IdApplication",
                    "Username",
                    "Passwd",
                    "FullName",
                    "Email",
                    "IsApproved",
                    "LastActivityDate",
                    "LastLoginDate",
                    "CreationDate",
                    "IsLockedOut",
                    "LastLockoutDate",
                    "PasswdFormat"
                };
            }
        }
        #endregion
    }
}
