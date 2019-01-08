using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para ReabastecimentoItemFornecedorCD utilizando o Dapper.
    /// </summary>
    public class DapperReabastecimentoItemFornecedorCDGateway : EntityDapperDataGatewayBase<ReabastecimentoItemFornecedorCD>, IReabastecimentoItemFornecedorCDGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperReabastecimentoItemFornecedorCDGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperReabastecimentoItemFornecedorCDGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ReabastecimentoItemFornecedorCD", "IdReabastecimentoItemFornecedorCD")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDItemDetalhe", "IDCD", "TipoReabastecimento", "EstoqueSeguranca" };
            }
        }
    }
}