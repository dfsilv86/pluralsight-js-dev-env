using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para RoteiroLoja utilizando o Dapper.
    /// </summary>
    public class DapperRoteiroLojaGateway : EntityDapperDataGatewayBase<RoteiroLoja>, IRoteiroLojaGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRoteiroLojaGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRoteiroLojaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RoteiroLoja", "IDRoteiroLoja")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "idRoteiro", "idloja", "blativo" };
            }
        }

        /// <summary>
        /// Obtém as lojas pelo roteiro.
        /// </summary>
        /// <param name="idRoteiro">O id.</param>
        /// <returns>A lista de lojas.</returns>
        public IEnumerable<RoteiroLoja> ObterPorIdRoteiro(long idRoteiro)
        {
            return this.Find("idRoteiroLoja, blativo", "idRoteiro=@idRoteiro", new { idRoteiro }).ToList();
        }

        /// <summary>
        /// Obtém as lojas válidas para vínculo com o roteiro.
        /// </summary>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="dsEstado">O estado da loja.</param>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A lista contendo as lojas válidas para vínculo com o roteiro.</returns>
        public IEnumerable<RoteiroLoja> ObterLojasValidas(long cdV9D, string dsEstado, int? idRoteiro, Paging paging)
        {
            return Resource.Query<RoteiroLoja>(
                Sql.RoteiroLoja.ObterLojasValidas,
                new { cdV9D, dsEstado, idRoteiro }).AsPaging(paging);
        }
    }
}