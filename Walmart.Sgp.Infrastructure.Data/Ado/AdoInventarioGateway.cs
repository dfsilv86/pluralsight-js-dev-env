#if ADO_BENCHMARK
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para inventário utilizando o ADO .NET.
    /// </summary>
    public class AdoInventarioGateway : EntityAdoDataGatewayBase<Inventario>, IInventarioGateway
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoInventarioGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public AdoInventarioGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Inventario", "IDInventario")
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
                return new string[]
                {
                    "IdLoja"
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Localiza a data do próximo inventário da loja informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A data do próximo inventário, se existir.</returns>
        public DateTime? ObterDataInventarioDaLoja(int idLoja)
        {
            var cmd = CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ObtemDataInventario";
            CreateParameters(cmd, new { IdLoja = idLoja });

            return (DateTime?)cmd.ExecuteScalar();
            
        }

        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        public int ObterQuantidadeLojasSemAgendamento(int idUsuario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém a lista de inventários agendados e abertos.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>
        /// Os Inventario que estão abertos.
        /// </returns>
        public IEnumerable<Inventario> ObterInventariosAbertosParaImportacao(int idLoja)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
#endif