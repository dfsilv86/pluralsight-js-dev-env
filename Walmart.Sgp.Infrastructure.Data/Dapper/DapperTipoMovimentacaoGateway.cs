using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para tipo movimentação utilizando o Dapper.
    /// </summary>
    public class DapperTipoMovimentacaoGateway : EntityDapperDataGatewayBase<TipoMovimentacao>, ITipoMovimentacaoGateway
    {
        #region Constructors                
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperTipoMovimentacaoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperTipoMovimentacaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "TipoMovimentacao", "IDTipoMovimentacao")
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
                    "dsTipoMovimentacao",
                    "bitApurarCustoMedio",
                    "Ordem",
                    "TipoMovimento"
                };
            }
        }
        #endregion
    }
}
