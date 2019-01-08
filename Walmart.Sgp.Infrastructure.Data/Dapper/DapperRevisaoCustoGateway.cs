using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Classe Revisao Custo
    /// </summary>
    public class DapperRevisaoCustoGateway : EntityDapperDataGatewayBase<RevisaoCusto>, IRevisaoCustoGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRevisaoCustoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperRevisaoCustoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RevisaoCusto", "IDRevisaoCusto")
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
                    "IDLoja",
                    "IDItemDetalhe",
                    "IDStatusRevisaoCusto",
                    "IDMotivoRevisaoCusto",
                    "IDUsuarioSolicitante",
                    "dtSolicitacao",
                    "vlCustoSolicitado",
                    "dsMotivo",
                    "IDUsuarioRevisor",
                    "dtCustoRevisado",
                    "vlCustoRevisado",
                    "dsRevisor",
                    "dtCriacao",
                    "dtRevisado"
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém uma revisao de custo pelo seu id e retorna a rebisao de custo com informações das entidades associadas.
        /// </summary>
        /// <param name="idRevisaoCusto">O id da revisao de custo.</param>
        /// <returns>A RevisaoCusto com informações de Loja, ItemDetalhe, StatusRevisaoCusto, MotivoRevisaoCusto, Departamento.</returns>
        public RevisaoCusto ObterEstruturadoPorId(int idRevisaoCusto)
        {
            RevisaoCusto result = null;

            this.Resource.Query<RevisaoCusto, Loja, ItemDetalhe, Departamento, MotivoRevisaoCusto, Usuario, StatusRevisaoCusto, RevisaoCusto>(
                Sql.RevisaoCusto.ObterEstruturadoPorId,
                new { idRevisaoCusto },
                (revisaoCusto, loja, itemDetalhe, departamento, motivoRevisaoCusto, usuarioSolicitante, statusRevisaoCusto) =>
                {
                    if (result == null)
                    {
                        result = revisaoCusto;
                        result.Loja = loja;
                        result.ItemDetalhe = itemDetalhe;
                        result.ItemDetalhe.Departamento = departamento;
                        result.MotivoRevisaoCusto = motivoRevisaoCusto;
                        result.UsuarioSolicitante = usuarioSolicitante;
                        result.StatusRevisaoCusto = statusRevisaoCusto;
                    }

                    return result;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").Perform();

            return result;
        }

        /// <summary>
        /// Pesquisa detalhe de revisoes de custos pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">A paginação</param>
        /// <returns>As revisões de custos.</returns>
        public IEnumerable<RevisaoCusto> PesquisarPorFiltros(RevisaoCustoFiltro filtro, Paging paging)
        {
            var args = new
            {
                idBandeira = filtro.IdBandeira,
                idLoja = filtro.IdLoja,
                idDepartamento = filtro.IdDepartamento,
                cdItem = filtro.CdItem,
                dsItem = filtro.DsItem,
                idStatus = filtro.IdStatus,
            };

            return this.Resource.Query<RevisaoCusto, Loja, ItemDetalhe, Departamento, MotivoRevisaoCusto, Usuario, StatusRevisaoCusto, RevisaoCusto>(
                Sql.RevisaoCusto.PesquisarPorFiltros,
                args,
                MapRevisaoCusto,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6")
            .AsPaging(paging);
        }

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public override void Insert(RevisaoCusto entity)
        {
            // Zera campos que fazem parte do caso de uso de revisão
            entity.IDUsuarioRevisor = null;
            entity.dtCustoRevisado = null;
            entity.vlCustoRevisado = null;
            entity.dsRevisor = null;
            entity.dtRevisado = null;

            base.Insert(entity);
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser atualizado não exista.</exception>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        public override void Update(RevisaoCusto entity)
        {
            // restaura campos que nao devem ser modificados
            RevisaoCusto original = FindById(entity.IDRevisaoCusto);

            entity.IDLoja = original.IDLoja;
            entity.IDItemDetalhe = original.IDItemDetalhe;
            entity.IDStatusRevisaoCusto = original.IDStatusRevisaoCusto;
            entity.IDMotivoRevisaoCusto = original.IDMotivoRevisaoCusto;
            entity.IDUsuarioSolicitante = original.IDUsuarioSolicitante;
            entity.dtCriacao = original.dtCriacao;
            entity.vlCustoSolicitado = original.vlCustoSolicitado;
            entity.dsMotivo = original.dsMotivo;

            base.Update(entity);
        }

        private RevisaoCusto MapRevisaoCusto(RevisaoCusto revisaoCusto, Loja loja, ItemDetalhe itemDetalhe, Departamento departamento, MotivoRevisaoCusto motivoRevisaoCusto, Usuario usuarioSolicitante, StatusRevisaoCusto statusRevisaoCusto)
        {
            revisaoCusto.Loja = loja;
            revisaoCusto.ItemDetalhe = itemDetalhe;
            revisaoCusto.ItemDetalhe.Departamento = departamento;
            revisaoCusto.MotivoRevisaoCusto = motivoRevisaoCusto;
            revisaoCusto.UsuarioSolicitante = usuarioSolicitante;
            revisaoCusto.StatusRevisaoCusto = statusRevisaoCusto;
            return revisaoCusto;
        }
        #endregion
    }
}
