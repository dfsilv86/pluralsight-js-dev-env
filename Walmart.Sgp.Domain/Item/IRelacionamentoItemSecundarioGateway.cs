using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface de um table data gateway para relacionamento item secundario.
    /// </summary>
    public interface IRelacionamentoItemSecundarioGateway
    {
        /// <summary>
        /// Obtém os relacionamentos onde o item participa como secundário.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>Os relacionamentos principais, onde o item participa como secundário, sendo que a lista de secundários retornada contém apenas o relacionamento onde o item está presente.</returns>
        IEnumerable<RelacionamentoItemPrincipal> ObterSecundariosPorItem(int idItemDetalhe);
    }
}
