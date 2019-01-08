using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para divisao utilizando o Dapper.
    /// </summary>
    public class DapperDivisaoGateway : DapperDataGatewayBase<Divisao>, IDivisaoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperDivisaoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperDivisaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtém a divisão pelo código da divisão e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A divisão.</returns>
        public Divisao ObterPorDivisaoESistema(int cdDivisao, byte cdSistema)
        {
            return this.Resource.QueryOne<Divisao>(Sql.Divisao.ObterPorDivisaoESistema, new { cdDivisao = cdDivisao, cdSistema = cdSistema });
        }

        /// <summary>
        /// Pesquisa divisões filtrando pelo código de divisão, descrição da divisão e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="dsDivisao">A descrição da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A divisão.</returns>
        /// <remarks>Não valida o usuário que está efetuando a pesquisa. O filtro da descrição é Contains.</remarks>
        public IEnumerable<Divisao> PesquisarPorSistema(int? cdDivisao, string dsDivisao, byte cdSistema, Paging paging)
        {
            return this.Resource.Query<Divisao>(Sql.Divisao.PesquisarPorSistema, new { cdDivisao = cdDivisao, dsDivisao = dsDivisao, cdSistema = cdSistema }).AsPaging(paging);
        }
        #endregion
    }
}
