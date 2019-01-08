using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Serviço de domínio relacionado a auditoria (log) de alterações.
    /// </summary>
    public class AuditService : DomainServiceBase<IAuditGateway>, IAuditService
    {
        #region Fields

        private static BindingFlags s_publicInstance = BindingFlags.Public | BindingFlags.Instance;

        private static string[] s_excluded = typeof(IEntity).GetProperties(s_publicInstance).Select(p => p.Name).Union(typeof(IStampContainer).GetProperties(s_publicInstance).Select(p => p.Name)).ToArray();

        private static Dictionary<string, string[]> s_properties = new Dictionary<string, string[]>();

        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AuditService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway de audit.</param>
        public AuditService(IAuditGateway mainGateway)
            : base(mainGateway)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loga a operação de INSERT, registrando o valor de todas as propriedades da entidade. Depende do id gerado no banco; deve ser chamado após o INSERT.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        public void LogInsert<TEntity>(TEntity entidade)
            where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("entidade", entidade);

            LogInsert(entidade, DeterminarPropriedadesLog<TEntity>());
        }

        /// <summary>
        /// Loga a operação de INSERT, registrando o valor das propriedades informadas. Depende do id gerado no banco; deve ser chamado após o INSERT.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        /// <param name="propriedades">As propriedades da entidade presentes no log.</param>
        public void LogInsert<TEntity>(TEntity entidade, params string[] propriedades)
              where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("entidade", entidade);
            ExceptionHelper.ThrowIfNull("propriedades", propriedades);

            AuditRecord<TEntity> audit = new AuditRecord<TEntity>(entidade, AuditKind.Insert);

            this.MainGateway.Insert(audit, propriedades);
        }

        /// <summary>
        /// Loga a operação de UPDATE, registrando o valor de todas as propriedades da entidade. Deve ser chamado após o UPDATE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        public void LogUpdate<TEntity>(TEntity entidade)
                 where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("entidade", entidade);

            LogUpdate(entidade, DeterminarPropriedadesLog<TEntity>());
        }

        /// <summary>
        /// Loga a operação de UPDATE, registrando o valor das propriedades informadas. Deve ser chamado após o UPDATE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        /// <param name="propriedades">As propriedades da entidade presentes no log.</param>
        public void LogUpdate<TEntity>(TEntity entidade, params string[] propriedades)
                  where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("entidade", entidade);
            ExceptionHelper.ThrowIfNull("propriedades", propriedades);

            AuditRecord<TEntity> audit = new AuditRecord<TEntity>(entidade, AuditKind.Update);

            this.MainGateway.Insert(audit, propriedades);
        }

        /// <summary>
        /// Loga a operação de DELETE, registrando o valor de todas as propriedades da entidade. Registra os últimos valores antes de apagar o registro, deve ser chamado antes do DELETE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        public void LogDelete<TEntity>(TEntity entidade)
                  where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("entidade", entidade);

            LogDelete(entidade, DeterminarPropriedadesLog<TEntity>());
        }

        /// <summary>
        /// Loga a operação de DELETE, registrando o valor das propriedades informadas. Registra os últimos valores antes de apagar o registro, deve ser chamado antes do DELETE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        /// <param name="propriedades">As propriedades da entidade presentes no log.</param>
        public void LogDelete<TEntity>(TEntity entidade, params string[] propriedades)
                where TEntity : IEntity
        {
            ExceptionHelper.ThrowIfNull("entidade", entidade);
            ExceptionHelper.ThrowIfNull("propriedades", propriedades);

            AuditRecord<TEntity> audit = new AuditRecord<TEntity>(entidade, AuditKind.Delete);

            this.MainGateway.Insert(audit, propriedades);
        }

        /// <summary>
        /// Obtém um relatório de alterações para a entidade informada considerando os filtros informados.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="idUsuario">O filtro de usuário.</param>
        /// <param name="idEntidade">O id da entidade.</param>
        /// <param name="intervaloInicio">Data de início do intervalo a pesquisar.</param>
        /// <param name="intervaloFim">Data de fim do intervalo a pesquisar.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>O relatório.</returns>
        /// <remarks>A entidade deve possuir uma tabela de log.</remarks>
        public IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(int? idUsuario, int? idEntidade, DateTime? intervaloInicio, DateTime? intervaloFim, Paging paging)
            where TEntity : IEntity
        {
            return this.MainGateway.ObterRelatorio<TEntity>(DeterminarPropriedadesLog<TEntity>(), idUsuario, idEntidade, intervaloInicio, intervaloFim, paging);
        }

        /// <summary>
        /// Obtém um relatório de alterações para a entidade informada considerando os filtros informados.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="propriedades">As propriedades relevantes para o log.</param>
        /// <param name="idUsuario">O filtro de usuário.</param>
        /// <param name="idEntidade">O id da entidade.</param>
        /// <param name="intervaloInicio">Data de início do intervalo a pesquisar.</param>
        /// <param name="intervaloFim">Data de fim do intervalo a pesquisar.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>O relatório.</returns>
        /// <remarks>A entidade deve possuir uma tabela de log.</remarks>
        public IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(IEnumerable<string> propriedades, int? idUsuario, int? idEntidade, DateTime? intervaloInicio, DateTime? intervaloFim, Paging paging)
            where TEntity : IEntity
        {
            return this.MainGateway.ObterRelatorio<TEntity>(propriedades, idUsuario, idEntidade, intervaloInicio, intervaloFim, paging);
        }

        private static string[] DeterminarPropriedadesLog<TEntity>()
        {
            // TODO: mover isso para AuditRecordHelper?
            Type entityType = typeof(TEntity);
            string entityFullName = entityType.FullName;

            lock (s_properties)
            {
                if (!s_properties.ContainsKey(entityFullName))
                {
                    s_properties[entityFullName] = entityType.GetProperties(s_publicInstance)
                        .Where(p => p.CanWrite && p.CanRead && !s_excluded.Contains(p.Name))
                        .Select(p => p.Name)
                        .ToArray();
                }

                return s_properties[entityFullName];
            }
        }

        #endregion
    }
}
