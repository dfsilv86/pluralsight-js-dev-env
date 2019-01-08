    using System.Collections.Generic;
using System.Data.SqlClient;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para detalhe de bandeira utilizando o Dapper.
    /// </summary>
    public class DapperBandeiraDetalheGateway : EntityDapperDataGatewayBase<BandeiraDetalhe>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperBandeiraDetalheGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperBandeiraDetalheGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "BandeiraDetalhe", "IDBandeiraDetalhe")
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
                    "IDBandeira",
                    "IDDepartamento",
                    "IDCategoria"
                };
            }
        }
        #endregion       
    }
}
