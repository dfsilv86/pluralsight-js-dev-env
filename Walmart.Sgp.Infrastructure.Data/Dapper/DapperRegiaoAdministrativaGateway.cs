using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para região administrativa utilizando o Dapper.
    /// </summary>
    public class DapperRegiaoAdministrativaGateway : EntityDapperDataGatewayBase<RegiaoAdministrativa>, IRegiaoAdministrativaGateway
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRegiaoAdministrativaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperRegiaoAdministrativaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RegiaoAdministrativa", "IdRegiaoAdministrativa")
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
                return new string[] { "dsRegiaoAdministrativa" };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obter todas as regiões administrativas.
        /// </summary>
        /// <returns>As regiões administrativas.</returns>
        public IEnumerable<RegiaoAdministrativa> ObterTodos()
        {
            return this.FindAll();
        }

        #endregion
    }
}
