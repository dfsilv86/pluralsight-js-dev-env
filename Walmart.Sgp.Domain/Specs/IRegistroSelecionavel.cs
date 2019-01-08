using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Specs
{
    /// <summary>
    /// A interface que especifica uma entidade selecionável.
    /// </summary>
    public interface IRegistroSelecionavel : IEntity
    {
        /// <summary>
        /// Se a entidade está selecionada.
        /// </summary>
        bool Selecionado { get; }
    }
}
