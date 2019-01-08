using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para parâmetro utilizando o Dapper.
    /// </summary>
    public class DapperParametroGateway : EntityDapperDataGatewayBase<Parametro>, IParametroGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperParametroGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperParametroGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Parametro", "IDParametro")
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
                    "cdUsuarioAdministrador",
                    "dsServidorSmartEndereco",
                    "dsServidorSmartDiretorio",
                    "dsServidorSmartNomeUsuario",
                    "dsServidorSmartSenha",
                    "dhAlteracao",
                    "cdUsuarioAlteracao",
                    "pcDivergenciaCustoCompra",
                    "qtdDiasSugestaoInventario",
                    "PercentualAuditoria",
                    "qtdDiasArquivoInventarioVarejo",
                    "qtdDiasArquivoInventarioAtacado",
                    "TpArquivoInventario",
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o parâmetro com seus relacionamentos.
        /// </summary>
        /// <returns>
        /// O parâmetro.
        /// </returns>
        public Parametro ObterEstruturado()
        {
            return Resource.Query<Parametro, Usuario, Usuario, Parametro>(
                Sql.Parametro.ObterEstruturado,
                null,
                (parametro, usuarioAdministrador, usuarioAlteracao) =>
                {
                    parametro.UsuarioAdministrador = usuarioAdministrador;
                    parametro.UsuarioAlteracao = usuarioAlteracao;

                    return parametro;
                },
                "SplitOn1,SplitOn2")
                .FirstOrDefault();
        }
        #endregion
    }
}
