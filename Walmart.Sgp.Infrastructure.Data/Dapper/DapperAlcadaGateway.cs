using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para alcada utilizando o Dapper.
    /// </summary>
    public class DapperAlcadaGateway : EntityDapperDataGatewayBase<Alcada>, IAlcadaGateway
    {
        private static String[] s_alcadaDetalheAuditProperties = new String[] { "IDAlcadaDetalhe", "vlPercentualAlterado" };
        private readonly IAlcadaDetalheGateway m_detalheGateway;
        private readonly IAuditService m_auditService;

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperAlcadaGateway"/> class.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperAlcadaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Alcada", "IDAlcada")
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperAlcadaGateway"/> class.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        /// <param name="detalheGateway">O gateway para AlcadaDetalhe.</param>
        /// <param name="auditService">O service para auditoria.</param>
        public DapperAlcadaGateway(ApplicationDatabases databases, IAlcadaDetalheGateway detalheGateway, IAuditService auditService)
            : base(databases.Wlmslp, "Alcada", "IDAlcada")
        {
            this.m_detalheGateway = detalheGateway;
            this.m_auditService = auditService;
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
                return new string[] { "IDPerfil", "blAlterarSugestao", "blAlterarInformacaoEstoque", "blAlterarPercentual", "vlPercentualAlterado", "blZerarItem" };
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public void Insert(Alcada entity, IAuditStrategy auditStrategy)
        {
            base.Insert(entity);

            auditStrategy.DidInsert(entity);

            ChildrenHelper.Insert(
                entity.Detalhe,
                m_detalheGateway,
                (s) => s.IDAlcada = entity.Id,
                auditStrategy);
        }

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser excluído não exista.</exception>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>   
        public void Delete(int id, IAuditStrategy auditStrategy)
        {
            var oldEntity = ObterEstruturado(id, null);

            ChildrenHelper.Delete(oldEntity.Detalhe, m_detalheGateway, auditStrategy);

            auditStrategy.WillDelete(oldEntity);

            base.Delete(id);
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser atualizado não exista.</exception>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        public void Update(Alcada entity, IAuditStrategy auditStrategy)
        {
            var oldEntity = ObterEstruturado(entity.Id, null);
            base.Update(entity);

            auditStrategy.DidUpdate(oldEntity);

            ChildrenHelper.Sync(
                oldEntity.Detalhe,
                entity.Detalhe,
                m_detalheGateway,
                (s) => s.IDAlcada = entity.Id,
                auditStrategy);
        }

        /// <summary>
        /// Obtém a alçada juntamente com o papel baseado no id do papel.
        /// </summary>
        /// <param name="idPerfil">O identificador do perfil.</param>
        /// <returns>
        /// A alçada que percence ao perfil.
        /// </returns>
        public Alcada ObterEstruturadoPorPerfil(int idPerfil)
        {
            return Resource.Query<Alcada, Papel, Alcada>(
                Sql.Alcada.ObterEstruturadoPorPerfil,
                new { idPerfil = idPerfil },
                MapearAlcada,
                "SplitOn1")
                .FirstOrDefault();
        }

        /// <summary>
        /// Obtém estruturado com entidades filhas.
        /// </summary>
        /// <param name="idAlcada">O id da alcada.</param>
        /// <param name="idPerfil">O id do perfil.</param>
        /// <returns>A entidade populada com as entidades filhas.</returns>
        public Alcada ObterEstruturado(int? idAlcada, int? idPerfil)
        {
            Alcada result = null;

            this.Resource.Query<Alcada, Papel, AlcadaDetalhe, RegiaoAdministrativa, Bandeira, Departamento, Alcada>(
                Sql.Alcada.ObterEstruturado,
                new { idAlcada = idAlcada, idPerfil = idPerfil },
                (alcada, papel, alcadaDetalhe, regiaoAdministrativa, bandeira, departamento) =>
                {
                    if (result == null)
                    {
                        result = alcada;
                        result.Papel = papel;
                    }

                    if (!alcadaDetalhe.IsNew && !result.Detalhe.Contains(alcadaDetalhe))
                    {
                        alcadaDetalhe.RegiaoAdministrativa = regiaoAdministrativa;
                        alcadaDetalhe.Bandeira = bandeira;
                        alcadaDetalhe.Departamento = departamento;

                        result.Detalhe.Add(alcadaDetalhe);
                    }

                    return result;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5").ToList();

            return result;
        }

        private static Alcada MapearAlcada(Alcada alcada, Papel papel)
        {
            alcada.Papel = papel;
            papel.Id = alcada.IDPerfil;
            return alcada;
        }
        #endregion
    }
}
