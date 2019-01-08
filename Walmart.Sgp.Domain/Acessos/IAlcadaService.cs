using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um serviço de alcada.
    /// </summary>
    public interface IAlcadaService : IDomainService<Alcada>
    {
        /// <summary>
        /// Verifica se não há tentativa de inserir registros filhos duplicados para Alçada.
        /// </summary>
        /// <param name="entidade">A entidade pai.</param>
        /// <returns>O SpecResult com o resultado a validação.</returns>
        SpecResult ValidarDuplicidadeDetalhe(Alcada entidade);

        /// <summary>
        /// Localiza uma alçada pelo seu id de perfil.
        /// </summary>
        /// <param name="idPerfil">O id de perfil.</param>
        /// <returns>A alcada.</returns>
        Alcada ObterPorPerfil(int idPerfil);

        /// <summary>
        /// Obtém a alçada juntamente com o papel baseado no id do papel.
        /// </summary>
        /// <param name="idPerfil">O identificador do perfil.</param>
        /// <returns>A alçada que percence ao perfil.</returns>
        Alcada ObterEstruturadoPorPerfil(int idPerfil);
    }
}
