using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para região compra utilizando o Dapper.
    /// </summary>
    public class DapperRegiaoCompraGateway : EntityDapperDataGatewayBase<RegiaoCompra>, IRegiaoCompraGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRegiaoCompraGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperRegiaoCompraGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RegiaoCompra", "IdRegiaoCompra")
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
                return new string[] { "dsRegiaoCompra" };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obter todas as regiões de compra.
        /// </summary>
        /// <returns>As regiões de compra.</returns>
        public IEnumerable<RegiaoCompra> ObterTodos()
        {
            return this.FindAll();
        }

        #endregion
    }
}