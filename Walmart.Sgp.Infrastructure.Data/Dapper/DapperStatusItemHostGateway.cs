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
    /// Implementação de um table data gateway para status item host utilizando o Dapper.
    /// </summary>
    public class DapperStatusItemHostGateway : DapperDataGatewayBase<StatusItemHost>, IStatusItemHostGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperStatusItemHostGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperStatusItemHostGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém todos os status disponíveis no idioma informado.
        /// </summary>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os status.</returns>
        public IEnumerable<StatusItemHost> ObterPorCultura(string cultureCode)
        {
            return this.Resource.Query<StatusItemHost>(Sql.StatusItemHost.ObterPorCultura, new { cultureCode = cultureCode });
        }
        #endregion
    }
}
