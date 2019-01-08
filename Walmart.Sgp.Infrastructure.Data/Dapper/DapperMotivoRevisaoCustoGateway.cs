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
    /// Implementação de um table data gateway para motivo revisao custo utilizando o Dapper.
    /// </summary>
    public class DapperMotivoRevisaoCustoGateway : EntityDapperDataGatewayBase<MotivoRevisaoCusto>, IMotivoRevisaoCustoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperMotivoRevisaoCustoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperMotivoRevisaoCustoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "MotivoRevisaoCusto", "IDMotivo")
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
                return new string[] { "dsMotivo" };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obter todas os motivos revisao custo.
        /// </summary>
        /// <returns>Os motivos revisao custo.</returns>
        public IEnumerable<MotivoRevisaoCusto> ObterTodos()
        {
            return this.FindAll();
        }

        #endregion
    }
}
