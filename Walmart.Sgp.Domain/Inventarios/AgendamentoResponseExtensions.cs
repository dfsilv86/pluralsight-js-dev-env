using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Extensões para AgendamentoResponse.
    /// </summary>
    public static class AgendamentoResponseExtensions
    {
        /// <summary>
        /// Sumariza um conjunto de agendamentos e respectivas validações.
        /// </summary>
        /// <param name="agendamentos">Os agendamentos.</param>
        /// <returns>O AgendamentoResponse.</returns>
        public static AgendamentoResponse Summarize(this IEnumerable<Tuple<InventarioAgendamento, SpecResult>> agendamentos)
        {
            var result = new AgendamentoResponse();

            result.Total = agendamentos.Count();
            result.Validos = agendamentos.Where(agendamento => agendamento.Item2.Satisfied).Count();

            var invalidos = agendamentos.Where(agendamento => !agendamento.Item2.Satisfied).Select(agendamento => agendamento.Item2.Reason).ToArray();
            result.Invalidos = invalidos.Length;

            result.Mensagem = string.Join("\n", invalidos.Distinct());

            return result;
        }
    }
}
