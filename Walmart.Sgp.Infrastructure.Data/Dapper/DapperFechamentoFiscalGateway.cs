using System.Collections.Generic;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para fechamento fiscal utilizando o Dapper.
    /// </summary>
    public class DapperFechamentoFiscalGateway : EntityDapperDataGatewayBase<FechamentoFiscal>, IFechamentoFiscalGateway
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperFechamentoFiscalGateway"/>
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperFechamentoFiscalGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "FechamentoFiscal", "IDFechamentoFiscal")
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
                return new[] { "IDLoja", "nrAno", "nrMes", "dhFechamentoFiscal", "dhContabilizacao", "dhWarehouseInventory", "dhInicioFechamentoFiscal" };
            }
        }

        #endregion

        /// <summary>
        /// Obtém o ultimo fechamento fiscal da loja.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>
        /// O ultimo fechamento fiscal da loja.
        /// </returns>
        public FechamentoFiscal ObterUltimo(int idLoja)
        {
            return Resource.QueryOne<FechamentoFiscal>(Sql.FechamentoFiscal.ObterUltimoPorLoja, new { idLoja });
        }
    }
}