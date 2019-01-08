using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para distrito utilizando o Dapper.
    /// </summary>
    public class DapperDistritoGateway : EntityDapperDataGatewayBase<Distrito>, IDistritoGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperDistritoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperDistritoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Distrito", "IDDistrito")
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
                return new string[] { "nmDistrito", "IDRegiao", "cdUsuarioResponsavelDistrito", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao" };
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtem um Distrito por Id.
        /// </summary>
        /// <param name="id">O ID do Distrito.</param>
        /// <returns>Retorna um Distrito.</returns>
        public Distrito ObterEstruturado(int id)
        {
            return Resource.Query<Distrito, Regiao, Bandeira, Distrito>(
                Sql.Distrito.Pesquisar_Paging,
                new
                {
                    cdSistema = (int?)null,
                    idBandeira = (int?)null,
                    idRegiao = (int?)null,
                    idDistrito = id,
                    idUsuario = RuntimeContext.Current.User.Id,
                    TipoPermissao = (int?)null
                },
                MapDistrito,
                "SplitOn1,SplitOn2").AsPaging(new Paging(0, 2), Sql.Distrito.Pesquisar_Paging, Sql.Distrito.Pesquisar_Count).SingleOrDefault();
        }

        /// <summary>
        /// Pesquisar Distritos
        /// </summary>
        /// <param name="cdSistema">Código do sistema.</param>
        /// <param name="idBandeira">ID da Bandeira.</param>
        /// <param name="idRegiao">ID da Região.</param>
        /// <param name="idDistrito">ID do Distrito.</param>
        /// <param name="paging">Parametro de paginação.</param>
        /// <returns>Retorna uma lista de Distritos como resultado da busca.</returns>
        public IEnumerable<Distrito> Pesquisar(int? cdSistema, int? idBandeira, int? idRegiao, int? idDistrito, Paging paging)
        {
            return Resource.Query<Distrito, Regiao, Bandeira, Distrito>(
                Sql.Distrito.Pesquisar_Paging,
                new
                {
                    cdSistema,
                    idBandeira,
                    idRegiao,
                    idDistrito,
                    idUsuario = RuntimeContext.Current.User.Id,
                    TipoPermissao = (int?)null
                },
                MapDistrito,
                "SplitOn1,SplitOn2").AsPaging(paging, Sql.Distrito.Pesquisar_Paging, Sql.Distrito.Pesquisar_Count);
        }

        private static Distrito MapDistrito(Distrito d, Regiao r, Bandeira b)
        {
            r.Bandeira = b;
            d.Regiao = r;

            return d;
        }

        #endregion
    }
}
