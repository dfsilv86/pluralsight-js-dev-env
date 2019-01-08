using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de um serviço de audit.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Loga a operação de INSERT, registrando o valor de todas as propriedades da entidade. Depende do id gerado no banco; deve ser chamado após o INSERT.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        void LogInsert<TEntity>(TEntity entidade)
            where TEntity : IEntity;

        /// <summary>
        /// Loga a operação de INSERT, registrando o valor das propriedades informadas. Depende do id gerado no banco; deve ser chamado após o INSERT.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        /// <param name="propriedades">As propriedades da entidade presentes no log.</param>
        void LogInsert<TEntity>(TEntity entidade, params string[] propriedades)
            where TEntity : IEntity;

        /// <summary>
        /// Loga a operação de UPDATE, registrando o valor de todas as propriedades da entidade. Deve ser chamado após o UPDATE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        void LogUpdate<TEntity>(TEntity entidade)
            where TEntity : IEntity;

        /// <summary>
        /// Loga a operação de UPDATE, registrando o valor das propriedades informadas. Deve ser chamado após o UPDATE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        /// <param name="propriedades">As propriedades da entidade presentes no log.</param>
        void LogUpdate<TEntity>(TEntity entidade, params string[] propriedades)
            where TEntity : IEntity;

        /// <summary>
        /// Loga a operação de DELETE, registrando o valor de todas as propriedades da entidade. Registra os últimos valores antes de apagar o registro, deve ser chamado antes do DELETE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        void LogDelete<TEntity>(TEntity entidade)
            where TEntity : IEntity;

        /// <summary>
        /// Loga a operação de DELETE, registrando o valor das propriedades informadas. Registra os últimos valores antes de apagar o registro, deve ser chamado antes do DELETE.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entidade">A entidade.</param>
        /// <param name="propriedades">As propriedades da entidade presentes no log.</param>
        void LogDelete<TEntity>(TEntity entidade, params string[] propriedades)
            where TEntity : IEntity;

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
        IEnumerable<AuditRecord<TEntity>> ObterRelatorio<TEntity>(IEnumerable<string> propriedades, int? idUsuario, int? idEntidade, DateTime? intervaloInicio, DateTime? intervaloFim, Paging paging)
            where TEntity : IEntity;
    }
}
