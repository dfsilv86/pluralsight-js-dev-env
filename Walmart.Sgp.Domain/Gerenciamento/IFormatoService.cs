using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um serviço de formato.
    /// </summary>
    public interface IFormatoService : IDomainService<Formato>
    {
        /// <summary>
        /// Obtém a lista de formatos associados ao sistema informado.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <returns>Os formatos</returns>
        IEnumerable<Formato> ObterPorSistema(int? cdSistema);
    }
}
