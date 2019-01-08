using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Extension methods para auditoria.
    /// </summary>
    public static class AuditExtensions
    {
        /// <summary>
        /// Obtém um relatório de alterações para a entidade informada considerando os filtros informados.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="service">O serviço de auditoria.</param>
        /// <param name="propriedades">As propriedades relevantes para o log.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>O relatório.</returns>
        /// <remarks>A entidade deve possuir uma tabela de log.</remarks>
        public static IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(this IAuditService service, IEnumerable<string> propriedades, AuditFilter filter, Paging paging)
          where TEntity : IEntity
        {            
            return service.ObterRelatorio<TEntity>(propriedades, filter.IdUsuario, filter.IdEntidade, filter.Intervalo.StartValue, filter.Intervalo.EndValue, paging);
        }
    }
}
