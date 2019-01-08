#if ADO_BENCHMARK
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Implementação de um table data gateway para loja utilizando o ADO .NET.
    /// </summary>
    public class AdoLojaGateway : EntityAdoDataGatewayBase<Loja>, ILojaGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoLojaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public AdoLojaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Loja", "IDLoja")
        {
        }

        #region Methods
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "cdSistema", "IDBandeira", "cdLoja", "nmLoja", "dsEndereco", "dsCidade", "dsServidorSmartEndereco", "dsServidorSmartDiretorio", "dsServidorSmartNomeUsuario", "dsServidorSmartSenha", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "nmDatabase", "IDDistrito", "blEnvioBI", "blCarregaSGP", "blContabilizar", "blCorrecaoPLU", "dsEstado", "DataConversao", "TipoCusto", "TipoArquivoInventario", "DataEnvioBI", "cdUsuarioResponsavelLoja", "blCalculaSugestao", "blEmitePedido", "blAutorizaPedido" };
            }
        }

        /// <summary>
        /// Obter por usuário e sistema.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <returns>As lojas.</returns>
        public Loja ObterPorLojaUsuarioEBandeira(int idUsuario, short? idBandeira, int cdLoja)
        {
            var sql = SqlResourceReader.Read("Loja", Sql.Loja.ObterPorBandeira);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, new { idUsuario, idBandeira, cdLoja });

            return Map<Loja>(cmd, BuildColumnsProjection()).SingleOrDefault();
        }

        /// <summary>
        /// Pesquisa lojas com os parâmetros especificados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="tipoPermissao">O tipo de permissão do usuário.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="nmLoja">O nome da loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Lista de lojas que atendem aos parâmetros especificados.</returns>
        public IEnumerable<Loja> Pesquisar(int idUsuario, byte tipoPermissao, byte cdSistema, short idBandeira, int? cdLoja, string nmLoja, Paging paging)
        {
            var sql = SqlResourceReader.Read("Loja", Sql.Loja.Pesquisar);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, new { IdUsuario = idUsuario, TipoPermissao = tipoPermissao, CdSistema = cdSistema, IdBandeira = idBandeira, CdLoja = cdLoja, NmLoja = nmLoja });

            // TODO: paging
            return Map<Loja>(cmd, BuildColumnsProjection());
        }
        #endregion
    }
}
#endif