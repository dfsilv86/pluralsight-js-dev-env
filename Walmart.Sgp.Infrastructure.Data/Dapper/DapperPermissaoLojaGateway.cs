using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para permissão loja utilizando o Dapper.
    /// </summary>
    public class DapperPermissaoLojaGateway : EntityDapperDataGatewayBase<PermissaoLoja>, IPermissaoLojaGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperPermissaoLojaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperPermissaoLojaGateway(ApplicationDatabases databases) 
            : base(databases.Wlmslp, "PermissaoLoja", "IDPermissaoLoja")
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
                return new string[] { "IDLoja", "IDPermissao" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se usuário possui permissâo à loja.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Retorna true caso possua permissão, false do contrário.</returns>
        public bool UsuarioPossuiPermissaoLoja(int idUsuario, int idLoja)
        {
            return Resource.ExecuteScalar<int>(Sql.PermissaoLoja.UsuarioPossuiPermissaoLoja, new { idUsuario, idLoja }) > 0;
        }
        #endregion
    }
}
