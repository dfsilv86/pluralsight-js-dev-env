#if ADO_BENCHMARK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para usuário utilizando o ADO .NET.
    /// </summary>
    public class AdoUsuarioGateway : EntityAdoDataGatewayBase<Usuario>, IUsuarioGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoUsuarioGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public AdoUsuarioGateway(ApplicationDatabases databases) 
            : base(databases.Wlmslp, "CWIUser", "Id")
        {
        }

        #region Methods        
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[]
                {
                    "Username",
                    "Passwd",
                    "Email",
                    "IdApplication",
                    "FailedPasswdCount"
                };
            }
        }
        #endregion
    }
}
#endif