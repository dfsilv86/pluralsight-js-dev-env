using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para sistema utilizando o Dapper.
    /// </summary>
    public class DapperSistemaGateway : DapperDataGatewayBase<Sistema>, ISistemaGateway
    {
        #region Constructors             
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSistemaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperSistemaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obter por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os sistemas.</returns>
        public IEnumerable<Sistema> ObterPorUsuario(int idUsuario, string cultureCode)
        {
            var result = this.Resource.Query<Sistema>(Sql.Sistema.ObterPorUsuario, new { IDUsuario = idUsuario, CultureCode = cultureCode });

            return result;
        }
        #endregion
    }
}
