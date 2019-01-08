using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para RelacaoItemCD utilizando o Dapper.
    /// </summary>
    public class DapperRelacaoItemCDGateway : EntityDapperDataGatewayBase<RelacaoItemCD>, IRelacaoItemCDGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRelacaoItemCDGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRelacaoItemCDGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RelacaoItemCD", "IDRelacaoItemCD")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "idItemEntrada", "idItemSaida", "idCD", "vlTipoReabastecimento", "vlEstoqueSeguranca" };
            }
        }

        /// <summary>
        /// Obtém um RelacaoItemCD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade RelacaoItemCD.</returns>
        public RelacaoItemCD ObterPorId(long id)
        {
            return this.Find("IDRelacaoItemCD=@IDRelacaoItemCD", new { IDRelacaoItemCD = id }).SingleOrDefault();
        }
    }
}