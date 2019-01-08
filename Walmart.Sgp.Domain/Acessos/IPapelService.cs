using System;
using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um serviço de papel.
    /// </summary>
    public interface IPapelService : IDomainService<Papel>
    {
        /// <summary>
        /// Obtém o papel pelo nome.
        /// </summary>
        /// <param name="nome">O nome.</param>
        /// <returns>O papel, se existir.</returns>
        Papel ObterPorNome(string nome);
    }
}
