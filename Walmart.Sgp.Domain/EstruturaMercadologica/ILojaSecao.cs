using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de uma seção de loja.
    /// </summary>
    public interface ILojaSecao : IEntity
    {       
        /// <summary>
        /// Obtém o código.
        /// </summary>
        int Codigo { get; }

        /// <summary>
        /// Obtém a descrição.
        /// </summary>
        string Descricao { get; }
    }
}
