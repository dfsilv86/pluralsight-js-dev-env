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
    /// Implementação de um table data gateway para CD utilizando o Dapper.
    /// </summary>
    public class DapperCDGateway : EntityDapperDataGatewayBase<CD>, ICDGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperCDGateway" />.
        /// </summary>
        /// <param name="databases">A data de .</param>
        public DapperCDGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "CD", "IDCD")
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
                    "cdCD",
                    "nmNome",
                    "blAtivo",
                    "stCD",
                    "dhCriacao",
                    "dhAtualizacao",
                    "cdUsuarioCriacao",
                    "cdUsuarioAtualizacao",
                    "cdSistema",
                    "dsUF",
                    "blConvertido"
                };
            }
        }
        #endregion
        
        /// <summary>
        /// Obtém o ID de um CD pelo seu Código.
        /// </summary>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>ID do CD.</returns>
        public int ObterIdCDPorCodigo(int cdCD, int cdSistema)
        {
            var cd = this.Find(
                "IDCD",
                "cdCD=@cdCD AND cdSistema=@cdSistema",
                new { cdCD = cdCD, cdSistema = cdSistema }).SingleOrDefault();

            return cd == null ? default(int) : cd.IDCD;
        }

        /// <summary>
        /// Obtém um CD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade CD.</returns>
        public CD Obter(long id)
        {
            return this.Find("IDCD=@IDCD", new { IDCD = id }).SingleOrDefault();
        }

        /// <summary>
        /// Obtém todos os CDs convertidos ativos.
        /// </summary>
        /// <returns>Retorna a lista com todos os CDs convertidos ativos.</returns>
        public IEnumerable<CD> ObterTodosConvertidosAtivos()
        {
            return Find(
                "IDCD, cdCD",
                "blAtivo=@blAtivo AND blConvertido=@blConvertido",
                new { blAtivo = true, blConvertido = true }).ToList();
        }
    }
}
