using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Retorno da operação de inserção ou atualização de agendamentos.
    /// </summary>
    public class AgendamentoResponse
    {
        /// <summary>
        /// Total de agendamentos identificados.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Número de agendamentos válidos para inserção/atualização.
        /// </summary>
        public int Validos { get; set; }

        /// <summary>
        /// Número de agendamentos inválidos.
        /// </summary>
        public int Invalidos { get; set; }

        /// <summary>
        /// Mensagem de validação.
        /// </summary>
        public string Mensagem { get; set; }
    }
}
