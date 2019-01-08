using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// O resultado da importação automática de inventário.
    /// </summary>
    public class ImportarInventarioResponse
    {
        /// <summary>
        /// Obtém um valor que indica se a operação foi executada com sucesso.
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade de arquivos importados.
        /// </summary>
        public int QtdArquivosTransferidos { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade de arquivos cujos dados realmente foram usados.
        /// </summary>
        public int QtdArquivosUsados { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade de críticas.
        /// </summary>
        public int QtdCriticas { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de retorno.
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Obtém ou define o resultado por arquivo.
        /// </summary>
        public object Arquivos { get; set; }
    }
}
