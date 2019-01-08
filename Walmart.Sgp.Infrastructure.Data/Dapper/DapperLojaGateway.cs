using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para bandeira utilizando o Dapper.
    /// </summary>
    public class DapperLojaGateway : EntityDapperDataGatewayBase<Loja>, ILojaGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperLojaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperLojaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Loja", "IDLoja")
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "cdSistema", "IDBandeira", "cdLoja", "nmLoja", "dsEndereco", "dsCidade", "dsServidorSmartEndereco", "dsServidorSmartDiretorio", "dsServidorSmartNomeUsuario", "dsServidorSmartSenha", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "nmDatabase", "IDDistrito", "blEnvioBI", "blCarregaSGP", "blContabilizar", "blCorrecaoPLU", "dsEstado", "DataConversao", "TipoCusto", "TipoArquivoInventario", "DataEnvioBI", "cdUsuarioResponsavelLoja", "blCalculaSugestao", "blEmitePedido", "blAutorizaPedido", "idRegiaoAdministrativa" };
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
            return this.Resource.Query<Loja, Bandeira, Loja>(
                Sql.Loja.ObterPorBandeira,
                new { IDUsuario = idUsuario, idBandeira = idBandeira, cdLoja = cdLoja },
                (l, b) =>
                {
                    l.Bandeira = b;
                    return l;
                },
                "SplitOn1").SingleOrDefault();
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
        public IEnumerable<Loja> Pesquisar(int idUsuario, TipoPermissao tipoPermissao, int cdSistema, int? idBandeira, int? cdLoja, string nmLoja, Paging paging)
        {
            return this.Resource.Query<Loja, Bandeira, Loja>(
                Sql.Loja.Pesquisar,
                new { IdUsuario = idUsuario, TipoPermissao = tipoPermissao, CdSistema = cdSistema, IdBandeira = idBandeira, CdLoja = cdLoja, NmLoja = nmLoja },
                (loja, bandeira) =>
                {
                    if (loja.IDBandeira.HasValue)
                    {
                        loja.Bandeira = bandeira;

                        if (null != bandeira)
                        {
                            bandeira.IDBandeira = loja.IDBandeira.Value;
                        }
                    }

                    return loja;
                },
                "SplitOn1")
                .AsPaging(paging, Sql.Loja.Pesquisar_Paging, Sql.Loja.Pesquisar_Count);
        }

        /// <summary>
        /// Pesquisa lojas por item destino e origem
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As lojas</returns>
        public IEnumerable<Loja> PesquisarLojasPorItemDestinoItemOrigem(LojaFiltro filtro, Paging paging)
        {
            var args = new
            {
                cdSistema = filtro.CdSistema,
                idItemDetalheDestino = filtro.IdItemDetalheDestino,
                idItemDetalheOrigem = filtro.IdItemDetalheOrigem,
                idLoja = filtro.IdLoja
            };

            return this.Resource.Query<Loja>(Sql.Loja.PesquisarPorItemDestinoOrigem, args);
        }

        /// <summary>
        /// Obtem as informações da loja e entidades associadas.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A loja.</returns>
        public Loja ObterEstruturadoPorId(int idLoja)
        {
            return this.Resource.Query<Loja, Bandeira, Formato, Distrito, Regiao, Loja>(
                Sql.Loja.ObterEstruturadoPorId,
                new { idLoja },
                (l, b, f, d, r) =>
                {
                    l.Bandeira = b;
                    l.Distrito = d;
                    
                    if (null != b)
                    {
                        b.Formato = f;
                    }

                    if (null != d)
                    {
                        d.Regiao = r;
                    }

                    return l;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4").SingleOrDefault();
        }

        #endregion
    }
}
