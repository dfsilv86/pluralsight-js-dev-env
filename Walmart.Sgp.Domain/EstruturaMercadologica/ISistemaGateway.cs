using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para sistema.
    /// </summary>
    public interface ISistemaGateway
    {
        /// <summary>
        /// Obter por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os sistemas.</returns>
        IEnumerable<Sistema> ObterPorUsuario(int idUsuario, string cultureCode);
    }
}
