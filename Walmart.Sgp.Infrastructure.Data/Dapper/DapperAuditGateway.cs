using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Data;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para audit utilizando o Dapper.
    /// </summary>
    /// <remarks>
    /// A tabela de log deve ter o seguinte formato:
    /// CREATE TABLE [Tabela]Log (   -- [nome da entidade]Log
    ///     IdAuditRecord INT IDENTITY(1,1) NOT NULL,  -- obrigatorio ser esse nome, id do registro de log
    ///     IdAuditUser INT NOT NULL,  -- obrigatorio ser esse nome, id do usuario que executou a operação
    ///     DhAuditStamp DATETIME NOT NULL,  -- obrigatorio ser esse nome, data/hora
    ///     CdAuditKind INT NOT NULL,  -- obrigatorio ser esse nome, tipo de operacao, 1=insert,2=update,3=delete
    ///     [pk tabela] [tipo] NOT NULL, -- coluna id da outra tabela
    ///     [coluna1] [tipo], -- coluna relevante
    ///     [coluna2] [tipo], -- coluna relevante
    ///     ...., -- outras colunas relevantes
    ///     PRIMARY KEY (IdAuditRecord)
    /// );
    /// </remarks>
    public class DapperAuditGateway : DapperDataGatewayBase<IEntity>, IAuditGateway
    {
        #region Fields

        private static Dictionary<string, string> s_sqlInsert = new Dictionary<string, string>();
        private static Dictionary<string, string> s_sqlSelect = new Dictionary<string, string>();
        private static Dictionary<string, Delegate> s_dynamicMapper = new Dictionary<string, Delegate>();

        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperAuditGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperAuditGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="record">A nova entidade a ser inserida.</param>
        /// <param name="propertyNames">As propriedades da entidade que devem ser auditadas/logadas.</param>
        /// <remarks>
        /// <para>Um novo registro será criado no banco de dados. 
        /// No caso de insert, o registro representa os dados que foram inseridos; no caso de update, representa os dados após a modificação; 
        /// no caso de delete, representa os dados da entidade no momento da deleção.</para>
        /// <para>A tabela de log deve ter o seguinte formato:
        /// CREATE TABLE [Tabela]Log (   -- [nome da entidade]Log
        ///     IdAuditRecord INT IDENTITY(1,1) NOT NULL,  -- obrigatorio ser esse nome, id do registro de log
        ///     IdAuditUser INT NOT NULL,  -- obrigatorio ser esse nome, id do usuario que executou a operação
        ///     DhAuditStamp DATETIME NOT NULL,  -- obrigatorio ser esse nome, data/hora
        ///     CdAuditKind INT NOT NULL,  -- obrigatorio ser esse nome, tipo de operacao, 1=insert,2=update,3=delete
        ///     [pk tabela] [tipo] NOT NULL, -- coluna id da outra tabela
        ///     [coluna1] [tipo], -- coluna relevante
        ///     [coluna2] [tipo], -- coluna relevante
        ///     ...., -- outras colunas relevantes
        ///     PRIMARY KEY (IdAuditRecord)
        /// );</para>
        /// </remarks>
        public void Insert<TEntity>(AuditRecord<TEntity> record, string[] propertyNames)
        {
            ExceptionHelper.ThrowIfNull("record", record);
            ExceptionHelper.ThrowIfNull("propertyNames", propertyNames);

            Type entityType = typeof(TEntity);
            string entityFullName = entityType.FullName;

            string sqlInsert = null;
            Func<AuditRecord<TEntity>, Dictionary<string, object>> mapper = null;

            lock (s_sqlInsert)
            {
                lock (s_dynamicMapper)
                {
                    if (!s_sqlInsert.ContainsKey(entityFullName))
                    {
                        PrepararParaInsert<TEntity>(propertyNames, entityFullName);
                    }

                    sqlInsert = s_sqlInsert[entityFullName];
                    mapper = (Func<AuditRecord<TEntity>, Dictionary<string, object>>)s_dynamicMapper[entityFullName];
                }
            }

            var idAuditRecord = this.Command.ExecuteScalar<int?>(sqlInsert, new DynamicParameters(mapper(record)));

            System.Diagnostics.Debug.Assert(idAuditRecord.HasValue && idAuditRecord > 0, "Log de auditoria deve criar exatamente um registro.");

            record.IdAuditRecord = idAuditRecord.Value;
        }

        /// <summary>
        /// Obtém um relatório de alterações para a entidade informada considerando os filtros informados.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="propertyNames">As propriedades relevantes para o log.</param>
        /// <param name="idUsuario">O filtro de usuário.</param>
        /// <param name="idEntidade">O id da entidade.</param>
        /// <param name="intervaloInicio">Data de início do intervalo a pesquisar.</param>
        /// <param name="intervaloFim">Data de fim do intervalo a pesquisar.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>O relatório.</returns>
        /// <remarks>A entidade deve possuir uma tabela de log.</remarks>
        public IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(IEnumerable<string> propertyNames, int? idUsuario, int? idEntidade, DateTime? intervaloInicio, DateTime? intervaloFim, Paging paging)
            where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("propertyNames", propertyNames);

            Type entityType = typeof(TEntity);
            string entityFullName = entityType.FullName;

            string sqlSelect = null;

            lock (s_sqlSelect)
            {
                if (!s_sqlSelect.ContainsKey(entityFullName))
                {
                    PrepararParaSelect(propertyNames, entityFullName);
                }

                sqlSelect = s_sqlSelect[entityFullName];
            }

            return this.Command
                .Query<AuditRecord<TEntity>, TEntity, AuditUser, AuditRecord<TEntity>>(
                    sqlSelect,
                    new { idUsuario, idEntidade, intervaloInicio, intervaloFim },
                    (ar, en, user) =>
                    {
                        ar.Entity = en;
                        ar.AuditUser = user;
                        return ar;
                    },
                    "SplitOn1,SplitOn2")
                .AsPaging(paging);
        }

        private static void PrepararParaSelect(IEnumerable<string> propertyNames, string entityFullName)
        {
            var tableModel = EntityTableModelRegistry.GetTableModelForEntity(entityFullName);

            var arProp = new string[] { "IdAuditRecord", "IdAuditUser", "DhAuditStamp", "CdAuditKind" };
            var entProp = propertyNames.Concat(new string[] { tableModel.Item2 }).Except(arProp).Distinct();

            string result = SqlResourceReader.Read(Sql.Audit.ObterRelatorio).With(tableModel.Item1, tableModel.Item2, string.Join(",L.", entProp));

            s_sqlSelect[entityFullName] = result;
        }

        private static void PrepararParaInsert<TEntity>(string[] propertyNames, string entityFullName)
        {
            var tableModel = EntityTableModelRegistry.GetTableModelForEntity(entityFullName);

            var allProp = new string[] { "IdAuditUser", "DhAuditStamp", "CdAuditKind" }.Concat(propertyNames).Concat(new string[] { tableModel.Item2 }).Distinct().ToArray();

            string result = @"INSERT INTO {0}Log ( {1} ) VALUES ( @{2} ); SELECT @@IDENTITY;".With(tableModel.Item1, string.Join(",", allProp), string.Join(",@", allProp));

            s_sqlInsert[entityFullName] = result;

            var theMapper = AuditRecordHelper.CreateMapper<TEntity>(propertyNames.Union(new string[] { tableModel.Item2 }).ToArray());

            s_dynamicMapper[entityFullName] = theMapper;
        }

        #endregion
    }
}
