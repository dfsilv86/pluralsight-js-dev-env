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
    /// Implementação de um table data gateway para permissão bandeira utilizando o Dapper.
    /// </summary>
    public class DapperPermissaoBandeiraGateway : EntityDapperDataGatewayBase<PermissaoBandeira>, IPermissaoBandeiraGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperPermissaoBandeiraGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperPermissaoBandeiraGateway(ApplicationDatabases databases) 
            : base(databases.Wlmslp, "PermissaoBandeira", "IDPermissaoBandeira")
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
                return new string[] { "IDBandeira", "IDPermissao" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se usuário possui permissâo à bandeira.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <returns>Retorna true caso possua permissão, false do contrário.</returns>
        public bool UsuarioPossuiPermissaoBandeira(int idUsuario, int idBandeira)
        {
            return Resource.ExecuteScalar<int>(Sql.PermissaoBandeira.UsuarioPossuiPermissaoBandeira, new { idUsuario, idBandeira }) > 0;
        }
        #endregion
    }
}
