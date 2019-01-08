using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para grade sugestao utilizando o Dapper.
    /// </summary>
    public class DapperGradeSugestaoGateway : EntityDapperDataGatewayBase<GradeSugestao>, IGradeSugestaoGateway
    {
        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperGradeSugestaoGateway"/> class.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperGradeSugestaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "GradeSugestao", "IDGradeSugestao")
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
                return new[] { "IDBandeira", "IDDepartamento", "IDLoja", "cdSistema", "vlHoraInicial", "vlHoraFinal", "cdUsuarioCriacao", "dhCriacao", "cdUsuarioAtualizacao", "dhAtualizacao" };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Verifica se a grade de sugestões para os parâmetros informados está aberta.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="vlHoraLimite">O valor de hora-minuto (HHMM)</param>
        /// <returns>True se a grade de sugestões estiver aberta.</returns>
        public bool ExisteGradeSugestaoAberta(int cdSistema, int idBandeira, int idDepartamento, int idLoja, int vlHoraLimite)
        {
            var grade = this.Resource.QueryOne<GradeSugestao>(Sql.GradeSugestao.ObterGradeSugestoesSeFechada, new { cdSistema, idBandeira, idDepartamento, idLoja, vlHoraLimite });

            // No legado, considera aberto sempre que a consulta retorna 0 rows - psqSugestaoPedido.aspx.cs linhas 253 ou 806
            return null == grade;
        }

        /// <summary>
        /// Pesquisa estruturado por filtro.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdLoja">O código de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os registros que satisfasem o filtro.</returns>
        public IEnumerable<GradeSugestao> PesquisarEstruturadoPorFiltro(int cdSistema, int? idBandeira, int? cdDepartamento, int? cdLoja, Paging paging)
        {
            return Resource.Query<GradeSugestao, Bandeira, Departamento, Loja, Sistema, dynamic, GradeSugestao>(
                Sql.GradeSugestao.PesquisarEstruturadoPorFiltro,
                new
                {
                    cdSistema,
                    idBandeira,
                    cdDepartamento,
                    cdLoja
                },
                (grade, bandeira, departamento, loja, sistema, d) =>
                {
                    MaperarGradeSugestao(grade, bandeira, departamento, loja, sistema);
                    MapearCarimboCriacao(grade, d);
                    MapearCarimboAtualizacao(grade, d);
                    
                    return grade;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5")
                .AsPaging(paging);
        }

        /// <summary>
        /// Obtém a grade de sugestão junto com seus relacionamentos.
        /// </summary>
        /// <param name="id">O id da grade de sugestão.</param>
        /// <returns>
        /// A grade de sugestão.
        /// </returns>
        public GradeSugestao ObterEstruturadoPorId(int id)
        {
            return Resource.Query<GradeSugestao, Bandeira, Departamento, Loja, Sistema, GradeSugestao>(
                Sql.GradeSugestao.ObterEstruturadoPorId,
                new
                {
                    id
                },
                MaperarGradeSugestao,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4")
                .FirstOrDefault();
        }

        private static GradeSugestao MaperarGradeSugestao(GradeSugestao grade, Bandeira bandeira, Departamento deparamento, Loja loja, Sistema sistema)
        {
            grade.Bandeira = bandeira;
            grade.IDBandeira = bandeira == null ? 0 : bandeira.IDBandeira;
            grade.Departamento = deparamento;
            grade.IDDepartamento = deparamento == null ? (int?)null : deparamento.IDDepartamento;
            grade.Loja = loja;
            grade.IDLoja = loja == null ? (int?)null : loja.IDLoja;
            grade.Sistema = sistema;
            grade.cdSistema = sistema == null ? 0 : sistema.cdSistema;           

            return grade;
        }

        private static void MapearCarimboCriacao(GradeSugestao grade, dynamic d)
        {
            if (d != null)
            {
                string usuarioCriacaoFullname = d.usuarioCriacaoFullname;
                grade.DhCriacao = d.dhCriacao;

                if (!String.IsNullOrEmpty(usuarioCriacaoFullname))
                {
                    grade.UsuarioCriacao = new Usuario { FullName = usuarioCriacaoFullname };
                }
            }
        }

        private static void MapearCarimboAtualizacao(GradeSugestao grade, dynamic d)
        {
            if (d != null)
            {           
                string usuarioAtualizacaoFullname = d.usuarioAtualizacaoFullname;
                grade.DhAtualizacao = d.dhAtualizacao;

                if (!String.IsNullOrEmpty(usuarioAtualizacaoFullname))
                {
                    grade.UsuarioAtualizacao = new Usuario { FullName = usuarioAtualizacaoFullname };
                }
            }
        }
        #endregion
    }
}
