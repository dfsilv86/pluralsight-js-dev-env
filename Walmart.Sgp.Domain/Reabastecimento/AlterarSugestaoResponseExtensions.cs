using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Extension methods para alterar sugestao response.
    /// </summary>
    public static class AlterarSugestaoResponseExtensions
    {
        /// <summary>
        /// Sumariza os resultados.
        /// </summary>
        /// <param name="resultados">Os resultados.</param>
        /// <returns>A visão geral da operação.</returns>
        public static AlterarSugestoesResponse Summarize(this IEnumerable<AlterarSugestaoResponse> resultados)
        {
            var result = new AlterarSugestoesResponse
            {
                Total = resultados.Count(),
                Inexistentes = resultados.Count(r => r.Inexistente),
                NaoSalvaPercentualAlteracao = resultados.Count(r => r.NaoSalvaPercentualAlteracao),
                NaoSalvaGradeSugestao = resultados.Count(r => r.NaoSalvaGradeSugestao),
                Sucesso = resultados.Count(r => r.Sucesso),
                IDSugestaoPedidoAlterados = resultados.Where(r => r.Sucesso).Select(r => r.IDSugestaoPedido).ToArray()
            };

            BuildMessage(result);

            return result;
        }

        private static void BuildMessage(AlterarSugestoesResponse result)
        {
            List<string> lines = new List<string>();

            lines.Add(result.Total.PluralizedMessage("ChangeOrderSuggestionTotal").With(result.Total));
            lines.Add(result.Sucesso.PluralizedMessage("ChangeOrderSuggestionSuccess").With(result.Sucesso));
            lines.Add(result.NaoSalvaPercentualAlteracao.PluralizedMessage("ChangeOrderSuggestionChangePercentage").With(result.NaoSalvaPercentualAlteracao));
            lines.Add(result.NaoSalvaGradeSugestao.PluralizedMessage("ChangeOrderSuggestionSuggestionGrid").With(result.NaoSalvaGradeSugestao));

            string message = string.Join("\n\n", lines.ToArray());

            result.Mensagem = message;
        }
    }
}
