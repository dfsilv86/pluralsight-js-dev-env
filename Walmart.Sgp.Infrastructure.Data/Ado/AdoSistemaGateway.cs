using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para sistema utilizando o Ado.
    /// </summary>
    public class AdoSistemaGateway : AdoDataGatewayBase<Sistema>, ISistemaGateway
    {
        #region Constructors             
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoSistemaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public AdoSistemaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp.Transaction)
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
            var cmd = CreateCommand();
            cmd.CommandText = SqlResourceReader.Read("Sistema", Sql.Sistema.ObterPorUsuario);
            CreateParameters(cmd, new { IDUsuario = idUsuario, CultureCode = cultureCode });

            return Map<Sistema>(cmd);
        }
        #endregion
    }
}
