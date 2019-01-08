#if ADO_BENCHMARK
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para bandeira utilizando o ADO .NET.
    /// </summary>
    public class AdoBandeiraGateway : EntityAdoDataGatewayBase<Bandeira>, IBandeiraGateway
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoBandeiraGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        protected AdoBandeiraGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Bandeira", "IDBandeira")
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
                return new string[] { "dsBandeira", "sgBandeira", "tpCusto", "cdSistema", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "blImportarTodos", "IDFormato" };
            }
        }

        /// <summary>
        /// Obter por usuário e sistema.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idSistema">O id de sistema.</param>
        /// <returns>As bandeiras (resumo).</returns>
        public IEnumerable<BandeiraResumo> ObterPorUsuarioESistema(int idUsuario, int idSistema)
        {
            var sql = SqlResourceReader.Read("Bandeira", Sql.Bandeira.ObterPorUsuarioESistema);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, new { IDUsuario = idUsuario, cdSistema = idSistema });

            return Map<BandeiraResumo>(cmd, "IDBandeira,DsBandeira,CdSistema");
        }
        #endregion
    }
}
#endif