using System.Collections.Generic;
using System.Data.SqlClient;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para parâmetro sistema utilizando o Dapper.
    /// </summary>
    public class DapperParametroSistemaGateway : EntityDapperDataGatewayBase<ParametroSistema>, IParametroSistemaGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperParametroSistemaGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperParametroSistemaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ParametroSistema", "IDParametroSistema")
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
                    "nmParametroSistema",
                    "dsParametroSistema",
                    "vlParametroSistema",
                    "dhCriacao",
                    "dhAtualizacao",
                    "cdUsuarioCriacao",
                    "cdUsuarioAtualizacao"
                };
            }
        }
        #endregion
    }
}
