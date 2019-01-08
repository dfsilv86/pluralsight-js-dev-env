using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um table data gateway para alcada.
    /// </summary>
    public interface IAlcadaGateway : IDataGateway<Alcada>
    {
        /// <summary>
        /// Obtém estruturado com entidades filhas.
        /// </summary>
        /// <param name="idAlcada">O id da alcada.</param>
        /// <param name="idPerfil">O id do perfil.</param>
        /// <returns>A entidade populada com as entidades filhas.</returns>
        Alcada ObterEstruturado(int? idAlcada, int? idPerfil);

        /// <summary>
        /// Obtém a alçada juntamente com o papel baseado no id do papel.
        /// </summary>
        /// <param name="idPerfil">O identificador do perfil.</param>
        /// <returns>A alçada que percence ao perfil.</returns>
        Alcada ObterEstruturadoPorPerfil(int idPerfil);

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        void Insert(Alcada entity, IAuditStrategy auditStrategy);

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        void Update(Alcada entity, IAuditStrategy auditStrategy);

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <param name="auditStrategy">A estratégia de auditoria.</param>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>
        void Delete(int id, IAuditStrategy auditStrategy);
    }
}
