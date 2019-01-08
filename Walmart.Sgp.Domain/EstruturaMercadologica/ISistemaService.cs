using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um sistema service.
    /// </summary>
    public interface ISistemaService
    {
        /// <summary>
        /// Obter the por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuario.</param>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os sistemas.</returns>
        IEnumerable<Sistema> ObterPorUsuario(int idUsuario, string cultureCode);
    }
}
