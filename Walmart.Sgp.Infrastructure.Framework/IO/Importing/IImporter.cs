using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.IO.Importing
{
    /// <summary>
    /// Define a interface de um importador.
    /// </summary>
    /// <typeparam name="TInput">O tipo de entrada para o importador.</typeparam>
    public interface IImporter<TInput>
    {
        /// <summary>
        /// Realiza a importação utilizando os dados de entrada.
        /// </summary>
        /// <param name="input">Os dados de entrada..</param>
        void Import(TInput input);
    }
}
