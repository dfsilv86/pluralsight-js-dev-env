using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.IO.Importing
{
      /// <summary>
    /// Define a interface de um tradutor para a estrutura de dados do sistema.
    /// </summary>
    /// <typeparam name="TResult">O tipo de dados que o tradutor irá gerar.</typeparam>
    public interface IDataTranslator<TResult>
    {
        /// <summary>
        /// Traduz os dados para o formato esperado.
        /// </summary>
        /// <returns>O resultado.</returns>
        TResult Translate();
    }
}
