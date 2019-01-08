using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um table data gateway para distrito.
    /// </summary>
    public interface IDistritoGateway : IDataGateway<Distrito>
    {
        /// <summary>
        /// Obtem um Distrito por Id.
        /// </summary>
        /// <param name="id">O ID do Distrito.</param>
        /// <returns>Retorna um Distrito.</returns>
        Distrito ObterEstruturado(int id);

        /// <summary>
        /// Pesquisar Distritos
        /// </summary>
        /// <param name="cdSistema">Código do sistema.</param>
        /// <param name="idBandeira">ID da Bandeira.</param>
        /// <param name="idRegiao">ID da Região.</param>
        /// <param name="idDistrito">ID do Distrito.</param>
        /// <param name="paging">Parametro de paginação.</param>
        /// <returns>Retorna uma lista de Distritos como resultado da busca.</returns>
        IEnumerable<Distrito> Pesquisar(int? cdSistema, int? idBandeira, int? idRegiao, int? idDistrito, Paging paging);
    }
}
