using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementa��o de um table data gateway para regi�o compra utilizando o Dapper.
    /// </summary>
    public class DapperRegiaoCompraGateway : EntityDapperDataGatewayBase<RegiaoCompra>, IRegiaoCompraGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova inst�ncia da classe <see cref="DapperRegiaoCompraGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplica��o.</param>
        public DapperRegiaoCompraGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RegiaoCompra", "IdRegiaoCompra")
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Obt�m o nome das colunas que devem ser consideradas nas opera��es de SELECT, INSERT e UPDATE.
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
        /// Obter todas as regi�es de compra.
        /// </summary>
        /// <returns>As regi�es de compra.</returns>
        public IEnumerable<RegiaoCompra> ObterTodos()
        {
            return this.FindAll();
        }

        #endregion
    }
}