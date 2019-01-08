using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de um table data gateway para audit.
    /// </summary>
    public interface IAuditGateway
    {
        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="record">A nova entidade a ser inserida.</param>
        /// <param name="propertyNames">As propriedades da entidade que devem ser auditadas/logadas.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        void Insert<TEntity>(AuditRecord<TEntity> record, string[] propertyNames);

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
        IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(IEnumerable<string> propertyNames, int? idUsuario, int? idEntidade, DateTime? intervaloInicio, DateTime? intervaloFim, Paging paging)
            where TEntity : IEntity;
    }
}
