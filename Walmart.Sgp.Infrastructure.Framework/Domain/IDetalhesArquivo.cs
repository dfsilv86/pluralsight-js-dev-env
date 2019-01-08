using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de detalhes de um arquivo.
    /// </summary>
    public interface IDetalhesArquivo
    {
        /// <summary>
        /// Obtém o nome do arquivo (sem o caminho).
        /// </summary>
        string NomeArquivo { get; }

        /// <summary>
        /// Obtém a data do arquivo.
        /// </summary>
        DateTime? DataArquivo { get; }
    }
}
