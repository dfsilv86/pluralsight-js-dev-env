using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para histórico de item detalhe utilizando o Dapper.
    /// </summary>
    public class DapperItemDetalheHistGateway : EntityDapperDataGatewayBase<ItemDetalheHist>, IItemDetalheHistGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperItemDetalheHistGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperItemDetalheHistGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ItemDetalheHist", "IDItemDetalheHist")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] 
                { 
                    "IDItemDetalhe",
                    "tpVinculado",
                    "tpReceituario",
                    "tpManipulado",
                    "vlFatorConversao",
                    "tpUnidadeMedida",
                    "blItemTransferencia",
                    "dhCriacao",
                    "cdUsuarioCriacao" 
                };
            }
        }       
    }
}
