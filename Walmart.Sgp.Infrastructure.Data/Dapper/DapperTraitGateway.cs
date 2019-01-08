using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para Trait utilizando o Dapper.
    /// </summary>
    public class DapperTraitGateway : EntityDapperDataGatewayBase<Trait>, ITraitGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperTraitGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperTraitGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Trait", "IDTrait")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "cdSistema", "IdLoja", "IdItemDetalhe", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao" };
            }
        }
        
        /// <summary>
        /// Verifica se existe Trait para uma loja/item.
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="cdLoja">codigo da loja.</param>
        /// <returns>True se possui trait.</returns>
        public bool PossuiTrait(long cdItem, long cdSistema, long cdLoja)
        {
            var count = Resource.ExecuteScalar<int>(Sql.Trait.PossuiTrait, new { cdItem, cdSistema, cdLoja });

            return count > 0;
        }

    }
}