using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para LogMensagemReturnSheetVigente utilizando o Dapper.
    /// </summary>
    public class DapperLogMensagemReturnSheetVigenteGateway : EntityDapperDataGatewayBase<LogMensagemReturnSheetVigente>, ILogMensagemReturnSheetVigenteGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperLogMensagemReturnSheetVigenteGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperLogMensagemReturnSheetVigenteGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "LogMensagemReturnSheetVigente", "IDLogMensagemReturnSheetVigente")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDUsuario", "IDLoja", "dhCriacao" };
            }
        }

        /// <summary>
        /// Obtém um LogMensagemReturnSheetVigente pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade LogMensagemReturnSheetVigente.</returns>
        public LogMensagemReturnSheetVigente ObterPorId(long id)
        {
            return this.Find("IDLogMensagemReturnSheetVigente=@IDLogMensagemReturnSheetVigente", new { IDLogMensagemReturnSheetVigente = id }).SingleOrDefault();
        }
    }
}