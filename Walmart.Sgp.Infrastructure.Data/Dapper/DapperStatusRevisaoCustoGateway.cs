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
    /// Implementação de um table data gateway para status revisao custo utilizando o Dapper.
    /// </summary>
    public class DapperStatusRevisaoCustoGateway : EntityDapperDataGatewayBase<StatusRevisaoCusto>, IStatusRevisaoCustoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperStatusRevisaoCustoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperStatusRevisaoCustoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "StatusRevisaoCusto", "IDStatusRevisaoCusto")
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
                return new string[] { "dsStatus" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obter todas os status revisao custo.
        /// </summary>
        /// <returns>Os status revisao custo.</returns>
        public IEnumerable<StatusRevisaoCusto> ObterTodos()
        {
            return this.FindAll();
        }
        #endregion
    }
}
